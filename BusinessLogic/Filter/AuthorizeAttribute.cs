using BusinessLogic.Services.Account;
using DataAccess.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;

using System.Net;
using System.Security.Claims;
using System.Web.Http.Results;


namespace BusinessLogic.Filter
{

    public class AuthorizeAttribute : TypeFilterAttribute
    {//Khi bạn sử dụng [Authorize("FunctionCode", "PermisionName")] trên một controller action, ASP.NET Core sẽ hiểu rằng cần sử dụng DemoAuthorizeActionFilter và truyền FunctionCode và PermisionName vào filter đó.
        public AuthorizeAttribute(string RoleName) : base(typeof(DemoAuthorizeActionFilter))
        {
            Arguments = new object[] { RoleName };

        }
    }

    public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _rolename;

        private IConfiguration _configuration;
        private readonly IAccountService _accountService;
        public DemoAuthorizeActionFilter(IAccountService accountService, IConfiguration configuration, string rolename)
        {
            _configuration = configuration;
            _accountService = accountService;
            _rolename = rolename;
        }
        //phương thức chính xử lý xác thực và ủy quyền khi người dùng gọi một API.
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Bước 1: Trích xuất thông tin từ token
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;//Xác định danh tính của người dùng dựa trên các claim trong token.
            if (identity != null)
            {
                var userClaims = identity.Claims;
                //Bước 1 : Lấy htoong tin user từ httpcontext
                var user = new Users
                {
                    Username = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    UserId = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value)

                };
                //Bước 2:xác nhận tính hợp lệ của người dùng
                if (user.UserId <= 0)// Kiểm tra nếu người dùng không hợp lệ
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Vui lòng đăng nhập để thực hiển chức năng này "
                    });
                    return;
                }
                // lấy RoleId dựa vào Rolename
                var Role = await _accountService.GetRole(_rolename);
                if (Role == null || Role.RoleId <= 0)
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Bạn không có quyền"
                    });
                    return;

                }
                // Bước 3: lấy userRole dựa vào userid, RoleId
                var userRole = await _accountService.GetUserRole(user.UserId, Role.RoleId); // nếu tìm được đối tượng có id dùng hiện tại và roleId truyền vào thì trra về 
                if (userRole == null )
                {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Bạn không có quyền"
                    });
                    return;

                }
            
            }

        }
    }

}
