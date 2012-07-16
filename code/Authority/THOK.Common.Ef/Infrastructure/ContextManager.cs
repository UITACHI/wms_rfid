using System;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Threading;
using System.Data.Entity;

namespace THOK.Common.Ef.Infrastructure
{
    public static class ContextManager
    {
        private static readonly Hashtable _threadDbContexts = new Hashtable();

        public static DbSet<T> GetObjectSet<T>(T entity, string contextKey) 
            where T : class
        {
            return GetDbContext(contextKey).Set<T>();
        }

        public static DbContext GetDbContext(string contextKey)
        {
            DbContext DbContext = GetCurrentDbContext(contextKey);
            if (DbContext == null) 
            {
                DbContext = CreateDbContext(contextKey);
                StoreCurrentDbContext(DbContext, contextKey);
            }
            return DbContext;
        }

        public static object GetRepositoryContext(string contextKey)
        {
            return GetDbContext(contextKey);
        }

        public static void SetRepositoryContext(object repositoryContext, string contextKey)
        {
            if (repositoryContext == null)
            {
                RemoveCurrentDbContext(contextKey);
            }
            else if (repositoryContext is DbContext)
            {
                StoreCurrentDbContext((DbContext)repositoryContext, contextKey);
            }
        }


        #region Object Context Lifecycle Management

        private static DbContext GetCurrentDbContext(string contextKey)
        {
            DbContext DbContext = null;
            if (HttpContext.Current == null)
                DbContext = GetCurrentThreadDbContext(contextKey);
            else
                DbContext = GetCurrentHttpContextDbContext(contextKey);
            return DbContext;
        }

        private static void StoreCurrentDbContext(DbContext DbContext, string contextKey)
        {
            if (HttpContext.Current == null)
                StoreCurrentThreadDbContext(DbContext, contextKey);
            else
                StoreCurrentHttpContextDbContext(DbContext, contextKey);
        }

        private static void RemoveCurrentDbContext(string contextKey)
        {
            if (HttpContext.Current == null)
                RemoveCurrentThreadDbContext(contextKey);
            else
                RemoveCurrentHttpContextDbContext(contextKey);
        }

        #region private methods - HttpContext related

        private static DbContext GetCurrentHttpContextDbContext(string contextKey)
        {
            DbContext DbContext = null;
            if (HttpContext.Current.Items.Contains(contextKey))
                DbContext = (DbContext)HttpContext.Current.Items[contextKey];
            return DbContext;
        }

        private static void StoreCurrentHttpContextDbContext(DbContext DbContext, string contextKey)
        {
            if (HttpContext.Current.Items.Contains(contextKey))
                HttpContext.Current.Items[contextKey] = DbContext;
            else
                HttpContext.Current.Items.Add(contextKey, DbContext);
        }

        private static void RemoveCurrentHttpContextDbContext(string contextKey)
        {
            DbContext DbContext = GetCurrentHttpContextDbContext(contextKey);
            if (DbContext != null)
            {
                HttpContext.Current.Items.Remove(contextKey);
                DbContext.Dispose();
            }
        }

        #endregion

        #region private methods - ThreadContext related

        private static DbContext GetCurrentThreadDbContext(string contextKey)
        {
            DbContext DbContext = null;
            Thread threadCurrent = Thread.CurrentThread;
            if (threadCurrent.Name == null)
                threadCurrent.Name = contextKey;
            else
            {
                object threadDbContext = null;
                lock (_threadDbContexts.SyncRoot)
                {
                    threadDbContext = _threadDbContexts[contextKey];
                }
                if (threadDbContext != null)
                    DbContext = (DbContext)threadDbContext;
            }
            return DbContext;
        }

        private static void StoreCurrentThreadDbContext(DbContext DbContext, string contextKey)
        {
            lock (_threadDbContexts.SyncRoot)
            {
                if (_threadDbContexts.Contains(contextKey))
                    _threadDbContexts[contextKey] = DbContext;
                else
                    _threadDbContexts.Add(contextKey, DbContext);
            }
        }

        private static void RemoveCurrentThreadDbContext(string contextKey)
        {
            lock (_threadDbContexts.SyncRoot)
            {
                if (_threadDbContexts.Contains(contextKey))
                {
                    DbContext DbContext = (DbContext)_threadDbContexts[contextKey];
                    if (DbContext != null)
                    {
                        DbContext.Dispose();
                    }
                    _threadDbContexts.Remove(contextKey);
                }
            }
        }

        #endregion

        #endregion

        private static Dictionary<string, System.Reflection.Assembly> assemblys = new Dictionary<string, System.Reflection.Assembly>();
        private static DbContext CreateDbContext(string typeName, params object[] args)
        {
            System.Reflection.Assembly assembly = null;
            if (assemblys.ContainsKey(typeName.Split(","[0])[1]))
            {
                assembly = assemblys[typeName.Split(","[0])[1]];
            }
            else
            {
                lock (assemblys)
                {
                    if (assemblys.ContainsKey(typeName.Split(","[0])[1]))
                    {
                        assembly = assemblys[typeName.Split(","[0])[1]];
                    }
                    else
                    {
                        assembly = System.Reflection.Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "bin\\" + typeName.Split(","[0])[1]);
                        assemblys.Add(typeName.Split(","[0])[1], assembly);
                    }
                }
            }
            return (DbContext)assembly.CreateInstance(typeName.Split(","[0])[0],false, System.Reflection.BindingFlags.Default,null,args,null,null);
        }
    }
}
