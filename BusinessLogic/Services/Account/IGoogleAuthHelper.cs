using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Account
{
    public interface IGoogleAuthHelper
    {
        string[] Getscopes();
        string ScopeToString();
        ClientSecrets GetClienrSecrets();

    }
    //giúp hỗ trợ xác thực OAuth 2.0 với Google
    public class GoogleAuthHelperService(IConfiguration config) : IGoogleAuthHelper
    {
        public ClientSecrets GetClienrSecrets()//Lấy ClientSecrets từ cấu hình
        {
            string clientId = config["Google:ClientId"]!;
            string clientSecret = config["Google:ClientSecret"]!;
            return new() { ClientId = clientId, ClientSecret = clientSecret };//Trả về đối tượng ClientSecrets dùng để xác thực với Google.
        }
        //1. Xác định quyền truy cập
        public string[] Getscopes()//Trả về danh sách quyền truy cập
        {
            var scopes = new[]
            {
                Oauth2Service.Scope.Openid, //giao thức Xác thực OpenID
                Oauth2Service.Scope.UserinfoEmail,//Truy cập email của người dùng.
                Oauth2Service.Scope.UserinfoProfile//Truy cập thông tin hồ sơ người dùng.
            };
            return scopes;//Google OAuth sẽ yêu cầu các quyền này khi người dùng đăng nhập.
        }
        // Chuyển danh sách Scopes thành chuỗi
        public string ScopeToString() => string.Join(", ", Getscopes());

    }
}
