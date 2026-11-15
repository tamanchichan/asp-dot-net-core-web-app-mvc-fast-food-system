using System.ComponentModel.DataAnnotations;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Enums
{
    // ON SIDE // ON TOP // EXTRA // LESS // ADD // NO // ONLY
    public enum Options
    {
        [Display(Name = "Sauce")]
        Sauce,

        [Display(Name ="Spicy")]
        Spicy,

        [Display(Name = "MSG")]
        Msg,
        [Display(Name = "Sugar")]
        Sugar,
        [Display(Name = "Salt")]
        Salt,
        [Display(Name = "Soy Sauce")]
        SoySauce,

        [Display(Name = "Oil")]
        Oil,

        [Display(Name = "Onions")]
        Onions,
        [Display(Name = "Peppers")]
        Peppers,
        [Display(Name = "Garlic")]
        Garlic,
        [Display(Name = "Green Onions")]
        GreenOnions,
        [Display(Name = "Ginger")]
        Ginger,

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
}
