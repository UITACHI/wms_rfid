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
    public class SortingLowerlimitController : Controller
    {
        [Dependency]
        public ISortingLowerlimitService SortingLowerlimitService { get; set; }
        //
        // GET: /SortingLowerLimit/

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
        // GET: /SortingLowerLimit/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string sortingLineCode = collection["sortingLineCode"] ?? "";
            string productCode = collection["productCode"] ?? "";
            string sortingLineName = collection["sortingLineName"] ?? "";
            string productName = collection["productName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var sortOrder = SortingLowerlimitService.GetDetails(page, rows, sortingLineCode, sortingLineName, productName, productCode, IsActive);
            return Json(sortOrder, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLowerLimit/Create/
        public ActionResult Create(SortingLowerlimit sortLowerlimin)
        {
            bool bResult = SortingLowerlimitService.Add(sortLowerlimin);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLowerLimit/Edit/
        public ActionResult Edit(SortingLowerlimit sortLowerlimin)
        {
            bool bResult = SortingLowerlimitService.Save(sortLowerlimin);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /SortingLowerLimit/Delete/
        public ActionResult Delete(string id)
        {
            bool bResult = SortingLowerlimitService.Delete(id);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
