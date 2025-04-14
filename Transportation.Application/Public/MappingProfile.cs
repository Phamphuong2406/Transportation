using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transportation.Application.DTO;
using Transportation.Domain.ViewModel;
using Transportation.Domain.ViewModel.Register;
using Transportation.Infrastructure.Data;

namespace Transportation.Application.Public
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<RegisterVM, User>();
            CreateMap<DriverDTO, User>();
            CreateMap<DispatcherDTO, User>();
            CreateMap<Shift, ShiftDTO>().ReverseMap();
            /* CreateMap<WarehouseVM, Mwarehouse>();
             CreateMap<InventoryVM, Inventory>();*/
        }
    }
}
