namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_cell_storage_row_version : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_cell", "lock_tag", c => c.String());
            AddColumn("dbo.wms_cell", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.wms_storage", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_storage", "row_version");
            DropColumn("dbo.wms_cell", "row_version");
            DropColumn("dbo.wms_cell", "lock_tag");
        }
    }
}
