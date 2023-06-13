using Microsoft.AspNetCore.Mvc;

namespace PocketMall.Components
{
    public class ProductDetailsViewComponent:ViewComponent 
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
