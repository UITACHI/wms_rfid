using System;
namespace THOK.Common.Ef.Interfaces
{
    public interface IRepository<T>
     where T : class
    {
        void Add(T entity);
        void Attach(T entity);
        long Count();
        long Count(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
        System.Data.Entity.DbSet<T> dbSet { get; }
        void Delete(T entity);
        void Delete<TSub>(TSub[] tsubs);
        void Detach(T entity);
        System.Collections.Generic.IList<T> GetAll();
        System.Collections.Generic.IList<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
        System.Linq.IQueryable<T> GetQueryable();
        System.Linq.ParallelQuery<T> GetParallelQuery();
        System.Data.Objects.ObjectSet<T> GetObjectSet();
        System.Data.Objects.ObjectQuery<T> GetObjectQuery();
        System.Data.Objects.ObjectContext GetObjectContext();
        T GetSingle();
        T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition);
        THOK.Common.Ef.Interfaces.IRepositoryContext RepositoryContext { get; set; }
        int SaveChanges();
        void Update(T entity);
    }
}
