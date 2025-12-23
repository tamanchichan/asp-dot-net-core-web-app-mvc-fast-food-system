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

        public IActionResult Index(DateTime? date)
        {
            if (date == null)
            {
                date = DateTime.Today;
            }

            DateTime startDate = date.Value.Date;
            DateTime endDate = startDate.AddDays(1);

            HashSet<Order> orders = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .Where(o => o.ReadyTime.Date >= startDate && o.ReadyTime < endDate)
                .OrderByDescending(o => o.ReadyTime)
                .ThenByDescending(o => o.Number)
                .ToHashSet();

            // Pass the current date being filtered/shown to the view
            ViewBag.SelectedDate = date.Value;

            return View(orders);
        }

        public IActionResult OrderDetails(Guid id)
        {
            Order order =
                _context.Orders
                    .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
