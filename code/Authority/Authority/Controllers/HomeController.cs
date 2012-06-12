using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THOK.Authority.Authorize;
using LitJson;
using System.IO;
using System.Web.Script.Serialization;
using System.Text;
using Authority.Models;
using System.Web.Routing;
using THOK.Authority.Authority;
using THOK.Authority.Data;

namespace Authority.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult GetMenu(string systemId)
        {
            string json = @"[{'menuid':'0','iconCls':'icon-sys','menuname':'通用权限管理系统','title':'menu','children':[
                                                    {'menuid':'1','iconCls':'icon-sys','menuname':'组织结构管理',
		                                                'children':[
				                                                {'menuid':'11','menuname':'公司信息管理','iconCls':'icon-magic','url':'/Company/','enable':'true'},
				                                                {'menuid':'12','menuname':'区域信息管理','iconCls':'icon-nav','url':'/Area/'},
                                                                {'menuid':'13','menuname':'部门信息管理','iconCls':'icon-nav','url':'/Department/'},
                                                                {'menuid':'14','menuname':'员工信息管理','iconCls':'icon-nav','url':'/Employee/'}
			                                                ]
	                                                },{'menuid':'2','iconCls':'icon-sys','menuname':'系统权限管理',
		                                                'children':[
				                                                {'menuid':'21','menuname':'系统信息管理','iconCls':'icon-add','url':'/System/'},
				                                                {'menuid':'22','menuname':'模块信息管理','iconCls':'icon-users','url':'/Module/'},
				                                                {'menuid':'23','menuname':'角色信息管理','iconCls':'icon-role','url':'/Role/'},
				                                                {'menuid':'24','menuname':'用户信息管理','iconCls':'icon-set','url':'/User/'},				                                   
                                                                {'menuid':'27','menuname':'授权管理','iconCls':'icon-set','url':'/Authorize/'},
				                                                {'menuid':'28','menuname':'系统登录日志','iconCls':'icon-log','url':'/LoginLog/'}
			                                                ]
	                                                },{'menuid':'3','iconCls':'icon-sys','menuname':'多服务器应用管理',
		                                                'children':[
				                                                {'menuid':'31','menuname':'地市信息管理','iconCls':'icon-nav','url':'/City/'},
				                                                {'menuid':'32','menuname':'服务器信息管理','iconCls':'icon-nav','url':'/Server/'}
			                                                ]
                                                    },{'menuid':'4','iconCls':'icon-sys','menuname':'系统日志管理',
		                                                'children':[
				                                                {'menuid':'41','menuname':'异常日志管理','iconCls':'icon-nav','url':'/LoginLog/'},
				                                                {'menuid':'42','menuname':'业务日志管理','iconCls':'icon-nav','url':'/LoginLog/'},
                                                                {'menuid':'43','menuname':'系统日志管理','iconCls':'icon-nav','url':'/LoginLog/'}
			                                                ]
                                                    },{'menuid':'5','iconCls':'icon-sys','menuname':'帮助文档管理',
		                                                'children':[
				                                                {'menuid':'51','menuname':'文档目录管理','iconCls':'icon-nav','url':'/LoginLog/'},
				                                                {'menuid':'52','menuname':'文档维护管理','iconCls':'icon-nav','url':'/LoginLog/'},
                                                                {'menuid':'53','menuname':'帮助文档主页','iconCls':'icon-nav','url':'/Home/ChangeServer/?ip=a.wms.sw&port=8090'}
			                                                ]
                                                    }
                                                ]}]";
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            JsonResult jr = new JsonResult();
            jr.Data = serializer.Deserialize<Menu[]>(json);
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        public ActionResult GetUser()
        {
            JsonResult jr = new JsonResult();
            jr.Data = this.ControllerContext.RequestContext.HttpContext.User;
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }

        [FormsAuthorize(FunctionID = "36C63439-F20E-43D2-A8F3-063C058A1650")]
        public ActionResult ChangeServer(string cityId,string systemId,string serverId)
        {
            JsonResult jr = new JsonResult();
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            if (ModelState.IsValid)
            {
                bool bResult = true;
                jr.Data = new { success = bResult, msg = bResult ? "切换成功" : "切换失败", href = "http://localhost:5618/" };

            }
            else
                jr.Data = new { success = false, msg = "切换失败" };
            jr.ContentType = "text";
            return jr;
        }
    }
}
