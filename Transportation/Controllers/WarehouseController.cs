using BusinessLogic.DTOs;
using DataAccess.DataContext;
using DataAccess.Entity;
using ExcelDataReader;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Transportation.Controllers
{
 
    public class WarehouseController : Controller
    {
        private MyDbContext _context;
        public WarehouseController(MyDbContext context)
        {
            _context = context;

        }
        private Customer GetCurrentCustomer()
        {
            var userIDClaim = HttpContext.User.Claims.SingleOrDefault(id => id.Type == "UserID");

            if (userIDClaim != null && long.TryParse(userIDClaim.Value, out long userID))
            {
                return _context.Customers.SingleOrDefault(x => x.UserId == (int)userID);
            }

            return null;
        }

        public IActionResult Index()
        {
           
            return View();
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var warehouse = _context.Warehouses.Find(id);
            if (warehouse == null)
            {
                return Json(new { success = false, errorMessage = "Kho không tồn tại!" });
            }

            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();

            return Json(new { success = true });
        }


        public IActionResult GetWarehouseListPartial()
        {
            var warehouses = _context.Warehouses.ToList();
            return PartialView("_WarehouseListPartial", warehouses);
        }


        [HttpPost]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("", "Vui lòng chọn file hợp lệ.");
                return View();
            }
            try
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
                                if (!isHeaderSkipped) // bỏ qua dòng tiêu đề
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                // Kiểm tra nếu dòng hiện tại hoàn toàn trống
                                if (IsEmptyRow(reader))
                                    break; // Dừng vòng lặp khi gặp dòng trống


                                Warehouse s = new Warehouse();
                                s.Name = reader.GetValue(0)?.ToString();
                                s.Address = reader.GetValue(1)?.ToString();
                                s.Latitude = Convert.ToDecimal(reader.GetValue(2)?.ToString());
                                s.Longitude = Convert.ToDecimal(reader.GetValue(3)?.ToString());
                                s.Capacity = Convert.ToInt32(reader.GetValue(4)?.ToString());
                                s.IsActive = Convert.ToBoolean(reader.GetValue(5)?.ToString());

                                string closingTimeString = reader.GetValue(6)?.ToString();
                                DateTime closingDateTime = DateTime.Parse(closingTimeString);
                                s.ClosingTime = TimeOnly.FromTimeSpan(closingDateTime.TimeOfDay);

                                string openTimeString = reader.GetValue(7)?.ToString();
                                DateTime openDateTime = DateTime.Parse(openTimeString);
                                s.OpeningTime = TimeOnly.FromTimeSpan(openDateTime.TimeOfDay);

                                s.CustomerId = Convert.ToInt32(reader.GetValue(8)?.ToString());

                                _context.Warehouses.Add(s);
                                await _context.SaveChangesAsync();
                               
                            }

                           
                        } while (reader.NextResult());


                        return RedirectToAction("Index");
                    }
                   
                }
               
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Lỗi khi xử lý file: " + ex.Message);
                return View();
            }
        }

        private bool IsEmptyRow(IExcelDataReader reader)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetValue(i) != null && !string.IsNullOrWhiteSpace(reader.GetValue(i)?.ToString()))
                {
                    return false; // Dòng có dữ liệu, tiếp tục đọc
                }
            }
            return true; // Dòng hoàn toàn trống
        }

    }
}
