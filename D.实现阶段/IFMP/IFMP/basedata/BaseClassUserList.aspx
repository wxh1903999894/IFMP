﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseClassUserList.aspx.cs" Inherits="IFMP.basedata.BaseClassUserList" %>

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
                return openbox('A_id', 'BaseClassEdit.aspx', '', 900, 300, -1);
            });
        });

        function setuser(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'BaseClassTimeSet.aspx', 'id=' + id, 900, 300, 0);
        }

        function editinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'BaseClassEdit.aspx', 'id=' + id, 900, 300, 0);
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
                    <td class="positiona"><a>首页</a><span>></span>基础数据管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="基础班次人员设置"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td>表单类型：
                            <asp:DropDownList ID="ddl_TableType" CssClass="searchbg" datatype="ddl" errormsg="请选择表单类型" runat="server"></asp:DropDownList>
                        </td>

                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" />
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
                            <input type="checkbox" name="checkbox" id="checkall" onclick="qx(this.name, this.id)" /></th>
                        <th><strong>表单名称</strong></th>
                        <th><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox1" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' />
                                    </label>
                                </td>
                                <td title="<%#Eval("Name") %>"><%# Eval("Name")%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick="return editinfo(this);">编辑</asp:LinkButton>
                                    <asp:HiddenField ID="hf_SelectID" Value='<%#Eval("ID") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="8">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>

