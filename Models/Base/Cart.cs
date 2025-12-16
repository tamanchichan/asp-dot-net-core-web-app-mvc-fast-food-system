using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string UserId { get; set; }

        public SystemUser User { get; set; } = null!;

        public HashSet<CartProduct>? CartProducts { get; set; } = new HashSet<CartProduct>();

        public int Quantity
        {
            get
            {
                return CartProducts?.Sum(cp => cp.Quantity) ?? 0;
            }
        }

        public decimal? AdditionalCharge { get; set; } = 0m;

        public decimal SubTotalPrice
        {
            get
            {
                return
                    (AdditionalCharge ?? 0) +
                    (CartProducts?.Sum(cp => cp.Price) ?? 0);
            }
        }

        public decimal Gst
        {
            get
            {
                return SubTotalPrice * 0.05m;
            }
        }

        public decimal Pst
        {
            get
            {
                return SubTotalPrice * 0.07m;
            }
        }

        public decimal? DeliveryFee { get; set; } = 0m;

        public decimal? Discount { get; set; } = 0m;

        public decimal TotalPrice
        {
            get
            {
                return ((SubTotalPrice + Gst + Pst) + DeliveryFee ?? 0) - (Discount ?? 0);
            }
        }

        public Cart() { }

        public Cart
        (
            string userId,
            SystemUser user,
            Guid? customerId,
            Customer? customer,
            string? customerName,
            string? customerPhoneNumber,
            string? customerAddress,
            HashSet<CartProduct> cartProducts
        )
        {
            UserId = userId;
            User = user;
            CartProducts = cartProducts;
        }
    }
}
