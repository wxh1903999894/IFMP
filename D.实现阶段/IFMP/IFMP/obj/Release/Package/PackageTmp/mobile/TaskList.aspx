<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskList.aspx.cs" Inherits="IFMP.mobile.TaskList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/layui/css/layui.css" rel="stylesheet" />
    <link href="../css/iconfont.css" rel="stylesheet" />
    <link href="../css/mobilemain.css" rel="stylesheet" />
    <script src="../plugins/jquery-3.3.1.js"></script>
    <script src="../plugins/layui/layui.js"></script>
    <script src="../plugins/mobile.js"></script>
</head>
<script>
    function editinfo(e) {
        var FlowID = $(e).next().next().val();
        var TType = $(e).next().next().next().val();
        var TaskID = $(e).next().next().next().next().val();
        var IsAudit = $(e).next().next().next().next().next().val();
        if (IsAudit == "True") {
            window.location.href = "TaskAuditEdit.aspx?taskid=" + TaskID + "&flowid=" + FlowID;
            //return openbox('A_id', 'TaskAuditEdit.aspx', 'taskid=' + TaskID + '&flowid=' + FlowID, 1060, 580, 1);
        } else {
            window.location.href = "TaskTableEdit.aspx?tabletype=" + TType + "&taskid=" + TaskID + "&flowid=" + FlowID;
            //return openbox('A_id', 'TaskTableEdit.aspx', 'tabletype=' + TType + '&taskid=' + TaskID + '&flowid=' + FlowID, 1060, 580, 1);
        }
        return false;
    }

    function viewinfo(e) {
        var FlowID = $(e).next().val();
        var TaskID = $(e).next().next().next().val();
        window.location.href = "MyTaskDetail.aspx?taskid=" + TaskID + "&flowid=" + FlowID;
        return false;
    }

    function back() {
        window.location.href = "Login.html";
    }
</script>
<body class="card-list big">
    <form id="form1" runat="server">
        <div class="layui-header">
            <asp:DropDownList runat="server" ID="ddl_User" AutoPostBack="true" OnSelectedIndexChanged="ddl_User_SelectedIndexChanged"></asp:DropDownList>
        </div>
        <div class="layui-form" action="">
            <div style="padding: 20px; background-color: #F2F2F2;">

                <div class="layui-row layui-col-space20" style="padding-top: 40px">
                    <asp:Repeater runat="server" ID="rp_List">
                        <ItemTemplate>
                            <div class="layui-col-sm6">
                                <div class="layui-card">
                                    <div class="layui-card-header"><%#Eval("TaskName") %></div>
                                    <div class="layui-card-body">
                                        <div class="layui-form-item layui-form-text">
                                            <label class="layui-form-label">表单类型</label>
                                            <div class="layui-input-block">
                                                <span style="" class="layui-form-mid"><%#Eval("TableTypeName") %></span>
                                            </div>
                                        </div>
                                        <div class="layui-form-item layui-form-text">
                                            <label class="layui-form-label">流程名称</label>
                                            <div class="layui-input-block">
                                                <span style='' class="layui-form-mid"><%#Eval("FlowName")+((Eval("IsMultiAccept").ToString()=="True")?(Eval("IsAudit").ToString()=="True"?"(已审核)":"(已填写)"):"") %></span>
                                            </div>
                                        </div>
                                        <div class="layui-form-item layui-form-text">
                                            <label class="layui-form-label">参与人员</label>
                                            <div class="layui-input-block">
                                                <span style="" class="layui-form-mid"><%#Eval("JoinUser") %></span>
                                            </div>
                                        </div>
                                        <div class="layui-form-item layui-form-text">
                                            <label class="layui-form-label">结束时间</label>
                                            <div class="layui-input-block">
                                                <span style="" class="layui-form-mid"><%#Eval("EndDate","{0:HH:mm:ss}") %></span>
                                            </div>
                                        </div>
                                        <div class="layui-form-item">
                                            <div class="layui-input-block">
                                                <asp:LinkButton ID="LinkButton1" Visible='<%#(Eval("IsEnd").ToString()=="True")?false:true%>' BackColor="#009688" runat="server" CssClass="layui-btn layui-btn-radius" ToolTip="操作" OnClientClick='return editinfo(this);'><%#Eval("IsAudit").ToString()=="True"?"审核":"填写" %></asp:LinkButton>
                                                <asp:LinkButton ID="lbtn_Detail" runat="server" CssClass="layui-btn layui-btn-normal layui-btn-radius" ToolTip="查看" OnClientClick='return viewinfo(this);'>查看</asp:LinkButton>
                                                <asp:HiddenField runat="server" ID="HiddenField1" Value='<%#Eval("FlowID") %>' />
                                                <asp:HiddenField runat="server" ID="HiddenField2" Value='<%#Eval("TType") %>' />
                                                <asp:HiddenField runat="server" ID="HiddenField3" Value='<%#Eval("TaskID") %>' />
                                                <asp:HiddenField runat="server" ID="HiddenField4" Value='<%#Eval("IsAudit") %>' />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    <div runat="server" id="div_null" class="layui-col-sm12">
                        <div class="layui-card">
                            <div class="layui-card-header">暂无可操作的表单</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <button class="layui-btn layui-btn-normal layui-btn-radius" onclick="back()">退出</button>
</body>
</html>
