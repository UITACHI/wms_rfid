namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_outBillAllot_UnitCode : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.wms_out_bill_allot", name: "UnitCode", newName: "unit_code");
        }
        
        public override void Down()
        {
            RenameColumn(table: "dbo.wms_out_bill_allot", name: "unit_code", newName: "UnitCode");
        }
    }
}
