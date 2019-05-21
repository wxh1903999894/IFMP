<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseFlowRoleEdit.aspx.cs" Inherits="IFMP.basedata.BaseFlowRoleEdit" %>

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

        function showbox() {
            return parent.openbox('S_id', '../sysmanage/RoleSelete.aspx', "", 1100, 585, 8);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_tflag" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <td align="right" width="100px">表单类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_TableType"  OnSelectedIndexChanged="ddl_TableTypeChanged" AutoPostBack="true" datatype="ddl" errormsg="请选择表单类型" runat="server">
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">流程：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Flow" datatype="ddl" errormsg="请选择流程" runat="server">
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">权限：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Role" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hf_RoleID" runat="server" />
                        <img src="../images/checkbtn.gif" id="btn_plancom" style="margin-top: 12px;" onclick="showbox()" />
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>


