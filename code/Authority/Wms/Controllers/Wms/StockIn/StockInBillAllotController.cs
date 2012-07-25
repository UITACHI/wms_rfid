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

        public ActionResult Allot(string billNo, string areaCode)
        {
            string[] areaCodes = new string[] { };
            bool bResult = InBillAllotService.Allot(billNo, areaCodes);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
