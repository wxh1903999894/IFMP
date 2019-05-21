<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreAuditUserEdit.aspx.cs" Inherits="IFMP.integration.ScoreAuditUserEdit" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_tflag" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <td align="right">类别：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_UserType" datatype="ddl" errormsg="请选择人员类别" runat="server">
                        </asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="100px">人员：
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txt_SysID" AutoCompleteType="Disabled" runat="server"></asp:TextBox>
                        <asp:HiddenField ID="hf_CID" runat="server" />
                        <img src="../images/checkbtn.gif" id="btn_plancom" style="margin-top: 12px;" onclick="showbox()" />--%>
                        <asp:TextBox ID="txt_SysID" cascadeCheck="false" runat="server" name="txt_SysID" multiline="true" multiple="true" onlyLeafCheck="true" url="../ashx/GetBaseDate.ashx?method=GetUserTxt&deptype=1" CssClass="easyui-combotree"></asp:TextBox>
                        <span style="color: red;">*</span>
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
<script>
    function say(ids) {
        document.getElementById("hf_CID").value = ids;
        $.ajax({
            url: "../ashx/SysUserHandler.ashx",
            cache: false,
            type: "get",
            dataType: "json",
            data: "method=GetName&id=" + ids,
            success: function (data) {
                if (data.result == "true") {
                    document.getElementById("txt_SysID").value = data.data[0].name;
                }
            }
        })
    }
</script>

