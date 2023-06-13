using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;

namespace PocketMall.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppRepository<User> _userRepo;
        public AccountController(IAppRepository<User> userRepo)
        {
            _userRepo = userRepo;
            
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(User model)
        {
            model.UserId=Guid.NewGuid();
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
        public async Task<IActionResult> Signin(User model)
        {
            if(await _userRepo.AuthorizeUserFromDb(model))
            {
                return RedirectToAction("GetAllProducts","Product");
            }
            return View();
        }
    }
}


