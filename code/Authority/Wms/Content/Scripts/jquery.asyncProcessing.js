(function ($) {
    var g_progressHTML = '<div style="padding: 20px;"><span id="g_total_progressbar_name">总体进度</span><div id="g_total_progressbar" class="easyui-progressbar" style="width:280px;"></div><br/><span id="g_current_progressbar_name">当前进度</span><div id="g_current_progressbar" class="easyui-progressbar" style="width:280px;"></div><div style="padding: 20px;"><ul id="messages"></ul></div></div>';
    var g_progressDialog;
    var g_connection;

    $.asyncProcessing =
    {
        Start: function (name, data, dlgTitle) {
            g_connection = $.connection(name);
            g_connection.received(function (state) {
                var progressState = state;

                $('#messages').empty();
                $.each(progressState.Messages, function (i, n) {
                    $('#messages').append('<li>' + n + '</li>');
                });
                $.each(progressState.Errors, function (i, n) {
                    $('#messages').append('<li style="color: #FF0000">' + n + '</li>');
                });

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
                        break;
                    case 4: //Error                        
                        break;
                    case 5: //Info                       
                        break;
                    case 6: //Question
                        break;
                    case 7: //Confirm
                        break;
                    case 8: //Complete
                        $.messager.confirm(g_MsgBoxTitle, progressState.Messages, function (r) {
                            g_connection.stop();
                            $.asyncProcessing.OnProgressComplete();
                            $.asyncProcessing.HideProgressDialog(g_progressDialog);
                        });
                        break;
                    case 9: //Stop                    
                        break;
                    default:
                        break;
                }
            });
            g_connection.start({ transport: 'webSockets' }, function () {
                g_connection.send(data);
            });
            g_progressDialog = $.asyncProcessing.ShowProgressDialog(dlgTitle);
        },
        Stop: function () {
            g_connection.send($.toJSON({ ActionType: 'stop' }, { ActionType: 'stop' }));
        },
        OnProgressComplete: $.noop(),
        ShowProgressDialog: function (dlgTitle) {
            if (typeof (dlgTitle) != "string")
                dlgTitle = "请求处理中,请稍后......";
            var j_progressDialog = $(g_progressHTML);
            j_progressDialog.appendTo('body').show().dialog({
                height: 280, width: 350, modal: true, resizable: false, closable: false, title: dlgTitle,
                buttons: [{
                    text: '取消',
                    handler: function () { $.asyncProcessing.Stop(); }
                }]
            });
            $.parser.parse(j_progressDialog);
            return j_progressDialog;
        },
        HideProgressDialog: function (j_progressDialog) {
            if (j_progressDialog == null)
                return;
            j_progressDialog.dialog('close');
            j_progressDialog.remove();
            j_progressDialog = null;
        }
    };
})(jQuery);
