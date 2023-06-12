using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using System.Diagnostics;

namespace PocketMall.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (HttpContext.Request.Cookies.ContainsKey("signUpDate"))
            {
                ViewBag.Cookie = HttpContext.Request.Cookies["signUpDate"];
                return View();
            }
            else
            {
                HttpContext.Response.Cookies.Append("signUpDate", DateTime.Now.ToString());
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}