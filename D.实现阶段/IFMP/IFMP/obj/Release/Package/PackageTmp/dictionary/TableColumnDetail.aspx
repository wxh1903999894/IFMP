<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableColumnDetail.aspx.cs" Inherits="IFMP.dictionary.TableColumnDetail" %>

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
    <script src="../plugins/Common.js"></script>
    <script>
        function editinfo(e) {
            var id = $(e).next().next().val();
            
            return parent.openbox('C_id', 'TableColumnEdit.aspx', 'type=' + unity.getURL("type") + "&id=" + id, 900, 400, 0);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_CheckIDS" />
        <%--<div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">表名称：
                        </td>
                        <td width="180px">
                            <asp:TextBox runat="server" ID="txt_Name"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" OnClick="btn_Search_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>--%>
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
                        <th>字段名称</th>
                        <th>合法字典</th>
                        <th>提示字典</th>
                        <th>默认文字</th>
                        <th>是否作为统计字段</th>
                        <th>排序</th>
                        <th width="160" align="center">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <%--<td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>--%>
                                <td align="center"><%#Eval("ColumnName") %></td>
                                <td align="center"><%#Eval("DictName") %></td>
                                <td align="center"><%#Eval("HintName") %></td>
                                <td align="center"><%#Eval("DefaultData") %></td>
                                <td align="center"><%#Eval("IsStats").ToString()=="False"?"否":"是" %></td>
                                <td align="center"><%#Eval("Order") %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick='return editinfo(this);'>编辑</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Delete" runat="server" CssClass="listbtn btneditcolor" ToolTip="删除" OnClientClick='return confirm("确定删除信息吗？")' CommandArgument='<%#Eval("ID") %>' OnClick="lbtn_Delete_Click">删除</asp:LinkButton>
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
        <%--<wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />--%>
    </form>
</body>
</html>
