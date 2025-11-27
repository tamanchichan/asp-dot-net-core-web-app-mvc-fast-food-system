using System.ComponentModel.DataAnnotations.Schema;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public abstract class CartProduct
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CartId { get; set; }

        public Cart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = null!;

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

        public CartProduct() { }

        public CartProduct
        (Guid cartId,
            Cart cart,
            Guid productId,
            Product product,
            int quantity,
            string? instructions,
            decimal? additionalPrice
        )
        {
            CartId = cartId;
            Cart = cart;
            ProductId = productId;
            Product = product;
            Quantity = quantity;
            Instructions = instructions;
            AdditionalPrice = additionalPrice ?? 0m;
        }
    }
}
