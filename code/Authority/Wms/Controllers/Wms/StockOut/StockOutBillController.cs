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
            string BillDate = collection["BillDate"] ?? "";
            string OperatePersonCode = collection["OperatePersonCode"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var inBillMaster = OutBillMasterService.GetDetails(page, rows, BillNo, BillDate, OperatePersonCode, Status, IsActive);
            return Json(inBillMaster, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /InBillDetail/InBillDetails/

        public ActionResult InBillDetails(int page, int rows, string BillNo)
        {
            var inBillDetail = OutBillDetailService.GetDetails(page, rows, BillNo);
            return Json(inBillDetail, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /InBillMaster/GenInBillNo/

        public ActionResult GenInBillNo()
        {
            var inBillNo = OutBillMasterService.GenInBillNo();
            return Json(inBillNo, "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Create/

        [HttpPost]
        public ActionResult Create(OutBillMaster outBillMaster)
        {
            bool bResult = OutBillMasterService.Add(outBillMaster);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Edit/

        [HttpPost]
        public ActionResult Edit(OutBillMaster outBillMaster)
        {
            bool bResult = OutBillMasterService.Save(outBillMaster);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillMaster/Delete/

        [HttpPost]
        public ActionResult Delete(string BillNo)
        {
            bool bResult = OutBillMasterService.Delete(BillNo);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /InBillDetail/OutBillDetailCreate/

        //[HttpPost]
        //public ActionResult OutBillDetailCreate(OutBillMaster outBillDetail)
        //{
        //    bool bResult = OutBillDetailService.Add(outBillDetail);
        //    string msg = bResult ? "新增成功" : "新增失败";
        //    return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        //}
    }
}
