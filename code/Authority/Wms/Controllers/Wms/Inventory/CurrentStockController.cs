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
    public class CurrentStockController : Controller
    {
        //
        // GET: /CurrentStock/

        [Dependency]
        public ICurrentStockService CurrentStockService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /CurrentStock/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string productCode = collection["ProductCode"] ?? "";
            string ware = collection["Ware"] ?? "";
            string area = collection["Area"] ?? "";
            string unitType = collection["UnitType"] ?? "";
            var storage = CurrentStockService.GetCellDetails(page, rows, productCode, ware, area, unitType);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
