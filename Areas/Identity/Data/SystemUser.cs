using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using Microsoft.AspNetCore.Identity;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data
{
    public class SystemUser : IdentityUser
    {
        // Additional properties can be added here in the future
        public Cart? Cart { get; set; }

        public HashSet<Order>? Orders { get; set; } = new HashSet<Order>();
    }
}
