using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Domain.Interfaces;
using Transportation.Infrastructure.Dapper;
using Transportation.Infrastructure.Data;

namespace Transportation.Infrastructure.DataAccess
{
    public class ShiftServiceDapper : BaseApplicationService, IShiftServiceDapper
    {
        public ShiftServiceDapper(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }


        public async Task<List<Shift>> GetAll()
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Thamso", 1);
            return await DbConnection.QueryAsync<Shift>("TEN", parameters);

        }
    }
}
