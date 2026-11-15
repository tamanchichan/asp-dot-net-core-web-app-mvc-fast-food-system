using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
                .GetMember(value.ToString())
                .FirstOrDefault()?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName() ?? value.ToString();
        }
    }
}
