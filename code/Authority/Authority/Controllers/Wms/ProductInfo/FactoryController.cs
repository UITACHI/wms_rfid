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
    public class FactoryController : Controller
    {
        [Dependency]
        public ISupplierService SupplierService { get; set; }
        //
        // GET: /Factory/

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
        // GET: /Factory/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string SupplierCode = collection["SupplierCode"] ?? "";
            string SupplierName = collection["SupplierName"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var supplier=SupplierService.GetDetails(page,rows,SupplierCode,SupplierName,IsActive);
            return Json(supplier, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /Factory/Create/

        [HttpPost]
        public ActionResult Create(Supplier supplier)
        {
            bool bResult = SupplierService.Add(supplier);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
