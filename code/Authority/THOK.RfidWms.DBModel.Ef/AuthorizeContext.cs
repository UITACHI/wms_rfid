using System.Data.Entity;
using THOK.RfidWms.DBModel.Ef.Models.Authority.Mapping;
using THOK.RfidWms.DBModel.Ef.Models.Authority;
using THOK.RfidWms.DBModel.Ef.Migrations;

namespace THOK.RfidWms.DBModel.Ef
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

        public DbSet<City> Cities { get; set; }
        public DbSet<Function> Functions { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleFunction> RoleFunctions { get; set; }
        public DbSet<RoleModule> RoleModules { get; set; }
        public DbSet<RoleSystem> RoleSystems { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<THOK.RfidWms.DBModel.Ef.Models.Authority.System> Systems { get; set; }
        public DbSet<SystemEventLog> SystemEventLogs { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFunction> UserFunctions { get; set; }
        public DbSet<UserModule> UserModules { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserSystem> UserSystems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
        }
    }
}
