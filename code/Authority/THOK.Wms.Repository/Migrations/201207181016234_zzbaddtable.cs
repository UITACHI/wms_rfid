namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zzbaddtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_profit_loss_bill_detail",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_profit_loss_bill_master", t => t.bill_no)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .Index(t => t.bill_no)
                .Index(t => t.product_code)
                .Index(t => t.unit_code);
            
            CreateTable(
                "dbo.wms_profit_loss_bill_master",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        check_bill_no = c.String(maxLength: 20),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_code = c.String(nullable: false, maxLength: 20),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_code = c.String(maxLength: 20),
                        verify_date = c.DateTime(nullable: false),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.bill_no)
                .ForeignKey("dbo.wms_bill_type", t => t.bill_type_code)
                .Index(t => t.bill_type_code);
            
            CreateTable(
                "dbo.wms_sort_order_detail",
                c => new
                    {
                        order_detail_id = c.String(nullable: false, maxLength: 20),
                        order_id = c.String(nullable: false, maxLength: 12),
                        product_code = c.String(nullable: false, maxLength: 20),
                        product_name = c.String(nullable: false, maxLength: 40, fixedLength: true),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        unit_name = c.String(nullable: false, maxLength: 20),
                        demand_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        amount = c.Decimal(nullable: false, precision: 9, scale: 2),
                        unit_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                    })
                .PrimaryKey(t => t.order_detail_id)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_sort_order", t => t.order_id)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .Index(t => t.product_code)
                .Index(t => t.order_id)
                .Index(t => t.unit_code);
            
            CreateTable(
                "dbo.wms_sort_order",
                c => new
                    {
                        order_id = c.String(nullable: false, maxLength: 12),
                        company_code = c.String(maxLength: 20),
                        sale_region_code = c.String(nullable: false, maxLength: 50),
                        order_date = c.String(nullable: false, maxLength: 14, fixedLength: true),
                        order_type = c.String(nullable: false, maxLength: 1),
                        customer_code = c.String(nullable: false, maxLength: 10),
                        customer_name = c.String(nullable: false, maxLength: 100),
                        quantity_sum = c.Decimal(nullable: false, precision: 9, scale: 2),
                        amount_sum = c.Decimal(nullable: false, precision: 9, scale: 2),
                        detail_num = c.Decimal(nullable: false, precision: 18, scale: 2),
                        deliver_order = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeliverDate = c.String(nullable: false, maxLength: 14, fixedLength: true),
                        description = c.String(nullable: false, maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.order_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_sort_order_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_sort_order_detail", new[] { "order_id" });
            DropIndex("dbo.wms_sort_order_detail", new[] { "product_code" });
            DropIndex("dbo.wms_profit_loss_bill_master", new[] { "bill_type_code" });
            DropIndex("dbo.wms_profit_loss_bill_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_profit_loss_bill_detail", new[] { "product_code" });
            DropIndex("dbo.wms_profit_loss_bill_detail", new[] { "bill_no" });
            DropForeignKey("dbo.wms_sort_order_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_sort_order_detail", "order_id", "dbo.wms_sort_order");
            DropForeignKey("dbo.wms_sort_order_detail", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_profit_loss_bill_master", "bill_type_code", "dbo.wms_bill_type");
            DropForeignKey("dbo.wms_profit_loss_bill_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_profit_loss_bill_detail", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_profit_loss_bill_detail", "bill_no", "dbo.wms_profit_loss_bill_master");
            DropTable("dbo.wms_sort_order");
            DropTable("dbo.wms_sort_order_detail");
            DropTable("dbo.wms_profit_loss_bill_master");
            DropTable("dbo.wms_profit_loss_bill_detail");
        }
    }
}
