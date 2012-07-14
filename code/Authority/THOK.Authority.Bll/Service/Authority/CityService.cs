using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
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
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string cityName, string description, string isActive)
        {
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.City> query = CityRepository.GetQueryable();
            bool isactive;
            var citys = query.OrderBy(i => i.CityID).Select(i => new { i.CityID, i.CityName, i.Description, IsActive = i.IsActive ? "启用" : "禁用" });
            if (cityName != "" || description != "" || isActive != "")
            {
                if (isActive == "true") isactive = true;
                else isactive = false;
                citys = query.Where(i => i.CityName.Contains(cityName)
                    && i.Description.Contains(description) && i.IsActive == isactive)
                    .OrderBy(i => i.CityID)
                    .Select(i => new { i.CityID, i.CityName, i.Description, IsActive = i.IsActive ? "启用" : "禁用" });
            }

            int total = citys.Count();
            citys = citys.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = citys.ToArray() };
        }

        public bool Add(string cityName, string description, bool isActive)
        {
            var city = new THOK.RfidWms.DBModel.Ef.Models.Authority.City()
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
        
        public object GetCityByCityID(string cityID)
        {
            Guid cid=new Guid(cityID);
            var city = CityRepository.GetQueryable().FirstOrDefault(c => c.CityID == cid);
            return city.CityName;
        }

        public object GetDetails(string userName, string cityID, string systemID)
        {
            Guid cityid = new Guid(cityID);
            Guid systemid = new Guid(systemID);
            var user = UserRepository.GetQueryable().FirstOrDefault(u => u.UserName == userName);            
            var userSystem = UserSystemRepository.GetQueryable().Where(us => us.User_UserID == user.UserID && us.System_SystemID == systemid && us.City_CityID == cityid).Select(us => us.UserSystemID);
            var usersystems = UserSystemRepository.GetQueryable().Where(us => !userSystem.Any(uid => uid == us.UserSystemID) && us.User_UserID == user.UserID && us.System_SystemID == systemid).Select(us => new { us.City.CityID, us.City.CityName, us.City.Description, Status = us.City.IsActive ? "启用" : "禁用" });
            return usersystems.ToArray();
        }
    }
}
