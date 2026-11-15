namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public static class TranslationExtensions
    {
        public static string GetTextAreaTranslation(this string observation)
        {
            observation = observation.ToUpper().Trim();

            foreach (KeyValuePair<string, string> translation in Translation.Translations)
            {
                observation = observation.Replace(
                    translation.Key,
                    translation.Value,
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return observation;
        }

        public static string GetOptionsTranslation(this string option)
        {
            string[] optionParts = option.Split(" ");
            string[] prefixes = { "EXTRA", "LESS", "ADD", "NO", "ONLY" };
            string[] sufixes = { "ON SIDE", "ON TOP" };

            string formattedOption = option.ToUpper().Trim();

            foreach (KeyValuePair<string, string> translation in Translation.Options)
            {
                formattedOption = formattedOption.Replace(
                    translation.Key.ToUpper(),
                    translation.Value,
                    StringComparison.OrdinalIgnoreCase
                );
            }

            // Uses Display(Name = "")
            /*
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
            */

            foreach (string prefix in prefixes)
            {
                if (prefix == "EXTRA")
                {
                    formattedOption = formattedOption.Replace(prefix, "额外");
                }
                else if (prefix == "LESS")
                {
                    formattedOption = formattedOption.Replace(prefix, "少");
                }
                else if (prefix == "ADD")
                {
                    formattedOption = formattedOption.Replace(prefix, "加");
                }
                else if (prefix == "NO")
                {
                    formattedOption = formattedOption.Replace(prefix, "不要");
                }
                else if (prefix == "ONLY")
                {
                    formattedOption = formattedOption.Replace(prefix, "只有");
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
