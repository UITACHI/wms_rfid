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
            string WarehouseCode = collection["WarehouseCode"] ?? "";
            string BeginDate = collection["BeginDate"] ?? "";
            string EndDate = collection["EndDate"] ?? "";
            string OperatePersonCode = collection["OperatePerson"] ?? "";
            string CheckPersonCode = collection["CheckPerson"] ?? "";
            string Operate_Status = collection["Operate_Status"] ?? "";
            var inBillMaster = StockIntoSearchService.GetDetails(page, rows, BillNo, WarehouseCode, BeginDate, EndDate, OperatePersonCode, CheckPersonCode, Operate_Status);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockIntoSearch/InfoDetails/

        public ActionResult DetailInfos(int page, int rows, string BillNo)
        {
            var inBillDetail = IntoSearchDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /StockIntoSearch/DetailInfos/

        public ActionResult InfoDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = StockIntoSearchService.GetDetailInfos(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
