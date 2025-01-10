using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportation.Infrastructure.Data;

namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

    [Authorize(Roles = "Dispatcher")]
    public class HomeController : Controller
    {
        private MyDbContext _context;
        public HomeController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Coordination");
        }

        public IActionResult GetList()
        {
            var data = _context.Trucks.Select( tr => new Truck
            {
                TruckId = tr.TruckId,
                DriverId = tr.DriverId,
                Capacity = tr.Capacity,
                FuelType = tr.FuelType,
                ParkingLocation = tr.ParkingLocation
            }).ToList();
            return Ok(data);
        }


    }
}
