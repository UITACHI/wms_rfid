using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THOK.Authority.Authority
{
    public interface IModuleService
    {
        object GetDetails(string systemId);
        bool AddModule(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemId, string moduleId);
        bool SaveModuleInfo(string moduleID,string moduleName,int showOrder,string moduleUrl,string indicateImage,string deskTopImage);
        bool Delete(string moduleId);
    }
}
