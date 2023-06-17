using Microsoft.AspNetCore.Mvc;
using PocketMall.Models.IRepositories;
using PocketMall.Models;
using Microsoft.Extensions.FileProviders;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.AspNetCore.SignalR;
using PocketMall.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace PocketMall.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAppGenericRepository<Product> _productRepo;
        private readonly IHubContext<SignalRConnection> _hubContext;

        public AdminController(IAppGenericRepository<Product> productRepo, IHubContext<SignalRConnection> hubContext)
        {
            _productRepo = productRepo;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> ListOfProducts()
        {
            var productList = await _productRepo.GetAllAsync();
            return View(productList);
        }
        public IActionResult AddProduct()
        {
            ViewBag.ProductAdded = TempData["ProductAdded"];
            var product = new Product();
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product model)
        {
            if (ModelState.IsValid)
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
                await _hubContext.Clients.All.SendAsync("ProductAdded", $"Product Id " + model.ProductId + " Has Been Added");
                Thread.Sleep(5000);
                return RedirectToAction("AddProduct");
            }
            return View(model);
        }

        //For Multiple Files, Only For Development Purpose
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
                model.Quantity = 100;
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    image.CopyTo(stream);

                }
                await _productRepo.AddAsync(model);
            }
            return Content("All Files Added");
        }

        public IActionResult ReceiveProductId(Guid productId)
        {
            TempData["ProductId"] = productId;
            return RedirectToAction("UpdateProduct");
        }
        public async Task<IActionResult> UpdateProduct()
        {
            var productId = (Guid)TempData["ProductId"];
            TempData["ProductId"] = productId;
            var result = await _productRepo.GetByIdAsync(productId);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatedProductToDb(Product model)
        {
            var productId = (Guid)TempData["ProductId"];

            var result = await _productRepo.GetByIdAsync(productId);
            if (model.Name != null)
            {
                result.Name = model.Name;
            }
            if (model.Description != null)
            {
                result.Description = model.Description;
            }
            if (model.Price != null)
            {
                result.Price = model.Price;
            }
            if (model.Quantity != null)
            {
                result.Quantity = model.Quantity;
            }
            if (model.Category != null)
            {
                result.Category = model.Category;
            }
            if (model.UpdateImage != null)
            {
                result.ImageUrl = model.ImageUrl;
                var extension = Path.GetExtension(model.UpdateImage.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "productImages", result.ProductId + extension);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    model.UpdateImage.CopyTo(stream);
                }
                result.ImageUrl = path;
            }
            await _productRepo.UpdateAsync(result);
            return RedirectToAction("ListOfProducts");
        }
        public async Task<IActionResult> DeleteProduct(Guid productId)
        {
            var result = await _productRepo.GetByIdAsync(productId);
            await _productRepo.DeleteAsync(result);
            return RedirectToAction("ListOfProducts");
        }
    }
}

