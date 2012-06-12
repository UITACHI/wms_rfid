using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Routing;
using THOK.Authority.Security;

namespace THOK.Authority.Authorize
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class FormsAuthorizeAttribute : AuthorizeAttribute
    {
        public string FunctionID { get; set; }
        private IUserService _UserService = new UserService();
        private IRoleService _RoleService = new RoleService();

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User == null || !(filterContext.HttpContext.User.Identity is FormsIdentity) || !filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "Home" }, { "action", "LogOn" }, { "returnUrl", filterContext.HttpContext.Request.Url.PathAndQuery } });
            }
            else
            {
                Roles = _RoleService.FindRolesForFunction(FunctionID);
                Users = _UserService.FindUsersForFunction(FunctionID);

                base.OnAuthorization(filterContext);
                if (filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.Result = new RedirectResult("~/Home/Unauthorized");
                }
            }
        }
    }
}