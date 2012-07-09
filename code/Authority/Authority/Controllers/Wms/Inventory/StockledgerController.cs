using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.Wms.Inventory
{
    public class StockledgerController : Controller
    {
        //
        // GET: /Stockledger/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            return View();
        }

    }
}
