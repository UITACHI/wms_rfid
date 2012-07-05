namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init_001 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.auth_city",
                c => new
                    {
                        city_id = c.Guid(nullable: false),
                        city_name = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                        is_active = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.city_id);
            
            CreateTable(
                "dbo.auth_role_system",
                c => new
                    {
                        role_system_id = c.Guid(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        role_role_id = c.Guid(nullable: false),
                        city_city_id = c.Guid(nullable: false),
                        system_system_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.role_system_id)
                .ForeignKey("dbo.auth_city", t => t.city_city_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_role", t => t.role_role_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_system", t => t.system_system_id, cascadeDelete: false)
                .Index(t => t.city_city_id)
                .Index(t => t.role_role_id)
                .Index(t => t.system_system_id);
            
            CreateTable(
                "dbo.auth_role",
                c => new
                    {
                        role_id = c.Guid(nullable: false),
                        role_name = c.String(nullable: false, maxLength: 50),
                        is_lock = c.Boolean(nullable: false),
                        memo = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.role_id);
            
            CreateTable(
                "dbo.auth_user_role",
                c => new
                    {
                        user_role_id = c.Guid(nullable: false),
                        role_role_id = c.Guid(nullable: false),
                        user_user_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.user_role_id)
                .ForeignKey("dbo.auth_role", t => t.role_role_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_user", t => t.user_user_id, cascadeDelete: false)
                .Index(t => t.role_role_id)
                .Index(t => t.user_user_id);
            
            CreateTable(
                "dbo.auth_user",
                c => new
                    {
                        user_id = c.Guid(nullable: false),
                        user_name = c.String(nullable: false, maxLength: 50),
                        pwd = c.String(nullable: false, maxLength: 50),
                        chinese_name = c.String(maxLength: 50),
                        is_lock = c.Boolean(nullable: false),
                        is_admin = c.Boolean(nullable: false),
                        login_pc = c.String(maxLength: 50),
                        memo = c.String(maxLength: 200),
                    })
                .PrimaryKey(t => t.user_id);
            
            CreateTable(
                "dbo.auth_login_log",
                c => new
                    {
                        log_id = c.Guid(nullable: false),
                        login_pc = c.String(nullable: false, maxLength: 50),
                        login_time = c.String(nullable: false, maxLength: 30),
                        logout_time = c.String(maxLength: 30),
                        user_user_id = c.Guid(nullable: false),
                        system_system_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.log_id)
                .ForeignKey("dbo.auth_system", t => t.system_system_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_user", t => t.user_user_id, cascadeDelete: false)
                .Index(t => t.system_system_id)
                .Index(t => t.user_user_id);
            
            CreateTable(
                "dbo.auth_system",
                c => new
                    {
                        system_id = c.Guid(nullable: false),
                        system_name = c.String(nullable: false, maxLength: 100),
                        description = c.String(),
                        status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.system_id);
            
            CreateTable(
                "dbo.auth_module",
                c => new
                    {
                        module_id = c.Guid(nullable: false),
                        module_name = c.String(nullable: false, maxLength: 20),
                        show_order = c.Int(nullable: false),
                        module_url = c.String(nullable: false, maxLength: 100),
                        indicate_image = c.String(maxLength: 100),
                        desk_top_image = c.String(maxLength: 100),
                        system_system_id = c.Guid(nullable: false),
                        parent_module_module_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.module_id)
                .ForeignKey("dbo.auth_module", t => t.parent_module_module_id)
                .ForeignKey("dbo.auth_system", t => t.system_system_id, cascadeDelete: false)
                .Index(t => t.parent_module_module_id)
                .Index(t => t.system_system_id);
            
            CreateTable(
                "dbo.auth_function",
                c => new
                    {
                        function_id = c.Guid(nullable: false),
                        function_name = c.String(nullable: false, maxLength: 50),
                        control_name = c.String(nullable: false, maxLength: 50),
                        indicate_image = c.String(maxLength: 100),
                        module_module_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.function_id)
                .ForeignKey("dbo.auth_module", t => t.module_module_id, cascadeDelete: false)
                .Index(t => t.module_module_id);
            
            CreateTable(
                "dbo.auth_role_function",
                c => new
                    {
                        role_function_id = c.Guid(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        role_module_role_module_id = c.Guid(nullable: false),
                        function_function_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.role_function_id)
                .ForeignKey("dbo.auth_function", t => t.function_function_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_role_module", t => t.role_module_role_module_id, cascadeDelete: false)
                .Index(t => t.function_function_id)
                .Index(t => t.role_module_role_module_id);
            
            CreateTable(
                "dbo.auth_role_module",
                c => new
                    {
                        role_module_id = c.Guid(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        role_system_role_system_id = c.Guid(nullable: false),
                        module_module_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.role_module_id)
                .ForeignKey("dbo.auth_module", t => t.module_module_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_role_system", t => t.role_system_role_system_id, cascadeDelete: false)
                .Index(t => t.module_module_id)
                .Index(t => t.role_system_role_system_id);
            
            CreateTable(
                "dbo.auth_user_function",
                c => new
                    {
                        user_function_id = c.Guid(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        user_module_user_module_id = c.Guid(nullable: false),
                        function_function_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.user_function_id)
                .ForeignKey("dbo.auth_function", t => t.function_function_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_user_module", t => t.user_module_user_module_id, cascadeDelete: false)
                .Index(t => t.function_function_id)
                .Index(t => t.user_module_user_module_id);
            
            CreateTable(
                "dbo.auth_user_module",
                c => new
                    {
                        user_module_id = c.Guid(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        user_system_user_system_id = c.Guid(nullable: false),
                        module_module_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.user_module_id)
                .ForeignKey("dbo.auth_module", t => t.module_module_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_user_system", t => t.user_system_user_system_id, cascadeDelete: false)
                .Index(t => t.module_module_id)
                .Index(t => t.user_system_user_system_id);
            
            CreateTable(
                "dbo.auth_user_system",
                c => new
                    {
                        user_system_id = c.Guid(nullable: false),
                        is_active = c.Boolean(nullable: false),
                        user_user_id = c.Guid(nullable: false),
                        city_city_id = c.Guid(nullable: false),
                        system_system_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.user_system_id)
                .ForeignKey("dbo.auth_city", t => t.city_city_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_system", t => t.system_system_id, cascadeDelete: false)
                .ForeignKey("dbo.auth_user", t => t.user_user_id, cascadeDelete: false)
                .Index(t => t.city_city_id)
                .Index(t => t.system_system_id)
                .Index(t => t.user_user_id);
            
            CreateTable(
                "dbo.auth_server",
                c => new
                    {
                        server_id = c.Guid(nullable: false),
                        server_name = c.String(nullable: false, maxLength: 50),
                        description = c.String(),
                        url = c.String(),
                        is_active = c.Boolean(nullable: false),
                        city_city_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.server_id)
                .ForeignKey("dbo.auth_city", t => t.city_city_id, cascadeDelete: false)
                .Index(t => t.city_city_id);
            
            CreateTable(
                "dbo.auth_system_event_log",
                c => new
                    {
                        event_log_id = c.Guid(nullable: false),
                        event_log_time = c.String(nullable: false, maxLength: 30),
                        event_type = c.String(nullable: false),
                        event_name = c.String(nullable: false),
                        event_description = c.String(nullable: false),
                        from_pc = c.String(nullable: false),
                        operate_user = c.String(nullable: false),
                        target_system = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.event_log_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.auth_server", new[] { "city_city_id" });
            DropIndex("dbo.auth_user_system", new[] { "user_user_id" });
            DropIndex("dbo.auth_user_system", new[] { "system_system_id" });
            DropIndex("dbo.auth_user_system", new[] { "city_city_id" });
            DropIndex("dbo.auth_user_module", new[] { "user_system_user_system_id" });
            DropIndex("dbo.auth_user_module", new[] { "module_module_id" });
            DropIndex("dbo.auth_user_function", new[] { "user_module_user_module_id" });
            DropIndex("dbo.auth_user_function", new[] { "function_function_id" });
            DropIndex("dbo.auth_role_module", new[] { "role_system_role_system_id" });
            DropIndex("dbo.auth_role_module", new[] { "module_module_id" });
            DropIndex("dbo.auth_role_function", new[] { "role_module_role_module_id" });
            DropIndex("dbo.auth_role_function", new[] { "function_function_id" });
            DropIndex("dbo.auth_function", new[] { "module_module_id" });
            DropIndex("dbo.auth_module", new[] { "system_system_id" });
            DropIndex("dbo.auth_module", new[] { "parent_module_module_id" });
            DropIndex("dbo.auth_login_log", new[] { "user_user_id" });
            DropIndex("dbo.auth_login_log", new[] { "system_system_id" });
            DropIndex("dbo.auth_user_role", new[] { "user_user_id" });
            DropIndex("dbo.auth_user_role", new[] { "role_role_id" });
            DropIndex("dbo.auth_role_system", new[] { "system_system_id" });
            DropIndex("dbo.auth_role_system", new[] { "role_role_id" });
            DropIndex("dbo.auth_role_system", new[] { "city_city_id" });
            DropForeignKey("dbo.auth_server", "city_city_id", "dbo.auth_city");
            DropForeignKey("dbo.auth_user_system", "user_user_id", "dbo.auth_user");
            DropForeignKey("dbo.auth_user_system", "system_system_id", "dbo.auth_system");
            DropForeignKey("dbo.auth_user_system", "city_city_id", "dbo.auth_city");
            DropForeignKey("dbo.auth_user_module", "user_system_user_system_id", "dbo.auth_user_system");
            DropForeignKey("dbo.auth_user_module", "module_module_id", "dbo.auth_module");
            DropForeignKey("dbo.auth_user_function", "user_module_user_module_id", "dbo.auth_user_module");
            DropForeignKey("dbo.auth_user_function", "function_function_id", "dbo.auth_function");
            DropForeignKey("dbo.auth_role_module", "role_system_role_system_id", "dbo.auth_role_system");
            DropForeignKey("dbo.auth_role_module", "module_module_id", "dbo.auth_module");
            DropForeignKey("dbo.auth_role_function", "role_module_role_module_id", "dbo.auth_role_module");
            DropForeignKey("dbo.auth_role_function", "function_function_id", "dbo.auth_function");
            DropForeignKey("dbo.auth_function", "module_module_id", "dbo.auth_module");
            DropForeignKey("dbo.auth_module", "system_system_id", "dbo.auth_system");
            DropForeignKey("dbo.auth_module", "parent_module_module_id", "dbo.auth_module");
            DropForeignKey("dbo.auth_login_log", "user_user_id", "dbo.auth_user");
            DropForeignKey("dbo.auth_login_log", "system_system_id", "dbo.auth_system");
            DropForeignKey("dbo.auth_user_role", "user_user_id", "dbo.auth_user");
            DropForeignKey("dbo.auth_user_role", "role_role_id", "dbo.auth_role");
            DropForeignKey("dbo.auth_role_system", "system_system_id", "dbo.auth_system");
            DropForeignKey("dbo.auth_role_system", "role_role_id", "dbo.auth_role");
            DropForeignKey("dbo.auth_role_system", "city_city_id", "dbo.auth_city");
            DropTable("dbo.auth_system_event_log");
            DropTable("dbo.auth_server");
            DropTable("dbo.auth_user_system");
            DropTable("dbo.auth_user_module");
            DropTable("dbo.auth_user_function");
            DropTable("dbo.auth_role_module");
            DropTable("dbo.auth_role_function");
            DropTable("dbo.auth_function");
            DropTable("dbo.auth_module");
            DropTable("dbo.auth_system");
            DropTable("dbo.auth_login_log");
            DropTable("dbo.auth_user");
            DropTable("dbo.auth_user_role");
            DropTable("dbo.auth_role");
            DropTable("dbo.auth_role_system");
            DropTable("dbo.auth_city");
        }
    }
}
