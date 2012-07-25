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
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            var MoveBillMaster = StockMoveSearchService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status);
            return Json(MoveBillMaster, "text", JsonRequestBehavior.AllowGet);
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
