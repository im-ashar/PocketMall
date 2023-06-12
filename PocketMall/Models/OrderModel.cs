using System.ComponentModel.DataAnnotations.Schema;

namespace PocketMall.Models
{
    public class OrderModel
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }

        public ICollection<ProductModel> Products { get; set; } = new List<ProductModel>();
    }
}
