using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Text;
using THOK.Authority.Security;
using THOK.Authority.Authorize;

namespace Authority.Controllers
{
    public class AccountController : Controller
    {

        public IFormsAuthenticationService _FormsService { get; set; }
        public IUserService _UserService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (_FormsService == null) { _FormsService = new FormsAuthenticationService(); }
            if (_UserService == null) { _UserService = new UserService(); }
            base.Initialize(requestContext);
        }

        public ActionResult LogOn(string userName, string password)
        {
            JsonResult jr = new JsonResult();
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (ModelState.IsValid)
            {
                bool bResult = _UserService.ValidateUser(userName, password);
                jr.Data = new { success = bResult, msg = bResult ? "登录成功" : "登录失败", href = "http://localhost:5618/" };
                if (bResult)
                {
                    _FormsService.SignIn(userName, true);                    
                }
            }
            else
                jr.Data = new { success = false, msg = "登录失败" };
            jr.ContentType = "text";
            return jr;
        }

        public ActionResult LogOff()
        {
            _FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string userName, string password,string newPassword)
        {
            JsonResult jr = new JsonResult();
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            if (ModelState.IsValid)
            {
                bool bResult = _UserService.ChangePassword(userName, password, newPassword);
                jr.Data = new { success = bResult, msg = bResult ? "修改密码成功" : "修改密码失败" };
                if (bResult)
                    _FormsService.SignIn(userName, true);
            }
            else
                jr.Data = new { success = false, msg = "修改密码失败" };
            return jr;
        }
        
        public ActionResult ChangeLogOnServer(string userName)
        {
            _FormsService.SignIn(userName, true);
            return new RedirectToRouteResult(new RouteValueDictionary {{ "controller", "Home" }});
        }
    }
}
