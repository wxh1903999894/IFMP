<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IntelligentDeviceManage.aspx.cs" Inherits="IFMP.intelligentdevice.IntelligentDeviceManage" %>

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
        $(function () {
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'IntelligentDeviceEdit.aspx', '', 1000, 430, -1);
            });
        });

        function editinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'IntelligentDeviceEdit.aspx', 'id=' + id, 1000, 430, 0);
        }

        function viewinfo(e) {
            var id = $(e).next().next().val();
            return openbox('A_id', 'IntelligentDeviceDataManage.aspx', 'id=' + id, 1000, 630, 0);
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
                    <td class="positiona"><a>首页</a><span>></span>智能设备<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="智能设备管理"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">设备名称：</td>
                        <td width="100px">
                            <asp:TextBox runat="server" ID="txt_Name"></asp:TextBox>
                        </td>
                        <td align="right" width="80px">设备类型：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_DeviceType" CssClass="searchbg" datatype="ddl" errormsg="请选择设备类型" runat="server"></asp:DropDownList>
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
                            <asp:Button ID="btn_Add" runat="server" CssClass="listbtncss listadd" Text="添加" />
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
                        <th>设备名称</th>
                        <th>设备类型</th>
                        <th>放置地点</th>
                        <th>机器标识</th>
                        <th>负责人</th>
                        <th>开始日期</th>
                        <th>结束日期</th>
                        <th width="130px">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>'/>
                                    </label>
                                </td>
                                <td align="center" title="<%#Eval("Name") %>"><%#Eval("Name") %></td>
                                <td align="center"><%#Enum.GetName(typeof(IFMPLibrary.Enums.DeviceType),Eval("DeviceType")) %></td>
                                <td align="center" title="<%#Eval("Place") %>"><%#Eval("Place") %></td>
                                <td align="center"><%#Eval("Identity") %></td>
                                <td align="center"><%#Eval("RealName") %></td>
                                <td align="center"><%#Eval("BeginDate","{0:yyyy-MM-dd}") %></td>
                                <td align="center"><%#Eval("EndDate","{0:yyyy-MM-dd}") %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="listbtn btndetialcolor" ToolTip="记录查看" OnClientClick='return viewinfo(this);'>记录查看</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick='return editinfo(this);'>编辑</asp:LinkButton>
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


