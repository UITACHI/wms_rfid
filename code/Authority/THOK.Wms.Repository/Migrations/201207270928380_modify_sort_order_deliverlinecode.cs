namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_sort_order_deliverlinecode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_sort_order", "deliver_line_code", c => c.String(nullable: false, maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_sort_order", "deliver_line_code");
        }
    }
}
