<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskSetManage.aspx.cs" Inherits="IFMP.taskflow.TaskSetManage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>智慧工厂管理平台</title>
    <link href="../plugins/layui/css/layui.css" rel="stylesheet">
    <link href="../css/iconfont.css" rel="stylesheet">
    <link href="../css/main.css" rel="stylesheet">

    <script src="../plugins/jquery-3.3.1.js"></script>
<script src="../plugins/layui/layui.js"></script>
    <script>
        $(function () {
            $('#btn_Add').click(function () {
                //box的ID，Url,data传值，宽，高,Type(-1为添加，0为修改,其他为查看)
                return openbox('A_id', 'TaskEdit.aspx', '', 1000, 630, -1);
            });
        });

        function editinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'TaskEdit.aspx', 'id=' + id, 1000, 630, 0);
        }

        function viewinfo(e) {
            var id = $(e).next().val();
            return openbox('A_id', 'EmployeeEdit.aspx', 'id=' + id, 860, 520, 1);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="hf_CheckIDS" />
        <form class="layui-form" style="width: 45%; float: left; margin-top: 10px" action="">
            <fieldset class="layui-elem-field layui-field-title">
                <legend>操作员1</legend>
            </fieldset>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">PH值</label>
                <div class="layui-input-block">
                    <span style="" class="layui-form-mid">7</span>
                </div>
            </div>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">设备型号</label>
                <div class="layui-input-block">
                    <span style="" class="layui-form-mid">D5G15B</span>
                </div>
            </div>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">设备编号</label>
                <div class="layui-input-block">
                    <span style="" class="layui-form-mid">1008031-13</span>
                </div>
            </div>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">处理方式</label>
                <div class="layui-input-block">
                    <span style="" class="layui-form-mid">正常</span>
                </div>
            </div>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">结果</label>
                <div class="layui-input-block">
                    <span style="" class="layui-form-mid">正常运行</span>
                </div>
            </div>
            <div class="layui-form-item layui-form-text">
                <label class="layui-form-label">提交时间</label>
                <div class="layui-input-block">
                    <span style="" class="layui-form-mid">2018-05-08 07:28</span>
                </div>
            </div>
        </form>
    </form>
</body>
</html>

