<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveStatistics.aspx.cs" Inherits="IFMP.sysmanage.LeaveStatistics" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧工厂管理平台</title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />

    <script src="../plugins/layui/layui.js"></script>
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>

    <script>
        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'LeaveDetail.aspx', 'id=' + id, 1000, 520, 0);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_CheckIDS" />
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>人事管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="请假统计"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">请假人：</td>
                        <td width="100px">
                            <asp:TextBox runat="server" ID="txt_Name" CssClass="layui-input"></asp:TextBox>
                        </td>                   
                        <td width="80px" align="right">请假日期：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" OnClick="btn_Search_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th>请假人</th>
                        <asp:Repeater runat="server" ID="rp_LeaveType">
                            <ItemTemplate>
                                <th><%#Eval("Name")%></th>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List" OnItemDataBound="rp_Count_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td align="center">
                                    <%#Eval("RealName") %>                                   
                                </td>

                                <asp:Repeater runat="server" ID="rp_Count">
                                    <ItemTemplate>
                                        <td align="center"><%#Eval("Count") %></td>
                                    </ItemTemplate>
                                </asp:Repeater>

                                 <asp:HiddenField ID="hf_ID" Value='<%#Eval("ID")%>' runat="server" />
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="9">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>


