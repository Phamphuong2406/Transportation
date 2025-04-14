using DataAccess.DataContext;
using DataAccess.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace Transportation.Controllers
{

    public class ShippingRequestsController : Controller
    {

        private MyDbContext _context;
        public ShippingRequestsController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
          
            return View();
        }

        [HttpGet]
        public IActionResult RealtimeTruck(int id)
        {
            //lấy đơn hàng 
            var assign = _context.DispatchAssignments.FirstOrDefault(u => u.RequestId == id);
            // lấy tripId
            if (assign == null) return NotFound();
            var trip = _context.Trips.FirstOrDefault(a => a.TripId == assign.TripId);
            if (trip == null) return NotFound();
            //lấy truckid
            var truck = _context.Trips.FirstOrDefault(t => t.TruckId == trip.TruckId);
            //lấy tọa độ của truck
            if (truck == null) return NotFound();
            var realtime = _context.RealTimeTrackings
                 .OrderBy(a => a.Timestamp.TimeOfDay)
                 .Where(x => x.TruckId == truck.TruckId &&
                        x.Timestamp >= assign.Pickupdate &&
                        (assign.Deliverydate == null || x.Timestamp <= assign.Deliverydate))
                 .Select( q => new
                 {
                     lat = q.CurrentLat,
                     lng = q.CurrentLng,
                 })
                .ToList();
            return Json(realtime);


        }
     



    }
}
