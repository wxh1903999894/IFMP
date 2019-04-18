
$(document).ready(function () {

    $(".Password").keydown(function (e) {
        var curKey = e.which;
        if (curKey == 13) {
            getlogin();
        }
    });

    $(".Account").keydown(function (e) {
        var curKey = e.which;
        if (curKey == 13) {
            getlogin();
        }
    });

    $(".Login").click(function (e) {
        getlogin();
    })

})

function getlogin() {

    var account = $(".Account").val();
    var password = $(".Password").val();
    //var iskeeping = $("#is_keeping").is(':checked');
    login(account, password);
}

function login(account, password) {
    $.ajax({
        url: store.Api + "/account/login",
        type: "POST",
        data: {
            "UserName": account,
            "Password": password
        },
        dataType: "json",
        success: function (data) {
            if (data.Status == 0) {
                ResetUserLogStatus(data);

                //是否保存密码
                //if (document.getElementById("iskeep").checked) {
                //    localStorage.setItem("IsKeepAccount", "1");
                //    localStorage.setItem("KeepAccount", account);
                //    localStorage.setItem("KeepPassword", password);
                //} else {
                //    localStorage.setItem("IsKeepAccount", "0");
                //    localStorage.setItem("KeepAccount", "");
                //    localStorage.setItem("KeepPassword", "");
                //}

                var userAgentInfo = navigator.userAgent;
                //alert(userAgentInfo);
                //console.log(userAgentInfo);
                var Agents = ["Android", "iPhone",
                            "SymbianOS", "Windows Phone",
                            "iPad", "iPod"];
                var flag = true;
                for (var v = 0; v < Agents.length; v++) {
                    if (userAgentInfo.indexOf(Agents[v]) > 0) {
                        flag = false;
                    }
                }
                              
                if (flag) {
                    window.location.href = "/Page/Index.html";
                } else {
                    window.history.go(-1);
                }
            } else {
                //alert(data.Status+":error");
                //alert(data.Data);
                layer.ready(function () {
                    title: false
                    layer.alert(data.Data, {
                        title: false
                    });
                });
                $(".Password").val("");
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


function ResetUserLogStatus(data) {
    //alert(data);
    //alert(data.Data.Token);
    store.userInfo.isLogin = true;
    store.userInfo.token = data.Data.Token;
    store.userInfo.headerUrl = data.Data.HeaderUrl;
    store.expries = unity.dealWithDate(data.Data.Expires);

    localStorage.setItem("USER_NAME", data.Data.RealName);
    localStorage.setItem("HeaderUrl", data.Data.HeaderUrl);
    localStorage.setItem("EXPRIES", unity.dealWithDate(data.Data.Expires)); //token过期时间
    localStorage.setItem("TOKEN", data.Data.Token);
    //alert(data.token);
    init.verificationToken();
    //window.location.reload();    
};

