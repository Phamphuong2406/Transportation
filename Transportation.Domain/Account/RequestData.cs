using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.Account
{
    public class RequestData
    {
    }

    public class AccountLoginRequestData //Nhận dữ liệu đăng nhập từ client gửi lên API
    {
        public string? UserName { get; set; } 
        public string? Password { get; set; } 
    }

    public class AccountUpdateRefeshTokenRequestData //Cập nhật RefreshToken khi tạo token mới
    {
        public int Id { get; set; }
        public string? RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExprired { get; set; }
    }
   


}
