using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using THOK.Authority.Bll.Interfaces.Authority;
using Microsoft.Practices.Unity;
using THOK.WebUtil;

namespace Authority.Controllers.Authority
{
    public class SystemController : Controller
    {
        [Dependency]
        public ISystemService SystemService { get; set; }

        // GET: /System/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /System/Details/
        public ActionResult Details(int page, int rows,FormCollection collection)
        {
            string systemName = collection["SystemName"]??"";
            string description = collection["Description"] ?? "";
            string status = collection["Status"] ?? "";
            var systems = SystemService.GetDetails(page, rows, systemName, description, status);
            return Json(systems,"text",JsonRequestBehavior.AllowGet);
        }

        // POST: /System/Create/
        [HttpPost]
        public ActionResult Create(string systemName, string description, bool status)
        {
            bool bResult = SystemService.Add(systemName, description, status);
            string msg = bResult ? "新增成功": "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult,msg,null),"text",JsonRequestBehavior.AllowGet);
        }

        // POST: /System/Edit/
        [HttpPost]
        public ActionResult Edit(string systemId, string systemName, string description, bool status)
        {
            bool bResult = SystemService.Save(systemId, systemName, description, status);
            string msg = bResult ? "修改成功" : "修改失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /System/Delete/
        [HttpPost]
        public ActionResult Delete(string systemId)
        {
            bool bResult = SystemService.Delete(systemId);
            string msg = bResult ? "删除成功": "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
