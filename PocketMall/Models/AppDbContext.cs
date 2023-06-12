using Microsoft.EntityFrameworkCore;

namespace PocketMall.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderModel>()
                .HasMany(x => x.Products)
                .WithMany(x => x.Orders)
                .UsingEntity(x => x.ToTable("OrderModelProductModel"));
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<UserModel> Users { get; set; }
    }
}
