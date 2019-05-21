<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Integralconfig.aspx.cs" Inherits="IFMP.integration.Integralconfig" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/asyncbox.css" rel="stylesheet" />
    <script src="../js/jquery-3.3.1.min.js"></script>
    <script src="../js/editinfor.js" type="text/javascript"></script>
    <script src="../js/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../js/My97/WdatePicker.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
    <style>
        .row {
            color: #316bc3;
            background: #fff;
            width: 98%;
            margin: auto;
            margin-top: 5px;
            padding: 8px 0px;
            border-bottom: 1px solid #e7eaec;
            min-width: 1400px;
        }

        .col-lg-10 {
            background: #fff;
            width: 98%;
            margin: auto;
        }

        .breadcrumb {
            background-color: #fff;
            padding: 0;
            margin-bottom: 0;
            float: left;
        }

            .breadcrumb li a, .breadcrumb li {
                font-size: 13px;
                text-decoration: none;
            }

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
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>系统管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="任务发布"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="2" align="left">积分配置</th>
                    </tr>
                    <tr>
                        <td colspan="2" style="text-align: center">
                            <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" ID="rbl_WorkType" CssClass="edilab"
                                OnSelectedIndexChanged="rbl_WorkType_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="按月结算" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="按日结算" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="200px">
                            <asp:Label ID="lbl_WorkingScore" runat="server"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_WorkingScore" runat="server" CssClass="searchbg" datatype="zheng" nullmsg="请录入分数"></asp:TextBox>
                            <span style="color: Red">*</span></td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_WDate" runat="server"></asp:Label>
                        </td>
                        <td id="td1" runat="server">
                            <asp:TextBox ID="txt_WDate1" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen',dateFmt:'yyyy-MM'})"></asp:TextBox>
                            <span style="color: Red">*</span></td>
                        <td id="td2" runat="server">
                            <asp:TextBox ID="txt_WDate2" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                            <span style="color: Red">*</span></td>
                    </tr>
                    <tr>
                        <td align="right">启动分数：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_BeginScore" runat="server" CssClass="searchbg" datatype="zheng" nullmsg="请录入启动分数"></asp:TextBox>
                            <span></span><span style="color: Red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
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
