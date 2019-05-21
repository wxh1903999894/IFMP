<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeManage.aspx.cs" Inherits="IFMP.sysmanage.EmployeeManage" %>

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
            var id = $(e).next().val();
            return openbox('A_id', 'EmployeeEdit.aspx', 'id=' + id, 1000, 630, 0);
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
                    <td class="positiona"><a>首页</a><span>></span>人事管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="档案管理"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="60px">部门：
                        </td>
                        <td width="100px">
                            <asp:DropDownList runat="server" ID="ddl_DepartmentID"></asp:DropDownList>
                        </td>
                        <td align="right" width="60px">姓名：</td>
                        <td width="190">
                            <asp:TextBox runat="server" ID="txt_RealName"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">员工分类：</td>
                        <td width="100px">
                            <asp:DropDownList runat="server" ID="ddl_UserType"></asp:DropDownList>
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
                            <asp:Button runat="server" ID="btn_OutPut" CssClass="listbtncss listoutput" Text="导出" OnClick="btn_OutPut_Click" />
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
                        <th>员工编号</th>
                        <th>姓名</th>
                        <th>性别</th>
                        <th>生日</th>
                        <th>部门</th>
                        <th>岗位</th>
                        <th>职务</th>
                        <th>员工分类</th>
                        <th>入职日期</th>
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
                                <td align="center"><%#Eval("UserNumber") %></td>
                                <td align="center"><%#Eval("RealName") %></td>
                                <td align="center"><%#Eval("Sex") %></td>
                                <td align="center"><%#Eval("BirthDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center"><%#Eval("DepName") %></td>
                                <td align="center"><%#Eval("PostName") %></td>
                                <td align="center"><%#Eval("Job") %></td>
                                <td align="center"><%# Enum.GetName(typeof(IFMPLibrary.Enums.UserType),Eval("UserType")) %></td>
                                <td align="center"><%#Eval("HireDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center"><%# Enum.GetName(typeof(IFMPLibrary.Enums.UserState),Eval("UserState")) %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick='return editinfo(this);'>编辑</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hf_SysID" Value='<%#Eval("ID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="11">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>
