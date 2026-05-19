using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Enums;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.PaginatedList;
using asp_dot_net_core_web_app_mvc_fast_food_system.POS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.Threading.Tasks;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Index(int? page, DateTime? date)
        {
            page ??= 1;

            if (date == null)
            {
                date = DateTime.Today;
            }

            DateTime startDate = date.Value.Date;
            DateTime endDate = startDate.AddDays(1);

            IQueryable<Order> orders = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .Where(o => o.ReadyTime.Date >= startDate && o.ReadyTime < endDate)
                .Where(o => (bool)!o.IsCanceled)
                .OrderByDescending(o => o.ReadyTime)
                    .ThenByDescending(o => (double)o.OrderProducts.Sum(op => op.Price * op.Quantity))
                .AsNoTracking();

            // Pass the current date being filtered/shown to the view
            ViewBag.SelectedDate = date.Value;

            int pageSize = 5;

            //return View(orders);
            return View(await PaginatedList<Order>.Create(orders, page ?? 1, pageSize));
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
                    .Include(o => o.Customer)
                    .Include(o => o.User)
                    .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        public IActionResult EditOrder(Guid id)
        {
            Order order = _context.Orders
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

        // Add orderProducts editing later
        [HttpPost]
        public IActionResult EditOrder(Guid id, Guid? customerId, Customer? customer, string? customerName, string? customerPhoneNumber, string? customerAddress, OrderType orderType, string? observations, DateOnly readyDateOnly, TimeOnly readyTimeOnly, string? additionalCharge, string? deliveryFee, string? discount)
        {
            Order order = _context.Orders
                .Include(o => o.OrderProducts)
                .ThenInclude(op => op.Product)
                .Include(o => o.User)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) NotFound();

            if (customer == null && !(String.IsNullOrEmpty(customerPhoneNumber)))
            {
                customer = new Customer()
                {
                    Name = customerName,
                    PhoneNumber = customerPhoneNumber,
                    Address = customerAddress
                };

                customer.Orders.Add(order);
                order.Customer = customer;
            }

            order.CustomerName = customerName;
            order.CustomerPhoneNumber = customerPhoneNumber;
            order.CustomerAddress = customerAddress;
            order.Type = orderType;
            order.Observations = observations;
            order.ReadyTime = readyDateOnly.ToDateTime(readyTimeOnly);
            order.AdditionalCharge = string.IsNullOrEmpty(additionalCharge) ? 0m : decimal.Parse(additionalCharge.Replace(",", "."), CultureInfo.InvariantCulture);
            order.DeliveryFee = string.IsNullOrEmpty(deliveryFee) ? 0m : decimal.Parse(deliveryFee.Replace(",", "."), CultureInfo.InvariantCulture);
            order.Discount = string.IsNullOrEmpty(discount) ? 0m : decimal.Parse(discount.Replace(",", "."), CultureInfo.InvariantCulture);

            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("OrderDetails", new { id = order.Id });
        }

        public IActionResult IncrementProduct(Guid id, int quantity = 1)
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            OrderProduct orderProduct = _context.OrderProducts
                .Include(op => op.Order)
                .FirstOrDefault(op => op.Id == id);

            if (orderProduct == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            orderProduct.Quantity += quantity;

            //_context.OrderProducts.Update(orderProduct);
            //_context.SaveChanges();

            return Redirect(returnUrl);
        }

        // Handle if there is only 1 orderProduct left, make the whole order as cancelled (needs to create a new field for order status) instead (do not delete order)
        // Perhaps not use "context.SaveChanges" and create a new action to save anything that was modified
        public IActionResult DecrementProduct(Guid id, int quantity = 1)
        {
            string returnUrl = Request.Headers["Referer"].ToString();

            OrderProduct orderProduct = _context.OrderProducts
                .Include(op => op.Order)
                .FirstOrDefault(op => op.Id == id);

            Order order = orderProduct.Order;

            if (orderProduct == null)
            {
                return NotFound();
            }

            if (quantity <= 0)
            {
                return BadRequest("Quantity must be greater than zero.");
            }

            if (orderProduct.Quantity == 1 || (orderProduct.Quantity - quantity) <= 0)
            {
                order.OrderProducts.Remove(orderProduct);
                _context.OrderProducts.Remove(orderProduct);
            }
            else
            {
                orderProduct.Quantity -= quantity;
                _context.OrderProducts.Update(orderProduct);
            }

            _context.SaveChanges();

            return Redirect(returnUrl);
        }

        public IActionResult CancelOrder(Guid id)
        {
            Order order = _context.Orders
                .Include(o => o.OrderProducts)
                .FirstOrDefault(o => o.Id == id);

            if (order == null) NotFound();

            order.IsCanceled = true;
            _context.Orders.Update(order);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        
        public IActionResult SearchOrder(int orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber.ToString()))
            {
                TempData["ErrorMessage"] = "Order number cannot be null/empty.";
                return RedirectToAction("Index");
            }

            Order order = _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.Number == orderNumber);

            if (order == null)
            {
                TempData["ErrorMessage"] = "Order number not found.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("OrderDetails", new { id = order.Id });
        }
    }
}
