using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.SignalR.Dispatch.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortWorkDispatchController : Controller
    {
        [Dependency]
        public ISortWorkDispatchService SortWorkDispatchService { get; set; }

        [Dependency]
        public ISortOrderWorkDispatchService SortOrderWorkDispatchService { get; set; }
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
            string SortingLineCode = collection["SortingLineCode"] ?? "";
            string DispatchStatus = collection["DispatchStatus"] ?? "";
            var sortOrder = SortWorkDispatchService.GetDetails(page, rows, OrderDate, SortingLineCode, DispatchStatus);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /SortWorkDispatch/Dispatch/
        public ActionResult Dispatch(string dispatchId)
        {
            SortOrderWorkDispatchService.Dispatch(dispatchId);
            string msg = true ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(true, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortWorkDispatch/Delete/
        public ActionResult Delete(string id)
        {
            bool bResult = SortWorkDispatchService.Delete(id);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
