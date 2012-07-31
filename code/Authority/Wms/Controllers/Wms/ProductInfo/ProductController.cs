using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
namespace Authority.Controllers.ProductInfo
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        [Dependency]
        public IProductService ProductService { get; set; }

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
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string productName = collection["ProductName"] ?? "";
            string productCode = collection["ProductCode"] ?? "";
            string customCode = collection["CustomCode"] ?? "";
            string brandCode = collection["BrandCode"] ?? "";
            string uniformCode = collection["UniformCode"] ?? "";
            string abcTypeCode = collection["AbcTypeCode"] ?? "";
            string shortCode = collection["ShortCode"] ?? "";
            string priceLevelCode = collection["PriceLevelCode"] ?? "";
            string supplierCode = collection["SupplierCode"] ?? "";
            var users = ProductService.GetDetails(page, rows, productName, productCode, customCode, brandCode, uniformCode, abcTypeCode, shortCode, priceLevelCode, supplierCode);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(Product product)
        {
            bool bResult = ProductService.Add(product);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string ProductCode)
        {
            bool bResult = ProductService.Delete(ProductCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(Product product)
        {
            bool bResult = ProductService.Save(product);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //下限查询卷烟信息 2012年7月24日 16:29:25
        // GET: /Product/FindProduct/
        public ActionResult FindProduct(int page, int rows, string QueryString, string value)
        {
            var product = ProductService.FindProduct(page, rows, QueryString, value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        //盘点查询卷烟信息 
        // GET: /Product/CheckFindProduct/
        public ActionResult CheckFindProduct()
        {
            var product = ProductService.checkFindProduct();
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
