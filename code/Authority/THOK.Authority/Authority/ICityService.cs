using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Authority
{
    public interface ICityService
    {
        object GetDetails(int page, int rows);
        bool Add(string cityname, bool isactive);
    }
}
