using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.StockMove
{
    public class StockMoveBillTypeController : Controller
    {
        //
        // GET: /MovePositionBillType/

        [Dependency]
        public IBillTypeService BillTypeService { get; set; }
        //
        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            return View();
        }
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string BillTypeCode = collection["BillTypeCode"] ?? "";
            string BillTypeName = collection["BillTypeName"] ?? "";
            string BillClass = "0003";
            string IsActive = collection["IsActive"] ?? "";
            var brand = BillTypeService.GetDetails(page, rows, BillTypeCode, BillTypeName, BillClass, IsActive);
            return Json(brand, "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Create(BillType billtype)
        {
            bool bResult = BillTypeService.Add(billtype);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Edit(BillType billtype)
        {
            bool bResult = BillTypeService.Save(billtype);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(string billtypeCode)
        {
            bool bResult = BillTypeService.Delete(billtypeCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
