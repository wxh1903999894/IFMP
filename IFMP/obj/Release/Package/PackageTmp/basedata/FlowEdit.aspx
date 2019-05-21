<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FlowEdit.aspx.cs" Inherits="IFMP.basedata.FlowEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/AddOption.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
    <style>
        .edilab label {
            float: none;
        }

        .edilab input {
            height: 13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">表单流程设置信息</th>
                    </tr>

                    <tr>
                        <td align="right" width="120px">流程名称：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_FlowName" datatype="*" nullmsg="请填写流程名称"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>

                        <td align="right">上级流程：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_ParentFlow"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">是否作为审核流程：</td>
                        <td colspan="3">
                            <asp:RadioButtonList runat="server" ID="rdo_IsAudit" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                                <asp:ListItem Selected="True" Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button runat="server" ID="btn_Submit" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
