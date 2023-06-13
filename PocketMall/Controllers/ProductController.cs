using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;
using System.IO;

namespace PocketMall.Controllers
{
    public class ProductController : Controller
    {
        private readonly IAppRepository<Product> _productRepo;

        public ProductController(IAppRepository<Product> productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult AddProduct()
        {
            var product = new Product { AllCategories = new List<string> { "Men", "Women", "Kids" } };
            ViewBag.ProductAdded = TempData["ProductAdded"];
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model)
        {
            model.ProductId = Guid.NewGuid();
            var extension = Path.GetExtension(model.Image.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productImages", model.ProductId + extension);
            model.ImageUrl = path;
            using (var stream = new FileStream(path, FileMode.Create))
            {
                model.Image.CopyTo(stream);

            }
            await _productRepo.AddAsync(model);
            TempData["ProductAdded"] = "Added";
            return RedirectToAction("AddProduct");
        }

        public async Task<IActionResult> GetAllProducts()
        {
            var productList = await _productRepo.GetAllAsync();
            return View(productList);
        }
        //For Adding Multiple Files
        public async Task<IActionResult> Store(List<IFormFile> images)
        {
            foreach (var image in images)
            {

                var model = new Product();
                model.ProductId = Guid.NewGuid();
                var extension = Path.GetExtension(image.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productImages", model.ProductId + extension);
                model.Name = Path.GetFileNameWithoutExtension(image.FileName);
                model.Price = "78";
                model.Description = "Description of " + image.FileName;
                model.Category = "Men";
                model.ImageUrl = path;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);

                }
                await _productRepo.AddAsync(model);
            }
            return Content("All Files Added");
        }

        public async Task<IActionResult> BuyProduct(Guid productId)
        {
            TempData["ProductId"] = productId;
            //Product product = await _productRepo.GetByIdAsync(productId);
            //Order order = new Order();
            //order.CustomerName = "Ali";
            //order.CustomerAddress = "Lahore";
            //order.Products=new List<Product>();
            //order.Products.Add(product);
            //product.Orders = new List<Order> { order };
            //await _productRepo.AddAsync(product);
            return View();
        }


    }
}
