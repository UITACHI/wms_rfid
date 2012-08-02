using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.StockMove
{
    public class StockMoveBillController : Controller
    {
        [Dependency]
        public IMoveBillMasterService MoveBillMasterService { get; set; }
        [Dependency]
        public IMoveBillDetailService MoveBillDetailService { get; set; }
        //
        // GET: /MovePositionBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //
        // GET: /MoveBillMaster/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var moveBillMaster = MoveBillMasterService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status, IsActive);
            return Json(moveBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /MoveBillDetail/MoveBillDetails/

        public ActionResult MoveBillDetails(int page, int rows, string BillNo)
        {
            var moveBillDetail = MoveBillDetailService.GetDetails(page, rows, BillNo);
            return Json(moveBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /MoveBillMaster/GenMoveBillNo/

        public ActionResult GenMoveBillNo()
        {
            var moveBillNo = MoveBillMasterService.GenMoveBillNo(this.User.Identity.Name.ToString());
            return Json(moveBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillMaster/Create/

        [HttpPost]
        public ActionResult Create(MoveBillMaster moveBillMaster)
        {
            bool bResult = MoveBillMasterService.Add(moveBillMaster, this.User.Identity.Name.ToString());
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillMaster/Edit/

        [HttpPost]
        public ActionResult Edit(MoveBillMaster moveBillMaster)
        {
            bool bResult = MoveBillMasterService.Save(moveBillMaster);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillMaster/Delete/

        [HttpPost]
        public ActionResult Delete(string BillNo)
        {
            bool bResult = MoveBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillMaster/Audit/

        [HttpPost]
        public ActionResult Audit(string BillNo)
        {
            bool bResult = MoveBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString());
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillMaster/AntiTrial/

        [HttpPost]
        public ActionResult AntiTrial(string BillNo)
        {
            bool bResult = MoveBillMasterService.AntiTrial(BillNo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillDetail/MoveBillDetailDelete/

        public ActionResult MoveBillDetailDelete(string ID)
        {
            bool bResult = MoveBillDetailService.Delete(ID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillDetail/MoveBillDetailCreate/

        [HttpPost]
        public ActionResult MoveBillDetailCreate(MoveBillDetail moveBillDetail)
        {
            bool bResult = MoveBillDetailService.Add(moveBillDetail);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MoveBillDetail/MoveBillDetailEdit/

        [HttpPost]
        public ActionResult MoveBillDetailEdit(MoveBillDetail moveBillDetail)
        {
            bool bResult = MoveBillDetailService.Save(moveBillDetail);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
