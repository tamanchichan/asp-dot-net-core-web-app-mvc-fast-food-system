using System.ComponentModel.DataAnnotations;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Enums
{
    // ON SIDE // EXTRA // LESS // ADD // REMOVE // ONLY // SAUCE // SIZE
    public enum Options
    {
        // Seasonings
        [Display(Name = "Black Pepper")]
        BlackPepper,
        [Display(Name = "Msg")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Salt & Pepper")]
        SaltAndPepper,
        [Display(Name = "Sauce")]
        Sauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "Spicy")]
        Spicy,

        // Ingredients
        [Display(Name = "Almonds")]
        Almonds,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Ginger")]
        Ginger,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Sesame Seed")]
        SesameSeed,

        // Proteins
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,

        // Sauces
        [Display(Name = "BGS")]
        Bgs,
        [Display(Name = "Curry Sauce")]
        CurrySauce,
        [Display(Name = "HGS")]
        Hgs,
        [Display(Name = "HHGS")]
        Hhgs,
        [Display(Name = "HLS")]
        Hls,
        [Display(Name = "Kung Pao Sauce")]
        KungPaoSauce,
        [Display(Name = "Satay Sauce")]
        SataySauce,
        [Display(Name = "Szechuan Sauce")]
        SzechuanSauce,
        [Display(Name = "Sesame Sauce")]
        SesameSauce,
        [Display(Name = "SSS")]
        Sss,

        // Sizes
        [Display(Name = "Small")]
        Small,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "Large")]
        Large
    }

    public enum OnSideOptions
    {
        // Seasonings
        [Display(Name = "Black Pepper")]
        BlackPepper,
        [Display(Name = "Msg")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Salt & Pepper")]
        SaltAndPepper,
        [Display(Name = "Sauce")]
        Sauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "Spicy")]
        Spicy,

        // Ingredients
        [Display(Name = "Almonds")]
        Almonds,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Ginger")]
        Ginger,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Sesame Seed")]
        SesameSeed,

        // Proteins
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,
    }

    public enum ExtraOptions
    {
        // Seasonings
        [Display(Name = "Black Pepper")]
        BlackPepper,
        [Display(Name = "Msg")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Salt & Pepper")]
        SaltAndPepper,
        [Display(Name = "Sauce")]
        Sauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "Spicy")]
        Spicy,

        // Ingredients
        [Display(Name = "Almonds")]
        Almonds,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Ginger")]
        Ginger,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Sesame Seed")]
        SesameSeed,

        // Proteins
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,
    }

    public enum LessOptions
    {
        // Seasonings
        [Display(Name = "Black Pepper")]
        BlackPepper,
        [Display(Name = "Msg")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Salt & Pepper")]
        SaltAndPepper,
        [Display(Name = "Sauce")]
        Sauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "Spicy")]
        Spicy,

        // Ingredients
        [Display(Name = "Almonds")]
        Almonds,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Ginger")]
        Ginger,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Sesame Seed")]
        SesameSeed,

        // Proteins
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,
    }

    public enum AddOptions
    {
        // Seasonings
        [Display(Name = "Black Pepper")]
        BlackPepper,
        [Display(Name = "Msg")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Salt & Pepper")]
        SaltAndPepper,
        [Display(Name = "Sauce")]
        Sauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "Spicy")]
        Spicy,

        // Ingredients
        [Display(Name = "Almonds")]
        Almonds,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Ginger")]
        Ginger,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Sesame Seed")]
        SesameSeed,

        // Proteins
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,
    }

    public enum RemoveOptions
    {
        // Seasonings
        [Display(Name = "Black Pepper")]
        BlackPepper,
        [Display(Name = "Msg")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Salt & Pepper")]
        SaltAndPepper,
        [Display(Name = "Sauce")]
        Sauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "Spicy")]
        Spicy,

        // Ingredients
        [Display(Name = "Almonds")]
        Almonds,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Ginger")]
        Ginger,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Sesame Seed")]
        SesameSeed,

        // Proteins
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,
    }

    public enum OnlyOptions
    {
        [Display(Name = "Beef")]
        Beef,
        [Display(Name = "Chicken")]
        Chicken,
        [Display(Name = "Fish")]
        Fish,
        [Display(Name = "Mushroom")]
        Mushroom,
        [Display(Name = "Pork")]
        Pork,
        [Display(Name = "Shrimp")]
        Shrimp,
        [Display(Name = "Tofu")]
        Tofu,
        [Display(Name = "Vegetables")]
        Vegetables,
    }

    public enum SauceOptions
    {
        [Display(Name = "BGS")]
        Bgs,
        [Display(Name = "Curry Sauce")]
        CurrySauce,
        [Display(Name = "HGS")]
        Hgs,
        [Display(Name = "HHGS")]
        Hhgs,
        [Display(Name = "HLS")]
        Hls,
        [Display(Name = "Kung Pao Sauce")]
        KungPaoSauce,
        [Display(Name = "Satay Sauce")]
        SataySauce,
        [Display(Name = "Szechuan Sauce")]
        SzechuanSauce,
        [Display(Name = "Sesame Sauce")]
        SesameSauce,
        [Display(Name = "Soy Sauce")]
        SoySauce,
        [Display(Name = "SSS")]
        Sss,
    }

    public enum SizeOptions
    {
        [Display(Name = "Small")]
        Small,
        [Display(Name = "Medium")]
        Medium,
        [Display(Name = "Large")]
        Large
    }
}
