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
            ViewBag.hasSettle = true;
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
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var outBillMaster = OutBillMasterService.GetDetails(page, rows, BillNo, beginDate,endDate, OperatePersonCode, Status, IsActive);
            return Json(outBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /StockOutBill/OutBillDetails/
        public ActionResult OutBillDetails(int page, int rows, string BillNo)
        {
            var outBillDetail = OutBillDetailService.GetDetails(page, rows, BillNo);
            return Json(outBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //生成单号
        // GET: /StockOutBill/GenInBillNo/
        public ActionResult GenInBillNo()
        {
            var outBillNo = OutBillMasterService.GenInBillNo(this.User.Identity.Name.ToString());
            return Json(outBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //新增主单
        // POST: /StockOutBill/Create/
        public ActionResult Create(OutBillMaster outBillMaster)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Add(outBillMaster, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //修改主单
        // POST: /StockOutBill/Edit/
        public ActionResult Edit(OutBillMaster outBillMaster)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Save(outBillMaster, out errorInfo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //删除主单
        // POST: /StockOutBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Delete(BillNo, out errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //新增细单
        // POST: /StockOutBill/OutBillDetailCreate/
        public ActionResult OutBillDetailCreate(OutBillDetail outBillDetail)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Add(outBillDetail, out errorInfo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //删除细单
        // POST: /StockOutBill/outBillDelete/
        public ActionResult outBillDelete(string BillNo, string ID)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Delete(ID, out errorInfo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //修改细单
        // POST: /StockOutBill/editOutBillDelete
        public ActionResult editOutBillDelete(OutBillDetail outBillDetail)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillDetailService.Save(outBillDetail, out errorInfo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单审核
        // POST: /StockOutBill/outBillMasterAudit/
        public ActionResult outBillMasterAudit(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单反审
        // POST: /StockOutBill/outBillMasterAntiTrial/
        public ActionResult outBillMasterAntiTrial(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.AntiTrial(BillNo, out errorInfo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);
        }

        //主单结单
        // POST: /StockOutBill/outBillMasterSettle/
        public ActionResult outBillMasterSettle(string BillNo)
        {
            string errorInfo = string.Empty;
            bool bResult = OutBillMasterService.Settle(BillNo, out errorInfo);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, errorInfo), "text", JsonRequestBehavior.AllowGet);

        }

    }
}
