using Microsoft.AspNetCore.Mvc;
using PocketMall.Models.IRepositories;
using PocketMall.Models;

namespace PocketMall.Controllers
{
    public class OrderController : Controller
    {

        private readonly IAppRepository<OrderModel> _orderRepo;

        public OrderController(IAppRepository<OrderModel> orderRepo)
        {
            _orderRepo = orderRepo;
        }

        public async Task<IActionResult> PlaceOrder(OrderModel orderModel)
        {
            await _orderRepo.AddAsync(orderModel);
            return RedirectToAction("GetAllProducts", "Product");
        }
    }
}
