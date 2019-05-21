<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskAuditEdit.aspx.cs" Inherits="IFMP.mobile.TaskAuditEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/layui/css/layui.css" rel="stylesheet">
    <link href="../css/iconfont.css" rel="stylesheet">
    <link href="../css/mobilemain.css" rel="stylesheet" />
    <script src="../plugins/jquery-3.3.1.js"></script>
    <script src="../plugins/layui/layui.js"></script>
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
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
        <div style="padding: 20px; background-color: #F2F2F2;">
            <div class="layui-row layui-col-space20">
                <div class="layui-col-sm12">
                    <div class="layui-card">
                        <div class="layui-card-header">任务信息</div>
                        <div class="layui-card-body">
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">任务名称</label>
                                <div class="layui-input-block">
                                    <span style="" class="layui-form-mid">
                                        <asp:Literal runat="server" ID="ltl_TaskName"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">班次类型</label>
                                <div class="layui-input-block">
                                    <span style="" class="layui-form-mid">
                                        <asp:Literal runat="server" ID="ltl_ClassType"></asp:Literal></span>
                                </div>
                            </div>
                           <%-- <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">表单类型</label>
                                <div class="layui-input-block">
                                    <span style="" class="layui-form-mid">
                                        <asp:Literal runat="server" ID="ltl_TableType"></asp:Literal></span>
                                </div>
                            </div>
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">班次选择</label>
                                <div class="layui-input-block">
                                    <span style="" class="layui-form-mid">
                                        <asp:CheckBoxList runat="server" ID="cbl_Select" Enabled="false" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow"></asp:CheckBoxList></span>
                                </div>
                            </div>--%>
                        </div>
                    </div>
                </div>
                <asp:Repeater runat="server" ID="rp_List" Visible="false" OnItemDataBound="rp_List_ItemDataBound">
                    <ItemTemplate>
                        <div class="layui-col-sm12">
                            <div class="layui-card">
                                <div class="layui-card-header">
                                    流程：<%#Eval("Name") %>
                                    <asp:HiddenField runat="server" ID="hf_FlowID" Value='<%#Eval("ID") %>' />
                                </div>
                               <%-- <div class="layui-card-body">
                                    <div class="layui-form-item layui-form-text">
                                        <label class="layui-form-label">基础班次人员</label>
                                        <div class="layui-input-block">
                                            <span style="" class="layui-form-mid">
                                                <asp:CheckBoxList runat="server" ID="chk_ClassList" CssClass="edilab" RepeatDirection="Horizontal" RepeatLayout="Flow" Enabled="false">
                                                </asp:CheckBoxList>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="layui-form-item layui-form-text">
                                        <label class="layui-form-label">选取人员</label>
                                        <div class="layui-input-block">
                                            <span style="" class="layui-form-mid">
                                                <asp:Literal ID="ltl_SysUser" runat="server"></asp:Literal>
                                            </span>
                                        </div>
                                    </div>
                                    <div class="layui-form-item layui-form-text">
                                        <label class="layui-form-label">开始时间</label>
                                        <div class="layui-input-block">
                                            <span style="" class="layui-form-mid">
                                                <asp:Literal runat="server" ID="ltl_BeginDate"></asp:Literal></span>
                                        </div>
                                    </div>
                                    <div class="layui-form-item layui-form-text">
                                        <label class="layui-form-label">结束时间</label>
                                        <div class="layui-input-block">
                                            <span style="" class="layui-form-mid">
                                                <asp:Literal runat="server" ID="ltl_EndDate"></asp:Literal></span>
                                        </div>
                                    </div>
                                    <div class="layui-form-item layui-form-text">
                                        <label class="layui-form-label">提醒时间</label>
                                        <div class="layui-input-block">
                                            <span style="" class="layui-form-mid">
                                                <asp:Literal runat="server" ID="ltl_RemindDate"></asp:Literal></span>
                                        </div>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                        <asp:Repeater runat="server" ID="rp_TableList" OnItemDataBound="rp_TableList_ItemDataBound">
                            <ItemTemplate>
                                <div class="layui-col-sm12">
                                    <div class="layui-card card-child">
                                        <div class="layui-card-header head-child">
                                            <%#Eval("UserName") %>&nbsp;&nbsp;<%#Eval("CreateDate","{0:yyyy-MM-dd HH:mm:ss}") %>
                                            <asp:HiddenField runat="server" ID="hf_TableID" Value='<%#Eval("ID") %>' />
                                            <asp:HiddenField runat="server" ID="hf_CreateUser" Value='<%#Eval("CreateUser") %>' />
                                            <asp:HiddenField runat="server" ID="hf_FID" Value='<%# DataBinder.Eval((Container.Parent.Parent as RepeaterItem).DataItem, "ID") %>' />
                                        </div>
                                        <div class="layui-card-body ">
                                            <asp:Repeater runat="server" ID="rp_ColList" OnItemDataBound="rp_ColList_ItemDataBound">
                                                <ItemTemplate>
                                                    <div class="layui-form-item layui-form-text">
                                                        <label class="layui-form-label"><%#Eval("ColumnName") %></label>
                                                        <div class="layui-input-block">
                                                            <span style="" class="layui-form-mid">
                                                                <font <%#Eval("IsAlert").ToString()=="True"?"style=\"color:red\"":"" %>><%#Eval("Data") %></font>
                                                                <span style="color: red;">
                                                                    <asp:Literal runat="server" ID="ltl_RegexData" Visible="false"></asp:Literal>
                                                                    <asp:HiddenField runat="server" ID="hf_HintDictionaryID" Value='<%#Eval("HintDictionaryID") %>' />
                                                                </span>
                                                            </span>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <div class="layui-card-body" runat="server" id="tr_result" visible="false">
                                            <div class="layui-form-item layui-form-text">
                                                <label class="layui-form-label">审核结果</label>
                                                <div class="layui-input-block">
                                                    <span style="" class="layui-form-mid">
                                                        <asp:Literal runat="server" ID="ltl_AuditResult"></asp:Literal>
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="layui-form-item layui-form-text">
                                                <label class="layui-form-label">审核意见</label>
                                                <div class="layui-input-block">
                                                    <span style="" class="layui-form-mid">
                                                        <asp:Literal runat="server" ID="ltl_AuditMessage"></asp:Literal>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="layui-card-body" runat="server" id="trnull" visible="false">
                                            <div class="layui-form-item layui-form-text blue1">
                                                <label class="">未提交任何信息</label>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </ItemTemplate>
                </asp:Repeater>
                <div class="layui-col-sm12" runat="server" id="tr_null" visible="false">
                    <div class="layui-card">
                        <div class="layui-card-header">暂无流程信息</div>
                    </div>
                </div>
                <div class="layui-col-sm12">
                    <div class="layui-card">
                        <div class="layui-card-body">
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">审核人</label>
                                <div class="layui-input-block">
                                    <span style="" class="">
                                        <asp:DropDownList runat="server" ID="ddl_User"></asp:DropDownList>
                                    </span>
                                </div>
                            </div>
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">审核结果</label>
                                <div class="layui-input-block">
                                    <span style="" class="layui-form-mid">
                                        <asp:RadioButtonList runat="server" ID="rdo_AuditResult" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                                            <asp:ListItem Value="1" Selected="True">通过</asp:ListItem>
                                            <asp:ListItem Value="2">不通过</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </span>
                                </div>
                            </div>
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">审核意见</label>
                                <div class="layui-input-block">
                                    <asp:TextBox runat="server" ID="txt_AuditMessage" Text="通过" TextMode="MultiLine" Width="60%" Height="100px"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <button type="button" style="background-color: red"  class="layui-btn layui-btn-normal layui-btn-radius" onclick='back()'>取消</button>
        <asp:Button runat="server"  ID="btn_Submit" CssClass="layui-btn layui-btn-normal layui-btn-radius" Text="提交" OnClick="btn_Submit_Click" />
    </form>
</body>
</html>
<script>
    function back() {
        window.location.href = "TaskList.aspx";
    }
</script>

