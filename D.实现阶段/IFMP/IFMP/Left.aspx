<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Left.aspx.cs" Inherits="IFMP.Left" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="plugins/layui/css/layui.css" rel="stylesheet">
    <link href="css/iconfont.css" rel="stylesheet">
    <link href="css/main.css" rel="stylesheet">
    <script src="plugins/layui/layui.js"></script>
</head>
<body class="leftbg">
    <form id="form1" runat="server">
        <ul class="layui-nav layui-nav-tree layui-inline" lay-filter="demo">
            <li class="layui-nav-item layui-nav-itemed">
                <a href="javascript:;" target="main">常用操作</a>
                <dl class="layui-nav-child">
                    <dd><a href="sysmanage/SysNoticeManage.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>通知消息</a></dd>
                    <dd><a href="integration/BuckleInquiryList.aspx?flag=1" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>我的积分</a></dd>
                    <dd><a href="integration/PersonAuditList.aspx" target="main"><span class="iconfont icon-icon-test"></span>积分审核</a></dd>
                    <dd><a href="integration/ScoreUserList.aspx?sflag=2" target="main"><span class="iconfont icon-icon-test"></span>我的奖票</a></dd>
                    <dd><a href="sysmanage/LeaveManage.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>我的请假</a></dd>
                    <dd><a href="sysmanage/LeaveAuditManage.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>请假审核</a></dd>
                    <dd><a href="taskflow/MyTaskManage.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>我的任务</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">日常生产</a>
                <dl class="layui-nav-child">
                    <dd><a href="dictionary/TableColumnManage.aspx" target="main"><span class="iconfont icon-icon-test"></span>表单管理</a></dd>
                    <dd><a href="production/ClassList.html" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>班次管理</a></dd>
                    <dd><a href="taskflow/TaskManage.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>任务管理</a></dd>
                    <dd><a href="taskflow/TaskSet.html" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>任务设置</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">数据统计</a>
                <dl class="layui-nav-child">
                    <dd><a href="statistics/Compare.html" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>数据对比</a></dd>
                    <dd><a href="statistics/AlertStatistics.html" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>报警统计</a></dd>

                    <dd><a href="statistics/Statistics.html" target="main"><span class="iconfont icon-kaoqin"></span>表单统计</a></dd>
                    <dd><a href="statistics/Class.html" target="main"><span class="iconfont icon-kaoqin"></span>班次统计</a></dd>
                    <dd><a href="statistics/StatList.html" target="main"><span class="iconfont icon-msnui-plan-time"></span>报表查看</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">基础数据管理</a><dl class="layui-nav-child">
                    <dd><a href="basedata/FlowList.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>流程查看</a></dd>
                    <dd><a href="basedata/BaseClassList.aspx" target="main"><span class="iconfont icon-icon-test"></span>基础班次设置</a></dd>
                    <dd><a href="basedata/BaseDateList.aspx" target="main"><span class="iconfont icon-icon-test1"></span>基础时间设置</a></dd>
                    <dd><a href="basedata/BaseFlowRoleList.aspx" target="main"><span class="iconfont icon-icon-test1"></span>基础流程权限设置</a></dd>
                    <dd><a href="dictionary/DictionaryManage.aspx" target="main"><span class="iconfont icon-icon-test1"></span>字典管理</a></dd>
                    <dd><a href="EnumDictionary.aspx" target="main"><span class="iconfont icon-icon-test1"></span>系统内置字典管理</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">人事管理</a><dl class="layui-nav-child">
                    <dd><a href="sysmanage/EmployeeManage.aspx" target="main"><span class="iconfont icon-fl-renshi"></span>档案管理</a></dd>
                    <dd><a href="sysmanage/LeaveList.aspx" target="main"><span class="iconfont icon-fl-renshi"></span>请假查询</a></dd>
                    <dd><a href="sysmanage/LeaveStatistics.aspx" target="main"><span class="iconfont icon-fl-renshi"></span>请假统计</a></dd>

                    <!--  <dd><a href="" target="main"><span class="iconfont icon-bumenguanli"></span>权限查看</a></dd>-->
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">积分管理</a>
                <dl class="layui-nav-child">
                    <dd><a href="integration/EventDataList.aspx" target="main"><span class="iconfont icon-icon-test"></span>日常事件类型</a></dd>
                    <dd><a href="integration/ScoreEventList.aspx?sflag=2" target="main"><span class="iconfont icon-event"></span>事件管理</a></dd>
                    <%--<dd><a href="integration/ScoreEventList.aspx?sflag=1" target="main"><span class="iconfont icon-icon-test"></span>固定事件管理</a></dd>--%>
                    <dd><a href="integration/UserTypeManage.aspx?tflag=2" target="main"><span class="iconfont icon-renyuanguanli"></span>人员分组管理</a></dd>
                    <dd><a href="integration/NoPMUserGroupList.aspx" target="main"><span class="iconfont icon-renyuanxiaozu"></span>不参与排名分组</a></dd>
                    <dd><a href="integration/ScoreAuditUserManage.aspx" target="main"><span class="iconfont icon-renyuanguanli"></span>审核人员管理</a></dd>
                    <%--<dd><a href="integration/FixedEventList.aspx" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>固定事件类型</a></dd>--%>
                    <dd><a href="integration/BuckleAdditionList.aspx" target="main"><span class="iconfont icon-icon-test"></span>积分奖扣</a></dd>
                    <dd><a href="integration/ScoreUserList.aspx?sflag=1" target="main"><span class="iconfont icon-icon--"></span>全部奖票</a></dd>
                    <dd><a href="integration/ScoreMonthList.aspx" target="main"><span class="iconfont icon-xiazai7"></span>积分排名</a></dd>
                    <dd><a href="integration/BuckleInquiryList.aspx?flag=2" target="main"><span class="iconfont icon-jiaojieqingkuang"></span>日常奖扣查询</a></dd>
                    <dd><a href="integration/TaskList.aspx" target="main"><span class="iconfont icon-dangan"></span>任务发布</a></dd>
                    <dd><a href="integration/TaskAuditList.aspx" target="main"><span class="iconfont icon-shenhe"></span>任务审核</a></dd>
                    <dd><a href="integration/RewardTaskList.aspx" target="main"><span class="iconfont icon-renwu"></span>任务大厅</a></dd>
                    <%--<dd><a href="integration/Integralconfig.aspx" target="main"><span class="iconfont icon-icon-test1"></span>积分配置</a></dd>--%>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">智能设备</a><dl class="layui-nav-child">
                    <dd><a href="intelligentdevice/IntelligentDeviceManage.aspx" target="main"><span class="iconfont icon-fl-renshi"></span>设备管理</a></dd>
                    <dd><a href="intelligentdevice/IntelligentDeviceDataManage.aspx" target="main"><span class="iconfont icon-bumenguanli"></span>数据查看</a></dd>
                    <dd><a href="intelligentdevice/Statistics.html" target="main"><span class="iconfont icon-bumenguanli"></span>统计图表</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">系统管理</a><dl class="layui-nav-child">
                    <dd><a href="sysmanage/SysUserManage.aspx" target="main"><span class="iconfont icon-fl-renshi"></span>用户列表</a></dd>
                    <dd><a href="sysmanage/RoleManage.aspx" target="main"><span class="iconfont icon-bumenguanli"></span>权限管理</a></dd>
                    <%--<dd><a href="sysmanage/SysModuleManage.aspx" target="main"><span class="iconfont icon-bumenguanli"></span>模块管理</a></dd>--%>
                    <dd><a href="sysmanage/DepartmentManage.aspx" target="main"><span class="iconfont icon-bumenguanli"></span>部门列表</a></dd>
                    <dd><a href="sysmanage/PostManage.aspx" target="main"><span class="iconfont icon-gangweiqiehuan"></span>岗位列表</a></dd>
                    <dd><a href="sysmanage/LogManage.aspx" target="main"><span class="iconfont icon-icon-test"></span>日志查看</a></dd>
                    <dd><a href="sysmanage/ResourceManage.aspx" target="main"><span class="iconfont icon-icon-test"></span>资源管理</a></dd>
                </dl>
            </li>
            <li class="layui-nav-item">
                <a href="javascript:;">员工宿舍</a><dl class="layui-nav-child">
                    <dd><a href="dormitory/DormitoryList.aspx" target="main"><span class="iconfont icon-fl-renshi"></span>宿舍管理</a></dd>
                </dl>
            </li>
        </ul>
        <script>
            layui.use('element', function () {
                var element = layui.element; //导航的hover效果、二级菜单等功能，需要依赖element模块

                //  //监听导航点击
                //  element.on('nav(demo)', function(elem){
                //    //console.log(elem)
                //    layer.msg(elem.text());
                //  });
            });
        </script>
    </form>
</body>
</html>
