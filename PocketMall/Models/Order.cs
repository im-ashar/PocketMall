using System.ComponentModel.DataAnnotations.Schema;

namespace PocketMall.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string TotalPrice { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}
