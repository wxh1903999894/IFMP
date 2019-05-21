<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="DormitoryAPP.Login" %>

<!DOCTYPE html>

<html>
<head>
    <meta charset="utf-8">
    <title>系统集成服务管理平台</title>
    <link rel="stylesheet" type="text/css" href="css/login.css">
</head>
<body onload="aaaa()">
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_Code"></asp:HiddenField>
        <div class="logincent">
            <div class="logindiv">
                <h2>高科系统集成服务管理平台</h2>
                <div class="userin">
                    <span class="logintxt">账　号:</span> <span class="loginin">
                        <asp:TextBox ID="txt_UserID" runat="server"></asp:TextBox>
                    </span>
                </div>
                <div class="pwdin">
                    <span class="logintxt">密　码:</span> <span class="loginin">
                        <asp:TextBox ID="txt_Pwd" runat="server" TextMode="Password"></asp:TextBox></span>
                </div>

                <div class="pwdin" style="margin-bottom: 0px; height: 12px">
                    <span class="logintxt" style="font-size: 12px">记住密码</span> <span class="loginin">
                        <input style="width: 20px; height: 20px; color: #fff" type="checkbox" id="Remember" />
                    </span>
                </div>
                <div class="btn">
                    <asp:Button ID="btn_Login" runat="server" Text="登 录" OnClientClick="RememberPW()" OnClick="btn_Login_Click" />

                </div>
            </div>
        </div>
    </form>
</body>
</html>
<script type="text/javascript">
    function aaaa() {
        Setcookie("ScreenH", screen.height);
        if (localStorage.getItem("pw") != null) {
            document.getElementById("txt_Pwd").value = localStorage.getItem("pw");
            document.getElementById("Remember").setAttribute("checked", "checked");
        };
    }
    function Setcookie(name, value) {
        //设置名称为name,值为value的Cookie
        var expdate = new Date();   //初始化时间
        expdate.setTime(expdate.getTime() + 30 * 60 * 1000);   //时间
        document.cookie = name + "=" + value + ";expires=" + expdate.toGMTString() + ";path=/";

        //即document.cookie= name+"="+value+";path=/";   时间可以不要，但路径(path)必须要填写，因为JS的默认路径是当前页，如果不填，此cookie只在当前页面生效！~
    }

    function RememberPW() {

        if (document.getElementById("Remember").checked) {
            localStorage.setItem("pw", document.getElementById("txt_Pwd").value);
        } else {
            localStorage.removeItem("pw");
        }
        return false;
    }

</script>
