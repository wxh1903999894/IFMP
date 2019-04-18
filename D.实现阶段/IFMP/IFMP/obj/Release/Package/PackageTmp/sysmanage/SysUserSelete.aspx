<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysUserSelete.aspx.cs" Inherits="IFMP.sysmanage.SysUserSelete" %>

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
                alert("请至少选择一名用户");
                return false;
            }

            if (flag == 1) {//积分奖扣添加页面选人
                $.opener("A_id").say(ids);
                $.close("S_id");
            }
            else {
                $.opener("A_id").document.getElementById("hf_CID").value = ids;
                $.opener("A_id").document.getElementById("txt_SysID").value = names;
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
                        <td width="50px" align="right">部门：</td>
                        <td>
                            <asp:DropDownList ID="ddl_Department" runat="server"></asp:DropDownList>
                        </td>
                        <td width="50px" align="right">姓名：</td>
                        <td width="180px">
                            <asp:TextBox ID="txt_RealName" runat="server" CssClass="searchbg"></asp:TextBox>
                        </td>
                        <td width="80px" align="right">入职日期：</td>
                        <td>
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>至  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
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
                        <th><strong>用户名</strong></th>
                        <th><strong>姓名</strong></th>
                        <th><strong>部门</strong></th>
                        <th><strong>职务</strong></th>
                        <th><strong>入职日期</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setidandname(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' />
                                        <input type="hidden" id="name_<%#Eval("ID") %>" value="<%#Eval("RealName")%>" />
                                        <%--<asp:HiddenField ID="name_<%#Eval("ID") %>" Value="<%#Eval("RealName")%>" runat="server" />--%>
                                    </label>
                                </td>
                                <td><%#Eval("UserName")%></td>
                                <td><%#Eval("RealName")%></td>
                                <td><%#Eval("DepartmentName")%></td>
                                <td><%#Eval("PostName")%></td>
                                <td><%#Eval("HireDate","{0:yyyy-MM-dd}")%></td>
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

