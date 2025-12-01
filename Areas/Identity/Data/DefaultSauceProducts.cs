using Microsoft.EntityFrameworkCore;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;

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
                await context.SauceProducts.AddRangeAsync
                (
                    new SauceProduct("Sauce", "Sauce", null, 1.75m, ProductCategory.Extras, true)
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
