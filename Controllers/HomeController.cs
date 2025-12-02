using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.VM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<SystemUser> _userManager;

        private readonly FastFoodSystemDbContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            HashSet<ProductVM> productsVM = _context.Products.Select(p => new ProductVM
            {
                Product = p,
                Quantity = p.CartProducts.Sum(cp => cp.Quantity)
            }).ToHashSet();

            List<ProductCategory> categoryOrder = new List<ProductCategory>
            {
                ProductCategory.Appetizers,
                ProductCategory.MixedGreens,
                ProductCategory.Noodles,
                ProductCategory.FriedRice,
                ProductCategory.ChopSuey,
                ProductCategory.EggFooYung,
                ProductCategory.SeaFood,
                ProductCategory.Chicken,
                ProductCategory.BeefAndPork,
                ProductCategory.HotAndSpicy,
                ProductCategory.ComboPlates,
                ProductCategory.FamilyDinners,
                ProductCategory.Extras
            };

            Dictionary<ProductCategory, List<ProductVM>> productsByCategory = productsVM
                .GroupBy(p => p.Product.Category)
                .OrderBy(g => categoryOrder.IndexOf(g.Key)) // Order categories according to categoryOrder list
                .ToDictionary(g => g.Key, g => g.OrderBy(p =>
                {
                    string code = p.Product.Code;
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

            //Dictionary<ProductCategory, List<Product>>? products = _context.Products
            //    .GroupBy(p => p.Category)
            //    //.ToDictionary(g => g.Key, g => g.ToList());
            //    //.ToDictionary(g => g.Key, g => g.OrderBy(p => p.Code).ToList());
            //    // Sort codes naturally: numbers first (1, 2, 3, ...),
            //    // then number+letter codes (6A, 7A, ...), 
            //    // then letter+number codes (F1, F2, ...)
            //    .ToDictionary(g => g.Key, g => g.OrderBy(p =>
            //    {
            //        string code = p.Code;
            //        int i = 0;

            //        while (i < code.Length && char.IsDigit(code[i]))
            //        {
            //            i++;

            //        }
            //        if (i > 0)
            //        {
            //            int number = int.Parse(code.Substring(0, i));
            //            string letter = code.Substring(i);

            //            return (0, number, letter);
            //        }
            //        else
            //        {
            //            return (1, 0, code);
            //        }
            //    }).ToList());

            return View(productsByCategory);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
