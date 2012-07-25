namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_storage_productcode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_storage", "product_code", c => c.String(maxLength: 20));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_storage", "product_code", c => c.String(nullable: false, maxLength: 20));
        }
    }
}
