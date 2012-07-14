using System.Data.Entity.ModelConfiguration.Configuration;

namespace THOK.Common.Ef.MappingStrategy
{
    public interface IMapping
    {
        void RegistTo(ConfigurationRegistrar configurationRegistrar);
    }
}