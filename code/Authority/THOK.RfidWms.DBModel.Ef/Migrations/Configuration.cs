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

            CreateServer(context);
            CreateSystemAuthority(context);
            CreateSystemLog(context);
            CreateHelpContents(context);

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

        private void CreateServer(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("E8344F88-08AD-4F9A-8F45-EAD8BB471104"));
            context.Set<Module>().AddOrUpdate(
                   new Module()
                   {
                       ModuleID = new Guid("87104EFF-4D12-4ac9-BD62-11F8C0DA1032"),
                       ModuleName = "服务器信息管理",
                       ShowOrder = 1,
                       ModuleURL = "",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("87104EFF-4D12-4ac9-BD62-11F8C0DA1032")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426"),
                       ModuleName = "地市信息",
                       ShowOrder = 1,
                       ModuleURL =  "/City/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("87104EFF-4D12-4ac9-BD62-11F8C0DA1032")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7"),
                       ModuleName = "服务器信息",
                       ShowOrder = 2,
                       ModuleURL = "/Server/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("87104EFF-4D12-4ac9-BD62-11F8C0DA1032")
                   }
               );
            context.SaveChanges();

            context.Set<Function>().AddOrUpdate(
                new Function()
                 {
                    FunctionID = new Guid("C2F089D7-90C6-4ae9-A941-C1454CADA3CA"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426")
                 },
                new Function()
                {
                    FunctionID = new Guid("6F34032B-6EC9-45fa-90C8-33FADDE2C4BA"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426")
                },
                new Function()
                {
                    FunctionID = new Guid("51E449C0-96C9-4f63-8453-360B4A38BF32"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426")
                },
                new Function()
                {
                    FunctionID = new Guid("EC68F16E-52E3-46bd-9D90-7E7E4A5ED5E1"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426")
                },
                new Function()
                {
                    FunctionID = new Guid("E355C8D8-E28F-4662-930F-67B61DCE4675"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426")
                },
                new Function()
                {
                    FunctionID = new Guid("63CD539F-2358-48d0-ABBE-6A09E027661A"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("4CCF72D8-2590-4b86-97B5-DCA5B6F2C426")
                },
                new Function()
                 {
                    FunctionID = new Guid("593116DE-EF9D-4ffa-BBFC-352A70B9545B"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7")
                 },
                new Function()
                {
                    FunctionID = new Guid("D255D452-767B-4dc3-8FE1-B5661E134E68"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7")
                },
                new Function()
                {
                    FunctionID = new Guid("1616070B-9D6B-4e60-9E8A-FF88BD20C2D2"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7")
                },
                new Function()
                {
                    FunctionID = new Guid("B056C5A6-8647-4f11-9471-BAE9811627E3"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7")
                },
                new Function()
                {
                    FunctionID = new Guid("09CB7C0C-8E5E-4d2b-802A-249BF9D76298"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7")
                },
                new Function()
                {
                    FunctionID = new Guid("8A0FFF66-2BE9-44c3-8B5C-C9C3FBF7EFF7"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("55F51C66-AF1F-4893-8710-A5420B899FD7")
                }
                );
            context.SaveChanges();
        }

        private void CreateSystemAuthority(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("E8344F88-08AD-4F9A-8F45-EAD8BB471104"));
            context.Set<Module>().AddOrUpdate(
                   new Module()
                   {
                       ModuleID = new Guid("0C07E22C-B747-453c-9ED6-D02962D00CE5"),
                       ModuleName = "系统权限管理",
                       ShowOrder = 1,
                       ModuleURL = "",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("0C07E22C-B747-453c-9ED6-D02962D00CE5")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052"),
                       ModuleName = "系统信息",
                       ShowOrder = 1,
                       ModuleURL = "/System/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("0C07E22C-B747-453c-9ED6-D02962D00CE5")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC"),
                       ModuleName = "模块信息",
                       ShowOrder = 2,
                       ModuleURL = "/Module/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("0C07E22C-B747-453c-9ED6-D02962D00CE5")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2"),
                       ModuleName = "角色信息",
                       ShowOrder = 3,
                       ModuleURL = "/Role/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("0C07E22C-B747-453c-9ED6-D02962D00CE5")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA"),
                       ModuleName = "用户信息",
                       ShowOrder = 4,
                       ModuleURL = "/User/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("0C07E22C-B747-453c-9ED6-D02962D00CE5")
                   }
               );
            context.SaveChanges();

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("CB027D95-9425-403e-B229-E569BC771887"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052")
                },
                new Function()
                {
                    FunctionID = new Guid("1C40D6EA-C8ED-4711-82E4-411836CA6C98"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052")
                },
                new Function()
                {
                    FunctionID = new Guid("6642B9DA-BA7F-407f-B206-A34A8FFDEAA5"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052")
                },
                new Function()
                {
                    FunctionID = new Guid("07D6EBEA-1FC8-4de3-95B4-2A36537C4553"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052")
                },
                new Function()
                {
                    FunctionID = new Guid("5497040B-EACE-4c77-896C-0431B5C226F4"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052")
                },
                new Function()
                {
                    FunctionID = new Guid("9703FC37-FA3D-4cbd-8A47-EF18FA57F7D2"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D06BA0E2-8CD8-4c61-9518-6172286C2052")
                },
                new Function()
                {
                    FunctionID = new Guid("76FE9BAF-7D5E-4022-BD8F-DBF129251D65"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("1C336946-1047-47bd-B8CD-8A4331C5075B"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("D753CA72-AA60-41b6-8879-59A34967A17C"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("2931BDD1-A043-4c4b-866A-310229C10B8C"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("E3F6C1FE-3F1A-4eb2-9AE7-CB835EFF5DB6"),
                    FunctionName = "功能",
                    ControlName = "functionadmin",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("E3C2261D-B643-4614-8943-C975E9BFFC64"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("FD685941-16E6-4965-B848-8411702BD943"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("182120F0-CC8E-43cb-858C-E074972412DC")
                },
                new Function()
                {
                    FunctionID = new Guid("1AFA4F03-297A-46a7-A634-59EAEFA7DF99"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("7143E1B5-C545-41f5-A475-BB05B058F6A7"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("726D92A4-8FEC-4836-92FE-D68BB8C8F1D2"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("78317237-70AC-41a4-8768-B17438315EE6"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("B238E721-ECC9-4b71-A82F-17C69EB1C03D"),
                    FunctionName = "权限",
                    ControlName = "permissionadmin",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("EFBF340C-1183-4a59-BABF-A1439696951F"),
                    FunctionName = "用户",
                    ControlName = "useradmin",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("71AA92B0-CD04-4042-9E6B-6B63701B90AB"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("A59DEFCF-3F17-47c2-8B8C-AEF6CEAF0B7A"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("ADD20759-88F7-43db-A885-4508DB9BEEC2")
                },
                new Function()
                {
                    FunctionID = new Guid("38EAD282-DA22-4c9a-8963-47E5B94C434C"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("7C176C75-3B0E-4f48-BEAF-54C6E68EBBAC"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("7407C692-E6B8-4731-8729-9F45BA1F42D1"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("838BE0BA-C9F1-409d-9644-6694418EF1BF"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("8CA2DF4E-F3EC-4d79-87F5-74699BFBBDA9"),
                    FunctionName = "权限",
                    ControlName = "permissionadmin",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("6B579D44-DA6C-422a-900A-DD8FB3DD028E"),
                    FunctionName = "角色",
                    ControlName = "roleadmin",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("2ABA26B1-126A-48dc-8337-E69D9D91C1A2"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                },
                new Function()
                {
                    FunctionID = new Guid("12DE4CCE-2EF1-4794-A314-FB825B38EE91"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D7A91161-12CB-4938-872C-E9CBFFFF80DA")
                }
                );
            context.SaveChanges();
        }

        private void CreateSystemLog(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("E8344F88-08AD-4F9A-8F45-EAD8BB471104"));
            context.Set<Module>().AddOrUpdate(
                   new Module()
                   {
                       ModuleID = new Guid("54C11B83-884A-4256-87E8-F3CD62004661"),
                       ModuleName = "系统日志管理",
                       ShowOrder = 1,
                       ModuleURL = "",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("54C11B83-884A-4256-87E8-F3CD62004661")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("A031D57E-2352-4a4d-81A5-8E7001915A29"),
                       ModuleName = "登录日志",
                       ShowOrder = 1,
                       ModuleURL = "/SystemEventLog/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("54C11B83-884A-4256-87E8-F3CD62004661")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("848EFB88-9816-4fd1-BD14-3D41A2876BB5"),
                       ModuleName = "业务日志",
                       ShowOrder = 2,
                       ModuleURL = "/SystemEventLog/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("54C11B83-884A-4256-87E8-F3CD62004661")
                   },
                    new Module()
                    {
                        ModuleID = new Guid("8A7D294A-FCD1-44de-A816-3D3ED2E21853"),
                        ModuleName = "错误日志",
                        ShowOrder = 3,
                        ModuleURL = "/SystemEventLog/",
                        IndicateImage = "",
                        DeskTopImage = "",
                        System = system,
                        System_SystemID = system.SystemID,
                        ParentModule_ModuleID = new Guid("54C11B83-884A-4256-87E8-F3CD62004661")
                    }
               );
            context.SaveChanges();

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("5400F22F-D11B-421a-9FBF-3B08D6BB4223"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A031D57E-2352-4a4d-81A5-8E7001915A29")
                },
                new Function()
                {
                    FunctionID = new Guid("A979C69D-BEA5-468e-9890-FA057093A0D3"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A031D57E-2352-4a4d-81A5-8E7001915A29")
                },
                new Function()
                {
                    FunctionID = new Guid("C223CB09-ADF4-4ad6-8ADB-1DDCCB0E4E5A"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A031D57E-2352-4a4d-81A5-8E7001915A29")
                },
                new Function()
                {
                    FunctionID = new Guid("FC581C8B-2D60-4a79-A112-4C7BE1C4BFCD"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("848EFB88-9816-4fd1-BD14-3D41A2876BB5")
                },
                new Function()
                {
                    FunctionID = new Guid("AC83BE1B-66FB-453f-B5CD-D4FC957C2235"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("848EFB88-9816-4fd1-BD14-3D41A2876BB5")
                },
                new Function()
                {
                    FunctionID = new Guid("B904FA69-268B-4be7-85DC-D9F30DD3C84F"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("848EFB88-9816-4fd1-BD14-3D41A2876BB5")
                },
                new Function()
                {
                    FunctionID = new Guid("F6DDF9EC-73F4-43de-803E-4E7027F9097B"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("8A7D294A-FCD1-44de-A816-3D3ED2E21853")
                },
                new Function()
                {
                    FunctionID = new Guid("864BF3F2-1717-400f-B176-C80D11017672"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("8A7D294A-FCD1-44de-A816-3D3ED2E21853")
                },
                new Function()
                {
                    FunctionID = new Guid("591A15A6-B8C0-4638-8E3B-28DBDB54C74E"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("8A7D294A-FCD1-44de-A816-3D3ED2E21853")
                }
                );
            context.SaveChanges();
        }

        private void CreateHelpContents(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("E8344F88-08AD-4F9A-8F45-EAD8BB471104"));
            context.Set<Module>().AddOrUpdate(
                   new Module()
                   {
                       ModuleID = new Guid("9280918A-632F-43a9-B611-D7597C858EA8"),
                       ModuleName = "帮助文档管理",
                       ShowOrder = 1,
                       ModuleURL = "",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("9280918A-632F-43a9-B611-D7597C858EA8")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED"),
                       ModuleName = "帮助目录",
                       ShowOrder = 1,
                       ModuleURL = "/HelpContents/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("9280918A-632F-43a9-B611-D7597C858EA8")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3"),
                       ModuleName = "帮助维护",
                       ShowOrder = 2,
                       ModuleURL = "/HelpEdit/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("9280918A-632F-43a9-B611-D7597C858EA8")
                   },
                   new Module()
                   {
                       ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0"),
                       ModuleName = "帮助主页",
                       ShowOrder = 3,
                       ModuleURL = "/Help/",
                       IndicateImage = "",
                       DeskTopImage = "",
                       System = system,
                       System_SystemID = system.SystemID,
                       ParentModule_ModuleID = new Guid("9280918A-632F-43a9-B611-D7597C858EA8")
                   }
               );
            context.SaveChanges();

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("4C0DF8C9-5B71-4e78-AC9A-6E4B6220245D"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED")
                },
                new Function()
                {
                    FunctionID = new Guid("C67CB9CE-732D-49c9-8AD3-33E417E39EB7"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED")
                },
                new Function()
                {
                    FunctionID = new Guid("973E55CE-A543-4e01-B907-992B67A63B2D"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED")
                },
                new Function()
                {
                    FunctionID = new Guid("8D0226E3-9790-4b6d-AB19-6FD9635AC9D3"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED")
                },
                new Function()
                {
                    FunctionID = new Guid("1DA521F8-25AD-40ca-B581-36168D3BF9AD"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED")
                },
                new Function()
                {
                    FunctionID = new Guid("DEEB9C0E-D9BB-446b-8A93-3448EFC9989E"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("008DD08E-CC34-4f41-98A6-3FAC28F89CED")
                },
                new Function()
                {
                    FunctionID = new Guid("1C4D26DE-B10A-4bf9-A060-17FEDA640C11"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3")
                },
                new Function()
                {
                    FunctionID = new Guid("57D9BE18-9943-405c-AC2B-A6CBD34A6C84"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3")
                },
                new Function()
                {
                    FunctionID = new Guid("3594D7A3-E40A-475e-A4A4-53335152ED42"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3")
                },
                new Function()
                {
                    FunctionID = new Guid("74CEDC92-2DF4-43a6-9BF0-DC24CAE024AD"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3")
                },
                new Function()
                {
                    FunctionID = new Guid("FA842F7D-2F2E-4563-BE14-1FC90066E49F"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3")
                },
                new Function()
                {
                    FunctionID = new Guid("D3E30CA1-7244-4ed4-ADF7-7984C3E56395"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A9FB8AFF-4233-4824-94A6-52D4B37466B3")
                },
                new Function()
                {
                    FunctionID = new Guid("F1F6E2D4-5A97-478e-82A5-BE66B1F03F71"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0")
                },
                new Function()
                {
                    FunctionID = new Guid("AB8A58CC-584E-4aee-9A36-96F99003C755"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0")
                },
                new Function()
                {
                    FunctionID = new Guid("8F0B2F3A-2A72-41ec-80F6-05756A252846"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0")
                },
                new Function()
                {
                    FunctionID = new Guid("225BAE5D-9516-43b4-8353-82B961099991"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0")
                },
                new Function()
                {
                    FunctionID = new Guid("2890F02A-FE5F-4459-82BD-1F7DD8A16D9B"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0")
                },
                new Function()
                {
                    FunctionID = new Guid("A187C965-7866-4945-9926-FA756925F584"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("63D5366F-1B84-4d72-9DC0-38BB68F0AAE0")
                }
                );
            context.SaveChanges();
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("A85AB2B3-5949-4ebf-A55F-7A46DA21EAD0"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("3E296244-5B4F-46c9-A456-FA88463D612E"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("84FC6FD9-4F81-4300-8946-7D250D98DF71"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("D368026A-68B0-4310-9532-681A62BD9670"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("C355CED1-0780-4a2e-9B81-42CC5F714808"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("65F990D8-9AC8-4718-A768-C85D37346F23"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("4957BC1E-FB21-455f-8CC3-BE1383824FC6"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("8B157B15-2827-424c-8099-806696639B1D"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("03E3D962-8442-4cf3-A634-3E0573A74046"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("C6BF5241-4D85-47a0-BAC6-969F3AF0D8E2"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("2767CA63-5260-45d1-9CCC-4AC05AAC50CB"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("4A453138-1DF0-444d-8869-FB9670E85757"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("DF01E03B-F6E2-4e68-AB0C-256F2F3FC7AE"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("8A457D90-3594-4293-AA5D-5E62A6537343"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("E322D367-75DB-43ce-9B7D-709A9131B484"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("179BE697-F7BF-4b54-8599-42FAF6BC7290"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("B31011DA-AA8D-4ea5-8F7E-979CFD31B605"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("54C285D8-AB98-454e-AC7E-F51E55339863"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("9B19A906-A2D3-4089-AF69-40E752F9C0D7"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("EF2FF820-81B7-410a-A3EE-E0809DD152C0"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("080FFAD5-9C58-4ed5-9F2A-24ABEAE7900E"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("C1E95428-3FEC-484c-845E-A4173B9FA924"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("F9F33A01-76BB-4232-87D2-1DC3B6109AC8"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("20DF1E53-5D2F-4147-8097-88F134E794AE"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("A8344F88-08AD-4FDA-8F45-EAD3BB471105")
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
                        ShowOrder = 2,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("57237A92-3213-4188-8240-BEF7A2C221AD"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("A6CA0BC0-215F-44c3-8AA3-0FFF4C2F0495"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("82EE5370-3E65-49b3-87BB-86B75E671A4D"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("D0FC3809-4204-4130-8948-3B0039E62851"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("39D78BC8-CA6B-41f3-907E-A6BE76D87487"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("5468C301-1B83-4311-98B4-98AA9A5CF3E0"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("B8344F88-08AD-4FDA-8F45-EAD3BB471102")
                }
                );
            context.SaveChanges();
        }

        private void CreateProduct(AuthorizeContext context)
        {
            System system = context.Set<System>().SingleOrDefault(s => s.SystemID == new Guid("ED0E6EF0-9DEB-4CDE-8DCF-702D5B666AA8"));
            context.Set<Module>().AddOrUpdate(
                    new Module()
                    {
                        ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471101"),
                        ModuleName = "卷烟信息管理",
                        ShowOrder = 3,
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
            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("89E04DB6-DC74-44ec-A6E5-382752824557"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102")
                 },
                new Function()
                {
                    FunctionID = new Guid("1374C404-3606-4c8b-BC2E-EB7E2626D4DD"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("561349C3-A2EA-4ee0-BA52-A434D14DA347"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("579D4CA9-6E99-40f0-AD1A-FD9B8916AF8F"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("C3C27660-DE3F-4e77-9C6E-BC37CB2C480A"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("12ACEC75-BEC2-457a-BDCE-0269C9BDED1E"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                     new Function()
                 {
                     FunctionID = new Guid("E3524C69-00BC-4d7d-BD11-73EB62A7D8C1"),
                     FunctionName = "查询",
                     ControlName = "search",
                     IndicateImage = "",
                     Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103")
                 },
                new Function()
                {
                    FunctionID = new Guid("7DD02B78-6936-4aa6-943D-CF77AC587AD6"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("1FC0760E-8E58-4e5d-9876-0F328BBFC447"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("DAF3A8A6-0092-49b2-9723-E97960A14722"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("1BFE2494-27EC-4b68-826C-AA42EBFA39C9"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("B4ACA461-0A8C-4387-BC20-952F931418BD"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                     new Function()
                 {
                     FunctionID = new Guid("7815DB01-45FE-4a64-A043-B011D992CA56"),
                     FunctionName = "查询",
                     ControlName = "search",
                     IndicateImage = "",
                     Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104")
                 },
                new Function()
                {
                    FunctionID = new Guid("727D0507-20E3-49f1-A8C3-1FBE1CA71C9A"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("2B4ED7C0-1645-4620-BFA3-C9F999593A76"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("CC924251-F880-4504-B938-D150A9E162C1"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("AAE6C29E-630D-4297-8830-FA2FC0F25F0D"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("3CD649E5-3B7F-4249-B6A7-C5B8FFB0CDE4"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                     new Function()
                 {
                     FunctionID = new Guid("8BB188B4-6280-4854-9340-9A3C14FE4E77"),
                     FunctionName = "查询",
                     ControlName = "search",
                     IndicateImage = "",
                     Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105")
                 },
                new Function()
                {
                    FunctionID = new Guid("C3FF7F83-067B-440f-A627-17932F216796"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("2D691698-BFD1-44fc-BE2A-9FAAF709354F"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("9BC0C657-734F-4904-9511-FF0E5254CE40"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("AB14EE13-BFFA-451b-9522-3552EA98F53D"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("B0CCE73E-E072-4510-8DC4-F21C36BA0DC1"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                     new Function()
                 {
                     FunctionID = new Guid("2406CEAA-5493-438c-A738-148C5966959C"),
                     FunctionName = "查询",
                     ControlName = "search",
                     IndicateImage = "",
                     Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106")
                 },
                new Function()
                {
                    FunctionID = new Guid("4FBC4C07-6E8B-4fe8-A440-757FC67DDD46"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("82DE0F87-6B85-4d39-BF9B-0B370F91328A"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("CC01F548-84AD-4a57-93FC-3294A9052F56"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("8504A60A-8982-4cc4-BB8B-D7AAE118A037"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("1C2DFE80-92ED-4c1b-98E9-9F8845430222"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("C8344F88-08AD-4FDA-8F45-EAD3BB471106")
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
                        ShowOrder = 4,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("693CC756-51ED-4b20-A26C-7AFD8F4C85E4"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("0372CE0F-0DDD-4e32-927D-E3A947575B97"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("0BCDD267-37DC-4cdc-84C8-2C88CE7EF952"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("BE278AF0-FB5C-4c5d-9592-F0B1A0AD6E46"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("28ACD285-B80D-499c-AE74-7F69F9ABA411"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("97C86966-6865-4b54-BB35-73FEA8D09A2C"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("6305942A-32EC-40b9-AAB6-178D048387B3"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("FE1E48EF-9086-48df-BD3B-AA3FFCAA4A24"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("51225C33-035C-4214-98F8-F3D8D33C101E"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("A4BE0DFB-B943-4d86-A0DB-2EECFAB85F96"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("87565519-A56F-44e2-856D-24C2868142C0"),
                    FunctionName = "下载",
                    ControlName = "Download",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("7EA785AF-EE61-4b68-9B7C-3E97E325D81C"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("A311C19E-7319-4dfa-A2F0-D79B449F29B7"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("D39FC0E7-F012-40f6-AA03-BD334AE89D64"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("FB8495F2-65CB-4629-83BF-ACC7E1D85C3B"),
                    FunctionName = "审核",
                    ControlName = "audit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("5FA0BB3E-09AC-4e83-8746-91F9F8F3192D"),
                    FunctionName = "反审",
                    ControlName = "antitrial",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("D7447E52-DFC6-46da-9BA5-98351B3D83CF"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("5471058C-39B2-4570-B9CA-18830A00A96B"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("2AB49DFF-BED2-4430-A816-AB527F099CEC"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("27A1E6FF-104A-4b9f-A079-D25BB36D5BF3"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("96CCDB93-A980-4e4d-B528-B725FB4A82DA"),
                    FunctionName = "反确认",
                    ControlName = "anticonfirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("CE3D55B0-9005-4d0f-8A36-C224ECADEA6A"),
                    FunctionName = "保存",
                    ControlName = "save",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("CE87246C-CFDA-4efe-B175-371E4D6791C2"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("0B017C30-E25A-410a-8E77-C94594FE9981"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("C23CF35B-BEC6-4cde-AAD7-C0EE160D3224"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("7E3D17A3-98A0-4a7c-A759-A6EEFF14983D"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("E42D0DFF-2759-4a41-A7E0-5B9B12CD0BA8"),
                    FunctionName = "反确认",
                    ControlName = "anticonfirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("9227749C-B6A7-4f10-86F2-8B50B168894E"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("714B6CD2-0C93-4be3-BCE0-14D494ECD180"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("D8344F88-08AD-4FDA-8F45-EAD3BB471106")
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
                        ShowOrder = 5,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("EE7542B3-3EA6-40b9-8167-73771B6E54AA"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("C01793EE-3FE8-4a37-9AC5-6C356BCA3A71"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("7703E31A-E512-4c22-B78E-82AC5AC38D03"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("6350A856-5538-4231-A82A-7BBB7BE92F92"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("A21DBC46-157F-4b75-BF61-B720F60A0D91"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("8E433214-44EA-49fd-B7B3-0018AD5F588F"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("660B5C53-48FC-4eee-96A3-72376912A044"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("1598883A-E9A1-4269-B0E8-E1E6C02E6989"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("9FE35ED3-FF3C-4952-948E-2E0939F9684A"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("AF49D51B-E421-4067-AF55-461FB0D63A83"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("EF3789C5-9369-4f93-BFFB-E300C0493698"),
                    FunctionName = "下载",
                    ControlName = "Download",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("3AA9334F-15FB-4849-BF90-67B24A0C8600"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("B30581E3-FC5B-4b49-87CD-AE03E4D80093"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("5A5A2214-CDCE-48fc-B4BF-4220F46B091B"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("6CA51067-7C6A-43b2-8508-8162F3F7D18C"),
                    FunctionName = "审核",
                    ControlName = "audit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("66195CCC-5C5A-406e-B6DD-9F875BF1650D"),
                    FunctionName = "反审",
                    ControlName = "antitrial",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("D0E72658-62B5-4983-A229-35EC4F225285"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("508CB581-263D-42c2-B4C5-63ED03B0DE83"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("58A58649-D872-4cf7-BE35-310F970CD08B"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("50909C0F-1D8B-4602-B936-7D3AB30ACBAA"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("C4F5CFA0-7476-4020-BEBE-97882CF1454A"),
                    FunctionName = "反确认",
                    ControlName = "anticonfirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("9030C99D-C997-4253-AB87-55A93EE28502"),
                    FunctionName = "保存",
                    ControlName = "save",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("5811B62F-03E0-46b4-A244-F261DCF95AF7"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("C18A58C0-10D9-493c-93D9-C9F1ABB81F9B"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("569E9708-4A8A-47ae-8307-79292B8E4102"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("C0F4311B-0D23-4fc1-8584-03098C7EC3C2"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("77E2599A-0BD3-4e79-9120-BEEB9426DE47"),
                    FunctionName = "反确认",
                    ControlName = "anticonfirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("6BEF0458-78B4-409e-8A0D-1BFF8FCD4329"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("F2AE02A2-BCE8-4a22-96A0-9A8CCCA0A42E"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("E8344F88-08AD-4FDA-8F45-EAD3BB471106")
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
                        ShowOrder = 6,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("474231FF-E0C0-4138-9C46-8D99E3AE6886"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("3FDCFDDB-6F56-4ed4-B22C-3499B1D12529"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("0A5FD1AA-6FC6-4584-84AE-CC121C53405E"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("907BB603-BB24-4663-AD98-5EA9D8CA271B"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("09EB61F6-B197-421a-BFDD-880851464E51"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("9B88C6D7-6848-4694-A2CE-0C254D5DC78C"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("EC3C66E1-4C9D-4374-B859-1B57347B1DA0"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("0E76C6E1-D27E-4534-B7B6-E1A53784E4DD"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("185F9355-253E-4052-843A-98BFEB1378CF"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("2AE3AFF1-E2E4-48f5-B66B-EBBFCD74EC6C"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("8B214732-8B57-4026-A89B-93BC125D2362"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("F2CB4D5B-7BD5-49e6-BFFC-78FDCEF7349E"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("C8758D3A-A1E4-493f-80C4-B5FC8B8F4D14"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("BB53A044-EBF6-499e-9CD9-26F5CD3BE235"),
                    FunctionName = "审核",
                    ControlName = "audit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("9061A3FF-D5E4-4e7a-9254-59CF9CE1970B"),
                    FunctionName = "反审",
                    ControlName = "antitrial",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("406A0D43-0F6E-438a-99A2-2BEF5BE23FB0"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("06FC240F-DEE8-4784-B275-AB00B293F0F6"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("4C19A853-C0D7-4d3b-81F1-EFD36C6CF2AF"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("65BFE91B-789B-45a4-BAAC-CD9006835749"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("9589F28D-1724-4829-9986-21CFCD6A7C66"),
                    FunctionName = "反确认",
                    ControlName = "anticonfirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("ACB0AEB6-6872-4338-AC5A-74A8DD7106DD"),
                    FunctionName = "保存",
                    ControlName = "save",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("10DD031E-E363-4944-9017-8F5F8A0AD588"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("22779D86-E0F3-456c-A9E1-44EBC35EEC97"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("C8DCDECA-ED8D-44b5-95C7-FB861DF5F220"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("044BB164-937E-41e1-A536-7FB7BF89AEB9"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("48D43ADC-AEF7-4d1c-999A-D6742B1D3460"),
                    FunctionName = "反确认",
                    ControlName = "anticonfirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("06B6D873-741E-47aa-8294-F8432585A0A9"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("D3F29586-EAD4-4f8e-8609-63A897AF58FC"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("F8344F88-08AD-4FDA-8F45-EAD3BB471106")
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
                        ShowOrder = 7,
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

            context.Set<Function>().AddOrUpdate(
                 new Function()
                {
                    FunctionID = new Guid("E866CD6B-4EC9-4b3d-BBBF-DFC9F7C94CDD"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("FD7D5617-7751-4369-80C8-9E392E1309C3"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("4E01EA45-A6ED-4eea-8300-7A1D5233AEC6"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("E5670FB7-D0EE-4935-A62A-3618C55FAF1A"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("571BBE20-44BC-48ef-8ABA-EE8CA7529D9A"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("A937EFBA-193C-4005-9E1F-59F3EC81AC66"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("B981741F-76D6-4da7-A417-873262A5890E"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("A98EDD95-4C18-4df5-9A82-87E1E42B26C9"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("2A0F2708-150D-43fb-8F41-26C94ED653D4"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("0042DEF4-07FE-4cd6-870E-A0A9C56652E5"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("36E6597A-64A9-4eb2-9C72-9C24C39F5652"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("20EB0C11-A9A3-42ab-860C-3BBCC0DE7935"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                 new Function()
                {
                    FunctionID = new Guid("086FAC58-BD7E-4319-8FD7-33661F0623F5"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("8FDF1767-F79C-4b62-A02A-685AAFBA860F"),
                    FunctionName = "审核",
                    ControlName = "audit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("4BF95B40-1114-4282-B434-BB08A7E2766C"),
                    FunctionName = "反审",
                    ControlName = "antitrial",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("C2E9A714-83F1-46ab-80C8-DB7C485B2CE7"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("0AA790FC-9D5E-4277-A8CA-9F9220B5F89F"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("F6DBE86D-CB8E-4900-AA8C-3619878A5729"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("5D905A4D-94BE-4ded-8634-A1598CC966C5"),
                    FunctionName = "确认",
                    ControlName = "Confirm",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("9CECE256-87DD-4c74-B716-7A4946A31F65"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("876F8393-7FB3-4dc1-98BF-3E33F4A5AB41"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FA344F88-08AD-4FDA-8F45-EAD3BB471106")
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
                        ShowOrder = 8,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("623CABAC-FC54-47d7-A337-8D265A46A1D6"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102")
                 },
                new Function()
                {
                    FunctionID = new Guid("3B947F09-78FF-44db-855F-45785DBC8F3F"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("06A9E8B3-C0DF-4c27-8CDC-B18B79427444"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("A55CE8BF-F080-4cba-8F6F-D8555AD40B5B"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("F1EE0CD3-3C6D-4fe5-B0F6-EF996DEC122C"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("D23B1541-8777-4f0b-B2E2-882DD7EBA88A"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("B509B802-D5B9-46eb-8013-AE7494D1CC90"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103")
                 },
                new Function()
                {
                    FunctionID = new Guid("BCA32B01-5FD2-4407-87D5-F86E62734722"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("0E40BB17-1A3E-49f5-8816-200093AF3150"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("BB80FFAE-27AD-4bf3-99B1-CDF420145AAA"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("C3C1362E-2946-46f4-8939-40ADD4A275DE"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("C70881F2-C98B-4b83-BCAD-E151E70E8439"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("87D24444-A4E9-4f98-A633-676D713C6AD9"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("A3BE77BF-1FFA-4647-8811-70DD92C5CDA2"),
                    FunctionName = "审核",
                    ControlName = "audit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("3F8AEB38-5BD9-40f6-97C7-1D464B6B4DEF"),
                    FunctionName = "反审",
                    ControlName = "antitrial",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("B466CA78-BEDA-45a0-9A68-8CA4EE19705C"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("105444C9-C1E4-47f9-BBFD-63E74B54002F"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FB344F88-08AD-4FDA-8F45-EAD3BB471104")
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
                        ShowOrder = 9,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("54D23A0C-DE19-4cb3-9501-B270AE8790A6"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("08620A48-281C-4cf9-8D61-D7A232F19557"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("FC13ED35-EC24-42c1-BB26-F1BDEEE8D1ED"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("EFCBEFED-32C9-4031-8FB4-21DFEF3EF9A8"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("6C52AB11-5E5E-4bce-88E8-AA33CB4994ED"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("51963EC5-0C7B-45e1-995E-65E89E5AD135"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("16FCCB0F-D99C-4e58-AE14-F61B0BAAA6B0"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("C1814498-1069-473d-95D8-DD18CFABEAA2"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("46BD48EF-BBE5-4699-B0E2-1AF6B353CC78"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("7F2A6227-2A40-45c6-8BF6-5271B041089F"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("45B68172-26D0-4d92-9135-4AAFA820E0CE"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("384D20FE-7035-40b8-9808-6A714AE2ED90"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("CFF1F588-4C65-412b-BDD8-A6732DFED24D"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("E28CD5F7-36AD-4df0-BA0F-867FA4E47AC0"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("99ABB6BB-5846-4ad7-9D31-B65BABFAAAE6"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("FF73D025-EDD8-41b2-B393-05DAC0BA75D3"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("0BB4ACC6-3964-4520-B3F3-D29E28A96D29"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471107")
                },
                new Function()
                {
                    FunctionID = new Guid("B6197758-0DEF-43d8-BDCC-E0609D48D80C"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471107")
                },
                new Function()
                {
                    FunctionID = new Guid("6375FB25-510E-4431-BD91-8FA8D75CAC05"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FC344F88-08AD-4FDA-8F45-EAD3BB471107")
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
                        ShowOrder = 10,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("2DDBDA1F-9495-4659-8975-F6388B642947"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("3E14D9AF-ACDB-4195-9FB6-47503B9F1565"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("E41A1AA9-D5B9-4bac-8A29-808FE129130B"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("FBAABA03-4697-4f83-98ED-C3550F54871F"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("6FF92D17-36ED-4282-8973-83A72535F415"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("19F95169-AB68-4dc3-8A07-7D324010A2A1"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("24F9F100-FC5C-4222-9982-D446403E72D9"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("7AF036DE-EA3A-4026-AC7F-F215AE888B7B"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("299263A1-BAFE-43b2-9739-69479153ED22"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("1A6D4FF6-BC82-4393-88E3-8521B5936889"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("AC729C78-3E3F-478a-8DAA-6C0347BA3F15"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("A6D9FB57-F3EC-43e0-94F4-1DE5115A68EC"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("2CF05C43-E5B0-4575-85D3-B730BD6E5104"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("3F7D17E0-750C-4fa5-A7F8-8D6904559960"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("C1712662-B353-4cda-B8BA-D4F03222C9F4"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("03A49A5E-3995-4bb1-BA11-1E459BB37767"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("135E3A1B-E86A-4a3a-AB2E-DA02AE9DF5B4"),
                    FunctionName = "下载",
                    ControlName = "Download",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("8F9C9A2E-8BE2-436f-AF4C-28BA6F9C13DE"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("BD299336-2207-4b17-B232-9AD55FBD4654"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("D33A7F8F-28BE-41c2-9F28-A2FC01D29E2B"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("880A30A1-63B5-4616-8543-92B661392968"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("5FC70683-E090-423a-9706-7C98AB4205AD"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("DAE3AEFF-9A80-4911-90BD-267BC363FBF3"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("2EFA4ADF-CC5C-48ed-B248-EDF496E18A09"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("839F9C61-6C05-48bf-A82E-E05705F727E7"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("DDE8894B-D24E-4049-8DD5-B7547E772883"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("30A8A88D-B338-4f2b-ACE2-205EAB732084"),
                    FunctionName = "新增",
                    ControlName = "add",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("600DE9BB-F3C3-4856-BF1B-17706AC6DE3C"),
                    FunctionName = "编辑",
                    ControlName = "edit",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("EB333E0F-9973-4bcc-BC87-EF253E0BFB7C"),
                    FunctionName = "删除",
                    ControlName = "delete",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("F9134624-2316-4f1b-B50A-3680FC7CA439"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("60A88801-6EB4-4921-94C1-EE8D31E7A9D4"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FD344F88-08AD-4FDA-8F45-EAD3BB471106")
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
                        ShowOrder = 11,
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

            context.Set<Function>().AddOrUpdate(
                new Function()
                {
                    FunctionID = new Guid("4E85AFA7-91BC-44ee-853B-A185A0246A88"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("7C77134B-A8CE-4ff1-88C7-2DCE25F89B51"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("E810A404-246D-4825-A721-419C582F33E4"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471102")
                },
                new Function()
                {
                    FunctionID = new Guid("8656F99A-977F-4777-B4FF-61EFDA5DBBBE"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("4C974309-03BE-452b-A8C7-FC44685E0922"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("D214B545-F49E-414c-898F-4831E9F10856"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471103")
                },
                new Function()
                {
                    FunctionID = new Guid("C71C71CC-700F-4258-8178-1AF5939FD130"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("FBBECF2C-66EE-447a-A196-4E2919854D85"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("6C1F7938-3777-4de1-8060-6C620F7B56C9"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471104")
                },
                new Function()
                {
                    FunctionID = new Guid("9C568B0A-4137-4760-B0A3-43F26F02E610"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("7C9F7A2C-AB08-42bc-982E-905D745EB146"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("08644954-4353-42f6-A966-BDC93D008A58"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471105")
                },
                new Function()
                {
                    FunctionID = new Guid("32152D4A-E601-41d3-ADAD-0EE2B8F7EF2E"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("FBD92FE9-A07F-4a15-9EA9-532FFB499747"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("E113E3D4-378C-486f-93E6-5EBA39214CC3"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471106")
                },
                new Function()
                {
                    FunctionID = new Guid("C6B0E124-345B-420a-A227-8F94D65C0768"),
                    FunctionName = "查询",
                    ControlName = "search",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471107")
                },
                new Function()
                {
                    FunctionID = new Guid("AF2B7AEB-8883-4e09-8153-28CCD42F74D9"),
                    FunctionName = "打印",
                    ControlName = "print",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471107")
                },
                new Function()
                {
                    FunctionID = new Guid("FF238EBF-7A7F-490e-ABD2-867FB726EAF1"),
                    FunctionName = "帮助",
                    ControlName = "help",
                    IndicateImage = "",
                    Module_ModuleID = new Guid("FE344F88-08AD-4FDA-8F45-EAD3BB471107")
                }
               );
            context.SaveChanges();
        }
    }
}
