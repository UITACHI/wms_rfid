using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using THOK.Common.Ef.Interfaces;
using Microsoft.Practices.Unity;

namespace THOK.Common.Ef.EntityRepository
{
    public abstract class RepositoryBase<T> : IRepository<T>
        where T: class
    {
        [Dependency]
        public IRepositoryContext RepositoryContext { get; set; }

        public DbSet<T> ObjectSet { get { return RepositoryContext.GetObjectSet<T>(); } }
       
        #region IRepository Members

        public void Add(T entity)
        {
            ObjectSet.Add(entity);
            RepositoryContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            ObjectSet.Remove(entity);
            RepositoryContext.SaveChanges();
        }

        public IList<T> GetAll()
        {
            return this.ObjectSet.ToList<T>();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).ToList<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).FirstOrDefault<T>();
        }

        public void Attach(T entity)
        {
            this.ObjectSet.Attach(entity);
        }

        public IQueryable<T> GetQueryable()
        {
            return this.ObjectSet.AsQueryable<T>();
        }

        public long Count()
        {
            return this.ObjectSet.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).LongCount<T>();
        }

        public int SaveChanges()
        {
            return RepositoryContext.SaveChanges();
        }

        public void Delete<TSub>(TSub[] tsubs)
        {
            foreach (var tsub in tsubs)
            {
                RepositoryContext.DbContext.Set(tsub.GetType()).Remove(tsub);
            }           
        }
        #endregion
            
    }
}
