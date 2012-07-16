using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THOK.Authority.Bll.Models
{
    public class Fun
    {
        public Fun[] funs { get; set; }
        public string funid { get; set; }
        public string funname { get; set; }
        public string iconCls { get; set; }
        public bool isActive { get; set; }
    }
}