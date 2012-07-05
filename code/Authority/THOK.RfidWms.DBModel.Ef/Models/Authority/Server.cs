using System;
using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Models.Authority
{
    public class Server
    {
        public Guid ServerID { get; set; }
        public string ServerName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public Guid City_CityID { get; set; }
        public virtual City City { get; set; }
    }
}
