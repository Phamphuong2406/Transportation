using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
   
    public class ShiftsController : Controller
    {
        private MyDbContext _context;

        public ShiftsController(MyDbContext context)
        {
            _context = context;
        }

       
        public IActionResult Index()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : "Khách";
            ViewData["UserName"] = userName;
            var data = _context.Shifts.ToList();
            return View(data);
        }

   

       


    }

}

