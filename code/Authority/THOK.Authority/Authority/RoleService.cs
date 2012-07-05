using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Data;

namespace THOK.Authority.Authority
{
   public class RoleService:IRoleService
    {
        #region IRoleService 成员

        public object GetDetails(int page, int rows)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var systems = from s in context.Role
                                  select new { s.RoleID, s.RoleName, s.Memo, IsLock = s.IsLock ? "启用" : "禁用" };
                    return systems.ToArray();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public object GetDetails(string id)
        {
            throw new NotImplementedException();
        }

        public bool AddRole(string roleName, string memo, bool islock)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var roleAdd = new Role();
                    roleAdd.RoleID = Guid.NewGuid();
                    roleAdd.RoleName = roleName;
                    roleAdd.IsLock = islock;
                    roleAdd.Memo = memo;
                    context.AddToRole(roleAdd);
                    context.SaveChanges();

                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion
    }
}
