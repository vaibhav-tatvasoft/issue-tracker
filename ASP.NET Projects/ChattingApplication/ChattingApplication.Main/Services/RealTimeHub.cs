using ChattingApplication.DataAccess;
using ChattingApplication.DataAccess.Repository;
using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
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
        private readonly ApplicationDBContext _db;
        private User user;

        public RealTimeHub(GroupService groupService, ApplicationDBContext db)
        {
            _groupService = groupService;
            _db = db;
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
            var groupName = $"Private_{orderedGroups[0]}_{orderedGroups[1]}";

            Console.WriteLine(nameof(StartPrivateChat) + " Private Chat Name " + groupName);

            Groups.AddToGroupAsync(orderedGroups[0], groupName);
            Groups.AddToGroupAsync(orderedGroups[1], groupName);

            if (!GroupsList.Contains(groupName))
            {
                GroupsList.Add(groupName);
            }

            foreach(var group in GroupsList) 
            {
                Console.WriteLine(nameof(StartPrivateChat) + " Groups List " + group);
            }

            Clients.Caller.SendAsync("ReceiveGroupName", groupName);
        }

        public async Task StartGroupChat(Object fromUserObject, Object groupOfUsers, string groupName)
        {
            if (!string.IsNullOrEmpty(fromUserObject.ToString()) && !string.IsNullOrEmpty(groupOfUsers.ToString()))
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve, // This handles cycles
                    MaxDepth = 64, // Optional: Can limit depth to avoid too deep object graphs
                };

                var userObj = JsonSerializer.Deserialize<User>(fromUserObject.ToString(), options);

                var ConnectedClientsIdsOfUsers = ConnectedClients.Select(x => x.Value.id).ToList();

                var orderedGroups = ConnectedClientsIdsOfUsers.OrderBy(u => u).ToArray();
                var groupId = "Group_" + string.Join("_", orderedGroups);

                Console.WriteLine("groupName is : " + groupId);

                foreach (var user in orderedGroups)
                {
                    await Groups.AddToGroupAsync(user, groupId);
                }

                List<User> groupMembers = _db.Users.Where(u => ConnectedClientsIdsOfUsers.Contains(u.id)).ToList();

                var response = await _groupService.GetGroupByIdAsync(groupId);

                if (response == null)
                {
                    response = await _groupService.CreateGroupAsync(userObj, groupId, groupMembers, groupName);
                }

                var serializedJsonCallerUser = JsonSerializer.Serialize(userObj, options);
                var serializedJsonGroup = JsonSerializer.Serialize(response, options);

                SendCreatedGroupObject(serializedJsonGroup, serializedJsonCallerUser);
            }
        }

        public async Task SendCreatedGroupObject(string group, string callerUser)
        {


            Clients.Caller.SendAsync("ReceiveCreatedGroupObject", group, callerUser);
        }

    }
}
