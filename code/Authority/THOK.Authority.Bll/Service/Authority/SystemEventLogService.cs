using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using THOK.Authority.Dal.EntityRepository.Authority;
using Microsoft.Practices.Unity;
using System.Linq;
using THOK.Common;


namespace THOK.Authority.Bll.Service.Authority
{
    public class SystemEventLogService : ServiceBase<SystemEventLog>, ISystemEventLogService
    {
        [Dependency]
        public ISystemEventLogRepository SystemEventLogRepository { get; set; }
        public object GetDetails(int page, int rows, string eventlogtime, string eventtype, string eventname, string frompc, string operateuser, string targetsystem)
        {
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.SystemEventLog> query = SystemEventLogRepository.GetQueryable();
            var systemeventlogs = query.Where(i => i.EventLogTime.Contains(eventlogtime)
                    && i.EventType.Contains(eventtype) && i.EventName.Contains(eventname) && i.FromPC.Contains(frompc) && i.OperateUser.Contains(operateuser) && i.TargetSystem.Contains(targetsystem))
                    .OrderBy(i => i.EventLogID)
                    .Select(i => new { i.EventLogID, i.EventName, i.EventType, i.FromPC, i.EventLogTime, i.OperateUser, i.EventDescription, i.TargetSystem });


            int total = systemeventlogs.Count();
            systemeventlogs = systemeventlogs.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = systemeventlogs.ToArray() };
        }


        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }
    }
}
