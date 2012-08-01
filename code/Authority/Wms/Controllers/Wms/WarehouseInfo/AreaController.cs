using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.DbModel;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;

namespace Authority.Controllers.Wms.WarehouseInfo
{
    public class AreaController : Controller
    {
        [Dependency]
        public IAreaService AreaService { get; set; }
        //
        // GET: /Area/

        public ActionResult Index()
        {
            return View();
        }

        //查询库区信息表
        // POST: /Area/Details
        [HttpPost]
        public ActionResult Details(string warehouseCode, string areaCode)
        {
            var area = AreaService.GetDetails(warehouseCode, areaCode);
            return Json(area, "text", JsonRequestBehavior.AllowGet);
        }

        //查询库区信息表
        // POST: /Area/FindArea
        [HttpPost]
        public ActionResult FindArea(string parameter)
        {
            var area = AreaService.FindArea(parameter);
            return Json(area, "text", JsonRequestBehavior.AllowGet);
        }

        //添加库区信息表
        // POST: /Area/AreaCreate
        [HttpPost]
        public ActionResult AreaCreate(Area area)
        {
            bool bResult = AreaService.Add(area);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //查询仓库数据tree结构,用于盘点
        // GET: /Area/GetDetailsTree/
        public ActionResult GetDetailsTree()
        {
            var wareArea = AreaService.GetWareArea();
            return Json(wareArea, "text", JsonRequestBehavior.AllowGet);
        }

        //编辑库区表
        // GET: /Area/Edit/
        public ActionResult Edit(Area area)
        {
            bool bResult = AreaService.Save(area);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除库区表
        // POST: /Area/Delete/
        [HttpPost]
        public ActionResult Delete(string areaCode)
        {
            bool bResult = AreaService.Delete(areaCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
