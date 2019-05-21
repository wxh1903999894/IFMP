<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogManage.aspx.cs" Inherits="IFMP.sysmanage.LogManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧工厂管理平台</title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
</head>
<body>
    <form runat="server" id="form1">
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>系统管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="日志查看"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td width="60" align="right">操作人：</td>
                        <td width="180">
                            <asp:TextBox runat="server" ID="txt_CreateUser" CssClass="layui-input"></asp:TextBox>
                        </td>
                        <td width="80" align="right">日志类型：</td>
                        <td width="150">
                            <asp:DropDownList runat="server" ID="ddl_LogType"></asp:DropDownList>
                        </td>
                        <td width="80" align="right">操作时间：</td>
                        <td width="230">
                            <asp:TextBox runat="server" ID="txt_BeginDate" Width="75px" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--
                            <asp:TextBox runat="server" ID="txt_EndDate" Width="75px" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btn_Search" Text="查询" OnClick="btn_Query_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <th>日志类型</th>
                    <th>日志内容</th>
                    <th>操作人</th>
                    <th>操作时间</th>
                </tr>
                <asp:Repeater runat="server" ID="rp_List">
                    <ItemTemplate>
                        <tr>
                            <td><%#GK.IFMP.Common.CommonFunction.CheckEnum<GK.IFMP.Common.CommonEnum.LogType>(Eval("LogType")) %></td>
                            <td><%#Eval("Message") %></td>
                            <td><%#Eval("RealName") %></td>
                            <td><%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <tr runat="server" id="tr_null">
                    <td align="center" colspan="4">暂无记录</td>
                </tr>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>
