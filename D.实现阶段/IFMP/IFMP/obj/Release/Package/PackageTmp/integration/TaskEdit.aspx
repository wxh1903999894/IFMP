<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskEdit.aspx.cs" Inherits="IFMP.integration.TaskEdit" %>

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
        function showboxFirstAduitUser() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelete.aspx', "&flag=5", 920, 585, 1);
        }
        function showboxLastAduitUser() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelete.aspx', "&flag=6", 920, 585, 1);
        }
        function showbox() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelectMore.aspx', "&flag=2", 920, 585, 8);
        }
        function getfile() {
            var hfimg = $id("hf_UpFile");
            var icons = $id("divicon").getElementsByTagName("input");
            hfimg.value = icons.length;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_UID" runat="server" />
        <asp:HiddenField ID="hf_SID" runat="server" />
        <asp:HiddenField ID="hf_Score" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <td align="right">任务名称：
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_TaskName" runat="server" CssClass="searchbg" Width="400px" datatype="*" nullmsg="请录入任务名称"></asp:TextBox>
                        <span style="color: Red">*</span></td>
                </tr>
                <tr>
                    <td align="right">汇报截止日期：
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_EndDate" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        <span style="color: Red">*</span></td>
                </tr>
                <tr>
                    <td align="right">悬赏分数：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_TScore" runat="server" Width="75px" CssClass="searchbg" datatype="zheng" nullmsg="请录入悬赏分数"></asp:TextBox>
                        <span></span><span style="color: Red">*</span>
                    </td>
                    <td align="right">报名分数：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_SignScore" runat="server" Width="75px" CssClass="searchbg" datatype="zeronum" nullmsg="请录入报名分"></asp:TextBox>
                        <span></span><span style="color: Red">*</span>
                    </td>
                </tr>

                <tr>
                    <td align="right">初审人：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_FirstAduitUser" runat="server" datatype="ddl" errormsg="请选择初审人"></asp:DropDownList>
                        <span style="color: Red">*</span>
                        <%--<asp:TextBox ID="txt_FirstAduitUser" runat="server" Enabled="false" datatype="*" nullmsg="请选择初审人"></asp:TextBox>
                                <asp:HiddenField ID="hf_FirstAduitUser" runat="server" />
                                <img id="img1" runat="server" src="../images/checkbtn.gif" onclick="showboxFirstAduitUser()" />--%>
                    </td>
                    <td align="right">终审人：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_LastAduitUser" runat="server" datatype="ddl" errormsg="请选择终审人"></asp:DropDownList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">任务内容：
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_TaskContent" runat="server" Rows="6" Height="100px" Width="98%" datatype="*" nullmsg="请录入任务内容" Style="resize: none;" TextMode="MultiLine" CssClass="searchbg"></asp:TextBox>
                        <span></span><span style="color: Red">*</span></td>
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
