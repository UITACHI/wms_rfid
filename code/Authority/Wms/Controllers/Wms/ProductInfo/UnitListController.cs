using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.ProductInfo
{
    public class UnitListController : Controller
    {
        //
        // GET: /UnitList/

        [Dependency]
        public IUnitListService UnitListService { get; set; }

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            return View();
        }

        //
        // GET: /UnitList/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string unitListCode = collection["UnitListCode"] ?? "";

            var users = UnitListService.GetDetails(page, rows, unitListCode);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /UnitList/Create/

        [HttpPost]
        public ActionResult Create(UnitList unitList)
        {
            bool bResult = UnitListService.Add(unitList);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /UnitList/Delete/

        [HttpPost]
        public ActionResult Delete(string unitListCode)
        {
            bool bResult = UnitListService.Delete(unitListCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /UnitList/Edit/

        public ActionResult Edit(UnitList unitList)
        {
            bool bResult = UnitListService.Save(unitList);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
