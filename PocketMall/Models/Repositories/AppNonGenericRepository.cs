using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PocketMall.Models.IRepositories;
using System.ComponentModel.DataAnnotations;

namespace PocketMall.Models.Repositories
{
    public class AppNonGenericRepository : IAppNonGenericRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AppNonGenericRepository(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task LogOutUser()
        {
            await _signInManager.SignOutAsync();
            return;
        }

        public async Task<IdentityResult> SignUpUser(SignUp model)
        {
            var user = new User
            {
                Name = model.Name,
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                PhoneNumberConfirmed = true,
                LockoutEnabled = false,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<SignInResult> LogInUser(LogIn model)
        {
            var email = new EmailAddressAttribute();
            if (email.IsValid(model.UserNameOrEmail))
            {
                var user = await _userManager.FindByEmailAsync(model.UserNameOrEmail);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, true, false);
                    return result;
                }

            }
            var result2 = await _signInManager.PasswordSignInAsync(model.UserNameOrEmail, model.Password, true, false);
            return result2;
        }
        public async Task<List<Product>> GetSimilarProductsAsync(string category, Guid productId)
        {
            var result = await _context.Products.Where(x => x.Category == category).ToListAsync();
            foreach (var product in result.ToList())
            {
                if (product.ProductId == productId)
                {
                    result.Remove(product);
                    break;
                }
            }
            return result;
        }
    }
}
