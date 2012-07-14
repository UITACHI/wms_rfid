using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces;

namespace Authority.Controllers.Authority
{
    public class SystemEventLogController : Controller
    {
        //
        // GET: /LoginLog/
        [Dependency]
        public ISystemEventLogService SystemEventLogService { get; set; }

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
            string eventlogtime = collection["EventLogTime"] ?? "";
            string eventtype = collection["EventType"] ?? "";
            string eventname = collection["EventName"] ?? "";
            string frompc = collection["FromPC"] ?? "";
            string operateuser = collection["OperateUser"] ?? "";
            string targetsystem = collection["TargetSystem"] ?? "";
            var users = SystemEventLogService.GetDetails(page, rows, eventlogtime, eventtype, eventname, frompc, operateuser, targetsystem);
            return Json(users, "text", JsonRequestBehavior.AllowGet);

        }


    }
}

