namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_cell_max : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_cell", "max_pallet_quantity", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_cell", "max_pallet_quantity");
        }
    }
}
