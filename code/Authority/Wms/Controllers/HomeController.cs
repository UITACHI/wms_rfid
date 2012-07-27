using System.Web.Mvc;
using System.Web.Script.Serialization;
using THOK.WebUtil;
using Microsoft.Practices.Unity;
using THOK.Security;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Bll.Models;

namespace Authority.Controllers
{
    public class HomeController : Controller
    {
        [Dependency]
        public IModuleService ModuleService { get; set; }
        [Dependency]
        public ICityService CityService { get; set; }
        [Dependency]
        public IServerService ServerService { get; set; }
        [Dependency]
        public ISystemService SystemService { get; set; }
        [Dependency]
        public IFormsAuthenticationService FormsService { get; set; }
        public ActionResult Index()
        {
            string cityId = this.GetCookieValue("cityid");
            string serverId = this.GetCookieValue("serverid");
            string systemId = this.GetCookieValue("systemid");
            if (!cityId.Equals(string.Empty) && !serverId.Equals(string.Empty) && !systemId.Equals(string.Empty))
            {
                ViewBag.CityName = CityService.GetCityByCityID(cityId).ToString();
                ViewBag.ServerName = ServerService.GetServerById(serverId).ToString();
                ViewBag.SystemName = SystemService.GetSystemById(systemId).ToString();
            }
            else
            {
                this.RemoveCookie(cityId);
                this.RemoveCookie(serverId);
                this.RemoveCookie(systemId);
                FormsService.SignOut();
            }
            return View();
        }

        public ActionResult GetUser()
        {
            return Json(User,"text", JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetMenu()
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var jmenus = serializer.Deserialize<Menu[]>(JsonHelper.getJsonMenu());

            var menus = ModuleService.GetUserMenus(User.Identity.Name,this.GetCookieValue("cityid"),this.GetCookieValue("systemid"));          
            return Json(menus,"text",JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult GetFun(string moduleId)
        {
            Fun fun = new Fun()
            {
                funs = new Fun[] { 
                new Fun() { funname = "search", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638", isActive = true },
                new Fun() { funname = "add", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "edit", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "delete", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                 new Fun() { funname = "functionadmin", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "permissionadmin", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "useradmin", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "roleadmin", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "authorize", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},                
                new Fun() { funname = "print", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638" , isActive = true},
                new Fun() { funname = "help", iconCls = "icon-search", funid = "EEB02601-9BF6-412F-A63E-92857BF38638", isActive = true }
                }
            };
            var funs = ModuleService.GetModuleFuns(User.Identity.Name, this.GetCookieValue("cityid"), moduleId);
            return Json(funs,"text",JsonRequestBehavior.AllowGet);
        }

        public ActionResult PageNotFound()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult AjaxPageNotFound()
        {
            return Json(JsonMessageHelper.getJsonMessage(false, "当前访问的服务不存在！", this.HttpContext.Request.RawUrl), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxServerError()
        {
            return Json(JsonMessageHelper.getJsonMessage(false, "当前访问的服务发生未处理的服务器错误！", this.HttpContext.Request.RawUrl), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxError()
        {
            return Json(JsonMessageHelper.getJsonMessage(false, "当前访问的服务发生未知的服务器错误！", this.HttpContext.Request.RawUrl), "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult AjaxUnauthorized()
        {
            return Json(JsonMessageHelper.getJsonMessage(false, "当前访问的服务验证失败，没有相应的权限！", this.HttpContext.Request.RawUrl), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
