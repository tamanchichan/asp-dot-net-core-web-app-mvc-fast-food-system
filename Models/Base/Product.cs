using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base
{
    public abstract class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [MinLength(1, ErrorMessage = "'Code' must be at least 1 character long.")]
        public string Code { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "'Name' must be at least 1 character long.")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "'Price' must be a non-negative value.")]
        public decimal Price { get; set; }

        [Required]
        public ProductCategory Category { get; set; } // Appetizer, MixedGreens, Noodles, etc.

        public bool HasOptions { get; set; } = false;

        public Product() { }

        public Product(string code, string name, decimal price, bool hasOptions = false)
        {
            Code = code;
            Name = name;
            Price = price;
        }

        public Product
        (
            string code,
            string name,
            string? description,
            decimal price,
            ProductCategory category,
            bool hasOptions = false
        )
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Description = description; // Nullable
            Price = price;
            Category = category;
        }
    }
}
