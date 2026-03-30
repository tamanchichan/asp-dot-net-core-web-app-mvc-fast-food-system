using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Interface;
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
