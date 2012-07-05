using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Authority.Controllers.WarehouseInfo
{
    public class WarehouseController : Controller
    {
        //
        // GET: /Warehouse/

        public ActionResult Index(string moduleID)
        {
            //ViewBag.hasSearch = true;
            //ViewBag.hasAdd = true;
            //ViewBag.hasEdit = true;
            //ViewBag.hasDelete = true;

            ViewBag.hasAddWarehouse = true;
            ViewBag.hasAddLibraryArea = true;
            ViewBag.hasAddShelf = true;
            ViewBag.hasAddCell = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

    }
}
