<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PersonAuditList.aspx.cs" Inherits="IFMP.integration.PersonAuditList" %>

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
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">

        $(function () {
            document.cookie = name + "ScreenH=" + screen.height;
        });

        function Audit(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'PersonAuditEdit.aspx', 'id=' + id, 860, 340, 34);

        }

        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'PersonAuditDetail.aspx', 'id=' + id, 1060, 600, 1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_CheckIDS" runat="server" />
        <asp:HiddenField ID="hf_Page" runat="server" />
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>常用操作<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="积分审核"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="60px">事件：</td>
                        <td width="220px">
                            <asp:TextBox ID="txt_EventName" runat="server" CssClass="searchbg" Width="180px"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">奖扣日期：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">审核状态：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_AduitState" CssClass="searchbg" datatype="ddl" errormsg="请选择审核状态" runat="server"></asp:DropDownList>
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
                        <th><strong>奖扣日期</strong></th>
                        <th><strong>主题</strong></th>
                        <th><strong>事件</strong></th>
                        <th><strong>记录人</strong></th>
                        <th><strong>审核状态</strong></th>
                        <th><strong>事件说明</strong></th>
                        <th width="130px"><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("CreateDate","{0:yyyy-MM-dd}")%></td>
                                <td><%#Eval("Title")%></td>
                                <td><%#Eval("EventName")%></td>
                                <td><%#Eval("RecordUserName")%></td>
                                <td><%#Enum.GetName(typeof(IFMPLibrary.Enums.AuditState), Eval("AuditState"))%></td>
                                <td title="<%#Eval("Content") %>"><%#Eval("Content")%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" Visible='<%#(Eval("AuditState").ToString()=="待初审"||Eval("AuditState").ToString()=="待终审")?true:false%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="审核" CommandArgument='<%#Eval("ID")%>' OnClientClick='return Audit(this);'>审核</asp:LinkButton>

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
