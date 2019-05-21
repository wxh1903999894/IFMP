<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskSetEdit.aspx.cs" Inherits="IFMP.taskflow.TaskSetEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
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
                        <th colspan="4" align="left">任务设定信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">任务名称：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_TaskName" datatype="*" nullmsg="请填写任务名称" AutoCompleteType="Disabled"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right" width="100px">班次类型：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_ClassType" datatype="ddl" errormsg="请选择班次信息" OnSelectedIndexChanged="ddl_ClassType_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">表单类型：</td>
                        <td align="left" colspan="3">
                            <%--<asp:DropDownList runat="server" ID="ddl_TableType" datatype="ddl" errormsg="请选择表单类型信息"></asp:DropDownList>--%>
                            <asp:CheckBoxList runat="server" ID="cbl_TableType" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td align="right">基础班次选择：</td>
                        <td colspan="3">
                            <asp:CheckBoxList runat="server" ID="cbl_BaseClassID" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td align="right">适用星期：</td>
                        <td colspan="3">
                            <asp:CheckBoxList runat="server" ID="ck_Weeks" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow">
                                <asp:ListItem Value="1">星期一</asp:ListItem>
                                <asp:ListItem Value="2">星期二</asp:ListItem>
                                <asp:ListItem Value="3">星期三</asp:ListItem>
                                <asp:ListItem Value="4">星期四</asp:ListItem>
                                <asp:ListItem Value="5">星期五</asp:ListItem>
                                <asp:ListItem Value="6">星期六</asp:ListItem>
                                <asp:ListItem Value="7">星期天</asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button runat="server" ID="btn_Submit" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>


