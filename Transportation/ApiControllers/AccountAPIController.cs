using Azure.Core;
using BusinessLogic.DTOs.Account;
using BusinessLogic.DTOs.SendEmail;
using BusinessLogic.Public;
using BusinessLogic.Services;
using BusinessLogic.Services.Account;
using DataAccess.Entity;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Transportation.ApiControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountAPIController : ControllerBase
    {
        private IConfiguration _configuration;  // configuration có sẵn được sử dụng để truy cập các cài đặt cấu hình, chẳng hạn như các chuỗi kết nối hoặc các khóa bảo mật
        private IAccountService _accountService;
        private IGooogleAuthorization _googleAuthorization;
        private readonly IEmailSender _emailSender;
        // private ILoggerManager _loggerManager;
        public AccountAPIController(IConfiguration configuration, IAccountService accountService, IGooogleAuthorization googleAuthorization/*Gọi API xác thực gg*/, IEmailSender emailSender)//, ILoggerManager loggerManager)
        {
            _configuration = configuration;
            _accountService = accountService;
            _googleAuthorization = googleAuthorization;
            _emailSender = emailSender;
            // _loggerManager = loggerManager;
        }
        [HttpPost("OTPAuthenticationAndAccountCreation")]
        public async Task<ActionResult> OTPAuthenticationAndAccountCreation([FromBody] AuthenOtpDTO authenOtp)
        {
            var responseData = new Response();
          
            try
            {
                //xác thực otp
                var data = await _emailSender.VerifyOtpAsync(authenOtp.Otp, authenOtp.Email);
                if(data == false)
                {
                    return BadRequest("Xác thực OTP thất bại");
                }
                var result = await _accountService.Register_Account(authenOtp.Email);

                // Nếu user đã tồn tại hoặc có lỗi khi tạo
                if (result == null)
                {
                    responseData.Status = "409"; 
                    responseData.Message = "Tài khoản đã tồn tại";
                    return Conflict(responseData);
                }

                responseData.Status = "201";
                responseData.Message = "Tạo tài khoản thành công";
                return Ok();
            }
            catch (Exception ex)
            {
                responseData.Status = "500"; // 500 Internal Server Error
                responseData.Message = "Đã xảy ra lỗi: " + ex.Message;
                return StatusCode(500, responseData);
            }
        }
        [HttpPost("SendOTP")]
        public async Task<ActionResult> SendOTP(AccountRegisterRequestData registerRequestData)
        {
            
            var otp =await  _emailSender.GenerateOtpForEmailAsync(registerRequestData);
            var message = new Message([registerRequestData.Email], "OTP của banj là ", otp);
            await _emailSender.SendEmailAsync(message);
            return Ok();
        }
   

        [HttpPost("DispatcherRegister")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> DispatcherRegister([FromForm] AccountRegisterRequestData registerRequestData)
        {
            var responseData = new Response();
            try
            {
                var result = await _accountService.Register_Dispatcher(registerRequestData);

                // Nếu user đã tồn tại hoặc có lỗi khi tạo
                if (result == null)
                {
                    responseData.Status = "409";
                    responseData.Message = "Tài khoản đã tồn tại";
                    return Conflict(responseData);
                }

                // Trả về thành công
                responseData.Status = "201";
                responseData.Message = "Tạo tài khoản thành công";
                return Ok();
            }
            catch (Exception ex)
            {
                responseData.Status = "500";
                responseData.Message = "Đã xảy ra lỗi: " + ex.Message;
                return StatusCode(500, responseData);
            }
        }
        [HttpPost("DriverRegister")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> DriverRegister([FromForm] AccountRegisterRequestData registerRequestData)
        {
            var responseData = new Response();
            try
            {
                var result = await _accountService.Register_Driver(registerRequestData);

                // Nếu user đã tồn tại hoặc có lỗi khi tạo
                if (result == null)
                {
                    responseData.Status = "409";
                    responseData.Message = "Tài khoản đã tồn tại";
                    return Conflict(responseData);
                }
                responseData.Status = "201";
                responseData.Message = "Tạo tài khoản thành công";
                return Ok();
            }
            catch (Exception ex)
            {
                responseData.Status = "500";
                responseData.Message = "Đã xảy ra lỗi: " + ex.Message;
                return StatusCode(500, responseData);
            }
        }


        [HttpPost("AccountLogin")]
        public async Task<ActionResult> AccountLogin(AccountLoginRequestData requestData)
        {
            var returnData = new ReturnData();//Lớp để ánh xạ cấu hình JWT từ appsettings.json.
            var logID = DateTime.Now.Ticks;
            try
            {
                //   _loggerManager.LogInfo("logID: " + logID + "| " + DateTime.Now.ToString("dd/MM/yyyy hh:MM:ss") + "| requetData:" + returnData);
                // Bước 1: kiểm tra yêu cầu đăng nhập
                if (requestData == null || string.IsNullOrEmpty(requestData.UserName) || string.IsNullOrEmpty(requestData.Password))//// Kiểm tra yêu cầu có null hoặc rỗng không
                { return BadRequest("Yêu cầu không hơpj lệ "); }
                //Bước 2: xác nhận thông tin đăng nhập
                var useLogin = await _accountService.Login_Account(requestData);// login user với hàm User_Login để xác thực thông tin đăng nhập
                if (useLogin == null || useLogin.UserId <= 0)
                {
                    returnData.ResponseCode = -1;
                    returnData.ResponseMessage = "Đăng nhập thất bại ";
                    return Ok(returnData);
                }
                //Bước 3: Tạo token và refresh token
                var authClaims = new List<Claim> { 
            new Claim(ClaimTypes.NameIdentifier, useLogin.Username) ,
            new Claim(ClaimTypes.PrimarySid, useLogin.UserId.ToString()),
            new Claim(ClaimTypes.GivenName, useLogin.Email.ToString())   };
                var newAccessToken = CreateToken(authClaims);
                var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                var refreshToken = GenerateRefreshToken();
                var expired = _configuration["JWT:RefeshTokenValidityInDays"] ?? "";
                var result_update = _accountService.AccountUpdateRefreshToken(new AccountUpdateRefeshTokenRequestData
                {
                    Id = useLogin.UserId,
                    RefreshToken = refreshToken,
                    RefreshTokenExprired = DateTime.Now.AddDays(Convert.ToInt32(expired))
                });
                // Lưu JWT vào HttpOnly Cookie
                Response.Cookies.Append("AuthToken", token, new CookieOptions
                {
                    HttpOnly = true,   // Không thể đọc bằng JavaScript
                    Secure = true,     // Chỉ gửi qua HTTPS
                    SameSite = SameSiteMode.Strict, // Ngăn chặn CSRF
                    Expires = DateTime.UtcNow.AddHours(1) // Hết hạn sau 1 giờ
                });
                returnData.ResponseCode = 1;
                returnData.ResponseMessage = "Đăng nhập thành công";
                returnData.token = token;
                returnData.refeshToken = refreshToken;
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ResponseCode = -1;
                returnData.ResponseMessage = ex.Message;
                return Ok(returnData);
            }
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // Xóa token trong cookie
            Response.Cookies.Delete("AuthToken");
            return RedirectToAction("Login", "Account");
        }


        [HttpPost("Forgotpassword")]

        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO forgotPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _accountService.FindByEmailAsync(forgotPassword.Email);
            if (user == null) {
                return BadRequest("Invalid Request");
            }
            var authClaims = new List<Claim> { // tạo danh sách claim cho token 
            new Claim(ClaimTypes.NameIdentifier, user.Username) ,
            new Claim(ClaimTypes.PrimarySid, user.UserId.ToString()),
            new Claim(ClaimTypes.GivenName, user.Email.ToString())   };
            var newAccessToken = CreateToken(authClaims);
            var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPassword.Email}
            };
            var callback = QueryHelpers.AddQueryString(forgotPassword.ClientUri, param);
            var message = new Message([user.Email], "Reset password token", callback);
            await _emailSender.SendEmailAsync(message);
            return Ok(token);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = await _accountService.FindByEmailAsync(resetPassword.Email);
            if (user == null) {
                return BadRequest("Invalid Request");
            }
            var principal = GetPrincipalFromExpiredToken(resetPassword.Token);// giải mã token
            if (principal == null)
            {
                return BadRequest();
            }
            // bước 3 ;lây user name từ claim trong token
            string email = principal?.FindFirst(ClaimTypes.GivenName)?.Value;
            var result = _accountService.UpdatePassword(email, resetPassword.Password);
            if(result == "Người dùng không hợp lệ")
            {
                return BadRequest("Yêu cầu thất bại");
            }
            return Ok();
        }
        [HttpGet("Authorize")] //Tạo URL xác thực Google.
        public IActionResult Authorize()
        {
            var returnData = _googleAuthorization.getAuthorizationUrl();
            return Ok(new { redirectUrl = returnData });// Khi gọi API này, nó sẽ trả về URL để người dùng đăng nhập Google.Khi người dùng truy cập URL này, họ sẽ được chuyển hướng đến trang đăng nhập của Google.
        }
        [HttpGet("CallBack")]
        public async Task<IActionResult> CallBack(string code)//Nhận mã code từ Google sau khi người dùng đăng nhập.
        {
            var returnData = new ReturnData();
            var userCredential = await _googleAuthorization.ExchangeCodeForToken(code); // đổi code nhận được lấy Access Token và IdToken
            var payload = await GoogleJsonWebSignature.ValidateAsync(userCredential.Token.IdToken); // Giải mã IdToken lấy GoogleUserId
            //
            var credential = await _accountService.GetByGoogleId(payload.Subject);
            //Bước 3: Tạo token và refresh token
            var authClaims = new List<Claim> { 
            new Claim(ClaimTypes.NameIdentifier, credential.Username) ,
            new Claim(ClaimTypes.PrimarySid, credential.UserId.ToString()),
            new Claim(ClaimTypes.GivenName, credential.Email.ToString())   };
            var newAccessToken = CreateToken(authClaims);
            var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
            var refreshToken = GenerateRefreshToken();
                                                      
            var expired = _configuration["JWT:RefeshTokenValidityInDays"] ?? "";
            var result_update = _accountService.AccountUpdateRefreshToken(new AccountUpdateRefeshTokenRequestData
            {
                Id = credential.UserId,
                RefreshToken = refreshToken,
                RefreshTokenExprired = DateTime.Now.AddDays(Convert.ToInt32(expired))
            });

            // Lưu JWT vào HttpOnly Cookie
            Response.Cookies.Append("AuthToken", token, new CookieOptions
            {
                HttpOnly = true,   // Không thể đọc bằng JavaScript
                Secure = true,     // Chỉ gửi qua HTTPS
                SameSite = SameSiteMode.Strict, // Ngăn chặn CSRF
                Expires = DateTime.UtcNow.AddHours(1) // Hết hạn sau 1 giờ
            });
            //Bước 5: Trả về token và refresh token cho người dùng.
            returnData.ResponseCode = 1;
            returnData.ResponseMessage = "Đăng nhập thành công";
            returnData.token = token;
            returnData.refeshToken = refreshToken;
            return Ok(returnData);
            
        }

        [HttpPost("RefreshToken")]
        public async Task<ActionResult> RefreshToken()
        {
            var returnData = new ReturnData();//Lớp để ánh xạ cấu hình JWT từ appsettings.json.
            try
            {
                // Bước 1: lây access token từ reqquest gưir lên 
                var accessToken = await HttpContext.GetTokenAsync("access_token");  // token gửi lên từ trình duyệt
                if (accessToken == null)
                {
                    return BadRequest("accesstoken không hợp lệ");
                }

                var jwtSecuriryToken = new JwtSecurityToken(accessToken);
                var validTo = jwtSecuriryToken.ValidTo.AddHours(7);

                if (validTo <= DateTime.Now) 
                {
                    //Bước 2: giải mã token dựa vào secretkey đã config trước đó để lấy thông tin người dùng

                    var principal = GetPrincipalFromExpiredToken(accessToken);
                    if (principal == null)
                    {
                        return BadRequest();
                    }
                    string username = principal?.Claims.ToList()[0].ToString()?.Split(' ')[1];
                    // bước 4: Kiểm tra xem refreshtoken đã hết hạn chưa 
                    var user_db = _accountService.GetAll().Result.Where(s => s.Username == username).FirstOrDefault();
                    if (user_db == null || user_db.RefreshTokenExprired <= DateTime.Now)
                    {
                        return BadRequest("refresh token hết hạn. Vui lòng đăng nhập lại");
                    }
                    var authClaims = new List<Claim> {
            new Claim(ClaimTypes.NameIdentifier, user_db.Username) ,
            new Claim(ClaimTypes.PrimarySid, user_db.UserId.ToString()),
            new Claim(ClaimTypes.GivenName, user_db.Email.ToString())   };
                    var newAccessToken = CreateToken(authClaims);
                    var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                    var refreshToken = GenerateRefreshToken();
                    var expired = _configuration["JWT:RefeshTokenValidityInDays"] ?? "";
                    var result_update = _accountService.AccountUpdateRefreshToken(new AccountUpdateRefeshTokenRequestData
                    {
                        Id = user_db.UserId,
                        RefreshToken = refreshToken,
                        RefreshTokenExprired = DateTime.Now.AddDays(Convert.ToInt32(expired))
                    });
                    returnData.ResponseCode = 1;
                    returnData.ResponseMessage = "Đăng nhập thành công";
                    returnData.token = token;
                    returnData.refeshToken = refreshToken;
                    return Ok(returnData);

                }

                return Ok(accessToken);
            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }
     
        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var TokenValidationParameters = new TokenValidationParameters // Dùng TokenValidationParameters để thiết lập các tham số xác thực token.
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])),

            };
            var tokenhandler = new JwtSecurityTokenHandler();//Dùng JwtSecurityTokenHandler để giải mã và xác thực token.
            //Kiểm tra tính hợp lệ của token, nếu không hợp lệ, ném ngoại lệ SecurityTokenException.
            var principal = tokenhandler.ValidateToken(token, TokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }
            return principal;
        }

        // tạo accesstoken dựa vaò các claim đã cung cấp ở trên
        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);
            //Sử dụng khóa bảo mật từ cấu hình và thiết lập thời hạn token.
            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningkey, SecurityAlgorithms.HmacSha256)); // sử dụng thuật toán HmacSha256
            return token;
        }

        // dựa với Tạo một refresh token ngẫu nhiên sử dụng RandomNumberGenerator và trả về dưới dạng chuỗi Base64.
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        [HttpGet("GetAllDispatcher")]
        public IActionResult GetAllDispatcher()
        {
            var listdispatcher = _accountService.GetAllDispatcher();
            return Ok(listdispatcher);
        }

        [HttpGet("GetAllDriver")]
        public IActionResult GetAllDriver()
        {
            var listdriver = _accountService.GetAllDriver();
            return Ok(listdriver);
        }


    }
}
