<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveDetail.aspx.cs" Inherits="IFMP.sysmanage.LeaveDetail" %>

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
        <asp:HiddenField runat="server" ID="hf_Type" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">请假详细信息</th>
                    </tr>
                    <tr>
                        <td align="right">请假人：</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_UserID"></asp:Literal></td>
                        <td align="right">请假天数：</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Day"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td align="right" width="100px">开始日期：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_BeginDate"></asp:Literal>
                        </td>
                        <td align="right" width="100px">结束日期：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_EndDate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">请假类型：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_LeaveType"></asp:Literal>
                        </td>
                        <td align="right">请假时间：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_CreateDate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">请假原因描述：
                        </td>
                        <td colspan="3">
                            <asp:Literal runat="server" ID="ltl_Content"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <th align="left" colspan="4">审核信息</th>
                    </tr>
                    <tr>
                        <td colspan="4" style="padding: 0px; margin: 0px;">
                            <table width="100%" cellspacing="0" cellpadding="0">
                                <tbody>
                                    <tr style="text-align: center; font-weight: bold; color: #508CE4;">
                                        <td>发起时间</td>
                                        <td>审核人</td>
                                        <td>审核角色</td>
                                        <td>审核时间</td>
                                        <td>状态</td>
                                    </tr>
                                    <asp:Repeater runat="server" ID="rp_List">
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%#Eval("SendDate") %></td>
                                                <td align="center"><%#Eval("RealName") %></td>
                                                <td align="center"><%#Eval("RoleName") %></td>
                                                <td align="center"><%#Eval("AuditDate") %></td>
                                                <td align="center"><%#Eval("LeaveState") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <tr runat="server" id="tr_null">
                                        <td align="center" colspan="5">暂无审核记录</td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

