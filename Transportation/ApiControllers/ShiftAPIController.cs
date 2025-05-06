using BusinessLogic.DTOs;
using BusinessLogic.Filter;
using BusinessLogic.Services;
using DataAccess.Entity;
using DataAccess.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;


namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize("Admin")]
    public class ShiftAPIController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        protected IShiftRepository _shiftRepository;
        protected IShiftService _shiftService;
        public ShiftAPIController(IShiftRepository shiftRepository, IDistributedCache cache, IShiftService shiftService)
        {
            _shiftRepository = shiftRepository;
            _cache = cache;
            _shiftService = shiftService;
        }

        [HttpGet("Get_Shift")]

        public async Task<IActionResult> Index()
        {
            var list = new List<ShiftDTO>();
            try
            {
                var cacheKey = "GetShift_Caching";
                byte[] cachedData = await _cache.GetAsync(cacheKey);
                if (cachedData != null)
                {
                    var cachedDataString = Encoding.UTF8.GetString(cachedData);
                    list = JsonConvert.DeserializeObject<List<ShiftDTO>>(cachedDataString);
                }
                else
                {
                    list = _shiftService.GetAllShifts();
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

        [HttpGet("GetShiftById")]
        public IActionResult GetShiftById(int shiftId)
        {
            var data = _shiftService.GetShiftById(shiftId);
            return Ok(data);
        }

        [HttpGet("GetAllShift")]
        public IActionResult GetAllShift()
        {
            return Ok(_shiftService.GetAllShifts());
        }
      
        [HttpPost("CreateShift")]
        public IActionResult CreateShift([FromBody] ShiftDTO model)
        {
            if (ModelState.IsValid)
            {
               return Ok(_shiftService.CreateOrUpdateShift(model));
            }
            return BadRequest();
        }

       
        [HttpPut("EditShift")]
        public IActionResult EditShift([FromBody] ShiftDTO model)
        {

            return Ok(_shiftService.CreateOrUpdateShift(model));

        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int shiftId)
        {
           _shiftService.DeleteShift(shiftId);
            return Ok();
        }

    }
}
