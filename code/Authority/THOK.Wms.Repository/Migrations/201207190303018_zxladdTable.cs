namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class zxladdTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_move_bill_master",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(nullable: false),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.bill_no)
                .ForeignKey("dbo.wms_bill_type", t => t.bill_type_code)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .ForeignKey("dbo.wms_employee", t => t.operate_person_id)
                .ForeignKey("dbo.wms_employee", t => t.verify_person_id)
                .Index(t => t.bill_type_code)
                .Index(t => t.warehouse_code)
                .Index(t => t.operate_person_id)
                .Index(t => t.verify_person_id);
            
            CreateTable(
                "dbo.wms_check_bill_master",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        operate_person_id = c.Guid(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        verify_person_id = c.Guid(),
                        verify_date = c.DateTime(nullable: false),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.bill_no)
                .ForeignKey("dbo.wms_bill_type", t => t.bill_type_code)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .ForeignKey("dbo.wms_employee", t => t.operate_person_id)
                .ForeignKey("dbo.wms_employee", t => t.verify_person_id)
                .Index(t => t.bill_type_code)
                .Index(t => t.warehouse_code)
                .Index(t => t.operate_person_id)
                .Index(t => t.verify_person_id);
            
            CreateTable(
                "dbo.wms_daily_balance",
                c => new
                    {
                        id = c.Guid(nullable: false),
                        settle_date = c.DateTime(nullable: false),
                        warehouse_code = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        beginning = c.Decimal(nullable: false, precision: 18, scale: 2),
                        entry_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        delivery_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        profit_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        loss_amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ending = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_warehouse", t => t.warehouse_code)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .Index(t => t.warehouse_code)
                .Index(t => t.product_code)
                .Index(t => t.unit_code);
            
            CreateTable(
                "dbo.wms_move_bill_detail",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        out_cell_code = c.String(nullable: false, maxLength: 20),
                        out_storage_code = c.String(nullable: false, maxLength: 50),
                        in_cell_code = c.String(nullable: false, maxLength: 20),
                        in_storage_code = c.String(nullable: false, maxLength: 50),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        operate_person_id = c.Guid(),
                        start_time = c.DateTime(nullable: false),
                        finish_time = c.DateTime(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_move_bill_master", t => t.bill_no)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .ForeignKey("dbo.wms_employee", t => t.operate_person_id)
                .ForeignKey("dbo.wms_cell", t => t.out_cell_code)
                .ForeignKey("dbo.wms_cell", t => t.in_cell_code)
                .ForeignKey("dbo.wms_storage", t => t.out_storage_code)
                .ForeignKey("dbo.wms_storage", t => t.in_storage_code)
                .Index(t => t.bill_no)
                .Index(t => t.product_code)
                .Index(t => t.unit_code)
                .Index(t => t.operate_person_id)
                .Index(t => t.out_cell_code)
                .Index(t => t.in_cell_code)
                .Index(t => t.out_storage_code)
                .Index(t => t.in_storage_code);
            
            CreateTable(
                "dbo.wms_check_bill_detail",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        real_product_code = c.String(nullable: false, maxLength: 20),
                        real_unit_code = c.String(nullable: false, maxLength: 20),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        operate_person_id = c.Guid(),
                        start_time = c.DateTime(nullable: false),
                        finish_time = c.DateTime(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_check_bill_master", t => t.bill_no)
                .ForeignKey("dbo.wms_cell", t => t.cell_code)
                .ForeignKey("dbo.wms_storage", t => t.storage_code)
                .ForeignKey("dbo.wms_product", t => t.product_code)
                .ForeignKey("dbo.wms_unit", t => t.unit_code)
                .ForeignKey("dbo.wms_product", t => t.real_product_code)
                .ForeignKey("dbo.wms_unit", t => t.real_unit_code)
                .ForeignKey("dbo.wms_employee", t => t.operate_person_id)
                .Index(t => t.bill_no)
                .Index(t => t.cell_code)
                .Index(t => t.storage_code)
                .Index(t => t.product_code)
                .Index(t => t.unit_code)
                .Index(t => t.real_product_code)
                .Index(t => t.real_unit_code)
                .Index(t => t.operate_person_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_check_bill_detail", new[] { "operate_person_id" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "real_unit_code" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "real_product_code" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "product_code" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "storage_code" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "cell_code" });
            DropIndex("dbo.wms_check_bill_detail", new[] { "bill_no" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "in_storage_code" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "out_storage_code" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "in_cell_code" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "out_cell_code" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "operate_person_id" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "unit_code" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "product_code" });
            DropIndex("dbo.wms_move_bill_detail", new[] { "bill_no" });
            DropIndex("dbo.wms_daily_balance", new[] { "unit_code" });
            DropIndex("dbo.wms_daily_balance", new[] { "product_code" });
            DropIndex("dbo.wms_daily_balance", new[] { "warehouse_code" });
            DropIndex("dbo.wms_check_bill_master", new[] { "verify_person_id" });
            DropIndex("dbo.wms_check_bill_master", new[] { "operate_person_id" });
            DropIndex("dbo.wms_check_bill_master", new[] { "warehouse_code" });
            DropIndex("dbo.wms_check_bill_master", new[] { "bill_type_code" });
            DropIndex("dbo.wms_move_bill_master", new[] { "verify_person_id" });
            DropIndex("dbo.wms_move_bill_master", new[] { "operate_person_id" });
            DropIndex("dbo.wms_move_bill_master", new[] { "warehouse_code" });
            DropIndex("dbo.wms_move_bill_master", new[] { "bill_type_code" });
            DropForeignKey("dbo.wms_check_bill_detail", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_check_bill_detail", "real_unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_check_bill_detail", "real_product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_check_bill_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_check_bill_detail", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_check_bill_detail", "storage_code", "dbo.wms_storage");
            DropForeignKey("dbo.wms_check_bill_detail", "cell_code", "dbo.wms_cell");
            DropForeignKey("dbo.wms_check_bill_detail", "bill_no", "dbo.wms_check_bill_master");
            DropForeignKey("dbo.wms_move_bill_detail", "in_storage_code", "dbo.wms_storage");
            DropForeignKey("dbo.wms_move_bill_detail", "out_storage_code", "dbo.wms_storage");
            DropForeignKey("dbo.wms_move_bill_detail", "in_cell_code", "dbo.wms_cell");
            DropForeignKey("dbo.wms_move_bill_detail", "out_cell_code", "dbo.wms_cell");
            DropForeignKey("dbo.wms_move_bill_detail", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_move_bill_detail", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_move_bill_detail", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_move_bill_detail", "bill_no", "dbo.wms_move_bill_master");
            DropForeignKey("dbo.wms_daily_balance", "unit_code", "dbo.wms_unit");
            DropForeignKey("dbo.wms_daily_balance", "product_code", "dbo.wms_product");
            DropForeignKey("dbo.wms_daily_balance", "warehouse_code", "dbo.wms_warehouse");
            DropForeignKey("dbo.wms_check_bill_master", "verify_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_check_bill_master", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_check_bill_master", "warehouse_code", "dbo.wms_warehouse");
            DropForeignKey("dbo.wms_check_bill_master", "bill_type_code", "dbo.wms_bill_type");
            DropForeignKey("dbo.wms_move_bill_master", "verify_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_move_bill_master", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_move_bill_master", "warehouse_code", "dbo.wms_warehouse");
            DropForeignKey("dbo.wms_move_bill_master", "bill_type_code", "dbo.wms_bill_type");
            DropTable("dbo.wms_check_bill_detail");
            DropTable("dbo.wms_move_bill_detail");
            DropTable("dbo.wms_daily_balance");
            DropTable("dbo.wms_check_bill_master");
            DropTable("dbo.wms_move_bill_master");
        }
    }
}
