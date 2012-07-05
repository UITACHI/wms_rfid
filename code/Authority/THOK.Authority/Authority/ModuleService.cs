using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using THOK.Authority.Data;
using System.Data.Objects.DataClasses;
using System.Collections;

namespace THOK.Authority.Authority
{
    public class ModuleService : IModuleService
    {
        public object GetDetails(string systemId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                var systems = from s in context.System
                              select s;
                if (systemId != null && systemId != string.Empty )
                {
                    Guid gsystemid = new Guid(systemId);
                    systems = from s in context.System
                              where s.SystemID == gsystemid
                              select s;
                }

                HashSet<Menu> systemMenuSet = new HashSet<Menu>();
                foreach (var system in systems)
                {
                    Menu systemMenu = new Menu();
                    systemMenu.ModuleID = system.SystemID.ToString();
                    systemMenu.ModuleName = system.SystemName;
                    systemMenu.SystemID = system.SystemID.ToString();
                    systemMenu.SystemName = system.SystemName;

                    var modules = from m in context.Module
                                  where m.System.SystemID == system.SystemID && m.ModuleID == m.ParentModule.ModuleID
                                  orderby m.ShowOrder
                                  select m;
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
                        SetMenu(moduleMenu, item);
                        moduleMenuSet.Add(moduleMenu);
                    }
                    systemMenu.children = moduleMenuSet.ToArray();
                    systemMenuSet.Add(systemMenu);
                }

                return systemMenuSet.ToArray();
            }
        }

        private void SetMenu(Menu menu, Module module)
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

                        SetMenu(childMenu, item);
                    }
                }
            }
            menu.children = childMenuSet.ToArray();
        }

        public bool AddModule(string moduleName, int showOrder, string moduleUrl, string indicateImage, string desktopImage, string systemId,string moduleId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                //try
                //{
                    string moduleid = moduleId != string.Empty && moduleId!= null ? moduleId : "40DD7298-F410-43F2-840A-7C04F09B5CE2";
                    var system = context.System.FirstOrDefault(i => i.SystemID == new Guid(systemId));
                    var parentModule = context.Module.FirstOrDefault(i => i.ModuleID == new Guid(moduleid));
                    var module = new Module();
                    module.ModuleID = Guid.NewGuid();
                    module.ModuleName = moduleName;
                    module.ShowOrder = showOrder;
                    module.ModuleURL = moduleUrl;
                    module.IndicateImage = indicateImage;
                    module.DeskTopImage = desktopImage;
                    module.System = system;
                    module.ParentModule = parentModule ?? module;
                    context.AddToModule(module);
                    context.SaveChanges();
                    return true;
                //}
                //catch (Exception e)
                //{
                //    throw e;
                //}
            }
        }
                
		public bool SaveModuleInfo(string moduleID, string moduleName, int showOrder, string moduleUrl, string indicateImage, string deskTopImage)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                try
                {
                    Guid sid = new Guid(moduleID);
                    var module = context.Module.FirstOrDefault(i => i.ModuleID == sid);
                    module.ModuleName = moduleName;
                    module.ShowOrder = showOrder;
                    module.ModuleURL = moduleUrl;
                    module.IndicateImage = indicateImage;
                    module.DeskTopImage = deskTopImage;
                    context.SaveChanges();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return true;
        }

        public bool Delete(string moduleId)
        {
            using (AuthorizeEntities context = new AuthorizeEntities())
            {
                Guid gmoduleId = new Guid(moduleId);
                var module = context.Module.FirstOrDefault(i => i.ModuleID == gmoduleId);
                if (module != null)
                {
                    Del(context, module.Functions);
                    Del(context, module.Modules);
                    Del(context, module.RoleModules);
                    Del(context, module.UserModules);
                    context.DeleteObject(module);
                    context.SaveChanges();
                }
                else
                    return false;
                return true;
            }
        }

        public void Del<TEntity>(AuthorizeEntities context, EntityCollection<TEntity> entities) where TEntity : class
        {
            var arrEntities = entities.ToArray();
            foreach (var item in arrEntities)
            {
                context.DeleteObject(item);
            }
        }

        public object GetModuleFuns(string moduleId)
        {
            throw new NotImplementedException();
        }
    }
}
