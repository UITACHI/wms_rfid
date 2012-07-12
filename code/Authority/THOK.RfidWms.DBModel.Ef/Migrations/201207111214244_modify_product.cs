namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_product : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.wms_product", name: "short_code)", newName: "short_code");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.wms_product", name: "short_code", newName: "short_code)");
        }
    }
}
