using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportation.Infrastructure.Data;

namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

    public class OrderController : Controller
    {
        private MyDbContext _context;
        public OrderController(MyDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            return View(_context.ShippingRequests.Include(x =>x.Customer).ToList());
        }
    }
}
