using System;

namespace THOK.RfidWms.DBModel.Ef.Models.MappingStrategy.Strategies
{
    public class ToLowerMappingStrategy : IMappingStrategy<string>
    {
        #region Implementation of IMappingStrategy<string>

        public string To(string from)
        {
            return from.ToLowerInvariant();
        }

        #endregion
    }
}