using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data
{
    public static class DefaultBeverageProducts
    {
        // Seeds the database with default BeverageProducts if none exist.  
        // Creates a DbContext using DI, checks for existing records, and inserts the default list when empty.
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            FastFoodSystemDbContext context = new FastFoodSystemDbContext
            (
                serviceProvider.GetRequiredService<DbContextOptions<FastFoodSystemDbContext>>()
            );

            if (!context.BeverageProducts.Any())
            {
                context.BeverageProducts.AddRangeAsync(DefaultProducts);
                await context.SaveChangesAsync();
            }
        }

        // Loads BeverageProduct data from a JSON file.
        // If the file doesn't exist, it creates the directory, writes default
        // products to JSON, and then returns those defaults.
        public static async Task<HashSet<BeverageProduct>> InitializeJson()
        {
            HashSet<BeverageProduct> products;

            if (!File.Exists(ProductsFilePath))
            {
                products = DefaultProducts;

                Directory.CreateDirectory(Path.GetDirectoryName(ProductsFilePath));

                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    ReferenceHandler = ReferenceHandler.Preserve
                };

                await File.WriteAllTextAsync(ProductsFilePath, JsonSerializer.Serialize(products, options));
            }
            else
            {
                string json = File.ReadAllText(ProductsFilePath);

                products = JsonSerializer.Deserialize<HashSet<BeverageProduct>>(json, new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve
                });
            }

            return products;
        }
        public static void SaveProducts(HashSet<BeverageProduct> products)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(ProductsFilePath));

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            File.WriteAllText(ProductsFilePath, JsonSerializer.Serialize(products, options));
        }

        // Parent.Parent.Parent (net9.0 > debug > bin > Project Folder)
        public static readonly string ProductsFilePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName, "Areas/Identity/Data", "beverageProducts.json");

        public static HashSet<BeverageProduct> DefaultProducts = new HashSet<BeverageProduct>()
        {
            new BeverageProduct("BEVERAGE", "Beverage", null, 2.25m, ProductCategory.Extras, null, true)
        };
    }
}
