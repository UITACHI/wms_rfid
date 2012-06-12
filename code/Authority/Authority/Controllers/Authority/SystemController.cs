using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;
using THOK.Authority.Authority;

namespace Authority.Controllers.Authority
{
    public class SystemController : Controller
    {
        public ISystemService _SystemService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (_SystemService == null) { _SystemService = new SystemService(); }
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
            return View("Index");
        }

        public ActionResult Details(int page, int rows,FormCollection collection)
        {
            string systemName = collection["SystemName"]??"";
            string description = collection["Description"] ?? "";
            string status = collection["Status"] ?? "";

            object details = _SystemService.GetDetails(page, rows, systemName, description, status);
            JsonResult jr = new JsonResult();
            jr.Data = details;
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult Create(string systemName, string description, bool status)
        {
            bool bResult = _SystemService.AddSystem(systemName, description, status);
            JsonResult jr = new JsonResult();
            jr.Data = new { success = bResult, msg = bResult ? "新增成功": "新增失败"};
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult Edit(string systemId, string systemName, string description, bool status)
        {
            bool bResult = _SystemService.SaveSystemInfo(systemId, systemName, description, status);
            JsonResult jr = new JsonResult();
            jr.Data = new { success = bResult, msg = bResult ? "修改成功": "修改失败"};
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult Delete(string systemId)
        {
            bool bResult = _SystemService.Delete(systemId);
            JsonResult jr = new JsonResult();
            jr.Data = new { success = bResult, msg = bResult ? "删除成功": "删除失败"};
            jr.ContentEncoding = Encoding.UTF8;
            jr.ContentType = "text";
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
    }
}
