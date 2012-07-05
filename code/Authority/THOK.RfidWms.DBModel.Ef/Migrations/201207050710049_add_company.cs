namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_company : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_company",
                c => new
                    {
                        company_id = c.Guid(nullable: false),
                        company_code = c.String(nullable: false, maxLength: 20),
                        company_name = c.String(nullable: false, maxLength: 100),
                        company_type = c.String(nullable: false, maxLength: 1),
                        description = c.String(),
                        parent_company_id = c.Guid(nullable: false),
                        uniform_code = c.String(maxLength: 20),
                        warehouse_space = c.Decimal(nullable: false, precision: 18, scale: 2),
                        warehouse_count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        warehouse_capacity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        sorting_count = c.Decimal(nullable: false, precision: 18, scale: 2),
                        is_active = c.String(nullable: false, maxLength: 1),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.company_id)
                .ForeignKey("dbo.wms_company", t => t.parent_company_id)
                .Index(t => t.parent_company_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_company", new[] { "parent_company_id" });
            DropForeignKey("dbo.wms_company", "parent_company_id", "dbo.wms_company");
            DropTable("dbo.wms_company");
        }
    }
}
