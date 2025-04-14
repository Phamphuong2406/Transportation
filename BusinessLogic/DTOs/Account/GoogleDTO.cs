using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Account
{
    public class GoogleDTO
    {
        public record Token(string AccesToken, string UserId);
    }
    public static class Constant
    {
        public const string Scheme = "GoogleAccessToken";
        public const string Key = "Credentity";
        public const string Client = "ApiClient";
    }
}
