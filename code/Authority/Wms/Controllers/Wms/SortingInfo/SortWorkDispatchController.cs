using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortWorkDispatchController : Controller
    {
        [Dependency]
        public ISortWorkDispatchService SortWorkDispatchService { get; set; }
        //
        // GET: /SortWorkDispatch/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /SortWorkDispatch/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string OrderDate = collection["OrderDate"] ?? "";
            string SortingLineName = collection["SortingLineName"] ?? "";
            string DispatchStatus = collection["DispatchStatus"] ?? "";
            var sortOrder = SortWorkDispatchService.GetDetails(page, rows, OrderDate, SortingLineName,DispatchStatus);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
