namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_data_length : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_storage", "quantity", c => c.Decimal(nullable: false, precision: 18, scale: 0));
            AlterColumn("dbo.wms_storage", "in_frozen_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 0));
            AlterColumn("dbo.wms_storage", "out_frozen_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 0));
            AlterColumn("dbo.wms_sort_order", "deliver_order", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_sort_order", "deliver_order", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_storage", "out_frozen_quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.wms_storage", "in_frozen_quantity", c => c.Int(nullable: false));
            AlterColumn("dbo.wms_storage", "quantity", c => c.Int(nullable: false));
        }
    }
}
