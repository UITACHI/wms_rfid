using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.Wms.ComplexSearch
{
    public class StockCheckSearchController : Controller
    {
        //
        // GET: /StockCheckSearch/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasEdit = true;
            ViewBag.hasAdd = true;
            ViewBag.hasHelp = true;
            return View();
        }

    }
}
