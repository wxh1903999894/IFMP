<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuckleAdditionDetail.aspx.cs" Inherits="IFMP.integration.BuckleAdditionDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" type="text/css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <th colspan="4" align="left">积分奖扣明细</th>
                </tr>
                <tr>
                    <td align="right" width="100px">事件：
                    </td>
                    <td colspan="3">
                        <asp:Literal ID="ltl_EventName" runat="server"></asp:Literal>

                    </td>
                </tr>
                <tr>
                    <td align="right">主题：
                    </td>
                    <td colspan="3">
                        <asp:Literal ID="ltl_STitle" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">奖扣对象：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_RewardUser" runat="server"></asp:Literal></td>
                    <td align="right" width="100px">是否作废：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_SState" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">B分：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_BScore" runat="server"></asp:Literal>
                    </td>
                    <td align="right">记录人：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_VUser" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">奖扣日期：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_VDate" runat="server"></asp:Literal></td>
                    <td align="right">审核状态：</td>
                    <td>
                        <asp:Literal ID="ltl_AduitState" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">初审人：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_FirstAduitUser" runat="server"></asp:Literal>
                    </td>
                    <td align="right">终审人：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_LastAduitUser" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">初审日期：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_FirstAduitDate" runat="server"></asp:Literal>
                    </td>
                    <td align="right">终审日期：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_LastAduitDate" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">初审说明：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_FirstAduitMark" runat="server"></asp:Literal>
                    </td>
                    <td align="right">终审说明：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_LastAduitMark" runat="server"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td align="right">事件说明：
                    </td>
                    <td colspan="3">
                        <asp:Literal ID="ltl_EventMark" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">图片</td>
                    <td colspan="3">
                        <asp:Image ID="img" runat="server" Width="400px" Height="400px" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

