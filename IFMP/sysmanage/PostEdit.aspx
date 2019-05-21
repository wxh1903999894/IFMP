<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PostEdit.aspx.cs" Inherits="IFMP.sysmanage.PostEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../plugins/editinfor.js"></script>
    <script>
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
                        <th colspan="2" align="left">岗位信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100">岗位名称：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Name" datatype="*" nullmsg="请填写岗位名称"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="100">排序：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Order" datatype="*" nullmsg="请填写排序信息"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="100">岗位简述：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Description" TextMode="MultiLine" Width="60%" Height="90"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button runat="server" ID="btn_Submit" lay-submit="" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
