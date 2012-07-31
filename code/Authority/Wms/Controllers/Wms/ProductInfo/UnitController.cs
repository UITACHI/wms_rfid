using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.ProductInfo
{
    public class UnitController : Controller
    {
        [Dependency]
        public IUnitService UnitService { get; set; }
        //
        // GET: /Units/

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
        // GET: /Unit/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string UnitCode = collection["UnitCode"] ?? "";
            string UnitName = collection["UnitName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var unit = UnitService.GetDetails(page, rows, UnitCode, UnitName, IsActive);
            return Json(unit, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Unit/Create/

        [HttpPost]
        public ActionResult Create(Unit unit)
        {
            bool bResult = UnitService.Add(unit);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Unit/Edit/

        public ActionResult Edit(Unit unit)
        {
            bool bResult = UnitService.Save(unit);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Unit/Delete/
        [HttpPost]
        public ActionResult Delete(string unitCode)
        {
            bool bResult = UnitService.Delete(unitCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //根据卷烟编码查询单位系列
        // POST: /Unit/FindUnit/
        public ActionResult FindUnit(string productCode)
        {
            var unit = UnitService.FindUnit(productCode);
            return Json(unit, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
