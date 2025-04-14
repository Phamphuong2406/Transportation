using DataAccess.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


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
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Khách";
            ViewData["UserName"] = userName;
            return View(_context.ShippingRequests.Include(x =>x.Customer).ToList());
        }
    }
}
