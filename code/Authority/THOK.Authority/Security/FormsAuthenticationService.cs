using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace THOK.Authority.Security
{
    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为 null 或为空。", "userName");
            FormsAuthentication.SetAuthCookie(userName, true);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }
}