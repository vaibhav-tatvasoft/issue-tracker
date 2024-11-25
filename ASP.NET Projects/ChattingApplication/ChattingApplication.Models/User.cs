using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Models
{
    public class User
    {
        [Key]
        public string id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
    }
}