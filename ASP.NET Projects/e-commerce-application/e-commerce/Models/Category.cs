using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; }
        public String DisplayOrder { get; set; }
    }
}
