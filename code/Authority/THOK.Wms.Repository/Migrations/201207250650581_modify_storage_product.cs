namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_storage_product : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.wms_storage", "Product_ProductCode", "dbo.wms_product");
            DropIndex("dbo.wms_storage", new[] { "Product_ProductCode" });
            DropColumn("dbo.wms_storage", "Product_ProductCode");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wms_storage", "Product_ProductCode", c => c.String(maxLength: 20));
            CreateIndex("dbo.wms_storage", "Product_ProductCode");
            AddForeignKey("dbo.wms_storage", "Product_ProductCode", "dbo.wms_product", "product_code");
        }
    }
}
