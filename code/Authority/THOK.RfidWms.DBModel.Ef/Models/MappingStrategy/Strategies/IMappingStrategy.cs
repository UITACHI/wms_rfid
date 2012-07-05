namespace THOK.RfidWms.DBModel.Ef.Models.MappingStrategy.Strategies
{
    public interface IMappingStrategy<T>
    {
        T To(T from);
    }
}