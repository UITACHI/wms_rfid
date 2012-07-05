using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using THOK.Authority.Dal.EntityRepository.Authority;
using Microsoft.Practices.Unity;
using System.Linq;

namespace THOK.Authority.Bll.Service.Authority
{
    public class ServerService : ServiceBase<Server>, IServerService
    {
        [Dependency]
        public IServerRepository ServerRepository { get; set; }
        [Dependency]
        public ICityRepository CityRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        public object GetDetails(int page, int rows, string serverName, string description, string url, string isActive)
        {
            IQueryable<THOK.RfidWms.DBModel.Ef.Models.Authority.Server> query = ServerRepository.GetQueryable();
            var servers = query.Where(i => i.ServerName.Contains(serverName)
                && i.Description.Contains(description)
                && i.Url.Contains(url))
                .OrderBy(i => i.ServerID)
                .Select(i => new { i.ServerID, i.ServerName,i.City.CityID,i.City.CityName,i.Description,i.Url,IsActive = i.IsActive ? "启用" : "禁用" });

            int total = servers.Count();
            servers = servers.Skip((page - 1) * rows).Take(rows);
            return new { total, rows = servers.ToArray() };
        }

        public bool Add(string serverName, string description, string url, bool isActive,string cityID)
        {
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable().Single(c => c.CityID == gCityID);
            var server = new THOK.RfidWms.DBModel.Ef.Models.Authority.Server()
            {
                ServerID = Guid.NewGuid(),
                ServerName = serverName,
                Description = description,
                Url = url,
                IsActive = isActive,
                City = city
            };
            ServerRepository.Add(server);
            ServerRepository.SaveChanges();
            return true;
        }

        public bool Delete(string serverID)
        {
            Guid gServerID = new Guid(serverID);
            var server = ServerRepository.GetQueryable()
                .FirstOrDefault(i => i.ServerID == gServerID);
            if (server != null)
            {
                ServerRepository.Delete(server);
                ServerRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string serverID, string serverName, string description, string url, bool isActive,string cityID)
        {
            Guid gServerID = new Guid(serverID);
            Guid gCityID = new Guid(cityID);
            var city = CityRepository.GetQueryable().Single(c => c.CityID == gCityID);
            var server = ServerRepository.GetQueryable()
                .FirstOrDefault(i => i.ServerID == gServerID);
            server.ServerName = serverName;
            server.Description = description;
            server.Url = url;
            server.IsActive = isActive;
            server.City = city;
            ServerRepository.SaveChanges();
            return true;
        }
    }
}
