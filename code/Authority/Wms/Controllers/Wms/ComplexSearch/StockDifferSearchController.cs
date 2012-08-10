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
            string WarehouseCode = collection["WarehouseCode"] ?? "";
            string CheckBillNo = collection["CheckBillNo"] ?? "";
            string BeginDate = collection["BeginDate"] ?? "";
            string EndDate = collection["EndDate"] ?? "";
            string OperatePersonCode = collection["OperatePerson"] ?? "";
            string CheckPersonCode = collection["CheckPerson"] ?? "";
            string Operate_Status = collection["Operate_Status"] ?? "";
            var differBillMaster = StockDifferSearchService.GetDetails(page, rows, BillNo, CheckBillNo, WarehouseCode, BeginDate, EndDate, OperatePersonCode, CheckPersonCode, Operate_Status);
            return Json(differBillMaster, "text", JsonRequestBehavior.AllowGet);
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
