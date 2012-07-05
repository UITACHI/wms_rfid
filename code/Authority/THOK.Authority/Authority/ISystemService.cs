using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Authority
{
    public interface ISystemService
    {
        object GetDetails(int page, int rows,string systemName,string description,string status);
        bool AddSystem(string systemName, string description, bool status);		
        bool SaveSystemInfo(string systemId, string systemName, string description, bool status);
        bool Delete(string systemId);
    }
}
