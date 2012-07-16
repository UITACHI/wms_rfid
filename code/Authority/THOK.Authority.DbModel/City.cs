using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class City
    {
        public City()
        {
            this.RoleSystems = new List<RoleSystem>();
            this.Servers = new List<Server>();
            this.UserSystems = new List<UserSystem>();
        }

        public Guid CityID { get; set; }
        public string CityName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<RoleSystem> RoleSystems { get; set; }
        public virtual ICollection<Server> Servers { get; set; }
        public virtual ICollection<UserSystem> UserSystems { get; set; }
    }
}
