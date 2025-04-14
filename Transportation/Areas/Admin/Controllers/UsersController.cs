using Microsoft.AspNetCore.Mvc;

namespace Transportation.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        [Area("Admin")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
