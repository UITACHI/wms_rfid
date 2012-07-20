namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_unit_rowVer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_unit", "row_version", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_unit", "row_version");
        }
    }
}
