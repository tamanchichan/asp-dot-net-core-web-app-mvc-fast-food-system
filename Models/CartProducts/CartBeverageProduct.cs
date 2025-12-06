using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts
{
    public class CartBeverageProduct : CartProduct
    {
        private BeverageOption? _beverageOption;

        public BeverageOption? BeverageOption // Coke, CokeZero, GingerAle, etc.
        {
            // Using ternaty operator to simplify the code
            get
            {
                if (Product != null && Product.HasOptions)
                {
                    return _beverageOption;
                }
                else
                {
                    return null;
                }
            }
            // Using ternary operator to simplify the code
            set
            {
                _beverageOption = (Product != null && Product.HasOptions) ? value : null;
            }
        }

        public CartBeverageProduct() { }

        // Polymorphic constructor to initialize base class properties
        public CartBeverageProduct
        (
            Guid cartId,
            Cart cart,
            Guid productId,
            BeverageProduct product,
            BeverageOption? option,
            int quantity,
            string? instructions,
            decimal? additionalPrice) : base(cartId, cart, productId, product, quantity, instructions, additionalPrice
        )
        {
            _beverageOption = option;
        }
    }
}
