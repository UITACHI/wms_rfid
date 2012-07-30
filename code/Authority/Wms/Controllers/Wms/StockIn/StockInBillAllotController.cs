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
            string result = string.Empty;
            bool bResult = InBillAllotService.Allot(billNo, areaCodes,out result);
            string msg = bResult ? "分配成功,确认后生效！" : "分配过程因异常错误中止.";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, result), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
