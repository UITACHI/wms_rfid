using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace THOK.WebUtil
{
    public static class JsonHelper
    {
        public static JsonResult getJson(object o, string contentType)
        {
            JsonResult jr = new JsonResult();
            jr.ContentEncoding = Encoding.UTF8;
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            jr.Data = o;
            jr.ContentType = contentType;
            return jr;
        }

        public static JsonResult getJson(object o)
        {
            return getJson(o, "text");
        }

        public static JsonResult getJsonMessage(bool success,string msg,object data)
        {
            var o = new { success = success, msg = msg, data = data }; 
            return getJson(o, "text");
        }

        public static string getJsonMenu()
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
            return json;
        }
    }
}
