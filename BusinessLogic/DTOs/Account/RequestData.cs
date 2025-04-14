using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Account
{
    public class RequestData
    {
    }

    public class AccountLoginRequestData // Nhận dữ liệu đăng nhập từ client gửi lên API
    {
        public string? UserName { get; set; } 
        public string? Password { get; set; } 
    }
    public class AccountRegisterRequestData
    {
        public string Username { get; set; } = null!;

        public string Email { get; set; } 

        public string? PhoneNumber { get; set; }

        
        public string PasswordHash { get; set; } = null!;

        public string FullName { get; set; }

        public string? Address { get; set; }
        public string? Department {  get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? Idcard { get; set; }
       public string? HealthStatus { get; set; }
    }

    public class AccountUpdateRefeshTokenRequestData //Cập nhật RefreshToken khi tạo token mới
    {
        public int Id { get; set; }
        public string? RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExprired { get; set; }
    }
   


}
