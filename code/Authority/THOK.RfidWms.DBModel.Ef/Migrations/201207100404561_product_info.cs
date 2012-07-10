namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class product_info : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_product",
                c => new
                    {
                        product_code = c.String(nullable: false, maxLength: 20),
                        product_name = c.String(nullable: false, maxLength: 50),
                        uniform_code = c.String(nullable: false, maxLength: 20),
                        custom_code = c.String(maxLength: 20),
                        short_code = c.String(name: "short_code)", maxLength: 10),
                        unit_list_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        supplier_code = c.String(nullable: false, maxLength: 20),
                        brand_code = c.String(nullable: false, maxLength: 20),
                        abc_type_code = c.String(maxLength: 1, fixedLength: true),
                        product_type_code = c.String(maxLength: 4, fixedLength: true),
                        pack_type_code = c.String(maxLength: 4, fixedLength: true),
                        price_level_code = c.String(maxLength: 4),
                        statistic_type = c.String(maxLength: 10),
                        piece_barcode = c.String(maxLength: 13),
                        bar_barcode = c.String(maxLength: 13),
                        package_barcode = c.String(maxLength: 13),
                        one_project_barcode = c.String(maxLength: 30),
                        buy_price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        trade_price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        retail_price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        cost_price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        is_filter_tip = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_new = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_famous = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_main_product = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_province_main_product = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        belong_region = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_confiscate = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_abnormity = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.product_code)
                .ForeignKey("dbo.wms_brand", t => t.brand_code)
                .ForeignKey("dbo.wms_supplier", t => t.supplier_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .ForeignKey("dbo.wms_unit_list", t => t.unit_list_code)
                .Index(t => t.brand_code)
                .Index(t => t.supplier_code)
                .Index(t => t.unit_code)
                .Index(t => t.unit_list_code);
            
            CreateTable(
                "dbo.wms_brand",
                c => new
                    {
                        brand_code = c.String(nullable: false, maxLength: 20),
                        uniform_code = c.String(maxLength: 20),
                        custom_code = c.String(maxLength: 20),
                        brand_name = c.String(nullable: false, maxLength: 50),
                        supplier_code = c.String(nullable: false, maxLength: 20),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.brand_code)
                .ForeignKey("dbo.wms_supplier", t => t.supplier_code)
                .Index(t => t.supplier_code);
            
            CreateTable(
                "dbo.wms_supplier",
                c => new
                    {
                        supplier_code = c.String(nullable: false, maxLength: 20),
                        uniform_code = c.String(maxLength: 20),
                        custom_code = c.String(maxLength: 20),
                        supplier_name = c.String(nullable: false, maxLength: 50),
                        province_name = c.String(maxLength: 20),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.supplier_code);
            
            CreateTable(
                "dbo.wms_unit",
                c => new
                    {
                        unit_code = c.String(nullable: false, maxLength: 20),
                        unit_name = c.String(nullable: false, maxLength: 20),
                        count = c.Int(nullable: false),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.unit_code);
            
            CreateTable(
                "dbo.wms_unit_list",
                c => new
                    {
                        unit_list_code = c.String(nullable: false, maxLength: 20),
                        uniform_code = c.String(maxLength: 20),
                        unit_list_name = c.String(nullable: false, maxLength: 50),
                        unit_code01 = c.String(nullable: false, maxLength: 8),
                        unit_name01 = c.String(nullable: false, maxLength: 50),
                        quantity01 = c.Decimal(nullable: false, precision: 9, scale: 2),
                        unit_code02 = c.String(nullable: false, maxLength: 8),
                        unit_name02 = c.String(nullable: false, maxLength: 50),
                        quantity02 = c.Decimal(nullable: false, precision: 9, scale: 2),
                        unit_code03 = c.String(nullable: false, maxLength: 8),
                        unit_name03 = c.String(nullable: false, maxLength: 50),
                        quantity03 = c.Decimal(nullable: false, precision: 9, scale: 2),
                        unit_code04 = c.String(nullable: false, maxLength: 8),
                        unit_name04 = c.String(nullable: false, maxLength: 50),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.unit_list_code);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_brand", new[] { "supplier_code" });
            DropIndex("dbo.wms_product", new[] { "unit_list_code" });
            DropIndex("dbo.wms_product", new[] { "unit_code" });
            DropIndex("dbo.wms_product", new[] { "supplier_code" });
            DropIndex("dbo.wms_product", new[] { "brand_code" });
            DropForeignKey("dbo.wms_brand", "supplier_code", "dbo.wms_supplier");
            DropForeignKey("dbo.wms_product", "unit_list_code", "dbo.wms_unit_list");
            DropForeignKey("dbo.wms_product", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_product", "supplier_code", "dbo.wms_supplier");
            DropForeignKey("dbo.wms_product", "brand_code", "dbo.wms_brand");
            DropTable("dbo.wms_unit_list");
            DropTable("dbo.wms_unit");
            DropTable("dbo.wms_supplier");
            DropTable("dbo.wms_brand");
            DropTable("dbo.wms_product");
        }
    }
}
