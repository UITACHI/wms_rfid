namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_deliver : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_deliver_dist",
                c => new
                    {
                        dist_code = c.String(nullable: false, maxLength: 50),
                        custom_code = c.String(maxLength: 50),
                        dist_name = c.String(nullable: false, maxLength: 100),
                        dist_center_code = c.String(maxLength: 20),
                        company_code = c.String(maxLength: 20),
                        uniform_code = c.String(nullable: false, maxLength: 50),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.dist_code);
            
            CreateTable(
                "dbo.wms_deliver_line",
                c => new
                    {
                        deliver_line_code = c.String(nullable: false, maxLength: 50),
                        custom_code = c.String(maxLength: 50),
                        deliver_line_name = c.String(nullable: false, maxLength: 100),
                        dist_code = c.String(maxLength: 50),
                        deliver_order = c.Int(nullable: false),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.deliver_line_code);
            
            CreateTable(
                "dbo.wms_customer",
                c => new
                    {
                        customer_code = c.String(nullable: false, maxLength: 50),
                        custom_code = c.String(maxLength: 50),
                        customer_name = c.String(nullable: false, maxLength: 100),
                        company_code = c.String(maxLength: 20),
                        sale_region_code = c.String(nullable: false, maxLength: 50),
                        uniform_code = c.String(nullable: false, maxLength: 50),
                        customer_type = c.String(nullable: false, maxLength: 4, fixedLength: true),
                        sale_scope = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        industry_type = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        city_or_countryside = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        deliver_line_code = c.String(nullable: false, maxLength: 50),
                        deliver_order = c.Int(nullable: false),
                        address = c.String(nullable: false, maxLength: 100),
                        phone = c.String(nullable: false, maxLength: 20),
                        license_type = c.String(maxLength: 10),
                        license_code = c.String(maxLength: 20),
                        principal_name = c.String(maxLength: 20),
                        principal_phone = c.String(maxLength: 60),
                        principal_address = c.String(maxLength: 100),
                        management_name = c.String(maxLength: 20),
                        management_phone = c.String(maxLength: 60),
                        bank = c.String(maxLength: 50),
                        bank_accounts = c.String(maxLength: 50),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.customer_code);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.wms_customer");
            DropTable("dbo.wms_deliver_line");
            DropTable("dbo.wms_deliver_dist");
        }
    }
}
