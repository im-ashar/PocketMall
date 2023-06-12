using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;

namespace PocketMall.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppRepository<UserModel> _userRepo;
        public AccountController(IAppRepository<UserModel> userRepo)
        {
            _userRepo = userRepo;
            
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(UserModel model)
        {
            model.Id=Guid.NewGuid();
            if (await _userRepo.AddAsync(model)!=null)
            {
                return RedirectToAction("Signin");
            }
            return View();
        }
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Signin(UserModel model)
        {
            if(await _userRepo.AuthorizeUserFromDb(model))
            {
                return RedirectToAction("GetAllProducts","Product");
            }
            return View();
        }
    }
}


