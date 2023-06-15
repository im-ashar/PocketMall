using Microsoft.AspNetCore.Identity;

namespace PocketMall.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
    }
}
