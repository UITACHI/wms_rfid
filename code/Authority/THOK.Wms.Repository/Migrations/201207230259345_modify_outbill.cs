namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_outbill : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.wms_out_bill_master", "warehouse_code", "dbo.wms_warehouse", "warehouse_code");
            AddForeignKey("dbo.wms_out_bill_master", "operate_person_id", "dbo.wms_employee", "employee_id");
            AddForeignKey("dbo.wms_out_bill_master", "verify_person_code", "dbo.wms_employee", "employee_id");
            CreateIndex("dbo.wms_out_bill_master", "warehouse_code");
            CreateIndex("dbo.wms_out_bill_master", "operate_person_id");
            CreateIndex("dbo.wms_out_bill_master", "verify_person_code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_out_bill_master", new[] { "verify_person_code" });
            DropIndex("dbo.wms_out_bill_master", new[] { "operate_person_id" });
            DropIndex("dbo.wms_out_bill_master", new[] { "warehouse_code" });
            DropForeignKey("dbo.wms_out_bill_master", "verify_person_code", "dbo.wms_employee");
            DropForeignKey("dbo.wms_out_bill_master", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_out_bill_master", "warehouse_code", "dbo.wms_warehouse");
        }
    }
}
