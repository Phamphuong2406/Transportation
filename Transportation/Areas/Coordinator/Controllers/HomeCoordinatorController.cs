using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

    public class HomeCoordinatorController : Controller
    {
        private MyDbContext _context;
        public HomeCoordinatorController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

    
    }
}


