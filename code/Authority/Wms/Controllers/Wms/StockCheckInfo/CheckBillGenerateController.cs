using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.Wms.StockCheckInfo
{
    public class CheckBillGenerateController : Controller
    {
        //
        // GET: /CheckBillGenerate/

        public ActionResult Index(string moduleID)
        {
            //ViewBag.hasSearch = true;
            //ViewBag.hasAdd = true;
            //ViewBag.hasEdit = true;
            //ViewBag.hasDelete = true;
            //ViewBag.hasPrint = true;
            //ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

    }
}
