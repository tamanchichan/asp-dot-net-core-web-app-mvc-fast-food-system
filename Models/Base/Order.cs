using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public class Order
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Number { get; set; }

        public HashSet<OrderProduct> OrderProducts { get; set; } = new HashSet<OrderProduct>();

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

        //public OrderStatus? Status { get; set; }

        public Order() { }

        public Order
        (
            int number,
            HashSet<OrderProduct> orderProducts,
            Guid? customerId,
            Customer? customer,
            string? customerName,
            string? customerPhoneNumber,
            string? customerAddress,
            OrderType type,
            string? observations,
            DateTime readyTime
        )
        {
            Number = number;
            OrderProducts = orderProducts;
            CustomerId = customerId;
            Customer = customer;
            CustomerName = customerName;
            CustomerPhoneNumber = customerPhoneNumber;
            CustomerAddress = customerAddress;
            Type = type;
            Observations = observations;
            ReadyTime = readyTime;
        }
    }
}
