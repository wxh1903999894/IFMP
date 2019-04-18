<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntelligentDeviceEdit.aspx.cs" Inherits="IFMP.intelligentdevice.IntelligentDeviceEdit" %>

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
    <script src="../plugins/My97/WdatePicker.js"></script>
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
        <asp:HiddenField runat="server" ID="hf_Photo" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">智能设备信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">设备名：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_Name" datatype="*" nullmsg="请填写设备名"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right" width="100px">设备类型：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_DeviceType" datatype="ddl" errormsg="请选择设备类型"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">放置地点：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_Place"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">机器标识：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_Identity" datatype="*" nullmsg="请填写机器标识"></asp:TextBox>
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
                        <td align="right">开始时间：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_BeginDate" onclick="WdatePicker({skin:'whyGreen',dateFmt:'HH:mm:ss'})"></asp:TextBox>
                        </td>
                        <td align="right">结束时间：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_EndDate" onclick="WdatePicker({skin:'whyGreen',dateFmt:'HH:mm:ss'})"></asp:TextBox>
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
