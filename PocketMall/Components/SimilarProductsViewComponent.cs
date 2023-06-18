using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;

namespace PocketMall.Components
{
    public class SimilarProductsViewComponent : ViewComponent
    {
        private readonly IAppNonGenericRepository _repo;
        public SimilarProductsViewComponent(IAppNonGenericRepository repo)
        {
            _repo = repo;
        }

        public async Task<IViewComponentResult> InvokeAsync(string category,Guid productId)
        {
            var result = await _repo.GetSimilarProductsAsync(category,productId);
            return View(result);
        }
    }
}
