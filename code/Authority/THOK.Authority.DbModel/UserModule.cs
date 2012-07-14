using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class UserModule
    {
        public UserModule()
        {
            this.UserFunctions = new List<UserFunction>();
        }

        public Guid UserModuleID { get; set; }
        public bool IsActive { get; set; }
        public Guid UserSystem_UserSystemID { get; set; }
        public Guid Module_ModuleID { get; set; }
        public virtual Module Module { get; set; }
        public virtual ICollection<UserFunction> UserFunctions { get; set; }
        public virtual UserSystem UserSystem { get; set; }
    }
}
