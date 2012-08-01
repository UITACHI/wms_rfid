using System.Data.Entity;
using THOK.Wms.DbModel.Mapping;
using THOK.Wms.Repository.Migrations;
using THOK.Authority.DbModel.Mapping;

namespace THOK.Wms.Repository
{
    public class AuthorizeContext : DbContext
    {
        static AuthorizeContext()
        {
            Database.SetInitializer<AuthorizeContext>(new MigrateDatabaseToLatestVersion<AuthorizeContext, Configuration>());
        }

		public AuthorizeContext()
			: base("Name=AuthorizeContext")
		{
		}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            #region auth

            modelBuilder.Configurations.Add(new CityMap());
            modelBuilder.Configurations.Add(new FunctionMap());
            modelBuilder.Configurations.Add(new LoginLogMap());
            modelBuilder.Configurations.Add(new ModuleMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new RoleFunctionMap());
            modelBuilder.Configurations.Add(new RoleModuleMap());
            modelBuilder.Configurations.Add(new RoleSystemMap());
            modelBuilder.Configurations.Add(new ServerMap());
            modelBuilder.Configurations.Add(new SystemMap());
            modelBuilder.Configurations.Add(new SystemEventLogMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserFunctionMap());
            modelBuilder.Configurations.Add(new UserModuleMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new UserSystemMap());

            #endregion

            #region wms

            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new DepartmentMap());
            modelBuilder.Configurations.Add(new JobMap());
            modelBuilder.Configurations.Add(new EmployeeMap());

            modelBuilder.Configurations.Add(new ProductMap());
            modelBuilder.Configurations.Add(new SupplierMap());
            modelBuilder.Configurations.Add(new UnitMap());
            modelBuilder.Configurations.Add(new UnitListMap());
            modelBuilder.Configurations.Add(new BrandMap());

            modelBuilder.Configurations.Add(new WarehouseMap());
            modelBuilder.Configurations.Add(new AreaMap());
            modelBuilder.Configurations.Add(new ShelfMap());
            modelBuilder.Configurations.Add(new CellMap());
            modelBuilder.Configurations.Add(new StorageMap());
            modelBuilder.Configurations.Add(new DailyBalanceMap());

            modelBuilder.Configurations.Add(new BillTypeMap());
            modelBuilder.Configurations.Add(new InBillMasterMap());
            modelBuilder.Configurations.Add(new InBillDetailMap());
            modelBuilder.Configurations.Add(new InBillAllotMap());
            modelBuilder.Configurations.Add(new OutBillMasterMap());
            modelBuilder.Configurations.Add(new OutBillDetailMap());
            modelBuilder.Configurations.Add(new OutBillAllotMap());

            modelBuilder.Configurations.Add(new MoveBillMasterMap());
            modelBuilder.Configurations.Add(new MoveBillDetailMap());
            modelBuilder.Configurations.Add(new CheckBillMasterMap());
            modelBuilder.Configurations.Add(new CheckBillDetailMap());

            modelBuilder.Configurations.Add(new ProfitLossBillMasterMap());
            modelBuilder.Configurations.Add(new ProfitLossBillDetailMap());
            modelBuilder.Configurations.Add(new SortOrderMap());
            modelBuilder.Configurations.Add(new SortOrderDetailMap());

            modelBuilder.Configurations.Add(new DeliverDistMap());
            modelBuilder.Configurations.Add(new DeliverLineMap());
            modelBuilder.Configurations.Add(new CustomerMap());

            modelBuilder.Configurations.Add(new SortingLineMap());
            modelBuilder.Configurations.Add(new SortingLowerlimitMap());
            modelBuilder.Configurations.Add(new SortOrderDispatchMap());
            modelBuilder.Configurations.Add(new SortWorkDispatchMap());
            #endregion
        }
    }
}
