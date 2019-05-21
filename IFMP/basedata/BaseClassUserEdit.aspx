<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseClassUserEdit.aspx.cs" Inherits="IFMP.basedata.BaseClassUserEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_tflag" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <td align="right">表单类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TableType" AutoPostBack="true" OnSelectedIndexChanged="ddl_TableType_Change" datatype="ddl" errormsg="请选择表单类型" runat="server">
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>

                <%--构建当前流程下的人员设置--%>
                <tr>
                    <td colspan="4" align="left">流程人员选择：
                    </td>
                </tr>

                <asp:Repeater runat="server" ID="rp_List" OnItemDataBound="Repeater_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td align="right"><%#Eval("Name") %>
                            </td>
                            <td>
                                <asp:HiddenField ID="hf_FlowID" Value='<%#Eval("ID")%>' runat="server" />
                                <asp:DropDownList datatype="ddl" ID="ddl_UserList" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>


                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>


