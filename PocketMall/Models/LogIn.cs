using System.ComponentModel.DataAnnotations;

namespace PocketMall.Models
{
    public class LogIn
    {
        [Required]
        public string UserNameOrEmail { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
