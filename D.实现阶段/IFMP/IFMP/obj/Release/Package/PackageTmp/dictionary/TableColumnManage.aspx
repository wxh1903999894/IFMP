<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableColumnManage.aspx.cs" Inherits="IFMP.dictionary.TableColumnManage" %>

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
            return openbox('A_id', 'TableColumnEdit.aspx', 'type=' + id, 900, 500, -1);
        }

        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'TableColumnDetail.aspx', 'type=' + id, 1100, 560, 1);
        }
        function info(e) {
            var id = $(e).next().val();
            return openbox('A_id', '../taskflow/TaskTableEdit.aspx', 'tabletype=' + id + '&flag=1', 1100, 560, 1);
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
                    <td class="positiona"><a>首页</a><span>></span>日常生产<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="表单管理"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
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
                        <th>表单类型</th>
                        <th>是否多填</th>
                        <th width="160" align="center">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <%--<td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>--%>
                                <td align="center"><%#Eval("name") %></td>
                                <td align="center"><%#Eval("IsMulti").ToString()=="True"?"是":"否" %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="字段添加" OnClientClick='return editinfo(this);'>字段添加</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btneditcolor" ToolTip="字段查看" OnClientClick='return viewinfo(this);'>字段查看</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hf_SysID" Value='<%#Eval("id") %>' />
                                    <asp:LinkButton ID="lbtn_PreView" runat="server" CssClass="listbtn btneditcolor" ToolTip="表单预览" OnClientClick='return info(this);'>表单预览</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="HiddenField1" Value='<%#Eval("id") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="5">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <%--<wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />--%>
    </form>
</body>
</html>

