namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_sorting : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.wms_sorting_line", "cell_code", c => c.String(nullable: false, maxLength: 20));
            AddForeignKey("dbo.wms_sorting_line", "cell_code", "dbo.wms_cell", "cell_code");
            CreateIndex("dbo.wms_sorting_line", "cell_code");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_sorting_line", new[] { "cell_code" });
            DropForeignKey("dbo.wms_sorting_line", "cell_code", "dbo.wms_cell");
            DropColumn("dbo.wms_sorting_line", "cell_code");
        }
    }
}
