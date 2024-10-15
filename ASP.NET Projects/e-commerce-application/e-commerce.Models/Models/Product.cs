﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    public class Product
    {
        [Key]
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 1000)]
        [DisplayName("List Price")]
        public double ListPrice { get; set; }
        [Required]
        [DisplayName("Price for 1-50")]
        public double Price { get; set; }
        [Required]
        [DisplayName("Price for 50+")]
        public double Price50 { get; set; }
        [Required]
        [DisplayName("Price for 100+")]
        public double Price100 { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category category { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; }

    }
}
