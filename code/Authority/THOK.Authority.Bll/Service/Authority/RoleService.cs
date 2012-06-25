using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.Authority.Dal.EntityModels;
using THOK.Authority.Dal.EntityRepository.Authority;
using Microsoft.Practices.Unity;
using System.Linq;
using THOK.Authority.Dal.Interfaces;
using System.Data.Objects.DataClasses;

namespace THOK.Authority.Bll.Service.Authority
{
    public class RoleService : ServiceBase<Role>, IRoleService
    {
        [Dependency]
        public IRoleRepository RoleRepository { get; set; }
        [Dependency]
        public IRoleSystemRepository RoleSystemRepository { get; set; }
        [Dependency]
        public IUserRoleRepository UserRoleRepository { get; set; }
        
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string roleName, string description,string status)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.Role> queryRole = RoleRepository.GetQueryable();
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
    }
}
