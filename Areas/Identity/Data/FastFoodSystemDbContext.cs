using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;

public class FastFoodSystemDbContext : IdentityDbContext<IdentityUser>
{
    public FastFoodSystemDbContext(DbContextOptions<FastFoodSystemDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Rename ASP.NET Identity tables if needed
        builder.Entity<IdentityUser>().ToTable("Users");
        builder.Entity<IdentityRole>().ToTable("Roles");
        builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
    }

    public DbSet<Cart> Carts { get; set; } = default!;

    //public DbSet<CartProduct> CartProducts { get; set; } = default!;

    public DbSet<Customer> Customers { get; set; } = default!;

    public DbSet<Order> Orders { get; set; } = default!;

    //public DbSet<Product> Products { get; set; } = default!;

    public DbSet<CartBeverageProduct> CartBeverageProducts { get; set; } = default!;

    public DbSet<CartFoodProduct> CartFoodProducts { get; set; } = default!;

    public DbSet<CartSauceProduct> CartSauceProducts { get; set; } = default!;

    public DbSet<BeverageProduct> BeverageProducts { get; set; } = default!;

    public DbSet<FoodProduct> FoodProducts { get; set; } = default!;

    public DbSet<SauceProduct> SauceProducts { get; set; } = default!;

}
