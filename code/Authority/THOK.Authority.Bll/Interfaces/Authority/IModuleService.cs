using THOK.Authority.Dal.EntityModels;

namespace THOK.Authority.Bll.Interfaces.Authority
{
    public interface IModuleService : IService<Module>
    {
        object GetDetails(string p);

        bool Add(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemId, string p);

        bool Delete(string moduleId);

        bool Save(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage);

        object GetUserMenus(string userName,string cityID,string systemID);

        object GetModuleFuns(string userName, string cityID, string moduleID);

        bool InitUserSystemInfo(string userID, string cityID, string systemID);

        void InitRoleSys(string roleID,string cityID,string systemID);

        object GetRoleSystemDetails(string systemID);

        bool ProcessRolePermissionStr(string rolePermissionStr);


    }
}
