using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }

        public string Username { get; set; } 

       

        public string Email { get; set; } 

        public bool? IsActive { get; set; }

        public string PhoneNumber { get; set; }

        public List<String> RolesName { get; set; } // Danh sách vai trò
    }
   /* public class RoleDTO
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }*/
}
