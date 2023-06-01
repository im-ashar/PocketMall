using Microsoft.EntityFrameworkCore;
using PocketMall.Models.IRepositories;

namespace PocketMall.Models.Repositories
{
    public class AccountsRepository : IAccountsRepository
    {
        private readonly AccountsDbContext _context;

        public AccountsRepository(AccountsDbContext context)
        {
            _context = context;
        }

        public async Task<bool> StoreUserToDb(UserModel model)
        {
            await _context.Users.AddAsync(model);
            var result = await _context.SaveChangesAsync();
            if (result >= 1)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> AuthorizeUserFromDb(UserModel model)
        {
            var result = await _context.Users.Where(x => x.Email == model.Email && x.Password == model.Password).FirstOrDefaultAsync();
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
