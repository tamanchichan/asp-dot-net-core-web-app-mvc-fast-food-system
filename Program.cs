using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FastFoodSystemDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FastFoodSystemDbContextConnection' not found."); ;

builder.Services.AddDbContext<FastFoodSystemDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDefaultIdentity<IdentityUser>
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
    .AddEntityFrameworkStores<FastFoodSystemDbContext>(); // Use the FastFoodSystemDbContext for Identity

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Seed default data to the database
using (IServiceScope scope = app.Services.CreateScope())
{
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        await DefaultUsers.Initialize(services);
        await DefaultFoodProducts.InitializeJson();
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
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
