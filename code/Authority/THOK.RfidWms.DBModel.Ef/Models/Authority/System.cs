using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class System
    {
        public System()
        {
            this.LoginLogs = new List<LoginLog>();
            this.Modules = new List<Module>();
            this.RoleSystems = new List<RoleSystem>();
            this.UserSystems = new List<UserSystem>();
        }

        public Guid SystemID { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual ICollection<RoleSystem> RoleSystems { get; set; }
        public virtual ICollection<UserSystem> UserSystems { get; set; }
    }
}
