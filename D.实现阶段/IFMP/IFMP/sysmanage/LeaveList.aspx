<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LeaveList.aspx.cs" Inherits="IFMP.sysmanage.LeaveList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧工厂管理平台</title>
    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/green_asyncbox.css" rel="stylesheet" />

    <script src="../plugins/layui/layui.js"></script>
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>

    <script>
        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'LeaveDetail.aspx', 'id=' + id, 1000, 520, 0);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_CheckIDS" />
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>人事管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="请假查询"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">请假类型：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_LeaveType" CssClass="searchbg" datatype="ddl" errormsg="请选择请假类型" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right" width="80px">审核状态：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_LeaveState" CssClass="searchbg" datatype="ddl" errormsg="请选择审核状态" runat="server"></asp:DropDownList>
                        </td>
                        <td width="80px" align="right">请假日期：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" OnClick="btn_Search_Click" />
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
                            <asp:Button ID="btn_Delete" runat="server" CssClass="listbtncss listdel" Text="删除" OnClick="btn_Delete_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th width="5%" align="center">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" value="复选框" id="checkall" onclick="qx(this.name, this.id)" /></label></th>
                        <th>请假人</th>
                        <th>请假类型</th>
                        <th>请假原因</th>
                        <th>请假天数</th>
                        <th>开始日期</th>
                        <th>结束日期</th>
                        <th>请假日期</th>
                        <th>状态</th>
                        <th width="130px">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' <%#Eval("LeaveState").ToString()=="未审核"?"":"disabled" %> />
                                    </label>
                                </td>
                                <td align="center"><%#Eval("RealName") %></td>
                                <td align="center"><%#Enum.GetName(typeof(IFMPLibrary.Enums.LeaveType),Eval("LeaveType")) %></td>
                                <td align="center" title="<%#Eval("Content") %>"><%#(Eval("Content").ToString().Length>15?Eval("Content").ToString().Substring(0,15)+"……":Eval("Content")) %></td>
                                <td align="center"><%#Eval("Day") %></td>
                                <td align="center"><%#Eval("BeginDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center"><%#Eval("EndDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center"><%#Eval("CreateDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center"><%#Enum.GetName(typeof(IFMPLibrary.Enums.LeaveState),Eval("LeaveState")) %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btneditcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="HiddenField1" Value='<%#Eval("ID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="9">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>


