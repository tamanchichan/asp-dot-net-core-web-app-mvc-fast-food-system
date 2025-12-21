using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UserManager<SystemUser> _userManager;

        private readonly FastFoodSystemDbContext _context;

        public OrdersController(ILogger<HomeController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            HashSet<Order> orders = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .OrderByDescending(o => o.Number)
                .ToHashSet();

            return View(orders);
        }
    }
}
