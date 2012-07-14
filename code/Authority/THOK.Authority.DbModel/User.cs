using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class User
    {
        public User()
        {
            this.LoginLogs = new List<LoginLog>();
            this.UserRoles = new List<UserRole>();
            this.UserSystems = new List<UserSystem>();
        }

        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string Pwd { get; set; }
        public string ChineseName { get; set; }
        public bool IsLock { get; set; }
        public bool IsAdmin { get; set; }
        public string LoginPC { get; set; }
        public string Memo { get; set; }
        public virtual ICollection<LoginLog> LoginLogs { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<UserSystem> UserSystems { get; set; }
    }
}
