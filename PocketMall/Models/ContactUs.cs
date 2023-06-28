using System.ComponentModel.DataAnnotations;

namespace PocketMall.Models
{
    public class ContactUs
    {
        public Guid ContactUsId { get; set; }

        [DataType(DataType.Text)]
        public string Name { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
