namespace THOK.Ef.Common.MappingStrategy.Strategies
{
    public class AddPrefixMappingStrategy : IMappingStrategy<string>
    {
        private readonly string m_prefix;
        public AddPrefixMappingStrategy(string prefix)
        {
            m_prefix = prefix;
        }

        public string To(string to)
        {
            return m_prefix + to;
        }
    }
}