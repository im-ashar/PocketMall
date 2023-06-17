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
            return View(productList);
        }

        public async Task<IActionResult> ProductDetails(Guid productId)
        {
            var product = await _productRepo.GetByIdAsync(productId);
            return View(product);
        }


    }
}
