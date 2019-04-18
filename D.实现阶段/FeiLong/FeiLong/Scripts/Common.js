//基础的登录验证
var store = {
    Api: "",
    userInfo: {
        isLogin: false,
        isKeeping: false,
        token: "",
        headerUrl: ""
    },
    expries: ""
};



$(document).ready(function () {
    updateStore();
    if (store.userInfo.isLogin) {
        init.verificationToken();
    } else {

    }

});

//初始化
var init = {
    verificationToken: function () {
        if (store.expries !== null && store.expries !== "") {
            var nowDate = "";
            var st;
            st = setTimeout(function () {
                nowDate = new Date().getTime();

                if (nowDate >= new Date(store.expries).getTime()) {
                    clearTimeout(st);
                    layer.confirm('账户安全期已过,请重新登录', {
                        btn: ['是'] //按钮
                    }, function () {
                        store.userInfo.isLogin = false;
                        localStorage.clear();
                        sessionStorage.clear();
                        window.location.href = "Index.html";
                    });
                } else {
                    init.verificationToken();
                }
            }, 3000);
        } else {
            store.userInfo.isLogin = false;
            localStorage.clear();
            sessionStorage.clear();
            window.location.href = "/Page/Index.html";
        }
    },
};



function updateStore() {
    store.userInfo.headerUrl = localStorage.getItem("HeaderUrl");
    store.expries = localStorage.getItem("EXPRIES"); //token 过期时间
    store.userInfo.token = localStorage.getItem("TOKEN");

    if (localStorage.getItem("IS_KEEPING") === "true") {
        store.userInfo.isKeeping = true;
    }
    //判断token是否错误
    if (store.userInfo.token !== "" && store.userInfo.token !== null) {
        if (getvalidateticket()) {
            store.userInfo.isLogin = true;
        } else {
            localStorage.clear();
            store.userInfo.isLogin = false;
            //layer.confirm('用户在其他地方登陆，请重新登录', {
            //    btn: ['是'] //按钮
            //}, function () {                
            //    //window.location.href = "index.html";
            //});
        }
    } else {
        localStorage.clear();
        store.userInfo.isLogin = false;
    }
}

function logout() {
    localStorage.clear();
    sessionStorage.clear();
    location.reload();
    window.location.href = "/Page/Index.html";
}


//验证token
function getvalidateticket() {
    var result = false;
    $.ajax({
        url: store.Api + "/account/getvalidateticket",
        contentType: "application/json; charset=utf-8",
        type: "POST",
        data: JSON.stringify({
            "token": store.userInfo.token,
            "realname": localStorage.getItem("USER_NAME"),
        }),
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.Status == 0) {
                result = true;
            } else {
                result = false;
            }
        },
        error: function () {
            layer.ready(function () {
                title: false
                layer.alert("当前网络可能有错误", {
                    title: false
                });
            });
        }
    });
    return result;
}



//转化日期格式
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};


var unity = {
    dealWithDate: function (date) {
        return date.replace("/Date(", "").replace(")/", "");
    },
    stringToDate: function (str) {
        return new Date(Date.parse(str.replace(/-/g, "/")));
    },
    getRequest: function () { //获取url中"?"符后的字串
        var url = location.search;
        var thisRequest = [];
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                thisRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
            }
        }
        return thisRequest;
    },
    getURL: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },
    InitNode: function (nodeName, nodeClass) {
        var node = document.createElement(nodeName);
        if (nodeClass != "" && nodeClass != null)
            node.className = nodeClass;
        return node;
    }
};


//分页
//未完成
var pagerunity = {
    getpages: function (total) {
        var html = "";
        if (total > pagesize) {
            var pagecount = Math.ceil(total / pagesize);
            html = html + "&nbsp;";
            html = html + "<span class=\"layui-laypage-count\">共 " + total + " 条</span>";
            html = html + "<a class=\"layui-laypage-prev layui-disabled\" onclick=pagerunity.getpageupdown(0," + pagecount + ")>上一页</a>";
            html = html + "<span class=\"layui-laypage-curr\">";
            html = html + "";
            if (pagecount > 10) {
                if (nowpagelist > 1) {
                    html = html + "<span class=\"layui-laypage-spr\" onclick=pagerunity.getnext(0," + Math.ceil(pagecount / 10) + ")>...</span>";
                }
                var endcount = 10 * (nowpagelist - 1) + 10;
                endcount = endcount > pagecount ? pagecount : endcount;
                for (var i = 10 * (nowpagelist - 1) + 1; i <= endcount; i++) {
                    if (i == nowpage) {
                        html = html + "<span class=\"layui-laypage-curr\"><em class=\"layui-laypage-em\"></em><em>" + i + "</em></span>";
                    } else {
                        html = html + "<a onclick=pagerunity.getpage(" + i + ")>" + i + "</a>";
                    }
                }

                if (nowpagelist < Math.ceil(pagecount / 10)) {
                    html = html + "<span class=\"layui-laypage-spr\" onclick=pagerunity.getnext(1," + Math.ceil(pagecount / 10) + ")>...</span>";
                }
            } else {
                for (var i = 1; i <= pagecount; i++) {
                    if (i == nowpage) {
                        html = html + "<span class=\"layui-laypage-curr\"><em class=\"layui-laypage-em\"></em><em>" + i + "</em></span>";
                    } else {
                        html = html + "<a onclick=pagerunity.getpage(" + i + ")>" + i + "</a>";
                    }
                }
            }
            html = html + "<a onclick=pagerunity.gettopend(1," + pagecount + ") class=\"layui-laypage-last\" title=\"尾页\">尾页</a>";
            html = html + "<a onclick=pagerunity.getpageupdown(1," + pagecount + ") class=\"layui-laypage-next\" >下一页</a>";
            //html = html + "<a onclick=pagerunity.gettopend(1," + pagecount + ")>末页</a>  共" + total + "条";
        } else {

            html = html + "<span class=\"layui-laypage-count\">共 " + total + " 条</span>";
        }

        $("#pager").html(html);
    },
    getnext: function (type, total) {
        if (type == 0) {
            if (nowpagelist > 1) {
                nowpagelist = nowpagelist - 1;
            }
        } else {
            if (nowpagelist < total) {
                nowpagelist = nowpagelist + 1;
            }
        }
        nowpage = (nowpagelist - 1) * 10 + 1;
        pagerunity.dopage();
    },
    gettopend: function (type, pagecount) {
        if (type == 0) {
            nowpagelist = 1;
            pagerunity.getpage(1);
        } else {
            nowpagelist = Math.ceil(pagecount / 10);
            pagerunity.getpage(pagecount);
        }
    },
    getpage: function (page) {
        nowpage = page;
        dopage();
    }, //dopage()
    getpageupdown: function (type, pagecount) {
        if (type == 0) {
            if (nowpage > 1) {
                nowpage = nowpage - 1;
                nowpagelist = Math.ceil(nowpage / 10);
                dopage();
            }
        } else {
            if (nowpage < pagecount) {
                nowpage = nowpage + 1;
                nowpagelist = Math.ceil(nowpage / 10);
                dopage();
            }
        }
    } //dopage()
};


//公用post方法
function basepost(data, path) {
    var result = false;
    $.ajax({
        url: store.Api + path,
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        async: false,
        dataType: "json",
        beforeSend: function (XHR) {
            XHR.setRequestHeader("Authorization", store.userInfo.token);
        },
        success: function (data) {
            if (data.Status == 0) {
                result = true;
            } else {

                layer.ready(function () {
                    title: false
                    layer.alert(data.Data, {
                        title: false
                    });
                });

            }
        },
        error: function () {
            layer.ready(function () {
                title: false
                layer.alert("当前网络可能有错误", {
                    title: false
                });
            });
        }
    });

    return result;
}


function buildth(head, type) {
    var html = "<tr>";
    for (var i = 0; i < head.length; i++) {
        if (type == 1 && i == 0) {
            html = html + "<th><input  align=\"center\" bgcolor=\"#ffffff\" type=\"checkbox\" id=\"selectall\">" + head[i] + "</th>";
        } else {
            html = html + "<th  align=\"center\"  bgcolor=\"#ffffff\">" + head[i] + "</th>";
        }
    }
    html = html + "</tr>";
    return html;
}


function InitRole(isblank) {
    $.ajax({
        url: "/role/getAll",
        type: "GET",
        dataType: "json",
        async: false,
        success: function (data) {
            if (data.Status == 0) {
                var html = "";
                if (isblank) {
                    html = html + "<option selected value=\"0\">全部</option>";
                }
                for (var i = 0; i < data.Data.List.length; i++) {
                    html = html + "<option value=\"" + data.Data.List[i].ID + "\">" + data.Data.List[i].Name + "</option>";
                }
                $("#Role").html(html);
            } else {
                layer.ready(function () {
                    title: false
                    layer.alert(data.Data, {
                        title: false
                    });
                });
            }
        },
        error: function () {
            layer.ready(function () {
                title: false
                layer.alert("当前网络可能有错误", {
                    title: false
                });
            });
        }
    });
}


function InitEnum(type, isblank, obj) {
    $.ajax({
        url: "/base/getAll",
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            Type: type
        },
        success: function (data) {
            if (data.Status == 0) {
                var html = "";
                if (isblank) {
                    html = html + "<option value=\"0\">全部</value>"
                }
                for (var i = 0; i < data.Data.length; i++) {
                    html = html + "<option value=\"" + data.Data[i].ID + "\">" + data.Data[i].Name + "</option>";
                }
                obj.html(html);
            } else {
                layer.ready(function () {
                    title: false
                    layer.alert(data.Data, {
                        title: false
                    });
                });
            }
        },
        error: function () {
            layer.ready(function () {
                title: false
                layer.alert("当前网络可能有错误", {
                    title: false
                });
            });
        }
    });
}


function InitFlow(tabletype) {
    var obj = null;
    $.ajax({
        url: "/flow/getAll",
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            TableType: tabletype
        },
        success: function (data) {
            if (data.Status == 0) {
                obj = data.Data;
            } else {
                layer.ready(function () {
                    title: false
                    layer.alert(data.Data, {
                        title: false
                    });
                });
            }
        },
        error: function () {
            layer.ready(function () {
                title: false
                layer.alert("当前网络可能有错误", {
                    title: false
                });
            });
        }
    });
    return obj;
}