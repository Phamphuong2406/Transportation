using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Domain.Account;
using Transportation.Domain.Entity;
using Transportation.Domain.Interfaces;
using Transportation.Infrastructure.Data;
using static System.Net.Mime.MediaTypeNames;

namespace Transportation.Infrastructure.DataAccess
{
    public class AccountRepo : IAccountRepo
    {
        private MyDbContext _context;
        public AccountRepo(MyDbContext context)
        {
            _context = context;
        }

        public async Task<List<Users>> GetAll()
        {
            return await _context.Userss.ToListAsync();
        }

        public async Task<Users> Login_Account(AccountLoginRequestData requestData) // trả về thông tin người dùng
        {
            //tạo tài khoản
            var user = new Users();
            try
            {
                //nếu tồn tại user
                var user_db = _context.Userss.Where(x => x.UserName == requestData.UserName && x.Password == requestData.Password).FirstOrDefault();
                if (user_db == null)
                {
                    return user;
                }
                user.Id = user_db.Id;
                user.UserName = user_db.UserName;
                user.FullName = user_db.FullName;
                return user;
            }
            catch (Exception)
            { throw; }
        }

        public async Task<int> AccountUpdateRefreshToken(AccountUpdateRefeshTokenRequestData tokenRequestData)
        {
            //nếu đăng nhập thành công thì taọ refeshtoken
            try
            {
                var user = _context.Userss.Where(x => x.Id == tokenRequestData.Id).FirstOrDefault();
                // nếu có user
                if (user != null)
                {
                    user.RefreshToken = tokenRequestData.RefreshToken;
                    user.RefreshTokenExprired = tokenRequestData.RefreshTokenExprired;
                    _context.Userss.Update(user);
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
            return _context.function.Where(s => s.FunctionCode == FunctionCode).FirstOrDefault();
        }

        public async Task<UserFunction> GetUserFunction(int UserId, int FunctionId, string PermisstionName)
        {
            var userfunction = _context.userfunction.Where(s => s.UserId == UserId && s.FunctionId == FunctionId ).ToList();

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
    }
}
