using THOK.Wms.DbModel;
using THOK.Wms.Dal.Interfaces;
using THOK.Common.Ef.EntityRepository;
using System.Linq;

namespace THOK.Wms.Dal.EntityRepository
{
    public class CellRepository : RepositoryBase<Cell>, ICellRepository
    {
        public new IQueryable<Cell> GetQueryable()
        {
            return this.dbSet.AsQueryable<Cell>();
        }

        public IQueryable<Cell> GetQueryableIncludeStorages()
        {
            return this.dbSet.Include("Storages")
                             .AsQueryable<Cell>();
        }
    }
}
