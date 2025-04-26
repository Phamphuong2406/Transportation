using AutoMapper;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
  //  [Authorize(Roles = "Admin")]
    public class HomeAdminController : Controller
    {
        private MyDbContext _context;

        public HomeAdminController(MyDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
           
            return View();
        }
       
    }
}


