<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DepartmentEdit.aspx.cs" Inherits="IFMP.sysmanage.DepartmentEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
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
                        <th colspan="2" align="left">部门信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100">上级部门</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_PID" AutoPostBack="true" OnSelectedIndexChanged="btn_Department_Change"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">部门名称</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_DepName" datatype="*" nullmsg="请填写部门名称"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">负责人</td>
                        <td>
                            <asp:TextBox ID="txt_Master" cascadeCheck="false" runat="server" name="txt_Master" onlyLeafCheck="true" url="../ashx/GetBaseDate.ashx?method=GetUserTxt&deptype=1" CssClass="easyui-combotree"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">排序</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_DepOrder"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">部门简述</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_DepMark" TextMode="MultiLine" Height="70px" Width="60%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">部门类型</td>
                        <td>
                            <asp:RadioButtonList ID="rbol_MState" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                                <asp:ListItem Selected="" Value="1">职能部门</asp:ListItem>
                                <asp:ListItem Value="0">积分制分组</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button runat="server" ID="btn_Submit" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
