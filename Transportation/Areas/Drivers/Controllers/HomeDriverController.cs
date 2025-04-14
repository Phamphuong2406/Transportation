using AutoMapper;
using Azure.Core;
using DataAccess.DataContext;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Transportation.Hubs;
using static Transportation.Areas.Coordinator.Controllers.CoordinationController;


namespace Transportation.Areas.Drivers.Controllers
{
    [Area("Drivers")]

    public class HomeDriverController : Controller
    {
        private MyDbContext _context;
      
        public readonly IWebHostEnvironment _webHostEnvironment;
        public HomeDriverController(MyDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
         
            _webHostEnvironment = webHostEnvironment;

        }
        [HttpGet]
        public IActionResult Index()
        {
            
            return View();
        }

        [HttpGet]

        public IActionResult Detail(int assignmentId)
        {
            ViewBag.assignmentId = assignmentId;
            return View();

        }
        public IActionResult Realtime()
        {
            // Tìm người dùng hiện tại
            var userClaim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserID");
            if (userClaim != null && long.TryParse(userClaim.Value, out long iduser))
            {
                var user = _context.Drivers.SingleOrDefault(x => x.UserId == (int)iduser);
                if (user != null)
                {
                    var truck = _context.Trucks.FirstOrDefault(x => x.DriverId == user.DriverId);
                    if (truck != null)
                    {
                        // gửi truckid sang view
                        ViewBag.truckId = truck.TruckId;
                    }
                }
            }

            return View();
        }
     

        [HttpGet]
        public IActionResult CurrentLocation()
        {
            return View();
        }
        public class updateLocation
        {
            public int tripId {  get; set; }
 
        }
        public class UpdateStatusRequest
        {
            public int RequestId { get; set; }
        }
    }
}
