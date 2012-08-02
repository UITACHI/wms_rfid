using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortingLineController : Controller
    {
        [Dependency]
        public ISortingLineService SortingLineService { get; set; }
        //
        // GET: /SortingLine/
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
        // GET: /SortingLine/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string sortingLineCode = collection["sortingLineCode"] ?? "";
            string sortingLineName = collection["sortingLineName"] ?? "";
            string SortingLineType = collection["SortingLineType"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var sortOrder = SortingLineService.GetDetails(page, rows, sortingLineCode, sortingLineName, SortingLineType, IsActive);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /SortingLine/GetSortLine/
        public ActionResult GetSortLine()
        {
            var sortOrder = SortingLineService.GetSortLine();
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLine/Create/
        public ActionResult Create(SortingLine sortLine)
        {
            bool bResult = SortingLineService.Add(sortLine);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLine/Edit/
        public ActionResult Edit(SortingLine sortLine)
        {
            bool bResult = SortingLineService.Save(sortLine);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLine/Delete/
        public ActionResult Delete(string sortLineCode)
        {
            bool bResult = SortingLineService.Delete(sortLineCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
