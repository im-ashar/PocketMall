using Microsoft.AspNetCore.Mvc;

namespace PocketMall.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Signup()
        {
            return View();
        }
        public IActionResult Signin()
        {
            return View();
        }
    }
}
