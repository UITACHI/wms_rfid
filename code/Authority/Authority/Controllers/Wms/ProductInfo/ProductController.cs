using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces.Wms;
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
            string cityName = collection["CityName"] ?? "";

            var users = ProductService.GetDetails(page, rows, cityName);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
