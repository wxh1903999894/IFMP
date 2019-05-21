<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysUserManage.aspx.cs" Inherits="IFMP.sysmanage.SysUserManage" %>

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
        $(function () {
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'SysUserEdit.aspx', '', 1000, 630, -1);
            });

            $('#btn_Import').click(function () {
                return openbox('A_id', 'SysUserImport.aspx', '', 720, 400, -1);
            });
        });

        function editinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'SysUserEdit.aspx', 'id=' + id, 1000, 630, 0);
        }

        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'EmployeeEdit.aspx', 'id=' + id, 860, 520, 1);
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
                    <td class="positiona"><a>首页</a><span>></span>系统管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="用户列表"></asp:Label>
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
                        <td width="200px">
                            <asp:TextBox runat="server" ID="txt_RealName"></asp:TextBox>
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
                            <asp:Button ID="btn_Add" runat="server" CssClass="listbtncss listadd" Text="添加" />
                            <asp:Button ID="btn_Delete" runat="server" CssClass="listbtncss listdel" Text="删除" OnClick="btn_Delete_Click" />
                            <asp:Button runat="server" ID="btn_OutPut" CssClass="listbtncss listoutput" Text="导出" OnClick="btn_OutPut_Click" />
                            <asp:Button runat="server" ID="btn_Import" CssClass="listbtncss listinput" Text="导入" />
                            <asp:Button runat="server" ID="btn_PwdReset" CssClass="listbtncss listpassword" Text="密码重置" OnClick="btn_PwdReset_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th width="5%" align="center">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" value="复选框" id="checkall" onclick="qx(this.name, this.id)" /></label></th>
                        <th>用户名</th>
                        <th>姓名</th>
                        <th>手机号码</th>
                        <th>身份证号</th>
                        <th>地址</th>
                        <th>状态</th>
                        <th width="130" align="center">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>
                                <td align="center"><%#Eval("UserName") %></td>
                                <td align="center"><%#Eval("RealName") %></td>
                                <td align="center"><%#Eval("CellPhone") %></td>
                                <td align="center"><%#Eval("Identity") %></td>
                                <td align="center"><%#Eval("Address") %></td>
                                <td align="center"><%# Enum.GetName(typeof(IFMPLibrary.Enums.UserState),Eval("UserState")) %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick='return editinfo(this);'>编辑</asp:LinkButton>
                                    <%--<asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="员工档案" OnClientClick='return viewinfo(this);'>员工档案</asp:LinkButton>--%>
                                    <asp:HiddenField runat="server" ID="hf_SysID" Value='<%#Eval("ID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="7">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>


