using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;

namespace Wms.Controllers.Wms.Inventory
{
    public class DailyBalanceController : Controller
    {
        //
        // GET: /DailyBalance/

        [Dependency]
        public IDailyBalanceService DailyBalanceService { get; set; }

        //
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasBalance = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /DailyBalance/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string beginDate = collection["BeginDate"] ?? "";
            string endDate = collection["EndDate"] ?? "";
            string warehouseCode = collection["WarehouseCode"] ?? "";
            string unitType = collection["UnitType"] ?? "";
            var DailyBalance = DailyBalanceService.GetDetails(page, rows, beginDate, endDate, warehouseCode, unitType);
            return Json(DailyBalance, "text", JsonRequestBehavior.AllowGet);
        }
        //
        // GET: /DailyBalance/InfoDetails/
        public ActionResult InfoDetails(int page, int rows, string warehouseCode, string settleDate,string unitType)
        {
            var DailyBalanceInfo = DailyBalanceService.GetInfoDetails(page, rows, warehouseCode, settleDate, unitType);
            return Json(DailyBalanceInfo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /DailyBalance/DailyBalanceInfos/
        public ActionResult DailyBalanceInfos(int page, int rows, string warehouseCode, string settleDate)
        {
            var DailyBalanceInfo = DailyBalanceService.GetDailyBalanceInfos(page, rows, warehouseCode, settleDate);
            return Json(DailyBalanceInfo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /DailyBalance/DoDailyBalance/
        public ActionResult DoDailyBalance(string warehouseCode, string settleDate)
        {
            string errorInfo = string.Empty;
            bool bResult = DailyBalanceService.DoDailyBalance(warehouseCode, settleDate,ref errorInfo);
            string msg = bResult ? "日结成功！" : "日结失败！";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
