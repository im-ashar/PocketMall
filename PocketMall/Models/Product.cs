using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocketMall.Models
{
    public class Product
    {
        public Product()
        {
            AllCategories = new List<string>() { "Men", "Women", "Kids" };
        }
        public Guid ProductId { get; set; }
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public string Price { get; set; }

        [RegularExpression("([0-9]+)", ErrorMessage = "Quantity Should Be A Positive Number")]
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string? ImageUrl { get; set; }

        [NotMapped]
        public List<string> AllCategories { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        [NotMapped]
        public IFormFile? UpdateImage { get; set; }

        public ICollection<OrderProduct>? Orders { get; set; }
    }
}
