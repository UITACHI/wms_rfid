using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Dal.Infrastructure;
using THOK.RfidWms.DBModel.Ef.Models.Authority;

namespace THOK.Authority.Dal.Interfaces.Authority
{
    public interface ISystemRepository : IRepository<THOK.RfidWms.DBModel.Ef.Models.Authority.System>
    {
    }
}
