namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editWareField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_cell", "col", c => c.Int(nullable: false));
            AddColumn("dbo.wms_cell", "img_x", c => c.Int(nullable: false));
            AddColumn("dbo.wms_cell", "img_y", c => c.Int(nullable: false));
            AddColumn("dbo.wms_area", "allot_in_order", c => c.Int(nullable: false));
            AddColumn("dbo.wms_area", "allot_out_order", c => c.Int(nullable: false));
            AddColumn("dbo.wms_shelf", "cell_rows", c => c.Int(nullable: false));
            AddColumn("dbo.wms_shelf", "cell_cols", c => c.Int(nullable: false));
            AddColumn("dbo.wms_shelf", "img_x", c => c.Int(nullable: false));
            AddColumn("dbo.wms_shelf", "img_y", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_shelf", "img_y");
            DropColumn("dbo.wms_shelf", "img_x");
            DropColumn("dbo.wms_shelf", "cell_cols");
            DropColumn("dbo.wms_shelf", "cell_rows");
            DropColumn("dbo.wms_area", "allot_out_order");
            DropColumn("dbo.wms_area", "allot_in_order");
            DropColumn("dbo.wms_cell", "img_y");
            DropColumn("dbo.wms_cell", "img_x");
            DropColumn("dbo.wms_cell", "col");
        }
    }
}
