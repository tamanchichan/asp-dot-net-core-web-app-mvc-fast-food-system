using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<SystemUser> _userManager;

        private readonly FastFoodSystemDbContext _context;

        public ProductsController(ILogger<HomeController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            Dictionary<ProductCategory, List<Product>> products = _context.Products
                .GroupBy(p => p.Category)
                .ToDictionary(g => g.Key, g => g.OrderBy(p =>
                {
                    string code = p.Code;
                    int i = 0;

                    while (i < code.Length && char.IsDigit(code[i]))
                    {
                        i++;

                    }
                    if (i > 0)
                    {
                        int number = int.Parse(code.Substring(0, i));
                        string letter = code.Substring(i);

                        return (0, number, letter);
                    }
                    else
                    {
                        return (1, 0, code);
                    }
                }).ToList());

            return View(products);
        }

        public IActionResult CreateProduct()
        {
            ViewBag.Categories = Enum.GetValues(typeof(ProductCategory));

            return View();
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductCategory category, string? code, string? name, string? price, bool? option)
        {
            ViewBag.Categories = Enum.GetValues(typeof(ProductCategory));

            TempData["ProductCode"] = code;
            TempData["ProductName"] = name;
            TempData["ProductPrice"] = price;
            TempData["ProductOption"] = option;
            
            if (string.IsNullOrEmpty(code))
            {
                TempData["ProductCode"] = "Product's code cannot be empty.";

                return View();
            }

            code = code.ToUpper();

            if (string.IsNullOrEmpty(name))
            {
                TempData["ProductName"] = "Product's name cannot be empty.";

                return View();
            }

            if (string.IsNullOrEmpty(price))
            {
                TempData["ProductPrice"] = "Product's price cannot be empty.";

                return View();
            }

            Product product = _context.Products.FirstOrDefault(p => p.Code == code);

            if (product != null)
            {
                TempData["ProductCode"] = "Product's code must be unique.";
                return View();
            }

            if (category == ProductCategory.Beverages)
            {
                product = new BeverageProduct
                {
                    Category = category,
                    Code = code,
                    Name = name,
                    Price = string.IsNullOrEmpty(price) ? 0m : decimal.Parse(price.Replace(",", "."), CultureInfo.InvariantCulture),
                    HasOptions = option ?? false
                };
            }
            else if (category == ProductCategory.Extras)
            {
                product = new ExtraProduct
                {
                    Category = category,
                    Code = code,
                    Name = name,
                    Price = string.IsNullOrEmpty(price) ? 0m : decimal.Parse(price.Replace(",", "."), CultureInfo.InvariantCulture),
                    HasOptions = option ?? false
                };
            }
            else if (category == ProductCategory.Sauces)
            {
                product = new SauceProduct
                {
                    Category = category,
                    Code = code,
                    Name = name,
                    Price = string.IsNullOrEmpty(price) ? 0m : decimal.Parse(price.Replace(",", "."), CultureInfo.InvariantCulture),
                    HasOptions = option ?? false
                };
            }
            else
            {
                product = new FoodProduct()
                {
                    Category = category,
                    Code = code,
                    Name = name,
                    Price = string.IsNullOrEmpty(price) ? 0m : decimal.Parse(price.Replace(",", "."), CultureInfo.InvariantCulture),
                    HasOptions = option ?? false
                };

            }

            _context.Products.Add(product);
            _context.SaveChanges();

            return RedirectToAction("ProductDetails", new { id = product.Id });
        }

        public IActionResult ProductDetails(Guid id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult EditProduct(Guid id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            ViewBag.Categories = Enum.GetValues(typeof(ProductCategory));

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        public IActionResult EditProduct(Guid id, string? code,string? name, string? price, ProductCategory category, bool? option)
        {
            code = code.ToUpper();

            Product product = _context.Products.FirstOrDefault(p => p.Id == id);

            string productCode = product.Code ;

            ViewBag.Categories = Enum.GetValues(typeof(ProductCategory));

            if (product == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(code))
            {
                TempData["ProductCodeMessage"] = "Product's code cannot be empty.";
                return View(product);
            }

            if (string.IsNullOrEmpty(name))
            {
                TempData["ProductNameMessage"] = "Product's name cannot be empty.";
                return View(product);
            }

            if (string.IsNullOrEmpty(price))
            {
                TempData["ProductPriceMessage"] = "Product's price cannot be empty.";
                return View(product);
            }

            if (productCode != code)
            {
                Product productWithCode = _context.Products.FirstOrDefault(p => p.Code == code);
                if (productWithCode != null)
                {
                    TempData["ProductCodeMessage"] = "Product's code must be unique.";
                    return View(product);
                }
            }

            product.Code = code;
            product.Name = name;
            product.Price = string.IsNullOrEmpty(price) ? 0m : decimal.Parse(price.Replace(",", "."), CultureInfo.InvariantCulture);
            product.Category = category;
            product.HasOptions = option ?? false;
            
            _context.Products.Update(product);
            _context.SaveChanges();

            return RedirectToAction("ProductDetails", new { id = product.Id });
        }

        public IActionResult DeleteProduct(Guid id)
        {
            Product product = _context.Products.FirstOrDefault(p => p.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
