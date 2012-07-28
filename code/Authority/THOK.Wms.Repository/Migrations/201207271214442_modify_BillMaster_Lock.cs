namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_BillMaster_Lock : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_move_bill_master", "lock_tag", c => c.String(maxLength: 50));
            AddColumn("dbo.wms_move_bill_master", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.wms_in_bill_master", "lock_tag", c => c.String(maxLength: 50));
            AddColumn("dbo.wms_in_bill_master", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.wms_out_bill_master", "lock_tag", c => c.String(maxLength: 50));
            AddColumn("dbo.wms_out_bill_master", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.wms_profit_loss_bill_master", "lock_tag", c => c.String(maxLength: 50));
            AddColumn("dbo.wms_profit_loss_bill_master", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_profit_loss_bill_master", "row_version");
            DropColumn("dbo.wms_profit_loss_bill_master", "lock_tag");
            DropColumn("dbo.wms_out_bill_master", "row_version");
            DropColumn("dbo.wms_out_bill_master", "lock_tag");
            DropColumn("dbo.wms_in_bill_master", "row_version");
            DropColumn("dbo.wms_in_bill_master", "lock_tag");
            DropColumn("dbo.wms_move_bill_master", "row_version");
            DropColumn("dbo.wms_move_bill_master", "lock_tag");
        }
    }
}
