<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysModuleManage.aspx.cs" Inherits="IFMP.sysmanage.SysModuleManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧校园基础管理平台</title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/jquery-1.8.2.min.js"></script>
    <script src="../js/AsyncBox.v1.4.js"></script>
    <script src="../js/AsyncBox.v1.4.5.js"></script>
    <script src="../js/choice.js"></script>
    <script type="text/javascript">
        var type = window.location.href.split("?");
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span><asp:Label ID="lbl_ParentMenu" runat="server" Text="系统管理"></asp:Label><span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="模块管理"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <table width="98%" align="center" border="0" cellpadding="0" cellspacing="0">
            <tr style="vertical-align: middle; height: 30px">
                <td width="15%" class="tabfirst">
                    <div style="width: 100%; background: #2d7bca; height: 40px; margin-top: 20px; border-radius: 5px; color: white; line-height: 40px; text-align: left; text-indent: 20px; font-size: 14px; font-weight: bold">
                        模块列表
                                <asp:LinkButton ID="btn_Add" runat="server" OnClick="btn_Add_Click"><span style="position:relative; top:12px; right:5px;float:right;color:#ffffff;font-size:12px;font-weight:100;" >添加</span>
                                </asp:LinkButton>
                    </div>
                </td>
                <td valign="top" rowspan="2">
                    <iframe scrolling="auto" width="100%" frameborder="0" src="SysModuleEdit.aspx" name="framemain"
                        id="framemain" runat="server" height="550px"></iframe>
                </td>
            </tr>
            <tr>
                <td valign="top" style="height: 90%; background: #FFFFFF">
                    <div>
                        <asp:TreeView ID="tv_Meun" ShowLines="True" Height="560px"
                            MaxDataBindDepth="2" BackColor="White" BorderColor="#666666" runat="server" OnSelectedNodeChanged="tv_Meun_SelectedNodeChanged" LineImagesFolder="~/TreeLineImages" ForeColor="#333333">
                            <RootNodeStyle BackColor="#FFFFFF" />
                        </asp:TreeView>
                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
