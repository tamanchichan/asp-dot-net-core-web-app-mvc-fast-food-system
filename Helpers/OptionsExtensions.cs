using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public class OptionsExtensions
    {
        public static string GetObservationTranslation(string observation)
        {
            observation = observation.ToUpper().Trim();

            Dictionary<string, string> translations = new Dictionary<string, string>()
            {
                { "ADD", "加" },
                { "EXTRA", "加" },

                { "VERY", "大" },
                
                { "LESS", "少" },
                
                { "ONLY", "只有" },
                
                { "NO", "不要" },
                
                { "ON TOP", "在上面" },
                
                { "ON SIDE", "另放" },

                { "SAUCE", "汁" },
                { "SPICY", "辣" },
                { "MSG", "MSG" },
                { "SUGAR", "糖" },
                { "SALT", "盐" },
                { "OIL", "油" },
                { "ONIONS", "洋葱" },
                { "PEPPERS", "椒" },
                { "GARLIC", "蒜" },
                { "GREEN ONIONS", "葱" },
                { "GINGER", "姜" },
                { "BEEF", "牛肉" },
                { "CHICKEN", "鸡肉" },
                { "FISH", "鱼" },
                { "MUSHROOM", "蘑菇" },
                { "PORK", "猪肉" },
                { "SHRIMP", "虾" },
                { "TOFU", "豆腐" },
                { "VEGETABLES", "菜" },
            };

            foreach (KeyValuePair<string, string> translation in translations)
            {
                observation = observation.Replace(
                    translation.Key,
                    translation.Value,
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return observation;
        }

        public static string GetOptionsDisplayName(string option)
        {
            string[] optionParts = option.Split(" ");
            string[] prefixes = { "EXTRA", "LESS", "ONLY", "NO" };
            string[] sufixes = { "ON SIDE", "ON TOP" };

            string formattedOption = option.ToUpper().Trim();

            foreach (Options enumOption in Enum.GetValues<Options>())
            {
                string enumName = enumOption.ToString().ToUpper();

                if (formattedOption.Contains(enumName))
                {
                    string displayName = enumOption.GetType()
                        .GetMember(enumOption.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()?
                        .GetName() ?? enumName;

                    formattedOption = formattedOption.Replace(enumName, displayName);

                    break;
                }
            }

            foreach (string prefix in prefixes)
            {
                if (prefix == "EXTRA")
                {
                    formattedOption = formattedOption.Replace(prefix, "加");
                }
                else if (prefix == "LESS")
                {
                    formattedOption = formattedOption.Replace(prefix, "少");
                }
                else if (prefix == "ONLY")
                {
                    formattedOption = formattedOption.Replace(prefix, "只有");
                }
                else if (prefix == "NO")
                {
                    formattedOption = formattedOption.Replace(prefix, "不要");
                }
            }

            foreach (string sufix in sufixes)
            {
                if (sufix == "ON SIDE")
                {
                    formattedOption = formattedOption.Replace(sufix, "另放");
                }
                else if (sufix == "ON TOP")
                {
                    formattedOption = formattedOption.Replace(sufix, "在上面");
                }
            }

            return formattedOption;
        }
    }
}
