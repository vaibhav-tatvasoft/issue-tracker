using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;

namespace ChattingApplication.Main.Services
{
    public class RealTimeHub : Hub
    {
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
