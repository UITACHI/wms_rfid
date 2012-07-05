using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using System.Web.Routing;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;

namespace THOK.Common
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private readonly IUnityContainer _container;

        public UnityControllerFactory()
        {            
            _container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container, "defaultContainer");
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }

        public UnityControllerFactory(IUnityContainer container)
        {
            _container = container;
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType != null)
            {
                return _container.Resolve(controllerType) as IController;
            }
            return base.GetControllerInstance(requestContext, controllerType);
        }

        public IServiceLocator GetServiceLocator()
        {
            return new UnityServiceLocator(_container);
        }
    }
}
