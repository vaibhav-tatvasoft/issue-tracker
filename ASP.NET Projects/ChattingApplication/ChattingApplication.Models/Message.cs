using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChattingApplication.Models
{
    public class Message
    {
        public string type { get; set; }
        public string content { get; set; }
        public string? timestamp { get; set; }
        public string to { get; set; }
        public string from { get; set; }
        public string groupName { get; set; }
        public bool? isRead { get; set; }    
    }
}
