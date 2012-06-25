using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.Authority.Dal.EntityModels;
using THOK.Authority.Dal.EntityRepository.Authority;
using System.Linq;
using Microsoft.Practices.Unity;
using THOK.Authority.Dal.Interfaces;
using System.Data.Objects.DataClasses;

namespace THOK.Authority.Bll.Service.Authority
{
    public class SystemService : ServiceBase<THOK.Authority.Dal.EntityModels.System>, ISystemService
    {
        [Dependency]
        public ISystemRepository SystemRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }
        [Dependency]
        public IRoleSystemRepository RoleSystemRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public ILoginLogRepository LoginLogRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string systemName, string description, string status)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.System> query = SystemRepository.GetQueryable();
            var systems = query.Where(i => i.SystemName.Contains(systemName) && i.Description.Contains(description))
                .OrderBy(i => i.SystemID)
                .Select(i => new { i.SystemID, i.SystemName, i.Description, Status = i.Status ? "启用" : "禁用" });
            if (status != "")
            {
                bool bStatus = Convert.ToBoolean(status);
                systems = query.Where(i => i.SystemName.Contains(systemName) && i.Description.Contains(description) && i.Status == bStatus)
                    .OrderBy(i => i.SystemID)
                    .Select(i => new { i.SystemID, i.SystemName, i.Description, Status = i.Status ? "启用" : "禁用" });
            }
            int total = systems.Count();
            systems = systems.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = systems.ToArray() };
        }

        public bool Add(string systemName, string description, bool status)
        {
            var system = new THOK.Authority.Dal.EntityModels.System()
            {
                SystemID = Guid.NewGuid(),
                SystemName = systemName,
                Description = description,
                Status = status
            };
            SystemRepository.Add(system);
            SystemRepository.SaveChanges();
            return true;
        }

        public bool Delete(string systemId)
        {
            Guid gsystemId = new Guid(systemId);
            var system = SystemRepository.GetQueryable()
                .FirstOrDefault(i => i.SystemID == gsystemId);
            if (system != null)
            {
                Del(ModuleRepository, system.Modules);
                Del(RoleSystemRepository, system.RoleSystems);
                Del(UserSystemRepository, system.UserSystems);
                Del(LoginLogRepository, system.LoginLogs);
                SystemRepository.Delete(system);
                SystemRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string systemId, string systemName, string description, bool status)
        {
            Guid sid = new Guid(systemId);
            var system = SystemRepository.GetQueryable()
                .FirstOrDefault(i => i.SystemID == sid);
            system.SystemName = systemName;
            system.Description = description;
            system.Status = status;
            SystemRepository.SaveChanges();
            return true;
        }
    }
}
