<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseDateList.aspx.cs" Inherits="IFMP.basedata.BaseDateList" %>

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
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/choice.js"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        $(function () {
            document.cookie = name + "ScreenH=" + screen.height;
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'BaseDateEdit.aspx', '', 900, 300, -1);
            });
        });

        function editinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'BaseDateEdit.aspx', 'id=' + id, 900, 300, 0);
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
                    <td class="positiona"><a>首页</a><span>></span>基础数据管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="基础时间设置"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="80px">班次类型：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_ClassType" CssClass="searchbg" datatype="ddl" errormsg="请选择班次类型" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right" width="80px">表单类型：</td>
                        <td width="100px">
                            <asp:DropDownList ID="ddl_TableType" CssClass="searchbg" datatype="ddl" errormsg="请选择表单类型" runat="server"></asp:DropDownList>
                        </td>
                        <td align="right" width="80px">结束时间：</td>
                        <td width="240px">
                            <asp:TextBox ID="txt_Begin" runat="server" Width="75px" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>--  
                            <asp:TextBox ID="txt_End" Width="75px" runat="server" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
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
                        <th><strong>流程名称</strong></th>
                        <th><strong>表单类型</strong></th>
                        <th><strong>班次类型</strong></th>
                        <th><strong>开始时间</strong></th>
                        <th><strong>结束时间</strong></th>
                        <th><strong>提醒时间</strong></th>
                        <th><strong>操作</strong></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>
                                <td title="<%#Eval("Name") %>"><%# Eval("Name")%></td>
                                <td><%#Eval("TableTypeName")%></td>
                                <td><%#Enum.GetName(typeof(IFMPLibrary.Enums.ClassTypeEnums),Eval("ClassType"))%></td>
                                <td><%#Eval("BeginDate","{0:HH:mm:ss}")%></td>
                                <td><%#Eval("EndDate","{0:HH:mm:ss}")%></td>
                                <td><%#Eval("RemindDate","{0:HH:mm:ss}")%></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Edit" runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick="return editinfo(this);">编辑</asp:LinkButton>
                                    <asp:HiddenField ID="hf_SelectID" Value='<%#Eval("ID") %>' runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td colspan="8">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager ID="Pager" runat="server" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>

