using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
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

        //查询货架信息表
        // POST: /Cell/FindCell
        [HttpPost]
        public ActionResult FindCell(string parameter)
        {
            var cell = CellService.FindCell(parameter);
            return Json(cell, "text", JsonRequestBehavior.AllowGet);
        }


        //
        // GET: /Cell/Details/
        public ActionResult Details(string wareCode,string id)
        {
            var wareCell = CellService.GetSearch(wareCode, id);
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

        //编辑货位表
        // GET: /Cell/Edit/
        public ActionResult Edit(Cell cell)
        {
            bool bResult = CellService.Save(cell);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除货位表
        // POST: /Cell/Delete/
        [HttpPost]
        public ActionResult Delete(string cellCode)
        {
            bool bResult = CellService.Delete(cellCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
