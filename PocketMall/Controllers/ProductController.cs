using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;
using System.IO;

namespace PocketMall.Controllers
{
    public class ProductController : Controller
    {
        private readonly IAppGenericRepository<Product> _productRepo;

        public ProductController(IAppGenericRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        

        public async Task<IActionResult> GetAllProducts()
        {
            var productList = await _productRepo.GetAllAsync();
            if (HttpContext.Request.Cookies.ContainsKey("signUpDate"))
            {
                ViewBag.Cookie = HttpContext.Request.Cookies["signUpDate"];
                return View(productList);
            }
            else
            {
                HttpContext.Response.Cookies.Append("signUpDate", DateTime.Now.ToString());
                return View(productList);
            }
        }
        public IActionResult BuyProduct(Guid productId)
        {
            TempData["ProductId"] = productId;
            return View();
        }
        public async Task<IActionResult> ProductDetails(Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            return View(product);
        }

        /*public async Task<IActionResult> AddToCart(Guid productId)
        {
            if(HttpContext.)
            HttpContext.Session.SetString("", "");
        }*/
    }
}
