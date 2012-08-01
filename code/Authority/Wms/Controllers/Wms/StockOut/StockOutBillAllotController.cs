using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Allot.Interfaces;

namespace Authority.Controllers.Wms.StockOut
{
    public class StockOutBillAllotController : Controller
    {
        [Dependency]
        public IOutBillAllotService OutBillAllotService { get; set; }

        public ActionResult Search(string billNo, int page, int rows)
        {
            var result = OutBillAllotService.Search(billNo, page, rows);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
