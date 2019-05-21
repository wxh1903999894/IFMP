﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentManage.aspx.cs" Inherits="IFMP.sysmanage.DepartmentManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />
    <script src="../plugins/layui/layui.js"></script>
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/choice.js"></script>
    <script>
        $(function () {
            $("#btn_Add").click(function () {
                return openbox('A_id', 'DepartmentEdit.aspx', '', 700, 480, -1);
            });
        });

        function editinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'DepartmentEdit.aspx', 'id=' + id , 700, 480, 0);
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
                    <td class="positiona"><a>首页</a><span>></span>系统管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="部门列表"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">部门名称：</td>
                        <td width="200px">
                            <asp:TextBox runat="server" ID="txt_Name" CssClass="layui-input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btn_Search" Text="查询" OnClick="btn_Search_Click" />
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
                            <asp:Button runat="server" ID="btn_Add" CssClass="listbtncss listadd" Text="添加" />
                            <asp:Button runat="server" ID="btn_Delete" CssClass="listbtncss listdel" Text="删除" OnClick="btn_Delete_Click" />
                            <%--<asp:Button runat="server" ID="btn_Import" CssClass="listbtncss listinput" Text="导入" />--%>
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
                        <th>部门名称</th>
                        <th>部门负责人</th>
                        <th>部门描述</th>
                        <th>是否行政部门</th>
                        <th>创建日期</th>
                        <th width="130">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td align="center" style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>
                                <td align="center"><%#GetName(Eval("ID")) %></td>
                                <td align="center"><%#Eval("RealName") %></td>
                                <td align="center"><%#Eval("Description") %></td>
                                <td align="center"><%#Eval("IsAdmin").ToString()=="False"?"否":"是" %></td>
                                <td align="center"><%#Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center">
                                    <asp:LinkButton runat="server" ID="lbtn_Edit" CssClass="listbtn btneditcolor" OnClientClick="return editinfo(this);">编辑</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hf_SysID" Value='<%#Eval("ID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td align="center" colspan="7">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
        <%--<asp:HiddenField runat="server" ID="hf_Flag" />
        <div class="main">
            <div class="page_nav"><span class="layui-breadcrumb"><a href="/">首页</a> <a href="/demo/">人事管理</a> <a><cite>部门管理</cite></a> </span></div>
        </div>
        <div class="layui-collapse" lay-accordion="">
            <asp:Literal runat="server" ID="ltl_Content"></asp:Literal>
            <div class="layui-colla-content layui-show">
                <p>
                    <asp:Button runat="server" ID="btn_Add" CssClass="layui-btn add-btn" Text="添加" />
                </p>
            </div>
        </div>--%>
    </form>
</body>
</html>

