using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;

namespace Authority.Controllers.Wms.Inventory
{
    public class HistoricalDetailController : Controller
    {
        //
        // GET: /HistoricalDetail/

        [Dependency]
        public IHistoricalDetailService HistoricalDetailService { get; set; }

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string warehouseCode = collection["WarehouseCode"] ?? "";
            string productCode = collection["ProductCode"] ?? "";
            string beginDate = collection["BeginDate"] ?? "";
            string endDate = collection["EndDate"] ?? "";
            var HistoricalDetail = HistoricalDetailService.GetDetails(page, rows, warehouseCode, productCode, beginDate, endDate);
            return Json(HistoricalDetail, "text", JsonRequestBehavior.AllowGet);
        }
    }
}

