using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.CartProducts;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
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

        public CartController(ILogger<CartController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
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
            BeverageOption? beverageOption = null,
            FoodOption? foodOption = null,
            FoodSize? foodSize = null,
            SauceOption? sauceOption = null
        )
        {
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
                        FoodOption = foodOption,
                        FoodSize = foodSize,
                        Quantity = 1
                    };
                }
                // add the rest of the derived class

                cart.CartProducts.Add(cartProduct);
            }
            else
            {
                cartProduct.Quantity++;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }
    }
}
