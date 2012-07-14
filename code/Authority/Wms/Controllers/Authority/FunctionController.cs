using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Wms.Bll.Interfaces;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.Bll.Interfaces;

namespace Authority.Controllers.Authority
{
    public class FunctionController : Controller
    {
        [Dependency]
        public IFunctionService FunctionService { get; set; }

        //
        // GET: /Function/

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Module/Details/
        [HttpPost]
        public ActionResult Details(string ModuleId)
        {
            var functions = FunctionService.GetDetails(ModuleId);
            return Json(functions, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Edit/
        [HttpPost]
        public ActionResult Edit(string id, string FunctionName, string ControlName, string IndicateImage)
        {
            bool bResult = FunctionService.Save(id, FunctionName, ControlName, IndicateImage);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Delete/
        [HttpPost]
        public ActionResult Delete(string id, string FunctionName, string ControlName, string IndicateImage)
        {
            bool bResult = FunctionService.Delete(id);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Create/
        [HttpPost]
        public ActionResult Create(string id, string FunctionName, string ControlName, string IndicateImage)
        {
            bool bResult = FunctionService.Add(id, FunctionName, ControlName, IndicateImage);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
