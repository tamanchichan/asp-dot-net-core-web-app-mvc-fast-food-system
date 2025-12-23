using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts
{
    public class CartSauceProduct : CartProduct
    {
        public SauceOption? _sauceOption;

        public SauceOption? SauceOption
        {
            get
            {
                return _sauceOption;
            }
            set
            {
                _sauceOption = value;
            }
        }

        public CartSauceProduct() { }

        // Polymorphic constructor to initialize base class properties
        public CartSauceProduct
        (
            Guid cartId,
            Cart cart,
            Guid productId,
            SauceProduct product,
            SauceOption? option,
            int quantity,
            string? instructions,
            decimal? aditionalPrice) : base(cartId, cart, productId, product, quantity, instructions, aditionalPrice
        )
        {
            _sauceOption = option;
        }
    }
}
