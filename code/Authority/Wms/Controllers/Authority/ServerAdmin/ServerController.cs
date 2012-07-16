using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Wms.Bll.Interfaces;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces;

namespace Authority.Controllers.ServerAdmin
{
    public class ServerController : Controller
    {
        [Dependency]
        public IServerService ServerService { get; set; }

        // GET: /Server/
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

        // GET: /Server/Details/
        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string serverName = collection["ServerName"] ?? "";
            string description = collection["Description"] ?? "";
            string url = collection["Url"] ?? "";
            string isActive = collection["IsActive"] ?? "";
            var users = ServerService.GetDetails(page, rows, serverName, description, url,isActive);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Server/Create
        [HttpPost]
        public ActionResult Create(string serverName, string description, string url,bool isActive,string cityID)
        {
            bool bResult = ServerService.Add(serverName, description, url, isActive, cityID);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Server/Edit/ 
        [HttpPost]
        public ActionResult Edit(string serverID, string serverName, string description, string url, bool isActive,string cityID)
        {
            bool bResult = ServerService.Save(serverID, serverName, description, url,isActive,cityID);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Server/Delete/
        [HttpPost]
        public ActionResult Delete(string serverID)
        {
            bool bResult = ServerService.Delete(serverID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /Server/GetDetailsServer/
        public ActionResult GetDetailsServer()
        {
            string cityID = this.GetCookieValue("cityid");
            string serverID = this.GetCookieValue("serverid");
            var server = ServerService.GetDetails(cityID, serverID);
            return Json(server, "text", JsonRequestBehavior.AllowGet);
        }
    }
}
