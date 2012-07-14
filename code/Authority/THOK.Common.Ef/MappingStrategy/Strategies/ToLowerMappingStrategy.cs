using System;

namespace THOK.Ef.Common.MappingStrategy.Strategies
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