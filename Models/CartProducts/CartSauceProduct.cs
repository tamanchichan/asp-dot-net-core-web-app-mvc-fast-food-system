using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts
{
    public class CartSauceProduct : CartProduct
    {
        public SauceOption? _option;

        public SauceOption? Option
        {
            // Using traditional if-else statements for clarity
            get
            {
                if (Product.HasOptions)
                {
                    return _option;
                }
                else
                {
                    return null;
                }
            }
            // Using ternaty operator to simplify the code
            set
            {
                _option = Product.HasOptions ? value : null;
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
            _option = option;
        }
    }
}
