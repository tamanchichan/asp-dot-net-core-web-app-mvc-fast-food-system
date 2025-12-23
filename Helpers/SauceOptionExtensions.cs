using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public class SauceOptionExtensions
    {
        public static string GetSauceAbbreviation(SauceOption? sauce)
        {
            if (sauce == null)
            {
                return "";
            }

            switch (sauce)
            {
                case SauceOption.SoySauce:
                    return "S.S.";
                case SauceOption.PlumSauce:
                    return "P.S.";
                case SauceOption.HotSauce:
                    return "H.S.";
                case SauceOption.SweetAndSourSauce:
                    return "S.S.S.";
                case SauceOption.HoneyLemonSauce:
                    return "H.L.S.";
                case SauceOption.HoneyGarlicSauce:
                    return "H.G.S.";
                case SauceOption.HotHoneyGarlicSauce:
                    return "H.H.G.S.";
                case SauceOption.BlackBeanGarlicSauce:
                    return "B.B.G.S.";
                case SauceOption.CurrySauce:
                    return "C.S.";
                default:
                    return "";
            }
        }
    }
}
