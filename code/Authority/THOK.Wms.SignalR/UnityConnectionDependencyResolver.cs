using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System.Configuration;
using Microsoft.Practices.ServiceLocation;
using SignalR;

namespace THOK.Wms.SignalR
{
    public class UnityConnectionDependencyResolver : DefaultDependencyResolver 
    {
        private readonly IUnityContainer _container;

        public UnityConnectionDependencyResolver()
        {            
            _container = new UnityContainer();
            UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            section.Configure(_container, "defaultContainer");
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }

        public UnityConnectionDependencyResolver(IUnityContainer container)
        {
            _container = container;
            ServiceLocatorProvider sp = new ServiceLocatorProvider(GetServiceLocator);
            ServiceLocator.SetLocatorProvider(sp);
        }

        public override object GetService(Type serviceType)
        {
            if (serviceType != null && _container.IsRegistered(serviceType))
            {
                return _container.Resolve(serviceType);
            }
            return base.GetService(serviceType);
        }

        public IServiceLocator GetServiceLocator()
        {
            return new UnityServiceLocator(_container);
        }
    }
}
