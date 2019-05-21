<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IFMP.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧工厂管理平台</title>
    <link href="css/login.css" type="text/css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        <div class="mtop">
            <div class="ld1200">
                <img src="images/fl_03_1.png" />
            </div>
            <div class="ldiv">
                <div class="ld1200">
                    <div class="bgdiv">
                        <img src="images/fl_07.png" />
                    </div>
                    <div class="logindiv">
                        <div class="flname">智慧工厂管理平台</div>
                        <div>
                            <input type="text" id="username" placeholder="用户名" />
                        </div>
                        <div>
                            <input type="password" id="password" placeholder="密码" />
                        </div>
                        <div>
                            <input type="button" value="登录" onclick="Login()" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="lgdes">
                版权所有权：芜湖市高科电子有限公司 皖ICP备0123231号<br>
                All Rights Reserved
            </div>
            <div class="by"></div>
        </div>
    </form>
</body>
</html>
<script src="plugins/jquery-3.3.1.js"></script>
<script type="text/javascript">
    function Login() {
        var username = $("#username").val().trim();
        var password = $("#password").val();
        if (username == null || username == "") {
            alert("请输入用户名");
            $("#username").focus();
            return false;
        }
        if (password == null || password == "") {
            alert("请输入密码");
            $("#password").focus();
            return false;
        }

        $.getJSON("ashx/Login.ashx", { method: "LoginIn", name: username, psw: password }, function (data) {
            if (data.result == "") {
                //alert("登录成功");
                window.location.href = "Index.html";
            }
            else {
                alert(data.result);
                $("#username").focus();
            }
        })
    }

    $("#username").keydown(function (e) {
        var curKey = e.which;
        if (curKey == 13) {
            Login();
        }
    });

    $("#password").keydown(function (e) {
        var curKey = e.which;
        if (curKey == 13) {
            Login();
        }
    });
</script>
