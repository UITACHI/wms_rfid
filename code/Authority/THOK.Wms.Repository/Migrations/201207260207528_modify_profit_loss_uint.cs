namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_profit_loss_uint : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.wms_profit_loss_bill_master", name: "verify_person_code", newName: "verify_person_id");
            AddColumn("dbo.wms_profit_loss_bill_master", "operate_person_id", c => c.Guid(nullable: false));
            AlterColumn("dbo.wms_unit_list", "unit_code01", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.wms_unit_list", "unit_code02", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.wms_unit_list", "unit_code03", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.wms_unit_list", "unit_code04", c => c.String(nullable: false, maxLength: 20));
            AddForeignKey("dbo.wms_profit_loss_bill_master", "warehouse_code", "dbo.wms_warehouse", "warehouse_code");
            AddForeignKey("dbo.wms_profit_loss_bill_master", "operate_person_id", "dbo.wms_employee", "employee_id");
            AddForeignKey("dbo.wms_profit_loss_bill_master", "verify_person_id", "dbo.wms_employee", "employee_id");
            AddForeignKey("dbo.wms_unit_list", "unit_code01", "dbo.wms_unit", "unit_code");
            AddForeignKey("dbo.wms_unit_list", "unit_code02", "dbo.wms_unit", "unit_code");
            AddForeignKey("dbo.wms_unit_list", "unit_code03", "dbo.wms_unit", "unit_code");
            AddForeignKey("dbo.wms_unit_list", "unit_code04", "dbo.wms_unit", "unit_code");
            CreateIndex("dbo.wms_profit_loss_bill_master", "warehouse_code");
            CreateIndex("dbo.wms_profit_loss_bill_master", "operate_person_id");
            CreateIndex("dbo.wms_profit_loss_bill_master", "verify_person_id");
            CreateIndex("dbo.wms_unit_list", "unit_code01");
            CreateIndex("dbo.wms_unit_list", "unit_code02");
            CreateIndex("dbo.wms_unit_list", "unit_code03");
            CreateIndex("dbo.wms_unit_list", "unit_code04");
            DropColumn("dbo.wms_profit_loss_bill_master", "operate_person_code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wms_profit_loss_bill_master", "operate_person_code", c => c.String(nullable: false, maxLength: 20));
            DropIndex("dbo.wms_unit_list", new[] { "unit_code04" });
            DropIndex("dbo.wms_unit_list", new[] { "unit_code03" });
            DropIndex("dbo.wms_unit_list", new[] { "unit_code02" });
            DropIndex("dbo.wms_unit_list", new[] { "unit_code01" });
            DropIndex("dbo.wms_profit_loss_bill_master", new[] { "verify_person_id" });
            DropIndex("dbo.wms_profit_loss_bill_master", new[] { "operate_person_id" });
            DropIndex("dbo.wms_profit_loss_bill_master", new[] { "warehouse_code" });
            DropForeignKey("dbo.wms_unit_list", "unit_code04", "dbo.wms_unit");
            DropForeignKey("dbo.wms_unit_list", "unit_code03", "dbo.wms_unit");
            DropForeignKey("dbo.wms_unit_list", "unit_code02", "dbo.wms_unit");
            DropForeignKey("dbo.wms_unit_list", "unit_code01", "dbo.wms_unit");
            DropForeignKey("dbo.wms_profit_loss_bill_master", "verify_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_profit_loss_bill_master", "operate_person_id", "dbo.wms_employee");
            DropForeignKey("dbo.wms_profit_loss_bill_master", "warehouse_code", "dbo.wms_warehouse");
            AlterColumn("dbo.wms_unit_list", "unit_code04", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.wms_unit_list", "unit_code03", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.wms_unit_list", "unit_code02", c => c.String(nullable: false, maxLength: 8));
            AlterColumn("dbo.wms_unit_list", "unit_code01", c => c.String(nullable: false, maxLength: 8));
            DropColumn("dbo.wms_profit_loss_bill_master", "operate_person_id");
            RenameColumn(table: "dbo.wms_profit_loss_bill_master", name: "verify_person_id", newName: "verify_person_code");
        }
    }
}
