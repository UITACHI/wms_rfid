namespace THOK.Ef.Common.MappingStrategy.Strategies
{
    public interface IMappingStrategy<T>
    {
        T To(T from);
    }
}