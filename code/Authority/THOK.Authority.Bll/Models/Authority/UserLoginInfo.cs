using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Bll.Models.Authority
{
    public class UserLoginInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string CityID { get; set; }
        public string SystemID { get; set; }
        public string ServerID { get; set; }
    }
}
