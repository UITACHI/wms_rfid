using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class Module
    {
        public Module()
        {
            this.Functions = new List<Function>();
            this.Modules = new List<Module>();
            this.RoleModules = new List<RoleModule>();
            this.UserModules = new List<UserModule>();
        }

        public Guid ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int ShowOrder { get; set; }
        public string ModuleURL { get; set; }
        public string IndicateImage { get; set; }
        public string DeskTopImage { get; set; }
        public Guid System_SystemID { get; set; }
        public Guid ParentModule_ModuleID { get; set; }
        public virtual ICollection<Function> Functions { get; set; }
        public virtual ICollection<Module> Modules { get; set; }
        public virtual Module ParentModule { get; set; }
        public virtual ICollection<RoleModule> RoleModules { get; set; }
        public virtual ICollection<UserModule> UserModules { get; set; }
        public virtual System System { get; set; }
    }
}
