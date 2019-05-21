<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DormitoryScoreDetail.aspx.cs" Inherits="IFMP.dormitory.DormitoryScoreDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" type="text/css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <th colspan="4" align="left">点检问题明细</th>
                </tr>
                <tr>
                    <td align="right" width="100px">宿舍：
                    </td>
                    <td colspan="3">
                        <asp:Literal ID="ltl_DorName" runat="server"></asp:Literal>

                    </td>
                </tr>
                <tr>
                    <td align="right">宿舍问题描述：
                    </td>
                    <td colspan="3">
                        <asp:Literal ID="ltl_ProDesc" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">问题责任人：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_DutyUser" runat="server"></asp:Literal></td>
                    <td align="right" width="100px">是否复查：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_SState" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">点检人：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_CreateUser" runat="server"></asp:Literal>
                    </td>
                    <td align="right">点检日期：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_CreateDate" runat="server"></asp:Literal>
                    </td>
                </tr>
                
                <tr>
                    <td align="right">复查人员：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_ReviewUser" runat="server"></asp:Literal>
                    </td>
                    <td align="right">复查日期：
                    </td>
                    <td>
                        <asp:Literal ID="ltl_ReviewDate" runat="server"></asp:Literal>
                    </td>
                </tr>
              
                <tr>
                    <td align="right">复查意见：
                    </td>
                    <td colspan="3">
                        <asp:Literal ID="ltl_ReviewMemo" runat="server"></asp:Literal></td>
                </tr>
                <tr>
                    <td align="right">图片</td>
                    <td colspan="3">
                        <asp:Image ID="img" runat="server" Width="400px" Height="400px" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
