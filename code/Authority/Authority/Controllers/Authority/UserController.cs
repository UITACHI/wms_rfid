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
    public class UserController : Controller
    {
        public IUserService _UserService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (_UserService == null) { _UserService = new UserService(); }
            base.Initialize(requestContext);
        }

        //
        // GET: /User/

        public ActionResult Index()
        {
            //ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            //ViewBag.hasEdit = true;
            //ViewBag.hasDelete = true;
            //ViewBag.hasPrint = true;
            //ViewBag.hasHelp = true;

            //ViewBag.hasPermissionAdmin = true;
            //ViewBag.hasRoleAdmin = true;
            //ViewBag.hasAuthorize = true;

            return View();
        }

        /// 
        ///GET: /User/Permission

        public ActionResult Permission()
        {
            return View();
        }

        //
        // GET: /User/Details/5

        public ActionResult Details(int page, int rows)
        {
            JsonResult jr = new JsonResult();
            jr.Data = _UserService.GetDetails(page, rows);
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

     

        //
        // POST: /User/Create

        [HttpPost]
        public ActionResult Create(string userName, string pwd, string ChineseName, bool isLock, bool isAdmin, string loginPc, string memo)
        {
            try
            {
                JsonResult jr = new JsonResult();
                pwd = pwd.Trim().Equals("") ? "123456" : pwd;
                jr.Data = _UserService.AddUser(userName, pwd, ChineseName, isLock, isAdmin, loginPc, memo);
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
        // GET: /User/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

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

        ////
        //// GET: /User/Delete/5
 
        //public ActionResult Delete(string id)
        //{
        //    JsonResult jr = new JsonResult();
        //    jr.Data = _UserService.(id);
        //    jr.ContentEncoding = Encoding.UTF8;
        //    jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    return jr;
        //}     
    }
}
