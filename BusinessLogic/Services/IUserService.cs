using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.DataContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public interface IUserService
    {
        List<UserDTO> GetAllUser();
    }
    public class UserService : IUserService
    {
        private MyDbContext _context;
        private readonly IMapper _mapper;
        public UserService(MyDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public List<UserDTO> GetAllUser()
        {
            //  Dùng ProjectTo<T>() nếu đang làm việc với Entity Framework Core và muốn tối ưu truy vấn SQL  ,Dùng Map<T>() nếu dữ liệu đã load vào bộ nhớ và bạn chỉ cần chuyển đổi giữa các đối tượng.
            /*      var users = _context.Users
                          .Include(u => u.UserRoles)
                          .ThenInclude(u => u.Role)
                           .ProjectTo<UserDTO>(_mapper.ConfigurationProvider).ToList();
                      return users;*/

            var users = _context.Users
      .Include(u => u.UserRoles)
      .ThenInclude(ur => ur.Role) // Load toàn bộ dữ liệu từ DB vào bộ nhớ
      .ToList(); // Lấy dữ liệu về trước

            return _mapper.Map<List<UserDTO>>(users); // Sau đó mới ánh xạ

        }

    }
}
