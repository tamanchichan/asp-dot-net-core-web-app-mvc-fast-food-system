using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        private static SauceOption? GetSauceOption(string option)
        {
            option = option.ToUpperInvariant();

            switch (option)
            {
                case "S.S.":
                    return SauceOption.SoySauce;
                case "P.S.":
                    return SauceOption.PlumSauce;
                case "H.S.":
                    return SauceOption.HotSauce;
                case "S.S.S.":
                    return SauceOption.SweetAndSourSauce;
                case "H.L.S.":
                    return SauceOption.HoneyLemonSauce;
                case "H.G.S.":
                    return SauceOption.HoneyGarlicSauce;
                case "H.H.G.S.":
                    return SauceOption.HotHoneyGarlicSauce;
                case "B.B.G.S.":
                    return SauceOption.BlackBeanGarlicSauce;
                case "C.S.":
                    return SauceOption.CurrySauce;
                default:
                    return null;
            }
        }

        private static FoodOption GetFoodOption(char c)
        {
            c = char.ToUpperInvariant(c);

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
        public IActionResult AddProductToCart(string input)
        {
            input = input.ToUpperInvariant();

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

            Cart cart = _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            if (cart == null)
            {
                ModelState.AddModelError(string.Empty, "No active cart found for the user.");
            }

            Product product = _context.Products.FirstOrDefault(p => p.Code == productCode);

            CartProduct cartProduct = null;

            if (product == null)
            {
                productCode = code.Substring(0, lastIndex);
                productOption = code[lastIndex];

                product = _context.Products.FirstOrDefault(p => p.Code == productCode);

                if (product == null)
                {
                    // if productCode not found, assume it is sauce product
                    product = _context.Products.FirstOrDefault(p => p.Code == "Sauce");

                    if (product == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid code. Please double-check the code entered");

                        return Redirect(returnUrl);
                    }
                }
            }

            if (product is FoodProduct)
            {
                cartProduct = _context.CartProducts
                    .OfType<CartFoodProduct>()
                    .Where(cp => cp.CartId == cart.Id)
                    .FirstOrDefault(cp => cp.ProductId == product.Id && cp.FoodOption == (productOption.HasValue ? GetFoodOption(productOption.Value) : null));
            }
            else if (product is SauceProduct)
            {
                cartProduct = _context.CartProducts
                    .OfType<CartSauceProduct>()
                    .Where(cp => cp.CartId == cart.Id)
                    .FirstOrDefault(cp => cp.ProductId == product.Id && cp.SauceOption == GetSauceOption(input));
            }


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
                            FoodOption = productOption.HasValue ? GetFoodOption((char)productOption) : null,
                        };
                    }
                }
                else if (product is SauceProduct)
                {
                    cartProduct = new CartSauceProduct()
                    {
                        SauceOption = GetSauceOption(input),
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

        public IActionResult SearchCustomers(string phoneNumber)
        {
            var customers = _context.Customers
                .Where(c => c.PhoneNumber.StartsWith(phoneNumber))
                .Select(c => new
                {
                    c.Address,
                    c.Name,
                    c.PhoneNumber
                }).ToList();

            return Json(customers);
        }
    }
}
