using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transportation.Domain.Account
{
    public class ResponseData
    {
    }
    public class ReturnData
    {
        public int ResponseCode { get; set; } //mã lỗi
        public string ResponseMessage { get; set; } // thông báo lỗi
        public string token { get; set; }
        public string? refeshToken { get; set; }
    }

    public class Response
    {
        public string Status { get; set; }
        public string Message { get; set; }
    }
}
