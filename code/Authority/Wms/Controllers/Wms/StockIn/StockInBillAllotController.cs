using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Allot.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;

namespace Authority.Controllers.Wms.StockIn
{
    public class StockInBillAllotController : Controller
    {        
        [Dependency]
        public IInBillAllotService InBillAllotService { get; set; }

        public ActionResult Search(string billNo,int page, int rows)
        {
            var result = InBillAllotService.Search(billNo,page,rows);
            return Json(result, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
