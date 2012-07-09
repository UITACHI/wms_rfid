using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.Wms.SortingInfo
{
    public class SortingOrderController : Controller
    {
        //
        // GET: /SortingOrder/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasDownload = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            return View();
        }

    }
}
