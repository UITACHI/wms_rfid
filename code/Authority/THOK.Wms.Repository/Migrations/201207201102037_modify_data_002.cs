namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_data_002 : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.wms_out_bill_detail", "product_code", "dbo.wms_product", "product_code", cascadeDelete: true);
            AddForeignKey("dbo.wms_out_bill_detail", "unit_code", "dbo.wms_unit", "unit_code", cascadeDelete: true);
            CreateIndex("dbo.wms_out_bill_detail", "product_code");
            CreateIndex("dbo.wms_out_bill_detail", "unit_code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_out_bill_detail", new[] { "product_code" });
            DropForeignKey("dbo.wms_out_bill_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_out_bill_detail", "product_code", "dbo.wms_product");
        }
    }
}
