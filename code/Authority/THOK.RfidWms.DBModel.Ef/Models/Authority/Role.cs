using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class Role
    {
        public Role()
        {
            this.RoleSystems = new List<RoleSystem>();
            this.UserRoles = new List<UserRole>();
        }

        public Guid RoleID { get; set; }
        public string RoleName { get; set; }
        public bool IsLock { get; set; }
        public string Memo { get; set; }
        public virtual ICollection<RoleSystem> RoleSystems { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
