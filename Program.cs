using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.AspNetCore.Mvc;
using asp_dot_net_core_web_app_mvc_fast_food_system.POS;
using System.Text;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = WebApplication.CreateBuilder(args);

// Load the SQL Server connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("FastFoodSystemDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FastFoodSystemDbContextConnection' not found."); ;

// Register the DbContext using SQL Server
//builder.Services.AddDbContext<FastFoodSystemDbContext>(options => options.UseSqlServer(connectionString));

// Register the DbContext using Sqlite
builder.Services.AddDbContext<FastFoodSystemDbContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<SystemUser>
    (options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 3;
    })
    .AddRoles<IdentityRole>() // Enable Roles
    .AddEntityFrameworkStores<FastFoodSystemDbContext>() // Use the FastFoodSystemDbContext for Identity
    .AddDefaultTokenProviders(); // no AddDefaultUI

builder.Services.AddScoped<ThermalPrinterService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/login";
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    // 0.0.0.0 listen on all network interfaces
    app.Urls.Add("http://0.0.0.0:5000"); // Enable mobile access when publishing to an executable file
}


// Seed default data to the database
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        await DefaultUsers.Initialize(services);
        await DefaultFoodProducts.Initialize(services);
        await DefaultBeverageProducts.Initialize(services);
        await DefaultSauceProducts.Initialize(services);

        // Initialize default food products by creating a JSON file
        //await DefaultFoodProducts.InitializeJson();
        //await DefaultBeverageProducts.InitializeJson();
    }
    catch (Exception ex)
    {
        // Log errors or handle them as needed
        Console.WriteLine(ex.Message);
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

    // Only redicret to https if it is in production (can access http request from mobile)
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();
app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages(); // ASP.NET Identity's default Razor Pages UI, which maps /Identity/Account/Login

app.Run();
