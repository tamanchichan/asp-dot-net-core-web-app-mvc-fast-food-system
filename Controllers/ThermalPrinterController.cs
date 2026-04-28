using asp_dot_net_core_web_app_mvc_fast_food_system.Areas.Identity.Data;
using asp_dot_net_core_web_app_mvc_fast_food_system.Models.Base;
using asp_dot_net_core_web_app_mvc_fast_food_system.POS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace asp_dot_net_core_web_app_mvc_fast_food_system.Controllers
{
    public class ThermalPrinterController : Controller
    {
        private readonly FastFoodSystemDbContext _context;

        private readonly ThermalPrinterService _printer;

        public ThermalPrinterController(FastFoodSystemDbContext context, ThermalPrinterService printer)
        {
            _context = context;
            _printer = printer;
        }

        public IResult PrintOrder(Guid id)
        {
            Order order = _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.Id == id);

            _printer.PrintReceiptUSB(order);
            
            Thread.Sleep(1000);

            _printer.PrintReceiptKitchenUSB(order);

            return Results.NoContent();
        }

        public IResult PrintKitchenOrder(Guid id)
        {
            Order order = _context.Orders
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .FirstOrDefault(o => o.Id == id);

            _printer.PrintReceiptKitchenUSB(order);

            return Results.NoContent();
        }
    }
}
