using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.WarehouseInfo
{
    public class WarehouseController : Controller
    {
        [Dependency]
        public IWarehouseService WarehouseService { get; set; }       

        //
        // GET: /Warehouse/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            return View();
        }

        //查询仓库信息表
        // POST: /Warehouse/Details
        [HttpPost]
        public ActionResult Details(int page, int rows, string warehouseCode)
        {
            var warehouse = WarehouseService.GetDetails(page, rows, warehouseCode);
            return Json(warehouse, "text", JsonRequestBehavior.AllowGet);
        }


        //添加仓库信息表
        // POST: /Warehouse/WareCreate
        [HttpPost]
        public ActionResult WareCreate(Warehouse warehouse)
        {
            bool bResult = WarehouseService.Add(warehouse);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        
    }
}
