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

namespace TransportationWAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private IConfiguration _configuration;  // configuration có sẵn được sử dụng để truy cập các cài đặt cấu hình, chẳng hạn như các chuỗi kết nối hoặc các khóa bảo mật
        private IAccountRepo _accountRepo;
        public ConfigController(IConfiguration configuration, IAccountRepo accountRepo)
        {
            _configuration = configuration;
            _accountRepo = accountRepo;
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

                if(validTo <= DateTime.Now) // nếu token hết hạn
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
