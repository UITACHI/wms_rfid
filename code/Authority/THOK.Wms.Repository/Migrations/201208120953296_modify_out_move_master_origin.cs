namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_out_move_master_origin : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_move_bill_master", "origin", c => c.String(nullable: false, maxLength: 1));
            AddColumn("dbo.wms_out_bill_master", "origin", c => c.String(nullable: false, maxLength: 1));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_out_bill_master", "origin");
            DropColumn("dbo.wms_move_bill_master", "origin");
        }
    }
}
