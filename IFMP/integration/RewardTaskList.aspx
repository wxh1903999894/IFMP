<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RewardTaskList.aspx.cs" Inherits="IFMP.integration.RewardTaskList" %>

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
            $("#btn_Compelete").click(function () {
                var checkid = document.getElementById("hf_CheckIDS").value;
                var count = 0;
                var strs = new Array();
                strs = checkid.split(",");
                for (i = 0; i < strs.length; i++) {
                    if (strs[i] != "") {
                        count++;
                        break;
                    }
                }
                if (count == 0) {
                    alert("系统提示：请至少选择一条信息！");
                    return false;
                }
                else {
                    return confirm("您确认完成选择中的任务吗？");
                }
            });
        });
        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'RewardTaskDetail.aspx', 'id=' + id, 1000, 520, 1);
        }
        function robinfo(e) {
            var id = $(e).next().next().next().val();
            return openbox('A_id', 'RewardTaskDetail.aspx', 'id=' + id + "&flag=1", 1000, 520, 83);
        }
        function compinfo(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'RewardTaskDetail.aspx', 'id=' + id + "&flag=2", 1000, 520, 60);
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
                    <td class="positiona"><a>首页</a><span>></span>积分管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="任务大厅"></asp:Label>
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
                            <asp:TextBox ID="txt_TaskName" runat="server"></asp:TextBox>
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
                        <td>
                            <asp:Button ID="btn_Search" OnClick="btn_Query_Click" runat="server" Text="查询" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listbt">
                <tbody>
                    <tr>
                        <td align="left"></td>
                        <td align="right" valign="middle">
                            <asp:Button ID="btn_Compelete" runat="server" OnClick="btn_Compelete_Click" CssClass="listbtncss listno" Text="确认完成" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th width="5%" align="center">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" value="复选框" id="checkall" onclick="qx(this.name, this.id)" />
                            </label>
                        </th>
                        <th><strong>任务名称</strong></th>
                        <th><strong>责任人</strong></th>
                        <th><strong>悬赏分数</strong></th>
                        <th><strong>报名分数</strong></th>
                        <th><strong>汇报截止日期</strong></th>
                        <th width="200px"><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' <%#Eval("IsSelf").ToString().ToLower()=="true"?"":"disabled" %> /></label>
                                </td>
                                <td title="<%#Eval("Name")%>"><%#Eval("Name")%></td>
                                <td><%#Eval("UserName")%></td>
                                <td><%#Eval("CompleteBScore")%></td>
                                <td><%#Eval("SignBScore")%></td>
                                <td><%#Eval("EndDate","{0:yyyy-MM-dd}")%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Robbing" runat="server" CssClass="listbtn btndetialcolor" Visible='<%#Eval("AuditState").ToString()==IFMPLibrary.Enums.AuditState.通过.ToString()?true:false%>' ToolTip="抢单" OnClientClick='return robinfo(this);'>抢单</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Complete" runat="server" CssClass="listbtn btndetialcolor" Visible='<%#Eval("AuditState").ToString()==IFMPLibrary.Enums.AuditState.通过.ToString()?true:false%>' ToolTip="完成" OnClientClick='return compinfo(this);'>完成</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField ID="hf_SelectID" Value='<%#Eval("ID") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="14">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>
