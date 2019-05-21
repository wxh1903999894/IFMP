<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableColumnEdit.aspx.cs" Inherits="IFMP.dictionary.TableColumnEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/AddOption.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
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
                        <th colspan="4" align="left">表单设置信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="120px">字段名称：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_ColumnName" datatype="*" nullmsg="请填写字段名称"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">排序：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_Order" datatype="zheng"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">是否作为填写字段：</td>
                        <td colspan="3">
                            <asp:RadioButtonList runat="server" ID="rdo_IsFill" AutoPostBack="true" OnSelectedIndexChanged="rdo_IsFill_Changed" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                                <asp:ListItem Selected="True" Value="1">是</asp:ListItem>
                                <asp:ListItem Value="0">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr runat="server" id="tr_IsFill">
                        <td align="right" width="100px">填写类型：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_ColumnShowType"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">包含数据：</td>
                        <td>
                            <asp:TextBox ID="txt_Range" cascadeCheck="false" runat="server" name="txt_SysID" url="../ashx/BaseData.ashx?method=GetTableColumn" multiline="true" multiple="true" onlyLeafCheck="true" CssClass="easyui-combotree"></asp:TextBox>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr runat="server" id="tr_Dicntionary">
                        <td align="right" width="100px">合法字典：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_DictionaryID"></asp:DropDownList>
                        </td>
                        <td align="right">提示字典：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_HintDictionaryID"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr>

                        <td align="right">是否作为统计字段：</td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="rdo_IsStats" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Selected="True" Value="0">否</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>

                        <td align="right">统计类型：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_ColumnStatType"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr runat="server" id="tr_DefaultData">
                        <td align="right">默认文字：</td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txt_DefaultData" TextMode="MultiLine" Height="100px" Width="60%"></asp:TextBox>
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
