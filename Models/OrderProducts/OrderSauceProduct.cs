using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts
{
    public class OrderSauceProduct : OrderProduct
    {
        public SauceOption? _sauceOption;

        public SauceOption? SauceOption { get => Product.HasOptions ? _sauceOption : null; set => _sauceOption = Product.HasOptions ? value : null; }

        public OrderSauceProduct() { }

        // Polymorphic constructor to initialize base class properties
        public OrderSauceProduct
        (
            Guid orderId,
            Order order,
            Guid productId,
            SauceProduct product,
            SauceOption? option,
            int quantity,
            string? instructions,
            decimal? aditionalPrice) : base(orderId, order, productId, product, quantity, instructions, aditionalPrice
        )
        {
            _sauceOption = option;
        }
    }
}
