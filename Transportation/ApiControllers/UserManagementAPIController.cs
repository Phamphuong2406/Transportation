using BusinessLogic.DTOs;
using BusinessLogic.Public;
using BusinessLogic.Services;
using DataAccess.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagementAPIController : ControllerBase
    {
        private IUserService _userService;
        public  UserManagementAPIController(IUserService userService)
        {
            _userService = userService;
        }
        //get user
        [HttpGet("GetAllUser")]
        public IActionResult GetAll() {
            var user = _userService.GetAllUser();

            return Ok(user);
        }
        // sửa user
        // xóa
    }
}
