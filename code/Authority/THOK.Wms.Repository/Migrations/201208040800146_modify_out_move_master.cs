namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_out_move_master : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_out_bill_master", "move_bill_master_bill_no", c => c.String(maxLength: 20));
            AddForeignKey("dbo.wms_out_bill_master", "move_bill_master_bill_no", "dbo.wms_move_bill_master", "bill_no");
            CreateIndex("dbo.wms_out_bill_master", "move_bill_master_bill_no");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_master", new[] { "move_bill_master_bill_no" });
            DropForeignKey("dbo.wms_out_bill_master", "move_bill_master_bill_no", "dbo.wms_move_bill_master");
            DropColumn("dbo.wms_out_bill_master", "move_bill_master_bill_no");
        }
    }
}
