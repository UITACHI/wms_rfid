using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.StockIn
{
    public class StockInBillController : Controller
    {
        [Dependency]
        public IInBillMasterService InBillMasterService { get; set; }
        [Dependency]
        public IInBillDetailService InBillDetailService { get; set; }
        //
        // GET: /StockInBill/

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

        //
        // GET: /InBillMaster/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillNo = collection["BillNo"] ?? "";
            string WareHouseCode = collection["WareHouseCode"] ?? "";
            string BeginDate = collection["BeginDate"] ?? "";
            string EndDate = collection["EndDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? string.Empty;
            string CheckPersonCode = collection["CheckPersonCode"] ?? string.Empty;
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var inBillMaster = InBillMasterService.GetDetails(page, rows, BillNo, WareHouseCode, BeginDate, EndDate,OperatePersonCode,CheckPersonCode, Status, IsActive);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /InBillDetail/InBillDetails/

        public ActionResult InBillDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = InBillDetailService.GetDetails(page,rows,BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /InBillMaster/GenInBillNo/

        public ActionResult GenInBillNo()
        {
            var inBillNo = InBillMasterService.GenInBillNo(this.User.Identity.Name.ToString());
            return Json(inBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Create/

        [HttpPost]
        public ActionResult Create(InBillMaster inBillMaster)
        {
            bool bResult = InBillMasterService.Add(inBillMaster, this.User.Identity.Name.ToString());
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Edit/

        [HttpPost]
        public ActionResult Edit(InBillMaster inBillMaster)
        {
            bool bResult = InBillMasterService.Save(inBillMaster);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Delete/
        public ActionResult Delete(string BillNo)
        {
            bool bResult = InBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillDetail/InBillDetailCreate/

        [HttpPost]
        public ActionResult InBillDetailCreate(InBillDetail inBillDetail)
        {
            bool bResult = InBillDetailService.Add(inBillDetail);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillDetail/InBillDetailDelete/

        [HttpPost]
        public ActionResult InBillDetailDelete(string ID)
        {
            bool bResult = InBillDetailService.Delete(ID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Audit/
        public ActionResult Audit(string BillNo)
        {
            bool bResult = InBillMasterService.Audit(BillNo, this.User.Identity.Name.ToString());
            string msg = bResult ? "审核成功" : "审核失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/AntiTria/
        public ActionResult AntiTrial(string BillNo)
        {
            bool bResult = InBillMasterService.AntiTrial(BillNo);
            string msg = bResult ? "反审成功" : "反审失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillDetail/InBillDetailEdit/

        [HttpPost]
        public ActionResult InBillDetailEdit(InBillDetail inBillDetail)
        {
            bool bResult = InBillDetailService.Save(inBillDetail);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/GetBillTypeDetail/

        [HttpPost]
        public ActionResult GetBillTypeDetail(string BillClass, string IsActive)
        {
            var billType = InBillMasterService.GetBillTypeDetail(BillClass,IsActive);
            return Json(billType, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/GetWareHouseDetail/

        [HttpPost]
        public ActionResult GetWareHouseDetail( string IsActive)
        {
            var wareHouse = InBillMasterService.GetWareHouseDetail(IsActive);
            return Json(wareHouse, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillDetail/GetProductDetails/

        [HttpPost]
        public ActionResult GetProductDetails(int page, int rows, string QueryString, string Value)
        {
            if (QueryString==null)
            {
                QueryString = "ProductCode";
            }
            if (Value==null)
            {
                Value = "";
            }
            var product = InBillDetailService.GetProductDetails(page,rows,QueryString,Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/inBillMasterSettle/

        public ActionResult InBillMasterSettle(string BillNo)
        {
            string strResult = string.Empty;
            bool bResult = InBillMasterService.Settle(BillNo, out strResult);
            string msg = bResult ? "结单成功" : "结单失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, strResult), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
