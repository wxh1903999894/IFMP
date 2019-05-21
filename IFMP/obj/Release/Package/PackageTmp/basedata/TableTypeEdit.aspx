<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableTypeEdit.aspx.cs" Inherits="IFMP.basedata.TableTypeEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/AddOption.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
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
                    <td align="right">表单名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Name" datatype="*1-30" nullmsg="请输入表单名称" CssClass="searchbg" runat="server">
                        </asp:TextBox>
                        <span style="color: Red">*</span>
                    </td>
                </tr>

                <tr>
                    <td align="right">生产线：</td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_ProductionLineID" datatype="ddl" errormsg="请选择生产线"></asp:DropDownList>
                        <span style="color: red;">*</span>
                    </td>
                </tr>


                <tr>
                    <td align="right">是否可多次填写：</td>
                    <td colspan="3">
                        <asp:RadioButtonList runat="server" ID="rdo_IsMulti" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                            <asp:ListItem Value="1">是</asp:ListItem>
                            <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>


                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

