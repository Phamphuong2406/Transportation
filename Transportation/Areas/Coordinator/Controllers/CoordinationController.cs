using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;



namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

    public class CoordinationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult ListCoordination()
        {
            return View();
        }
        [HttpGet]

        public IActionResult Detail(int Id)
        {
            @ViewBag.RequestId = Id;
            return View();

        }

    }
}


