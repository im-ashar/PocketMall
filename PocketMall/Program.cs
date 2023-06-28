using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PocketMall.Models;
using PocketMall.Models.IRepositories;
using PocketMall.Models.Repositories;
using PocketMall.SignalR;

var builder = WebApplication.CreateBuilder(args);

string connString = @$"Data Source=(localdb)\MSSQLLocalDB;Database=PocketMallDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

var migrationAssembly = typeof(Program).Assembly.GetName().Name;
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseSqlServer(connString, sql => sql.MigrationsAssembly(migrationAssembly)));
// Add services to the container.

builder.Services.AddIdentity<User, IdentityRole>(options => options.User.RequireUniqueEmail = true).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped(typeof(IAppGenericRepository<>), typeof(AppGenericRepository<>));
builder.Services.AddScoped<IAppNonGenericRepository, AppNonGenericRepository>();
builder.Services.AddSignalR();


builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => options.IdleTimeout = TimeSpan.FromMinutes(60));

var app = builder.Build();
//enable session
app.UseSession();

var scope = app.Services.CreateScope();

await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();

var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
if (!await roleManager.RoleExistsAsync("Admin"))
{
    var adminRole = new IdentityRole("Admin");
    await roleManager.CreateAsync(adminRole);
}

// Check if the "User" role exists and create it if it doesn't
if (!await roleManager.RoleExistsAsync("User"))
{
    var userRole = new IdentityRole("User");
    await roleManager.CreateAsync(userRole);
}

var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
var adminUser = await userManager.FindByEmailAsync("admin@pocketmall.com");

if (adminUser == null)
{
    adminUser = new User
    {
        UserName = "admin",
        Email = "admin@pocketmall.com",
        EmailConfirmed = true,
        LockoutEnabled = false,
        PhoneNumber = "923094394150",
        Name = "Admin",
        PhoneNumberConfirmed = true
    };
    var result = await userManager.CreateAsync(adminUser, "Admin@123");
    if (result.Succeeded)
    {
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=LogIn}/{id?}");

app.MapHub<SignalRConnection>("/connection");
app.Run();
