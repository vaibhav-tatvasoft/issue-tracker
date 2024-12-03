using ChattingApplication.DataAccess;
using ChattingApplication.DataAccess.Repository;
using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChattingApplication.Main.Services
{
    public class RealTimeHub : Hub
    {
        private static ConcurrentDictionary<string, User> ConnectedClients = new ConcurrentDictionary<string, User>();
        private static ConcurrentBag<string> GroupsList = new ConcurrentBag<string>();
        private readonly GroupService _groupService;
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDBContext _db;
        private User user;

        public RealTimeHub(GroupService groupService, ApplicationDBContext db, IUserRepository userRepository)
        {
            _groupService = groupService;
            _db = db;
            _userRepository = userRepository;
        }

        public override Task OnConnectedAsync()
        {
            var userString = Context.GetHttpContext()?.Request.Query["user"];

            if (!string.IsNullOrEmpty(userString))
            {
                user = JsonSerializer.Deserialize<User>(userString);
            }
                // Store the connection ID of the client
            ConnectedClients.TryAdd(Context.ConnectionId, user);

             Console.WriteLine($"Client connected: {Context.ConnectionId}");
            Console.WriteLine(ConnectedClients.Count);
            Console.WriteLine(ConnectedClients.ToString);

            SendAllClientsList(ConnectedClients.Keys.ToList());

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the client from the dictionary
            ConnectedClients.Remove(Context.ConnectionId, out _);
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");

            SendAllClientsList(ConnectedClients.Keys.ToList());

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAllClientsList(List<string> users)
        {
            await Clients.Clients(users).SendAsync("ReceiveAllClientsList", ConnectedClients);
        }

        public async Task SendMessage(Object message) //user who SENT the message and message object
        {
            var messageObject = JsonSerializer.Deserialize<Message>(message.ToString());

            if (messageObject.type.ToLower() == "incoming")
            {
                messageObject.type = "outgoing";
            }
            else
            {
                messageObject.type = "incoming";
            }

            //messageObject.timestamp = DateTime.Now.ToString();
            
            //await Clients.User(user).SendAsync("ReceiveMessage", Context.UserIdentifier, messageObject);
            await Clients.OthersInGroup(messageObject.groupName).SendAsync("ReceiveMessage", Context.UserIdentifier, messageObject);
        }

        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }

        public async Task StartPrivateChat(string fromConnectionId, string toConnectionId)
        {
            var orderedGroups = new[] { fromConnectionId, toConnectionId }.OrderBy(u => u).ToArray();
            var privateChatName = $"Private_{orderedGroups[0]}_{orderedGroups[1]}";

            Console.WriteLine(nameof(StartPrivateChat) + " Private Chat Name " + privateChatName);

            Groups.AddToGroupAsync(orderedGroups[0], privateChatName);
            Groups.AddToGroupAsync(orderedGroups[1], privateChatName);

            if (!GroupsList.Contains(privateChatName))
            {
                GroupsList.Add(privateChatName);
            }

            foreach(var group in GroupsList) 
            {
                Console.WriteLine(nameof(StartPrivateChat) + " Groups List " + group);
            }

            Clients.Caller.SendAsync("ReceiveGroupName", privateChatName);
        }

        public async Task StartGroupChat(Object fromUserObject, Object groupOfUsers, string groupName)
        {
            if (!string.IsNullOrEmpty(fromUserObject.ToString()) && !string.IsNullOrEmpty(groupOfUsers.ToString()))
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, // This handles cycles
                    MaxDepth = 3, // Optional: Can limit depth to avoid too deep object graphs
                };

                var userObj = JsonSerializer.Deserialize<User>(fromUserObject.ToString(), options);
                var groupOfUsersObj = JsonSerializer.Deserialize<List<string>>(groupOfUsers.ToString(), options);

                var ConnectedClientsIdsOfUsers = ConnectedClients.Select(x => x.Value.id).ToList();

                var ConnectedClientsConnectionIdOfGroupUsers = ConnectedClients.Where(u => groupOfUsersObj.Contains(u.Key)).Select(u => u.Key).ToArray();

                var orderedGroupsOfIds = ConnectedClientsIdsOfUsers.OrderBy(u => u).ToArray();

                var groupId = "Group";

                foreach (var id in orderedGroupsOfIds)
                {
                    groupId += "_" + id;
                }

                Console.WriteLine("groupName is : " + groupId);

                foreach (var connectionId in ConnectedClientsConnectionIdOfGroupUsers)
                {
                    await Groups.AddToGroupAsync(connectionId, groupId);
                }

                var response = await _groupService.GetGroupAsync(u => u.groupId == groupId);

                if (response == null || response.groupName != groupName)
                {
                    response = await _groupService.CreateGroupAsync(userObj, groupId, ConnectedClientsIdsOfUsers, groupName);
                }

                //fetch all the members from group for bulk send to react
                //from group, see which all ids are mapped ----- or take the groupMembers string and retrieve those users
                //those will be updated users having this new group in them, sounnds good??
                var uid = new List<string>();

                List<User> members = _db.Users.Where(u => ConnectedClientsIdsOfUsers.Contains(u.id)).ToList();

                 //user = await _userRepository.GetUser(u => u.id == userObj.id);

                var serializedJsonGroup = JsonSerializer.Serialize(response, options);
                //var serializedJsonCallerUser = JsonSerializer.Serialize(user, options);

                SendCreatedGroupObject(serializedJsonGroup, members, groupId);
            }
        }

        public async Task SendCreatedGroupObject(string group, List<User> members, string groupId)
        {

            Clients.Group(groupId).SendAsync("ReceiveCreatedGroupObject", group, members);
        }

        //private string createTemporaryGroupToSendGroupDetailsToAllMembers(List<User> members, string groupId)
        //{
        //    Clients.Group(groupId).SendAsync(members)
        //}


    }
}
