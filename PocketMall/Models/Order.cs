using System.ComponentModel.DataAnnotations.Schema;

namespace PocketMall.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }

        public ICollection<OrderProduct> Products { get; set; }
    }
}
