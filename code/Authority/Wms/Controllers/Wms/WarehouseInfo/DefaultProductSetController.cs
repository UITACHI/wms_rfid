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

        //查询卷烟信息表
        // POST: /DefaultProductSet/Details/
        [HttpPost]
        public ActionResult Details(int page, int rows, string ProductName, string ProductCode, string CustomCode, string BrandCode, string UniformCode, string AbcTypeCode, string ShortCode, string PriceLevelCode, string SupplierCode)
        {
            var product = ProductService.GetDetails(page, rows, ProductName, ProductCode, CustomCode, BrandCode, UniformCode, AbcTypeCode, ShortCode, PriceLevelCode, SupplierCode);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        //加载烟卷信息表
        // POST: /DefaultProductSet/LoadProduct/
        public ActionResult LoadProduct(int page, int rows)
        {
            var product = ProductService.LoadProduct(page, rows);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        //查询卷烟信息表
        // POST: /DefaultProductSet/FindProduct/
        [HttpPost]
        public ActionResult FindProduct(string parameter)
        {
            var product = ProductService.FindProduct();
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        //添加货位预设编码DefaultProductCode
        // POST: /DefaultProductSet/CellInsertCode/
        public ActionResult CellInsertCode(string wareCodes, string areaCodes, string shelfCodes, string cellCodes, string defaultProductCode)
        {
            bool bResult = CellService.SaveCell(wareCodes, areaCodes, shelfCodes, cellCodes, defaultProductCode);
            string msg = bResult ? "保存成功" : "保存失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //编辑卷烟表
        // GET: /DefaultProductSet/Edit/
        public ActionResult Edit(Product product)
        {
            bool bResult = ProductService.Save(product);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //删除卷烟表
        // POST: /DefaultProductSet/Delete/
        [HttpPost]
        public ActionResult Delete(string productCode)
        {
            bool bResult = ProductService.Delete(productCode);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

    }
}
