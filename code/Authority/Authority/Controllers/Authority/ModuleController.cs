using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using System.Web.Script.Serialization;
using THOK.Authority.Bll.Interfaces.Authority;
using Microsoft.Practices.Unity;
using THOK.WebUtil;

namespace Authority.Controllers.Authority
{
    public class ModuleController : Controller
    {
        [Dependency]
        public IModuleService ModuleService { get; set; }

        // GET: /Module/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasFunctionAdmin = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /Module/Details/
        public ActionResult Details(string systemId)
        {
            var modules = ModuleService.GetDetails(systemId);
            return Json(modules, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Create/
        [HttpPost]
        public ActionResult Create(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemId,string parentModuleID)
        {
            bool bResult = ModuleService.Add(moduleName, showOrder, moduleUrl, indicateImage, desktopImage, systemId, parentModuleID ?? null);
            string msg = bResult ? "新增成功" : "新增失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Edit/
        [HttpPost]
        public ActionResult Edit(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage)
        {
            bool bResult = ModuleService.Save(moduleID, moduleName, showOrder, moduleUrl, indicateImage, deskTopImage);
            string msg = bResult ? "修改成功" : "修改失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Delete/
        [HttpPost]
        public ActionResult Delete(string moduleId)
        {
            bool bResult = ModuleService.Delete(moduleId);
            string msg = bResult ? "删除成功" : "删除失败" ;
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
