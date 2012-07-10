namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Utilities;
    using System.Linq;
    using THOK.RfidWms.DBModel.Ef.Models;
    using THOK.RfidWms.DBModel.Ef.Models.Authority;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<AuthorizeContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AuthorizeContext context)
        {           
            context.Set<Models.Authority.City>().AddOrUpdate(
                new Models.Authority.City()
                {
                    CityID = new Guid("F8344F88-08AD-4F9A-8F45-EAD8BB471105"),
                    CityName = "梧州市",
                    Description = "梧州市",
                    IsActive = true
                }
            );
            context.SaveChanges();

            City city = context.Set<Models.Authority.City>().SingleOrDefault(c => c.CityID == new Guid("F8344F88-08AD-4F9A-8F45-EAD8BB471105"));
            context.Set<Models.Authority.Server>().AddOrUpdate(
                new Models.Authority.Server()
                {
                    ServerID = new Guid("F8344F88-08AD-4F9A-8F45-EAD8BB471106"),
                    ServerName = "梧州市主服务器",
                    Description = "梧州市主服务器",
                    Url = "",
                    IsActive = true,
                    City = city,
                    City_CityID = city.CityID
                }
            );
            context.SaveChanges();

            context.Set<Models.Authority.System>().AddOrUpdate(
                    new Models.Authority.System()
                    {
                        SystemID = new Guid("E8344F88-08AD-4F9A-8F45-EAD8BB471104"),
                        SystemName = "权限管理系统",
                        Description = "权限管理系统",
                        Status = true
                    },
                    new Models.Authority.System()
                    {
                        SystemID = new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"),
                        SystemName = "烟草商业企业RFID综合管理系统",
                        Description = "烟草商业企业RFID综合管理系统",
                        Status = true
                    }
                );
            context.SaveChanges();

            List<Module> modules = new List<Module>();
            System system_1 = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("E8344F88-08AD-4F9A-8F45-EAD8BB471104"));
            
            
            Module module1 = new Module();
            module1.ModuleID = new Guid("F8344F8A-08AD-4FDA-8F45-EAD3BB471101");
            module1.ModuleName = "服务器信息管理";
            module1.ShowOrder = 1;
            module1.ModuleURL = "";
            module1.IndicateImage = "";
            module1.DeskTopImage = "";
            module1.System = system_1;
            module1.ParentModule = module1;
            modules.Add(module1);

            Module module1_1 = new Module();
            module1_1.ModuleID = new Guid("F8344F88-08AD-4D9A-8F45-EAD3BB471102");
            module1_1.ModuleName = "地市信息";
            module1_1.ShowOrder = 1;
            module1_1.ModuleURL = "/City/";
            module1_1.IndicateImage = "";
            module1_1.DeskTopImage = "";
            module1_1.System = system_1;
            module1_1.ParentModule = module1;
            modules.Add(module1_1);

            Module module1_2 = new Module();
            module1_2.ModuleID = new Guid("F8344F88-08AD-4D9A-8F45-EAD3BB471103");
            module1_2.ModuleName = "服务器信息";
            module1_2.ShowOrder = 2;
            module1_2.ModuleURL = "/Server/";
            module1_2.IndicateImage = "";
            module1_2.DeskTopImage = "";
            module1_2.System = system_1;
            module1_2.ParentModule = module1;
            modules.Add(module1_2);

            Module module2 = new Module();
            module2.ModuleID = new Guid("F8344F88-AAAD-4F9A-8F45-EAD3BB471104");
            module2.ModuleName = "系统权限管理";
            module2.ShowOrder = 2;
            module2.ModuleURL = "";
            module2.IndicateImage = "";
            module2.DeskTopImage = "";
            module2.System = system_1;
            module2.ParentModule = module2;
            modules.Add(module2);

            Module module2_1 = new Module();
            module2_1.ModuleID = new Guid("F8344F88-08AD-4F9A-8F45-EAD3BB471105");
            module2_1.ModuleName = "系统信息";
            module2_1.ShowOrder = 1;
            module2_1.ModuleURL = "/System/";
            module2_1.IndicateImage = "";
            module2_1.DeskTopImage = "";
            module2_1.System = system_1;
            module2_1.ParentModule = module2;
            modules.Add(module2_1);

            Module module2_2 = new Module();
            module2_2.ModuleID = new Guid("F8344F88-08AD-4F9A-8F45-EAD3BB471106");
            module2_2.ModuleName = "模块信息";
            module2_2.ShowOrder = 2;
            module2_2.ModuleURL = "/Module/";
            module2_2.IndicateImage = "";
            module2_2.DeskTopImage = "";
            module2_2.System = system_1;
            module2_2.ParentModule = module2;
            modules.Add(module2_2);

            Module module2_3 = new Module();
            module2_3.ModuleID = new Guid("F8344F88-08AD-4F9A-8F45-EAD3BB471107");
            module2_3.ModuleName = "角色信息";
            module2_3.ShowOrder = 3;
            module2_3.ModuleURL = "/Role/";
            module2_3.IndicateImage = "";
            module2_3.DeskTopImage = "";
            module2_3.System = system_1;
            module2_3.ParentModule = module2;
            modules.Add(module2_3);

            Module module2_4 = new Module();
            module2_4.ModuleID = new Guid("F8344F88-08AD-4F9A-8F45-EAD3BB471108");
            module2_4.ModuleName = "用户信息";
            module2_4.ShowOrder = 4;
            module2_4.ModuleURL = "/User/";
            module2_4.IndicateImage = "";
            module2_4.DeskTopImage = "";
            module2_4.System = system_1;
            module2_4.ParentModule = module2;
            modules.Add(module2_4);

            Module module3 = new Module();
            module3.ModuleID = new Guid("F8344F88-08AD-4F9A-8F45-EAD3BB471109");
            module3.ModuleName = "系统日志管理";
            module3.ShowOrder = 3;
            module3.ModuleURL = "";
            module3.IndicateImage = "";
            module3.DeskTopImage = "";
            module3.System = system_1;
            module3.ParentModule = module3;
            modules.Add(module3);

            Module module3_1 = new Module();
            module3_1.ModuleID = new Guid("F8344F88-AAAD-4F9A-8F45-EAD3BB471110");
            module3_1.ModuleName = "登录日志";
            module3_1.ShowOrder = 1;
            module3_1.ModuleURL = "/SystemEventLog/";
            module3_1.IndicateImage = "";
            module3_1.DeskTopImage = "";
            module3_1.System = system_1;
            module3_1.ParentModule = module3;
            modules.Add(module3_1);

            Module module3_2 = new Module();
            module3_2.ModuleID = new Guid("F8344F88-AAAD-4F9A-8F45-EAD3BB471111");
            module3_2.ModuleName = "业务日志";
            module3_2.ShowOrder = 2;
            module3_2.ModuleURL = "/SystemEventLog/";
            module3_2.IndicateImage = "";
            module3_2.DeskTopImage = "";
            module3_2.System = system_1;
            module3_2.ParentModule = module3;
            modules.Add(module3_2);

            Module module3_3 = new Module();
            module3_3.ModuleID = new Guid("F8344F88-AAAD-4F9A-8F45-EAD3BB471112");
            module3_3.ModuleName = "错误日志";
            module3_3.ShowOrder = 3;
            module3_3.ModuleURL = "/SystemEventLog/";
            module3_3.IndicateImage = "";
            module3_3.DeskTopImage = "";
            module3_3.System = system_1;
            module3_3.ParentModule = module3;
            modules.Add(module3_3);

            Module module4 = new Module();
            module4.ModuleID = new Guid("F8344F88-08AD-4F9A-8F45-EAD3BB471113");
            module4.ModuleName = "帮助文档管理";
            module4.ShowOrder = 4;
            module4.ModuleURL = "";
            module4.IndicateImage = "";
            module4.DeskTopImage = "";
            module4.System = system_1;
            module4.ParentModule = module4;
            modules.Add(module4);

            Module module4_1 = new Module();
            module4_1.ModuleID = new Guid("F8344A88-08AD-4F9A-8F45-EAD3BB471114");
            module4_1.ModuleName = "帮助目录";
            module4_1.ShowOrder = 1;
            module4_1.ModuleURL = "/HelpContents/";
            module4_1.IndicateImage = "";
            module4_1.DeskTopImage = "";
            module4_1.System = system_1;
            module4_1.ParentModule = module4;
            modules.Add(module4_1);

            Module module4_2 = new Module();
            module4_2.ModuleID = new Guid("F8344A88-08AD-4F9A-8F45-EAD3BB471115");
            module4_2.ModuleName = "帮助维护";
            module4_2.ShowOrder = 2;
            module4_2.ModuleURL = "/HelpEdit/";
            module4_2.IndicateImage = "";
            module4_2.DeskTopImage = "";
            module4_2.System = system_1;
            module4_2.ParentModule = module4;
            modules.Add(module4_2);

            Module module4_3 = new Module();
            module4_3.ModuleID = new Guid("F8344A88-08AD-4F9A-8F45-EAD3BB471116");
            module4_3.ModuleName = "帮助主页";
            module4_3.ShowOrder = 3;
            module4_3.ModuleURL = "/Help/";
            module4_3.IndicateImage = "";
            module4_3.DeskTopImage = "";
            module4_3.System = system_1;
            module4_3.ParentModule = module4;
            modules.Add(module4_3);

            modules.ForEach(m => m.System_SystemID = m.System.SystemID);
            modules.ForEach(m => m.ParentModule_ModuleID = m.ParentModule.ModuleID);
            context.Set<Module>().AddOrUpdate(modules.ToArray());
            
            CreateOrg(context);
            CreateWarehouse(context);
            CreateProduct(context);
            CreateStockIn(context);
            CreateStockOut(context); 
            CreateStockMove(context);
            CreateStockCheck(context);
            CreateProfitLoss(context);
            CreateStock(context);
            CreateSorting(context);
            CreateSearch(context);
        }

        private void CreateOrg(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                   new Module()
                   {
                       ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                       ModuleName = "组织结构管理",
                       ShowOrder = 1,
                       ModuleURL = "",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471101")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102"),
                       ModuleName = "公司信息",
                       ShowOrder = 1,
                       ModuleURL = "/Company/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471101")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103"),
                       ModuleName = "部门信息",
                       ShowOrder = 2,
                       ModuleURL = "/Department/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471101")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104"),
                       ModuleName = "岗位信息",
                       ShowOrder = 3,
                       ModuleURL = "/Job/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471101")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105"),
                       ModuleName = "员工信息",
                       ShowOrder = 4,
                       ModuleURL = "/Employee/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471101")
                   }
               );
            context.SaveChanges(); 
        }

        private void CreateWarehouse(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "仓库信息管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "仓库设置",
                        ShowOrder = 1,
                        ModuleURL = "/Warehouse/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateProduct(AuthorizeContext context)
        {
            System system= context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "卷烟信息管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "卷烟信息",
                        ShowOrder = 1,
                        ModuleURL = "/Product/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "厂商信息",
                        ShowOrder = 2,
                        ModuleURL = "/Supplier/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "卷烟品牌",
                        ShowOrder = 3,
                        ModuleURL = "/Brand/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "单位系列",
                        ShowOrder = 4,
                        ModuleURL = "/UnitList/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "计量单位",
                        ShowOrder = 5,
                        ModuleURL = "/Unit/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateStockIn(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "入库管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "单据类型设置",
                        ShowOrder = 1,
                        ModuleURL = "/StockInBillType/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "入库单",
                        ShowOrder = 2,
                        ModuleURL = "/StockInBill/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "入库单审核",
                        ShowOrder = 3,
                        ModuleURL = "/StockInBillCheck/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "入库单分配",
                        ShowOrder = 4,
                        ModuleURL = "/StockInBillAllot/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "入库单确认",
                        ShowOrder = 5,
                        ModuleURL = "/StockInBillAllotConfirm/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateStockOut(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "出库管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "单据类型设置",
                        ShowOrder = 1,
                        ModuleURL = "/StockOutBillType/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "出库单",
                        ShowOrder = 2,
                        ModuleURL = "/StockOutBill/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "出库单审核",
                        ShowOrder = 3,
                        ModuleURL = "/StockOutBillCheck/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "出库单分配",
                        ShowOrder = 4,
                        ModuleURL = "/StockOutBillAllot/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "出库单确认",
                        ShowOrder = 5,
                        ModuleURL = "/StockOutBillAllotConfirm/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateStockMove(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "移库管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "单据类型设置",
                        ShowOrder = 1,
                        ModuleURL = "/StockMoveBillType/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "移库单",
                        ShowOrder = 2,
                        ModuleURL = "/StockMoveBill/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "移库单审核",
                        ShowOrder = 3,
                        ModuleURL = "/StockMoveBillCheck/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "移库单分配",
                        ShowOrder = 4,
                        ModuleURL = "/StockMoveBillAllot/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "移库单确认",
                        ShowOrder = 5,
                        ModuleURL = "/StockMoveBillAllotConfirm/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateStockCheck(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "盘点管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "单据类型设置",
                        ShowOrder = 1,
                        ModuleURL = "/CheckBillType/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "盘点单生成",
                        ShowOrder = 2,
                        ModuleURL = "/CheckBillGenerate/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "盘点单",
                        ShowOrder = 3,
                        ModuleURL = "/CheckBill/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "盘点单审核",
                        ShowOrder = 4,
                        ModuleURL = "/CheckBillVerify/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "盘点损益确认",
                        ShowOrder = 5,
                        ModuleURL = "/CheckBillConfirm/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateProfitLoss(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "损益管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "单据类型设置",
                        ShowOrder = 1,
                        ModuleURL = "/ProfitLossBillType/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "损益单",
                        ShowOrder = 2,
                        ModuleURL = "/ProfitLossBill/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "损益单审核",
                        ShowOrder = 3,
                        ModuleURL = "/ProfitLossBillVerify/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateStock(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "库存管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "库存日结",
                        ShowOrder = 1,
                        ModuleURL = "/DailyBalance/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "当前库存查询",
                        ShowOrder = 2,
                        ModuleURL = "/CurrentStock/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "库存分布查询",
                        ShowOrder = 3,
                        ModuleURL = "/StockDistribution/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "货位库存查询",
                        ShowOrder = 4,
                        ModuleURL = "/Cargospace/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "历史库存总账",
                        ShowOrder = 5,
                        ModuleURL = "/StockLedger/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                    ,
                    new Module()
                    {
                        ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471107"),
                        ModuleName = "历史明细查询",
                        ShowOrder = 6,
                        ModuleURL = "/HistoricalDetail/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateSorting(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "分拣管理",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "分拣线信息设置",
                        ShowOrder = 1,
                        ModuleURL = "/SortingLine/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "备货区下限设置",
                        ShowOrder = 2,
                        ModuleURL = "/SortingLowerLimit/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "分拣订单管理",
                        ShowOrder = 3,
                        ModuleURL = "/SortingOrder/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "分拣线路调度",
                        ShowOrder = 4,
                        ModuleURL = "/SortingAllot/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "分拣作业调度",
                        ShowOrder = 5,
                        ModuleURL = "/SortingOrderOptimize/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }

        private void CreateSearch(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "综合查询",
                        ShowOrder = 1,
                        ModuleURL = "",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471102"),
                        ModuleName = "入库单查询",
                        ShowOrder = 1,
                        ModuleURL = "/StockInSearch/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471103"),
                        ModuleName = "出库单查询",
                        ShowOrder = 2,
                        ModuleURL = "/StockOutSearch/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471104"),
                        ModuleName = "移库单查询",
                        ShowOrder = 3,
                        ModuleURL = "/StockMoveSearch/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471105"),
                        ModuleName = "盘点单查询",
                        ShowOrder = 4,
                        ModuleURL = "/StockCheckSearch/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471106"),
                        ModuleName = "损益单查询",
                        ShowOrder = 5,
                        ModuleURL = "/ProfitLossSearch/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    },
                    new Module()
                    {
                        ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471107"),
                        ModuleName = "分拣单查询",
                        ShowOrder = 6,
                        ModuleURL = "/SortOrderSearch/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471101")
                    }
                );
            context.SaveChanges();
        }
    }
}
