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

        //
        // GET: /Warehouse/Details/
        public ActionResult Details(string shelfCode)
        {
            var wareCell = CellService.GetSearch(shelfCode);
            return Json(wareCell, "text", JsonRequestBehavior.AllowGet);
        }

        //添加货位信息表
        // POST: /Company/CellCreate
        [HttpPost]
        public ActionResult CellCreate(Cell cell)
        {
            bool bResult = CellService.Add(cell);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
