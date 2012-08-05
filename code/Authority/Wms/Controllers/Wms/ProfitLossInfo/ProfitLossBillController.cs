using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.ProfitLossInfo
{
    public class ProfitLossBillController : Controller
    {
        [Dependency]
        public IProfitLossBillMasterService ProfitLossBillMasterService { get; set; }
        [Dependency]
        public IProfitLossBillDetailService ProfitLossBillDetailService { get; set; }
        //
        // GET: /ProfitLossBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasAudit = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /ProfitLossBillMaster/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var profitLossBillMaster = ProfitLossBillMasterService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status, IsActive);
            return Json(profitLossBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /ProfitLossBillMaster/GenMoveBillNo/

        public ActionResult GenProfitLossBillNo()
        {
            var profitLossBillNo = ProfitLossBillMasterService.GenProfitLossBillNo(this.User.Identity.Name.ToString());
            return Json(profitLossBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Create/

        [HttpPost]
        public ActionResult Create(ProfitLossBillMaster profitLossBillMaster)
        {
            bool bResult = ProfitLossBillMasterService.Add(profitLossBillMaster, this.User.Identity.Name.ToString());
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Edit/

        [HttpPost]
        public ActionResult Edit(ProfitLossBillMaster profitLossBillMaster)
        {
            bool bResult = ProfitLossBillMasterService.Save(profitLossBillMaster);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Delete/

        [HttpPost]
        public ActionResult Delete(string BillNo)
        {
            bool bResult = ProfitLossBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillMaster/Audit/

        [HttpPost]
        public ActionResult Audit(string BillNo)
        {
            bool bResult = ProfitLossBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString());
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /ProfitLossBillDetail/ProfitLossBillDetails/

        public ActionResult ProfitLossBillDetails(int page, int rows, string BillNo)
        {
            var moveBillDetail = ProfitLossBillDetailService.GetDetails(page, rows, BillNo);
            return Json(moveBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillDetail/ProfitLossBillDetailDelete/

        public ActionResult ProfitLossBillDetailDelete(string ID)
        {
            bool bResult = ProfitLossBillDetailService.Delete(ID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillDetail/ProfitLossBillDetailCreate/

        [HttpPost]
        public ActionResult ProfitLossBillDetailCreate(ProfitLossBillDetail profitLossBillDetail)
        {
            string strResult = string.Empty;
            bool bResult = ProfitLossBillDetailService.Add(profitLossBillDetail, out strResult);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /ProfitLossBillDetail/ProfitLossBillDetailEdit/

        [HttpPost]
        public ActionResult ProfitLossBillDetailEdit(ProfitLossBillDetail profitLossBillDetail)
        {
            bool bResult = ProfitLossBillDetailService.Save(profitLossBillDetail);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
