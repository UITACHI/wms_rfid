using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class StockOutSearchController : Controller
    {
        [Dependency]
        public IStockOutSearchService StockOutSearchService { get; set; }
        [Dependency]
        public IOutSearchDetailService OutSearchDetailService { get; set; }
        //
        // GET: /StockOutSearch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /StockOutSearch/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            var outBillMaster = StockOutSearchService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status);
            return Json(outBillMaster, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /StockOutSearch/InfoDetails/

        public ActionResult InfoDetails(int page, int rows, string BillNo)
        {
            var outBillDetail = OutSearchDetailService.GetDetails(page, rows, BillNo);
            return Json(outBillDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
