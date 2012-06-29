using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using THOK.Authority.Bll.Interfaces.Authority;
using Microsoft.Practices.Unity;
using THOK.WebUtil;
using THOK.Authority.Dal.EntityModels;

namespace Authority.Controllers.Authority
{
    public class UserController : Controller
    {
        [Dependency]
        public IUserService UserService { get; set; }

        // GET: /User/
        public ActionResult Index(string moduleID)
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            ViewBag.hasPrint = true;
            ViewBag.hasHelp = true;
            ViewBag.hasPermissionAdmin = true;
            ViewBag.hasRoleAdmin = true;
            //ViewBag.hasAuthorize = true;
            ViewBag.ModuleID = moduleID;
            return View();
        }

        // GET: /User/Details/
        public ActionResult Details(int page, int rows,FormCollection collection)
        {
            string userName = collection["UserName"]??"";
            string chineseName= collection["ChineseName"]??"";
            string isLock= collection["IsLock"]??"";
            string isAdmin= collection["IsAdmin"]??"";
            string memo= collection["Memo"]??"";
            var users= UserService.GetDetails(page, rows, userName, chineseName, isLock, isAdmin, memo);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /User/GetUserRole/
        [HttpPost]
        public ActionResult GetUserRole(string UserID)
        {
            var users = UserService.GetUserRole(UserID);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /User/GetRoleInfo/
        [HttpPost]
        public ActionResult GetRoleInfo(string UserID)
        {
            var users = UserService.GetRoleInfo(UserID);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }


        // POST: /User/Create/
        [HttpPost]
        public ActionResult Create(string userName, string pwd, string ChineseName, bool isLock, bool isAdmin, string loginPc, string memo)
        {
            pwd = string.IsNullOrEmpty(pwd) || string.IsNullOrEmpty(pwd.Trim())? "123456" : pwd;
            bool bResult = UserService.Add(userName, pwd, ChineseName, isLock, isAdmin, memo);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /User/AddUserRole/
        [HttpPost]
        public ActionResult AddUserRole(string userId, string roleIDstr)
        {
            bool bResult = UserService.AddUserRole(userId,roleIDstr);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);           
        }

        // POST: /User/Edit/
        [HttpPost]
        public ActionResult Edit(string userID, string userName, string pwd, string chineseName, bool isLock, bool isAdmin, string memo)
        {
            bool bResult = UserService.Save(userID, userName, pwd, chineseName, isLock, isAdmin, memo);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /User/Delete/
        [HttpPost]
        public ActionResult Delete(string userID)
        {
            bool bResult = UserService.Delete(userID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /User/DeleteUserRole/
        [HttpPost]
        public ActionResult DeleteUserRole(string userRoleIdStr)
        {
            bool bResult = UserService.DeleteUserRole(userRoleIdStr);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
    }
}
