using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Security.Claims;
using Transportation.Domain.Entity;
using Transportation.Domain.Interfaces;

namespace TransportationWAPI.Fillter
{
    public class AuthorizeAttribute : TypeFilterAttribute
    {//Khi bạn sử dụng [Authorize("FunctionCode", "PermisionName")] trên một controller action, ASP.NET Core sẽ hiểu rằng cần sử dụng DemoAuthorizeActionFilter và truyền FunctionCode và PermisionName vào filter đó.
        public AuthorizeAttribute(string FunctionCode, string PermisionName) : base(typeof(DemoAuthorizeActionFilter))
        {
            Arguments = new object[] { FunctionCode, PermisionName };

        }
    }

    public class DemoAuthorizeActionFilter : IAsyncAuthorizationFilter
    {
        private readonly string _functionCode;
        private readonly string _permission;

        private IConfiguration _configuration;
        private readonly IAccountRepo _accountRepo;
        public DemoAuthorizeActionFilter(IAccountRepo accountRepo, IConfiguration configuration, string functionCode, string permission)
        {
            _configuration = configuration;
            _accountRepo = accountRepo;
           _functionCode = functionCode;
            _permission = permission;
        }
        //phương thức chính xử lý xác thực và ủy quyền khi người dùng gọi một API.
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Bước 1: Trích xuất thông tin từ token
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;//Xác định danh tính của người dùng dựa trên các claim trong token.
            if (identity != null)
            {
                var userClaims = identity.Claims;
                //Bước 1 : Lấy htoong tin user từ token
                var user = new Users
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                    Id = Convert.ToInt32(userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value)

                };
                //Bước 2:xác nhận tính hợp lệ của người dùng
                if (user.Id <= 0)// Kiểm tra nếu người dùng không hợp lệ
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
                // lấy functionId dựa vào functionCode
                var function =  await _accountRepo.GetFunction(_functionCode);
                if (function == null || function.FunctionId <=0) {
                    context.HttpContext.Response.ContentType = "application/json";
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Result = new JsonResult(new
                    {
                        Code = HttpStatusCode.Unauthorized,
                        Message = "Bạn không có quyền"
                    });
                    return;

                }
                // Bước 3: lấy userfunction dựa vào userid, functionId, permisstion,
                var userFunction = await _accountRepo.GetUserFunction(user.Id, function.FunctionId, _permission);
                if (userFunction == null || userFunction.FunctionId <= 0)
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
                //Bước4 : kiểm tra kết quả
                switch (_permission)
                {
                    case "IsView":
                        if(userFunction.IsView == 0)
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Message = "Bạn không có quyền xem danh sách"
                            });
                            return;
                        }
                        break;    
                    case "IsUpdate":
                        if (userFunction.IsUpdate == 0)
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Message = "Bạn không có quyền cập nhật danh sách"
                            });
                            return;
                        }
                        break;
                    case "IsCreate":
                        if (userFunction.IsCreate == 0)
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Message = "Bạn không có quyền tạo mới"
                            });
                            return;
                        }
                        break;
                    case "IsDelete":
                        if (userFunction.IsDelete == 0)
                        {
                            context.HttpContext.Response.ContentType = "application/json";
                            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Result = new JsonResult(new
                            {
                                Code = HttpStatusCode.Unauthorized,
                                Message = "Bạn không có quyền xóa"
                            });
                            return;
                        }
                        break;
                
                }
            }

        }
    }

}
