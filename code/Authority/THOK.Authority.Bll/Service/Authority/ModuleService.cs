using System;
using THOK.Authority.Bll.Interfaces.Authority;
using THOK.Authority.Dal.Interfaces.Authority;
using THOK.Authority.Dal.EntityModels;
using THOK.Authority.Dal.EntityRepository.Authority;
using THOK.Authority.Bll.Models.Authority;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using System.Data.Objects.DataClasses;
using THOK.Authority.Dal.Interfaces;
using Authority.Models;

namespace THOK.Authority.Bll.Service.Authority
{
    public class ModuleService : ServiceBase<Module>, IModuleService
    {
        [Dependency]
        public IUserRepository UserRepository { get; set; }
        [Dependency]
        public ICityRepository CityRepository { get; set; }
        [Dependency]
        public ISystemRepository SystemRepository { get; set; }
        [Dependency]
        public IModuleRepository ModuleRepository { get; set; }
        [Dependency]
        public IUserSystemRepository UserSystemRepository { get; set; }
        [Dependency]
        public IUserModuleRepository UserModuleRepository { get; set; }
        [Dependency]
        public IUserFunctionRepository UserFunctionRepository { get; set; }
        [Dependency]
        public IRoleSystemRepository RoleSystemRepository { get; set; }
        [Dependency]
        public IRoleModuleRepository RoleModuleRepository { get; set; }
        [Dependency]
        public IRoleFunctionRepository RoleFunctionRepository { get; set; }
        [Dependency]
        public IFunctionRepository FunctionRepository { get; set; }
        [Dependency]
        public IRoleRepository RoleRepository { get; set; }

        protected override Type LogPrefix
        {
            get { return this.GetType(); }
        }

        #region 模块信息维护        
        
        public object GetDetails(string systemID)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.Module> queryModule = ModuleRepository.GetQueryable();
            var systems = querySystem.AsEnumerable();
            if (systemID != null && systemID != string.Empty)
            {
                Guid gsystemid = new Guid(systemID);
                systems = querySystem.Where(i => i.SystemID == gsystemid)
                                     .Select(i => i);
            }

            HashSet<Menu> systemMenuSet = new HashSet<Menu>();
            foreach (var system in systems)
            {
                Menu systemMenu = new Menu();
                systemMenu.ModuleID = system.SystemID.ToString();
                systemMenu.ModuleName = system.SystemName;
                systemMenu.SystemID = system.SystemID.ToString();
                systemMenu.SystemName = system.SystemName;

                var modules = queryModule.Where(m => m.System.SystemID == system.SystemID && m.ModuleID == m.ParentModule.ModuleID)
                                         .OrderBy(m => m.ShowOrder)
                                         .Select(m => m);
                HashSet<Menu> moduleMenuSet = new HashSet<Menu>();
                foreach (var item in modules)
                {
                    Menu moduleMenu = new Menu();
                    moduleMenu.ModuleID = item.ModuleID.ToString();
                    moduleMenu.ModuleName = item.ModuleName;
                    moduleMenu.SystemID = item.System.SystemID.ToString();
                    moduleMenu.SystemName = item.System.SystemName;
                    moduleMenu.ParentModuleID = item.ParentModule.ModuleID.ToString();
                    moduleMenu.ParentModuleName = item.ParentModule.ModuleName;

                    moduleMenu.ModuleURL = item.ModuleURL;
                    moduleMenu.iconCls = item.IndicateImage;
                    moduleMenu.ShowOrder = item.ShowOrder;
                    moduleMenuSet.Add(moduleMenu);
                    GetChildMenu(moduleMenu, item);
                    moduleMenuSet.Add(moduleMenu);
                }
                systemMenu.children = moduleMenuSet.ToArray();
                systemMenuSet.Add(systemMenu);
            }

            return systemMenuSet.ToArray();
        }

        public bool Add(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemID, string moduleID)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.Module> queryModule = ModuleRepository.GetQueryable();
            moduleID = !String.IsNullOrEmpty(moduleID) ? moduleID : "40DD7298-F410-43F2-840A-7C04F09B5CE2";
            var system = querySystem.FirstOrDefault(i => i.SystemID == new Guid(systemID));
            var parentModule = queryModule.FirstOrDefault(i => i.ModuleID == new Guid(moduleID));
            var module = new Module();
            module.ModuleID = Guid.NewGuid();
            module.ModuleName = moduleName;
            module.ShowOrder = showOrder;
            module.ModuleURL = moduleUrl;
            module.IndicateImage = indicateImage;
            module.DeskTopImage = desktopImage;
            module.System = system;
            module.ParentModule = parentModule ?? module;
            ModuleRepository.Add(module);
            ModuleRepository.SaveChanges();
            return true;
        }

        public bool Delete(string moduleID)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.Module> queryModule = ModuleRepository.GetQueryable();

            Guid gmoduleId = new Guid(moduleID);
            var module = queryModule.FirstOrDefault(i => i.ModuleID == gmoduleId);
            if (module != null)
            {
                Del(FunctionRepository, module.Functions);
                Del(ModuleRepository, module.Modules);
                Del(RoleModuleRepository, module.RoleModules);
                Del(UserModuleRepository, module.UserModules);

                ModuleRepository.Delete(module);
                ModuleRepository.SaveChanges();
            }
            else
                return false;
            return true;
        }

        public bool Save(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.Module> queryModule = ModuleRepository.GetQueryable();
            Guid sid = new Guid(moduleID);
            var module = queryModule.FirstOrDefault(i => i.ModuleID == sid);
            module.ModuleName = moduleName;
            module.ShowOrder = showOrder;
            module.ModuleURL = moduleUrl;
            module.IndicateImage = indicateImage;
            module.DeskTopImage = deskTopImage;
            ModuleRepository.SaveChanges();
            return true;
        }

        private void GetChildMenu(Menu menu, Module module)
        {
            HashSet<Menu> childMenuSet = new HashSet<Menu>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Menu childMenu = new Menu();
                    childMenu.ModuleID = item.ModuleID.ToString();
                    childMenu.ModuleName = item.ModuleName;
                    childMenu.SystemID = item.System.SystemID.ToString();
                    childMenu.SystemName = item.System.SystemName;
                    childMenu.ParentModuleID = item.ParentModule.ModuleID.ToString();
                    childMenu.ParentModuleName = item.ParentModule.ModuleName;
                    childMenu.ModuleURL = item.ModuleURL;
                    childMenu.iconCls = item.IndicateImage;
                    childMenu.ShowOrder = item.ShowOrder;
                    childMenuSet.Add(childMenu);
                    if (item.Modules.Count > 0)
                    {
                        GetChildMenu(childMenu, item);
                    }
                }
            }
            menu.children = childMenuSet.ToArray();
        }

        #endregion

        #region 页面权限控制        
        
        public object GetUserMenus(string userName, string cityID, string systemID)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为NULL或为空。", "userName");
            if (String.IsNullOrEmpty(cityID)) throw new ArgumentException("值不能为NULL或为空。", "cityID");
            if (String.IsNullOrEmpty(systemID)) throw new ArgumentException("值不能为NULL或为空。", "systemId");

            IQueryable<THOK.Authority.Dal.EntityModels.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.City> queryCity = CityRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();

            Guid gSystemID = new Guid(systemID);
            Guid gCityID = new Guid(cityID);
            var user = queryUser.Single(u => u.UserName == userName);
            var city = queryCity.Single(c => c.CityID == gCityID);
            var system = querySystem.Single(s => s.SystemID == gSystemID);
            InitUserSystem(user, city, system);

            var userSystem = (from us in user.UserSystems
                              where us.User.UserID == user.UserID
                                && us.City.CityID == city.CityID
                                && us.System.SystemID == system.SystemID
                              select us).Single();

            HashSet<Menu> systemMenuSet = new HashSet<Menu>();
            Menu systemMenu = new Menu();
            systemMenu.ModuleID = userSystem.System.SystemID.ToString();
            systemMenu.ModuleName = userSystem.System.SystemName;
            systemMenu.SystemID = userSystem.System.SystemID.ToString();
            systemMenu.SystemName = userSystem.System.SystemName;

            var userModules = from um in userSystem.UserModules
                              where um.Module.ModuleID == um.Module.ParentModule.ModuleID
                              orderby um.Module.ShowOrder
                              select um;

            HashSet<Menu> moduleMenuSet = new HashSet<Menu>();
            foreach (var userModule in userModules)
            {
                var roles = from ur in user.UserRoles
                            select ur.Role;
                foreach (var role in roles)
                {
                    InitRoleSystem(role, city, system);
                }
                var roleModules = queryRoleModule.Where(rm => rm.RoleSystem.System.SystemID == userSystem.System.SystemID
                    && rm.Module.ModuleID == userModule.Module.ModuleID)
                    .Select(rm => rm);

                if (userModule.IsActive ||
                    roleModules.Any(rm => roles.Any(rl => rl.RoleID == rm.RoleSystem.Role.RoleID)))
                {
                    var module = userModule.Module;
                    Menu moduleMenu = new Menu();
                    moduleMenu.ModuleID = module.ModuleID.ToString();
                    moduleMenu.ModuleName = module.ModuleName;
                    moduleMenu.SystemID = module.System.SystemID.ToString();
                    moduleMenu.SystemName = module.System.SystemName;
                    moduleMenu.ParentModuleID = module.ParentModule.ModuleID.ToString();
                    moduleMenu.ParentModuleName = module.ParentModule.ModuleName;
                    moduleMenu.ModuleURL = module.ModuleURL;
                    moduleMenu.iconCls = module.IndicateImage;
                    moduleMenu.ShowOrder = module.ShowOrder;
                    GetChildMenu(moduleMenu, userSystem, module);
                    moduleMenuSet.Add(moduleMenu);
                }
            }
            systemMenu.children = moduleMenuSet.ToArray();
            systemMenuSet.Add(systemMenu);
            return systemMenuSet.ToArray();
        }

        public object GetModuleFuns(string userName, string cityID, string moduleID)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("值不能为NULL或为空。", "userName");
            if (String.IsNullOrEmpty(cityID)) throw new ArgumentException("值不能为NULL或为空。", "cityID");
            if (String.IsNullOrEmpty(moduleID)) throw new ArgumentException("值不能为NULL或为空。", "moduleID");

            IQueryable<THOK.Authority.Dal.EntityModels.User> queryUser = UserRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.City> queryCity = CityRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.Module> queryModule = ModuleRepository.GetQueryable();

            Guid gCityID = new Guid(cityID);
            Guid gModuleID = new Guid(moduleID);
            var user = queryUser.Single(u => u.UserName == userName);
            var city = queryCity.Single(c => c.CityID == gCityID);
            var module = queryModule.Single(m => m.ModuleID == gModuleID);

            var userModule = (from um in module.UserModules
                              where um.UserSystem.User.UserID == user.UserID
                                && um.UserSystem.City.CityID == city.CityID
                              select um).Single();

            var userFunctions = userModule.UserFunctions;
            Fun fun = new Fun();
            HashSet<Fun> moduleFunctionSet = new HashSet<Fun>();
            foreach (var userFunction in userFunctions)
            {
                var roles = from ur in user.UserRoles
                            select ur.Role;
                bool bResult = roles.Any(rl => rl.RoleSystems.Any(rs => rs.IsActive
                    && rs.RoleModules.Any(rm => rm.IsActive
                        && rm.Module.ModuleID == module.ModuleID
                        && rm.RoleFunctions.Any(rf => rf.IsActive
                            && rf.Function.FunctionID == userFunction.Function.FunctionID))));

                moduleFunctionSet.Add(new Fun()
                    {
                        funid = userFunction.Function.FunctionID.ToString(),
                        funname = userFunction.Function.FunctionName,
                        iconCls = userFunction.Function.IndicateImage,
                        isActive = userFunction.IsActive || bResult
                    });

            }
            fun.funs = moduleFunctionSet.ToArray();
            return fun;
        }

        private void GetChildMenu(Menu moduleMenu, UserSystem userSystem, Module module)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            HashSet<Menu> childMenuSet = new HashSet<Menu>();
            var userModules = from um in userSystem.UserModules
                              where um.Module.ParentModule == module
                              orderby um.Module.ShowOrder
                              select um;
            foreach (var userModule in userModules)
            {
                var childModule = userModule.Module;
                if (childModule != module)
                {
                    var roles = from ur in userSystem.User.UserRoles
                                select ur.Role;
                    var roleModules = queryRoleModule.Where(rm => rm.RoleSystem.System.SystemID == userSystem.System.SystemID
                        && rm.Module.ModuleID == userModule.Module.ModuleID)
                        .Select(rm => rm);
                    if (userModule.IsActive ||
                        roleModules.Any(rm => roles.Any(rl => rl.RoleID == rm.RoleSystem.Role.RoleID)))
                    {
                        Menu childMenu = new Menu();
                        childMenu.ModuleID = childModule.ModuleID.ToString();
                        childMenu.ModuleName = childModule.ModuleName;
                        childMenu.SystemID = childModule.System.SystemID.ToString();
                        childMenu.SystemName = childModule.System.SystemName;
                        childMenu.ParentModuleID = childModule.ParentModule.ModuleID.ToString();
                        childMenu.ParentModuleName = childModule.ParentModule.ModuleName;
                        childMenu.ModuleURL = childModule.ModuleURL;
                        childMenu.iconCls = childModule.IndicateImage;
                        childMenu.ShowOrder = childModule.ShowOrder;
                        childMenuSet.Add(childMenu);
                        if (childModule.Modules.Count > 0)
                        {
                            GetChildMenu(childMenu, userSystem, childModule);
                        }
                    }
                }
            }
            moduleMenu.children = childMenuSet.ToArray();
        }

        #endregion

        #region 初始化角色权限

        private void InitRoleSystem(Role role, City city, Dal.EntityModels.System system)
        {
            var roleSystems = role.RoleSystems.Where(rs => rs.City.CityID == city.CityID
                && rs.System.SystemID == system.SystemID);
            if (roleSystems.Count() == 0)
            {
                RoleSystem rs = new RoleSystem()
                {
                    RoleSystemID = Guid.NewGuid(),
                    Role = role,
                    City = city,
                    System = system,
                    IsActive = false
                };
                RoleSystemRepository.Add(rs);
                RoleSystemRepository.SaveChanges();
            }

            var roleSystem = role.RoleSystems.Single(rs => rs.City.CityID == city.CityID
                && rs.System.SystemID == system.SystemID);
            InitRoleModule(roleSystem);
        }

        private void InitRoleModule(RoleSystem roleSystem)
        {
            foreach (var module in roleSystem.System.Modules)
            {
                var roleModules = roleSystem.RoleModules.Where(rm => rm.Module.ModuleID == module.ModuleID
                    && rm.RoleSystem.System.SystemID == roleSystem.System.SystemID);
                if (roleModules.Count() == 0)
                {
                    RoleModule rm = new RoleModule()
                    {
                        RoleModuleID = Guid.NewGuid(),
                        RoleSystem = roleSystem,
                        Module = module,
                        IsActive = false
                    };
                    RoleModuleRepository.Add(rm);
                    RoleModuleRepository.SaveChanges();
                }
                var roleModule = roleSystem.RoleModules.Single(rm => rm.Module.ModuleID == module.ModuleID
                    && rm.RoleSystem.System.SystemID == roleSystem.System.SystemID);
                InitRoleFunctions(roleModule);
            }
        }

        private void InitRoleFunctions(RoleModule roleModule)
        {
            foreach (var function in roleModule.Module.Functions)
            {
                var roleFunctions = roleModule.RoleFunctions.Where(rf => rf.Function.FunctionID == function.FunctionID);
                if (roleFunctions.Count() == 0)
                {
                    RoleFunction rf = new RoleFunction()
                    {
                        RoleFunctionID = Guid.NewGuid(),
                        RoleModule = roleModule,
                        Function = function,
                        IsActive = false
                    };
                    RoleFunctionRepository.Add(rf);
                    RoleFunctionRepository.SaveChanges();
                }
            }
        }

        #endregion

        #region 初始化用户权限

        private void InitUserSystem(User user, City city, Dal.EntityModels.System system)
        {
            var userSystems = user.UserSystems.Where(us => us.City.CityID == city.CityID
                && us.System.SystemID == system.SystemID);
            if (userSystems.Count() == 0)
            {
                UserSystem us = new UserSystem()
                {
                    UserSystemID = Guid.NewGuid(),
                    User = user,
                    City = city,
                    System = system,
                    IsActive = false
                };
                UserSystemRepository.Add(us);
                UserSystemRepository.SaveChanges();
            }
            var userSystem = user.UserSystems.Single(us => us.City.CityID == city.CityID
                && us.System.SystemID == system.SystemID);
            InitUserModule(userSystem);
        }

        private void InitUserModule(UserSystem userSystem)
        {
            foreach (var module in userSystem.System.Modules)
            {
                var userModules = userSystem.UserModules.Where(um => um.Module.ModuleID == module.ModuleID
                    && um.UserSystem.UserSystemID == userSystem.UserSystemID);
                if (userModules.Count() == 0)
                {
                    UserModule um = new UserModule()
                    {
                        UserModuleID = Guid.NewGuid(),
                        UserSystem = userSystem,
                        Module = module,
                        IsActive = false
                    };
                    UserModuleRepository.Add(um);
                    UserModuleRepository.SaveChanges();
                }
                var userModule = userSystem.UserModules.Single(um => um.Module.ModuleID == module.ModuleID
                    && um.UserSystem.UserSystemID == userSystem.UserSystemID);
                InitUserFunctions(userModule);
            }
        }

        private void InitUserFunctions(UserModule userModule)
        {
            foreach (var function in userModule.Module.Functions)
            {
                var userFunctions = userModule.UserFunctions.Where(uf => uf.Function.FunctionID == function.FunctionID);
                if (userFunctions.Count() == 0)
                {
                    UserFunction uf = new UserFunction()
                    {
                        UserFunctionID = Guid.NewGuid(),
                        UserModule = userModule,
                        Function = function,
                        IsActive = false
                    };
                    UserFunctionRepository.Add(uf);
                    UserFunctionRepository.SaveChanges();
                }
            }
        }

        private void SetMenu(Menu menu, Module module)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            HashSet<Menu> childMenuSet = new HashSet<Menu>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Menu childMenu = new Menu();
                    childMenu.ModuleID = item.ModuleID.ToString();
                    childMenu.ModuleName = item.ModuleName;
                    childMenu.SystemID = item.System.SystemID.ToString();
                    childMenu.SystemName = item.System.SystemName;
                    childMenu.ParentModuleID = item.ParentModule.ModuleID.ToString();
                    childMenu.ParentModuleName = item.ParentModule.ModuleName;
                    childMenu.ModuleURL = item.ModuleURL;
                    childMenu.iconCls = item.IndicateImage;
                    childMenu.ShowOrder = item.ShowOrder;
                    childMenuSet.Add(childMenu);
                    if (item.Modules.Count > 0)
                    {
                        SetMenu(childMenu, item);
                    }
                }
            }
            menu.children = childMenuSet.ToArray();
        }

        private void SetFunTree(Tree childTree, Module item)
        {
            var function = FunctionRepository.GetQueryable().Where(f => f.Module.ModuleID == item.ModuleID);
            IQueryable<THOK.Authority.Dal.EntityModels.RoleFunction> queryRoleFunction = RoleFunctionRepository.GetQueryable();
            HashSet<Tree> functionTreeSet = new HashSet<Tree>();
            foreach (var func in function)
            {
                Tree funcTree = new Tree();
                funcTree.id = func.FunctionID.ToString();
                funcTree.text = "功能：" + func.FunctionName;
                var roleFunction = queryRoleFunction.FirstOrDefault(rf=>rf.Function.FunctionID==func.FunctionID);
                funcTree.@checked = roleFunction == null ? false : roleFunction.IsActive;
                funcTree.attributes = "function";
                functionTreeSet.Add(funcTree);
            }
            childTree.children = functionTreeSet.ToArray();
        }

        #region IModuleService 成员


        public void InitRoleSys(string roleID, string cityID, string systemID)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.Role> queryRole = RoleRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.City> queryCity = CityRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.System> querySystem = SystemRepository.GetQueryable();
            var role = queryRole.Single(i => i.RoleID == new Guid(roleID));
            var city = queryCity.Single(i => i.CityID == new Guid(cityID));
            var system = querySystem.Single(i => i.SystemID == new Guid(systemID));
            InitRoleSystem(role, city, system);
        }

        public object GetRoleSystemDetails(string systemID)
        {
            IQueryable<THOK.Authority.Dal.EntityModels.System> querySystem = SystemRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.Module> queryModule = ModuleRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.RoleSystem> queryRoleSystem = RoleSystemRepository.GetQueryable();
            IQueryable<THOK.Authority.Dal.EntityModels.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            var systems = querySystem.Single(i => i.SystemID == new Guid(systemID));
            HashSet<Tree> RolesystemTreeSet = new HashSet<Tree>();
            Tree roleSystemTree = new Tree();
            roleSystemTree.id = systems.SystemID.ToString();
            roleSystemTree.text = "系统：" + systems.SystemName;
            var roleSystems = queryRoleSystem.FirstOrDefault(i => i.System.SystemID == new Guid(systemID));
            roleSystemTree.@checked = roleSystems.IsActive;
            roleSystemTree.attributes = "system";

            var modules = queryModule.Where(m => m.System.SystemID == systems.SystemID && m.ModuleID == m.ParentModule.ModuleID)
                                     .OrderBy(m => m.ShowOrder)
                                     .Select(m => m);
            HashSet<Tree> moduleTreeSet = new HashSet<Tree>();
            foreach (var item in modules)
            {
                Tree moduleTree = new Tree();
                moduleTree.id = item.ModuleID.ToString();
                moduleTree.text = "模块：" + item.ModuleName;
                string moduleID = item.ModuleID.ToString();
                var roleModules = queryRoleModule.FirstOrDefault(i => i.Module.ModuleID == new Guid(moduleID));
                moduleTree.@checked = roleModules.IsActive;
                moduleTree.attributes = "module";

                moduleTreeSet.Add(moduleTree);
                SetTree(moduleTree, item);
                moduleTreeSet.Add(moduleTree);
            }
            roleSystemTree.children = moduleTreeSet.ToArray();
            RolesystemTreeSet.Add(roleSystemTree);
            return RolesystemTreeSet.ToArray();
        }

        private void SetTree(Tree tree,Module module )
        {
            IQueryable<THOK.Authority.Dal.EntityModels.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
            HashSet<Tree> childTreeSet = new HashSet<Tree>();
            var modules = from m in module.Modules
                          orderby m.ShowOrder
                          select m;
            foreach (var item in modules)
            {
                if (item != module)
                {
                    Tree childTree = new Tree();
                    childTree.id = item.ModuleID.ToString();
                    childTree.text = "模块：" + item.ModuleName;
                    string moduleID = item.ModuleID.ToString();
                    var roleModules = queryRoleModule.FirstOrDefault(i => i.Module.ModuleID == new Guid(moduleID));
                    childTree.@checked = roleModules == null ? false : roleModules.IsActive;
                    childTree.attributes = "module";
                    childTreeSet.Add(childTree);
                    if (item.Modules.Count > 0)
                    {
                        SetTree(childTree, item);
                    }
                    if (item.Functions.Count > 0)
                    {
                        SetFunTree(childTree, item);
                    }
                }
            }
            tree.children = childTreeSet.ToArray();
        }

        public bool UpdateRolePermission(string type, string id, bool isActive)
        {
            bool result = false;
            if (type=="system")
            {
                IQueryable<THOK.Authority.Dal.EntityModels.RoleSystem> queryRoleSystem = RoleSystemRepository.GetQueryable();
                Guid sid = new Guid(id);
                var system = queryRoleSystem.FirstOrDefault(i => i.RoleSystemID == sid);
                system.IsActive = isActive;
                RoleSystemRepository.SaveChanges();
                result = true;
            }
            else if (type=="module")
            {
                IQueryable<THOK.Authority.Dal.EntityModels.RoleModule> queryRoleModule = RoleModuleRepository.GetQueryable();
                Guid mid = new Guid(id);
                var module = queryRoleModule.FirstOrDefault(i => i.RoleModuleID == mid);
                module.IsActive = isActive;
                RoleModuleRepository.SaveChanges();
                result = true;
            }
            else if (type=="function")
            {
                IQueryable<THOK.Authority.Dal.EntityModels.RoleFunction> queryRoleFunction = RoleFunctionRepository.GetQueryable();
                Guid fid = new Guid(id);
                var system = queryRoleFunction.FirstOrDefault(i => i.RoleFunctionID== fid);
                system.IsActive = isActive;
                RoleSystemRepository.SaveChanges();
                result = true;
            }
            return result;
        }

        #endregion
    }
}
