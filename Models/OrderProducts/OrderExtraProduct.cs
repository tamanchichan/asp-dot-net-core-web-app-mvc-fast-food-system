using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts
{
    public class OrderExtraProduct : OrderProduct
    {
        public OrderExtraProduct() { }

        public OrderExtraProduct
        (
            Guid orderId,
            Order order,
            Guid productId,
            ExtraProduct product,
            int quantity,
            string? instruction,
            List<string>? instructions,
            decimal? additionalPrice,
            decimal price) : base(orderId, order, productId, product, quantity, instruction, instructions, additionalPrice, price)
        {

        }
    }
}
