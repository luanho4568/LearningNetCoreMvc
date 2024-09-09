using System.ComponentModel.DataAnnotations;

namespace WebStoreBook.Models
{
    public class ShoppingCart
    {
        public Product Product { get; set; }

        [Range(1, 1000)]
        public int Count { get; set; }
    }
}
