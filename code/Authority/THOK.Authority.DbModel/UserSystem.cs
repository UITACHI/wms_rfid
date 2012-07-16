using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class UserSystem
    {
        public UserSystem()
        {
            this.UserModules = new List<UserModule>();
        }

        public Guid UserSystemID { get; set; }
        public bool IsActive { get; set; }
        public Guid User_UserID { get; set; }
        public Guid City_CityID { get; set; }
        public Guid System_SystemID { get; set; }
        public virtual City City { get; set; }
        public virtual System System { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<UserModule> UserModules { get; set; }
    }
}
