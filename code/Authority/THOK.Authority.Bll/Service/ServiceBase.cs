using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Common;
using THOK.Authority.Bll.Interfaces;
using THOK.Authority.Dal.Interfaces;
using System.Data.Objects.DataClasses;

namespace THOK.Authority.Bll.Service
{
    public abstract class ServiceBase<T> : LoggerBase, IService<T> 
    {
        public T GetSingle(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public void Add(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public IList<T> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public long Count(System.Linq.Expressions.Expression<Func<T, bool>> whereCondition)
        {
            throw new NotImplementedException();
        }

        public long Count()
        {
            throw new NotImplementedException();
        }

        public void Del<TEntity>(IRepository<TEntity> context, EntityCollection<TEntity> entities) where TEntity : class
        {
            var arrEntities = entities.ToArray();
            foreach (var item in arrEntities)
            {
                context.Delete(item);
            }
        }
    }
}
