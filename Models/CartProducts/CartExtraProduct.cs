using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts
{
    public class CartExtraProduct : CartProduct
    {
        public CartExtraProduct() { }

        // Polymorphic constructor to initialize base class properties
        public CartExtraProduct
        (
            Guid cartId,
            Cart cart,
            Guid productId,
            ExtraProduct product,
            int quantity,
            string? instructions,
            decimal? additionalPrice) : base(cartId, cart, productId, product, quantity, instructions, additionalPrice
        )
        {

        }
    }
}
