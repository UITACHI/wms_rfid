(function ($) {
    var g_progressHTML = '<div style="padding: 20px;"><span id="g_total_progressbar_name">总体进度</span><div id="g_total_progressbar" class="easyui-progressbar" style="width:280px;"></div><br/><span id="g_current_progressbar_name">当前进度</span><div id="g_current_progressbar" class="easyui-progressbar" style="width:280px;"></div><div><br/><ul id="messages"></ul></div></div>';
    var g_progressDialog;
    var g_connection;
    jQuery.extend({
        ProgressStart: function (name, data, dlgTitle) {
            g_connection = $.connection(name);
            g_connection.received(function (state) {
                var progressState = state;
                //处理返回结果
                switch (progressState.State) {
                    case 0: //Start
                        break;
                    case 1: //Processing
                        $('#g_total_progressbar_name').empty();
                        $('#g_total_progressbar_name').append(progressState.TotalProgressName);
                        $('#g_total_progressbar').progressbar('setValue', progressState.TotalProgressValue);
                        $('#g_current_progressbar_name').empty();
                        $('#g_current_progressbar_name').append(progressState.CurrentProgressName);
                        $('#g_current_progressbar').progressbar('setValue', progressState.CurrentProgressValue);
                        break;
                    case 2: //Prompt
                        break;
                    case 3: //Warning
                        $.messager.alert(g_MsgBoxTitle, progressState.Messages[0], "warning");
                        break;
                    case 4: //Error
                        $.messager.alert(g_MsgBoxTitle, progressState.Messages[0], "error");
                        break;
                    case 5: //Info
                        $.messager.alert(g_MsgBoxTitle, progressState.Messages[0], "info");
                        break;
                    case 6: //Question
                        break;
                    case 7: //Confirm
                        break;
                    case 8: //Complete
                        //$.messager.alert(g_MsgBoxTitle, progressState.Errors[0], "error");
                        g_connection.stop();
                        //HideProgressDialog(g_progressDialog);
                        OnProgressComplete();
                        break;
                    default:
                };
                $('#messages').empty();
                $.each(progressState.Messages, function (i, n) {
                    $('#messages').append('<li>' + n + '</li>');
                });
                $.each(progressState.Errors, function (i, n) {
                    $('#messages').append('<li>' + n + '</li>');
                });
            });
            g_connection.start(function () {
                g_connection.send(data);
            });
            g_progressDialog = ShowProgressDialog(dlgTitle);
        },
        ProgressStop: function () {
            g_connection.send({ ActionType: 'stop' });
        },
        OnProgressComplete: $.noop
    });

    function ShowProgressDialog(dlgTitle) {
        if (typeof (dlgTitle) != "string")
            dlgTitle = "请求处理中,请稍后......";
        var j_progressDialog = $(g_progressHTML);
        j_progressDialog.appendTo('body').show().dialog({
            height: 300, width: 350, modal: true, resizable: false, closable: false, title: dlgTitle,
            buttons:[{
				text:'确定',
				handler:function(){}
			},{
				text:'取消',
				handler:function(){}
			}]
        });
        $.parser.parse(j_progressDialog);
        return j_progressDialog;
    }

    function HideProgressDialog(j_progressDialog) {
        if (j_progressDialog == null)
            return;
        j_progressDialog.dialog('close');
        j_progressDialog.remove();
        j_progressDialog = null;
    }
} (jQuery));
