using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortingOrderController : Controller
    {
        [Dependency]
        public ISortOrderService SortOrderService { get; set; }
        [Dependency]
        public ISortOrderDetailService SortOrderDetailService { get; set; }
        //
        // GET: /SortingOrder/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasDownload = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //查询主单
        // GET: /SortingOrder/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string OrderID = collection["OrderID"] ?? "";
            string orderDate = collection["orderDate"] ?? "";
            string CustomerCode = collection["CustomerCode"] ?? "";
            string CustomerName = collection["CustomerName"] ?? "";
            var sortOrder = SortOrderService.GetDetails(page, rows, OrderID, orderDate, CustomerCode, CustomerName);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /SortingOrder/sortOrderDetails/
        public ActionResult sortOrderDetails(int page, int rows, string OrderID)
        {
            var SortOrderDetail = SortOrderDetailService.GetDetails(page, rows, OrderID);
            return Json(SortOrderDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
