using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Transportation.Infrastructure.Data;

namespace Transportation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
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
        public IActionResult Compareorders()
        {
            var data = _context.DispatchAssignments.
                Select(x => new
                {
                    TruckId = x.TruckId,
                    Status = x.Status,

                }).ToList();
            var result = data.Where(x => x.Status == "Đã giao hàng")
                .GroupBy(x => x.TruckId)
                .Select(item => new
                {
                    Truckid = item.Key,
                    TotalProcessed = item.Count()
                });
            return Json(result);
        }

        public IActionResult CompareRevenue()
        {
            // lấy dữ liệu của bảng
            var data = _context.DispatchAssignments.ToList();

            var resultt = data.Where(x => x.Status == "Đã giao hàng")
               .GroupBy(x => x.RequestDate.Month)
               .Select(item => new
               {
                   Date = item.Key,
                   Cost = item.Sum(x => x.ShippingCost)
               });
            return Json(resultt);
        }
    }
}


