using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class Function
    {
        public Function()
        {
            this.RoleFunctions = new List<RoleFunction>();
            this.UserFunctions = new List<UserFunction>();
        }

        public Guid FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string ControlName { get; set; }
        public string IndicateImage { get; set; }
        public Guid Module_ModuleID { get; set; }
        public virtual ICollection<RoleFunction> RoleFunctions { get; set; }
        public virtual ICollection<UserFunction> UserFunctions { get; set; }
        public virtual Module Module { get; set; }
    }
}
