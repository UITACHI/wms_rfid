using System;
using System.Linq;
using THOK.Authority.Data;
using System.Data.Objects.DataClasses;

namespace THOK.Authority.Authority
{
    public class SystemService:ISystemService
    {
        public object GetDetails(int page, int rows, string systemName, string description, string status)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                var systems = from s in context.System
                              where s.SystemName.Contains(systemName) && s.Description.Contains(description) 
                              select new { s.SystemID, s.SystemName, s.Description, Status = s.Status ? "启用" : "禁用" };
                if (status != "")
                {
                    bool bStatus = Convert.ToBoolean(status);
                    systems = from s in context.System
                              where s.SystemName.Contains(systemName) && s.Description.Contains(description) && s.Status == bStatus
                              select new { s.SystemID, s.SystemName, s.Description, Status = s.Status ? "启用" : "禁用" };
                }
                return new { total = 1000, rows = systems.ToArray() };
            }
        }        

        public bool AddSystem(string systemName, string description, bool status)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var systemAdd = new THOK.Authority.Data.System();
                    systemAdd.SystemID = Guid.NewGuid();
                    systemAdd.SystemName = systemName;
                    systemAdd.Description = description;
                    systemAdd.Status = status;

                    context.AddToSystem(systemAdd);
                    context.SaveChanges();
                    
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        public bool SaveSystemInfo(string systemId, string systemName, string description, bool status)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    Guid sid =new Guid(systemId);
                    var system = context.System.FirstOrDefault(i => i.SystemID== sid);
                        system.SystemName = systemName;
                        system.Description=description;
                        system.Status = status;
                        context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return true;
        }

        public bool Delete(string systemId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                Guid gsystemId = new Guid(systemId);
                var system = context.System.FirstOrDefault(i => i.SystemID == gsystemId);
                if (system != null)
                {
                    Del(context, system.Modules);
                    Del(context, system.RoleSystems);
                    Del(context, system.UserSystems);
                    Del(context, system.LoginLogs);
                    context.DeleteObject(system);
                    context.SaveChanges();
                }
                else
                    return false;
                return true;
            }
        }

        public void Del<TEntity>(AuthorizeEntities context, EntityCollection<TEntity> entities) where TEntity : class
        {
            var arrEntities = entities.ToArray();
            foreach (var item in arrEntities)
            {
                context.DeleteObject(item);
            }
        }
    }
}
