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
    public class EmployeeController : Controller
    {
        [Dependency]
        public IEmployeeService EmployeeService { get; set; }

        //
        // GET: /Employee/

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
            string EmployeeCode = collection["EmployeeCode"] ?? "";
            string EmployeeName = collection["EmployeeName"] ?? "";
            string DepartmentID = collection["DepartmentID"] ?? "";
            string Status = collection["Status"] ?? "";
            string IsActive = collection["IsActive"] ?? "";
            var systems = EmployeeService.GetDetails(page, rows, EmployeeCode, EmployeeName, DepartmentID, Status, IsActive);
            return Json(systems, "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Department/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            Employee employee = new Employee();
            employee.EmployeeCode = collection["EmployeeCode"].ToString();
            employee.EmployeeName = collection["EmployeeName"].ToString();
            employee.Description = collection["Description"].ToString();
            employee.DepartmentID = new Guid(collection["DepartmentID"].ToString());
            employee.JobID = new Guid(collection["JobID"].ToString());
            employee.Sex = collection["Sex"].ToString();
            employee.Tel = collection["Tel"].ToString();
            employee.Status = collection["Status"].ToString();
            employee.IsActive = collection["IsActive"].ToString();
            employee.UpdateTime = Convert.ToDateTime(collection["UpdateTime"].ToString());

            bool bResult = EmployeeService.Add(employee);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Department/Edit/5

        public ActionResult Edit(FormCollection collection)
        {
            Employee employee = new Employee();
            employee.EmployeeCode = collection["EmployeeCode"].ToString();
            employee.EmployeeName = collection["EmployeeName"].ToString();
            employee.Description = collection["Description"].ToString();
            employee.DepartmentID = new Guid(collection["DepartmentID"].ToString());
            employee.JobID = new Guid(collection["JobID"].ToString());
            employee.Sex = collection["Sex"].ToString();
            employee.Tel = collection["Tel"].ToString();
            employee.Status = collection["Status"].ToString();
            employee.IsActive = collection["IsActive"].ToString();
            employee.UpdateTime = Convert.ToDateTime(collection["UpdateTime"].ToString());
            bool bResult = EmployeeService.Save(employee);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }


        //
        // POST: /Department/Delete/

        [HttpPost]
        public ActionResult Delete(string demployeeId)
        {
            bool bResult = EmployeeService.Delete(demployeeId);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
