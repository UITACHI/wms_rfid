using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.WebUtil;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;

namespace Wms.Controllers.Wms.SortingInfo
{
    public class SortOrderDispatchController : Controller
    {
        [Dependency]
        public ISortOrderDispatchService SortOrderDispatchService { get; set; }
        //
        // GET: /SortOrderDispatch/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /SortOrderDispatch/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string SortingLineCode = collection["SortingLineCode"] ?? "";
            string OrderDate = collection["OrderDate"] ?? "";
            var sortOrder = SortOrderDispatchService.GetDetails(page, rows,OrderDate,SortingLineCode);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //查询未作业的线路调度数据
        // GET: /SortOrderDispatch/GetWorkStatus/
        public ActionResult GetWorkStatus()
        {
            var sortOrder = SortOrderDispatchService.GetWorkStatus();
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //新增线路调度
        // POST: /SortOrderDispatch/Create/
        public ActionResult Create(string SortingLineCode, string DeliverLineCodes)
        {
            bool bResult = SortOrderDispatchService.Add(SortingLineCode, DeliverLineCodes);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortOrderDispatch/Edit/
        public ActionResult Edit(SortOrderDispatch sortDisoatch)
        {
            bool bResult = SortOrderDispatchService.Save(sortDisoatch);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortOrderDispatch/Delete/
        public ActionResult Delete(string id)
        {
            bool bResult = SortOrderDispatchService.Delete(id);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
