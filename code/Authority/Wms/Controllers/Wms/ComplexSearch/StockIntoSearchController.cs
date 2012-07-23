using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class StockIntoSearchController : Controller
    {
        [Dependency]
        public IStockIntoSearchService StockIntoSearchService { get; set; }
        //
        // GET: /StockIntoSearch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /StockIntoSearch/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string IntoBillNo = collection["IntoBillNo"] ?? "";
            string MakeDate = collection["MakeDate"] ?? "";
            string OperatePerson = collection["OperatePerson"] ?? "";
            string OperateStatus = collection["OperateStatus"] ?? "";
            var inBillMaster = StockIntoSearchService.GetDetails(page, rows, IntoBillNo, MakeDate, OperatePerson, OperateStatus);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
