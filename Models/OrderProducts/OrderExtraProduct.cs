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
            string? instructions,
            decimal? additionalPrice) : base(orderId, order, productId, product, quantity, instructions, additionalPrice
        )
        {

        }
    }
}
