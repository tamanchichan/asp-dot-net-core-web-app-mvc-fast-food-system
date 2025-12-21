using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Number { get; set; }

        public HashSet<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();

        public int Quantity
        {
            get
            {
                return OrderProducts?.Sum(op => op.Quantity) ?? 0;
            }
        }

        public string UserId { get; set; }

        public SystemUser User { get; set; }

        public Guid? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhoneNumber { get; set; }

        public string? CustomerAddress { get; set; }

        public OrderType Type { get; set; }

        public string? Observations { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ReadyTime { get; set; }

        public decimal? AdditionalCharge { get; set; } = 0m;

        public decimal SubTotalPrice
        {
            get
            {
                return
                    (AdditionalCharge ?? 0) +
                    (OrderProducts?.Sum(op => op.Price * op.Quantity) ?? 0);
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

        //public OrderStatus? Status { get; set; }

        public Order() { }

        public Order
        (
            HashSet<OrderProduct> orderProducts,
            string userId,
            SystemUser user,
            Guid? customerId,
            Customer? customer,
            string? customerName,
            string? customerPhoneNumber,
            string? customerAddress,
            OrderType type,
            string? observations,
            DateTime readyTime,
            decimal? additionalCharge,
            decimal? deliveryFee,
            decimal? discount

        )
        {
            OrderProducts = orderProducts;
            UserId = userId;
            User = user;
            CustomerId = customerId;
            Customer = customer;
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerAddress = customerAddress;
            Type = type;
            Observations = observations;
            ReadyTime = readyTime;
            AdditionalCharge = additionalCharge ?? 0m;
            DeliveryFee = deliveryFee ?? 0m;
            Discount = discount ?? 0m;
        }
    }
}
