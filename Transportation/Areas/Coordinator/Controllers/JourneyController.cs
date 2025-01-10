using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Transportation.Infrastructure.Data;

namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

    [Authorize(Roles = "Dispatcher")]
    public class JourneyController : Controller
    {
        private MyDbContext _context;
        public  JourneyController(MyDbContext context)
        {
            _context = context;
        }
        //Hôm có danh sách các xe
        public IActionResult Index()
        {

            return View(_context.Trucks.Include(x => x.Driver).ToList());
        }
        [HttpGet]

        public IActionResult Detail(int Id)
        {
            var data = _context.DispatchAssignments.Include(x =>x.Truck).FirstOrDefault(x => x.TruckId == Id);
            if (data != null)
            {

                return View(data);
            }
            return NotFound();

        }
        
        public IActionResult Realtime(int Id)
        {
            // Lấy danh sách vị trí từ RealTimeTracking, sắp xếp theo thời gian
            var locations = _context.RealTimeTrackings
                .Where(rt => rt.TruckId == Id)
                .OrderBy(rt => rt.Timestamp) // Sắp xếp theo thời gian (từ nhỏ đến lớn)
                .Select(rt => new RealTimeTracking
                {
                    CurrentLat = rt.CurrentLat,
                    CurrentLng = rt.CurrentLng,
                    Timestamp = rt.Timestamp,
                    Truck = rt.Truck
                })
                .ToList();

            return View(locations); // Truyền danh sách RealTimeTracking làm model
        }


    }
}
