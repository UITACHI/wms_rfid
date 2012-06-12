using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Data;
namespace THOK.Authority.Authority
{
    public class CityService : ICityService
    {
        #region ICityService 成员

        public object GetDetails(int page, int rows)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var citys = from s in context.City
                                  select new { s.CityID, s.CityName, s.IsActive };
                    return citys.ToArray();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        public bool Add(string cityname, bool isactive)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    var citys =new City();
                    citys.CityID=Guid.NewGuid();
                    citys.CityName = cityname;
                    citys.IsActive = isactive;
                    
                    context.AddToCity(citys);
                    context.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }

        #endregion
    }
}
