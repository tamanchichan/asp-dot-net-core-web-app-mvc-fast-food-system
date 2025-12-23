using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts
{
    public class CartFoodProduct : CartProduct
    {
        private FoodOption? _foodOption;
        public FoodOption? FoodOption  // Beef, Chicken, Fish, Mushroom, etc.
        {
            get
            {
                return _foodOption;
            }
            set
            {
                _foodOption = value;
            }
        }

        private FoodSize? _foodSize;

        public FoodSize? FoodSize // Small, Medium, Large
        {
            get => _foodSize;
            set => _foodSize = value;
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
            _foodOption = option;
            _foodSize = size;
        }
    }
}
