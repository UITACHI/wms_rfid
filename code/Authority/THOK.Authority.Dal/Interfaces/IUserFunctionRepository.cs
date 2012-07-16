using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.DbModel;
using THOK.Common.Ef.Interfaces;

namespace THOK.Authority.Dal.Interfaces
{
    public interface IUserFunctionRepository : IRepository<UserFunction>
    {
    }
}
