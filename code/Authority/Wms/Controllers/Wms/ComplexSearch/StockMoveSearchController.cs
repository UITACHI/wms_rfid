using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class StockMoveSearchController : Controller
    {
        [Dependency]
        public IStockMoveSearchService StockMoveSearchService { get; set; }
        [Dependency]
        public IMoveSearchDetailService MoveSearchDetailService { get; set; }
        //
        // GET: /StockMoveSearch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /StockMoveSearch/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string WarehouseCode = collection["WarehouseCode"] ?? "";
            string BeginDate = collection["BeginDate"] ?? "";
            string EndDate = collection["EndDate"] ?? "";
            string OperatePersonCode = collection["OperatePerson"] ?? "";
            string CheckPersonCode = collection["CheckPerson"] ?? "";
            string Operate_Status = collection["Operate_Status"] ?? "";
            var moveBillMaster = StockMoveSearchService.GetDetails(page, rows, BillNo, WarehouseCode, BeginDate, EndDate, OperatePersonCode, CheckPersonCode, Operate_Status);
            return Json(moveBillMaster, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /StockMoveSearch/InfoDetails/

        public ActionResult InfoDetails(int page, int rows, string BillNo)
        {
            var MoveBillDetail = MoveSearchDetailService.GetDetails(page, rows, BillNo);
            return Json(MoveBillDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
