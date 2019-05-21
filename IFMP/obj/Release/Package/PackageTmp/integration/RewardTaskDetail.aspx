<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RewardTaskDetail.aspx.cs" Inherits="IFMP.integration.RewardTaskDetail" %>

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
    <%--<style>
        #tb td {
            text-align: center;
            border: none;
            border-right: 1px solid #000;
            border-bottom: 1px solid #000;
            width: 33.3%;
        }

            #tb td:last-child {
                border-right: 0px;
            }

        #tb tr:last-child td {
            border-bottom: 0px;
        }
    </style>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <td align="right" width="100px">任务名称：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_TaskName" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td align="right">汇报截止日期：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_EndDate" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td align="right">责任人：
                        </td>
                        <td>
                            <asp:Literal ID="ltl_TaskUser" runat="server"></asp:Literal></td>
                        <td align="right">状态：
                        </td>
                        <td>
                            <asp:Literal ID="ltl_TState" runat="server"></asp:Literal></td>
                    </tr>
                    <tr>
                        <td align="right">悬赏分数：
                        </td>
                        <td>
                            <asp:Literal ID="ltl_TScore" runat="server"></asp:Literal>
                        </td>
                        <td align="right">报名分数：
                        </td>
                        <td>
                            <asp:Literal ID="ltl_SignScore" runat="server"></asp:Literal>
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
                            <asp:Literal ID="ltl_FirstAduitMess" runat="server"></asp:Literal>
                        </td>
                        <td align="right">终审说明：
                        </td>
                        <td>
                            <asp:Literal ID="ltl_LastAduitMess" runat="server"></asp:Literal>
                        </td>
                    </tr>
                    <tr id="tr" runat="server">
                        <td align="right">人员信息：
                        </td>
                        <td colspan="3" style="padding: 0px; margin: 0px;">
                            <table id="tb" width="100%" border="0" cellspacing="0" cellpadding="0" class="border-r">
                                <tbody>
                                    <tr style="text-align: center; color: #508CE4; font-weight: bold;">
                                        <td>报名人员</td>
                                        <td>是否完成</td>
                                        <td>完成日期</td>
                                    </tr>
                                    <asp:Repeater ID="rp_List" runat="server">
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%#Eval("UserName") %></td>
                                                <td align="center"><%#Eval("CompleteDate")==null?"未完成":"已完成" %></td>
                                                <td align="center"><%#Eval("CompleteDate")==null?"": Eval("CompleteDate","{0:yyyy-MM-dd}") %></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">任务内容：
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="ltl_TaskContent" runat="server"></asp:Literal></td>
                    </tr>
                    <tr id="div" runat="server">
                        <td colspan="4" align="center">
                            <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick='$.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
