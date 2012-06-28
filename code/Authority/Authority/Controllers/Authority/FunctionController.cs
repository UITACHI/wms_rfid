using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Authority.Bll.Interfaces.Authority;
using Microsoft.Practices.Unity;
using THOK.WebUtil;

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
        public ActionResult Edit(string FunctionId, string FunctionName, string ControlName, string IndicateImage)
        {
            bool bResult = FunctionService.Save(FunctionId, FunctionName, ControlName, IndicateImage);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Delete/
        [HttpPost]
        public ActionResult Delete(string FunctionId)
        {
            bool bResult = FunctionService.Delete(FunctionId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /Module/Create/
        [HttpPost]
        public ActionResult Create(string ModuleId, string FunctionName, string ControlName, string IndicateImage)
        {
            bool bResult = FunctionService.Add(ModuleId, FunctionName, ControlName, IndicateImage);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
