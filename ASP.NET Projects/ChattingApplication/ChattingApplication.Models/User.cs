using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Models
{
    public class User
    {
        public User()
        {
            sentMessages = new HashSet<Message>();
            receivedMessages = new HashSet<Message>();
        }
        [Key]
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public ICollection<Message> sentMessages { get; set; }
        public ICollection<Message> receivedMessages { get; set; }
        //public List<Group> groups { get; set; }

        public List<Group> groups { get; set; }
    }
}