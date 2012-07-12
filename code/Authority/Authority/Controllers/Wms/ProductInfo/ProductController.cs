using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
namespace Authority.Controllers.ProductInfo
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/
        [Dependency]
        public IProductService ProductService { get; set; }

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            return View();
        }
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string productName = collection["Product"] ?? "";

            var users = ProductService.GetDetails(page, rows, productName);
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
    }
}
