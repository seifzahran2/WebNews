using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebNews.Models.ViewModels
{
    public class UserRolesVM
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public List<RoleVM> UserRole { get; set; }
    }
}
