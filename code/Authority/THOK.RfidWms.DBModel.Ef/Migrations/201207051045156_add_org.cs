namespace THOK.RfidWms.DBModel.Ef.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_org : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.wms_department",
                c => new
                    {
                        department_id = c.Guid(nullable: false),
                        department_code = c.String(nullable: false, maxLength: 20),
                        department_name = c.String(nullable: false, maxLength: 100),
                        department_leader_id = c.Guid(nullable: false),
                        description = c.String(),
                        company_id = c.Guid(nullable: false),
                        parent_department_id = c.Guid(nullable: false),
                        uniform_code = c.String(maxLength: 20),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.department_id)
                .ForeignKey("dbo.wms_employee", t => t.department_leader_id)
                .ForeignKey("dbo.wms_company", t => t.company_id)
                .ForeignKey("dbo.wms_department", t => t.parent_department_id)
                .Index(t => t.department_leader_id)
                .Index(t => t.company_id)
                .Index(t => t.parent_department_id);
            
            CreateTable(
                "dbo.wms_employee",
                c => new
                    {
                        employee_id = c.Guid(nullable: false),
                        employee_code = c.String(nullable: false, maxLength: 20),
                        employee_name = c.String(nullable: false, maxLength: 100),
                        description = c.String(),
                        department_id = c.Guid(nullable: false),
                        job_id = c.Guid(nullable: false),
                        sex = c.String(nullable: false, maxLength: 2, fixedLength: true),
                        tel = c.String(maxLength: 50),
                        Status = c.String(nullable: false, maxLength: 4, fixedLength: true),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.employee_id)
                .ForeignKey("dbo.wms_department", t => t.department_id)
                .ForeignKey("dbo.wms_job", t => t.job_id)
                .Index(t => t.department_id)
                .Index(t => t.job_id);
            
            CreateTable(
                "dbo.wms_job",
                c => new
                    {
                        job_id = c.Guid(nullable: false),
                        job_code = c.String(nullable: false, maxLength: 20),
                        job_name = c.String(nullable: false, maxLength: 20),
                        description = c.String(),
                        is_active = c.String(nullable: false, maxLength: 1, fixedLength: true),
                        update_time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.job_id);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_employee", new[] { "job_id" });
            DropIndex("dbo.wms_employee", new[] { "department_id" });
            DropIndex("dbo.wms_department", new[] { "parent_department_id" });
            DropIndex("dbo.wms_department", new[] { "company_id" });
            DropIndex("dbo.wms_department", new[] { "department_leader_id" });
            DropForeignKey("dbo.wms_employee", "job_id", "dbo.wms_job");
            DropForeignKey("dbo.wms_employee", "department_id", "dbo.wms_department");
            DropForeignKey("dbo.wms_department", "parent_department_id", "dbo.wms_department");
            DropForeignKey("dbo.wms_department", "company_id", "dbo.wms_company");
            DropForeignKey("dbo.wms_department", "department_leader_id", "dbo.wms_employee");
            DropTable("dbo.wms_job");
            DropTable("dbo.wms_employee");
            DropTable("dbo.wms_department");
        }
    }
}
