using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Transportation.Domain.Account;
using Transportation.Domain.Interfaces;
using TransportationWAPI.Logging;

namespace TransportationWAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private IConfiguration _configuration;  // configuration có sẵn được sử dụng để truy cập các cài đặt cấu hình, chẳng hạn như các chuỗi kết nối hoặc các khóa bảo mật
        private IAccountRepo _accountRepo;
        private ILoggerManager _loggerManager;
        public AccountController(IConfiguration configuration, IAccountRepo accountRepo, ILoggerManager loggerManager)
        {
            _configuration = configuration;
            _accountRepo = accountRepo;
            _loggerManager = loggerManager;
        }

        [HttpPost("AccountLogin")]
        public async Task<ActionResult> AccountLogin(AccountLoginRequestData requestData)
        {
            var returnData = new ReturnData();//Lớp để ánh xạ cấu hình JWT từ appsettings.json.
            var logID = DateTime.Now.Ticks;
            try
            {  
                _loggerManager.LogInfo("logID: " + logID + "| "+ DateTime.Now.ToString("dd/MM/yyyy hh:MM:ss") + "| requetData:" + returnData);
                // Bước 1: kiểm tra yêu cầu đăng nhập
                if (requestData == null || string.IsNullOrEmpty(requestData.UserName) || string.IsNullOrEmpty(requestData.Password))//// Kiểm tra yêu cầu có null hoặc rỗng không
                { return BadRequest("Yêu cầu không hơpj lệ "); }
                //Bước 2: xác nhận thông tin đăng nhập
                var useLogin = await _accountRepo.Login_Account(requestData);// login user với hàm User_Login để xác thực thông tin đăng nhập
                if (useLogin == null || useLogin.Id <= 0) // <=0 là k tìm thấy id nào mong muốn
                {
                    returnData.ResponseCode = -1;
                    returnData.ResponseMessage = "Đăng nhập thất bại ";
                    return Ok(returnData);
                }
                //Bước 3: Tạo token và refresh token
                var authClaims = new List<Claim> { // tạo danh sách claim cho token 
            new Claim(ClaimTypes.NameIdentifier, useLogin.UserName) ,
            new Claim(ClaimTypes.PrimarySid, useLogin.Id.ToString()),
            new Claim(ClaimTypes.GivenName, useLogin.FullName.ToString())   };
                var newAccessToken = CreateToken(authClaims);//Gọi phương thức CreateToken để tạo JWT token.
                                                             // tạo token
                var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                var refreshToken = GenerateRefreshToken();//Tạo refresh token bằng phương thức GenerateRefreshToken.
                                                          //Bước 4: update refeshToken vào db
                var expired = _configuration["JWT:RefeshTokenValidityInDays"] ?? "";
                //Cập nhật refresh token và ngày hết hạn vào cơ sở dữ liệu.
                var result_update = _accountRepo.AccountUpdateRefreshToken(new AccountUpdateRefeshTokenRequestData
                {
                    Id = useLogin.Id,
                    RefreshToken = refreshToken,
                    RefreshTokenExprired = DateTime.Now.AddDays(Convert.ToInt32(expired))
                });
                //Bước 5: Trả về token và refresh token cho người dùng.
                returnData.ResponseCode = 1;
                returnData.ResponseMessage = "Đăng nhập thành công";
                returnData.token = token;// trả về access token 
                returnData.refeshToken = refreshToken;//Trả về refresh token cho người dùng.
                return Ok(returnData);
            }
            catch (Exception ex)
            {
                returnData.ResponseCode = -1;
                returnData.ResponseMessage = ex.Message;
                return Ok(returnData);
            }
        }

        //  Phương thức này đảm bảo rằng các token đã hết hạn có thể được kiểm tra và làm mới một cách an toàn và hợp lệ.
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

                if (validTo <= DateTime.Now) // nếu token hết hạn
                {
                    //Bước 2: giải mã token dựa vào secretkey đã config trước đó để lấy thông tin người dùng

                    var principal = GetPrincipalFromExpiredToken(accessToken);// giải mã token
                    if (principal == null)
                    {
                        return BadRequest();
                    }
                    // bước 3 ;lây user name từ claim trong token
                    string username = principal?.Claims.ToList()[0].ToString()?.Split(' ')[1];
                    // bước 4: Kiểm tra xem refreshtoken đã hết hạn chưa 
                    var user_db = _accountRepo.GetAll().Result.Where(s => s.UserName == username).FirstOrDefault();
                    if (user_db == null || user_db.RefreshTokenExprired <= DateTime.Now)
                    {
                        return BadRequest("refresh token hết hạn. Vui lòng đăng nhập lại");
                    }
                    // Tạo token mới( giống login)
                    //Bước 3: Tạo token và refresh token
                    var authClaims = new List<Claim> { // tạo danh sách claim cho token 
            new Claim(ClaimTypes.NameIdentifier, user_db.UserName) ,
            new Claim(ClaimTypes.PrimarySid, user_db.Id.ToString()),
            new Claim(ClaimTypes.GivenName, user_db.FullName.ToString())   };
                    var newAccessToken = CreateToken(authClaims);//Gọi phương thức CreateToken để tạo JWT token.
                                                                 // tạo token
                    var token = new JwtSecurityTokenHandler().WriteToken(newAccessToken);
                    var refreshToken = GenerateRefreshToken();//Tạo refresh token bằng phương thức GenerateRefreshToken.
                                                              //Bước 4: update refeshToken vào db
                    var expired = _configuration["JWT:RefeshTokenValidityInDays"] ?? "";
                    //Cập nhật refresh token và ngày hết hạn vào cơ sở dữ liệu.
                    var result_update = _accountRepo.AccountUpdateRefreshToken(new AccountUpdateRefeshTokenRequestData
                    {
                        Id = user_db.Id,
                        RefreshToken = refreshToken,
                        RefreshTokenExprired = DateTime.Now.AddDays(Convert.ToInt32(expired))
                    });
                    //Bước 5: Trả về token và refresh token cho người dùng.
                    returnData.ResponseCode = 1;
                    returnData.ResponseMessage = "Đăng nhập thành công";
                    returnData.token = token;// trả về access token 
                    returnData.refeshToken = refreshToken;//Trả về refresh token cho người dùng.
                    return Ok(returnData);

                }


                // Nếu mọi thứ hợp lệ, trả về access token
                return Ok(accessToken);
            }
            catch (Exception)
            {

                throw;
            }
            return Ok();
        }
        // Phương thức để giải mã token và lấy thông tin principal
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
      
       




    }
}
