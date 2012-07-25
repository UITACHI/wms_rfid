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

        //查询货位要生成的盘点数据表
        // POST: /Storage/CheckCellDetails/
        public ActionResult CheckCellDetails(int page, int rows, string ware,string area,string shelf,string cell)
        {
            var storage = StorageService.GetCellDetails(page, rows, ware, area, shelf, cell);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询产品要生成的盘点数据表
        // POST: /Storage/CheckProductDetails/        
        public ActionResult CheckProductDetails(int page, int rows, string products)
        {
            var storage = StorageService.GetProductDetails(page, rows, products);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
