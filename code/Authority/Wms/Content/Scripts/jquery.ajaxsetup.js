var g_MsgBoxTitle = "系统信息";
var __waitHTML = '<div style="padding: 20px;"><span style="font-weight: bold;padding-left: 10px; color: #FF66CC;"><div id="g_p" class="easyui-progressbar" style="width:280px;"></div></span></div>';
var j_waitDialog;

function progress() {
    var value;
    if ($('#g_p').length == 1){
        value = $('#g_p').progressbar('getValue');
        if (value < 100) {
            value += Math.floor(Math.random() * 10);
            $('#g_p').progressbar('setValue', value);
            setTimeout(arguments.callee, 200);
        } else {
            value = 0;
            value += Math.floor(Math.random() * 10);
            $('#g_p').progressbar('setValue', value);
            setTimeout(arguments.callee, 200);
        };
    };
};

// 显示Ajax操作时的提示窗口
function ShowWaitMessageDialog(dlgTitle) {
    if (typeof (dlgTitle) != "string")
        dlgTitle = "请求处理中,请稍后......";    
    var j_dialog = $(__waitHTML);
    j_dialog.appendTo('body').show().dialog({
        height: 120, width: 350, modal: true, resizable: false, closable: false, title: dlgTitle
    });
    $.parser.parse(j_dialog);
    progress();
    return j_dialog;
}

// 关闭Ajax操作时的提示窗口
function HideWaitMessageDialog(j_dialog) {
    if (j_dialog == null)
        return;
    j_dialog.dialog('close');
    j_dialog.remove();
    j_dialog = null;
}

$(function () {
    // 设置Ajax操作的默认设置
    $.ajaxSetup({
        cache: false,
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (typeof (errorThrown) != "undefined")
                $.messager.alert(g_MsgBoxTitle, "调用服务器失败。<br />" + errorThrown, 'error');
            else {
                var error = "<b style='color: #f00'>" + XMLHttpRequest.status + "  " + XMLHttpRequest.statusText + "</b>";
                var start = XMLHttpRequest.responseText.indexOf("<title>");
                var end = XMLHttpRequest.responseText.indexOf("</title>");
                if (start > 0 && end > start)
                    error += "<br /><br />" + XMLHttpRequest.responseText.substring(start + 7, end);
                $.messager.alert(g_MsgBoxTitle, "调用服务器失败。<br />" + error, 'error');
            }
        }
    });
});
(function ($){
    $.ajaxSender = 
    {
        send:function(url,data,callback) {
            var j_waitDialog = ShowWaitMessageDialog();
            $.ajaxSender.complete = callback;
            $.ajax({
                url: url, type: "POST", dataType: "text",
                data: data,
                complete: function () { HideWaitMessageDialog(j_waitDialog); },
                success: function (responseText) {
                    var result = $.evalJSON(responseText);
                    if (result.success) {
                        $.messager.alert(g_MsgBoxTitle, result.msg, "info",function()
                        {
                             $.ajaxSender.complete(result.data);
                        });                       
                    } else {
                        $.messager.alert(g_MsgBoxTitle, result.msg + '<br />' + result.data, "error");
                    }
                }
            })
         },
         complete: $.noop()
    } 
})(jQuery);