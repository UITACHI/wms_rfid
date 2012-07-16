using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Bll.Models
{
    public class Tree
    {
        public string id{ get; set; }
        public string text { get; set; }
        public string iconCls { get; set; }
        public bool @checked { get; set; }
        public string state { get; set; }
        public object attributes { get; set; }
        public Tree[] children { get; set; }
    }
}
