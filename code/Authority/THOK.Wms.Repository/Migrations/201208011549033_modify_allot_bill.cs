namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_allot_bill : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_in_bill_allot", "in_bill_detail_id", c => c.Int(nullable: false));
            AddColumn("dbo.wms_out_bill_allot", "out_bill_detail_id", c => c.Int(nullable: false));
            AddForeignKey("dbo.wms_in_bill_allot", "in_bill_detail_id", "dbo.wms_in_bill_detail", "id");
            AddForeignKey("dbo.wms_out_bill_allot", "out_bill_detail_id", "dbo.wms_out_bill_detail", "id");
            CreateIndex("dbo.wms_in_bill_allot", "in_bill_detail_id");
            CreateIndex("dbo.wms_out_bill_allot", "out_bill_detail_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_allot", new[] { "out_bill_detail_id" });
            DropIndex("dbo.wms_in_bill_allot", new[] { "in_bill_detail_id" });
            DropForeignKey("dbo.wms_out_bill_allot", "out_bill_detail_id", "dbo.wms_out_bill_detail");
            DropForeignKey("dbo.wms_in_bill_allot", "in_bill_detail_id", "dbo.wms_in_bill_detail");
            DropColumn("dbo.wms_out_bill_allot", "out_bill_detail_id");
            DropColumn("dbo.wms_in_bill_allot", "in_bill_detail_id");
        }
    }
}
