using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using Microsoft.Practices.ServiceLocation;
using MvcDemo.Dal.Interfaces;

namespace MvcDemo.Dal.Infrastructure
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        public IUnitOfWork Create()
        {
            return ServiceLocator.Current.GetInstance<IUnitOfWork>();
        }
    }
}
