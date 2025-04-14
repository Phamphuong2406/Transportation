using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;



namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TrucksController : Controller
    {

        private MyDbContext _context;
        public TrucksController(MyDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           
            return View();
        }
        [HttpGet]
        public IActionResult RegisterTruck()
        {
           
            return RedirectToAction("Index");
        }
   
        

    }
}
