using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    }
}
