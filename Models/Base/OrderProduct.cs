using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public abstract class OrderProduct : IProductItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid OrderId { get; set; }

        public Order Order { get; set; }

        public Guid? ProductId { get; set; }

        public Product? Product { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; } = 1;

        public string? Instructions { get; set; }

        public List<string>? Options { get; set; } = new List<string>();

        public bool IsFreeItem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalPrice { get; set; } = 0m;

        private decimal _price;

        public decimal Price
        {
            get
            {
                //return IsFreeItem ? 0m : Product?.Price ?? 0m;
                return IsFreeItem ? 0m : _price;
            }
            set
            {
                _price = value;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return (AdditionalPrice + Price) * Quantity;
            }
        }

        public ProductCategory? Category { get; set; }

        public bool? HasOptions { get; set; }

        public OrderProduct() { }

        public OrderProduct
        (
            Guid orderId,
            Order order,
            Guid productId,
            Product product,
            int quantity,
            string? instruction,
            List<string>? options,
            decimal? additionalPrice,
            decimal price
        )
        {
            OrderId = orderId;
            Order = order;
            ProductId = productId;
            Product = product;
            Quantity = quantity;
            Instructions = instruction;
            Options = options;
            AdditionalPrice = additionalPrice ?? 0;
            Price = price;
        }
    }
}
