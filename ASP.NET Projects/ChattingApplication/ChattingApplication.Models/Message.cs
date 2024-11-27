using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class Message
    {
        public string id {  get; set; }
        public string type { get; set; }
        public string content { get; set; }
        public string? timestamp { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        [ForeignKey("from")]
        public User fromUser { get; set; }
        [ForeignKey("to")]
        public User toUser { get; set; }
        public string groupName { get; set; }
        public string groupId { get; set; }
        public bool? isRead { get; set; }
        [ForeignKey("groupId")]
        public Group group { get; set; }   
    }
}
