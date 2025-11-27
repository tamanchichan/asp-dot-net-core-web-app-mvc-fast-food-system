using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products
{
    public class BeverageProduct : Product
    {
        public BeverageProduct() { }

        public BeverageProduct(string code, string name, decimal price, bool hasOptions = false) : base(code, name, price, hasOptions) { }
        public BeverageProduct
        (
            string code,
            string name,
            string? description,
            decimal price,
            ProductCategory category,
            bool hasOptions = false
        ) : base(code, name, description, price, category, hasOptions) { }
    }
}
