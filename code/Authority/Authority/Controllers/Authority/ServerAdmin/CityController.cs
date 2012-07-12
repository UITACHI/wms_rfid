using System.Web.Mvc;
using System.Web.Routing;
using System.Text;
using THOK.Authority.Bll.Interfaces.Authority;
using Microsoft.Practices.Unity;
using THOK.WebUtil;

namespace Authority.Controllers.ServerAdmin
{
    public class CityController : Controller
    {
        [Dependency]
        public ICityService CityService { get; set; }

        // GET: /City/
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

        // GET: /City/Details/
        public ActionResult Details(int page, int rows,FormCollection collection)
        {
            string cityName = collection["CityName"] ?? "";
            string description = collection["Description"] ?? "";
            string isActive = collection["IsActive"] ?? "";
            string username = collection["username"] ?? "";
            var users = CityService.GetDetails(page, rows, cityName, description,isActive);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /City/Create/
        [HttpPost]
        public ActionResult Create(string cityName, string description, bool isActive)
        {
            bool bResult = CityService.Add(cityName, description,isActive);
            string msg = bResult ? "新增成功" : "新增失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }
       
        // POST: /City/Edit/
        [HttpPost]
        public ActionResult Edit(string cityID, string cityName, string description, bool isActive)
        {
            bool bResult = CityService.Save(cityID, cityName, description, isActive);
            string msg = bResult ? "修改成功" : "修改失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // POST: /City/Delete/
        [HttpPost]
        public ActionResult Delete(string cityID)
        {
            bool bResult = CityService.Delete(cityID);
            string msg = bResult ? "删除成功" : "删除失败";
            return Json(JsonMessageHelper.getJsonMessage(bResult, msg, null), "text", JsonRequestBehavior.AllowGet);
        }

        // GET: /City/GetDetailsCity/
        public ActionResult GetDetailsCity()
        {
            string cityId = this.GetCookieValue("cityid");
            string userName = this.User.Identity.Name;
            string systemId = this.GetCookieValue("systemid");
            var users = CityService.GetDetails(userName, cityId, systemId);
            return Json(users, "text", JsonRequestBehavior.AllowGet);
        }
        
    }
}
