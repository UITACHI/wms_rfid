using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.WebUtil;

namespace Authority.Controllers.ProductInfo
{
    public class UnitController : Controller
    {
        [Dependency]
        public IUnitService UnitService { get; set; }
        //
        // GET: /Units/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
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

    }
}
