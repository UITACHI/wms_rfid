namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_ProfitLossBill_storage : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.wms_profit_loss_bill_detail", "storage_code", "dbo.wms_storage", "storage_code");
            CreateIndex("dbo.wms_profit_loss_bill_detail", "storage_code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_profit_loss_bill_detail", new[] { "storage_code" });
            DropForeignKey("dbo.wms_profit_loss_bill_detail", "storage_code", "dbo.wms_storage");
        }
    }
}
