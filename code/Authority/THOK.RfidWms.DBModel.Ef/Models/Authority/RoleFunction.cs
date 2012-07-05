using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class RoleFunction
    {
        public Guid RoleFunctionID { get; set; }
        public bool IsActive { get; set; }
        public Guid RoleModule_RoleModuleID { get; set; }
        public Guid Function_FunctionID { get; set; }
        public virtual Function Function { get; set; }
        public virtual RoleModule RoleModule { get; set; }
    }
}
