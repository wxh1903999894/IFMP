<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictionaryEdit.aspx.cs" Inherits="IFMP.dictionary.DictionaryEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/AddOption.js"></script>
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
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">字典信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">字典名称：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Name" datatype="*" nullmsg="请填写字典名称"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right" width="100px">字典类型：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_DisplayType" OnSelectedIndexChanged="ddl_DisplayType_SelectedIndexChanged" AutoPostBack="true" datatype="ddl" errormsg="请选择字典类型"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">验证内容：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_RegexType"></asp:DropDownList>
                        </td>
                        <td align="right">范围数据：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_RegexData"></asp:TextBox>
                            <span style="color: red;">内容请用"|"隔开</span>
                        </td>
                    </tr>
                    <tr runat="server" id="trtitle" visible="false">
                        <th colspan="4" align="left">选项信息</th>
                    </tr>
                    <tr runat="server" id="trvis" visible="false">
                        <td colspan="4" style="margin: 0; padding: 0;">
                            <div id="t_view">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo" id="addoption">
                                    <tr>
                                        <td>
                                            <input type="text" name="option" size="100" /></td>
                                        <td>
                                            <img alt="" id="IB1_Add" src="../images/allpic.png" onclick="Fun_AddOption()" />&nbsp;
                                    <img alt="" id="IB1_Del" src="../images/allinpic.png" onclick="return confirm('此项不可删，至少保留两项！')" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <input type="text" name="option" size="100" />
                                        </td>
                                        <td>
                                            <img alt="" id="IB2_Add" src="../images/allpic.png" onclick="Fun_AddOption()" />&nbsp;
                                    <img alt="" id="IB2_Del" src="../images/allinpic.png" onclick="return confirm('此项不可删，至少保留两项！')" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                    <tr runat="server" id="trinfo" visible="false">
                        <td colspan="4" style="padding: 0px; margin: 0px;">
                            <table width="100%" class="border-r" cellspacing="0">
                                <tr>
                                    <th colspan="4" align="left">选项信息
                                    </th>
                                </tr>
                                <tr style="text-align: center; color: #508CE4; font-weight: bold;">
                                    <td>内容</td>
                                    <td style="width: 100px">操作</td>
                                </tr>
                                <asp:Repeater runat="server" ID="rp_List">
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center"><%#Eval("Data") %></td>
                                            <td align="center">
                                                <asp:ImageButton ID="ImageButton3" ImageUrl="../images/del.png" runat="server" Width="16px" Height="16px" CommandArgument='<%#Eval("ID") %>'
                                                    OnClientClick="return  confirm('您确认删除选项信息吗？');" OnClick="ImageButton3_Click" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr runat="server" id="tr_null">
                                    <td colspan="6" style="text-align: center">暂无记录</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" colspan="4">
                            <asp:Button runat="server" ID="btn_Submit" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
