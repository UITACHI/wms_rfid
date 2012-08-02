using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.Inventory
{
    public class CargospaceController : Controller
    {
        //
        // GET: /Cargospace/

        [Dependency]
        public ICargospaceService CargospaceService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();    
        }

        //
        // GET: /Cargospace/Details/

        public ActionResult Details(int page, int rows, string type, string id)
        {
            var storage = CargospaceService.GetCellDetails(page, rows, type, id);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
