using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Interface;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts
{
    public class OrderBeverageProduct : OrderProduct
    {
        private BeverageOption? _beverageOption;

        public BeverageOption? BeverageOption
        {
            get
            {
                return _beverageOption;
            }
            set
            {
                _beverageOption = value;
            }
        }

        public FoodOption? FoodOption => null; // Not applicable for beverage products

        public FoodSize? FoodSize => null; // Not applicable for beverage products

        public SauceOption? SauceOption => null; // Not applicable for beverage products

        public OrderBeverageProduct() { }

        public OrderBeverageProduct
        (
            Guid orderId,
            Order order,
            Guid productId,
            BeverageProduct product,
            BeverageOption? option,
            int quantity,
            string? instructions,
            decimal? additionalPrice
        ) : base(orderId, order, productId, product, quantity, instructions, additionalPrice)
        {
            _beverageOption = option;
        }
    }
}
