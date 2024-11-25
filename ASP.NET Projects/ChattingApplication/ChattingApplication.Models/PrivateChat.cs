using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class PrivateChat
    {
        public string id { get; set; }
        public string? user1Id { get; set; }
        public string? user2Id { get; set;}
        public List<string>? messages { get; set; }
        public string? lastMessage { get; set; }
        public string? lastMessageTimestamp { get; set; }
        public bool? isUser1Typing { get; set; }
        public bool? isUser2Typing { get;set; }
    }
}
