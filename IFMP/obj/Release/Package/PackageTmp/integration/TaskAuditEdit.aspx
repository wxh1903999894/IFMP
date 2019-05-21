<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskAuditEdit.aspx.cs" Inherits="IFMP.integration.TaskAuditEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <td align="right" width="200px">审核状态：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Audit" runat="server" CssClass="searchbg"></asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="right">说明：</td>
                    <td>
                        <asp:TextBox ID="txt_EventMark" runat="server" Rows="6" Height="100px" Width="98%" Style="resize: none;" TextMode="MultiLine" CssClass="searchbg"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
