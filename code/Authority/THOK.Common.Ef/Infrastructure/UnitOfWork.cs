using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using MvcDemo.Dal.Interfaces;

namespace MvcDemo.Dal.Infrastructure
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private IRepositoryContext _repositoryContext;

        public UnitOfWork(IRepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public void Commit()
        {
            _repositoryContext.SaveChanges();
        }

        public void Dispose()
        {
            if (_repositoryContext != null)
            {
                _repositoryContext.Terminate();
            }
            GC.SuppressFinalize(this);
        }

    }
}
