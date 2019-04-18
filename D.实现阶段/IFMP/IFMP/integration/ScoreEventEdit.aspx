<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreEventEdit.aspx.cs" Inherits="IFMP.integration.ScoreEventEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js" type="text/javascript"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
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
                        <th colspan="4" align="left">积分事件信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">事件名称：
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txt_EventName" runat="server" datatype="*1-30" CssClass="searchbg"
                                nullmsg="请填写事件名称" Width="400px"></asp:TextBox>
                            <span style="color: Red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">B分：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_BSCore" runat="server" datatype="zs" CssClass="searchbg"
                                nullmsg="请填写B分" MaxLength="30"></asp:TextBox>
                            <span style="color: Red">*</span>
                        </td>
                    <%--</tr>
                    <tr>--%>
                        <%--<td align="right">是否专人审核：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_IsSpecializedUser" runat="server" RepeatDirection="Horizontal" CssClass="edilab"
                                RepeatLayout="Flow">
                            </asp:RadioButtonList>
                        </td>--%>
                        <td align="right" width="100px">类型：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_EType" CssClass="searchbg" datatype="ddl" errormsg="请选择类型" runat="server"></asp:DropDownList>
                            <span style="color: Red">*</span>
                        </td>
                    </tr>
                    <%--<tr id="tr" runat="server">
                        <td align="right">初审人：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_FirstAduitUser" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right">终审人：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_LastAduitUser" runat="server"></asp:DropDownList>
                        </td>
                    </tr>--%>
                    <tr>
                        <td align="right">事件说明：
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txt_EventMark" runat="server" Rows="6" Height="100px" Width="98%" Style="resize: none;" TextMode="MultiLine" CssClass="searchbg"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
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
