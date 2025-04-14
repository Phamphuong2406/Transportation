using AutoMapper;
using BusinessLogic.DTOs;
using DataAccess.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Public
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
           // CreateMap<RegisterVM, User>();
            CreateMap<DriverDTO, Users>();
            CreateMap<DispatcherDTO, Users>();
          
            CreateMap<Shift, ShiftDTO>().ReverseMap();//Nếu cần chuyển đổi cả hai chiều (ví dụ: đọc từ database Entity → DTO, sau đó chỉnh sửa và lưu lại DTO → Entity)
            CreateMap<Users,UserDTO>()
                    .ForMember(dest => dest.RolesName,opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.RoleName)));
            CreateMap<Warehouse, WarehouseDto>().ReverseMap();
            CreateMap<ShippingRequetsDTO, ShippingRequest>();
            CreateMap<TruckDTO, Truck>();

        }
    }
}
