﻿using System.ComponentModel.DataAnnotations;

namespace ChattingApplication.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
    }
}