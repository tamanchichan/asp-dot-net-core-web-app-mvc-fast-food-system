using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Interface
{
    public interface IProductItem
    {
        Guid Id { get; }

        Guid? ProductId { get; }

        Product? Product { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        int Quantity { get; set; }

        string Instructions { get; }

        decimal AdditionalPrice { get; }

        decimal Price { get; }

        decimal TotalPrice { get; }

        ProductCategory? Category { get; set; }

        bool? HasOptions { get; set; }
    }
}
