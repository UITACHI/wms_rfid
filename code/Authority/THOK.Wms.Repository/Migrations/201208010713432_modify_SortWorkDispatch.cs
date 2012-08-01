namespace THOK.Wms.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class modify_SortWorkDispatch : DbMigration
    {
        public override void Up()
        {
            AddForeignKey("dbo.wms_sort_work_dispatch", "sorting_line_code", "dbo.wms_sorting_line", "sorting_line_code");
            AddForeignKey("dbo.wms_sort_work_dispatch", "out_bill_no", "dbo.wms_out_bill_master", "bill_no");
            AddForeignKey("dbo.wms_sort_work_dispatch", "move_bill_no", "dbo.wms_move_bill_master", "bill_no");
            CreateIndex("dbo.wms_sort_work_dispatch", "sorting_line_code");
            CreateIndex("dbo.wms_sort_work_dispatch", "out_bill_no");
            CreateIndex("dbo.wms_sort_work_dispatch", "move_bill_no");
        }
        
        public override void Down()
        {
            DropIndex("dbo.wms_sort_work_dispatch", new[] { "move_bill_no" });
            DropIndex("dbo.wms_sort_work_dispatch", new[] { "out_bill_no" });
            DropIndex("dbo.wms_sort_work_dispatch", new[] { "sorting_line_code" });
            DropForeignKey("dbo.wms_sort_work_dispatch", "move_bill_no", "dbo.wms_move_bill_master");
            DropForeignKey("dbo.wms_sort_work_dispatch", "out_bill_no", "dbo.wms_out_bill_master");
            DropForeignKey("dbo.wms_sort_work_dispatch", "sorting_line_code", "dbo.wms_sorting_line");
        }
    }
}
