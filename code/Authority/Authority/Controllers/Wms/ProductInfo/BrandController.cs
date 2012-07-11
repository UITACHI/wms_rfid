using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.ProductInfo
{
    public class BrandController : Controller
    {
        [Dependency]
        public IBrandService BrandService { get; set; }
        //
        // GET: /Brand/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            return View();
        }

        //
        // GET: /Brand/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BrandCode = collection["BrandCode"] ?? "";
            string BrandName = collection["BrandName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var brand = BrandService.GetDetails(page, rows, BrandCode, BrandName, IsActive);
            return Json(brand, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Brand/Create/

        [HttpPost]
        public ActionResult Create(Brand brand)
        {
            bool bResult = BrandService.Add(brand);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
