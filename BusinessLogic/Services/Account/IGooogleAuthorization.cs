using DataAccess.DataContext;
using DataAccess.Entity;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Account
{
    public interface IGooogleAuthorization
    {
        string getAuthorizationUrl();
        Task<UserCredential> ExchangeCodeForToken(string code);
       // Task<UserCredential> ValidateToken(string accessToken);
    }
    //thực hiện xác thực OAuth 2.0 với Google.
    public class GoogleAuthorizationService : IGooogleAuthorization
    {
        private readonly MyDbContext _context;
        private readonly IGoogleAuthHelper _googleAuth;
        private readonly string _redirectUrl;

        public GoogleAuthorizationService(MyDbContext context, IGoogleAuthHelper googleAuth, IConfiguration config)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _googleAuth = googleAuth ?? throw new ArgumentNullException(nameof(googleAuth));
            _redirectUrl = config["Google:RedirectUri"] ?? throw new ArgumentNullException("Google:RedirectUri is missing in configuration");
        }

        public async Task<UserCredential> ExchangeCodeForToken(string code)
        {
            if (string.IsNullOrEmpty(code)) throw new ArgumentException("Authorization code cannot be null or empty.");

            var flow = new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = _googleAuth.GetClienrSecrets(),
                Scopes = _googleAuth.Getscopes(),
            });

            var token = await flow.ExchangeCodeForTokenAsync("user", code, _redirectUrl, CancellationToken.None);
            var payload = await GoogleJsonWebSignature.ValidateAsync(token.IdToken);

            if (payload == null) throw new InvalidOperationException("Invalid Google token payload.");

            var googleUserId = payload.Subject;
            if (string.IsNullOrEmpty(googleUserId)) throw new InvalidOperationException("Google User ID is null or empty.");
            var email = payload.Email;
            if (_context.Users.Any(c => c.GoogleUserId == null))
            {
                Console.WriteLine("Cảnh báo: Cơ sở dữ liệu có giá trị NULL trong GoogleUserId!");
            }

            var existingUser = await _context.Users
     .Where(c => c.GoogleUserId != null && c.GoogleUserId == googleUserId)
     .FirstOrDefaultAsync();

            Console.WriteLine($"Existing User: {existingUser?.GoogleUserId ?? "NULL"}");
            if (existingUser != null)
            {
                //existingUser.RefreshToken = token.RefreshToken;
                existingUser.IdToken = token.IdToken;
               // existingUser.RefreshTokenExprired = token.IssuedUtc;
                _context.Users.Update(existingUser);
            }
            else
            {
                _context.Users.Add(new Users
                {
                    Username = payload.FamilyName,
                    Email = payload.Email,
                    GoogleUserId = googleUserId,
                    IdToken = token.IdToken,
                    PasswordHash = "12345",
                   
                });
            }
            await _context.SaveChangesAsync();

            return new UserCredential(flow, "user", token);
        }

        public string getAuthorizationUrl()
        {
            return new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
            {
                ClientSecrets = _googleAuth.GetClienrSecrets(),
                Scopes = _googleAuth.Getscopes(),
                Prompt = "consent"
            }).CreateAuthorizationCodeRequest(_redirectUrl).Build().ToString();
        }
    


    //=>Trả về URL để chuyển hướng người dùng đến Google


    //3. Xác thực Access Token
    /*   public async Task<UserCredential> ValidateToken(string accessToken)
       {
           var _credential = await context.Credential.FirstOrDefaultAsync(c => c.AccessToken == accessToken) ??// Tìm Access Token trong database.
               throw new UnauthorizedAccessException("No Authentication token found. Please login again");
           // Tạo UserCredential từ database để xác thực người dùng.
           var flow = new GoogleAuthorizationCodeFlow(
               new GoogleAuthorizationCodeFlow.Initializer
               {
                   ClientSecrets = googleAuth.GetClienrSecrets(),
                   Scopes = googleAuth.Getscopes(),
               });
           var tokenResponse = new TokenResponse
           {
               AccessToken = _credential.AccessToken,
               RefreshToken = _credential.RefreshToken,
               ExpiresInSeconds = _credential.ExpiresInSeconds,
               IdToken = _credential.IdToken,
               IssuedUtc = _credential.IssuedUtc,
           };
           return new UserCredential(flow, "user", tokenResponse);

       }*/
}
}