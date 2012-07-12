using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using THOK.Authority.Dal.EntityRepository.Authority;
using System.Web.Script.Serialization;
using Microsoft.Practices.Unity;
using System.Linq;
using THOK.Authority.Dal.Interfaces;
using System.Data.Objects.DataClasses;
using System.Security.Cryptography;
using System.IO;
using THOK.Authority.Common;
using THOK.Authority.Bll.Models.Authority;

namespace THOK.Authority.Bll.Service.Authority
{
    public class UserService : ServiceBase<User>, IUserService
    {
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        [Dependency]
        public IRoleRepository RoleRepository { get; set; }
        [Dependency]
        public IUserRoleRepository UserRoleRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public ICityRepository CityRepository { get; set; }
        [Dependency]
        public IServerRepository ServerRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string userName, string chineseName, string isLock, string isAdmin, string memo)
        {
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.User> query = UserRepository.GetQueryable();
            var users = query.Where(i => i.UserName.Contains(userName)
                && i.ChineseName.Contains(chineseName)
                && i.Memo.Contains(memo))
                .OrderBy(i => i.UserID)
                .Select(i => new { i.UserID, i.UserName, i.ChineseName, i.Memo, IsLock = i.IsLock ? "是" : "否", IsAdmin = i.IsAdmin ? "是" : "否" });

            
            int total = users.Count();
            users = users.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = users.ToArray() };
        }

        public bool Add(string userName, string pwd, string chineseName, bool isLock, bool isAdmin, string memo)
        {
            var user = new User()
            {
                UserID = Guid.NewGuid(),
                UserName = userName,
                Pwd = EncryptPassword(pwd),
                ChineseName = chineseName,
                IsLock = isLock,
                IsAdmin = isAdmin,
                Memo = memo
            };
            UserRepository.Add(user);
            UserRepository.SaveChanges();
            return true;
        }

        public bool Delete(string userID)
        {
            Guid gUserID = new Guid(userID);
            var user = UserRepository.GetQueryable()
                .FirstOrDefault(u => u.UserID == gUserID);
            if (user != null)
            {
                Del(UserRoleRepository, user.UserRoles);
                Del(UserSystemRepository, user.UserSystems);
                UserRepository.Delete(user);
                UserRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string userID, string userName, string pwd, string chineseName, bool isLock, bool isAdmin, string memo)
        {
            Guid gUserID = new Guid(userID);
            var user = UserRepository.GetQueryable()
                .FirstOrDefault(u => u.UserID == gUserID);
            user.UserName = userName;
            user.Pwd = !string.IsNullOrEmpty(pwd) ? EncryptPassword(pwd) : user.Pwd;
            user.ChineseName = chineseName;
            user.IsLock = isLock;
            user.IsAdmin = isAdmin;
            user.Memo = memo;
            UserRepository.SaveChanges();
            return true;
        }

        public bool ValidateUser(string userName, string password)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为NULL或为空。", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("值不能为NULL或为空。", "password");

            var adduser = new User();
            adduser.UserID = Guid.NewGuid();
            adduser.UserName = userName;
            adduser.ChineseName = "";
            adduser.LoginPC = "";
            adduser.Memo = "";
            adduser.Pwd = EncryptPassword(password);
            adduser.IsLock = false;
            adduser.IsAdmin = false;

            var user = UserRepository.GetSingle(i => i.UserName == userName);
            if (user == null)
            {
                UserRepository.Add(adduser);
                UserRepository.SaveChanges();
            }
            return user != null && ComparePassword(password, user.Pwd);
        }

        public bool ValidateUserPermission(string userName, string cityId, string systemId)
        {
            return true;
        }

        public string GetLogOnUrl(string userName, string password, string cityId, string systemId, string serverId)
        {
            string url = "";
            string logOnKey = "";

            url = GetUrlFromCity(new Guid(cityId));

            if (string.IsNullOrEmpty(password))
            {
                IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.User> queryCity = UserRepository.GetQueryable();
                var user = queryCity.Single(c => c.UserName == userName);
                password = user.Pwd;
            }

            if (!string.IsNullOrEmpty(serverId))
            {
                url = GetUrlFromServer(new Guid(serverId)); 
            }                       
            var key = new UserLoginInfo()
                    {
                        CityID = cityId,
                        SystemID = systemId,
                        UserName = userName,
                        Password = password
                    };
            logOnKey = Des.EncryptDES((new JavaScriptSerializer()).Serialize(key),"12345678");
            url += @"/Account/LogOn/?LogOnKey=" + Uri.EscapeDataString(logOnKey);
            return url;
        }

        public bool ChangePassword(string userName, string password, string newPassword)
        {
            throw new NotImplementedException();
        }

        public string FindUsersForFunction(string strFunctionID)
        {
            throw new NotImplementedException();
        }

        private bool ComparePassword(string password, string hash)
        {
            return EncryptPassword(password) == hash || password == hash;
        }

        private string EncryptPassword(string password)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = x.ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }

        private string GetUrlFromCity(Guid gCityID)
        {
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.City> queryCity = CityRepository.GetQueryable();
            var city = queryCity.Single(c => c.CityID == gCityID);
            return city.Servers.OrderBy(s => s.ServerID).First().Url;                
        }

        private string GetUrlFromServer(Guid gServerID)
        {
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.Server> query = ServerRepository.GetQueryable();
            var system = query.Single(s => s.ServerID == gServerID);
            return system.Url;
        }        

        public object GetUserRole(string userID)
        {
            Guid uid = new Guid(userID);
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.Role> queryRole = RoleRepository.GetQueryable();
            var user = queryUser.FirstOrDefault(u => u.UserID == uid);
            var roles = user.UserRoles.OrderBy(r=>r.Role.RoleID).Select(r => new { r.UserRoleID, r.User.UserID, r.User.UserName, r.Role.RoleID, r.Role.RoleName });
            return roles.ToArray();
        }

        public object GetRoleInfo(string userID)
        {
            Guid uid = new Guid(userID);
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.Role> queryRole = RoleRepository.GetQueryable();
            var user = queryUser.FirstOrDefault(u => u.UserID == uid);
            var roleIDs = user.UserRoles.Select(ur => ur.Role.RoleID);
            var role = queryRole.Where(r => !roleIDs.Any(rid => rid == r.RoleID))
                .Select(r => new { r.RoleID, r.RoleName, Description = r.Memo, Status = r.IsLock ? "启用" : "禁用" });
            return role.ToArray();
        }

        public bool DeleteUserRole(string userRoleIdStr)
        {
            string[] userRoleIdList = userRoleIdStr.Split(',');
            for (int i = 0; i < userRoleIdList.Length-1; i++)
            {
                Guid userRoleId = new Guid(userRoleIdList[i]);
                var UserRole = UserRoleRepository.GetQueryable().FirstOrDefault(ur => ur.UserRoleID == userRoleId);
                if (UserRole != null)
                {
                    UserRoleRepository.Delete(UserRole);
                    UserRoleRepository.SaveChanges();
                }
            }
            return true;
        }

        public bool AddUserRole(string userID, string roleIDStr)
        {
            try
            {
                var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserID == new Guid(userID));
                string[] roleIdList = roleIDStr.Split(',');
                for (int i = 0; i < roleIdList.Length - 1; i++)
                {
                    Guid rid = new Guid(roleIdList[i]);
                    var role = RoleRepository.GetQueryable().FirstOrDefault(r => r.RoleID == rid);
                    var userRole = new UserRole();
                    userRole.UserRoleID = Guid.NewGuid();
                    userRole.User = user;
                    userRole.Role = role;
                    UserRoleRepository.Add(userRole);
                    UserRoleRepository.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}

