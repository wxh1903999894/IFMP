<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EnumDictionary.aspx.cs" Inherits="IFMP.EnumDictionary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="2" align="left">系统内置字典</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">字典名称：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_EnumName" OnSelectedIndexChanged="ddl_EnumName_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="-2">--请选择--</asp:ListItem>
                                <asp:ListItem Value="UserType">员工分类</asp:ListItem>
                                <asp:ListItem Value="ClassTypeEnums">班次类型</asp:ListItem>
                                <asp:ListItem Value="ApplyTypeEnums">提交状态</asp:ListItem>
                                <asp:ListItem Value="RegexType">表单验证</asp:ListItem>
                                <asp:ListItem Value="UserLeaveType">岗位类型</asp:ListItem>
                                <asp:ListItem Value="LeaveType">请假类型</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">字典内容：</td>
                        <td align="left" style="margin: 0px; padding: 0px;">
                            <asp:Repeater runat="server" ID="rp_List">
                                <ItemTemplate>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                                        <tbody>
                                            <tr>
                                                <td><%#Eval("EnumContent") %></td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </ItemTemplate>
                            </asp:Repeater>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
