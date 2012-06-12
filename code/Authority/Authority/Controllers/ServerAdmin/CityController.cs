using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Authority.Authority;
using System.Web.Routing;
using System.Text;

namespace Authority.Controllers.ServerAdmin
{
    public class CityController : Controller
    {
        public ICityService _CityService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (_CityService == null) { _CityService = new CityService(); }
            base.Initialize(requestContext);
        }
        //
        // GET: /City/

        public ActionResult Index()
        {
            ViewBag.hasSearch = true;
            ViewBag.hasAdd = true;
            ViewBag.hasEdit = true;
            ViewBag.hasDelete = true;
            //ViewBag.hasPrint = true;
            //ViewBag.hasHelp = true;
            return View();
        }

        //
        // GET: /City/Details/5

        public ActionResult SearchPartial(int page, int rows)
        {
            JsonResult jr = new JsonResult();
            jr.Data = _CityService.GetDetails(page, rows);
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

       

        //
        // POST: /City/Create

        [HttpPost]
        public ActionResult Create(string cityname, bool isactive)
        {
                JsonResult jr = new JsonResult();
                jr.Data = _CityService.Add(cityname,isactive);
                jr.ContentEncoding = Encoding.UTF8;
                jr.ContentType = "text";
                jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
                return jr;
          
        }
        
        //
        // GET: /City/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /City/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /City/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /City/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
