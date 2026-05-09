using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Interface;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public abstract class CartProduct : IProductItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid CartId { get; set; }

        public Cart Cart { get; set; } = null!;

        public Guid? ProductId { get; set; }

        public Product? Product { get; set; }

        public string Code
        {
            get
            {
                return Product?.Code ?? string.Empty;
            }
            set
            {

            }
        }

        public string Name
        {
            get
            {
                return Product?.Name ?? string.Empty;
            }
            set
            {

            }
        }

        public int Quantity { get; set; } = 1;

        public string? Instructions { get; set; }

        public bool IsFreeItem { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AdditionalPrice { get; set; } = 0m;

        public decimal Price
        {
            get
            {
                return IsFreeItem ? 0m : Product?.Price ?? 0m;
            }
            set
            {
                
            }
        }

        public decimal TotalPrice
        {
            get
            {
                return (AdditionalPrice + Price) * Quantity;
            }
        }

        public ProductCategory? Category
        {
            get
            {
                return Product?.Category;
            }
            set
            {
            }
        }

        public bool? HasOptions
        {
            get
            {
                return Product?.HasOptions;
            }
            set
            {
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
