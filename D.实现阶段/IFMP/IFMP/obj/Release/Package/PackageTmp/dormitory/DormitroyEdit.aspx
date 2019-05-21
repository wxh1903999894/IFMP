﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DormitroyEdit.aspx.cs" Inherits="IFMP.dormitory.DormitroyEdit" %>

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
                    <td align="right">宿舍名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_User" datatype="*1-30"  nullmsg="请输入宿舍名称"  CssClass="searchbg" runat="server">
                        </asp:TextBox>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">备注：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_memo" runat="server">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">宿舍编号：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_code" datatype="*" nullmsg="请输入宿舍编号"  CssClass="searchbg" runat="server">
                        </asp:TextBox>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">是否点检：
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbl_IsCheck" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                        </asp:RadioButtonList>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="100px">宿舍人员：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_SysID" cascadeCheck="false" runat="server" name="txt_SysID" multiline="true" multiple="true" onlyLeafCheck="true" url="../ashx/GetBaseDate.ashx?method=GetUserTxt&deptype=1" CssClass="easyui-combotree"></asp:TextBox>
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

