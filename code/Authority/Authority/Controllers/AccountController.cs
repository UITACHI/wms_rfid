using System.Web.Mvc;
using System.Web.Routing;
using THOK.WebUtil;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Security;

namespace Authority.Controllers
{
    public class AccountController : Controller
    {
        [Dependency]
        public IFormsAuthenticationService FormsService { get; set; }
        [Dependency]
        public IUserService UserService { get; set; }

        [HttpPost]
        public ActionResult LogOn(string userName, string password, string cityId, string systemId, string serverId)
        {
            bool bResult = false;
            string msg = "";
            if(UserService.ValidateUser(userName, password))
            {
                if (UserService.ValidateUserPermission(userName, cityId, systemId))
                {
                    bResult = true;
                    msg = "登录成功!";
                }
                else
                {
                    msg = "登录失败:当前用户没有访问请求的系统服务器的权限!";
                }
            }
            else
            {
                msg = "登录失败:用户名或密码错误！";
            }
            string url = bResult ? UserService.GetLogOnUrl(User, cityId, systemId, serverId) : "";            
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, url),"text");
        }

        public ActionResult LogOn(string logOnKey)
        {
            //todo
            string userName = "Admin";
            string password = "a";
            string cityId = "F8344F88-08AD-4F9A-8F45-EAD8BB471105";
            string systemId = "ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8";

            bool bResult = UserService.ValidateUser(userName, password)
                && UserService.ValidateUserPermission(userName, cityId, systemId);
            if (bResult)
            {
                FormsService.SignIn(userName, false);
                this.AddCookie("cityid", cityId);
                this.AddCookie("systemid", systemId);
            }
            return new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" } });
        }

        public ActionResult LogOff()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(string userName, string password,string newPassword)
        {
            bool bResult = UserService.ChangePassword(userName, password, newPassword); 
            string msg = bResult ? "修改密码成功" : "修改密码失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult,msg),"text");
        }

        [Authorize]
        public ActionResult ChangeServer(string cityId, string systemId, string serverId)
        {
            bool bResult = false;
            string msg = "";
            string userName = this.User.Identity.Name;
            cityId = cityId ?? this.GetCookieValue("cityid");
            systemId = systemId ?? this.GetCookieValue("systemid");

            if (UserService.ValidateUserPermission(userName,cityId, systemId))
            {
                bResult = true;                
                msg = "切换成功!";
            }
            else
            {
                msg = "切换失败:当前用户没有访问请求的系统服务器的权限!";
            }

            this.AddCookie("c", cityId ?? "NULL");
            this.AddCookie("s", systemId ?? "NULL");
            this.AddCookie("ss", serverId ?? "NULL");

            string url = bResult ? UserService.GetLogOnUrl(this.User, cityId, systemId, serverId) : "";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, url),"text", JsonRequestBehavior.AllowGet);
        }
    }
}
