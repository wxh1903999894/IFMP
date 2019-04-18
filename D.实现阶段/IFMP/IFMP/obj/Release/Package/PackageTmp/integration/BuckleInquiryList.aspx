<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuckleInquiryList.aspx.cs" Inherits="IFMP.integration.BuckleInquiryList" %>

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
            document.cookie = name + "ScreenH=" + screen.height;
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'BuckleAdditionEdit.aspx', '', 900, 560, -1);
            });
            $('#btn_Edit').click(function () {
                return openbox('A_id', 'BuckleAdditionEdit.aspx', '', 900, 560, 0)
            });
        });
        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('DV_id', 'BuckleAdditionDetail.aspx', 'id=' + id, 860, 520, 1);
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
                    <td class="positiona"><a>首页</a><span>></span>积分管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="日常奖扣查询"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">奖扣对象：</td>
                        <td width="170px">
                            <asp:TextBox ID="txt_VUser" runat="server" CssClass="searchbg"></asp:TextBox>
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
                        <td width="100px">
                            <asp:DropDownList ID="ddl_AduitState" CssClass="searchbg" datatype="ddl" errormsg="请选择审核状态" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right" width="80px">是否作废：</td>
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
                            <asp:Button ID="lbtn_ZF" OnClick="lbtn_ZF_Click" runat="server" CssClass="listbtncss listdel" Text="作废" />
                            <asp:Button ID="btn_OutPut" OnClick="btn_OutPut_Click" runat="server" CssClass="listbtncss listoutput" Text="导出" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th style="width: 5px">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" id="checkall" onclick="qx(this.name, this.id)" />
                            </label>
                        </th>
                        <th><strong>奖扣对象</strong></th>
                        <th><strong>奖扣日期</strong></th>
                        <th><strong>主题</strong></th>
                        <th><strong>事件</strong></th>
                        <th><strong>B分</strong></th>
                        <th><strong>记录人</strong></th>
                        <th><strong>审核状态</strong></th>
                        <th><strong>是否作废</strong></th>
                        <th><strong>事件说明</strong></th>
                        <th width="130px"><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" id='ck_<%#Eval("ID") %>' value='<%#Eval("ID") %>' <%# Eval("IsDel").ToString()=="1"?"disabled":""%> />
                                    </label>
                                </td>
                                <td><%#Eval("RealName")%></td>
                                <td><%#Eval("CreateDate","{0:yyyy-MM-dd}")%></td>
                                <td title="<%#Eval("Title") %>"><%#(Eval("Title").ToString().Length>15?Eval("Title").ToString().Substring(0,15)+"……":Eval("Title").ToString())%></td>
                                <td><%#Eval("EventName")%></td>
                                <td><%#Eval("BSCore")%></td>
                                <td><%#Eval("RecordUName")%></td>
                                <td><%#(Enum.GetName(typeof(IFMPLibrary.Enums.AuditState), Eval("AuditState")))%></td>
                                <td><%#Eval("IsDel").ToString().ToLower()=="false"?"否":"是"%></td>
                                <td title="<%#Eval("Content") %>"><%#(Eval("Content").ToString().Length>15?Eval("Content").ToString().Substring(0,15)+"……":Eval("Content").ToString())%></td>
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
