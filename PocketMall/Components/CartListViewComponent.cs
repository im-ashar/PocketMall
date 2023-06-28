using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using System.Text.Json;

namespace PocketMall.Components
{
    public class CartListViewComponent : ViewComponent
    {
        List<Product> OrderProductsList = new List<Product>();
        public IViewComponentResult Invoke()
        {
            var sess = HttpContext.Session.Keys.Where(x => x == "OrderProductsList").FirstOrDefault();
            if (sess != null)
            {
                OrderProductsList = JsonSerializer.Deserialize<List<Product>>(HttpContext.Session.GetString(sess));
                return View(OrderProductsList);

            }
            return View();
        }
    }
}
