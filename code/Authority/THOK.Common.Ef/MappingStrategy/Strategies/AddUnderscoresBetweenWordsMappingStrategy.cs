using System;
using System.Text;

namespace THOK.Ef.Common.MappingStrategy.Strategies
{
    public class AddUnderscoresBetweenWordsMappingStrategy : IMappingStrategy<string >
    {
        #region Implementation of IMappingStrategy<string>

        public string To(string from)
        {
            var chars = from.ToCharArray();
            var sb = new StringBuilder(chars.Length);

            var prev = 'A';
            foreach (var c in chars)
            {
                if (c != '_' && char.IsUpper(c) && prev != '_' && !char.IsUpper(prev))
                {
                    sb.Append('_');
                }
                sb.Append(c);
                prev = c;
            }

            return sb.ToString();
        }

        #endregion
    }
}