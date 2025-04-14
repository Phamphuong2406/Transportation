using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Domain.Account;
using Transportation.Domain.Entity;

namespace Transportation.Domain.Interfaces
{
    public interface IAccountRepo
    {
        Task<List<Users>> GetAll();
        Task<Users> Login_Account(AccountLoginRequestData requestData);
        Task<int> AccountUpdateRefreshToken(AccountUpdateRefeshTokenRequestData tokenRequestData);

        Task<Function> GetFunction(string FunctionCode);
        Task<UserFunction> GetUserFunction (int UserId,int FunctionId,string PermisstionName);
    }
}
