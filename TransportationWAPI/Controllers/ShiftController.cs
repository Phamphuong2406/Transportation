using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Transportation.Application.DTO;
using Transportation.Domain.Interfaces;
using Transportation.Infrastructure.Data;
using TransportationWAPI.Fillter;

namespace TransportationWAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        protected IShiftRepository _shiftRepository;
        public ShiftController(IShiftRepository shiftRepository, IConfiguration configuration, IDistributedCache cache)
        {
            _shiftRepository = shiftRepository;
            _cache = cache;
        }

        [HttpGet("Get_Shift")]
        /*[Authorize("SELECT_SHIFTS", "IsView")]*/
        public async Task<IActionResult> Index()
        {
            var list = new List<Shift>();
            try
            {
                var cacheKey = "GetShift_Caching";
                byte[] cachedData = await _cache.GetAsync(cacheKey);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    list = JsonConvert.DeserializeObject<List<Shift>>(cachedDataString);
                }
                else
                {
                    list = _shiftRepository.GetAll();
                    // Lưu cache
                    var cacheDataString = JsonConvert.SerializeObject(list);
                    var dataToCache = Encoding.UTF8.GetBytes(cacheDataString);

                    DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                        .SetAbsoluteExpiration(DateTime.Now.AddMinutes(5))
                        .SetSlidingExpiration(TimeSpan.FromSeconds(30));

                    await _cache.SetAsync(cacheKey, dataToCache, options);

                    
                }
                return Ok(list);

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost("Create_EditShift")]
        public IActionResult Create_EditShift([FromBody] ShiftDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState); // Trả về lỗi 400 nếu dữ liệu không hợp lệ
                }

                var result = _shiftRepository.Create_Edit(model);

                if (result == -2)
                {
                    return NotFound("Không tìm thấy ca làm việc cần cập nhật.");
                }
                if (result == 0)
                {
                    return NoContent(); // Không có thay đổi nào
                }

                return Ok(new { message = "Thành công", affectedRows = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Lỗi máy chủ", error = ex.Message });
            }
        }




    }
}
