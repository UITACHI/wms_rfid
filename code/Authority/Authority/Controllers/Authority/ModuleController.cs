using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using THOK.Authority.Authority;
using THOK.Authority.Security;
using System.Web.Routing;
using System.Web.Script.Serialization;

namespace Authority.Controllers.Authority
{
    public class ModuleController : Controller
    {
        public IModuleService _ModuleService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (_ModuleService == null) { _ModuleService = new ModuleService(); }
            base.Initialize(requestContext);
        }

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            return View();
        }

        public ActionResult Details(string systemId)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonResult jr = new JsonResult();
            jr.Data = _ModuleService.GetDetails(systemId);
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult Create(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemId, params string[] parentModuleID)
        {
            bool bResult = _ModuleService.AddModule(moduleName, showOrder, moduleUrl, indicateImage, desktopImage, systemId, parentModuleID != null ? parentModuleID[0] : null);
            JsonResult jr = new JsonResult();
            jr.Data = new { success = bResult, msg = bResult ? "新增成功" : "新增失败" };
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        
        public ActionResult Edit(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage)
        {
            bool bResult = _ModuleService.SaveModuleInfo(moduleID, moduleName, showOrder, moduleUrl, indicateImage, deskTopImage);
            JsonResult jr = new JsonResult();
            jr.Data = new { success = bResult, msg = bResult ? "修改成功" : "修改失败" };
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult Delete(string moduleId)
        {
            bool bResult = _ModuleService.Delete(moduleId);
            JsonResult jr = new JsonResult();
            jr.Data = new { success = bResult, msg = bResult ? "删除成功" : "删除失败" };
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
    }
}
