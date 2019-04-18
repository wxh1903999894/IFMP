<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NoPMUserGroupEdit.aspx.cs" Inherits="IFMP.integration.NoPMUserGroupEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });

        function showbox() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelete.aspx', "", 1100, 585, 8);
        }
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
        <asp:HiddenField ID="hf_tflag" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="2" align="left">不参与排名信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="110px">人员：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_SysID" cascadeCheck="false" runat="server" name="txt_SysID" multiline="true" multiple="true" onlyLeafCheck="true" url="../ashx/GetBaseDate.ashx?method=GetUserTxt&deptype=1" CssClass="easyui-combotree"></asp:TextBox>
                            <span style="color: red;">*</span>
                            <%--<asp:TextBox ID="txt_SysID" runat="server" Enabled="false"></asp:TextBox>
                            <asp:HiddenField ID="hf_CID" runat="server" />
                            <img src="../images/checkbtn.gif" id="btn_plancom" style="margin-top: 12px;" onclick="showbox()" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">可见的排名分组：
                        </td>
                        <td>
                            <asp:CheckBoxList ID="ckl_Groups" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" RepeatColumns="7" CssClass="edilab"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="2">
                            <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

