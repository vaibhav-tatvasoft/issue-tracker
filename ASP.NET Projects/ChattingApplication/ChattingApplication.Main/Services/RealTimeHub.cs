using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Concurrent;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ChattingApplication.Main.Services
{
    public class RealTimeHub : Hub
    {
        private static ConcurrentDictionary<string, User> ConnectedClients = new ConcurrentDictionary<string, User>();
        private static ConcurrentBag<string> GroupsList = new ConcurrentBag<string>();
        private readonly GroupService _groupService;
        private User user;

        public RealTimeHub(GroupService groupService)
        {
            _groupService = groupService;
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

            Console.WriteLine(nameof(StartPrivateChat) + " Group Name " + groupName);

            Groups.AddToGroupAsync(orderedGroups[0], groupName);
            Groups.AddToGroupAsync(orderedGroups[1], groupName);

            GroupsList.Add(groupName);

            foreach(var group in GroupsList) 
            {
                Console.WriteLine(nameof(StartPrivateChat) + " Groups List " + group);
            }

            Clients.Caller.SendAsync("ReceiveGroupName", groupName);
        }

        public async Task StartGroupChat(string fromConnectionId, Object groupOfUsers)
        {
            List<string> groups = JsonSerializer.Deserialize<List<string>>(groupOfUsers.ToString());
            
            groups.Where(user => user != fromConnectionId).ToList().Add(fromConnectionId);
             var orderedGroups = groups.OrderBy(u => u).ToArray();
            var groupName = "Group";
            foreach(var user in orderedGroups)
            {
                groupName += "_" + user;
            }
            Console.WriteLine("groupName is : " + groupName);
            foreach (var user in orderedGroups)
            {
                Groups.AddToGroupAsync(user, groupName);
            }

            if(await _groupService.GetGroupByIdAsync(groupName) == null)
            {
                await _groupService.CreateGroupAsync(groupName);
            }
        }
    }
}
