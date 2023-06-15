using System.ComponentModel.DataAnnotations;

namespace PocketMall.Models
{
    public class SignUp
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [StringLength(20, ErrorMessage = "UserName Length Should Not Exceed 20 Characters")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password And Confirm Password Does Not Match")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Phone Number Should Be Valid")]
        public string PhoneNumber { get; set; }
    }
}
