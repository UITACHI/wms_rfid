namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_outbill_001 : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.wms_out_bill_master", name: "verify_person_code", newName: "verify_person_id");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.wms_out_bill_master", name: "verify_person_id", newName: "verify_person_code");
        }
    }
}
