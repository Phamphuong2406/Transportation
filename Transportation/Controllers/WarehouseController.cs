using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Transportation.Application.DTO;
using Transportation.Infrastructure.Data;

namespace Transportation.Controllers
{
    [Authorize(Roles = "Customer")]
    public class WarehouseController : Controller
    {
        private MyDbContext _context;
        public WarehouseController(MyDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var userIDClaim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == "UserID");

            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null && long.TryParse(userIDClaim.Value, out long userID))
            {
                var customer = _context.Customers.SingleOrDefault(x => x.UserId == (int)userID);

                if (customer != null)
                {

                   var data = _context.Warehouses.Where(u => u.CustomerId == customer.CustomerId).Select(x => new WarehouseDto
                   {
                       WarehouseId = x.WarehouseId,
                       Name = x.Name,
                       Address = x.Address,
                       Latitude = x.Latitude,
                       Longitude = x.Longitude,
                       Capacity = x.Capacity,
                       IsActive = x.IsActive,
                      ClosingTime = x.ClosingTime,
                     OpeningTime = x.OpeningTime
                   }).ToList();    

                    return View(data);

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

           

            return View();
        }
       

        public IActionResult Create()
        {
            var data = new Warehouse();


       

            return View(data);

        }
        [HttpPost]
        public IActionResult Create(Warehouse model)
           
        {
            var userIDClaim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == "UserID");

            // Kiểm tra nếu claim tồn tại và userID hợp lệ
            if (userIDClaim != null && long.TryParse(userIDClaim.Value, out long userID))
            {
                var customer = _context.Customers.SingleOrDefault(x => x.UserId == (int)userID);

                if (customer != null)
                {
                    
                    model.CustomerId = customer.CustomerId; // Gán CustomerId từ bảng Customer

                    // Thêm vào cơ sở dữ liệu
                    _context.Warehouses.Add(model);
                    _context.SaveChanges();

                    return View();

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
      
        public IActionResult GetWarehouseListPartial()
        {
            var warehouses = _context.Warehouses.ToList();
            return PartialView("_WarehouseListPartial", warehouses);
        }


        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                var filePath = Path.Combine(uploadFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);

                }

                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        do
                        {
                            bool isHeaderSkipped = false;

                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                Warehouse s = new Warehouse();
                                s.Name = reader.GetValue(0).ToString();
                                s.Address = reader.GetValue(1).ToString();
                                s.Latitude = Convert.ToDecimal(reader.GetValue(2).ToString());
                                s.Longitude = Convert.ToDecimal(reader.GetValue(3).ToString());
                                s.Capacity = Convert.ToInt32(reader.GetValue(4).ToString());
                                s.IsActive = Convert.ToBoolean(reader.GetValue(5).ToString());
 
                                string closingTimeString = reader.GetValue(6).ToString();
                                DateTime closingDateTime = DateTime.Parse(closingTimeString);
                                s.ClosingTime = TimeOnly.FromTimeSpan(closingDateTime.TimeOfDay);

                                string openTimeString = reader.GetValue(7).ToString();
                                DateTime openDateTime = DateTime.Parse(openTimeString);
                                s.OpeningTime = TimeOnly.FromTimeSpan(openDateTime.TimeOfDay);

                                s.CustomerId = Convert.ToInt32(reader.GetValue(8).ToString());

                                _context.Warehouses.Add(s);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        ViewBag.Message = "success";
                    }
                }

            }
            return View();
        }

    }
}
