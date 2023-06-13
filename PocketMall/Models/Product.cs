using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace PocketMall.Models
{
    public class Product
    {
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        [NotMapped]
        public List<string> AllCategories { get; set; }

        [NotMapped]
        public IFormFile Image { get; set; }

        public ICollection<OrderProduct> Orders { get; set; }
    }
}
