using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using THOK.Authority.Data;
using System.Web.Script.Serialization;

namespace THOK.Authority.Security
{
    public class UserService : IUserService
    {
        public UserService()
        {
        }

        public string FindUsersForFunction(string functionId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                Guid fid = new Guid(functionId);
                var users = context.User.Where(user
                    => (user.UserSystems.Where(userSystem
                        => (userSystem.UserModules.Where(userModule
                            => (userModule.UserFunctions.Where(userFunction
                                => userFunction.Function.FunctionID == fid)).Count() > 0)).Count() > 0)).Count() > 0);
                var usernames = users.Select(i => i.UserName);

                string result = "";
                foreach (var username in usernames)
                {
                    result += username + ",";
                }
                return result;
            }
        }

   
        public bool ChangePassword(string userName, string password, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var user = context.User.FirstOrDefault(i => i.UserName == userName);
                    if (user != null && ValidateUser(userName, password))
                    {
                        user.UserName = userName ?? user.UserName;
                        user.Pwd = newPassword != null ? EncryptPassword(newPassword) : user.Pwd;
                        context.SaveChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return true;
        }

        public bool ValidateUser(string userName, string password)
        {

            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");

            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var adduser = new User();
                    adduser.UserID = Guid.NewGuid();
                    adduser.UserName = userName;
                    adduser.Pwd = EncryptPassword(password);
                    adduser.IsLock = false;
                    adduser.IsAdmin = false;

                    var user = context.User.FirstOrDefault(i => i.UserName == userName);
                    if (user == null)
                    {
                        context.AddToUser(adduser);
                        context.SaveChanges();
                    }
                    return user != null && ComparePassword(password, user.Pwd);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #region 查找用户信息
        public object GetDetails(int page, int rows)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var user = from s in context.User
                               select new { s.UserID, s.UserName, IsLock = s.IsLock ? "锁定" : "未锁定", s.Memo };
                    return user.ToArray();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion
        #region delete删除用户信息
        public bool delete(string systemId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    Guid guserId = new Guid(systemId);//从子表开始删除 一直到system表

                    var userid = from userinfo in context.User
                                 where userinfo.UserID == guserId
                                 select userinfo;
                    var userSystem = from usersys in context.UserSystem
                                     where usersys.UserSystemID == guserId
                                     select usersys;
                    var sys = from system in context.System
                              where system.SystemID == guserId
                              select system;
                    if (userid.FirstOrDefault() != null)
                    {
                        context.DeleteObject(userSystem.First());
                        context.SaveChanges();
                    }
                    if (userid.FirstOrDefault() != null)
                    {
                        context.DeleteObject(userid.First());
                        context.SaveChanges();
                    }
                    if (userid.FirstOrDefault() != null)
                    {
                        context.DeleteObject(sys.First().UserSystems);
                        context.SaveChanges();
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }
                return true;
            }
        }
        #endregion
        private bool ComparePassword(string password, string hash)
        {
            return EncryptPassword(password) == hash;
        }

        private string EncryptPassword(string password)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        public string GetLogOnUrl(System.Security.Principal.IPrincipal iPrincipal, string cityId, string systemId, string serverId)
        {
            string url = "";
            string logOnKey = "";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            if (cityId != null && cityId != string.Empty)
            {
                url = GetUrlFromCity(new Guid(cityId));
            }

            if (serverId != null && serverId != string.Empty)
            {
                url = GetUrlFromServer(new Guid(cityId));
            }

            if (systemId != null && serverId != string.Empty)
            {

            }
            var key = new { User = iPrincipal, CityID = cityId, ServerID = serverId ,SystemID = systemId};
            logOnKey = serializer.Serialize(key);
            url += "/Account/LogOn/?LogOnKey=" + logOnKey;
            return url;
        }

        private string GetUrlFromServer(Guid guid)
        {
            throw new NotImplementedException();
        }

        private string GetUrlFromCity(Guid guid)
        {
            throw new NotImplementedException();
        }

        public bool ValidateUserPermission(string userName, string cityId, string systemId)
        {
            return true;
        }
    }    
}