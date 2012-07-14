/// <reference path="../jquery-easyui-1.2.6/jquery-1.7.2.min.js" />
/// <reference path="../jquery-easyui-1.2.6/jquery.easyui.min.js" />
(function ($) {
    var menus_root = {};
    $(function () {
        LogOn();
    });
    function LogOn() {
        $.getJSON("/Home/GetUser/?t=" + new Date(), function (data) {
            if (data) {
                if (!data.Identity.IsAuthenticated) {
                    clearMenu();
                    $('#login-bg').addClass("login");
                    show('#c', 350, 350);
                    $('#dlg-logon').dialog('open');
                }
                else {
                    hide('#c');
                    initMenu();
                    $('#welcome').html('欢迎：' + data.Identity.Name);
                    //init_changecity(data.Identity.Name);
                    //init_changesystem(data.Identity.Name);
                    //init_changeserver(data.Identity.Name);
                }
            }
            else {
                $('#login - bg').addClass("login");
                show('#c', 350, 350);
                $('#dlg-logon').dialog('open');
            }
        });
    }

    //初始化菜单
    function initMenu() {
        clearMenu();
        $.getJSON("/Home/GetMenu/?t=" + new Date(), function (data) {
            menus_root = data[0];
            initMainMenu(menus_root);
            initLeftMenu(menus_root);
        });
        tabEven();
        tabMenuEven();
    }

    //清空菜单
    function clearMenu() {
        var callbacks = $.Callbacks();
        $.each($('#leftmenu').accordion('panels'), function (c, p) {
            callbacks.add(function () {
                $('#leftmenu').accordion('remove', p.panel('options').title);
            });
        });
        callbacks.fire();
        $("#mainmenu").empty();
    }

    function initMainMenu(menus_root) {
        if (menus_root.children) {
            $.each(menus_root.children, function (i, n) {
                var html = '<a href="javascript:void(0)" id="m-' + n.menuid + '" ><span class="icon ' + n.iconCls + '" >&nbsp;</span>' + n.menuname + '</a>';
                $("#mainmenu").append(html);
                addhildrenMenu(n, true);
                $('#m-' + n.menuid).splitbutton({
                    menu: '#m-c' + n.menuid
                });
            });
        }
    }

    function addhildrenMenu(o, isRoot) {
        if (o.children != null) {
            html = '<div id="m-c' + o.menuid + '" style="width:180px;">';
            html += '</div>';
            if (isRoot) {
                $("body").append(html);
            } else {
                $("#m-" + o.menuid).append(html);
            }
            $.each(o.children, function (j, c) {
                var html = '<div class="menu-item" useiframe="' + c.useiframe + '" id="m-' + c.menuid + '" iconCls="icon ' + c.iconCls + '" rel="' + c.url + '" ref="' + c.menuid + '">' + c.menuname + '</div>';
                $("#m-c" + o.menuid).append(html);
                addhildrenMenu(c, false);
            });
            $("#m-c" + o.menuid).find("div.menu-item").click(function () {
                var tabTitle = $(this).children('.menu-text').text();
                var url = $(this).attr("rel");
                var menuid = $(this).attr("ref");
                var icon = getIcon(menuid);
                var useiframe = true;
                if ($(this).attr("useiframe") == "false") {
                    useiframe = false;
                } else if ($(this).attr("useiframe") == "true") {
                    useiframe = true;
                } else {
                    useiframe = true;
                }
                addTab(tabTitle, url, icon, useiframe, true);
            });
        }
    }

    function initLeftMenu(menus_root) {
        $("#leftmenu").accordion({ animate: false });

        $.each(menus_root.children, function (i, n) {
            var menulist = '';

            menulist += '<ul>';
            $.each(n.children, function (j, o) {
                menulist += '<li><div><a useiframe="' + o.useiframe + '"  ref="' + o.menuid + '" href="#" rel="' + o.url + '" ><span class="icon ' + o.iconCls + '" >&nbsp;</span><span class="leftmenu">' + o.menuname + '</span></a></div></li> ';
            })
            menulist += '</ul>';

            $('#leftmenu').accordion('add', {
                title: n.menuname,
                content: menulist,
                iconCls: 'icon ' + n.iconCls
            });

        });

        $('.easyui-accordion li a').click(function () {
            var tabTitle = $(this).children('.leftmenu').text();
            var url = $(this).attr("rel");
            var menuid = $(this).attr("ref");
            var icon = getIcon(menuid);
            var useiframe = true;
            if ($(this).attr("useiframe") == "false") {
                useiframe = false;
            } else if ($(this).attr("useiframe") == "true") {
                useiframe = true;
            } else {
                useiframe = true;
            }
            addTab(tabTitle, url, icon, useiframe, true);
        }).hover(function () {
            $(this).parent().addClass("hover");
        }, function () {
            $(this).parent().removeClass("hover");
        });

        //选中第一个
        var panels = $('#leftmenu').accordion('panels');
        var t = panels[0].panel('options').title;
        $('#leftmenu').accordion('select', t);
    }

    //获取左侧导航的图标
    function getIcon(menuid) {
        var icon = 'icon ';
        $.each(menus_root.children, function (i, n) {
            $.each(n.children, function (j, o) {
                if (o.menuid == menuid) {
                    icon += o.iconCls;
                }
            })
        })
        return icon;
    }

    function addTab(subtitle, url, icon, useiframe, closable) {
        $.getJSON("/Home/GetUser/?t=" + new Date(), function (data) {
            if (data) {
                if (!data.Identity.IsAuthenticated) {
                    $.messager.confirm('系统提示', '操作已超时，请重新登录！', function (r) {
                        location.reload();
                    });
                }
                else {
                    addTabToMainPanl(subtitle, url, icon, useiframe, closable);
                }
            }
            else {
                $.messager.confirm('系统提示', '操作已超时，请重新登录！', function (r) {
                    location.reload();
                });
            }
        });
    }
    function addTabToMainPanl(subtitle, url, icon, useiframe, closable) {

        if (!$('#tabs').tabs('exists', subtitle)) {
            $('#tabs').tabs('add', {
                title: subtitle,
                fit: true,
                width: 'auto',
                height: 'auto',
                content: '',
                closable: closable,
                icon: icon
            });

            $('#tabs').tabs('select', subtitle);
            var currTab = $('#tabs').tabs('getSelected');
            currTab.data("src", url);
            currTab.data("useiframe", useiframe);
            tabEven();

            $('#mm-tabupdate').click();
        } else {
            $('#tabs').tabs('select', subtitle);
        }
    }

    function tabEven() {
        $(".tabs-inner").off('dblclick contextmenu');
        /*双击关闭TAB选项卡*/
        $(".tabs-inner").bind('dblclick', function () {
            var subtitle = $(this).children(".tabs-closable").text();
            $('#tabs').tabs('close', subtitle);
        })
        /*为选项卡绑定右键*/
        $(".tabs-inner").bind('contextmenu', function (e) {
            var subtitle = $(this).children(".tabs-title").text();
            $('#mm').data("currtab", subtitle);
            $('#tabs').tabs('select', subtitle);

            //begin todo
            //禁用关闭全部
            var tabcount = $('#tabs').tabs('tabs').length; //tab选项卡的个数
            if (tabcount <= 1) {
                $('#mm-tabcloseother').attr("disabled", "disabled").css({ "cursor": "default", "opacity": "0.4" });
            }
            else {
                $('#mm-tabcloseother').removeAttr("disabled").css({ "cursor": "pointer", "opacity": "1" });
            }

            //禁用关闭右边全部
            var tabs = $('#tabs').tabs('tabs');     //获得所有的Tab选项卡
            var tabcount = tabs.length;  //Tab选项卡的个数
            var lasttab = tabs[tabcount - 1];  //获得最后一个Tab选项卡
            var lasttitle = lasttab.panel('options').tab.text(); //最后一个Tab选项卡的Title
            var currtab_title = $('#mm').data("currtab");  //当前Tab选项卡的Title

            if (lasttitle == currtab_title) {
                $('#mm-tabcloseright').attr("disabled", "disabled").css({ "cursor": "default", "opacity": "0.4" });
            }
            else {
                $('#mm-tabcloseright').removeAttr("disabled").css({ "cursor": "pointer", "opacity": "1" });
            }

            //禁用关闭左边全部
            var onetab = tabs[0];  //第一个Tab选项卡
            var onetitle = onetab.panel('options').tab.text();  //第一个Tab选项卡的Title

            if (onetitle == currtab_title) {
                $('#mm-tabcloseleft').attr("disabled", "disabled").css({ "cursor": "default", "opacity": "0.4" });
            }
            else {
                $('#mm-tabcloseleft').removeAttr("disabled").css({ "cursor": "pointer", "opacity": "1" });
            }
            //end todo

            $('#mm').menu('show', {
                left: e.pageX,
                top: e.pageY
            });
            $('#mm').css({'z-index': 2147483647});
            return false;
        });
    }

    //绑定右键菜单事件
    function tabMenuEven() {
        //刷新
        $('#mm-tabupdate').click(function () {
            var currTab = $('#tabs').tabs('getSelected');
            var url = currTab.data("src") + '&t=' + new Date();
            var useiframe = currTab.data("useiframe");
            if (!useiframe) {
                $.get(url, function (data) {
                    $('#tabs').tabs('update', {
                        tab: currTab,
                        options: {
                            content: data
                        }
                    });
                }, "html");
            } else {
                $('#tabs').tabs('update', {
                    tab: currTab,
                    options: {
                        content: createFrame(url)
                    }
                })
            }
        })

        //当前全屏
        $('#mm-tabFullScreen').click(function (e) {
            e.preventDefault();
            $('#tabs').tabs('resize').fullScreen({ callback: function () {
                $('#tabs').tabs('resize');
            }
            });
        });

        //关闭当前
        $('#mm-tabclose').click(function () {
            var currtab_title = $('#mm').data("currtab");
            $('#tabs').tabs('close', currtab_title);
        })

        //全部关闭
        $('#mm-tabcloseall').click(function () {
            $('.tabs-inner span').each(function (i, n) {
                var t = $(n).text();
                $('#tabs').tabs('close', t);
            });
        });

        //关闭除当前之外的TAB
        $('#mm-tabcloseother').click(function () {
            $('#mm-tabcloseright').click();
            $('#mm-tabcloseleft').click();
        });

        //关闭当前右侧的TAB
        $('#mm-tabcloseright').click(function () {
            var nextall = $('.tabs-selected').nextAll();
            if (nextall.length == 0) {
                return false;
            }
            nextall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#tabs').tabs('close', t);
            });
            return false;
        });

        //关闭当前左侧的TAB
        $('#mm-tabcloseleft').click(function () {
            var prevall = $('.tabs-selected').prevAll();
            if (prevall.length == 0) {
                return false;
            }
            prevall.each(function (i, n) {
                var t = $('a:eq(0) span', $(n)).text();
                $('#tabs').tabs('close', t);
            });
            return false;
        });

        //退出MENU
        $("#mm-exit").click(function () {
            $('#mm').menu('hide');
        })
    }

    function createFrame(url) {
        var s = '<iframe scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;padding:0px;"></iframe>';
        return s;
    }
})(jQuery)
