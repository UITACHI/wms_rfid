using THOK.Authority.Dal.EntityModels;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface ICityService : IService<City>
    {
        object GetDetails(int page, int rows, string cityName, string description, string isActive);

        bool Add(string cityName, string description, bool isActive);        

        bool Delete(string cityID);

        bool Save(string cityID, string cityName, string description, bool isActive);
    }
}
