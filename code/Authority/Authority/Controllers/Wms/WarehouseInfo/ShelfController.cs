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


        //添加货架信息表
        // POST: /Company/ShelfCreate
        [HttpPost]
        public ActionResult ShelfCreate(Shelf shelf)
        {
            bool bResult = ShelfService.Add(shelf);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
