<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="IFMP.integration.TaskList" %>

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
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        $(function () {
            document.cookie = name + "ScreenH=" + screen.height;
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'TaskEdit.aspx', '', 900, 500, -1);
            });
        });

        function editinfo(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'TaskEdit.aspx', 'id=' + id, 1100, 500, 0);
        }

        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'RewardTaskDetail.aspx', 'id=' + id, 860, 420, 1);
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
                    <td class="positiona"><a>首页</a><span>></span>积分管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="任务发布"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td width="80px" align="right">任务名称：</td>
                        <td width="180px">
                            <asp:TextBox ID="txt_TaskName" runat="server" CssClass="searchbg"></asp:TextBox>
                        </td>
                        <td width="100px" align="right">汇报截止日期：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td width="80px" align="right">审核状态：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_AduitState" CssClass="searchbg" datatype="ddl" errormsg="请选择审核状态" runat="server"></asp:DropDownList>
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
                            <asp:Button ID="btn_Add" runat="server" CssClass="listbtncss listadd" Text="添加" />
                            <asp:Button ID="btn_Delete" runat="server" CssClass="listbtncss listdel" Text="删除" OnClick="btn_Delete_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th style="width: 5px">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" id="checkall" onclick="qx(this.name, this.id)" />
                            </label>
                        </th>
                        <th><strong>任务名称</strong></th>
                        <th><strong>悬赏分数</strong></th>
                        <th><strong>报名分数</strong></th>
                        <th><strong>汇报截止日期</strong></th>
                        <th><strong>审核状态</strong></th>
                        <th><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox1" onclick="setid(this.id)" id='ck_<%#Eval("ID") %>' value='<%#Eval("ID") %>' <%#Eval("IsDel").ToString()!="1"?"disabled":"" %> />
                                    </label>
                                </td>
                                <td title="<%#Eval("Name") %>"><%#GetCutStr( Eval("Name"),15)%></td>
                                <td><%#Eval("CompleteBScore")%></td>
                                <td><%#Eval("SignBScore")%></td>
                                <td><%#Eval("EndDate","{0:yyyy-MM-dd}")%></td>
                                <td><%# Enum.GetName(typeof(IFMPLibrary.Enums.AuditState),Eval("AuditState"))%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" Visible='<%#(Eval("AuditState").ToString()==((int)IFMPLibrary.Enums.AuditState.待初审).ToString()||Eval("AuditState").ToString()==((int)IFMPLibrary.Enums.AuditState.待终审).ToString())?true:false%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick="return editinfo(this);">编辑</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField ID="hf_SelectID" Value='<%#Eval("ID") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="15">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>

