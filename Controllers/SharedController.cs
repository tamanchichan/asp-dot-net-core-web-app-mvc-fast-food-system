using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    public class SharedController : Controller
    {
        private readonly FastFoodSystemDbContext _context;

        private readonly UserManager<SystemUser> _userManager;

        public SharedController(FastFoodSystemDbContext context, UserManager<SystemUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return NotFound();
        }

        //public Array GetEnum(Product product)
        //{
        //    if (product is FoodProduct && product.HasOptions)
        //    {
        //        return Enum.GetValues<FoodOption>();
        //    }
        //    else if (product is BeverageOption && product.HasOptions)
        //    {
        //        return Enum.GetValues<BeverageOption>();
        //    }
        //    else if (product is SauceOption && product.HasOptions)
        //    {
        //        return Enum.GetValues<SauceOption>();
        //    }
            
        //    return Array.Empty<object>();
        //}

        private static FoodOption GetFoodOption(char c)
        {
            foreach (FoodOption foodOption in Enum.GetValues<FoodOption>())
            {
                if (foodOption.ToString()[0] == c)
                {
                    return foodOption;
                }
            }

            return FoodOption.Chicken;
        }

        [HttpPost]
        public IActionResult AddProductToCart(string input) // 'productOption' only works for FoodProduct ATM
        {
            string returnUrl = Request.Headers["Referer"].ToString();
            string code;
            int quantity;

            if (input.Contains("*") || input.Contains("x"))
            {
                string[] parts = input.Split(new char[] { '*', 'x' });
                code = parts[0];
                quantity = int.Parse(parts[1]);
            }
            else
            {
                code = input;
                quantity = 1;
            }

            int lastIndex = code.Length - 1;
            string productCode = code;
            char? productOption = null;

            Cart cart = _context.Carts.FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            if (cart == null)
            {
                cart = new Cart()
                {
                    UserId = _userManager.GetUserId(User)
                };
            }

            Product product = _context.Products.FirstOrDefault(p => p.Code == productCode);

            if (product == null)
            {
                productCode = code.Substring(0, lastIndex);
                productOption = code[lastIndex];

                product = _context.Products.FirstOrDefault(p => p.Code == productCode);

                if (product == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid code. Please double-check the code entered");

                    return Redirect(returnUrl);
                }
            }

            CartProduct cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.ProductId == product.Id);

            if (cartProduct == null)
            {
                if (product is BeverageProduct)
                {
                    cartProduct = new CartBeverageProduct()
                    {
                        //BeverageOption = 
                    };
                }
                else if (product is FoodProduct)
                {
                    if (product.Code == "25")
                    {
                        cartProduct = new CartFoodProduct()
                        {
                            //FoodSize = 
                        };
                    }
                    else
                    {
                        cartProduct = new CartFoodProduct()
                        {
                            FoodOption = productOption.HasValue ? GetFoodOption(productOption.Value) : null,
                        };
                    }
                }
                else if (product is SauceProduct)
                {
                    cartProduct = new CartSauceProduct()
                    {
                        //SauceOption =
                    };
                }

                if (cartProduct == null)
                {
                    ModelState.AddModelError(string.Empty, "Error creating a 'cartProduct'.");
                    return Redirect(returnUrl);
                }

                cartProduct.CartId = cart.Id;
                cartProduct.Cart = cart;
                cartProduct.ProductId = product.Id;
                cartProduct.Product = product;
                cartProduct.Quantity = (int)quantity;

                _context.CartProducts.Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity += (int)quantity;

                _context.CartProducts.Update(cartProduct);
            }

            _context.SaveChangesAsync();

            return Redirect(returnUrl);
        }
    }
}
