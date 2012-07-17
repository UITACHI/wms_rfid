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
				                                                {'menuid':'12','menuname':'岗位信息管理','iconCls':'icon-nav','url':'/Job/'},
                                                                {'menuid':'13','menuname':'部门信息管理','iconCls':'icon-nav','url':'/Department/'},
                                                                {'menuid':'14','menuname':'员工信息管理','iconCls':'icon-nav','url':'/Employee/'}
			                                                ]
	                                                },{'menuid':'2','iconCls':'icon-sys','menuname':'系统权限管理',
		                                                'children':[
				                                                {'menuid':'21','menuname':'系统信息管理','iconCls':'icon-add','url':'/System/'},
				                                                {'menuid':'22','menuname':'模块信息管理','iconCls':'icon-users','url':'/Module/'},
				                                                {'menuid':'23','menuname':'角色信息管理','iconCls':'icon-role','url':'/Role/'},
				                                                {'menuid':'24','menuname':'用户信息管理','iconCls':'icon-set','url':'/User/'},				                                   
				                                                {'menuid':'28','menuname':'系统登录日志','iconCls':'icon-log','url':'/SystemEventLog/'}
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
                                                    },{'menuid':'7','iconCls':'icon-sys','menuname':'产品信息管理',
		                                                'children':[
				                                                {'menuid':'71','menuname':'产品信息管理','iconCls':'icon-nav','url':'/Product/Index'},
				                                                {'menuid':'72','menuname':'厂商信息管理','iconCls':'icon-nav','url':'/Supplier/Index'},
                                                                {'menuid':'73','menuname':'产品品牌管理','iconCls':'icon-nav','url':'/Brand/'},
                                                                {'menuid':'73','menuname':'计量单位管理','iconCls':'icon-nav','url':'/Unit/'},
                                                                {'menuid':'73','menuname':'单位系列管理','iconCls':'icon-nav','url':'/UnitList/'}
			                                                ]
                                                    },{'menuid':'6','iconCls':'icon-sys','menuname':'仓库信息管理',
		                                                'children':[
				                                                {'menuid':'61','menuname':'仓库设置','iconCls':'icon-magic','url':'/Warehouse/'}				                                             
			                                                ]
	                                                },{'menuid':'8','iconCls':'icon-sys','menuname':'入库管理',
		                                                'children':[
				                                                {'menuid':'81','menuname':'入库单据类型设置','iconCls':'icon-add','url':'/StockInBillType/Index'},
				                                                {'menuid':'82','menuname':'入库单','iconCls':'icon-users','url':'/StockInBill/Index'},
				                                                {'menuid':'83','menuname':'入库单审核','iconCls':'icon-role','url':'/StockInBillCheck/Index'},
				                                                {'menuid':'84','menuname':'入库单分配','iconCls':'icon-set','url':'/StockInBillAllot/Index'},
				                                                {'menuid':'85','menuname':'入库单分配确认','iconCls':'icon-log','url':'/StockInBillAllotConfirm/Index'}
			                                                ]
	                                                },{'menuid':'9','iconCls':'icon-sys','menuname':'出库管理',
		                                                'children':[
				                                                {'menuid':'91','menuname':'出库单据类型设置','iconCls':'icon-add','url':'/StockOutBillType/Index'},
				                                                {'menuid':'92','menuname':'出库单','iconCls':'icon-users','url':'/StockOutBill/Index'},
				                                                {'menuid':'93','menuname':'出库单审核','iconCls':'icon-role','url':'/StockOutBillCheck/Index'},
				                                                {'menuid':'94','menuname':'出库单分配','iconCls':'icon-set','url':'/StockOutBillAllot/Index'},
				                                                {'menuid':'95','menuname':'出库单分配确认','iconCls':'icon-log','url':'/StockOutBillAllotConfirm/Index'}
			                                                ]
	                                                },{'menuid':'10','iconCls':'icon-sys','menuname':'移库管理',
		                                                'children':[
				                                                {'menuid':'101','menuname':'移库单据类型设置','iconCls':'icon-add','url':'/StockMoveBillType/Index'},
				                                                {'menuid':'102','menuname':'移库单生成','iconCls':'icon-users','url':'/StockMoveBillGenerate/Index'},
				                                                {'menuid':'103','menuname':'移库单','iconCls':'icon-role','url':'/StockMoveBill/Index'},
				                                                {'menuid':'104','menuname':'移库单审核','iconCls':'icon-set','url':'/StockMoveBillCheck/Index'}
			                                                ]
	                                                },{'menuid':'11','iconCls':'icon-sys','menuname':'分拣管理',
		                                                'children':[
				                                                {'menuid':'111','menuname':'分拣线管理','iconCls':'icon-add','url':'/SortingLine/'},
				                                                {'menuid':'112','menuname':'分拣线下限设置','iconCls':'icon-users','url':'/SortingLowerLimit/'},
				                                                {'menuid':'113','menuname':'分拣订单管理','iconCls':'icon-role','url':'/SortingOrder/'},
				                                                {'menuid':'114','menuname':'分拣订单优化','iconCls':'icon-set','url':'/SortingOrderOptimize/'}
			                                                ]
	                                                },{'menuid':'12','iconCls':'icon-sys','menuname':'盘点管理',
		                                                'children':[
				                                                {'menuid':'121','menuname':'盘点单据类型设置','iconCls':'icon-add','url':'/CheckBillType/'},
				                                                {'menuid':'122','menuname':'盘点单生成','iconCls':'icon-users','url':'/CheckBillGenerate/'},
				                                                {'menuid':'123','menuname':'盘点单','iconCls':'icon-role','url':'/CheckBill/'},
				                                                {'menuid':'124','menuname':'盘点单审核','iconCls':'icon-set','url':'/CheckBillVerify/'},				                                   
				                                                {'menuid':'125','menuname':'盘点损益确认','iconCls':'icon-log','url':'/CheckBillConfirm/'}
			                                                ]
	                                                },{'menuid':'13','iconCls':'icon-sys','menuname':'损益管理',
		                                                'children':[
				                                                {'menuid':'131','menuname':'损益单据类型设置','iconCls':'icon-add','url':'/ProfitLossBillType/'},
				                                                {'menuid':'132','menuname':'损益单','iconCls':'icon-users','url':'/ProfitLossBill/'},
				                                                {'menuid':'133','menuname':'损益单审核','iconCls':'icon-role','url':'/ProfitLossBillVerify/'}
			                                                ]
	                                                },{'menuid':'14','iconCls':'icon-sys','menuname':'预警管理',
		                                                'children':[
				                                                {'menuid':'141','menuname':'库存安全预警设置','iconCls':'icon-add','url':'/System/'},
				                                                {'menuid':'142','menuname':'库存安全提醒','iconCls':'icon-users','url':'/Module/'}
			                                                ]
	                                                },{'menuid':'15','iconCls':'icon-sys','menuname':'库存管理',
		                                                'children':[
				                                                {'menuid':'151','menuname':'库存总账','iconCls':'icon-add','url':'/Stockledger/'},
				                                                {'menuid':'152','menuname':'产品库存分布','iconCls':'icon-users','url':'/distribution/'},
				                                                {'menuid':'153','menuname':'货位查询','iconCls':'icon-role','url':'/Cargospace/'},
				                                                {'menuid':'152','menuname':'库区库存查询','iconCls':'icon-users','url':'/Reservoir/'},
				                                                {'menuid':'153','menuname':'卷烟库存查询','iconCls':'icon-role','url':'/Cigarette/'}
			                                                ]
	                                                },{'menuid':'16','iconCls':'icon-sys','menuname':'综合管理',
		                                                'children':[
				                                                {'menuid':'161','menuname':'积压产品清单查询','iconCls':'icon-add','url':'/OverStockProductSearch/'},
				                                                {'menuid':'162','menuname':'入库单查询','iconCls':'icon-users','url':'/StockIntoSearch/'},
				                                                {'menuid':'163','menuname':'出库单查询','iconCls':'icon-role','url':'/StockOutSearch/'},
				                                                {'menuid':'164','menuname':'移库单查询','iconCls':'icon-set','url':'/StockMoveSearch/'},	
				                                                {'menuid':'165','menuname':'盘点单查询','iconCls':'icon-set','url':'/StockCheckSearch/'},				                                   
				                                                {'menuid':'166','menuname':'损益单查询','iconCls':'icon-set','url':'/StockDifferSearch/'},				                                                                                                                                                                    			                                   
				                                                {'menuid':'167','menuname':'分拣单查询','iconCls':'icon-log','url':'/SortOrderSearch/'}
			                                                ]
	                                                }
                                                ]}]";
            return json;
        }
    }
}
