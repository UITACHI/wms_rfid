using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Authority.Authority;
using System.Web.Routing;
using System.Text;

namespace Authority.Controllers.Authority
{
    public class RoleController : Controller
    {
        public IRoleService _RoleService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (_RoleService == null) { _RoleService = new RoleService(); }
            base.Initialize(requestContext);
        }

        //
        // GET: /Role/

        public ActionResult Index()
        {
            //ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            //ViewBag.hasEdit = true;
            //ViewBag.hasDelete = true;
            //ViewBag.hasPrint = true;
            //ViewBag.hasHelp = true;

            //ViewBag.hasPermissionAdmin = true;
            //ViewBag.hasUserAdmin = true;

            return View();
        }

        public ActionResult Permission()
        {
            return View();
        }
        //
        // GET: /Role/Details/5

        public ActionResult Details(int page, int rows)
        {
            JsonResult jr = new JsonResult();
            jr.Data = _RoleService.GetDetails(page, rows);
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

    

        //
        // POST: /Role/Create

        [HttpPost]
        public ActionResult Create(string roleName, string memo, bool islock)
        {
            try
            {
                JsonResult jr = new JsonResult();
                jr.Data = _RoleService.AddRole(roleName, memo, islock);
                jr.ContentEncoding = Encoding.UTF8;
                jr.ContentType = "text";
                jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jr;
            }
            catch
            {
                return View();
            }
        }
        
        //
        // GET: /Role/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Role/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Role/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Role/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
