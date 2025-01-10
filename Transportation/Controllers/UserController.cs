using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
using Transportation.Application.Public;
using Transportation.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Transportation.Domain.ViewModel;


namespace Transportation.Controllers
{
    public class UserController : Controller
    {
        private MyDbContext _myDb;
     private readonly IMapper _mapper;

        public UserController(MyDbContext myDb, IMapper mapper)
        {
            _myDb = myDb;
          _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            var model = new RegisterVM();
            return View(model);
        }
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(model);

               user.RandomKey = Until.GenerateRandomkey();
                user.PasswordHash = model.PasswordHash.ToMd5Hash(user.RandomKey);
                user.RoleId = 2;
                user.IsActive = true;
                _myDb.Users.Add(user);
                _myDb.SaveChanges();
                //nếu user có role bằng 2 thì sẽ tạo mới 1 khách hàng 
                    var khachhang = new Customer();
                    khachhang.UserId = user.UserId;
                    khachhang.FullName = model.FullName;
                khachhang.Address = model.Address;
                    _myDb.Customers.Add(khachhang);
                _myDb.SaveChanges();

                return RedirectToAction("Index", "Home");

            }

            return View();
        }
      
        [HttpGet]
        public IActionResult Login(string? returnURL)
        {

            ViewBag.ReturnURL = returnURL;
            var user = new LoginVM();
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model, string? returnURL)
        {
            ViewBag.ReturnURL = returnURL;
            if (ModelState.IsValid)
            {
                var user = _myDb.Users.Include(x =>x.Customers).Include(x => x.Role)
                    .SingleOrDefault(u => u.Username == model.Username);
                if (user == null)
                {
                    ModelState.AddModelError("loi", "Không tồn tại khách hàng");
                }
                else
                {
                    if (user.PasswordHash != model.Password.ToMd5Hash(user.RandomKey))
                    {

                        ModelState.AddModelError("loi", "Sai thông tin đăng nhập");
                    }
                    else
                    {
                        // thông tin đúng
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Email,user.Email),
                            new Claim(ClaimTypes.Name, user.Username),
                          new Claim("UserID", user.UserId.ToString()),
                        
                            new Claim(ClaimTypes.Role, user.Role.RoleName)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims,
                            CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        if (Url.IsLocalUrl(returnURL))
                        {
                            return Redirect(returnURL);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                    }
                }

            }
            return View();
        }
        [Authorize]
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return Redirect("/"); ;
        }

    }
}
