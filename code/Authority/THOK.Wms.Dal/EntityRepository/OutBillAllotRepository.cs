using THOK.Wms.Dal.Interfaces;
using THOK.Wms.DbModel;
using THOK.Common.Ef.EntityRepository;
using System.Linq;

namespace THOK.Wms.Dal.EntityRepository
{
    public class OutBillAllotRepository:RepositoryBase<OutBillAllot>,IOutBillAllotRepository
    {
        public new IQueryable<OutBillAllot> GetQueryable()
        {
            return this.dbSet.Include("Cell")
                             .Include("Storage")
                             .Include("Storage.Cell")
                             .Include("Product")
                             .Include("Unit")
                             .Include("OutBillDetail")
                             .AsQueryable<OutBillAllot>();
        }

        public new ParallelQuery<OutBillAllot> GetParallelQuery()
        {
            return this.dbSet.Include("Cell")
                             .Include("Storage")
                             .Include("Storage.Cell")
                             .Include("Product")
                             .Include("Unit")
                             .Include("OutBillDetail")
                             .AsParallel<OutBillAllot>();
        }
    }
}
