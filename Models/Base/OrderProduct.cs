using System.ComponentModel.DataAnnotations.Schema;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public abstract class OrderProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public int Quantity { get; set; } = 1;

        public string? Instructions { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalPrice { get; set; } = 0m;

        public decimal Price
        {
            get
            {
                return (AdditionalPrice + Product.Price);
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return Price * Quantity;
            }
        }

        public OrderProduct() { }

        public OrderProduct
        (
            Guid orderId,
            Order order,
            Guid productId,
            Product product,
            int quantity,
            string?
            instructions,
            decimal? additionalPrice
        )
        {
            OrderId = orderId;
            Order = order;
            ProductId = productId;
            Product = product;
            Quantity = quantity;
            Instructions = instructions;
            AdditionalPrice = additionalPrice ?? 0;
        }
    }
}
