using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

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
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // Remove the client from the dictionary
            ConnectedClients.Remove(Context.ConnectionId, out _);
            Console.WriteLine($"Client disconnected: {Context.ConnectionId}");

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendAllClientsList()
        {
            await Clients.User(Context.UserIdentifier).SendAsync("ReceiveAllClientsList", ConnectedClients);
        }

        public async Task SendMessage(string user, string message)
        {
            //await Clients.All.SendAsync("ReceiveMessage", user, message);
            //await Clients.Caller.SendAsync("ReceiveMessage", user, message);

            Console.WriteLine(Clients.All.ToString);

            await Clients.User(user).SendAsync("ReceiveMessage", Context.UserIdentifier, message);
        }

        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
