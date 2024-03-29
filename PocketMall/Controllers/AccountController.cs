﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PocketMall.Models;
using PocketMall.Models.IRepositories;

namespace PocketMall.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAppNonGenericRepository _userRepo;
        public AccountController(IAppNonGenericRepository userRepo)
        {
            _userRepo = userRepo;

        }
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUp model)
        {
            if (ModelState.IsValid)
            {
                var signUpResult = await _userRepo.SignUpUser(model);
                if (signUpResult.Succeeded)
                {
                    return RedirectToAction("LogIn");
                }
                foreach (var error in signUpResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult LogIn()
        {
            if (User.Identity.IsAuthenticated)
            {
                HttpContext.Response.Cookies.Append($"loginDate{User.Identity.Name}", DateTime.Now.ToString(), new CookieOptions { Expires = DateTime.Now.AddYears(1) });
                return RedirectToAction("GetAllProducts", "Product");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LogIn model, string returnUrl = null)
        {

            if (ModelState.IsValid)
            {
                var logInResult = await _userRepo.LogInUser(model);
                if (logInResult.Succeeded)
                {
                    HttpContext.Response.Cookies.Append($"loginDate{User.Identity.Name}", DateTime.Now.ToString(), new CookieOptions { Expires = DateTime.Now.AddYears(1) });
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl);
                    }
                    return RedirectToAction("GetAllProducts", "Product");
                }
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
            }
            return View();
        }

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await _userRepo.LogOutUser();
            HttpContext.Session.Remove("OrderProductsList");
            return RedirectToAction("GetAllProducts", "Product");
        }
        public IActionResult AccessDenied()
        {
            return RedirectToAction("GetAllProducts", "Product");
        }
    }
}


