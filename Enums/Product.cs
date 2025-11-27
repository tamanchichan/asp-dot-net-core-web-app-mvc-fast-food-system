using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Enums
{
    public enum ProductCategory
    {
        [Display(Name = "Appetizer")]
        Appetizer,

        [Display(Name = "Mixed Greens")]
        MixedGreens,

        [Display(Name = "Noodles")]
        Noodles,

        [Display(Name = "Fried Rice")]
        FriedRice,

        [Display(Name = "Chop Suey")]
        ChopSuey,

        [Display(Name = "Egg Foo Yung")]
        EggFooYung,

        [Display(Name = "Sea Food")]
        SeaFood,

        [Display(Name = "Chicken")]
        Chicken,

        [Display(Name = "Beef & Pork")]
        BeefAndPork,

        [Display(Name = "Hot & Spicy")]
        HotAndSpicy,

        [Display(Name = "Combination Plates")]
        ComboPlates,

        [Display(Name = "Family Dinners")]
        FamilyDinners,

        [Display(Name = "Extras")]
        Extras
    }

    public enum FoodOption
    {
        Beef,
        Chicken,
        Fish,
        Mushroom,
        Pork,
        Shrimp,
        Tofu
    }

    public enum FoodSize
    {
        Small,
        Medium,
        Large
    }

    public enum BeverageOption
    {
        [Display(Name = "Coca-Cola")]
        Coke,

        [Display(Name = "Coca-Cola Zero")]
        CokeZero,

        [Display(Name = "Ginger Ale")]
        GingerAle,

        [Display(Name = "Iced Tea")]
        IcedTea,

        [Display(Name = "Pepsi")]
        Pepsi,

        [Display(Name = "Pepsi Zero")]
        PepsiZero,

        [Display(Name = "Root Bear")]
        RootBear,

        [Display(Name = "7 Up")]
        SevenUp,

        [Display(Name = "Sprite")]
        Sprite,

        [Display(Name = "Green Tea")]
        GreenTea,

        [Display(Name = "Jasmine Tea")]
        JasmineTea,

        [Display(Name = "Bottled Water")]
        BottledWater

    }

    public enum SauceOption
    {
        [Display(Name = "Soy Sauce")]
        SoySauce,

        [Display(Name = "Plum Sauce")]
        PlumSauce,

        [Display(Name = "Hot Sauce")]
        HotSauce,

        [Display(Name = "Sweet & Sour Sauce")]
        SweetAndSourSauce,

        [Display(Name = "Honey Lemon Sauce")]
        HoneyLemonSauce,

        [Display(Name = "Honey Garlic Sauce")]
        HoneyGarlicSauce,

        [Display(Name = "Hot Honey Garlic Sauce")]
        HotHoneyGarlicSauce,

        [Display(Name = "Black Bean Garlic Sauce")]
        BlackBeanGarlicSauce,

        [Display(Name = "Curry Sauce")]
        CurrySauce
    }
}
