using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Wms.DbModel;
using THOK.Common.Ef.Interfaces;

namespace THOK.Wms.Dal.Interfaces
{
    public interface ICellRepository : IRepository<Cell>
    {
        IQueryable<Cell> GetQueryableIncludeStorages();
    }
}
