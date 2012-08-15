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

        //根据条件查找卷烟信息
        //POST: /DefaultProductSet/GetProductBy/
        public ActionResult GetProductBy(int page, int rows, string QueryString, string Value)
        {
            if (QueryString == null)
            {
                QueryString = "ProductCode";
            }
            if (Value == null)
            {
                Value = "";
            }
            var product = ProductService.GetProductBy(page, rows, QueryString, Value);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCellBy(int page ,int rows,string QueryString ,string Value)
        {
            if (QueryString == null)
            {
                QueryString = "ProductCode";
            }
            if (Value == null)
            {
                Value = "";
            }
            var cell = CellService.GetCellBy(page, rows, QueryString, Value);
            return Json(cell, "text", JsonRequestBehavior.AllowGet);
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
        //POST: /DefaultProductSet/SearchProductCell/
        public ActionResult SearchProductCell(string productCode)
        {
            var product = CellService.GetCellInfo(productCode);
            return Json(product, "text", JsonRequestBehavior.AllowGet);
        }
        
        //添加货位预设编码
        // POST: /DefaultProductSet/CellInsertCode/
        public ActionResult CellInsertCode(string wareCodes, string areaCodes, string shelfCodes, string cellCodes, string defaultProductCode, string editType)
        {
            bool bResult = CellService.SaveCell(wareCodes, areaCodes, shelfCodes, cellCodes, defaultProductCode, editType);
            string msg = bResult ? "保存成功" : "保存失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //获得货位勾选状态
        // GET: /DefaultProductSet/CellCodeSet/
        public ActionResult CellCodeSet(string productId)
        {
            var wareCell = CellService.GetCellCheck(productId);
            return Json(wareCell, "text", JsonRequestBehavior.AllowGet);
        }

        //删除货位信息
        //POST: /DefaultProductSet/CellDel/
        public ActionResult CellDel(string productCodes)
        {
            bool bResult = CellService.DeleteCell(productCodes);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        //POST: /DefaultProductSet/SetTree2/
        public ActionResult SetTree2(string strId, string proCode)
        {
            bool bResult = CellService.SetTree2(strId, proCode);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
