namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_employee_username : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_employee", "user_name", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.wms_employee", "user_name");
        }
    }
}
