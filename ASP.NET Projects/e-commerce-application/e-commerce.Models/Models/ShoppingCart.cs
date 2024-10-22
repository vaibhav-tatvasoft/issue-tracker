using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace e_commerce.Models
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Range(1,1000, ErrorMessage = "Please select a value which is within range(1,1000)")]
        public int Count { get; set; }
        [NotMapped]
        public double Price { get; set; }

    }
}
