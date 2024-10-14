using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace e_commerce.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(100)]
        public String Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage ="value would be between 1 and 100")]
        public String DisplayOrder { get; set; }
    }
}
