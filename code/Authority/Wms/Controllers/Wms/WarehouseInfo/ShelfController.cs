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

        //添加货架信息表
        // POST: /Shelf/ShelfCreate
        [HttpPost]
        public ActionResult ShelfCreate(Shelf shelf)
        {
            bool bResult = ShelfService.Add(shelf);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
