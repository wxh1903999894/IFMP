<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskEdit.aspx.cs" Inherits="IFMP.taskflow.TaskEdit" %>

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
                        <th colspan="4" align="left">任务信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">任务名称：</td>
                        <td align="left" colspan="3">
                            <asp:TextBox runat="server" ID="txt_TaskName" datatype="*" nullmsg="请填写任务名称" AutoCompleteType="Disabled"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">班次类型：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_ClassType" datatype="ddl" errormsg="请选择班次信息" AutoPostBack="true" OnSelectedIndexChanged="ddl_ClassType_SelectedIndexChanged"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">表单类型：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_TableType" datatype="ddl" errormsg="请选择表单类型信息" AutoPostBack="true" OnSelectedIndexChanged="ddl_TableType_SelectedIndexChanged"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr runat="server" id="trnull" visible="false">
                        <td align="right">基础班次选择：</td>
                        <td colspan="3">
                            <asp:CheckBoxList runat="server" ID="cbl_Select" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="cbl_Select_SelectedIndexChanged" AutoPostBack="true"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List" Visible="false" OnItemDataBound="rp_List_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td colspan="4" style="margin: 0; padding: 0;">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                                        <tbody>
                                            <tr>
                                                <th align="left" colspan="2">流程：<%#Eval("Name") %><asp:HiddenField runat="server" ID="hf_FlowID" Value='<%#Eval("ID") %>' />
                                                </th>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100px" rowspan="2">人员选取：</td>
                                                <td>
                                                    <asp:CheckBoxList runat="server" ID="chk_ClassList" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false">
                                                    </asp:CheckBoxList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_SysUser" cascadeCheck="false" runat="server" multiline="true" multiple="true" name="txt_SysUser" onlyLeafCheck="true" CssClass="easyui-combotree" Width="90%"></asp:TextBox>
                                                    <%--<asp:TextBox runat="server" ID="txt_SysUser"></asp:TextBox>
                                                    <img src="../images/checkbtn.gif" id="btn_plancom" runat="server" onclick="showbox()" />--%>
                                                    <%--<asp:HiddenField runat="server" ID="hf_UIDS" />--%>
                                                    <asp:Literal runat="server" ID="ltl_Content"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">开始时间：</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_BeginDate" AutoCompleteType="Disabled" onclick="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">结束时间：</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_EndDate" AutoCompleteType="Disabled" onclick="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">提醒时间：</td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_RemindDate" AutoCompleteType="Disabled" onclick="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM-dd HH:mm:ss'})"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null" visible="false">
                        <td align="center" colspan="4">暂无流程信息</td>
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

