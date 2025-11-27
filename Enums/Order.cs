using System.ComponentModel.DataAnnotations;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Enums
{
    public enum OrderType
    {
        [Display(Name = "Dine-In")]
        DineIn,

        [Display(Name = "Take Away")]
        TakeAway,

        [Display(Name = "Delivery")]
        Delivery
    }

    public enum OrderStatus
    {
        [Display(Name = "Pending")]
        Pending,

        [Display(Name = "In Preparation")]
        InPreparation,

        [Display(Name = "Ready")]
        Ready,

        [Display(Name = "Completed")]
        Completed,

        [Display(Name = "Cancelled")]
        Cancelled
    }
}
