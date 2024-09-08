using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebStoreBook.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public int ISBN { get; set; }

        [Required]
        public string Author { get; set; }
        [Required]
        [Range(1, 1000000000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 1000000000)]
        public double Price50 { get; set; }

        [Required]
        [Range(1, 1000000000)]
        public double Price100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ValidateNever]
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        [Required]
        [Display(Name = "Cover Type")]
        public int CoverTypeId { get; set; }
        [ValidateNever]
        [ForeignKey("CoverTypeId")]
        public CoverType CoverType { get; set; }

    }
}
