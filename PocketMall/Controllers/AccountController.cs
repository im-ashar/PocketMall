using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;

namespace PocketMall.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountsRepository _repo;
        public AccountController(IAccountsRepository repo)
        {
            _repo = repo;
        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Signup(UserModel model)
        {
            if (await _repo.StoreUserToDb(model))
            {
                return RedirectToAction("Index","Home");
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
            if(await _repo.AuthorizeUserFromDb(model))
            {
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}


