namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_department_employee : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.wms_department", "department_leader_id", c => c.Guid());
            AlterColumn("dbo.wms_employee", "department_id", c => c.Guid());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.wms_employee", "department_id", c => c.Guid(nullable: false));
            AlterColumn("dbo.wms_department", "department_leader_id", c => c.Guid(nullable: false));
        }
    }
}
