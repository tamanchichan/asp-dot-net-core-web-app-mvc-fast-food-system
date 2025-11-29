using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts;
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

        // Configure decimal precision for Cart properties
        builder.Entity<Cart>()
            .Property(c => c.AdditionalCharge)
            .HasPrecision(18, 2);

        builder.Entity<Cart>()
            .Property(c => c.DeliveryFee)
            .HasPrecision(18, 2);

        builder.Entity<Cart>()
            .Property(c => c.Discount)
            .HasPrecision(18, 2);

        // Configure decimal precision for Product properties
        builder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

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

    public DbSet<OrderBeverageProduct> OrderBeverageProducts { get; set; } = default!;

    public DbSet<OrderFoodProduct> OrderFoodProducts { get; set; } = default!;

    public DbSet<OrderSauceProduct> OrderSauceProducts { get; set; } = default!;

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<BeverageProduct> BeverageProducts { get; set; } = default!;

    public DbSet<FoodProduct> FoodProducts { get; set; } = default!;

    public DbSet<SauceProduct> SauceProducts { get; set; } = default!;

}
