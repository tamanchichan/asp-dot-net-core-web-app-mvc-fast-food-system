namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public static class Translation
    {
        public static readonly Dictionary<string, string> Translations = new Dictionary<string, string>()
        {
            // instructions
            { "ADD", "加" },
            { "EXTRA", "额外" },
            { "VERY", "很" },
            { "LESS", "少" },
            { "ONLY", "只有" },
            { "NO", "不要" },
            { "NOT", "不要" },
            { "ON TOP", "在上面" },
            { "ON SIDE", "另放" },

            { "SAUCE", "汁" },

            { "SPICY", "辣" },

            // seasoning
            { "MSG", "MSG" },
            { "SUGAR", "糖" },
            { "SALT", "盐" },

            { "OIL", "油" },

            // ingredients
            { "GARLIC", "蒜" },
            { "GINGER", "姜" },
            { "GREEN ONIONS", "葱" },
            { "ONIONS", "洋葱" },
            { "PEPPERS", "椒" },

            // proteins
            { "BEEF", "牛肉" },
            { "CHICKEN", "鸡肉" },
            { "FISH", "鱼" },
            { "MUSHROOM", "蘑菇" },
            { "PORK", "猪肉" },
            { "SHRIMP", "虾" },
            { "TOFU", "豆腐" },
            { "VEGETABLES", "菜" },
        };

        public static readonly Dictionary<string, string> Options = new Dictionary<string, string>
        {
            { "Sauce", "汁" },

            { "Spicy", "辣" },
            
            { "Msg", "MSG" },
            { "Sugar", "糖" },
            { "Salt", "盐" },
            { "SoySauce", "酱油" },
            
            { "Oil", "油" },
            
            { "Garlic", "蒜" },
            { "Ginger", "姜" },
            { "GreenOnions", "葱" },
            { "Onions", "洋葱" },
            { "Peppers", "椒" },

            { "Beef", "牛肉" },
            { "Chicken", "鸡肉" },
            { "Fish", "鱼" },
            { "Mushroom", "蘑菇" },
            { "Pork", "猪肉" },
            { "Shrimp", "虾" },
            { "Tofu", "豆腐" },
            { "Vegetables", "菜" }
        };
    }
}
