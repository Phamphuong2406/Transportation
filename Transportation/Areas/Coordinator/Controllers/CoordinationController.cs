using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Transportation.Infrastructure.Data;

namespace Transportation.Areas.Coordinator.Controllers
{
    [Area("Coordinator")]

   [Authorize(Roles = "Dispatcher")]
    public class CoordinationController : Controller
    {
        private MyDbContext _context;
        public CoordinationController(MyDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index()
        {
            var data = _context.ShippingRequests.ToList();
            return View(data);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var coordinate = new DispatchAssignment();
            //ViewBag.Driver = new SelectList(_context.Trucks, "DriverId", "FullName"); // tạo 1 dropdown lựa chọn có value = id và giá trị = tên
            return View(coordinate);
        }



        [HttpPost]
        public IActionResult Createe([FromBody] AssignTruckRequest requestt)
        {
            if (requestt == null)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
          

            // Truy cập các giá trị từ request
            int requestId = requestt.RequestId;
            int truckId = requestt.TruckId;

            var request = _context.ShippingRequests.Find(requestId);
            var truck = _context.Trucks.Find(truckId);
            // tìm người dùng hiện tại
            var userClaim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserID");
            if (userClaim != null && long.TryParse(userClaim.Value, out long iduser)) // nếu tồn tại người dùng và khi chuyển giá trị của người dùng đó sang dạng long mà nhận được giá trị là idusser chứu k phải null
            {
                var user = _context.Dispatchers.SingleOrDefault(x => x.UserId == (int)iduser); // tìm người dùng theo giá trị value  trả về 
                                                                                               //nếu người dùng có trong database
                if (user != null)
                {
                    var assignment = new DispatchAssignment
                    {
                        RequestId = requestId,
                        TruckId = truckId,
                        AssignedBy = user.DispatcherId,
                        AssignedDate = DateOnly.FromDateTime(DateTime.Now),
                        RequestDate = request.RequestDate,
                        PickupLocation = request.PickupLocation,
                        PickupLat = request.PickupLat,
                        PickupLng = request.PickupLng,
                        DropoffLocation = request.DropoffLocation,
                        DropoffLat = request.DropoffLat,
                        DropoffLng = request.DropoffLng,
                        Weight = request.Weight,
                        ProductTypeId = request.ProductTypeId,
                        ShippingCost = request.ShippingCost,
                        ParkingLat = truck.ParkingLat,
                        ParkingLng = truck.ParkingLng,
                        Capacity = truck.Capacity,
                        FuelType = truck.FuelType,
                        Note = request.Note,
                        Status = "Đã xử lý"

                    };
                    _context.DispatchAssignments.Add(assignment);
                    _context.SaveChanges();
                    // Xử lý logic điều xe ở đây
                    // Ví dụ: Gán xe cho đơn hàng

                    return Ok(new { message = "Điều xe thành công" });
                }
            }
            return NotFound();

        }

        public class AssignTruckRequest
        {
            public int RequestId { get; set; }
            public int TruckId { get; set; }
        }



        [HttpPost]
        public IActionResult GetByIDRequest(int Requestid)
        {
            var Request = _context.ShippingRequests.Find(Requestid);
            if (Request != null)
            {
                //  return Content($"PickupLocation: {Request.PickupLocation}, DropoffLocation: {Request.DropoffLocation}, ShippingCost: {Request.ShippingCost}");
                return Json(new
                {
                    PickupLocation = Request.PickupLocation ?? "Không có dữ liệu",
                    DropoffLocation = Request.DropoffLocation ?? "Không có dữ liệu",
                    ShippingCost = Request.ShippingCost,
                    Weight = Request.Weight,
                    RequestDate = Request.RequestDate,
                    PickupLat = Request.PickupLat,
                    PickupLng = Request.PickupLng,
                    DropoffLat = Request.DropoffLat,
                    DropoffLng = Request.DropoffLng,
                });
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult GetByIDTruck(int Truckid)
        {
            var Truck = _context.Trucks.Find(Truckid);
            if (Truck != null)
            {

                return Ok(new
                {
                    ParkingLat = Truck.ParkingLat,
                    ParkingLng = Truck.ParkingLng,
                    Capacity = Truck.Capacity,
                    FuelType = Truck.FuelType,
                });
            }
            return BadRequest();
        }
        [HttpGet]
       
        public IActionResult Detail(int Id) {
            var data = _context.ShippingRequests.SingleOrDefault(x => x.RequestId == Id);
            if (data != null)
            {

                return View(data);
            }
            return NotFound();
        
        }
        [HttpPost]
        public IActionResult ProcessRow([FromBody] AssignmentRequest rq)
        {
            // Tìm đơn yêu cầu từ database
            var data = _context.ShippingRequests.SingleOrDefault(x => x.RequestId == rq.Id);

            if (data != null)
            {
                // Tìm trọng tải của đơn hàng
                var weight = data.Weight;

                // Lấy danh sách xe phù hợp với trọng tải yêu cầu
                var trucks = _context.Trucks
                    .Where(t => t.Capacity > weight)
                    .Select(t => new
                    {
                        TruckId = t.TruckId,
                        DriverName = t.Driver.FullName,
                        Capacity = t.Capacity,
                        FuelType = t.FuelType,
                        ParkingLocation = t.ParkingLocation
                    })
                    .ToList();

                // Trả về dữ liệu xe dưới dạng JSON
                return Json(trucks);
            }

            return NotFound(); // Nếu không tìm thấy đơn hàng
        }
        [HttpPost]
        public async Task<bool> UpdateStatus([FromBody] UpdateStatusRequest request)
        {
            if (request != null)
            {
                var requestt =  _context.ShippingRequests.Find(request.RequestId);
                if (requestt != null)
                {
                    requestt.Status = "Đã điều phối xe";
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        [HttpPost]
        public IActionResult Vehicleroute([FromBody] GetRoute model)
        {
          
            if (model != null)
            {
                // timf danh sách các ycvc của xe
                var request = _context.DispatchAssignments.Where(x => x.TruckId == model.TruckId).Select(t => new
                {
                    PickupLat = t.PickupLat,
                    PickupLng = t.PickupLng,
                    DropoffLat = t.DropoffLat,
                    DropoffLng = t.DropoffLng
                }).ToList();
                return Json(request);
            }

            return NotFound();
        }
        [HttpGet]
        public async Task< IActionResult> Filter(DateOnly keyword)
        {
            var data = await _context.ShippingRequests
                 .Where(p => p.RequestDate == keyword)
                 .ToListAsync();

            // Trả dữ liệu về view
            return Ok(data);
        }
        public class GetRoute
        {
            public int TruckId { get; set; }
        }
        public class UpdateStatusRequest
        {
            public int RequestId { get; set; }
        }

        public class AssignmentRequest
        {
            public int Id { get; set; }
        }


    }
}


