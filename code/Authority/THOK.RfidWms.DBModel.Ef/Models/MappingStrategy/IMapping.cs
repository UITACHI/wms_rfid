using System.Data.Entity.ModelConfiguration.Configuration;

namespace THOK.RfidWms.DBModel.Ef.Models.MappingStrategy
{
    public interface IMapping
    {
        void RegistTo(ConfigurationRegistrar configurationRegistrar);
    }
}