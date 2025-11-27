using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts
{
    public class CartFoodProduct : CartProduct
    {
        private FoodOption? _option;
        public FoodOption? Option  // Beef, Chicken, Fish, Mushroom, etc.
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
            set
            {
                if (Product.HasOptions)
                {
                    _option = value;
                }
                else
                {
                    _option = null;
                }
            }
        }

        private FoodSize? _size;

        public FoodSize? Size // Small, Medium, Large
        {
            // Using ternaty operator to simplify the code
            get
            {
                return Product.HasOptions ? _size : null;
            }
            set
            {
                _size = Product.HasOptions ? value : null;
            }
        }

        public CartFoodProduct() { }

        // Polymorphic constructor to initialize base class properties
        public CartFoodProduct
        (
            Guid cartId,
            Cart cart,
            Guid productId,
            FoodProduct product,
            FoodOption? option,
            FoodSize? size,
            int quantity,
            string? instructions,
            decimal? additionalPrice) : base(cartId, cart, productId, product, quantity, instructions, additionalPrice
        )
        {
            _option = option;
            _size = size;
        }
    }
}
