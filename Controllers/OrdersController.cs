using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.POS;
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

        private readonly ThermalPrinterService _printer;

        public OrdersController(ILogger<HomeController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context, ThermalPrinterService printer)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
            _printer = printer;
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

        // GET: /Orders/GetOrdersAt?dateTime=2024-01-01T12:30:00
        // Setup U/I | U/X later
        public IActionResult GetOrdersAt(DateTime dateTime)
        {
            DateTime startTime = new DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                0
            );

            DateTime endTime = startTime.AddMinutes(1);

            HashSet<Order> orders = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .Where(o => o.ReadyTime >= startTime && o.ReadyTime < endTime)
                .ToHashSet();

            if (orders.Count > 1)
            {
                _printer.PrintAllOrdersAt(orders, dateTime);
            }

            return Json(orders);
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
