using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts
{
    public class OrderFoodProduct : OrderProduct
    {
        private FoodOption? _option;

        public FoodOption? Option
        {
            get
            {
                return Product.HasOptions ? _option : null;
            }
            set
            {
                _option = Product.HasOptions ? value : null;
            }
        }

        private FoodSize? _size;

        public FoodSize? Size { get => Product.HasOptions ? _size : null; set => _size = Product.HasOptions ? value : null; }

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
            _option = option;
            _size = size;
        }
    }
}
