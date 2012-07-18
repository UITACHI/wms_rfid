using System.Data.Entity;
using THOK.Wms.DbModel.Mapping;
using THOK.Wms.DbModel;
using THOK.Wms.Repository.Migrations;
using THOK.Wms.DbModel;
using THOK.Wms.DbModel.Mapping;
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

        #region auth
                
        //public DbSet<City> Cities { get; set; }
        //public DbSet<Function> Functions { get; set; }
        //public DbSet<LoginLog> LoginLogs { get; set; }
        //public DbSet<Module> Modules { get; set; }
        //public DbSet<Role> Roles { get; set; }
        //public DbSet<RoleFunction> RoleFunctions { get; set; }
        //public DbSet<RoleModule> RoleModules { get; set; }
        //public DbSet<RoleSystem> RoleSystems { get; set; }
        //public DbSet<Server> Servers { get; set; }
        //public DbSet<THOK.Authority.DbModel.System> Systems { get; set; }
        //public DbSet<SystemEventLog> SystemEventLogs { get; set; }
        //public DbSet<User> Users { get; set; }
        //public DbSet<UserFunction> UserFunctions { get; set; }
        //public DbSet<UserModule> UserModules { get; set; }
        //public DbSet<UserRole> UserRoles { get; set; }
        //public DbSet<UserSystem> UserSystems { get; set; }

        #endregion

        #region wms

        //public DbSet<Company> Companies { get; set; }
        //public DbSet<Department> Departments { get; set; }
        //public DbSet<Job> Jobs { get; set; }
        //public DbSet<Employee> Employees { get; set; }

        #endregion

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
            modelBuilder.Configurations.Add(new DailyBalanceMap());//++

            modelBuilder.Configurations.Add(new BillTypeMap());
            modelBuilder.Configurations.Add(new InBillMasterMap());
            modelBuilder.Configurations.Add(new InBillDetailMap());
            modelBuilder.Configurations.Add(new InBillAllotMap());
            modelBuilder.Configurations.Add(new OutBillMasterMap());
            modelBuilder.Configurations.Add(new OutBillDetailMap());
            modelBuilder.Configurations.Add(new OutBillAllotMap());

            modelBuilder.Configurations.Add(new MoveBillMasterMap());//++
            modelBuilder.Configurations.Add(new MoveBillDetailMap());//++
            modelBuilder.Configurations.Add(new CheckBillMasterMap());//++
            modelBuilder.Configurations.Add(new CheckBillDetailMap());//++

            #endregion
        }
    }
}
