namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_sort_table : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_sort_order_dispatch",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        order_date = c.String(nullable: false, maxLength: 14),
                        sorting_line_code = c.String(nullable: false, maxLength: 20),
                        deliver_line_code = c.String(nullable: false, maxLength: 50),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_sorting_line", t => t.sorting_line_code)
                .ForeignKey("dbo.wms_deliver_line", t => t.deliver_line_code)
                .Index(t => t.sorting_line_code)
                .Index(t => t.deliver_line_code);
            
            CreateTable(
                "dbo.wms_sorting_line",
                c => new
                    {
                        sorting_line_code = c.String(nullable: false, maxLength: 20),
                        sorting_line_name = c.String(nullable: false, maxLength: 100),
                        sorting_line_type = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.sorting_line_code);
            
            CreateTable(
                "dbo.wms_sorting_lowerlimit",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        sorting_line_code = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        quantity = c.Decimal(nullable: false, precision: 18, scale: 2),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_sorting_line", t => t.sorting_line_code)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .Index(t => t.sorting_line_code)
                .Index(t => t.product_code)
                .Index(t => t.unit_code);
            
            AlterColumn("dbo.wms_product", "buy_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_product", "trade_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_product", "retail_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_product", "cost_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_move_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_check_bill_detail", "quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_check_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_in_bill_allot", "allot_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_in_bill_allot", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_out_bill_allot", "allot_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_out_bill_allot", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "bill_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "allot_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_profit_loss_bill_detail", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_profit_loss_bill_detail", "quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "demand_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "unit_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order", "quantity_sum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_sort_order", "amount_sum", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_unit_list", "quantity01", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_unit_list", "quantity02", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_unit_list", "quantity03", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "bill_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "allot_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddForeignKey("dbo.wms_sort_order", "deliver_line_code", "dbo.wms_deliver_line", "deliver_line_code");
            CreateIndex("dbo.wms_sort_order", "deliver_line_code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_sorting_lowerlimit", new[] { "unit_code" });
            DropIndex("dbo.wms_sorting_lowerlimit", new[] { "product_code" });
            DropIndex("dbo.wms_sorting_lowerlimit", new[] { "sorting_line_code" });
            DropIndex("dbo.wms_sort_order_dispatch", new[] { "deliver_line_code" });
            DropIndex("dbo.wms_sort_order_dispatch", new[] { "sorting_line_code" });
            DropIndex("dbo.wms_sort_order", new[] { "deliver_line_code" });
            DropForeignKey("dbo.wms_sorting_lowerlimit", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_sorting_lowerlimit", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_sorting_lowerlimit", "sorting_line_code", "dbo.wms_sorting_line");
            DropForeignKey("dbo.wms_sort_order_dispatch", "deliver_line_code", "dbo.wms_deliver_line");
            DropForeignKey("dbo.wms_sort_order_dispatch", "sorting_line_code", "dbo.wms_sorting_line");
            DropForeignKey("dbo.wms_sort_order", "deliver_line_code", "dbo.wms_deliver_line");
            AlterColumn("dbo.wms_in_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "allot_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "bill_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_in_bill_detail", "price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_unit_list", "quantity03", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_unit_list", "quantity02", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_unit_list", "quantity01", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order", "amount_sum", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order", "quantity_sum", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "unit_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "amount", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_sort_order_detail", "demand_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_profit_loss_bill_detail", "quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_profit_loss_bill_detail", "price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "allot_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "bill_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_out_bill_detail", "price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_out_bill_allot", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_out_bill_allot", "allot_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_in_bill_allot", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_in_bill_allot", "allot_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_check_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_check_bill_detail", "quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_move_bill_detail", "real_quantity", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_product", "cost_price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_product", "retail_price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_product", "trade_price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            AlterColumn("dbo.wms_product", "buy_price", c => c.Decimal(nullable: false, precision: 9, scale: 2));
            DropTable("dbo.wms_sorting_lowerlimit");
            DropTable("dbo.wms_sorting_line");
            DropTable("dbo.wms_sort_order_dispatch");
        }
    }
}
