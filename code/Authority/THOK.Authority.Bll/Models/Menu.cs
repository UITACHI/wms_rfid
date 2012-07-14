using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace THOK.Authority.Bll.Models
{
    public class Menu
    {
        public string SystemID { get; set; }
        public string SystemName { get; set; }
        public string ModuleID { get; set; }
        public string ModuleName { get; set; }
        public string ParentModuleID { get; set; }
        public string ParentModuleName { get; set; }
        public string IndicateImage { get; set; }
        public string ModuleURL { get; set; }
        public int ShowOrder { get; set; }   
        public Menu[] children { get; set; }

        public string menuid { get { return ModuleID; } set { ModuleID = value; } }
        public string menuname { get { return ModuleName; } set { ModuleName = value; } }
        public string icon { get { return IndicateImage; } set { IndicateImage = value; } }
        public string url { get { return ModuleURL + "?ModuleID=" + ModuleID; } set { ModuleURL = value; } }
        public string iconCls
        {
            get
            {
                return IndicateImage;
            }

            set
            {
                IndicateImage = value;
            }
        }
        public string useiframe { get; set; }
        public bool enable { get; set; }
    }
}