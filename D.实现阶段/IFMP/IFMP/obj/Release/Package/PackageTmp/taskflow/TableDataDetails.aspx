<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TableDataDetails.aspx.cs" Inherits="IFMP.taskflow.TableDataDetails" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
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
                    <asp:Repeater runat="server" ID="rp_ColList">
                        <ItemTemplate>
                            <tr>
                                <td width="100px" align="right">
                                    <%#Eval("ColumnName") %>：
                                </td>
                                <td>
                                    <%#Eval("Data") %>
                                    <span style="color:#ff0000">                                      
                                        <%#Eval("HintData") %>
                                    </span>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr>
                        <td colspan="2" align="center">
                           <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("B_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

