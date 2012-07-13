namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_WarehouseInfo : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_cell",
                c => new
                    {
                        cell_code = c.String(nullable: false, maxLength: 20),
                        cell_name = c.String(nullable: false, maxLength: 20),
                        short_name = c.String(nullable: false, maxLength: 10),
                        cell_type = c.String(nullable: false, maxLength: 4),
                        layer = c.Int(nullable: false),
                        rfid = c.String(maxLength: 100),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        area_code = c.String(nullable: false, maxLength: 20),
                        shelf_code = c.String(nullable: false, maxLength: 20),
                        default_product_code = c.String(maxLength: 20),
                        max_quantity = c.Int(nullable: false),
                        is_single = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.cell_code)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .ForeignKey("dbo.wms_area", t => t.area_code)
                .ForeignKey("dbo.wms_shelf", t => t.shelf_code)
                .ForeignKey("dbo.wms_product", t => t.default_product_code)
                .Index(t => t.warehouse_code)
                .Index(t => t.area_code)
                .Index(t => t.shelf_code)
                .Index(t => t.default_product_code);
            
            CreateTable(
                "dbo.wms_warehouse",
                c => new
                    {
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        warehouse_name = c.String(nullable: false, maxLength: 20),
                        short_name = c.String(nullable: false, maxLength: 10),
                        warehouse_type = c.String(maxLength: 1),
                        company_code = c.String(maxLength: 20),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.warehouse_code);
            
            CreateTable(
                "dbo.wms_area",
                c => new
                    {
                        area_code = c.String(nullable: false, maxLength: 20),
                        area_name = c.String(nullable: false, maxLength: 20),
                        short_name = c.String(nullable: false, maxLength: 10),
                        area_type = c.String(nullable: false, maxLength: 2),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.area_code)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .Index(t => t.warehouse_code);
            
            CreateTable(
                "dbo.wms_shelf",
                c => new
                    {
                        shelf_code = c.String(nullable: false, maxLength: 20),
                        shelf_name = c.String(nullable: false, maxLength: 20),
                        short_name = c.String(nullable: false, maxLength: 10),
                        shelf_type = c.String(nullable: false, maxLength: 2),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        area_code = c.String(nullable: false, maxLength: 20),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.shelf_code)
                .ForeignKey("dbo.wms_area", t => t.area_code)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .Index(t => t.area_code)
                .Index(t => t.warehouse_code);
            
            CreateTable(
                "dbo.wms_storage",
                c => new
                    {
                        storage_code = c.String(nullable: false, maxLength: 50),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        quantity = c.Int(nullable: false),
                        storage_time = c.DateTime(nullable: false),
                        rfid = c.String(maxLength: 100),
                        in_frozen_quantity = c.Int(nullable: false),
                        out_frozen_quantity = c.Int(nullable: false),
                        is_lock = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        lock_tag = c.String(maxLength: 50),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.storage_code)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_cell", t => t.cell_code)
                .Index(t => t.product_code)
                .Index(t => t.cell_code);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_storage", new[] { "cell_code" });
            DropIndex("dbo.wms_storage", new[] { "product_code" });
            DropIndex("dbo.wms_shelf", new[] { "warehouse_code" });
            DropIndex("dbo.wms_shelf", new[] { "area_code" });
            DropIndex("dbo.wms_area", new[] { "warehouse_code" });
            DropIndex("dbo.wms_cell", new[] { "default_product_code" });
            DropIndex("dbo.wms_cell", new[] { "shelf_code" });
            DropIndex("dbo.wms_cell", new[] { "area_code" });
            DropIndex("dbo.wms_cell", new[] { "warehouse_code" });
            DropForeignKey("dbo.wms_storage", "cell_code", "dbo.wms_cell");
            DropForeignKey("dbo.wms_storage", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_shelf", "warehouse_code", "dbo.wms_warehouse");
            DropForeignKey("dbo.wms_shelf", "area_code", "dbo.wms_area");
            DropForeignKey("dbo.wms_area", "warehouse_code", "dbo.wms_warehouse");
            DropForeignKey("dbo.wms_cell", "default_product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_cell", "shelf_code", "dbo.wms_shelf");
            DropForeignKey("dbo.wms_cell", "area_code", "dbo.wms_area");
            DropForeignKey("dbo.wms_cell", "warehouse_code", "dbo.wms_warehouse");
            DropTable("dbo.wms_storage");
            DropTable("dbo.wms_shelf");
            DropTable("dbo.wms_area");
            DropTable("dbo.wms_warehouse");
            DropTable("dbo.wms_cell");
        }
    }
}
