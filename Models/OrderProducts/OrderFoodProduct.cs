using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts
{
    public class OrderFoodProduct : OrderProduct
    {
        private FoodOption? _foodOption;

        public FoodOption? FoodOption
        {
            get
            {
                return Product.HasOptions ? _foodOption : null;
            }
            set
            {
                _foodOption = Product.HasOptions ? value : null;
            }
        }

        private FoodSize? _foodSize;

        public FoodSize? FoodSize { get => Product.HasOptions ? _foodSize : null; set => _foodSize = Product.HasOptions ? value : null; }

        public OrderFoodProduct() { }

        public OrderFoodProduct
        (
            Guid orderId,
            Order order,
            Guid productId,
            FoodProduct product,
            FoodOption? option,
            FoodSize? size,
            int quantity,
            string? instructions,
            decimal? additionalPrice
        ) : base(orderId, order, productId, product, quantity, instructions, additionalPrice)
        {
            _foodOption = option;
            _foodSize = size;
        }
    }
}
