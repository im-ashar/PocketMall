using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PocketMall.Models.IRepositories;
using System.ComponentModel.DataAnnotations;

namespace PocketMall.Models.Repositories
{
    public class AppRepository<T> : IAppRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AppRepository(AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager)
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


        //Generic Repository Functions
        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int?> AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return result;
            }
            return null;
        }

        public async Task<int?> UpdateAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return result;
            }
            return null;
        }

        public async Task<int?> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return result;
            }
            return null;
        }
        public async Task<T> GetIfExistsAsync(T entity)
        {
            var existingEntity = await _context.Set<T>().Where(e => e.Equals(entity)).FirstOrDefaultAsync();
            return existingEntity;
        }

    }
}
