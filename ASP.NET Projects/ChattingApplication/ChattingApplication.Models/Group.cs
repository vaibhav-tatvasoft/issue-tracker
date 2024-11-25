using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class Group
    {
        [Key]
        public string id { get; set; }
        public string? groupName { get; set; }
        public string? groupDescription { get; set; } // Optional
        public List<User>? members { get; set; } = new List<User>();
        public List<Message>? messages { get; set; } = new List<Message>();
        public string? createdBy { get; set; }
        [ForeignKey("createdBy")]
        public User createdByUser { get; set; }
        public DateTime? createdAt { get; set; }
        public string? lastMessage { get; set; } // For preview
        public DateTime? lastMessageTimestamp { get; set; } // Nullable for preview
        public List<User>? isTyping { get; set; } = new List<User>(); // Users currently typing
        public string? groupAvatar { get; set; }
    }
}
