using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Wms.Bll.Interfaces;
using THOK.Wms.DbModel;

namespace Wms.Controllers.Wms.WarehouseInfo
{
    public class DefaultProductSetController : Controller
    {
        [Dependency]
        public IProductService ProductService { get; set; }
        [Dependency]
        public ICellService CellService { get; set; }

        //
        // GET: /DefaultProductSet/

        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        //加载烟卷信息表
        // POST: /DefaultProductSet/LoadProduct/
        public ActionResult LoadProduct(int page, int rows)
        {
            var product = ProductService.LoadProduct(page, rows);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        //首页加载卷烟信息
        //POST: /DefaultProductSet/GetProductCell/
        public ActionResult GetProductCell()
        {
            var product = CellService.GetCellInfo();
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        //查找卷烟信息
        public ActionResult SearchProductCell(string productCode)
        {
            var product = CellService.GetCellInfo(productCode);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        //获得货位的ID
        public ActionResult GetCellCode(string productCode)
        {
            var product = CellService.GetCellCode(productCode);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        //添加货位预设编码
        // POST: /DefaultProductSet/CellInsertCode/
        public ActionResult CellInsertCode(string wareCodes, string areaCodes, string shelfCodes, string cellCodes, string defaultProductCode)
        {
            bool bResult = CellService.SaveCell(wareCodes, areaCodes, shelfCodes, cellCodes, defaultProductCode);
            string msg = bResult ? "保存成功" : "保存失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
