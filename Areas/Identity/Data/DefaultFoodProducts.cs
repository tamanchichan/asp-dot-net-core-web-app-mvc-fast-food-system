using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data
{
    public static class DefaultFoodProducts
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            // This method can be used to initialize default food products in the database.
            // Implementation can be added as needed.

            FastFoodSystemDbContext context = new FastFoodSystemDbContext
            (
                serviceProvider.GetRequiredService<DbContextOptions<FastFoodSystemDbContext>>()
            );

            AddProductCategory(AppetizerFoodProducts, "Soup & Appetizers");
            AddProductCategory(MixedGreensProducts, "Mixed Greens");
            AddProductCategory(NoodlesProducts, "Noodles");
            AddProductCategory(FriedRiceProducts, "Fried Rice");
            AddProductCategory(ChopSueyProducts, "Chop Suey");
            AddProductCategory(EggFooYungProducts, "Egg Foo Yung");
            AddProductCategory(SeaFoodProducts, "Sea Food");
            AddProductCategory(ChickenProducts, "Chicken");
            AddProductCategory(BeefAndPorkProducts, "Beef & Pork");
            AddProductCategory(HotAndSpicyProducts, "Hot & Spicy");
            AddProductCategory(ComboPlateProducts, "Combo Plates");
            AddProductCategory(FamilyDinnerProducts, "Family Dinners");

            HashSet<FoodProduct> products = new HashSet<FoodProduct>
            (
                AppetizerFoodProducts
                    .Concat(MixedGreensProducts)
                    .Concat(NoodlesProducts)
                    .Concat(FriedRiceProducts)
                    .Concat(ChopSueyProducts)
                    .Concat(EggFooYungProducts)
                    .Concat(SeaFoodProducts)
                    .Concat(ChickenProducts)
                    .Concat(BeefAndPorkProducts)
                    .Concat(HotAndSpicyProducts)
                    .Concat(ComboPlateProducts)
                    .Concat(FamilyDinnerProducts)
            );

            // Add products to the database if there are none
            if (!context.FoodProducts.Any())
            {
                context.FoodProducts.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
        public static void AddProductCategory(HashSet<FoodProduct> products, string category)
        {
            foreach (FoodProduct product in products)
            {
                switch (category)
                {
                    case "Soup & Appetizers":
                        product.Category = ProductCategory.Appetizers;
                        break;

                    case "Mixed Greens":
                        product.Category = ProductCategory.MixedGreens;
                        break;

                    case "Noodles":
                        product.Category = ProductCategory.Noodles;
                        break;

                    case "Fried Rice":
                        product.Category = ProductCategory.FriedRice;
                        break;

                    case "Chop Suey":
                        product.Category = ProductCategory.ChopSuey;
                        break;

                    case "Egg Foo Yung":
                        product.Category = ProductCategory.EggFooYung;
                        break;

                    case "Sea Food":
                        product.Category = ProductCategory.SeaFood;
                        break;

                    case "Chicken":
                        product.Category = ProductCategory.Chicken;
                        break;

                    case "Beef & Pork":
                        product.Category = ProductCategory.BeefAndPork;
                        break;

                    case "Hot & Spicy":
                        product.Category = ProductCategory.HotAndSpicy;
                        break;

                    case "Combo Plates":
                        product.Category = ProductCategory.ComboPlates;
                        break;

                    case "Family Dinners":
                        product.Category = ProductCategory.FamilyDinners;
                        break;

                    case "Extras":
                        product.Category = ProductCategory.Extras;
                        break;
                    default:
                        break;
                }
            }
        }

        public static readonly HashSet<FoodProduct> AppetizerFoodProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("1", "Deluxe Wonton Soup", 13.95m),
            new FoodProduct("2", "Wonton Soup", 6.95m),
            new FoodProduct("3", "Hot & Sour Soup", 13.95m),
            new FoodProduct("4", "Consomme Soup", 3.45m),
            new FoodProduct("5", "Barbecue Pork", 11.45m),
            new FoodProduct("6", "Deep Fried Wontons", 6.50m),
            new FoodProduct("6A", "Chicken Wings", 14.50m),
            new FoodProduct("7", "Egg Roll", 1.75m),
            new FoodProduct("7A", "Spring Roll", 1.95m),
        };

        public static readonly HashSet<FoodProduct> MixedGreensProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("8", "Deluxe Mixed Greens", 15.50m),
            new FoodProduct("8A", "Deluxe Snow Peas", 15.50m),
            new FoodProduct("9", "Shrimp Mixed Greens", 15.50m),
            new FoodProduct("9A", "Shrimp with Snow Peas", 15.50m),
            new FoodProduct("9B", "Fish with Snow Peas", 15.50m),
            new FoodProduct("9C", "Squid with Snow Peas", 15.50m),
            new FoodProduct("10", "Mixed Greens with", 13.95m, true),
            new FoodProduct("10A", "Snow Peas with", 13.95m, true),
            new FoodProduct("11", "Vegetarian Mixed Greens", 10.95m),
            new FoodProduct("11A", "Tofu with Black Bean Garlic Sauce", 13.95m),
            new FoodProduct("12", "Beef Broccoli", 14.50m),
            new FoodProduct("12A", "Beef Gai Lan", 15.50m),
            new FoodProduct("12B", "Gai Lan with Oyster Sauce", 13.95m),
        };

        public static readonly HashSet<FoodProduct> NoodlesProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("13", "Cantonese Chow Mein", 16.50m),
            new FoodProduct("14", "Shrimp Chow Mein", 16.50m),
            new FoodProduct("15", "Chow Mein with", 14.50m, true),
            new FoodProduct("16", "Vegetarian Chow Mein", 12.50m),
            new FoodProduct("16A", "Plain Lo Mein", 12.50m),
            new FoodProduct("17", "Deluxe Shangai Noodles", 15.50m),
            new FoodProduct("17B", "Beef Shangai Noodles", 15.50m),
            new FoodProduct("18", "Deluxe Rice Noodles", 15.50m),
            new FoodProduct("18A", "Beef Ho Fan", 15.50m),
            new FoodProduct("18B", "Beef Ho Fan with Black Bean Garlic Sauce", 15.50m),
            new FoodProduct("19", "Lo Mein with", 14.50m, true),
            new FoodProduct("19A", "Deluxe Lo Mein", 15.50m),
            new FoodProduct("20", "Deluxe Singapore Noodles", 15.50m)
        };

        public static readonly HashSet<FoodProduct> FriedRiceProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("21", "Deluxe Fried Rice", 14.50m),
            new FoodProduct("22", "Shrimp Fried Rice", 14.50m),
            new FoodProduct("23", "Fried Rice with", 11.50m, true),
            new FoodProduct("24", "Vegetarian Fried Rice", 12.50m),
            new FoodProduct("24A", "Plain Fried Rice", 12.50m),
            new FoodProduct("25S", "Small Steamed Rice", 2.50m),
            new FoodProduct("25L", "Large Steamed Rice", 5.00m)
        };

        public static readonly HashSet<FoodProduct> ChopSueyProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("26", "Deluxe Chop Suey", 14.50m),
            new FoodProduct("27", "Shrimp Chop Suey", 14.50m),
            new FoodProduct("28", "Chop Suey with", 13.50m, true),
            new FoodProduct("29", "Vegetarian Chop Suey", 11.50m)
        };

        public static readonly HashSet<FoodProduct> EggFooYungProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("30", "Deluxe Egg Foo Yung", 14.50m),
            new FoodProduct("31", "Shrimp Egg Foo Yung", 14.50m),
            new FoodProduct("32", "Egg Foo Yung with", 13.50m, true),
            new FoodProduct("33", "Vegetarian Egg Foo Yung", 11.50m)
        };

        public static HashSet<FoodProduct> SeaFoodProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("34", "Breaded Shrimp", 16.50m),
            new FoodProduct("34A", "Breaded Fish Fillet", 14.50m),
            new FoodProduct("34B", "Breaded Squid", 16.50m),
            new FoodProduct("35", "Black Bean Garlic Shrimp", 16.50m),
            new FoodProduct("35A", "Squid with Black Bean Garlic Sauce", 16.50m),
            new FoodProduct("35B", "Fish with Black Bean Garlic Sauce", 15.50m),
            new FoodProduct("36", "Deep Fried Shrimp", 16.50m),
            new FoodProduct("36A", "Deep Fried Fish Fillet", 14.50m),
            new FoodProduct("36B", "Deep Fried Squid", 16.50m),
            new FoodProduct("36C", "Squid with Green Onions & Ginger", 16.50m),
            new FoodProduct("37", "Sweet & Sour Shrimp", 15.50m),
            new FoodProduct("37A", "Sweet & Sour Fish Fillet", 14.50m),
            new FoodProduct("38", "Pan Fried Shrimp", 16.50m),
            new FoodProduct("39", "Spicy Garlic Shrimp", 16.50m)
        };

        public static HashSet<FoodProduct> ChickenProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("40", "Sweet & Sour Chicken Balls", 14.50m),
            new FoodProduct("41", "Honey Garlic Chicken Balls", 14.50m),
            new FoodProduct("41A", "Deep Fried Sliced Chicken with Honey Garlic Sauce", 14.50m),
            new FoodProduct("42", "Hot Honey Glazed Chicken Balls", 14.50m),
            new FoodProduct("42A", "Deep Fried Sliced Chicken with Hot Honey Garlic Sauce", 14.50m),
            new FoodProduct("43", "Mushroom Chicken Balls", 14.50m),
            new FoodProduct("44", "Honey Lemon Chicken", 14.50m),
            new FoodProduct("44A", "Sliced Chicken with Mushroom", 14.50m),
            new FoodProduct("45", "Breaded Almond Chicken", 14.50m),
            new FoodProduct("45A", "Dry Breaded Chicken", 14.50m),
            new FoodProduct("46", "Sliced Chicken with Black Bean Garlic Sauce", 14.50m),
            new FoodProduct("46A", "Sliced Chicken with Green Onions & Ginger", 14.50m)
        };

        public static HashSet<FoodProduct> BeefAndPorkProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("47", "Breaded Veal", 15.50m),
            new FoodProduct("48", "Hot Honey Glazed Veal", 15.50m),
            new FoodProduct("49", "Honey Garlic Veal", 15.50m),
            new FoodProduct("49A", "Beef with Honey Garlic Sauce", 15.50m),
            new FoodProduct("50", "Breaded Pork", 11.50m),
            new FoodProduct("50A", "Sweet & Sour Pork", 13.50m),
            new FoodProduct("51", "Honey Garlic Pork", 13.50m),
            new FoodProduct("52", "Breaded Spareribs", 15.50m),
            new FoodProduct("53", "Honey Garlic Spareribs", 15.50m),
            new FoodProduct("54", "Sweet & Sour Spareribs", 13.50m),
            new FoodProduct("55", "Spareribs with Black Bean Garlic Sauce", 15.50m),
            new FoodProduct("55A", "Pork with with Black Bean Garlic Sauce", 13.50m),
            new FoodProduct("56", "Beef with Black Bean Garlic Sauce", 15.50m),
            new FoodProduct("56A", "Beef with Green Onions & Ginger", 15.50m),
            new FoodProduct("56B", "Mushroom & Beef Stir-Fry", 15.50m),
            new FoodProduct("56C", "Pork with Green Onions & Ginger", 13.50m)
        };

        public static HashSet<FoodProduct> HotAndSpicyProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("57", "Szechuan Shrimp", 16.50m),
            new FoodProduct("57A", "Szechuan Fish Fillet", 14.50m),
            new FoodProduct("57B", "Szechuan Squid", 16.50m),
            new FoodProduct("58", "Szechuan Beef", 15.50m),
            new FoodProduct("59", "Szechuan Chicken", 14.50m),
            new FoodProduct("60", "Ginger Beef", 15.50m),
            new FoodProduct("61", "Ginger Chicken", 14.50m),
            new FoodProduct("62", "Curry Shrimp", 16.50m),
            new FoodProduct("62A", "Curry Squid", 16.50m),
            new FoodProduct("63", "Curry Beef", 15.50m),
            new FoodProduct("64", "Curry Chicken", 14.50m),
            new FoodProduct("65", "Kung Pao Beef", 15.50m),
            new FoodProduct("66", "Kung Pao Chicken", 14.50m),
            new FoodProduct("66A", "Sesame Chicken", 14.50m),
            new FoodProduct("67", "Salt & Pepper Chicken", 14.50m),
            new FoodProduct("67A", "Salt & Pepper Fish Fillet", 14.50m),
            new FoodProduct("67B", "Salt & Pepper Squid", 16.50m),
            new FoodProduct("68", "Salt & Pepper Shrimp", 16.50m),
            new FoodProduct("68A", "Kung Pao Shrimp", 16.50m),
            new FoodProduct("69", "Salt & Pepper Tofu", 13.50m),
            new FoodProduct("70", "Kung Pao Tofu", 13.50m),
            new FoodProduct("71", "Tofu with Beef & Mixed Greens", 14.50m),
            new FoodProduct("72", "Beef in Satay Sauce", 15.50m),
            new FoodProduct("73", "Chicken in Satay Sauce", 14.50m),
            new FoodProduct("74", "Pork in Satay Sauce", 13.50m),
            new FoodProduct("75", "Shrimp in Satay Sauce", 16.50m),
            new FoodProduct("76", "Squid in Satay Sauce", 16.50m),
            new FoodProduct("77", "Fish in Satay Sauce", 14.50m)
        };

        public static HashSet<FoodProduct> ComboPlateProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("A", "Combo A", 15.50m),
            new FoodProduct("B", "Combo B", 14.50m),
            new FoodProduct("C", "Combo C", 14.50m),
            new FoodProduct("D", "Combo D", 15.50m),
            new FoodProduct("E", "Combo E", 14.50m),
            new FoodProduct("F", "Combo F", 15.50m)
        };

        public static HashSet<FoodProduct> FamilyDinnerProducts = new HashSet<FoodProduct>()
        {
            new FoodProduct("F2", "For Two", 38.50m),
            new FoodProduct("F3", "For Three", 56.50m),
            new FoodProduct("F4", "For Four", 71.50m),
            new FoodProduct("F5", "For Five", 87.50m),
            new FoodProduct("F6", "For Six", 104.50m)
        };
    }
}
