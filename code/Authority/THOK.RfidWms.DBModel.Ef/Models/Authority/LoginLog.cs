using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class LoginLog
    {
        public Guid LogID { get; set; }
        public string LoginPC { get; set; }
        public string LoginTime { get; set; }
        public string LogoutTime { get; set; }
        public Guid User_UserID { get; set; }
        public Guid System_SystemID { get; set; }
        public virtual System System { get; set; }
        public virtual User User { get; set; }
    }
}
