using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.Wms.Inventory
{
    public class CargospaceController : Controller
    {
        //
        // GET: /Cargospace/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();    
        }

    }
}
