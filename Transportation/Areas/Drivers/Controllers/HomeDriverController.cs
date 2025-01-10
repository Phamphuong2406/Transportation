using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using Transportation.Hubs;
using Transportation.Infrastructure.Data;
using static Transportation.Areas.Coordinator.Controllers.CoordinationController;


namespace Transportation.Areas.Drivers.Controllers
{
    [Area("Drivers")]
    [Authorize(Roles = "Driver")]
    public class HomeDriverController : Controller
    {
        private MyDbContext _context;
        private readonly IHubContext<TrackingHub> _hubContext;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public HomeDriverController(MyDbContext context, IHubContext<TrackingHub> hubContext, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _hubContext = hubContext;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            var userClaim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserID");
            if (userClaim != null && long.TryParse(userClaim.Value, out long iduser)) // nếu tồn tại người dùng và khi chuyển giá trị của người dùng đó sang dạng long mà nhận được giá trị là idusser chứu k phải null
            {
                var user = _context.Drivers.SingleOrDefault(x => x.UserId == (int)iduser); // tìm người dùng theo giá trị value  trả về 
                                                                                           //nếu người dùng có trong database
                if (user != null)
                {

                    return View(_context.DispatchAssignments.Include(tr => tr.Truck)
                                                            .Include(cl => cl.Request)
                                                              .ThenInclude(c => c.Customer)
                                                            .Where(u => u.Truck.DriverId == user.DriverId).ToList());


                }
            }

            return View();
        }
        [HttpGet]

        public IActionResult Detail(int Id)
        {
            var data = _context.DispatchAssignments.Include(x => x.Truck).SingleOrDefault(x => x.AssignmentId == Id);
            if (data != null)
            {

                return View(data);
            }
            return NotFound();

        }
        public IActionResult Realtime()
        {
           

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateLocation([FromBody] RealTimeTracking model)
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
                        // Cập nhật thông tin vị trí
                        model.Timestamp = DateTime.Now;
                        model.TruckId = truck.TruckId;

                        _context.RealTimeTrackings.Add(model);
                        _context.SaveChanges();

                        // Gửi thông báo qua SignalR
                        await _hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", new
                        {
                            TruckId = truck.TruckId,
                            CurrentLat = model.CurrentLat,
                            CurrentLng = model.CurrentLng,
                            Timestamp = model.Timestamp
                        });

                        return Ok(new { success = true, message = "Location updated successfully!" });
                    }
                }
            }

            return BadRequest(new { success = false, message = "Failed to update location." });
        }

        [HttpPost]

        public async Task<IActionResult> UpdateStatus([FromBody] UpdateStatusRequest request)
        {
            if (request == null)
            {
                return BadRequest(new { message = "Dữ liệu không hợp lệ." });
            }
            try
            {
                // Tìm Assignment theo RequestId
                var assignment = await _context.DispatchAssignments
                    .FirstOrDefaultAsync(a => a.RequestId == request.RequestId);

                if (assignment == null)
                {
                    return NotFound(new { message = "Không tìm thấy đơn hàng với mã yêu cầu." });
                }
                var shipping = await _context.ShippingRequests.FirstOrDefaultAsync(a => a.RequestId == request.RequestId);
                if (shipping == null) {
                    return NotFound(new { message = "Không tìm thấy đơn hàng tương ứng" });
                }
                // Cập nhật trạng thái giao hàng (ví dụ: "Đã giao")
                assignment.Status = "Đã giao hàng";
                shipping.Status = "Đã nhận hàng";
               // assignment.DeliveryDate = DateTime.Now; // Cập nhật thời gian giao hàng

                // Lưu thay đổi vào database
                _context.DispatchAssignments.Update(assignment);
                _context.ShippingRequests.Update(shipping);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã xác nhận giao hàng thành công." });
            }
            catch (Exception ex)
            {
                // Log lỗi nếu cần
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi cập nhật trạng thái.", error = ex.Message });
            }

            
        }

        [HttpPost]
        public async Task<IActionResult> UploadImgAndStatus(int RequestId, IFormFile ImageUpload)
        {
            if (RequestId <= 0)
            {
                return BadRequest(new { message = "Mã yêu cầu không hợp lệ." });
            }

            if (ImageUpload == null || ImageUpload.Length == 0)
            {
                return BadRequest(new { message = "Vui lòng tải lên hình ảnh hợp lệ." });
            }

            try
            {
                // Tìm assignment theo RequestId
                var assignment = await _context.DispatchAssignments
                    .FirstOrDefaultAsync(a => a.RequestId == RequestId);

                if (assignment == null)
                {
                    return NotFound(new { message = "Không tìm thấy đơn hàng với mã yêu cầu." });
                }

                var shipping = await _context.ShippingRequests
                    .FirstOrDefaultAsync(s => s.RequestId == RequestId);

                if (shipping == null)
                {
                    return NotFound(new { message = "Không tìm thấy đơn hàng vận chuyển tương ứng." });
                }

                // Đường dẫn thư mục để lưu hình ảnh
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "media/ImgUpload");

                // Tạo thư mục nếu chưa tồn tại
                if (!Directory.Exists(uploadsDir))
                {
                    Directory.CreateDirectory(uploadsDir);
                }

                // Tạo tên file duy nhất
                string imageName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
                string filePath = Path.Combine(uploadsDir, imageName);

                // Sao chép file vào thư mục đích
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(fs);
                }

                // Cập nhật thông tin giao hàng và trạng thái
                assignment.DeliveryImage = imageName;
                assignment.Status = "Đã giao hàng";
                shipping.Status = "Đã nhận hàng";

                // Cập nhật database
                _context.DispatchAssignments.Update(assignment);
                _context.ShippingRequests.Update(shipping);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Đã xác nhận giao hàng thành công." });
            }
            catch (Exception ex)
            {
                // Log lỗi chi tiết (nếu cần)
                return StatusCode(500, new
                {
                    message = "Đã xảy ra lỗi khi xử lý yêu cầu.",
                    error = ex.Message
                });
            }
        }

    }
}
