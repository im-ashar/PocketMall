using Microsoft.AspNetCore.Mvc;
using PocketMall.Models.IRepositories;
using PocketMall.Models;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PocketMall.SignalR;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace PocketMall.Controllers
{
    public class OrderController : Controller
    {

        private readonly IAppGenericRepository<Order> _orderRepo;
        private readonly IAppGenericRepository<Product> _productRepo;
        private readonly IHubContext<SignalRConnection> _hubContext;

        List<Product> OrderProductsList = new List<Product>();


        public OrderController(IAppGenericRepository<Order> orderRepo, IAppGenericRepository<Product> productRepo, IHubContext<SignalRConnection> hubContext)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> PlaceOrder(Order orderModel)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var sess = HttpContext.Session.Keys.Where(x => x == "OrderProductsList").FirstOrDefault();
            OrderProductsList = JsonSerializer.Deserialize<List<Product>>(HttpContext.Session.GetString(sess));
            if (sess != null && OrderProductsList.Count != 0)
            {
                int totalPrice = 0;
                orderModel.OrderId = Guid.NewGuid();
                orderModel.Products = new List<Product>();
                foreach (var product in OrderProductsList)
                {
                    totalPrice = totalPrice + int.Parse(product.Price);
                    var x = await _productRepo.GetByIdAsync(product.ProductId);
                    orderModel.Products.Add(x);
                }
                orderModel.TotalPrice = totalPrice.ToString();
                orderModel.OrderDate = DateTime.Now;
                await _orderRepo.AddAsync(orderModel);
                HttpContext.Session.Remove(sess);
                await _hubContext.Clients.Users(userId).SendAsync("OrderPlaced", $"Your Order Has Been Placed. Your Order No Is {orderModel.OrderId}.");
                Thread.Sleep(2000);
            }
            return RedirectToAction("GetAllProducts", "Product");
        }


        [Authorize]
        public async Task<IActionResult> AddToCart(Guid productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var product = await _productRepo.GetByIdAsync(productId);
            var sess = HttpContext.Session.Keys.Where(x => x == "OrderProductsList").FirstOrDefault();
            if (sess != null)
            {
                OrderProductsList = JsonSerializer.Deserialize<List<Product>>(HttpContext.Session.GetString(sess));
                TempData["CartCount"] = OrderProductsList.Count;
                TempData.Keep();
                OrderProductsList.Add(product);
                var serealizeOrderProductsList = JsonSerializer.Serialize(OrderProductsList);
                HttpContext.Session.SetString("OrderProductsList", serealizeOrderProductsList);
                await _hubContext.Clients.Users(userId).SendAsync("AddedToCart", $"{product.Name} Added To Cart");
                Thread.Sleep(3000);
                return RedirectToAction("GetAllProducts", "Product");
            }
            else
            {
                OrderProductsList.Add(product);
                TempData["CartCount"] = OrderProductsList.Count;
                TempData.Keep();
                var serealizeOrderProductsList = JsonSerializer.Serialize(OrderProductsList);
                HttpContext.Session.SetString("OrderProductsList", serealizeOrderProductsList);
                await _hubContext.Clients.Users(userId).SendAsync("AddedToCart", $"{product.Name} Added To Cart");
                Thread.Sleep(3000);
                return RedirectToAction("GetAllProducts", "Product");
            }
        }
        public IActionResult ViewCart()
        {
            return ViewComponent("CartList");
        }
        public IActionResult RemoveFromCart(Guid productId)
        {
            var sess = HttpContext.Session.Keys.Where(x => x == "OrderProductsList").FirstOrDefault();
            if (sess != null)
            {
                OrderProductsList = JsonSerializer.Deserialize<List<Product>>(HttpContext.Session.GetString(sess));
                foreach (var product in OrderProductsList.ToList())
                {
                    if (product.ProductId == productId)
                    {
                        OrderProductsList.Remove(product);
                        break;
                    }
                }
                var serealizeOrderProductsList = JsonSerializer.Serialize(OrderProductsList);
                HttpContext.Session.SetString("OrderProductsList", serealizeOrderProductsList);
            }
            return ViewComponent("CartList");
        }
        public IActionResult Checkout()
        {
            return View();
        }
    }
}

/* var tempProductId = TempData["ProductId"];
 var productId = (Guid)tempProductId;
 orderModel.OrderId = Guid.NewGuid();
 await _orderRepo.AddAsync(orderModel);
 var productModel = await _productRepo.GetByIdAsync(productId);
 //Now add to OrderProduct Table
 var orderProduct = new OrderProduct { Order = orderModel, Product = productModel };
 await _orderProductRepo.AddAsync(orderProduct);
 await _hubContext.Clients.All.SendAsync("OrderPlaced", $"Order Id " + orderModel.OrderId + " Has Been Placed");
 Thread.Sleep(2000);*/