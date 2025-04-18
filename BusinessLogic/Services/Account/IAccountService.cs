using AutoMapper;
using BusinessLogic.DTOs.Account;
using BusinessLogic.Public;
using DataAccess.DataContext;
using DataAccess.Entity;
using DataAccess.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Account
{
    public interface IAccountService
    {
        Task<List<Users>> GetAll();
        Task<Users> Login_Account(AccountLoginRequestData requestData);
        Task<int> AccountUpdateRefreshToken(AccountUpdateRefeshTokenRequestData tokenRequestData);
        Task<Users> Register_Account(AccountRegisterRequestData registerRequestData);
        Task<Users> FindByEmailAsync(string email);
        Task<Users> Register_Dispatcher(AccountRegisterRequestData registerRequestData);
        Task<Users> Register_Driver(AccountRegisterRequestData registerRequestData);
        Task<Function> GetFunction(string FunctionCode);
        Task<UserFunction> GetUserFunction(int UserId, int FunctionId, string PermisstionName);
       Task<Role> GetRole(string RoleName);
        Task<UserRole> GetUserRole(int UserId, int RoleId);
        Task<Users> GetByGoogleId(string GoogleId);
        List<Dispatcher> GetAllDispatcher();
        List<Driver> GetAllDriver();
        Driver GetDriverByUserId(int userId);
        string UpdatePassword(string email, string password);
    }
    public class AccountService : IAccountService
    {
        private MyDbContext _context;
        private IAccountRepo _accountRepo;
        private readonly IMapper _mapper;
        public AccountService(MyDbContext context, IAccountRepo accountRepo, IMapper mapper)
        {
            _context = context;
            _accountRepo = accountRepo;
            _mapper = mapper;
        }

        public async Task<List<Users>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<Users> GetByGoogleId(string GoogleId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.GoogleUserId == GoogleId);
            if (user != null)
            {
                return user;
            }
            return null;
        }

        public async Task<Users> Login_Account(AccountLoginRequestData requestData) // trả về thông tin người dùng
        {
            //tạo tài khoản
            var user = new Users();
            try
            {
                //nếu tồn tại user
                var user_db = _context.Users.Where(x => x.Username == requestData.UserName).FirstOrDefault();
                if (user_db == null)
                {
                    return null;
                }
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(requestData.Password, user_db.PasswordHash);
                if (isPasswordValid == false) {
                    return null;
                }
                user.UserId = user_db.UserId;
                user.Username = user_db.Username;
                user.Email = user_db.Email;
                return user;
            }
            catch (Exception)
            { throw; }
        }

        public async Task<Users> FindByEmailAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email == email);
            if(user == null)
            {
                return null;
            }
            return user;
        }

        public async Task<Users> Register_Account(AccountRegisterRequestData registerRequestData)
        {
            //nếu tồn tại user
            var user_db = _context.Users.Where(x => x.Username == registerRequestData.Username).FirstOrDefault();
            if (user_db != null)
            {
                return null;
            }
            // Mã hóa mật khẩu
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequestData.PasswordHash);
            //tạo tài khoản
            var user = new Users();
            user.Username = registerRequestData.Username;
            user.Email = registerRequestData.Email;
            user.PhoneNumber = registerRequestData.PhoneNumber;

            user.PasswordHash = hashedPassword;
            user.IsActive = true;
            _context.Users.Add(user);
            _context.SaveChanges();
            //Tạo khách hàng
            var Customer = new Customer
            {
                UserId = user.UserId,
                FullName = registerRequestData.FullName,
                Address = registerRequestData.Address,
            };
            _context.Customers.Add(Customer);
            //tạo role
            var UserRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = 2
            };

            _context.UserRoles.Add(UserRole);

            _context.SaveChanges();
            return user;

        }

        public async Task<Users> Register_Dispatcher(AccountRegisterRequestData registerRequestData)
        {
            //tạo tài khoản
            var user = new Users();

            //nếu tồn tại user
            var user_db = _context.Users.Where(x => x.Username == registerRequestData.Username).FirstOrDefault();
            if (user_db != null)
            {
                return null;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequestData.PasswordHash);
            user.Username = registerRequestData.Username;
            user.Email = registerRequestData.Email;
            user.PhoneNumber = registerRequestData.PhoneNumber;
            user.PasswordHash = hashedPassword;

            user.IsActive = true;
            _context.Users.Add(user);
            _context.SaveChanges();
            //Tạo khách hàng
            var dispatcher = new Dispatcher
            {
                UserId = user.UserId,
                FullName = registerRequestData.FullName,
                Department = registerRequestData.Department,
                //  Address = registerRequestData.Address,
            };
            _context.Dispatchers.Add(dispatcher);
            //tạo role
            var UserRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = 1
            };

            _context.UserRoles.Add(UserRole);

            _context.SaveChanges();
            return user;

        }
        public async Task<Users> Register_Driver(AccountRegisterRequestData registerRequestData)
        {
            //tạo tài khoản
            var user = new Users();

            //nếu tồn tại user
            var user_db = _context.Users.Where(x => x.Username == registerRequestData.Username).FirstOrDefault();
            if (user_db != null)
            {
                return null;
            }
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(registerRequestData.PasswordHash);
            user.Username = registerRequestData.Username;
            user.Email = registerRequestData.Email;
            user.PhoneNumber = registerRequestData.PhoneNumber;
            user.PasswordHash = hashedPassword;
            user.IsActive = true;
            _context.Users.Add(user);
            _context.SaveChanges();
            var driver = new Driver
            {
                UserId = user.UserId,
                FullName = registerRequestData.FullName,
                DateOfBirth = registerRequestData.DateOfBirth,
                Idcard = registerRequestData.Idcard,
                HealthStatus = registerRequestData.HealthStatus,
            };
            _context.Drivers.Add(driver);
            //tạo role
            var UserRole = new UserRole
            {
                UserId = user.UserId,
                RoleId = 3
            };

            _context.UserRoles.Add(UserRole);

            _context.SaveChanges();
            return user;

        }

        public async Task<int> AccountUpdateRefreshToken(AccountUpdateRefeshTokenRequestData tokenRequestData)
        {
            //nếu đăng nhập thành công thì taọ refeshtoken
            try
            {
                var user = _context.Users.Where(x => x.UserId == tokenRequestData.Id).FirstOrDefault();
                // nếu có user
                if (user != null)
                {
                    user.RefreshToken = tokenRequestData.RefreshToken;
                    user.RefreshTokenExprired = tokenRequestData.RefreshTokenExprired;
                    _context.Users.Update(user);
                    _context.SaveChanges();
                    return 1;
                }
            }
            catch (Exception)
            { throw; }
            return 0;
        }

        public async Task<Function> GetFunction(string FunctionCode)
        {
            return _context.Functions.Where(s => s.FunctionCode == FunctionCode).FirstOrDefault();
        }

        public async Task<UserFunction> GetUserFunction(int UserId, int FunctionId, string PermisstionName)
        {
            var userfunction = _context.UserFunctions.Where(s => s.UserId == UserId && s.FunctionId == FunctionId).ToList();

            switch (PermisstionName)
            {
                case "IsView":
                    return userfunction.Where(s => s.IsView == 1).FirstOrDefault();
                case "IsUpdate":
                    return userfunction.Where(s => s.IsUpdate == 1).FirstOrDefault();
                case "IsCreate":
                    return userfunction.Where(s => s.IsCreate == 1).FirstOrDefault();
                case "IsDelete":
                    return userfunction.Where(s => s.IsDelete == 1).FirstOrDefault();

                default:
                    return userfunction.Where(s => s.IsView == 1).FirstOrDefault();

            }

        }
        public async Task<Role> GetRole(string RoleName)
        {
            return _context.Roles.Where(s => s.RoleName == RoleName).FirstOrDefault();
        }

        // nếu tìm được đối tượng có id dùng hiện tại và roleId truyền vào thì trra về 
        public async Task<UserRole> GetUserRole(int UserId, int RoleId)
        {
            //tìm user theo dữ liệu truyền vào 
            var userfunction = _context.UserRoles.Where(s => s.UserId == UserId && s.RoleId == RoleId).FirstOrDefault();

            return userfunction;

        }
        public List<Dispatcher> GetAllDispatcher()
        {
            return _context.Dispatchers.ToList();
        }

        public List<Driver> GetAllDriver()
        {
            return _context.Drivers.ToList();
        }
        public Driver GetDriverByUserId(int userId)
        {
            var driver = _context.Drivers.FirstOrDefault(x => x.UserId == userId);
            if (driver == null)
            {
                return null;
            }
            return driver;
        }
        public string UpdatePassword(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(x => x.Email  == email);
            if(user == null)
            {
                return "Người dùng không hợp lệ";
            }
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(password);
            _context.SaveChanges();
            return "Cập nhật thành công";
        }

    }
       
}
