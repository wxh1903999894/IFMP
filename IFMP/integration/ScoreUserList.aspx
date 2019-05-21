<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScoreUserList.aspx.cs" Inherits="IFMP.integration.ScoreUserList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>

    <link href="../css/green_list.css" rel="stylesheet" />
    <link href="../css/asyncbox.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/AsyncBox.v1.4.js"></script>
    <script src="../plugins/AsyncBox.v1.4.5.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">

        $(function () {
            document.cookie = name + "ScreenH=" + screen.height;
        });

        function OutPut(type) {
            var ids = "";
            if (type == 1) {

            } else {
                ids = $("#hf_CheckIDS").val();
                if (ids == "") {
                    alert("请选择要导出的奖票");
                    return false;
                }
            }
            //alert("asdasd");
            $.ajax({
                url: "../ashx/TicketDownloadHandler.ashx?method=DownLoadTicket&ids=" + ids,
                type: "GET",
                dataType: "json",
                async: false,
                success: function () {
                    alert("成功导出！");
                    window.open("../Resource/Score.docx");
                    location.reload();
                },
            });
            return false;
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
                    <td class="positiona"><a>首页</a><span>></span>积分管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="全部奖票"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td width="80" align="right">创建日期：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" />
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
                            <asp:Button ID="btn_OutPut" runat="server" CssClass="listbtncss listinput" Text="导出" OnClick="btn_Out_Click" />
                            <asp:Button ID="btn_OutPutFull" runat="server" CssClass="listbtncss listoutput" Text="全部导出" OnClick="btn_OutFull_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th width="5%" align="center">
                            <label class="wxz" id="checkalll">
                                <input type="checkbox" name="checkbox" value="复选框" id="checkall" onclick="qx(this.name, this.id)"></label></th>
                        <th><strong>奖扣对象</strong></th>
                        <th><strong>奖扣时间</strong></th>
                        <th><strong>主题</strong></th>
                        <th><strong>事件</strong></th>
                        <th><strong>B分</strong></th>
                        <th><strong>记录人</strong></th>
                        <th><strong>初审人</strong></th>
                        <th><strong>终审人</strong></th>
                        <th><strong>打印状态</strong></th>
                        <th><strong>事件描述</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' <%#Convert.ToBoolean(Eval("IsPrint"))?"disabled":""%> /></label>
                                </td>
                                <td>
                                    <%#Eval("RealName")%></td>
                                <td><%#Eval("CreateDate","{0:yyyy-MM-dd}")%></td>
                                <td title="<%#Eval("Title")%>"><%#(Eval("Title").ToString().Length>15?Eval("Title").ToString().Substring(0,15)+"……":Eval("Title").ToString())%></td>
                                <td><%#Eval("EventName")%></td>
                                <td><%#Eval("BSCore")%></td>
                                <td><%#Eval("RecordUserName")%></td>
                                <td><%#Eval("FirstAuditUserName")%></td>
                                <td><%#Eval("LastAuditUserName")%></td>
                                <td><%#Eval("IsPrint").ToString().ToLower()=="false"?"未打印":"已打印"%></td>
                                <td><%#Eval("Content")%></td>
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
