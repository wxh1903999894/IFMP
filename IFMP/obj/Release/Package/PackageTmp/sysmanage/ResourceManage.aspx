<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResourceManage.aspx.cs" Inherits="IFMP.sysmanage.ResourceManage" %>

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
    <script src="../plugins/Common.js"></script>
    <script>
        $(function () {
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'ResourceDataEdit.aspx', 'path=' + unity.getURL("id"), 1000, 630, -1);
            });

            $('#btn_PathAdd').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'ResourcePathEdit.aspx', 'parent=' + unity.getURL("id"), 1000, 630, -1);
            });


        });

        function editinfo(e) {
            var id = $(e).next().val();
            var type = $(e).next().next().val();
            return openbox('A_id', 'ResourcePathEdit.aspx', 'parent=' + unity.getURL("id") + '&path=' + id, 1000, 630, 0);
        }

        function deletedata() {
            if (window.confirm("确认删除该文件或文件夹吗?")) {
                return true;
            } else {
                return false;
            }
        }

        function openinfo(e) {
            var id = $(e).next().next().val();
            window.location.href = "ResourceManage.aspx?id=" + id;
            return false;

            //var id = $(e).next().next().val();
            //var type = $(e).next().next().next().val();
            //return openbox('A_id', 'ResourcePathEdit.aspx', 'parent=' + unity.getURL("id") + '&path=' + id, 1000, 630, 0);
        }

        function backinfo(e) {
            var id = $(e).next().val();

            window.location.href = "ResourceManage.aspx?id=" + id;
            return false;

            //var id = $(e).next().next().val();
            //var type = $(e).next().next().next().val();
            //return openbox('A_id', 'ResourcePathEdit.aspx', 'parent=' + unity.getURL("id") + '&path=' + id, 1000, 630, 0);
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
                    <td class="positiona"><a>首页</a><span>></span>系统管理<span>></span><asp:Label ID="lbl_Menuname" runat="server" Text="用户列表"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="listcent searclass">
            <table width="99%" border="0" align="center" cellpadding="0" cellspacing="10">
                <tbody>
                    <tr>
                        <td align="right" width="60px">名称：</td>
                        <td width="200px">
                            <asp:TextBox runat="server" ID="txt_Name"></asp:TextBox>
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
                            <asp:Button ID="btn_Add" runat="server" CssClass="listbtncss listadd" Text="添加图片" />
                            <asp:Button ID="btn_PathAdd" runat="server" CssClass="listbtncss listadd" Text="添加文件夹" />
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
                        <th>名称</th>
                        <th>预览</th>
                        <th>是否滚动预览</th>
                        <th width="130" align="center">操作</th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <tr>
                                <td style="width: 5px;">
                                    <label class="wxz" id='ck_<%#Eval("ID")%>l'>
                                        <input type="checkbox" name="checkbox" onclick="setid(this.id)" value='<%#Eval("ID") %>' id='ck_<%#Eval("ID") %>' /></label>
                                </td>
                                <td align="center"><%#Eval("Name") %></td>
                                <td align="center"><%# (string.IsNullOrEmpty(Eval("Path").ToString())?"":"<a href=\""+Eval("Path")+"\" target=\"_blank\"><img width=\"100px\" src=\""+Eval("Path")+"\"></img></a>") %></td>
                                <td align="center"><%#Eval("IsCarousel") %></td>
                                <td>
                                    <asp:LinkButton ID="lbtn_Delete" Visible='<%#(Eval("Type")=="最后"?false:true)%>' CommandArgument='<%#Eval("Type")+"_"+Eval("ID")%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="删除" OnClick="btn_Delete_Click" OnClientClick="return deletedata()">删除</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Back" Visible='<%#(Eval("Type")=="最后"?true:false)%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="返回" OnClientClick="return backinfo(this)">返回</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Open" Visible='<%#(Eval("Type")=="文件夹"?true:false)%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="打开" OnClientClick='return openinfo(this);'>打开</asp:LinkButton>
                                    <asp:LinkButton ID="lbtn_Edit" Visible='<%#(Eval("Type")=="文件夹"?true:false)%>' runat="server" CssClass="listbtn btneditcolor" ToolTip="编辑" OnClientClick='return editinfo(this);'>编辑</asp:LinkButton>
                                    <asp:HiddenField runat="server" ID="hf_SysID" Value='<%#Eval("ID") %>' />
                                    <asp:HiddenField runat="server" ID="hf_TypeID" Value='<%#Eval("Type") %>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null">
                        <td bgcolor="#ffffff" align="center" colspan="7">暂无记录</td>
                    </tr>
                </tbody>
            </table>
        </div>
        <wuc:Pager runat="server" ID="Pager" OnPageChanged="Pager_PageChanged" />
    </form>
</body>
</html>


