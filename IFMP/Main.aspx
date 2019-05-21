<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="IFMP.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="css/style.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="css/timeline.css" />

    <!--<script src="plugins/jquery-3.3.1.js"></script>-->
    <script src="plugins/jquery.SuperSlide2.1.2.js"></script>
    <script type="text/javascript" src="plugins/jquery-1.9.1.min.js"></script>
    <script type="text/javascript" src="plugins/jquery.timelinr-0.9.53.js"></script>

    <script src="plugins/echarts.js"></script>
    <script>
        $(function () {
            $().timelinr({
                //autoPlay: 'true',
                autoPlayDirection: 'forward',
                startAt: 1
            })
        });
    </script>
    <style type="text/css">
        #timeline {
            width: 75%;
            /*height: 300px;*/
            overflow: hidden;
            margin: 50px auto;
            position: relative;
            background: url('images/dot.gif') left 45px repeat-x;
        }

        #dates {
            width: 760px;
            height: 60px;
            overflow: hidden;
        }

            #dates li {
                list-style: none;
                float: left;
                width: 100px;
                height: 50px;
                font-size: 24px;
                text-align: center;
                background: url('images/biggerdot.png') center bottom no-repeat;
            }

            #dates a {
                line-height: 38px;
                padding-bottom: 10px;
            }

            #dates .selected {
                font-size: 38px;
            }

        #issues {
            width: 760px;
            /*height: 300px;*/
            overflow: hidden;
        }

            #issues li {
                width: 760px;
                /*height: 300px;*/
                list-style: none;
                float: left;
            }

                #issues li h1 {
                    color: #ffcc00;
                    font-size: 30px;
                    margin: 20px 0;
                    text-shadow: #000 1px 1px 2px;
                }

                #issues li p {
                    font-size: 14px;
                    margin-right: 70px;
                    margin: 10px;
                    font-weight: normal;
                    line-height: 22px;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Literal ID="ltl_Content" runat="server"></asp:Literal>
        <div style="width: 98%; margin: 0 auto;">
            <div>
                <div style="position: relative; margin: 0 auto; padding: 0 15px;">
                    <div class="layui-row layui-col-space15">
                        <div style="position: relative; display: block; box-sizing: border-box; float: left; width: 50%;">
                            <div>
                                <div style="float: left; width: 100%; position: relative; display: block; box-sizing: border-box;">
                                    <div class="layui-card">
                                        <div class="layui-card-header">当前时间：<asp:Literal runat="server" ID="ltl_DateTime"></asp:Literal></div>
                                    </div>
                                    <div class="layui-card">
                                        <div class="layui-tab layui-tab-brief layadmin-latestData">
                                            <ul class="layui-tab-title">
                                                <li class="layui-this">最新消息</li>
                                            </ul>
                                            <div style="padding: 10px;">
                                                <table cellspacing="0" cellpadding="0" border="0" class="layui-table" lay-skin="line">
                                                    <thead>
                                                        <tr>
                                                            <th data-minwidth="300">
                                                                <div class="layui-table-cell laytable-cell-1-keywords"><span>通知内容</span></div>
                                                            </th>
                                                            <th data-minwidth="120">
                                                                <div class="layui-table-cell laytable-cell-1-frequency"><span>发送时间</span><span class="layui-table-sort layui-inline"></span></div>
                                                            </th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <asp:Repeater runat="server" ID="rp_List">
                                                            <ItemTemplate>
                                                                <tr data-index="<%#Container.ItemIndex %>" class="">
                                                                    <td title='<%#Eval("Contenet") %>'>
                                                                        <div class="layui-table-cell laytable-cell-1-keywords">
                                                                            <a href="#" class="layui-table-link"><%#Eval("Contenet") %></a>
                                                                        </div>
                                                                    </td>
                                                                    <td>
                                                                        <%#Eval("SendDate") %>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <tr runat="server" id="tr_null">
                                                            <td align="center" colspan="2">暂无记录</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                                <style>
                                                    .laytable-cell-1-0 {
                                                        width: 40px;
                                                    }

                                                    .laytable-cell-1-keywords {
                                                        width: 400px;
                                                    }
                                                </style>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="layui-col-md4">
                            <div class="layui-card">
                                <div class="layui-card-header">工作日历</div>
                                <div id="divclander" style="width: 100%; height: 500px;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="layui-tab layui-tab-brief layadmin-latestData">
                <ul class="layui-tab-title">
                    <li class="layui-this">时间轴</li>
                </ul>
            </div>
            <div id="main">
                <h2 class="top_title"></h2>
                <div id="timeline">
                    <ul id="dates">
                    </ul>
                    <ul id="issues">
                    </ul>
                </div>
            </div>
            <%--<div class="event_box">
                <div style="height: 120px;" class="parHd clearfix">
                    <ul>
                        <li class="act"><span></span>09:00审核切削液浓度表单</li>
                        <li><span></span>09:25审核设备参数点检表</li>
                        <li><span></span>16:00填写分层审核表</li>
                        <li><span></span>17:00审核分层审核表</li>
                    </ul>
                </div>
            </div>--%>
        </div>
        <!--发展历程结束-->
        <%--<script>
            $(function () {
                jQuery(".event_box").slide({ titCell: ".parHd li", mainCell: ".parBd", defaultPlay: false, titOnClassName: "act", prevCell: ".sPrev", nextCell: ".sNext" });
                jQuery(".parHd").slide({ mainCell: " ul", vis: 6, effect: "leftLoop", defaultPlay: false, prevCell: ".sPrev", nextCell: ".sNext" });
            });
        </script>--%>
    </form>
</body>
</html>
<script type="text/javascript">
    var myChart = echarts.init(document.getElementById('divclander'));
    var myDate = new Date();
    var totalDay = mGetDate(myDate.getFullYear(), (parseInt(myDate.getMonth()) + parseInt(1)));
    var data = "";
    var dateList = "[";
    for (var i = 0; i < totalDay; i++) {
        dateList += '["' + myDate.getFullYear() + '-' + (parseInt(myDate.getMonth()) + parseInt(1)) + '-' + (i + 1) + '", "勤学班", "奋进班" ],';
        if (i == totalDay - 1) {
            dateList += '["' + myDate.getFullYear() + '-' + (parseInt(myDate.getMonth()) + parseInt(1)) + '-' + (i + 1) + '", "勤学班", "奋进班" ]]';
        }
    }
    //console.log(dateList);
    dateList = JSON.parse(dateList);
    //console.log(dateList);

    var lunarData = [];
    for (var i = 0; i < dateList.length; i++) {
        lunarData.push([
            dateList[i][0],
            1,
            dateList[i][1],
            dateList[i][2],
            dateList[i][3],
        ]);
    }
    //console.log(lunarData);
    var option = {
        calendar: [{
            left: 'center',
            top: 'middle',
            cellSize: [70, 100],
            yearLabel: { show: false },
            orient: 'vertical',
            dayLabel: {
                firstDay: 1,
                nameMap: 'cn'
            },
            monthLabel: {
                show: false
            },
            range: myDate.getFullYear() + "-" + (parseInt(myDate.getMonth()) + parseInt(1))
        }],

        series: [{
            type: 'scatter',
            coordinateSystem: 'calendar',
            symbolSize: 1,
            label: {
                normal: {
                    show: true,
                    formatter: function (params) {
                        //console.log(params);
                        var d = echarts.number.parseDate(params.value[0]);
                        if (d.getDate() == 10) {
                            return d.getDate() + '\n\n' + params.value[2] + '\n\n' + (params.value[3] || '') + '\n\n' + (params.value[4] || '');
                        } else {
                            return d.getDate() + '\n\n' + params.value[2] + '\n\n' + (params.value[3] || '') + '\n\n' + (params.value[4] || '');
                        }
                    },
                    textStyle: {
                        color: '#000'
                    },
                }
            },
            data: lunarData
        }]
    };
    myChart.setOption(option);

    $(function () {
        if (getCookie("UserID") == null || getCookie("UserID") == "") {
            parent.location.href = "Default.aspx";
        }
    })

    function mGetDate(year, month) {
        var d = new Date(year, month, 0);
        return d.getDate();
    }

    function getCookie(c_name) {
        if (document.cookie.length > 0) {
            c_start = document.cookie.indexOf(c_name + "=")
            if (c_start != -1) {
                c_start = c_start + c_name.length + 1
                c_end = document.cookie.indexOf(";", c_start)
                if (c_end == -1) c_end = document.cookie.length
                return unescape(document.cookie.substring(c_start, c_end))
            }
        }
        return ""
    }
</script>

<script>
    $(document).ready(function () {
        InitTimeLine();
    })
    function InitTimeLine() {
        $.ajax({
            url: "../ashx/MainTimeHandler.ashx",
            type: "GET",
            dataType: "json",
            async: false,
            data: "method=GetTimeLine",
            success: function (data) {
                if (data.length > 0) {
                    console.log(data);
                    var dateshtml = "";
                    var issueshtml = "";
                    for (var i = 0; i < data.length; i++) {
                        dateshtml = dateshtml + "<li><a href=\"#" + data[i].BeginDate + "\">" + data[i].BeginDate + "</a></li>";
                    }
                    for (var i = 0; i < data.length; i++) {
                        issueshtml = issueshtml + "<li id=\"" + data[i].BeginDate + "\"><h1>" + data[i].BeginDate + data[i].TableType + "</h1></li>";
                    }
                    $("#dates").html(dateshtml);
                    $("#issues").html(issueshtml);
                } else {
                    //alert("获取数据失败，请联系系统管理员");
                    var html = "<li id='0' style='text-align:center;'><h1>暂无数据</h1></li>"
                    $("#issues").html(html);
                }
            },
            error: function () {
                alert("当前网络可能有错误");
            }
        });
    }
</script>
