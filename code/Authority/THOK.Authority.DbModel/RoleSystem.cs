using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class RoleSystem
    {
        public RoleSystem()
        {
            this.RoleModules = new List<RoleModule>();
        }

        public Guid RoleSystemID { get; set; }
        public bool IsActive { get; set; }
        public Guid Role_RoleID { get; set; }
        public Guid City_CityID { get; set; }
        public Guid System_SystemID { get; set; }
        public virtual City City { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        public virtual System System { get; set; }
    }
}
