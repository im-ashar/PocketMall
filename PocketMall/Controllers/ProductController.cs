using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;
using System.IO;

namespace PocketMall.Controllers
{
    public class ProductController : Controller
    {
        private readonly IAppRepository<ProductModel> _productRepo;

        public ProductController(IAppRepository<ProductModel> productRepo)
        {
            _productRepo = productRepo;
        }

        public IActionResult AddProduct()
        {
            var product = new ProductModel { AllCategories = new List<string> { "Men", "Women", "Kids" } };
            ViewBag.ProductAdded = TempData["ProductAdded"];
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductModel model)
        {
            model.Id = Guid.NewGuid();
            var extension = Path.GetExtension(model.Image.FileName);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productImages", model.Id + extension);
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

                var model = new ProductModel();
                model.Id = Guid.NewGuid();
                var extension = Path.GetExtension(image.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productImages", model.Id + extension);
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
            var product = await _productRepo.GetByIdAsync(productId);
            OrderModel order = new OrderModel();
            order.CustomerName = "Ali";
            order.CustomerAddress = "Lahore";
            order.Products.Add(product);
            product.Orders = new List<OrderModel> { order };
            //await _productRepo.AddAsync(product);
            return  RedirectToAction("PlaceOrder","Order",order);
        }


    }
}
