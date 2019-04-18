<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntelligentDeviceDataManage.aspx.cs" Inherits="IFMP.intelligentdevice.IntelligentDeviceDataManage" %>

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
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_CheckIDS" />
        <div class="positionc">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="18" valign="left" height="30">
                        <span class="zcbz"></span></td>
                    <td class="positiona"><a>首页</a><span>></span>智能设备<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="智能设备数据查看"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent0 searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="40px">名称：</td>
                        <td width="80px">
                            <asp:TextBox runat="server" ID="txt_Name"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">设备类型：</td>
                        <td width="80px">
                            <asp:DropDownList ID="ddl_DeviceType" CssClass="searchbg" datatype="ddl" errormsg="请选择设备类型" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right" width="80px">数据类型：</td>
                        <td width="80px">
                            <asp:DropDownList ID="ddl_DeviceDataType" CssClass="searchbg" datatype="ddl" errormsg="请选择数据类型" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right" width="80px">日期：
                        </td>
                        <td width="240px">
                            <asp:TextBox runat="server" ID="txt_BeginDate" Width="75px" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--
                            <asp:TextBox runat="server" ID="txt_EndDate" Width="75px" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btn_Search" runat="server" Text="查询" OnClick="btn_Search_Click" />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <div class="listcent0 pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th>设备名称</th>
                        <th>设备类型</th>
                        <th>数据类型</th>
                        <th>数据</th>
                        <th>日期</th>
                        <th>是否报警</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td align="center" title="<%#Eval("Name") %>"><%#Eval("Name") %></td>
                                <td align="center"><%#Enum.GetName(typeof(IFMPLibrary.Enums.DeviceType),Eval("DeviceType")) %></td>
                                <td align="center"><%#Enum.GetName(typeof(IFMPLibrary.Enums.DeviceDataType),Eval("DeviceDataType")) %></td>
                                <td align="center"><%#Eval("Data") %></td>
                                <td align="center"><%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}") %></td>
                                <td align="center"><%#(Eval("IsAlert")==null||Eval("IsAlert").ToString()=="false")?"否":"<font style=\"color:red\">是</font>" %></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="6">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>


