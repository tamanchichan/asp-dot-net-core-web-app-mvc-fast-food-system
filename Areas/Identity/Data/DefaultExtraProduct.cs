using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data
{
    public static class DefaultExtraProducts
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            FastFoodSystemDbContext context = new FastFoodSystemDbContext
            (
                serviceProvider.GetRequiredService<DbContextOptions<FastFoodSystemDbContext>>()
            );

            if (!context.ExtraProducts.Any())
            {
                await context.ExtraProducts.AddRangeAsync
                (
                    new ExtraProduct("DN", "Dry Noodles", null, 1.00m, ProductCategory.Extras, null, false)
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
