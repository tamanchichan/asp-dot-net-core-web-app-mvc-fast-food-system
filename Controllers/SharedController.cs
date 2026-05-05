using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Interface;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

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
            string returnUrl = Request.Headers["Referer"].ToString();
            
            // If input is NULL or EMPTY, do nothing, return to "previous" page
            if (string.IsNullOrEmpty(input))
            {
                return Redirect(returnUrl);
            }

            input = input.ToUpperInvariant();

            string code;
            int quantity;
            decimal additionalPrice = 0m;
            string instructions = null;

            if (input.Contains("*") || input.Contains("x"))
            {
                string[] parts = input.Split(new char[] { '*', 'x' });
                code = parts[0];

                if (string.IsNullOrEmpty(parts[1]))
                {
                    TempData["ErrorMessage"] = "Quantity must be entered";

                    return Redirect(returnUrl);
                }

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
                    product = _context.Products.FirstOrDefault(p => p.Code == "SAUCE");

                    if (product == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid code. Please double-check the code entered");

                        return Redirect(returnUrl);
                    }
                }
            }

            if (product is BeverageProduct)
            {
                cartProduct = _context.CartProducts
                    .OfType<CartBeverageProduct>()
                    .Where(cp => cp.CartId == cart.Id)
                    .FirstOrDefault(
                        cp =>
                            cp.ProductId == product.Id &&
                            //cp.BeverageOption == (productOption.HasValue ? GetBeverageOption(productOption.Value) : null) && // Create GetBeverageOption function (?)
                            cp.AdditionalPrice == additionalPrice &&
                            cp.Instructions == instructions
                    );
            }
            else if (product is ExtraProduct)
            {
                cartProduct = _context.CartProducts
                    .OfType<CartExtraProduct>()
                    .Where(cp => cp.CartId == cart.Id)
                    .FirstOrDefault(
                        cp =>
                            cp.ProductId == product.Id &&
                            cp.AdditionalPrice == additionalPrice &&
                            cp.Instructions == instructions
                    );
            }
            else if (product is FoodProduct)
            {
                cartProduct = _context.CartProducts
                    .OfType<CartFoodProduct>()
                    .Where(cp => cp.CartId == cart.Id)
                    .FirstOrDefault(
                        cp =>
                            cp.ProductId == product.Id &&
                            cp.FoodOption == (productOption.HasValue ? GetFoodOption(productOption.Value) : null) &&
                            cp.AdditionalPrice == additionalPrice &&
                            cp.Instructions == instructions
                    );
            }
            else if (product is SauceProduct)
            {
                cartProduct = _context.CartProducts
                    .OfType<CartSauceProduct>()
                    .Where(cp => cp.CartId == cart.Id)
                    .FirstOrDefault(
                        cp =>
                            cp.ProductId == product.Id &&
                            cp.SauceOption == GetSauceOption(input) &&
                            cp.AdditionalPrice == additionalPrice &&
                            cp.Instructions == instructions
                    );
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
                else if (product is ExtraProduct)
                {
                    cartProduct = new CartExtraProduct()
                    {
                        //No options for extra products for now, but can add later if needed
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
                        if (product.HasOptions && productOption == null)
                        {
                            TempData["ErrorMessage"] = "Please select an option for this product.";
                            return Redirect(returnUrl);
                        }

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
                cartProduct.AdditionalPrice = 0m;
                cartProduct.Quantity = (int)quantity;

                _context.CartProducts.Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity += (int)quantity;

                _context.CartProducts.Update(cartProduct);
            }

            _context.SaveChanges();

            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult AddProductToOrder(Guid orderId, string input)
        {
            input = input.ToUpperInvariant();

            string returnUrl = Request.Headers["Referer"].ToString();
            string code;
            int quantity;
            decimal additionalPrice = 0m;
            string instructions = null;

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

            Order order = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.Id == orderId);

            if (order == null)
            {
                ModelState.AddModelError(string.Empty, "No active order found for the user.");
            }

            Product product = _context.Products.FirstOrDefault(p => p.Code == productCode);

            OrderProduct orderProduct = null;

            if (product == null)
            {
                productCode = code.Substring(0, lastIndex);
                productOption = code[lastIndex];

                product = _context.Products.FirstOrDefault(p => p.Code == productCode);

                if (product == null)
                {
                    // if productCode not found, assume it is sauce product
                    product = _context.Products.FirstOrDefault(p => p.Code == "SAUCE");

                    if (product == null)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid code. Please double-check the code entered");

                        return Redirect(returnUrl);
                    }
                }
            }

            if (product is BeverageProduct)
            {
                orderProduct = _context.OrderProducts
                    .OfType<OrderBeverageProduct>()
                    .Where(op => op.OrderId == order.Id)
                    .FirstOrDefault(
                        op =>
                            op.ProductId == product.Id &&
                            //op.BeverageOption == (productOption.HasValue ? GetBeverageOption(productOption.Value) : null) && // Create GetBeverageOption function (?)
                            op.AdditionalPrice == additionalPrice &&
                            op.Instructions == instructions
                    );
            }
            else if (product is ExtraProduct)
            {
                orderProduct = _context.OrderProducts
                    .OfType<OrderExtraProduct>()
                    .Where(op => op.OrderId == order.Id)
                    .FirstOrDefault(
                        op =>
                            op.ProductId == product.Id &&
                            op.AdditionalPrice == additionalPrice &&
                            op.Instructions == instructions
                    );
            }
            else if (product is FoodProduct)
            {
                orderProduct = _context.OrderProducts
                    .OfType<OrderFoodProduct>()
                    .Where(op => op.OrderId == order.Id)
                    .FirstOrDefault(
                        op =>
                            op.ProductId == product.Id &&
                            op.FoodOption == (productOption.HasValue ? GetFoodOption(productOption.Value) : null) &&
                            op.AdditionalPrice == additionalPrice &&
                            op.Instructions == instructions
                    );
            }
            else if (product is SauceProduct)
            {
                orderProduct = _context.OrderProducts
                    .OfType<OrderSauceProduct>()
                    .Where(op => op.OrderId == order.Id)
                    .FirstOrDefault(
                        op =>
                            op.ProductId == product.Id &&
                            op.SauceOption == GetSauceOption(input) &&
                            op.AdditionalPrice == additionalPrice &&
                            op.Instructions == instructions
                    );
            }

            if (orderProduct == null)
            {
                if (product is BeverageProduct)
                {
                    orderProduct = new OrderBeverageProduct()
                    {
                        //BeverageOption = 
                    };
                }
                else if (product is ExtraProduct)
                {
                    orderProduct = new OrderExtraProduct()
                    {
                        //No options for extra products for now, but can add later if needed
                    };
                }
                else if (product is FoodProduct)
                {
                    if (product.Code == "25")
                    {
                        orderProduct = new OrderFoodProduct()
                        {
                            //FoodSize = 
                        };
                    }
                    else
                    {
                        if (product.HasOptions && productOption == null)
                        {
                            TempData["ErrorMessage"] = "Please select an option for this product.";
                            return Redirect(returnUrl);
                        }

                        orderProduct = new OrderFoodProduct()
                        {
                            FoodOption = productOption.HasValue ? GetFoodOption((char)productOption) : null,
                        };
                    }
                }
                else if (product is SauceProduct)
                {
                    orderProduct = new OrderSauceProduct()
                    {
                        SauceOption = GetSauceOption(input),
                    };
                }

                if (orderProduct == null)
                {
                    ModelState.AddModelError(string.Empty, "Error creating a 'cartProduct'.");
                    return Redirect(returnUrl); //ModelState doesn't work for Redirect, so update this and others later to TempData
                }

                orderProduct.OrderId = order.Id;
                orderProduct.Order = order;
                orderProduct.ProductId = product.Id;
                orderProduct.Product = product;
                orderProduct.Code = product.Code;
                orderProduct.Name = product.Name;
                orderProduct.Price = product.Price;
                orderProduct.Category = product.Category;
                orderProduct.HasOptions = product.HasOptions;
                orderProduct.AdditionalPrice = 0m;
                orderProduct.Quantity = (int)quantity;

                _context.OrderProducts.Add(orderProduct);
            }
            else
            {
                orderProduct.Quantity += (int)quantity;

                _context.OrderProducts.Update(orderProduct);
            }

            _context.SaveChangesAsync();

            return Redirect(returnUrl);
        }

        [HttpPost]
        public IActionResult IncrementProduct(Guid id, int quantity = 1)
        {
            Cart cart = _context.Carts?.FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            CartProduct cartProduct = _context.CartProducts
                .Include(cp => cp.Cart)
                    .ThenInclude(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                .FirstOrDefault(cp => cp.Id == id);

            if (cartProduct == null)
            {
                OrderProduct orderProduct = _context.OrderProducts
                    .Include(op => op.Order)
                        .ThenInclude(o => o.OrderProducts)
                            .ThenInclude(op => op.Product)
                    .FirstOrDefault(op => op.Id == id);

                if (orderProduct == null)
                {
                    return NotFound();
                }

                orderProduct.Quantity += quantity;

                _context.OrderProducts.Update(orderProduct);
                _context.SaveChanges();

                return Json(new
                {
                    quantity = orderProduct.Quantity,
                    productTotalPrice = orderProduct.TotalPrice,
                    subTotalPrice = orderProduct.Order.SubTotalPrice,
                    totalPrice = orderProduct.Order.TotalPrice
                });
            }
            else
            {
                cartProduct.Quantity += quantity;
                _context.CartProducts.Update(cartProduct);
                _context.SaveChanges();

                return Json(new
                {
                    quantity = cartProduct.Quantity,
                    productTotalPrice = cartProduct.TotalPrice,
                    subTotalPrice = cart.SubTotalPrice,
                    totalPrice = cart.TotalPrice
                });
            }
        }

        [HttpPost]
        public IActionResult DecrementProduct(Guid id, int quantity = 1)
        {
            Cart cart = _context.Carts?.FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            IProductItem product = _context.CartProducts
                .Include(cp => cp.Cart)
                    .ThenInclude(c => c.CartProducts)
                        .ThenInclude(cp => cp.Product)
                .FirstOrDefault(cp => cp.Id == id) ;

            if (product == null)
            {
                product = _context.OrderProducts
                    .Include(op => op.Order)
                        .ThenInclude(o => o.OrderProducts)
                            .ThenInclude(op => op.Product)
                    .FirstOrDefault(op => op.Id == id);
            }

            if (product == null)
            {
                return NotFound();
            }

            product.Quantity -= quantity;

            if (product.Quantity == 0)
            {
                if (product is CartProduct cartProduct)
                {
                    _context.CartProducts.Remove(cartProduct);
                }
                else if (product is OrderProduct orderProduct)
                {
                    _context.OrderProducts.Remove(orderProduct);
                }

                _context.SaveChanges();

                return Json(new
                {
                    empty = true,
                    quantity = product.Quantity,
                    productTotalPrice = product.TotalPrice,
                    subTotalPrice = cart != null ? cart.SubTotalPrice : ((OrderProduct)product).Order.SubTotalPrice,
                    totalPrice = cart != null ? cart.TotalPrice : ((OrderProduct)product).Order.TotalPrice
                });
            }

            _context.SaveChanges();

            return Json(new
            {
                quantity = product.Quantity,
                productTotalPrice = product.TotalPrice,
                subTotalPrice = cart != null ? cart.SubTotalPrice : ((OrderProduct)product).Order.SubTotalPrice,
                totalPrice = cart != null ? cart.TotalPrice : ((OrderProduct)product).Order.TotalPrice
            });
        }

        [HttpGet]
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

        [HttpGet]
        public IActionResult RoundNumber(double value, int decimals = 2)
        {
            double rounded = Math.Round(value, decimals);

            return Json(new { original = value, rounded = rounded });
        }

        [HttpPost]
        public IActionResult EditProductAdditionalPriceAndInstructions(Guid id, string additionalPrice, string instructions)
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            CartProduct cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.Id == id);

            if (cartProduct == null)
            {
                OrderProduct orderProduct = _context.OrderProducts.FirstOrDefault(op => op.Id == id);

                if (orderProduct == null)
                {
                    return NotFound();
                }

                orderProduct.AdditionalPrice = string.IsNullOrEmpty(additionalPrice) ? 0m : decimal.Parse(additionalPrice.Replace(",", "."), CultureInfo.InvariantCulture);
                orderProduct.Instructions = instructions;

                _context.OrderProducts.Update(orderProduct);
                _context.SaveChanges();
            }
            else
            {
                cartProduct.AdditionalPrice = string.IsNullOrEmpty(additionalPrice) ? 0m : decimal.Parse(additionalPrice.Replace(",", "."), CultureInfo.InvariantCulture);
                cartProduct.Instructions = instructions;

                _context.CartProducts.Update(cartProduct);
                _context.SaveChanges();

            }

            return Redirect(returnUrl);
        }
    }
}
