using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces.Wms;
using THOK.RfidWms.DBModel.Ef.Models.Wms;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.WarehouseInfo
{
    public class CellController : Controller
    {
        [Dependency]
        public ICellService CellService { get; set; }

        //
        // GET: /Cell/

        public ActionResult Index()
        {
            return View();
        }

        //查询货架信息表
        // POST: /Cell/CellDetails
        [HttpPost]
        public ActionResult CellDetails(int page, int rows, string cellCode)
        {
            var cell = CellService.GetDetails(page, rows, cellCode);
            return Json(cell, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Cell/Details/
        public ActionResult Details(string shelfCode)
        {
            var wareCell = CellService.GetSearch(shelfCode);
            return Json(wareCell, "text", JsonRequestBehavior.AllowGet);
        }

        //添加货位信息表
        // POST: /Cell/CellCreate
        [HttpPost]
        public ActionResult CellCreate(Cell cell)
        {
            bool bResult = CellService.Add(cell);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
