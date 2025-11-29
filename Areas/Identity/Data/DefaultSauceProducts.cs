using Microsoft.EntityFrameworkCore;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data
{
    public static class DefaultSauceProducts
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            FastFoodSystemDbContext context = new FastFoodSystemDbContext
            (
                serviceProvider.GetRequiredService<DbContextOptions<FastFoodSystemDbContext>>()
            );

            if (!context.SauceProducts.Any())
            {
                context.SauceProducts.AddRange
                (
                    new SauceProduct("Sauce", "Sauce", 1.75m, true)
                );
            }
        }
    }
}
