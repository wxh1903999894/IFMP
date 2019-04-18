﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceDataEdit.aspx.cs" Inherits="IFMP.sysmanage.ResourceDataEdit" %>

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
                        <th colspan="4" align="left">资源添加</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">名称：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_Name" datatype="*" nullmsg="请填写名称"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">是否首页显示：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_IsCarousel" datatype="ddl"></asp:DropDownList>
                            <span style="color: red;">*</span>
                    </tr>

                    <tr>
                        <td align="right">图片：</td>
                        <td colspan="3">
                            <asp:Image ID="img" Width="100px" Height="100px" Visible="false" runat="server" />
                            <div id="divicon">
                                <asp:FileUpload ID="fl_UpFile" runat="server" onchange="if(this.value)judgepic(this.value,this);" />
                            </div>
                            <asp:HiddenField ID="hf_UpFile" runat="server" />
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
