<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskAuditList.aspx.cs" Inherits="IFMP.integration.TaskAuditList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />

    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        $(function () {
            document.cookie = name + "ScreenH=" + screen.height;
        });
        function auditinfo(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'TaskAuditEdit.aspx', 'id=' + id, 860, 260, 33);

        }
        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'RewardTaskDetail.aspx', 'id=' + id, 860, 420, 1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>积分管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="任务审核"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">任务名称：</td>
                        <td width="180px">
                            <asp:TextBox ID="txt_TaskName" runat="server" CssClass="searchbg"></asp:TextBox>
                        </td>
                        <td align="right" width="60px">责任人：</td>
                        <td width="180px">
                            <asp:TextBox ID="txt_TaskUser" runat="server" CssClass="searchbg"></asp:TextBox>
                        </td>
                        <td align="right" width="100px">汇报截止日期：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">审核状态：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_AduitState" CssClass="searchbg" datatype="ddl" errormsg="请选择审核状态" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" OnClick="btn_Query_Click" runat="server" CssClass="btn" Text="查询" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th><strong>任务名称</strong></th>
                        <th><strong>责任人</strong></th>
                        <th><strong>悬赏分数</strong></th>
                        <th><strong>报名分数</strong></th>
                        <th><strong>汇报截止日期</strong></th>
                        <th><strong>审核状态</strong></th>
                        <th width="120px"><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td title="<%#Eval("Name") %>"><%#Eval("Name")%></td>
                                <td><%#Eval("UserName")%></td>
                                <td><%#Eval("CompleteBScore")%></td>
                                <td><%#Eval("SignBScore")%></td>
                                <td><%#Eval("EndDate","{0:yyyy-MM-dd}")%></td>
                                <td><%#Enum.GetName(typeof(IFMPLibrary.Enums.AuditState), Eval("AuditState"))%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" Visible='<%#(Eval("AuditState").ToString()==IFMPLibrary.Enums.AuditState.待初审.ToString()||Eval("AuditState").ToString()==IFMPLibrary.Enums.AuditState.待终审.ToString())?true:false%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="审核" OnClientClick="return auditinfo(this);">审核</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField ID="hf_SelectID" Value='<%#Eval("ID") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="7">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>
