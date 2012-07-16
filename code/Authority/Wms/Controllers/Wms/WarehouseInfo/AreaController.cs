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
        public ActionResult Details(int page, int rows, string areaCode)
        {
            var area = AreaService.GetDetails(page, rows, areaCode);
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
    }
}
