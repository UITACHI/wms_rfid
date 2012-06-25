using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.WebUtil;

namespace Authority.Controllers.Authority
{
    public class RoleController : Controller
    {
        [Dependency]
        public IRoleService RoleService { get; set; }

        // GET: /Role/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.hasPermissionAdmin = true;
            ViewBag.hasUserAdmin = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /Role/Details/
        public ActionResult Details(int page, int rows,FormCollection collection)
        {
            string roleName = collection["RoleName"] ?? "";
            string description = collection["Description"] ?? "";
            string status = collection["Status"] ?? "";
            var roles = RoleService.GetDetails(page, rows, roleName, description, status);
            return Json(roles,"text",JsonRequestBehavior.AllowGet);
        }    

        // POST: /Role/Create/
        [HttpPost]
        public ActionResult Create(string roleName, string description, bool status)
        {
            bool bResult = RoleService.Add(roleName, description, status);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/Edit/
        [HttpPost]
        public ActionResult Edit(string roleID, string roleName, string description, bool status)
        {
            bool bResult = RoleService.Save(roleID, roleName, description, status);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Role/Delete/
        [HttpPost]
        public ActionResult Delete(string roleID)
        {
            bool bResult = RoleService.Delete(roleID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
