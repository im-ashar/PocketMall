using Microsoft.EntityFrameworkCore;
using PocketMall.Models.IRepositories;
using PocketMall.Models.Repositories;

var builder = WebApplication.CreateBuilder(args);

string connString = @$"Data Source=(localdb)\MSSQLLocalDB;Database=PocketMallDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

var migrationAssembly = typeof(Program).Assembly.GetName().Name;
builder.Services.AddDbContext<AccountsDbContext>(options =>
options.UseSqlServer(connString, sql => sql.MigrationsAssembly(migrationAssembly)));
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();

var app = builder.Build();
var scope=app.Services.CreateScope();

scope.ServiceProvider.GetRequiredService<AccountsDbContext>().Database.MigrateAsync().Wait();
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
    pattern: "{controller=Account}/{action=Signin}/{id?}");

app.Run();
