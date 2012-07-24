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
        [Dependency]
        public IIntoSearchDetailService IntoSearchDetailService { get; set; }
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
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            var inBillMaster = StockIntoSearchService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /StockIntoSearch/InfoDetails/

        public ActionResult InfoDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = IntoSearchDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
