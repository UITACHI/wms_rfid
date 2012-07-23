namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_inbill : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.wms_in_bill_master", "warehouse_code", "dbo.wms_warehouse", "warehouse_code");
            AddForeignKey("dbo.wms_in_bill_master", "operate_person_id", "dbo.wms_employee", "employee_id");
            AddForeignKey("dbo.wms_in_bill_master", "verify_person_id", "dbo.wms_employee", "employee_id");
            CreateIndex("dbo.wms_in_bill_master", "warehouse_code");
            CreateIndex("dbo.wms_in_bill_master", "operate_person_id");
            CreateIndex("dbo.wms_in_bill_master", "verify_person_id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_in_bill_master", new[] { "verify_person_id" });
            DropIndex("dbo.wms_in_bill_master", new[] { "operate_person_id" });
            DropIndex("dbo.wms_in_bill_master", new[] { "warehouse_code" });
            DropForeignKey("dbo.wms_in_bill_master", "verify_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_in_bill_master", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_in_bill_master", "warehouse_code", "dbo.wms_warehouse");
        }
    }
}
