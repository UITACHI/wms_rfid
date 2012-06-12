using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Data;

namespace THOK.Authority.Authority
{
   public class UserService:IUserService
    {
        #region IUserService 成员

        public object GetDetails(int page, int rows)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var systems = from s in context.User
                                  select new { s.UserID, s.UserName, IsLock = s.IsLock ? "锁定" : "未锁", IsAdmin = s.IsAdmin ? "是" : "否",s.Memo };
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

        public bool AddUser(string userName, string pwd, string ChineseName, bool isLock, bool isAdmin, string loginPc, string memo)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var userAdd = new User();
                    userAdd.UserID = Guid.NewGuid();
                    userAdd.UserName = userName;
                    userAdd.Pwd = pwd;
                    userAdd.ChineseName = ChineseName;
                    userAdd.IsLock = isLock;
                    userAdd.IsAdmin = isAdmin;
                    userAdd.LoginPC = loginPc;
                    userAdd.Memo = memo;
                    context.AddToUser(userAdd);
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
