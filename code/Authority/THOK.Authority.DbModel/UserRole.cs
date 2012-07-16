using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class UserRole
    {
        public Guid UserRoleID { get; set; }
        public Guid Role_RoleID { get; set; }
        public Guid User_UserID { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
