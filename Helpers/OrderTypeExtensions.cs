using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Helpers
{
    public class OrderTypeExtensions
    {
        public static string GetName(OrderType? orderType)
        {
            if (orderType == null)
            {
                return "Take-Out";
            }
            switch (orderType)
            {
                case OrderType.Delivery:
                    return "Delivery";
                case OrderType.DineIn:
                    return "Dine-In";
                case OrderType.TakeAway:
                    return "Take-Out";
                default:
                    return "Take-Out";

            }
        }

        public static string GetChineseName(OrderType? orderType)
        {
            if (orderType == null)
            {
                return "取出";
            }
            switch (orderType)
            {
                case OrderType.Delivery:
                    return "送货";
                case OrderType.DineIn:
                    return "堂食";
                case OrderType.TakeAway:
                    return "取出";
                default:
                    return "取出";

            }
        }
    }
}
