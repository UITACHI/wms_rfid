using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using THOK.Authority.Bll.Interfaces.Wms;
using THOK.WebUtil;
using THOK.RfidWms.DBModel.Ef.Models.Wms;

namespace Authority.Controllers.Organization
{
    public class DepartmentController : Controller
    {
        [Dependency]
        public IDepartmentService DepartmentService { get; set; }

        //
        // GET: /Department/

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

        //
        // GET: /Department/Details/

        public ActionResult Details(int page, int rows, FormCollection collection)
        {
            string DepartmentCode = collection["DepartmentCode"] ?? "";
            string DepartmentName = collection["DepartmentName"] ?? "";
            string DepartmentLeaderID = collection["DepartmentLeaderID"] ?? "";
            string CompanyID = collection["CompanyID"] ?? "";
            var systems = DepartmentService.GetDetails(page, rows, DepartmentCode, DepartmentName, DepartmentLeaderID, CompanyID);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }

       
        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(Department department)
        {
            //Department department = new Department();
            //department.DepartmentCode = collection["DepartmentCode"].ToString();
            //department.DepartmentName = collection["DepartmentName"].ToString();
            //department.DepartmentLeaderID = new Guid(collection["DepartmentLeaderID"].ToString());
            //department.Description = collection["Description"].ToString();
            //department.CompanyID = new Guid(collection["CompanyID"].ToString());
            //department.ParentDepartmentID = new Guid(collection["ParentDepartmentID"].ToString());
            //department.UniformCode = collection["UniformCode"].ToString();
            //department.IsActive = collection["IsActive"].ToString();
            //department.UpdateTime = Convert.ToDateTime(collection["UpdateTime"].ToString());
            
            bool bResult = DepartmentService.Add(department);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        
       
        //
        // POST: /Department/Edit/

        public ActionResult Edit(Department department)
        {
            //Department department = new Department();
            //department.DepartmentCode = collection["DepartmentCode"].ToString();
            //department.DepartmentName = collection["DepartmentName"].ToString();
            //department.DepartmentLeaderID = new Guid(collection["DepartmentLeaderID"].ToString());
            //department.Description = collection["Description"].ToString();
            //department.CompanyID = new Guid(collection["CompanyID"].ToString());
            //department.ParentDepartmentID = new Guid(collection["ParentDepartmentID"].ToString());
            //department.UniformCode = collection["UniformCode"].ToString();
            //department.IsActive = collection["IsActive"].ToString();
            //department.UpdateTime = Convert.ToDateTime(collection["UpdateTime"].ToString());
            bool bResult = DepartmentService.Save(department);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
        

        //
        // POST: /Department/Delete/

        [HttpPost]
        public ActionResult Delete(string departId)
        {
            bool bResult = DepartmentService.Delete(departId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
