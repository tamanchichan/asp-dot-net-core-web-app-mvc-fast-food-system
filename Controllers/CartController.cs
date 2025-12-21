using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.OrderProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using asp_dot_net_core_web_app_mvc_fast_food_system.POS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ILogger<CartController> _logger;

        private readonly UserManager<SystemUser> _userManager;

        private readonly SignInManager<SystemUser> _signInManager;

        private readonly FastFoodSystemDbContext _context;

        private readonly ThermalPrinterService _printer;

        public CartController(ILogger<CartController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context, ThermalPrinterService printer)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _printer = printer;
        }

        public IActionResult Index()
        {
            Cart cart = _context.Carts?
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            if (cart == null)
            {
                ModelState.AddModelError(string.Empty, "Cart not found for the current user. Contact support.");
                return View();
            }

            if (!cart.CartProducts.Any())
            {
                ModelState.AddModelError(string.Empty, "Cart is empty. Add products to continue.");
                return View(cart);
            }

            return View(cart);
        }

        public IActionResult AddProductToCart
        (
            Guid id,
            string? instructions = null,
            decimal? additionalPrice = null,
            BeverageOption? beverageOption = null,
            FoodOption? foodOption = null,
            FoodSize? foodSize = null,
            SauceOption? sauceOption = null
        )
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            Cart cart = _context.Carts?
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            if (cart == null)
            {
                ModelState.AddModelError(string.Empty, "Cart not found for the current user. Contact support.");
                return RedirectToAction("Index");
            }

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            CartProduct cartProduct = cart.CartProducts.FirstOrDefault(cp => cp.ProductId == product.Id);

            if (cartProduct == null)
            {
                if (product is FoodProduct)
                {
                    cartProduct = new CartFoodProduct
                    {
                        CartId = cart.Id,
                        Cart = cart,
                        ProductId = product.Id,
                        Product = product,
                        Instructions = instructions,
                        AdditionalPrice = additionalPrice ?? 0m,
                        FoodOption = foodOption,
                        FoodSize = foodSize,
                        Quantity = 1
                    };
                }
                else if (product is BeverageProduct)
                {
                    cartProduct = new CartBeverageProduct()
                    {
                        CartId = cart.Id,
                        Cart = cart,
                        ProductId = product.Id,
                        Product = product,
                        Instructions = instructions,
                        AdditionalPrice = additionalPrice ?? 0m,
                        BeverageOption = beverageOption,
                        Quantity = 1
                    };
                }
                else if (product is SauceProduct)
                {
                    cartProduct = new CartSauceProduct()
                    {
                        CartId = cart.Id,
                        Cart = cart,
                        ProductId = product.Id,
                        Product = product,
                        Instructions = instructions,
                        AdditionalPrice = additionalPrice ?? 0m,
                        SauceOption = sauceOption,
                        Quantity = 1
                    };
                }

                cart.CartProducts.Add(cartProduct);
                _context.CartProducts.Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity++;
            }

            _context.SaveChanges();

            return Redirect(returnUrl);
        }

        public IActionResult IncrementProduct(Guid id, int quantity = 1)
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            CartProduct cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.Id == id);

            if (cartProduct == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            cartProduct.Quantity += quantity;

            _context.CartProducts.Update(cartProduct);
            _context.SaveChanges();

            return Redirect(returnUrl);
        }

        public IActionResult DecrementProduct(Guid id, int quantity = 1)
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            Cart cart = _context.Carts?.FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            if (cart == null)
            {
                return NotFound(cart);
            }

            CartProduct cartProduct = _context.CartProducts.FirstOrDefault(cp => cp.Id == id);

            if (cartProduct == null)
            {
                return NotFound(cartProduct);
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            if (cartProduct.Quantity == 1 || (cartProduct.Quantity - quantity) <= 0)
            {
                cart.CartProducts.Remove(cartProduct);
                _context.CartProducts.Remove(cartProduct);
            }
            else
            {
                cartProduct.Quantity -= quantity;
                _context.CartProducts.Update(cartProduct);
            }

            _context.SaveChanges();

            return Redirect(returnUrl);
        }

        public IActionResult ClearCartProducts()
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            Cart cart = _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            if (cart == null)
            {
                return NotFound(cart);
            }

            if (cart.CartProducts.Any())
            {
                foreach (CartProduct cartProduct in cart.CartProducts)
                {
                    cart.CartProducts.Remove(cartProduct);
                }

                _context.Carts.Update(cart);
                _context.SaveChanges();
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceCartOrder(string? customerName, string? customerPhoneNumber, string? customerAddress, OrderType orderType, string? observations, DateTime readyTime, decimal? additionalCharge = 0, decimal? deliveryFee = 0, decimal? discount = 0)
        {
            Cart cart = _context.Carts
                .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
                .FirstOrDefault(c => c.UserId == _userManager.GetUserId(User));

            HashSet<CartProduct> cartProducts = cart.CartProducts;

            Order order = new Order()
            {
                UserId = _userManager.GetUserId(User),
                User = await _userManager.GetUserAsync(User),
                CustomerName = customerName,
                CustomerPhoneNumber = customerPhoneNumber,
                CustomerAddress = customerAddress,
                Type = orderType,
                Observations = observations,
                ReadyTime = readyTime,
                AdditionalCharge = additionalCharge ?? 0m,
                DeliveryFee = deliveryFee ?? 0m,
                Discount = discount ?? 0m
            };

            foreach (CartProduct cartProduct in cartProducts)
            {
                if (cartProduct is CartFoodProduct cartFoodProduct)
                {
                    OrderFoodProduct orderFoodProduct = new OrderFoodProduct()
                    {
                        OrderId = order.Id,
                        Order = order,
                        Product = cartFoodProduct.Product,
                        ProductId = cartFoodProduct.ProductId,
                        FoodOption = cartFoodProduct.FoodOption,
                        FoodSize = cartFoodProduct.FoodSize,
                        Quantity = cartFoodProduct.Quantity,
                        Instructions = cartFoodProduct.Instructions,
                        AdditionalPrice = cartFoodProduct.AdditionalPrice,
                    };

                    order.OrderProducts.Add(orderFoodProduct);
                    _context.OrderProducts.Add(orderFoodProduct);
                }
                else if (cartProduct is CartBeverageProduct cartBeverageProduct)
                {
                    OrderBeverageProduct orderBeverageProduct = new OrderBeverageProduct()
                    {
                        OrderId = order.Id,
                        Order = order,
                        Product = cartBeverageProduct.Product,
                        ProductId = cartBeverageProduct.ProductId,
                        BeverageOption = cartBeverageProduct.BeverageOption,
                        Quantity = cartBeverageProduct.Quantity,
                        Instructions = cartBeverageProduct.Instructions,
                        AdditionalPrice = cartBeverageProduct.AdditionalPrice,
                    };

                    order.OrderProducts.Add(orderBeverageProduct);
                    _context.OrderProducts.Add(orderBeverageProduct);
                }
                else if (cartProduct is CartSauceProduct cartSauceProduct)
                {
                    OrderSauceProduct orderSauceProduct = new OrderSauceProduct()
                    {
                        OrderId = order.Id,
                        Order = order,
                        Product = cartSauceProduct.Product,
                        ProductId = cartSauceProduct.ProductId,
                        SauceOption = cartSauceProduct.SauceOption,
                        Quantity = cartSauceProduct.Quantity,
                        Instructions = cartSauceProduct.Instructions,
                        AdditionalPrice = cartSauceProduct.AdditionalPrice,
                    };

                    order.OrderProducts.Add(orderSauceProduct);
                    _context.OrderProducts.Add(orderSauceProduct);
                }

                //cart.CartProducts.Remove(cartProduct);
                //_context.Carts.Update(cart);
            }

            if (!String.IsNullOrEmpty(customerPhoneNumber))
            {
                customerPhoneNumber = customerPhoneNumber.Trim();

                Customer customer = _context.Customers.FirstOrDefault(c => c.PhoneNumber == customerPhoneNumber);

                if (customer == null)
                {
                    customer = new Customer()
                    {
                        Name = customerName,
                        PhoneNumber = customerPhoneNumber,
                        Address = customerAddress
                    };

                    _context.Customers.Add(customer);
                }

                order.Customer = customer;
            }

            // Increment Order's number
            int lastNumber = _context.Orders.Any() ? _context.Orders.Max(o => o.Number) : 1000;
            order.Number = lastNumber + 1;

            // Add the order to the database
            _context.Orders.Add(order);

            // Clear cart products
            cart.CartProducts.Clear();
            _context.Carts.Update(cart);

            // Save changes to the database
            _context.SaveChanges();

            _printer.PrintReceiptUSB(order);

            return RedirectToAction("Index", "Home");
        }
    }
}
