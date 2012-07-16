using System;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.DbModel;

namespace THOK.Authority.Bll.Service
{
    public class RoleService : ServiceBase<Role>, IRoleService
    {
        [Dependency]
        public IRoleRepository RoleRepository { get; set; }
        [Dependency]
        public IRoleSystemRepository RoleSystemRepository { get; set; }
        [Dependency]
        public IUserRoleRepository UserRoleRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string roleName, string description,string status)
        {
            IQueryable<THOK.Authority.DbModel.Role> queryRole = RoleRepository.GetQueryable();
            var roles = queryRole.Where(r => r.RoleName.Contains(roleName) && r.Memo.Contains(description))
                    .OrderBy(r => r.RoleName)
                    .Select(r => new { r.RoleID, r.RoleName, Description = r.Memo, Status = r.IsLock ? "启用" : "禁用" });
            if (!String.IsNullOrEmpty(status))
            {
                bool bStatus = Convert.ToBoolean(status);
                roles = queryRole.Where(r => r.RoleName.Contains(roleName) 
                    && r.Memo.Contains(description)
                    && r.IsLock == bStatus)
                    .OrderBy(r => r.RoleName)
                    .Select(r => new { r.RoleID, r.RoleName, Description = r.Memo, Status = r.IsLock ? "启用" : "禁用" });
            }              
            int total = roles.Count();
            roles = roles.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = roles.ToArray() };
        }

        public bool Add(string roleName, string description, bool status)
        {
            var role = new Role()
            {
                RoleID = Guid.NewGuid(),
                RoleName = roleName,
                Memo = description,
                IsLock = status
            };
            RoleRepository.Add(role);
            RoleRepository.SaveChanges();
            return true;
        }
       
        public bool Delete(string roleID)
        {
            Guid gRoleID = new Guid(roleID);
            var role = RoleRepository.GetQueryable()
                .FirstOrDefault(i => i.RoleID == gRoleID);
            if (role != null)
            {
                Del(RoleSystemRepository, role.RoleSystems);
                Del(UserRoleRepository, role.UserRoles);
                RoleRepository.Delete(role);
                RoleRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string roleID, string roleName, string description, bool status)
        {
            Guid gRoleID = new Guid(roleID);
            var role = RoleRepository.GetQueryable()
                .FirstOrDefault(i => i.RoleID == gRoleID);
            role.RoleName = roleName;
            role.Memo = description;
            role.IsLock = status;
            RoleRepository.SaveChanges();
            return true;
        }

        public string FindRolesForFunction(string strFunctionID)
        {
            throw new NotImplementedException();
        }



        #region IRoleService 成员


        public object GetRoleUser(string roleID)
        {
            Guid rid = new Guid(roleID);
            IQueryable<THOK.Authority.DbModel.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Role> queryRole = RoleRepository.GetQueryable();
            var role = queryRole.FirstOrDefault(r => r.RoleID== rid);
            var users = role.UserRoles.OrderBy(u => u.User.UserID).Select(u => new { u.UserRoleID, u.Role.RoleID, u.Role.RoleName,u.User.UserID, u.User.UserName });
            return users.ToArray();
        }

        public object GetUserInfo(string roleID)
        {
            Guid rid = new Guid(roleID);
            IQueryable<THOK.Authority.DbModel.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.DbModel.Role> queryRole = RoleRepository.GetQueryable();
            var role = queryRole.FirstOrDefault(r => r.RoleID == rid);
            var userIDs =role.UserRoles.Select(ru => ru.User.UserID);
            var user = queryUser.Where(u => !userIDs.Any(uid => uid == u.UserID))
                .Select(u => new { u.UserID, u.UserName, Description = u.Memo, Status = u.IsLock ? "启用" : "禁用" });
            return user.ToArray();
        }

        public bool DeleteRoleUser(string roleUserIdStr)
        {
            string[] roleUserIdList = roleUserIdStr.Split(',');
            for (int i = 0; i < roleUserIdList.Length - 1; i++)
            {
                Guid roleUserId = new Guid(roleUserIdList[i]);
                var RoleUser = UserRoleRepository.GetQueryable().FirstOrDefault(ur => ur.UserRoleID == roleUserId);
                if (RoleUser != null)
                {
                    UserRoleRepository.Delete(RoleUser);
                    UserRoleRepository.SaveChanges();
                }
            }
            return true;
        }

        public bool AddRoleUser(string roleID, string userIDStr)
        {
            try
            {
                var role = RoleRepository.GetQueryable().FirstOrDefault(r => r.RoleID== new Guid(roleID));
                string[] userIdList = userIDStr.Split(',');
                for (int i = 0; i < userIdList.Length - 1; i++)
                {
                    Guid uid = new Guid(userIdList[i]);
                    var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserID == uid);
                    var roleUser = new UserRole();
                    roleUser.UserRoleID = Guid.NewGuid();
                    roleUser.User = user;
                    roleUser.Role = role;
                    UserRoleRepository.Add(roleUser);
                    UserRoleRepository.SaveChanges();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        #endregion
    }
}
