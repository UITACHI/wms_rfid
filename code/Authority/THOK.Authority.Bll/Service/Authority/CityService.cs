using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.Authority.Dal.EntityModels;
using THOK.Authority.Dal.EntityRepository.Authority;
using Microsoft.Practices.Unity;
using System.Linq;
using THOK.Common;

namespace THOK.Authority.Bll.Service.Authority
{
    public class CityService : ServiceBase<City>, ICityService
    {
        [Dependency]
        public ICityRepository CityRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string cityName, string description, string isActive)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.City> query = CityRepository.GetQueryable();
            var citys = query.Where(i => i.CityName.Contains(cityName)
                && i.Description.Contains(description))
                .OrderBy(i => i.CityID)
                .Select(i => new { i.CityID, i.CityName, i.Description, IsActive = i.IsActive ? "启用" : "禁用"});

            int total = citys.Count();
            citys = citys.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = citys.ToArray() };
        }

        public bool Add(string cityName, string description, bool isActive)
        {
            var city = new THOK.Authority.Dal.EntityModels.City()
            {
                CityID = Guid.NewGuid(),
                CityName = cityName,
                Description = description,
                IsActive = isActive
            };
            CityRepository.Add(city);
            CityRepository.SaveChanges();
            return true;
        }

        public bool Delete(string cityID)
        {
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable()
                .FirstOrDefault(i => i.CityID == gCityID);
            if (city != null)
            {
                CityRepository.Delete(city);                
                CityRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string cityID, string cityName, string description, bool isActive)
        {
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable()
                .FirstOrDefault(i => i.CityID == gCityID);
            city.CityName = cityName;
            city.Description = description;
            city.IsActive = isActive;
            CityRepository.SaveChanges();
            return true;
        }
    }
}
