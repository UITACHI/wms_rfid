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

        //查询库存盘点预览信息
        // POST: /Storage/Details/    
        public ActionResult Details(int page, int rows,string type,string id)
        {
            var storage = StorageService.GetDetails(page, rows, type, id);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
        
        //查询库存信息用于移库根据inOrOut判断是移出还是移入-移库用
        // POST: /Storage/Details/
        public ActionResult GetMoveStorgeDetails(int page, int rows, string type, string id, string inOrOut, string productCode)
        {
            var storage = StorageService.GetMoveStorgeDetails(page, rows, type, id,inOrOut,productCode);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询库存信息用于移库时-移库用
        // POST: /Storage/Details/
        public ActionResult GetMoveInStorgeDetails(int page, int rows, string type, string id, string cellCode, string productCode)
        {
            var storage = StorageService.GetMoveInStorgeDetails(page, rows, type, id, cellCode, productCode);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
