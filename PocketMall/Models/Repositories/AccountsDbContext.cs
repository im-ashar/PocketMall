using Microsoft.EntityFrameworkCore;

namespace PocketMall.Models.Repositories
{
    public class AccountsDbContext:DbContext
    {
        public AccountsDbContext(DbContextOptions<AccountsDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<UserModel> Users { get; set; }
    }
}
