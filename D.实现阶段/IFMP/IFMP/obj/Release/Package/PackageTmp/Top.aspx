<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Top.aspx.cs" Inherits="IFMP.Top" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="plugins/layui/css/layui.css" rel="stylesheet">
    <link href="css/iconfont.css" rel="stylesheet">
    <link href="css/main.css" rel="stylesheet">
    <script src="plugins/layui/layui.js"></script>
    <script src="plugins/jquery-1.8.2.min.js"></script>
    <script>
        function LoginOut() {
            var aresult = true;
            $.ajax({
                url: "/ashx/Login.ashx",
                cache: false,
                type: "GET",
                async: false,
                data: "method=Out",
                dataType: "json",
                success: function (data) {
                    if (data.result == "fail") {
                        aresult = false;
                    }
                }
            });
            if (!aresult) {
                alert("系统提示：退出登录时出错，请联系系统管理员");
                return;
            }
            else {
                //localStorage.removeItem('ModelSelArray');
                try {
                    parent.parent.location.href = '../Default.aspx';
                }
                catch (e) {
                    parent.location.href = 'Default.aspx';
                }
            }
        }
    </script>
</head>
<body class="layui-layout-body topbg">
    <form id="form1" runat="server">
        <%--   <asp:HiddenField runat="server" ID="hf_RealName" />
        <asp:HiddenField runat="server" ID="hf_ID" />--%>
        <div class="layui-layout layui-layout-admin">
            <div class="layui-logo">
                <a href="Main.aspx" target="main">
                    <img src="images/fl_03.png"></a>
            </div>
            <ul class="layui-nav layui-layout-right topmenu">
                <li class="layui-nav-item"><a href="Main.aspx" target="main" class="iconfont icon-zhuye">主页</a></li>
                <li class="layui-nav-item"><a href="sysmanage/SysUserInfo.aspx" target="main" class="iconfont icon-gerenzhongxin">个人中心</a></li>
                <li class="layui-nav-item"><a href="sysmanage/SysNoticeManage.aspx" target="main" class="iconfont icon-xinxi-copy">消息中心</a></li>
                <li class="layui-nav-item"><a onclick="LoginOut()" class="iconfont icon-tuichu">退出</a></li>
            </ul>
        </div>
    </form>
</body>
</html>
