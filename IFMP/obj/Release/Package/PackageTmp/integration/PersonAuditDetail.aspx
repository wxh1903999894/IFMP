<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonAuditDetail.aspx.cs" Inherits="IFMP.integration.PersonAuditDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
    <style>
        #tb td {
            text-align: center;
            border: none;
            border-right: 1px solid #000;
            border-bottom: 1px solid #000;
            width: 50%;
        }

            #tb td:last-child {
                border-right: 0px;
            }

        #tb tr:last-child td {
            border-bottom: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th align="left" colspan="4">积分奖扣信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">事件：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_EventName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td align="right">主题：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_STitle" runat="server"></asp:Literal></td>
                    </tr>

                    <tr>
                        <td align="right">记录人：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_VUser" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">奖扣日期：
                        </td>
                        <td>
                            <asp:Literal ID="ltl_VDate" runat="server"></asp:Literal></td>
                        <td align="right" width="100px">审核状态：</td>
                        <td>
                            <asp:Literal ID="ltl_AduitState" runat="server"></asp:Literal></td>
                    </tr>
                    <tr id="tr" runat="server">
                        <td align="right">人员信息：
                        </td>
                        <td colspan="3" style="padding: 0px; margin: 0px;">
                            <table width="100%" class="border-r" cellspacing="0" cellpadding="0">
                                <tr style="text-align: center; color: #508CE4; font-weight: bold;">
                                    <td style="width: 20%">奖扣人员</td>
                                    <td style="width: 20%">B分
                                    </td>
                                </tr>
                                <asp:Repeater runat="server" ID="rp_List">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%#Eval("RealName") %></td>
                                            <td><%#Eval("BScore") %></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr runat="server" id="tr_null">
                                    <td colspan="2" style="text-align: center">暂无记录</td>
                                </tr>
                            </table>
                        </td>
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
                        <td align="right">图片</td>
                        <td colspan="3">
                            <asp:Image ID="img" runat="server" Width="100px" Height="100px" /></td>
                    </tr>
                    <tr>
                        <td align="right">事件说明：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_EventMark" runat="server"></asp:Literal></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
