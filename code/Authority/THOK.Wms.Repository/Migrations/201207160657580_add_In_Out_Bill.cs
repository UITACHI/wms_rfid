namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_In_Out_Bill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_bill_type",
                c => new
                    {
                        bill_type_code = c.String(nullable: false, maxLength: 4),
                        bill_type_name = c.String(nullable: false, maxLength: 20),
                        bill_class = c.String(nullable: false, maxLength: 4, fixedLength: true),
                        description = c.String(maxLength: 100),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.bill_type_code);
            
            CreateTable(
                "dbo.wms_in_bill_master",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
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
                "dbo.wms_in_bill_detail",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        bill_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        allot_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_in_bill_master", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_in_bill_allot",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        UnitCode = c.String(nullable: false, maxLength: 20),
                        allot_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        operate_person_code = c.String(nullable: false, maxLength: 20),
                        start_time = c.DateTime(nullable: false),
                        finish_time = c.DateTime(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_in_bill_master", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_out_bill_master",
                c => new
                    {
                        bill_no = c.String(nullable: false, maxLength: 20),
                        bill_date = c.DateTime(nullable: false),
                        bill_type_code = c.String(nullable: false, maxLength: 4),
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
                "dbo.wms_out_bill_detail",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        product_code = c.String(nullable: false, maxLength: 20),
                        unit_code = c.String(nullable: false, maxLength: 20),
                        price = c.Decimal(nullable: false, precision: 9, scale: 2),
                        bill_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        allot_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        description = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_out_bill_master", t => t.bill_no)
                .Index(t => t.bill_no);
            
            CreateTable(
                "dbo.wms_out_bill_allot",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        bill_no = c.String(nullable: false, maxLength: 20),
                        out_pallet_tag = c.Int(name: "out_pallet_tag)", nullable: false),
                        product_code = c.String(nullable: false, maxLength: 20),
                        cell_code = c.String(nullable: false, maxLength: 20),
                        storage_code = c.String(nullable: false, maxLength: 50),
                        UnitCode = c.String(nullable: false, maxLength: 20),
                        allot_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        real_quantity = c.Decimal(nullable: false, precision: 9, scale: 2),
                        operate_person_code = c.String(nullable: false, maxLength: 20),
                        start_time = c.DateTime(nullable: false),
                        finish_time = c.DateTime(nullable: false),
                        status = c.String(nullable: false, maxLength: 1, fixedLength: true),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.wms_out_bill_master", t => t.bill_no)
                .Index(t => t.bill_no);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_allot", new[] { "bill_no" });
            DropIndex("dbo.wms_out_bill_detail", new[] { "bill_no" });
            DropIndex("dbo.wms_out_bill_master", new[] { "bill_type_code" });
            DropIndex("dbo.wms_in_bill_allot", new[] { "bill_no" });
            DropIndex("dbo.wms_in_bill_detail", new[] { "bill_no" });
            DropIndex("dbo.wms_in_bill_master", new[] { "bill_type_code" });
            DropForeignKey("dbo.wms_out_bill_allot", "bill_no", "dbo.wms_out_bill_master");
            DropForeignKey("dbo.wms_out_bill_detail", "bill_no", "dbo.wms_out_bill_master");
            DropForeignKey("dbo.wms_out_bill_master", "bill_type_code", "dbo.wms_bill_type");
            DropForeignKey("dbo.wms_in_bill_allot", "bill_no", "dbo.wms_in_bill_master");
            DropForeignKey("dbo.wms_in_bill_detail", "bill_no", "dbo.wms_in_bill_master");
            DropForeignKey("dbo.wms_in_bill_master", "bill_type_code", "dbo.wms_bill_type");
            DropTable("dbo.wms_out_bill_allot");
            DropTable("dbo.wms_out_bill_detail");
            DropTable("dbo.wms_out_bill_master");
            DropTable("dbo.wms_in_bill_allot");
            DropTable("dbo.wms_in_bill_detail");
            DropTable("dbo.wms_in_bill_master");
            DropTable("dbo.wms_bill_type");
        }
    }
}
