using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.WarehouseInfo
{
    public class StorageController : Controller
    {
        [Dependency]
        public IStorageService StorageService { get; set; }

        //
        // GET: /Storage/

        public ActionResult Index()
        {
            return View();
        }

        //查询库存信息表
        // POST: /Storage/Details/
        [HttpPost]
        public ActionResult Details(int page, int rows,string type,string id)
        {
            var storage = StorageService.GetDetails(page, rows, type, id);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
