using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using THOK.Authority.Dal.Interfaces;

namespace THOK.Authority.Dal.Infrastructure
{
    public class HttpRequestModule : IHttpModule
    {
        public string ModuleName
        {
            get { return "THOK.Authority.Dal.EF.HttpRequestModule"; }
        }

        public void Init(HttpApplication application)
        {
            application.EndRequest += new EventHandler(this.Application_EndRequest);
        }

        private void Application_EndRequest(object source, EventArgs e)
        {
            var repositoryContexts = ServiceLocator.Current.GetAllInstances(typeof(IRepositoryContext));

            foreach (var item in repositoryContexts)
            {
                if (item is IRepositoryContext)
                {
                    ((IRepositoryContext)item).Terminate();
                }
            }
        }

        public void Dispose() { }

    }
}
