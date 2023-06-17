using Microsoft.AspNetCore.Mvc;
using PocketMall.Models.IRepositories;
using PocketMall.Models;
using Microsoft.AspNetCore.SignalR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using PocketMall.SignalR;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Security.Claims;

namespace PocketMall.Controllers
{
    public class OrderController : Controller
    {

        private readonly IAppGenericRepository<Order> _orderRepo;
        private readonly IAppGenericRepository<Product> _productRepo;
        private readonly IAppGenericRepository<OrderProduct> _orderProductRepo;
        private readonly IHubContext<SignalRConnection> _hubContext;



        public OrderController(IAppGenericRepository<Order> orderRepo, IAppGenericRepository<Product> productRepo, IAppGenericRepository<OrderProduct> orderProductRepo, IHubContext<SignalRConnection> hubContext)
        {
            _productRepo = productRepo;
            _orderRepo = orderRepo;
            _orderProductRepo = orderProductRepo;
            _hubContext = hubContext;
        }

        public async Task<IActionResult> PlaceOrder(Order orderModel)
        {
            var tempProductId = TempData["ProductId"];
            var productId = (Guid)tempProductId;
            orderModel.OrderId = Guid.NewGuid();
            await _orderRepo.AddAsync(orderModel);
            var productModel = await _productRepo.GetByIdAsync(productId);
            //Now add to OrderProduct Table
            var orderProduct = new OrderProduct { Order = orderModel, Product = productModel };
            await _orderProductRepo.AddAsync(orderProduct);
            await _hubContext.Clients.All.SendAsync("OrderPlaced", $"Order Id " + orderModel.OrderId + " Has Been Placed");
            Thread.Sleep(2000);
            return RedirectToAction("GetAllProducts", "Product");
        }

        public IActionResult BuyProduct(Guid productId)
        {
            TempData["ProductId"] = productId;
            return View();
        }
        List<Product> OrderProductsList = new List<Product>();
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
                _hubContext.Clients.Users(userId).SendAsync("AddedToCart", $"{product.Name} Added To Cart");
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
                _hubContext.Clients.Users(userId).SendAsync("AddedToCart", $"{product.Name} Added To Cart");
                Thread.Sleep(3000);
                return RedirectToAction("GetAllProducts", "Product");
            }
        }
    }
}
