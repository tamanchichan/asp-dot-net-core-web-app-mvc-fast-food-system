using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ILogger<CustomersController> _logger;

        private readonly UserManager<SystemUser> _userManager;

        private readonly FastFoodSystemDbContext _context;

        public CustomersController(ILogger<CustomersController> logger, UserManager<SystemUser> userManager, FastFoodSystemDbContext context)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            HashSet<Customer> customers = _context.Customers.ToHashSet();

            return View(customers);
        }

        [HttpGet]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateCustomer(string? customerName, string customerPhoneNumber, string? customerAddress)
        {
            if (string.IsNullOrEmpty(customerPhoneNumber))
            {
                TempData["CustomerName"] = customerName;
                TempData["ErrorMessage"] = "Phone number cannot be null/empty";
                TempData["CustomerAddress"] = customerAddress;

                return View();
            }

            Customer customer = _context.Customers.FirstOrDefault(c => c.PhoneNumber == customerPhoneNumber);

            if (customer == null)
            {
                customer = new Customer()
                {
                    Id = Guid.NewGuid(),
                    Name = customerName,
                    PhoneNumber = customerPhoneNumber,
                    Address = customerAddress
                };

                _context.Customers.Add(customer);
                _context.SaveChanges();

                return RedirectToAction("Index");

            }

            TempData["CustomerName"] = customerName;
            TempData["ErrorMessage"] = "Phone number already exist";
            TempData["CustomerAddress"] = customerAddress;

            return View();
        }

        public IActionResult CustomerDetails(Guid id)
        {
            HashSet<Order> orders = _context.Orders
                .Where(o => o.CustomerId == id)
                .OrderByDescending(o => o.ReadyTime)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .Take(5)
                .ToHashSet();

            Customer customer = _context.Customers
                .Where(c => c.Id == id)
                .Select(c => new Customer
                {
                    Id = c.Id,
                    Name = c.Name,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    Remarks = c.Remarks,
                    Orders = orders
                })
                .FirstOrDefault();

            return View(customer);
        }

        [HttpGet]
        public IActionResult EditCustomer(Guid id)
        {
            Customer customer = _context.Customers.FirstOrDefault(c => c.Id == id);

            return View(customer);
        }

        [HttpPost]
        public IActionResult EditCustomer(Guid id, string? customerName, string customerPhoneNumber, string? customerAddress)
        {
            if (string.IsNullOrEmpty(customerPhoneNumber))
            {
                TempData["CustomerName"] = customerName;
                TempData["ErrorMessage"] = "Phone number cannot be null/empty";
                TempData["CustomerAddress"] = customerAddress;
                return View();
            }

            Customer customer = _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                .FirstOrDefault(c => c.Id == id);
            
            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = customerName;
            customer.PhoneNumber = customerPhoneNumber;
            customer.Address = customerAddress;
            
            _context.Customers.Update(customer);
            _context.SaveChanges();
            
            return RedirectToAction("CustomerDetails", new { id = customer.Id });
        }

        public IActionResult SearchCustomer(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                TempData["ErrorMessage"] = "Phone number cannot be null/empty";
                return RedirectToAction("Index");
            }

            Customer customer = _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                .FirstOrDefault(c => c.PhoneNumber == phoneNumber);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Customer with this phone number does not exist";
                return RedirectToAction("Index");
            }

            return RedirectToAction("CustomerDetails", new { id = customer.Id });
        }
    }
}
