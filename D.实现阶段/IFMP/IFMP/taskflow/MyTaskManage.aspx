<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyTaskManage.aspx.cs" Inherits="IFMP.taskflow.MyTaskManage" %>

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
    <script>
        function editinfo(e) {
            var id = $(e).next().next().val();
            var taskid = $(e).next().next().next().val();
            var taskflowid = $(e).next().next().next().next().val();
            var flowid = $(e).next().next().next().next().next().val();
            var isaudit = $(e).next().next().next().next().next().next().val();
            if (isaudit == "True") {
                return openbox('A_id', 'TaskAuditEdit.aspx', 'taskid=' + taskid + '&taskflowid=' + taskflowid, 1060, 580, 34);
            }
            else {
                return openbox('A_id', 'TaskTableEdit.aspx', 'tabletype=' + id + '&taskid=' + taskid + '&taskflowid=' + taskflowid + '&flowid=' + flowid, 1000, 630, 0);
            }
        }

        function viewinfo(e) {
            var taskid = $(e).next().next().val();
            var taskflowid = $(e).next().next().next().val();
            return openbox('A_id', 'MyTaskDetail.aspx', 'taskid=' + taskid + '&taskflowid=' + taskflowid, 1060, 580, 1);
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
                    <td class="positiona"><a>首页</a><span>></span>常用操作<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="我的任务"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="100px">任务名称：
                        </td>
                        <td width="180px">
                            <asp:TextBox runat="server" ID="txt_TaskName"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">班次类型：</td>
                        <td width="100px">
                            <asp:DropDownList runat="server" ID="ddl_ClassType"></asp:DropDownList>
                        </td>
                        <td align="right" width="100px">表单类型：</td>
                        <td width="100px">
                            <asp:DropDownList runat="server" ID="ddl_TableType"></asp:DropDownList>
                        </td>
                        <td align="right" width="60px">状态：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_ApplyType"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" OnClick="btn_Search_Click" />
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
                            <%--<asp:Button ID="btn_Add" runat="server" CssClass="listbtncss listadd" Text="添加" />
                            <asp:Button ID="btn_Delete" runat="server" CssClass="listbtncss listdel" Text="删除" OnClick="btn_Delete_Click" />--%>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <%--<th width="5%" align="center">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" value="复选框" id="checkall" onclick="qx(this.name, this.id)" /></label></th>--%>
                        <th>任务名称</th>
                        <th>班次类型</th>
                        <th>表单类型</th>
                        <th>流程名称</th>
                        <th>开始时间</th>
                        <th>结束时间</th>
                        <th>状态</th>
                        <th width="130" align="center">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <%--<td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>--%>
                                <td align="center"><%#Eval("TaskName") %></td>
                                <td align="center"><%#Eval("ClassType") %></td>
                                <td align="center"><%#Eval("TableTypeName") %></td>
                                <td align="center"><%#Eval("Name") %></td>
                                <td align="center"><%#Eval("BeginDate","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                <td align="center"><%#Eval("EndDate","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                <td align="center"><%#Eval("ApplyTypeName") %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" Visible='<%#Eval("IsVis").ToString()=="True"?true:false %>' ToolTip="填写" OnClientClick='return editinfo(this);'><%#Eval("IsAudit").ToString()=="True"?"审核":"填写" %></asp:LinkButton>
                                    <%--<asp:LinkButton ID="lbtn_Audit" runat="server" CssClass="listbtn btndetialcolor" Visible='<%#Eval("IsAudit").ToString()=="False"?false:(Eval("ApplyType").ToString()=="未交"?true:false) %>' ToolTip="审核" OnClientClick='return auditinfo(this);'>审核</asp:LinkButton>--%>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hf_SysID" Value='<%#Eval("TableTypeID") %>' />
                                    <asp:HiddenField runat="server" ID="hf_TaskID" Value='<%#Eval("TaskID") %>' />
                                    <asp:HiddenField runat="server" ID="hf_TaskFlowID" Value='<%#Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hf_FlowID" Value='<%#Eval("FlowID") %>' />
                                    <asp:HiddenField runat="server" ID="hf_IsAudit" Value='<%#Eval("IsAudit") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="8">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>

