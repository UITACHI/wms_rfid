using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class UserFunction
    {
        public Guid UserFunctionID { get; set; }
        public bool IsActive { get; set; }
        public Guid UserModule_UserModuleID { get; set; }
        public Guid Function_FunctionID { get; set; }
        public virtual Function Function { get; set; }
        public virtual UserModule UserModule { get; set; }
    }
}
