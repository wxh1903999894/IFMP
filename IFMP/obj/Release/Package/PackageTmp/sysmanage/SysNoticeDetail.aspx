<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysNoticeDetail.aspx.cs" Inherits="IFMP.sysmanage.SysNoticeDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_OldPwd" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">通知详细消息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">通知人</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_SendUser"></asp:Literal>
                        </td>
                        <td align="right" width="100px">通知日期</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_SendDate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4"><asp:Literal runat="server" ID="ltl_Contenet"></asp:Literal></td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

