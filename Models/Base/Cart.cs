using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }

        public SystemUser User { get; set; } = null!;

        public Guid? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhoneNumber { get; set; }

        public string? CustomerAddress { get; set; }

        public HashSet<CartFoodProduct>? CartFoodProducts { get; set; } = new HashSet<CartFoodProduct>();

        public HashSet<CartBeverageProduct>? CartBeverageProducts { get; set; } = new HashSet<CartBeverageProduct>();

        public HashSet<CartSauceProduct>? CartSauceProducts { get; set; } = new HashSet<CartSauceProduct>();

        public int Quantity
        {
            get
            {
                int foodQuantity = CartFoodProducts?.Sum(cfp => cfp.Quantity) ?? 0;
                int beverageQuantity = CartBeverageProducts?.Sum(cbp => cbp.Quantity) ?? 0;
                int sauceQuantity = CartSauceProducts?.Sum(csp => csp.Quantity) ?? 0;

                return foodQuantity + beverageQuantity;
            }
        }

        public decimal? AdditionalCharge { get; set; } = 0m;

        public decimal SubTotalPrice
        {
            get
            {
                return
                    (AdditionalCharge ?? 0) +
                    (CartFoodProducts?.Sum(cfp => cfp.Price) ?? 0) +
                    (CartBeverageProducts?.Sum(cbp => cbp.Price) ?? 0) +
                    (CartSauceProducts?.Sum(csp => csp.Price) ?? 0);
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
            Guid? customerId,
            Customer? customer,
            string? customerName,
            string? customerPhoneNumber,
            string? customerAddress,
            HashSet<CartFoodProduct>? cartFoodProducts,
            HashSet<CartBeverageProduct>? cartBeverageProducts,
            HashSet<CartSauceProduct>? cartSauceProducts
        )
        {
            CustomerId = customerId;
            Customer = customer;
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerAddress = customerAddress;
            CartFoodProducts = cartFoodProducts;
            CartBeverageProducts = cartBeverageProducts;
            CartSauceProducts = cartSauceProducts;
        }
    }
}
