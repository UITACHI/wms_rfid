namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_sorting_out_move_type : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_sorting_line", "out_bill_type_code", c => c.String(nullable: false, maxLength: 4));
            AddColumn("dbo.wms_sorting_line", "move_bill_type_code", c => c.String(nullable: false, maxLength: 4));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_sorting_line", "move_bill_type_code");
            DropColumn("dbo.wms_sorting_line", "out_bill_type_code");
        }
    }
}
