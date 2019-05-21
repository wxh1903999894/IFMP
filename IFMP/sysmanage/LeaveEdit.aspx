<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveEdit.aspx.cs" Inherits="IFMP.sysmanage.LeaveEdit" %>

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
                        <th colspan="4" align="left">请假申请</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">开始日期：</td>
                        <td align="left">
                            <asp:TextBox runat="server" EnableViewState="false" AutoCompleteType="None" ID="txt_BeginDate" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right" width="100px">结束日期：</td>
                        <td align="left">
                            <asp:TextBox runat="server" EnableViewState="false" AutoCompleteType="None" ID="txt_EndDate" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">请假天数：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_Day" datatype="zeronum" nullmsg="请填写请假天数"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">请假类型：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_LeaveType" datatype="ddl" errormsg="请选择请假类型"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>

                    <tr id="LeaveAuditUser">
                        <td align="right">请假审核人员选择：
                        </td>
                        <td align="left" colspan="3">
                            <asp:DropDownList runat="server" ID="ddl_AuditUser" datatype="ddl" errormsg="请选择审核人员"></asp:DropDownList>
                        </td>
                    </tr>

                    <tr>
                        <td align="right">请假原因描述：
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txt_Content" TextMode="MultiLine" Width="60%" Height="80px"></asp:TextBox>
                        </td>
                    </tr>


                    <tr>
                        <td align="right">请假提示：
                        </td>
                        <td colspan="3">
                            <asp:Literal runat="server" ID="ltl_Censusaddr">
                                 1.后勤人员事假、差假超过7天，必须由总经理审批通过方可生效，否则一律按旷工处理。<br/>
                                 2.车间人员事假、差假超过7天，必须由主管班长、车间主任、生产副总审批通过方可生效。<br/>
                                 3.请假7天以内,需要所属部门上级审批通过。
                            </asp:Literal>
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
<script>

    jQuery(document).ready(function () {
        jQuery("#LeaveAuditUser").hide();
    })

    jQuery("#txt_Day").blur(function () {
        if (parseFloat(jQuery("#txt_Day").val()) > 7 ) {
            jQuery("#LeaveAuditUser").show();
        } else {
            jQuery("#LeaveAuditUser").hide();
        }
    });
</script>
