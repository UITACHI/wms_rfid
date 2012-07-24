﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using THOK.Common.Ef.Interfaces;
using Microsoft.Practices.Unity;
using System.Data;
using System.Data.Entity.Infrastructure;

namespace THOK.Common.Ef.EntityRepository
{
    public abstract class RepositoryBase<T> : IRepository<T>
        where T: class
    {
        [Dependency]
        public IRepositoryContext RepositoryContext { get; set; }

        public DbSet<T> dbSet { get { return RepositoryContext.GetDbSet<T>(); } }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Update(T entity)
        {
            dbSet.Attach(entity);        }

        public void Attach(T entity)
        {
            var entry = RepositoryContext.DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                entry.State = EntityState.Modified;
            }
        }

        public void Detach(T entity)
        {
            ((IObjectContextAdapter)RepositoryContext.DbContext).ObjectContext.Detach(entity);
        }

        public IQueryable<T> GetQueryable()
        {
            return this.dbSet.AsQueryable<T>();
        }

        public IList<T> GetAll()
        {
            return this.dbSet.ToList<T>();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return this.dbSet.Where(whereCondition).ToList<T>();
        }

        public T GetSingle()
        {
            return this.dbSet.FirstOrDefault<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return this.dbSet.Where(whereCondition).FirstOrDefault<T>();
        }              

        public long Count()
        {
            return this.dbSet.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return this.dbSet.Where(whereCondition).LongCount<T>();
        }

        //todo
        public int SaveChanges()
        {
            return RepositoryContext.SaveChanges();
        }
        //todo
        public void Delete<TSub>(TSub[] tsubs)
        {
            foreach (var tsub in tsubs)
            {
                RepositoryContext.DbContext.Set(tsub.GetType()).Remove(tsub);
            }
            RepositoryContext.SaveChanges();
        }            
    }
}
