using System;
using System.Web;
using Microsoft.Practices.ServiceLocation;
using THOK.Common.Ef.Interfaces;

namespace THOK.Common.Ef.Infrastructure
{
    public class HttpRequestModule : IHttpModule
    {
        public string ModuleName
        {
            get { return "THOK.Common.Ef.HttpRequestModule"; }
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
