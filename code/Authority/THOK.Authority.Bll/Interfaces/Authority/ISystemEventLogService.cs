using THOK.RfidWms.DBModel.Ef.Models.Authority;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface ISystemEventLogService : IService<SystemEventLog>
    {
        object GetDetails(int page, int rows, string eventlogtime, string eventtype, string eventname, string frompc, string operateuser, string targetsystem);

    }
}
