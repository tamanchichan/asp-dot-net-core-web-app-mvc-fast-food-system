namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public static class TranslationExtensions
    {
        public static string GetTranslation(this string value)
        {
            value = value.ToUpper().Trim();

            foreach (KeyValuePair<string, string> translation in Translation.Translations)
            {
                value = value.Replace(
                    translation.Key,
                    translation.Value,
                    StringComparison.OrdinalIgnoreCase
                );
            }

            return value;
        }
    }
}
