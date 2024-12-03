using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class Group
    {
        [Key]
        public string id { get; set; }
        public string? groupName { get; set; }
        public string? groupId { get; set; } // Optional
        //[JsonIgnore]
        //[NotMapped]
        //public List<string>? membersIds
        //{
        //    get => string.IsNullOrEmpty(membersIdsJson) ? new List<string> () : JsonSerializer.Deserialize<List<string>> (membersIdsJson);
        //    set => membersIdsJson = JsonSerializer.Serialize(value);
        //}
        //[Column(TypeName = "nvarchar(max)")]
        //public string? membersIdsJson { get; set; }
        [JsonIgnore]
        public List<Message>? messages { get; set; } = new List<Message>();
        public string? createdBy { get; set; }
        [ForeignKey("createdBy")]
        [JsonIgnore]
        public User createdByUser { get; set; }
        public DateTime? createdAt { get; set; }
        public string? lastMessage { get; set; } // For preview
        public DateTime? lastMessageTimestamp { get; set; } // Nullable for preview
        //public List<User>? isTyping { get; set; } = new List<User>(); // Users currently typing
        public string? groupAvatar { get; set; }


        public List<User> members { get; set; }
    }
}
