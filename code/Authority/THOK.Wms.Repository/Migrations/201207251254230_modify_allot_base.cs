namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_allot_base : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.wms_in_bill_allot", "product_code", "dbo.wms_product", "product_code");
            AddForeignKey("dbo.wms_in_bill_allot", "storage_code", "dbo.wms_storage", "storage_code");
            AddForeignKey("dbo.wms_in_bill_allot", "cell_code", "dbo.wms_cell", "cell_code");
            AddForeignKey("dbo.wms_in_bill_allot", "UnitCode", "dbo.wms_unit", "unit_code");
            AddForeignKey("dbo.wms_out_bill_allot", "product_code", "dbo.wms_product", "product_code");
            AddForeignKey("dbo.wms_out_bill_allot", "storage_code", "dbo.wms_storage", "storage_code");
            AddForeignKey("dbo.wms_out_bill_allot", "cell_code", "dbo.wms_cell", "cell_code");
            AddForeignKey("dbo.wms_out_bill_allot", "UnitCode", "dbo.wms_unit", "unit_code");
            CreateIndex("dbo.wms_in_bill_allot", "product_code");
            CreateIndex("dbo.wms_in_bill_allot", "storage_code");
            CreateIndex("dbo.wms_in_bill_allot", "cell_code");
            CreateIndex("dbo.wms_in_bill_allot", "UnitCode");
            CreateIndex("dbo.wms_out_bill_allot", "product_code");
            CreateIndex("dbo.wms_out_bill_allot", "storage_code");
            CreateIndex("dbo.wms_out_bill_allot", "cell_code");
            CreateIndex("dbo.wms_out_bill_allot", "UnitCode");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_allot", new[] { "UnitCode" });
            DropIndex("dbo.wms_out_bill_allot", new[] { "cell_code" });
            DropIndex("dbo.wms_out_bill_allot", new[] { "storage_code" });
            DropIndex("dbo.wms_out_bill_allot", new[] { "product_code" });
            DropIndex("dbo.wms_in_bill_allot", new[] { "UnitCode" });
            DropIndex("dbo.wms_in_bill_allot", new[] { "cell_code" });
            DropIndex("dbo.wms_in_bill_allot", new[] { "storage_code" });
            DropIndex("dbo.wms_in_bill_allot", new[] { "product_code" });
            DropForeignKey("dbo.wms_out_bill_allot", "UnitCode", "dbo.wms_unit");
            DropForeignKey("dbo.wms_out_bill_allot", "cell_code", "dbo.wms_cell");
            DropForeignKey("dbo.wms_out_bill_allot", "storage_code", "dbo.wms_storage");
            DropForeignKey("dbo.wms_out_bill_allot", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_in_bill_allot", "UnitCode", "dbo.wms_unit");
            DropForeignKey("dbo.wms_in_bill_allot", "cell_code", "dbo.wms_cell");
            DropForeignKey("dbo.wms_in_bill_allot", "storage_code", "dbo.wms_storage");
            DropForeignKey("dbo.wms_in_bill_allot", "product_code", "dbo.wms_product");
        }
    }
}
