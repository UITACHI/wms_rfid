namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_datatype_nullable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_in_bill_master", "operate_person_id", c => c.Guid(nullable: false));
            AddColumn("dbo.wms_in_bill_master", "verify_person_id", c => c.Guid());
            AddColumn("dbo.wms_in_bill_allot", "operate_person_id", c => c.Guid());
            AddColumn("dbo.wms_out_bill_master", "operate_person_id", c => c.Guid(nullable: false));
            AddColumn("dbo.wms_out_bill_allot", "operate_person_id", c => c.Guid(nullable: false));
            AlterColumn("dbo.wms_in_bill_master", "verify_date", c => c.DateTime());
            AlterColumn("dbo.wms_in_bill_allot", "start_time", c => c.DateTime());
            AlterColumn("dbo.wms_in_bill_allot", "finish_time", c => c.DateTime());
            AlterColumn("dbo.wms_out_bill_master", "verify_person_code", c => c.Guid());
            AlterColumn("dbo.wms_out_bill_master", "verify_date", c => c.DateTime());
            AlterColumn("dbo.wms_out_bill_allot", "start_time", c => c.DateTime());
            AlterColumn("dbo.wms_out_bill_allot", "finish_time", c => c.DateTime());
            DropColumn("dbo.wms_in_bill_master", "operate_person_code");
            DropColumn("dbo.wms_in_bill_master", "verify_person_code");
            DropColumn("dbo.wms_in_bill_allot", "operate_person_code");
            DropColumn("dbo.wms_out_bill_master", "operate_person_code");
            DropColumn("dbo.wms_out_bill_allot", "operate_person_code");
        }
        
        public override void Down()
        {
            AddColumn("dbo.wms_out_bill_allot", "operate_person_code", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.wms_out_bill_master", "operate_person_code", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.wms_in_bill_allot", "operate_person_code", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.wms_in_bill_master", "verify_person_code", c => c.String(maxLength: 20));
            AddColumn("dbo.wms_in_bill_master", "operate_person_code", c => c.String(nullable: false, maxLength: 20));
            AlterColumn("dbo.wms_out_bill_allot", "finish_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_out_bill_allot", "start_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_out_bill_master", "verify_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_out_bill_master", "verify_person_code", c => c.String(maxLength: 20));
            AlterColumn("dbo.wms_in_bill_allot", "finish_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_in_bill_allot", "start_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_in_bill_master", "verify_date", c => c.DateTime(nullable: false));
            DropColumn("dbo.wms_out_bill_allot", "operate_person_id");
            DropColumn("dbo.wms_out_bill_master", "operate_person_id");
            DropColumn("dbo.wms_in_bill_allot", "operate_person_id");
            DropColumn("dbo.wms_in_bill_master", "verify_person_id");
            DropColumn("dbo.wms_in_bill_master", "operate_person_id");
        }
    }
}
