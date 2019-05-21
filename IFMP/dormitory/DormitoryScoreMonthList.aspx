<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DormitoryScoreMonthList.aspx.cs" Inherits="IFMP.dormitory.DormitoryScoreMonthList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>宿舍管理</title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <style type="text/css">
        .tabs {
            height: 26px;
            border-bottom: 2px solid #328fdd;
            width: 100%;
            margin: 16px auto;
        }

            .tabs li {
                height: 26px;
                line-height: 26px;
                float: left;
                border-left: none;
                background: #fff;
                overflow: hidden;
                position: relative;
                color: #85BAB0;
                border-radius: 10px 10px 0px 0px;
                font-weight: bold;
            }

                .tabs li a {
                    display: block;
                    padding: 0 20px;
                    outline: none;
                    line-height: 26px;
                    color: #000;
                    font-weight: bold;
                }

                    .tabs li a:hover {
                        background: #85BAB2;
                        color: #fff;
                    }

            .tabs .thistab, .tabs .thistab a:hover {
                background: #497976;
                color: #F1F5F8;
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_CheckIDS" runat="server" />
        <asp:HiddenField ID="hf_Page" runat="server" />
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>宿舍管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="宿舍排名"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <%--        <div class="dvTab">
            <ul class="menuall">
                <li class='tab <%=Flag!=2?"activeTab":"" %>'><a href="SysUserManage.aspx?flag=1">年度</a></li>
                <li class='tab <%=Flag==2?"activeTab":"" %>'><a href="SysUserManage.aspx?flag=2">月度</a></li>
                <li class="tab"><a href="WSysUserManage.aspx">累计</a></li>
            </ul>
            <div class="dv"></div>
        </div>--%>
        <div style="width: 98%; margin: auto">
            <ul class="tabs" id="tabs">
                <li style="margin-left: 10px; border-left: 1px solid #d0d0d0;"><a>
                    <asp:LinkButton ID="lbtn_Saturday" Style="text-decoration: none" runat="server" Text="年度"
                        CommandName="nd" OnClick="lbtn_Monday_Click"></asp:LinkButton></a></li>
                <li><a>
                    <asp:LinkButton ID="lbtn_Staff" Style="text-decoration: none" runat="server" Text="月度"
                        CommandName="yd" OnClick="lbtn_Monday_Click"></asp:LinkButton>
                </a></li>
 <%--               <li>
                    <a>
                        <asp:LinkButton ID="lbtn_Monday" Style="text-decoration: none;" runat="server" Text="累计"
                            CommandName="lj" OnClick="lbtn_Monday_Click"></asp:LinkButton></a>
                </li>--%>
            </ul>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td width="60px" align="right">
                            <asp:Literal ID="ltl_M2" runat="server" Text="日期："></asp:Literal></td>
                        <td width="220px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                            <asp:Literal ID="ltl_zhi" runat="server" Text="--"></asp:Literal>
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox></td>
                        <td align="right" width="60px">
                            <asp:Literal ID="ltl_M1" runat="server" Text="年月："></asp:Literal></td>
                        <td>
                            <asp:DropDownList ID="ddl_Year" runat="server"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_Month" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" OnClick="btn_Query_Click" runat="server" Text="查询" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th><strong>排名</strong></th>
                        <th><strong>宿舍名称</strong></th>
                        <th><strong>宿舍分数</strong></th>
                    </tr>
                    <asp:Repeater ID="rp_List" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#GetName(Eval("Number"),Eval("Number")) %></td>
                                <td><%#GetName(Eval("Number"),Eval("RealName"))%></td>
                                <td><%#GetName(Eval("Number"),Eval("Total"))%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="6">暂无记录                                          
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
