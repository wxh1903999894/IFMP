<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyTaskDetail.aspx.cs" Inherits="IFMP.taskflow.MyTaskDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <style>
        .edilab label {
            float: none;
        }

        .edilab input {
            height: 13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="6" align="left">任务信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">任务名称：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_TaskName"></asp:Literal>
                        </td>
                        <td align="right" width="100px">班次类型：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_ClassType"></asp:Literal>
                        </td>
                        <td align="right" width="100px">表单类型：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_TableType"></asp:Literal>
                        </td>
                    </tr>
                    <tr runat="server">
                        <td align="right">班次选择：</td>
                        <td colspan="5">
                            <asp:CheckBoxList runat="server" ID="cbl_Select" Enabled="false" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List" Visible="false" OnItemDataBound="rp_List_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td colspan="6" style="margin: 0; padding: 0;">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                                        <tbody>
                                            <tr>
                                                <th align="left" colspan="6">流程：<%#Eval("Name") %>
                                                    <asp:HiddenField runat="server" ID="hf_FlowID" Value='<%#Eval("ID") %>' />
                                                </th>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100px">人员选取：</td>
                                                <td colspan="3">
                                                    <asp:CheckBoxList runat="server" ID="chk_ClassList" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false">
                                                    </asp:CheckBoxList>
                                                </td>
                                                <td colspan="2">
                                                    <asp:Literal ID="ltl_SysUser" runat="server"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" width="100px">开始时间：</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltl_BeginDate"></asp:Literal>
                                                </td>
                                                <td align="right" width="100px">结束时间：</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltl_EndDate"></asp:Literal>
                                                </td>
                                                <td align="right" width="100px">提醒时间：</td>
                                                <td>
                                                    <asp:Literal runat="server" ID="ltl_RemindDate"></asp:Literal>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="margin: 0; padding: 0;">
                                    <asp:Repeater runat="server" ID="rp_TableList" OnItemDataBound="rp_TableList_ItemDataBound">
                                        <ItemTemplate>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                                                <tbody>
                                                    <tr>
                                                        <td align="center" colspan="6" style="font-weight: bold;"><%#Eval("UserName") %>&nbsp;&nbsp;<%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}") %>
                                                            <asp:HiddenField runat="server" ID="hf_TableID" Value='<%#Eval("ID") %>' />
                                                            <asp:HiddenField runat="server" ID="hf_CreateUser" Value='<%#Eval("CreateUser") %>' />
                                                            <asp:HiddenField runat="server" ID="hf_FID" Value='<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "ID") %>' />
                                                        </td>
                                                    </tr>
                                                    <asp:Repeater runat="server" ID="rp_ColList">
                                                        <ItemTemplate>
                                                            <tr>
                                                                <td width="350px" align="right">
                                                                    <%#Eval("ColumnName") %>：
                                                                </td>
                                                                <td>
                                                                    <font <%#Eval("IsAlert").ToString()=="True"?"style=\"color:red\"":"" %> ><%#Eval("Data") %></font>
                                                                    <span>
                                                                        <asp:Literal runat="server" ID="ltl_RegexData" Visible="false"></asp:Literal></span>
                                                                </td>
                                                            </tr>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                    <tr runat="server" id="tr_result" visible="false">
                                                        <td align="right" width="100px">审核结果：</td>
                                                        <td>
                                                            <asp:Literal runat="server" ID="ltl_AuditResult"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="tr_message" visible="false">
                                                        <td align="right" width="100px">审核意见：</td>
                                                        <td>
                                                            <asp:Literal runat="server" ID="ltl_AuditMessage"></asp:Literal>
                                                        </td>
                                                    </tr>
                                                    <tr runat="server" id="trnull">
                                                        <td align="center" colspan="6">未提交任何信息</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr runat="server" id="tr_null" visible="false">
                        <td align="center" colspan="6">暂无流程信息</td>
                    </tr>
                    <tr>
                        <td colspan="6" align="center">
                            <input type="button" name="button" id="cancell" value="返回" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

