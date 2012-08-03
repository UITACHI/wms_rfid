namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_out_bill_allot : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_out_bill_allot", "operate_person_id", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_out_bill_allot", "operate_person_id", c => c.Guid(nullable: false));
        }
    }
}
