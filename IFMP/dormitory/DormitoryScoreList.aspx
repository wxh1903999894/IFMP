<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DormitoryScoreList.aspx.cs" Inherits="IFMP.dormitory.DormitoryScoreList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('DV_id', 'DormitoryScoreDetail.aspx', 'id=' + id, 860, 520, 1);
        }
    </script>
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
                    <td class="positiona"><a>首页</a><span>></span>宿舍管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="点检问题查询"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">宿舍名称：</td>
                        <td width="170px">
                            <asp:TextBox ID="txt_DorName" runat="server" CssClass="searchbg"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">点检日期：</td>
                        <td width="230px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">是否复查：</td>
                        <td>
                            <asp:DropDownList ID="ddl_SState" CssClass="searchbg" runat="server"></asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" OnClick="btn_Query_Click" runat="server" Text="查询" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listbt">
                <tbody>
                    <tr>
                        <td align="left"></td>
                        <td align="right" valign="middle">
                            <asp:Button ID="btn_OutPut" OnClick="btn_OutPut_Click" runat="server" CssClass="listbtncss listoutput" Text="导出" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <%--<th style="width: 5px">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" id="checkall" onclick="qx(this.name, this.id)" />
                            </label>
                        </th>--%>
                        <th><strong>宿舍名称</strong></th>
                        <th><strong>点检日期</strong></th>
                        <th><strong>宿舍问题描述</strong></th>
                        <th><strong>问题责任人</strong></th>
                        <th><strong>点检人</strong></th>
                        <th><strong>复查人员</strong></th>
                        <th><strong>复查意见</strong></th>
                        <th><strong>复查日期</strong></th>
                        <th><strong>是否复查</strong></th>
                        <th width="130px"><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                               <%--<td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>--%>
                                <td><%#Eval("DormiName")%></td>
                                <td><%#Eval("CreateDate","{0:yyyy-MM-dd}")%></td>
                                <td title="<%#Eval("ProDesc") %>"><%#(Eval("ProDesc").ToString().Length>15?Eval("ProDesc").ToString().Substring(0,15)+"……":Eval("ProDesc").ToString())%></td>
                                <td><%#Eval("DutyUser")%></td>
                                <td><%#Eval("CreateUser")%></td>
                                <td><%#Eval("ReviewUser")%></td>
                                <td><%#Eval("ReviewMemo")%></td>
                                <td><%#Eval("ReviewDate","{0:yyyy-MM-dd}")%></td>
                                <td><%#Eval("IsreView").ToString().ToLower()=="false"?"否":"是"%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField ID="hf_SelectID" Value='<%#Eval("ID") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="14">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>
