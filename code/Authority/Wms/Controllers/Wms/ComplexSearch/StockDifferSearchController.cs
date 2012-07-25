using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class StockDifferSearchController : Controller
    {
        [Dependency]
        public IStockDifferSearchService StockDifferSearchService { get; set; }
        [Dependency]
        public IDifferSearchDetailService DifferSearchDetailService { get; set; }
        //
        // GET: /StockDifferSearch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /StockDifferSearch/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            var DifferBillMaster = StockDifferSearchService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status);
            return Json(DifferBillMaster, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /StockDifferSearch/InfoDetails/

        public ActionResult InfoDetails(int page, int rows, string BillNo)
        {
            var DifferBillDetail = DifferSearchDetailService.GetDetails(page, rows, BillNo);
            return Json(DifferBillDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
