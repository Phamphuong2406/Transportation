using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


using Transportation.Infrastructure.Data;


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
            var truck = _context.Trucks.Include(x => x.Driver).ToList();
            return View(truck);
        }
        [HttpGet]
        public IActionResult RegisterTruck()
        {
           
            ViewBag.Driver = new SelectList(_context.Drivers, "DriverId", "FullName"); // tạo 1 dropdown lựa chọn có value = id và giá trị = tên
            return View();
        }
      [HttpPost]
        public IActionResult RegisterTruck(Truck model, [FromForm] decimal latitude, [FromForm] decimal longitude)
        {

            ViewBag.Driver = new SelectList(_context.Drivers, "DriverId", "FullName"); // tạo 1 dropdown lựa chọn có value = id và giá trị = tên
            if (ModelState.IsValid)
            {
                model.ParkingLat = latitude;
                model.ParkingLng = longitude;
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            
            return View();
        }
    }
}
