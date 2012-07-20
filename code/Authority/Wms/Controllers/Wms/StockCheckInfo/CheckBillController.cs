using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.StockCheckInfo
{
    public class CheckBillController : Controller
    {
        [Dependency]
        public ICheckBillMasterService CheckBillMasterService { get; set; }

        [Dependency]
        public ICheckBillDetailService CheckBillDetailService { get; set; }

        //
        // GET: /CheckBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //获取盘点BIllNO
        // POST: /CheckBill/GetCheckBillNo
        [HttpPost]
        public ActionResult GetCheckBillNo()
        {
            var area = CheckBillMasterService.GetCheckBillNo();
            return Json(area, "text", JsonRequestBehavior.AllowGet);
        }

        //根据货位添加盘点单主表和细表数据
        // POST: /CheckBill/CheckCreate/       
        public ActionResult CheckCreate(string wareCodes, string areaCodes, string shelfCodes, string cellCodes)
        {
            bool bResult = CheckBillMasterService.CellAdd(wareCodes, areaCodes, shelfCodes, cellCodes);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }        
    }
}
