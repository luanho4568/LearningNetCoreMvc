using System.ComponentModel.DataAnnotations;

namespace WebStoreBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Display Order")]
        [Range(1, 100, ErrorMessage = "Value to {0} must be between {1} and {2}")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; } = DateTime.Now;
    }
}
