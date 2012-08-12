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
            //ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasAudit = true;
            ViewBag.hasConfirm = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //获取盘点BIllNO
        // POST: /CheckBill/GetCheckBillNo
        public ActionResult GetCheckBillNo()
        {
            var area = CheckBillMasterService.GetCheckBillNo();
            return Json(area, "text", JsonRequestBehavior.AllowGet);
        }

        //根据货位生成盘点单主表和细表数据
        // POST: /CheckBill/CheckCellCreate/       
        public ActionResult CheckCellCreate(string wareCodes, string areaCodes, string shelfCodes, string cellCodes,string billType)
        {
            string info = string.Empty;
            bool bResult = CheckBillMasterService.CellAdd(wareCodes, areaCodes, shelfCodes, cellCodes,this.User.Identity.Name.ToString(),billType, out info);
            string msg = bResult ? "新增成功" : "新增失败";            
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }

        //根据产品生成盘点单主表和细表数据
        // POST: /CheckBill/CheckProductCreate/       
        public ActionResult CheckProductCreate(string products,string billType)
        {
            string info = string.Empty;
            bool bResult = CheckBillMasterService.ProductAdd(products, this.User.Identity.Name.ToString(),billType, out info);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }

        //根据产品生成盘点单主表和细表数据
        // POST: /CheckBill/CheckChangedCreate/       
        public ActionResult CheckChangedCreate(string beginDate, string endDate,string billType)
        {
            string info = string.Empty;
            bool bResult = CheckBillMasterService.ChangedAdd(beginDate, endDate, this.User.Identity.Name.ToString(),billType, out info);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, info), "text", JsonRequestBehavior.AllowGet);
        }

        //查询货位要预览的盘点数据表
        // POST: /CheckBill/CheckCellDetails/
        public ActionResult CheckCellDetails(int page, int rows, string ware, string area, string shelf, string cell)
        {
            var storage = CheckBillMasterService.GetCellDetails(page, rows, ware, area, shelf, cell);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询产品要预览的盘点数据表
        // POST: /CheckBill/CheckProductDetails/        
        public ActionResult CheckProductDetails(int page, int rows, string products)
        {
            var storage = CheckBillMasterService.GetProductDetails(page, rows, products);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询异动要预览的盘点数据表
        // POST: /CheckBill/CheckChangedDetails/
        public ActionResult CheckChangedDetails(int page, int rows, string beginDate, string endDate)
        {
            var storage = CheckBillMasterService.GetChangedCellDetails(page, rows, beginDate, endDate);
            return Json(storage, "text", JsonRequestBehavior.AllowGet);
        }

        //查询主单
        // GET: /CheckBill/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string beginDate = collection["beginDate"] ?? "";
            string endDate = collection["endDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var inBillMaster = CheckBillMasterService.GetDetails(page, rows, BillNo,beginDate,endDate, OperatePersonCode, Status, IsActive);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //查询细单
        // GET: /CheckBill/CheckBillDetails/
        public ActionResult CheckBillDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = CheckBillDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //删除主单
        // POST: /CheckBill/Delete/
        public ActionResult Delete(string BillNo)
        {
            bool bResult = CheckBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单审核
        // POST: /CheckBill/checkBillMasterAudit/
        public ActionResult checkBillMasterAudit(string BillNo)
        {
            bool bResult = CheckBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString());
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单反审
        // POST: /CheckBill/checkBillMasterAntiTrial/
        public ActionResult checkBillMasterAntiTrial(string BillNo)
        {
            bool bResult = CheckBillMasterService.AntiTrial(BillNo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //主单确认
        // POST: /CheckBill/checkBillMasterConfirm/
        public ActionResult checkBillMasterConfirm(string BillNo)
        {
            string errorInfo=string.Empty;
            bool bResult = CheckBillMasterService.confirmCheck(BillNo, this.User.Identity.Name.ToString(), out errorInfo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
