using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using System.Web;

namespace THOK.WebUtil
{
    public static class CookieHelper
    {
        public static void AddCookie(this Controller c,string key,string value)
        {
            string cookieValue = value;
            var cookie = c.HttpContext.Request.Cookies[key] ?? new HttpCookie(key, cookieValue);
            cookie.Value = cookieValue;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.HttpOnly = true;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            c.HttpContext.Response.Cookies.Remove(cookie.Name);
            c.HttpContext.Response.Cookies.Add(cookie);
        }

        public static void RemoveCookie(this Controller c, string key)
        {
            c.HttpContext.Response.Cookies.Remove(key);
        }

        public static string GetCookieValue(this Controller c, string key)
        {
            string strValue = string.Empty;
            var cookie = c.HttpContext.Request.Cookies[key];
            if (cookie != null)
            {
                strValue = cookie.Value;
            }
            return strValue;
        }
    }
}
