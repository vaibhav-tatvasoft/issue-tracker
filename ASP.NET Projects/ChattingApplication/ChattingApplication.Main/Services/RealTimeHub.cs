﻿using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Collections.Concurrent;
using System.Text.Json;

namespace ChattingApplication.Main.Services
{
    public class RealTimeHub : Hub
    {
        private static ConcurrentDictionary<string, string> ConnectedClients = new ConcurrentDictionary<string, string>();

        public override Task OnConnectedAsync()
        {
            // Store the connection ID of the client
            ConnectedClients.TryAdd(Context.ConnectionId, Context.UserIdentifier);
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
            var groupName = $"Group_{orderedGroups[0]}_{orderedGroups[1]}";

            Console.WriteLine(nameof(StartPrivateChat) + " Group Name " + groupName);

            Groups.AddToGroupAsync(orderedGroups[0], groupName);
            Groups.AddToGroupAsync(orderedGroups[1], groupName);

            Clients.Caller.SendAsync("ReceiveGroupName", groupName);
        }
    }
}
