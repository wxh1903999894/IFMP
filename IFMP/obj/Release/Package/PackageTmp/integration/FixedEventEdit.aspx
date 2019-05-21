<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FixedEventEdit.aspx.cs" Inherits="IFMP.integration.FixedEventEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../js/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../js/editinfor.js" type="text/javascript"></script>
    <script src="../js/Validform_v5.3.2.js" type="text/javascript"></script>
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
                <tbody>
                    <tr>
                        <th colspan="4" align="left">固定事件信息</th>
                    </tr>
                    <tr>
                        <td align="right">名称：</td>
                        <td>
                            <asp:TextBox ID="txt_DataName" runat="server" datatype="*1-100"
                                nullmsg="请填写名称" CssClass="searchbg" Width="200"></asp:TextBox>
                            <span style="color: Red">*</span></td>
                    </tr>
                    <tr>
                        <td align="right">备注：</td>
                        <td>
                            <asp:TextBox ID="txt_Desc" runat="server" CssClass="searchbg" Width="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                            <input type="button" name="button" id="cancell" value="返回" class="editor" onclick='javascript: window.history.back(-1);' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
