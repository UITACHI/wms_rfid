using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Linq.Expressions;
using THOK.Authority.Dal.Interfaces;
using THOK.Authority.Dal.Infrastructure;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using THOK.Authority.Dal.Infrastructure.RepositoryContext;
using System.Data.Entity;

namespace THOK.Authority.Dal.EntityRepository
{
    public abstract class RepositoryBase<T> : IRepository<T>
        where T: class
    {
        public DbSet<T> ObjectSet { get; set; }
        public IRepositoryContext RepositoryContext { get; set; }

        public RepositoryBase()
            : this(new AuthorityRepositoryContext())
        {
        }

        public RepositoryBase(IRepositoryContext repositoryContext)
        {
            RepositoryContext = repositoryContext ?? new AuthorityRepositoryContext();
            ObjectSet = RepositoryContext.GetObjectSet<T>();
        }
        
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
