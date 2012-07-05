using System.Collections.Generic;

namespace THOK.RfidWms.DBModel.Ef.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        IEnumerable<TEntity> FindAll();
        void Add(TEntity entity);
        void Delete(TEntity entity);
        void Update(TEntity entity);
    }
}