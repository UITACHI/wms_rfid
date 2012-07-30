using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.Wms.Inventory
{
    public class DailyBalanceController : Controller
    {
        //
        // GET: /DailyBalance/
        [Dependency]
        public IDailyBalanceService DailyBalanceService { get; set; }

        //
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasBalance = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        //public ActionResult Details(int page, int rows, FormCollection collection)
        //{
        //    var brand = DailyBalanceService.GetDetails(page, rows);
        //    return Json(brand, "text", JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult InfoDetails(int page, int rows, string date)
        //{
        //    var brand = DailyBalanceService.MXGetDetails(page, rows, date);
        //    return Json(brand, "text", JsonRequestBehavior.AllowGet);
        //}

    }
}
