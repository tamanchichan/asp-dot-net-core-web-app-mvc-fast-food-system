using ESCPOS_NET.Emitters.BaseCommandValues;
using System.Text.RegularExpressions;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public static class TranslationExtensions
    {
        public static string GetTranslation(this string value)
        {
            foreach (KeyValuePair<string, string> translation in Translation.Translations
                .OrderByDescending(t => t.Key.Length))
            {
                value = Regex.Replace(
                    value,
                    $@"\b{Regex.Escape(translation.Key)}\b",
                    translation.Value,
                    RegexOptions.IgnoreCase
                );
            }

            return value.Trim();
        }
    }
}
