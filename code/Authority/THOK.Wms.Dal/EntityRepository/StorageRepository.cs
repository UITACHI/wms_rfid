using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.Ef.EntityRepository;
using System.Linq;

namespace THOK.Wms.Dal.EntityRepository
{
    public class StorageRepository : RepositoryBase<Storage>, IStorageRepository
    {
        public new IQueryable<Storage> GetQueryable()
        {
            return this.dbSet.Include("Product")
                             .Include("Product.Unit")
                             .AsQueryable<Storage>();
        }
    }
}
