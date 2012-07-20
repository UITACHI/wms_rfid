namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_data_001 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_move_bill_master", "verify_date", c => c.DateTime());
            AlterColumn("dbo.wms_move_bill_detail", "start_time", c => c.DateTime());
            AlterColumn("dbo.wms_move_bill_detail", "finish_time", c => c.DateTime());
            AlterColumn("dbo.wms_check_bill_detail", "start_time", c => c.DateTime());
            AlterColumn("dbo.wms_check_bill_detail", "finish_time", c => c.DateTime());
            AlterColumn("dbo.wms_check_bill_master", "verify_date", c => c.DateTime());
            AlterColumn("dbo.wms_profit_loss_bill_master", "verify_person_code", c => c.Guid(nullable: false));
            AlterColumn("dbo.wms_profit_loss_bill_master", "verify_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_profit_loss_bill_master", "verify_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_profit_loss_bill_master", "verify_person_code", c => c.String(maxLength: 20));
            AlterColumn("dbo.wms_check_bill_master", "verify_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_check_bill_detail", "finish_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_check_bill_detail", "start_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_move_bill_detail", "finish_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_move_bill_detail", "start_time", c => c.DateTime(nullable: false));
            AlterColumn("dbo.wms_move_bill_master", "verify_date", c => c.DateTime(nullable: false));
        }
    }
}
