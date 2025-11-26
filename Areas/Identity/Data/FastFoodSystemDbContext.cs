using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;

public class FastFoodSystemDbContext : IdentityDbContext<IdentityUser>
{
    public FastFoodSystemDbContext(DbContextOptions<FastFoodSystemDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
