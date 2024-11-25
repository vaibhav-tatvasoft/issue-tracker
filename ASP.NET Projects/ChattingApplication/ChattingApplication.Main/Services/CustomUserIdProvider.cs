using ChattingApplication.Models;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace ChattingApplication.Main.Services
{
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string? GetUserId(HubConnectionContext connection)
        {
            var user = connection.GetHttpContext()?.Request.Query["user"];
            if (!string.IsNullOrEmpty(user))
            {
                return user;
            }
            return null;
        }

    }
}
