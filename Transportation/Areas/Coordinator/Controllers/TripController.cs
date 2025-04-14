using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

 
    public class TripController : Controller
    {
        private MyDbContext _context;
        public TripController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            return View();
        }
       
        [HttpPost]
        public IActionResult CreateTrip(Trip model)
        {

          
            return RedirectToAction("Index");

        }
    }
}
