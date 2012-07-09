using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.Wms.ProfitLossInfo
{
    public class ProfitLossBillVerifyController : Controller
    {
        //
        // GET: /ProfitLossBillConfirm/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasAudit = true;
            ViewBag.hasAntiTrial = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            return View();
        }

    }
}
