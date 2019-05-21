<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseClassEdit.aspx.cs" Inherits="IFMP.basedata.BaseClassEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_tflag" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <th colspan="4" align="left">基础班次信息</th>
                </tr>
                <tr>
                    <td align="right" width="100px">班次名称：</td>
                    <td>
                        <asp:TextBox runat="server" ID="txt_Name" datatype="*" nullmsg="请填写班次名称"></asp:TextBox>
                        <span style="color: red;">*</span>
                    </td>
                    <td align="right" width="100px">班次类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_ClassType" datatype="ddl" errormsg="请选择班次类型" runat="server">
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>

                     <td align="right" width="100px">生产线：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_ProductionLineID" datatype="ddl" errormsg="请选择生产线" runat="server">
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>


