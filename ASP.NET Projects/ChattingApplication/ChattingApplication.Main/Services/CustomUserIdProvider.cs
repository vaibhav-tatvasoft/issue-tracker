using Microsoft.AspNetCore.SignalR;

namespace ChattingApplication.Main.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var userId = connection.GetHttpContext()?.Request.Query["userId"];
            return userId;
        }
    }
}
