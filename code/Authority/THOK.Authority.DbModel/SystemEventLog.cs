using System;
using System.Collections.Generic;

namespace THOK.Authority.DbModel
{
    public class SystemEventLog
    {
        public Guid EventLogID { get; set; }
        public string EventLogTime { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string FromPC { get; set; }
        public string OperateUser { get; set; }
        public string TargetSystem { get; set; }
    }
}
