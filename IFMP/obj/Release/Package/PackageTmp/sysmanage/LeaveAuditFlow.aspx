<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveAuditFlow.aspx.cs" Inherits="IFMP.sysmanage.LeaveAuditFlow" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_Type" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">请假审核</th>
                    </tr>
                    <tr>
                        <td align="right" width="130px">请假人：</td>
                        <td align="left" colspan="3">
                            <asp:Literal runat="server" ID="ltl_LeaveUser"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">开始日期：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_BeginDate"></asp:Literal>
                        </td>
                        <td align="right" width="100px">结束日期：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_EndDate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">请假天数：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_Day"></asp:Literal>
                        </td>
                        <td align="right">请假类型：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_LeaveType"></asp:Literal>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">请假原因描述：
                        </td>
                        <td colspan="3">
                            <asp:Literal runat="server" ID="ltl_Content"></asp:Literal>
                        </td>
                    </tr>

                    <tr runat="server" id="LeaveAuditUser">
                        <td align="right">请假审核人员选择：
                        </td>
                        <td align="left" colspan="3">
                            <asp:DropDownList runat="server" ID="ddl_AuditUser" datatype="ddl" errormsg="请选择审核人员"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">审核状态：
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddl_Audit" runat="server" CssClass="searchbg"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">说明：</td>
                        <td colspan="3">
                            <asp:TextBox ID="txt_EventMark" runat="server" Rows="6" Height="100px" Width="98%" Style="resize: none;" TextMode="MultiLine" CssClass="searchbg"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button runat="server" ID="btn_Submit" lay-submit="" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

