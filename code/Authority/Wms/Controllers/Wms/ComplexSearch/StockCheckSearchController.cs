using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class StockCheckSearchController : Controller
    {
        [Dependency]
        public IStockCheckSearchService StockCheckSearchService { get; set; }
        [Dependency]
        public ICheckSearchDetailService CheckSearchDetailService { get; set; }
        //
        // GET: /StockCheckSearch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /StockCheckSearch/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            var checkBillMaster = StockCheckSearchService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status);
            return Json(checkBillMaster, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /StockCheckSearch/InfoDetails/

        public ActionResult InfoDetails(int page, int rows, string BillNo)
        {
            var checkBillDetail = CheckSearchDetailService.GetDetails(page, rows, BillNo);
            return Json(checkBillDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
