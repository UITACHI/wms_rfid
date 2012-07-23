using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.StockOut
{
    public class StockOutBillController : Controller
    {

        [Dependency]
        public IOutBillMasterService OutBillMasterService { get; set; }
        [Dependency]
        public IOutBillDetailService OutBillDetailService { get; set; }
        //
        // GET: /StockOutBill/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasDownload = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasAllot = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;            
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //查询主单
        // GET: /StockOutBill/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var inBillMaster = OutBillMasterService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status, IsActive);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /StockOutBill/OutBillDetails/
        public ActionResult OutBillDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = OutBillDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //生成单号
        // GET: /StockOutBill/GenInBillNo/
        public ActionResult GenInBillNo()
        {
            var inBillNo = OutBillMasterService.GenInBillNo();
            return Json(inBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //新增主单
        // POST: /StockOutBill/Create/
        [HttpPost]
        public ActionResult Create(OutBillMaster outBillMaster)
        {
            bool bResult = OutBillMasterService.Add(outBillMaster);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //修改主单
        // POST: /StockOutBill/Edit/

        [HttpPost]
        public ActionResult Edit(OutBillMaster outBillMaster)
        {
            bool bResult = OutBillMasterService.Save(outBillMaster);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除主单
        // POST: /StockOutBill/Delete/
        [HttpPost]
        public ActionResult Delete(string BillNo)
        {
            bool bResult = OutBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //修改主单状态
        // POST: /StockOutBill/editoutBillMasterStatus/
        public ActionResult editoutBillMasterStatus(string BillNo, string status)
        {
            bool bResult = OutBillMasterService.UpdateBillMasterStatus(BillNo, status);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //新增细单
        // POST: /StockOutBill/OutBillDetailCreate/
        [HttpPost]
        public ActionResult OutBillDetailCreate(OutBillDetail outBillDetail)
        {
            bool bResult = OutBillDetailService.Add(outBillDetail);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除细单
        // POST: /StockOutBill/outBillDelete/
        public ActionResult outBillDelete(string BillNo, string ID)
        {
            bool bResult = OutBillDetailService.Delete(ID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //修改细单
        // POST: /StockOutBill/editOutBillDelete
        [HttpPost]
        public ActionResult editOutBillDelete(OutBillDetail outBillDetail)
        {
            bool bResult = OutBillDetailService.Save(outBillDetail);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单审核
        // POST: /StockOutBill/outBillMasterAudit/
        public ActionResult outBillMasterAudit(string BillNo)
        {
            bool bResult = OutBillMasterService.Audit(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单反审
        // POST: /StockOutBill/outBillMasterAntiTrial/
        public ActionResult outBillMasterAntiTrial(string BillNo)
        {
            bool bResult = OutBillMasterService.AntiTrial(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        
    }
}
