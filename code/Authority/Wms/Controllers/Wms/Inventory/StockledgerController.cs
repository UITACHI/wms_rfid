using System.Web.Mvc;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;

namespace Authority.Controllers.Wms.Inventory
{
    public class StockledgerController : Controller
    {
        //
        // GET: /Stockledger/

        [Dependency]
        public IStockledgerService StockledgerService { get; set; }

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
            var Stockledger = StockledgerService.GetDetails(page, rows, warehouseCode, productCode, beginDate, endDate);
            return Json(Stockledger, "text", JsonRequestBehavior.AllowGet);
        }

    }
}
