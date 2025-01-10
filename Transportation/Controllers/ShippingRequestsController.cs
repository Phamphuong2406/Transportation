using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Transportation.Infrastructure.Data;

namespace Transportation.Controllers
{
    [Authorize(Roles = "Customer")]
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
            var userIDClaim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == "UserID");
            ViewBag.productType = new SelectList(_context.ProductTypes, "ProductTypeId", "Name"); // tạo 1 dropdown lựa chọn có value = id và giá trị = tên
            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null && long.TryParse(userIDClaim.Value, out long userID))
            {
                var customer = _context.Customers.SingleOrDefault(x => x.UserId == (int)userID);
                if (customer != null)
                {
                    // Lấy danh sách kho của customer này
                    var warehouses = _context.Warehouses
                        .Where(w => w.CustomerId == customer.CustomerId)
                        .Select(w => new
                        {
                            w.WarehouseId,
                            w.Name,
                            w.Latitude,
                            w.Longitude,
                            w.Address
                        })
                        .ToList();

                    // Gửi danh sách kho vào ViewBag để sử dụng trong view
                    ViewBag.Warehouses = warehouses;

                }
            }

            var request = new ShippingRequest();
            return View(request);
        }
      
        [HttpPost]
        public decimal CalculateShippingCost(decimal distance, decimal weight, int productType)
        {
            // Bước 2: Đặt các thông số phí cơ bản
            decimal basePrice = 50000; // Phí cơ bản
            decimal pricePerKm = 5000; // Giá mỗi km
            decimal pricePerKg = 2000; // Giá mỗi kg

            // Bước 3: Điều chỉnh phí theo loại hàng hóa
            switch (productType)
            {
                case 2: // Hàng dễ vỡ
                    basePrice *= 1.2m; // Tăng phí cơ bản 20%
                    pricePerKg *= 1.5m; // Tăng phí mỗi kg 50%
                    break;

                case 3: // Hàng cồng kềnh
                    pricePerKm *= 1.3m; // Tăng phí mỗi km 30%
                    break;

                case 4: // Hàng thông thường
                        // Không thay đổi, sử dụng các giá trị mặc định
                    break;

                case 5: // Hàng nguy hiểm
                    basePrice *= 1.5m; // Tăng phí cơ bản 50%
                    pricePerKg *= 2m; // Tăng phí mỗi kg gấp đôi
                    break;

                default:
                    throw new ArgumentException("Loại hàng hóa không hợp lệ");
            }

            // Bước 4: Tính tổng phí
            decimal shippingCost = basePrice + (pricePerKm * distance) + (pricePerKg * weight);
            return shippingCost;
        }

        [HttpGet]

        public IActionResult Create()
        {
            var userIDClaim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == "UserID");

            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null && long.TryParse(userIDClaim.Value, out long userID))
            {
                var customer = _context.Customers.SingleOrDefault(x => x.UserId == (int)userID);
                if (customer != null) {
                    // Lấy danh sách kho của customer này
                    var warehouses = _context.Warehouses
                        .Where(w => w.CustomerId == customer.CustomerId)
                        .Select(w => new
                        {
                            w.WarehouseId,
                            w.Name,
                            w.Latitude,
                            w.Longitude
                        })
                        .ToList();
                    // Gửi danh sách kho vào ViewBag để sử dụng trong view
                    ViewBag.Warehouses = warehouses;

                }
            }

                var request = new ShippingRequest();
        
            return View(request);
        }

        [HttpPost]
        public IActionResult Create(ShippingRequest model, [FromForm] decimal FromLatitude, [FromForm] decimal FromLongitude, [FromForm] decimal ToLatitude, [FromForm] decimal ToLongitude)
        {
            var userIDClaim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == "UserID");

            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null && long.TryParse(userIDClaim.Value, out long userID))
            {
                var customer = _context.Customers.SingleOrDefault(x => x.UserId == (int)userID);

                if (customer != null)
                {
                    // Cập nhật các giá trị cho ShippingRequest
                    model.RequestDate = DateOnly.FromDateTime(DateTime.Now);
                    model.PickupLat = FromLatitude;
                    model.PickupLng = FromLongitude;
                    model.DropoffLat = ToLatitude;
                    model.DropoffLng = ToLongitude;
                    model.CustomerId = customer.CustomerId; // Gán CustomerId từ bảng Customer
                    model.Status = "Chờ xác nhận";
                    // Thêm vào cơ sở dữ liệu
                    _context.ShippingRequests.Add(model);
                    _context.SaveChanges();

                    return RedirectToAction("Index");

                }
                else
                {
                    // Thêm lỗi nếu không tìm thấy Customer
                    ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
                }
            }
            else
            {
                // Thêm lỗi nếu không có UserID hợp lệ
                ModelState.AddModelError("", "Không thể xác định tài khoản người dùng.");
            }

            return View(model);
        }

        // Action trả về danh sách yêu cầu vận chuyển dưới dạng JSON
        [HttpGet]
        public JsonResult GetShippingRequests()
        {
            var shippingRequests = _context.ShippingRequests.ToList();
            return Json(shippingRequests);
        }



    }
}
