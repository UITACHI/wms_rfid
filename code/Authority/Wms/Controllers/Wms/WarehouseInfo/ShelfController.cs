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
    public class ShelfController : Controller
    {
        [Dependency]
        public IShelfService ShelfService { get; set; }
        //
        // GET: /Shelf/

        public ActionResult Index()
        {
            return View();
        }

        //查询货架信息表
        // POST: /Shelf/Details
        [HttpPost]
        public ActionResult Details(int page, int rows, string shelfCode)
        {
            var shelf = ShelfService.GetDetails(page, rows, shelfCode);
            return Json(shelf, "text", JsonRequestBehavior.AllowGet);
        }

        //查询货架信息表
        // POST: /Shelf/FindShelf
        [HttpPost]
        public ActionResult FindShelf(string parameter)
        {
            var shelf = ShelfService.FindShelf(parameter);
            return Json(shelf, "text", JsonRequestBehavior.AllowGet);
        }

        //添加货架信息表
        // POST: /Shelf/ShelfCreate
        [HttpPost]
        public ActionResult ShelfCreate(Shelf shelf)
        {
            bool bResult = ShelfService.Add(shelf);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //编辑货架表
        // GET: /Shelf/Edit/
        public ActionResult Edit(Shelf shelf)
        {
            bool bResult = ShelfService.Save(shelf);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除货架表
        // POST: /Shelf/Delete/
        [HttpPost]
        public ActionResult Delete(string shelfCode)
        {
            bool bResult = ShelfService.Delete(shelfCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
