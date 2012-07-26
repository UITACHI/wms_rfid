using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class SortOrderSearchController : Controller
    {
        [Dependency]
        public ISortOrderSearchService SortOrderSearchService { get; set; }
        [Dependency]
        public IOrderSearchDetailService OrderSearchDetailService { get; set; }

        //
        // GET: /SortOrderSearch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /SortOrderSearch/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string OrderID = collection["OrderID"] ?? "";
            string OrderDate = collection["OrderDate"] ?? "";
            var SortOrder = SortOrderSearchService.GetDetails(page, rows, OrderID, OrderDate);
            return Json(SortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /SortOrderSearch/InfoDetails/

        public ActionResult InfoDetails(int page, int rows, string OrderID)
        {
            var SortOrderDetail = OrderSearchDetailService.GetDetails(page, rows, OrderID);
            return Json(SortOrderDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
