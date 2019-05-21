<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysModuleEdit.aspx.cs" Inherits="IFMP.sysmanage.SysModuleEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧校园基础管理平台</title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../js/jquery.min.js"></script>
    <script src="../js/common.js"></script>
    <script type="text/javascript">
        function succ() {
            window.parent.location.href = "SysModuleManage.aspx";
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
        <asp:HiddenField ID="hf_PID" runat="server" />
        <asp:HiddenField ID="hf_ID" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="2" align="left">模块信息
                        </th>
                    </tr>
                    <tr>
                        <td align="right" width="150px">上级模块名称：</td>
                        <td align="left">
                            <asp:TextBox ID="txt_PMName" runat="server" Enabled="false" Width="200"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td align="right">模块名称：</td>
                        <td align="left">
                            <asp:TextBox ID="txt_MName" runat="server" datatype="*1-100"
                                nullmsg="请填写模块名称" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">链接地址：</td>
                        <td align="left">
                            <asp:TextBox ID="txt_Url" runat="server" datatype="*1-100"
                                nullmsg="请填写链接地址" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">模块按钮：</td>
                        <td align="left">
                            <asp:CheckBoxList ID="cbl_Button" CssClass="edilab" runat="server" RepeatDirection="Horizontal" RepeatColumns="10" RepeatLayout="Flow"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">图标：</td>
                        <td align="left">
                            <asp:TextBox ID="txt_Icon" runat="server" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">排序 ：</td>
                        <td align="left">
                            <asp:TextBox ID="txt_Order" runat="server" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">是否有效：</td>
                        <td align="left">
                            <asp:RadioButtonList ID="rbol_MType" runat="server" RepeatDirection="Horizontal" CssClass="edilab"
                                RepeatLayout="Flow">
                                <asp:ListItem Selected="True" Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0">
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        &nbsp; &nbsp;&nbsp;&nbsp;                                               
                            <asp:Button ID="btn_Deleted" runat="server" Text="删除" CssClass="addbtn" OnClick="btn_Delete_Click" OnClientClick="return confirm('确认删除选中的信息')" />
                        &nbsp; &nbsp;&nbsp;&nbsp; 
                            <asp:Button ID="btn_Adds" runat="server" Text="添加子模块" CssClass="deletebtn" OnClick="btn_Add_Click" />
                        &nbsp; &nbsp;&nbsp;&nbsp;           
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

