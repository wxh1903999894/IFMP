<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RoleSelete.aspx.cs" Inherits="IFMP.sysmanage.RoleSelete" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        function getvalue() {
            var flag = document.getElementById("hf_Flag").value;
            var ids = document.getElementById("hf_CheckIDS").value;
            var names = document.getElementById("hf_CheckNames").value;

            if (ids == "" || ids == null) {
                alert("请至少选择一个角色");
                return false;
            }

            if (flag == 1) {//积分奖扣添加页面选人
                $.opener("A_id").say(ids);
                $.close("S_id");
            }
            else {
                $.opener("A_id").document.getElementById("hf_RoleID").value = ids;
                $.opener("A_id").document.getElementById("txt_Role").value = names;
            }

            $.close("S_id");
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_Flag" runat="server" />
        <asp:HiddenField ID="hf_CheckIDS" runat="server" />
        <asp:HiddenField ID="hf_CheckNames" runat="server" />
        <asp:HiddenField ID="hf_Page" runat="server" />
        <asp:HiddenField ID="hf_ObjID" runat="server" />
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td width="50px" align="right">名称：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Name"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" CssClass="btn" Text="查询" OnClick="btn_Query_Click" />&nbsp;&nbsp;
                            <asp:Button ID="btn_OK" runat="server" CssClass="btn" Text="确认" OnClientClick="return getvalue();" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th width="5%" align="center">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" value="复选框" id="checkall" onclick="qx(this.name, this.id)" />
                            </label>
                        </th>
                        <th><strong>权限名称</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setidandname(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' />
                                        <input type="hidden" id="name_<%#Eval("ID") %>" value="<%#Eval("Name")%>" />
                                        <%--<asp:HiddenField ID="name_<%#Eval("ID") %>" Value="<%#Eval("RealName")%>" runat="server" />--%>
                                    </label>
                                </td>
                                <td><%#Eval("Name")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="3">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>

