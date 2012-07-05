using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class RoleModule
    {
        public RoleModule()
        {
            this.RoleFunctions = new List<RoleFunction>();
        }

        public Guid RoleModuleID { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleSystem_RoleSystemID { get; set; }
        public Guid Module_ModuleID { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
        public virtual RoleSystem RoleSystem { get; set; }
    }
}
