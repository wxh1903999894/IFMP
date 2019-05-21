<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuckleAdditionList.aspx.cs" Inherits="IFMP.integration.BuckleAdditionList" %>

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
        $(function () {
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'BuckleAdditionEdit.aspx', '', 900, 560, -1);
            });
        });
        function editinfo(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'BuckleAdditionEdit.aspx', 'id=' + id, 900, 560, 0);
        }

        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'BuckleAdditionDetail.aspx', 'id=' + id, 860, 520, 1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_CheckIDS" runat="server" />
        <asp:HiddenField ID="hf_Page" runat="server" />
        <asp:HiddenField ID="hf_sflag" runat="server" />
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>积分管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="积分奖扣"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">奖扣对象：
                        </td>
                        <td width="180px">
                            <asp:TextBox ID="txt_VUser" runat="server"></asp:TextBox>
                        </td>
                        <td align="right" width="50px">事件：</td>
                        <td width="230px">
                            <asp:TextBox ID="txt_EventName" runat="server" CssClass="searchbg" Width="200px"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">奖扣日期：</td>
                        <td width="230px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">审核状态：</td>
                        <td>
                            <asp:DropDownList ID="ddl_AduitState" CssClass="searchbg" runat="server"></asp:DropDownList>
                        </td>
                        <%--<td align="right">是否作废：</td>
                        <td>
                            <asp:DropDownList ID="ddl_SState" CssClass="searchbg" runat="server"></asp:DropDownList>
                        </td>--%>
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
                            <asp:Button ID="btn_Add" runat="server" CssClass="listbtncss listadd" Text="添加" />
                            <%--<asp:Button ID="btn_Delete" runat="server" CssClass="listbtncss listdel" Text="删除" OnClick="btn_Delete_Click" />--%>
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
                        <th>奖扣对象</th>
                        <th>奖扣日期</th>
                        <th>主题</th>
                        <th>事件</th>
                        <th>B分</th>
                        <th>记录人</th>
                        <th>审核状态</th>
                        <th>是否作废</th>
                        <th>事件说明</th>
                        <th width="100px">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' <%#(Eval("AuditState").ToString()!="待初审" ||(Eval("IsDel").ToString()=="True"))?"disabled":"" %> /></label>
                                </td>
                                <td><%#Eval("RealName")%></td>
                                <td><%#Eval("CreateDate","{0:yyyy-MM-dd}")%></td>
                                <td title="<%#Eval("Title")%>"><%#GetCutStr(Eval("Title"),15)%></td>
                                <td><%#Eval("EventName")%></td>
                                <td><%#Eval("BScore")%></td>
                                <td><%#Eval("RecordUName")%></td>
                                <td><%#GK.IFMP.Common.CommonFunction.CheckEnum<GK.IFMP.Common.CommonEnum.AduitState>(Eval("AuditState"))%></td>
                                <td><%#Eval("IsDel").ToString()=="True"?"是":"否"%></td>
                                <td title="<%#Eval("Content") %>"><%#GetCutStr(Eval("Content"),15)%></td>
                                <td>
                                    <%--<asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick='return editinfo(this);'>编辑</asp:LinkButton>--%>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="详细" OnClientClick='return viewinfo(this);'>详细</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hf_ID" Value='<%#Eval("ID") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="11">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>
