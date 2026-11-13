using System.ComponentModel.DataAnnotations;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Enums
{
    // ON SIDE // ON TOP // EXTRA // LESS // NO // ONLY
    public enum Options
    {
        [Display(Name = "汁")]
        Sauce,

        [Display(Name ="辣")]
        Spicy,

        [Display(Name = "MSG")]
        Msg,
        [Display(Name = "糖")]
        Sugar,
        [Display(Name = "盐")]
        Salt,
        [Display(Name = "油")]
        Oil,

        [Display(Name = "洋葱")]
        Onions,
        [Display(Name = "椒")]
        Peppers,
        [Display(Name = "蒜")]
        Garlic,
        [Display(Name = "葱")]
        GreenOnions,
        [Display(Name = "姜")]
        Ginger,

        [Display(Name = "牛肉")]
        Beef,
        [Display(Name = "鸡肉")]
        Chicken,
        [Display(Name = "鱼")]
        Fish,
        [Display(Name = "蘑菇")]
        Mushroom,
        [Display(Name = "猪肉")]
        Pork,
        [Display(Name = "虾")]
        Shrimp,
        [Display(Name = "豆腐")]
        Tofu,
        [Display(Name = "菜")]
        Vegetables,
    }
}
