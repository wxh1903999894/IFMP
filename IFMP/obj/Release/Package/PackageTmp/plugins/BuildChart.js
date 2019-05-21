function Chart(data, type, statitem, container) {
    isrefresh = false;
    if (type == "折线图") {
        var datearray = [];
        var userarray = [];
        var seriesarray = [];
        //console.log(data.List);
        if (statitem == 0) {
            for (var i = 0; i < data.List.length; i++) {
                var usercount = unity.flitercount(function (e) { return e == data.List[i].UserName; }, userarray);
                if (usercount == -1)
                    userarray.push(data.List[i].UserName);

                usercount = unity.flitercount(function (e) { return e == data.List[i].UserName; }, userarray);

                if (datearray[usercount] == null) {
                    datearray[usercount] = [];
                }
                datearray[usercount].push(data.List[i].CreateDate.replace("T", " "));
                //var datecount = unity.flitercount(function (e) { return e == data.List[i].CreateDate.split('T')[0]; }, datearray);
                //if (datecount == -1)
                //    datearray.push(data.List[i].CreateDate);
            }
        } else {
            for (var i = 0; i < data.List.length; i++) {
                var usercount = unity.flitercount(function (e) { return e == data.List[i].StatData; }, userarray);
                if (usercount == -1)
                    userarray.push(data.List[i].StatData);

                usercount = unity.flitercount(function (e) { return e == data.List[i].UserName; }, userarray);

                if (datearray[usercount] == null) {
                    datearray[usercount] = [];
                }
                datearray[usercount].push(data.List[i].CreateDate.replace("T", " "));
                //var datecount = unity.flitercount(function (e) { return e == data.List[i].CreateDate.split('T')[0]; }, datearray);
                //if (datecount == -1)
                //    datearray.push(data.List[i].CreateDate);
            }
        }


        //根据人和时间获取分组数据
        var colorL = ['#01D8DB', '#FFBB44', '#3EA8FE', '#7BCE4C', 'rgb(0 ,0 ,255)', 'rgb(70 ,130 ,180)', 'rgb(0 ,139 ,139)', 'rgb(0, 255, 255)', 'rgb(78 ,238 ,148)', 'rgb(0,191 ,255)', 'rgb(46 ,139 ,87)'];

        for (var i = 0; i < userarray.length; i++) {
            var seriesdata = {};
            seriesdata.itemStyle = {};
            seriesdata.itemStyle.normal = {};
            seriesdata.itemStyle.normal.color = colorL[i % colorL.length];
            seriesdata.smooth = true;
            seriesdata.name = userarray[i];
            seriesdata.type = "line";
            seriesdata.data = [];
            seriesdata.symbolSize = 10;
            if (statitem == 0) {
                var selarray = unity.fliter(function (e) { return e.UserName == userarray[i] }, data.List);
                for (var j = 0; j < selarray.length; j++) {
                    seriesdata.data.push(selarray[j].Data);
                }
            } else {
                var selarray = unity.fliter(function (e) { return e.StatData == userarray[i] }, data.List);
                for (var j = 0; j < selarray.length; j++) {
                    seriesdata.data.push(selarray[j].Data);
                }
            }

            //for (var j = 0; j < datearray.length; j++) {
            //    var seldata = unity.fliterdata(function (e) { return e.UserName == userarray[i] && e.CreateDate.split('T')[0] == datearray[j]; }, data.List);
            //    if (seldata != null) {
            //        seriesdata.data.push(seldata.Data);
            //    } else {
            //        seriesdata.data.push(null);
            //    }
            //}
            seriesarray.push(seriesdata);
        }

        var yAxis = {};
        yAxis.type = "value";
        yAxis.axisLine = {};
        yAxis.axisLine.lineStyle = {};
        yAxis.axisLine.lineStyle.color = "#ccc";

        //标线
        if (data.MarkLine.Max != null && data.MarkLine.Max != "") {
            var markline = {};

            markline.type = "line";

            markline.markLine = {};
            markline.markLine.precision = 3;
            markline.markLine.data = [];
            markline.markLine.label = {};
            markline.markLine.label.position = "middle";
            var max = {};
            max.name = "最大值";
            max.yAxis = data.MarkLine.Max;
            max.lineStyle = {};
            max.lineStyle.color = "#f44444";

            var min = {};
            min.name = "最小值";
            min.yAxis = data.MarkLine.Min;
            min.lineStyle = {};
            min.lineStyle.color = "#f44444";

            markline.markLine.data.push(max);
            markline.markLine.data.push(min);

            seriesarray.push(markline);


            yAxis.min = unity.toDecimal(data.MarkLine.Min - (data.MarkLine.Max - data.MarkLine.Min) / 2, 3);
            yAxis.max = unity.toDecimal(data.MarkLine.Max + (data.MarkLine.Max - data.MarkLine.Min) / 2, 3);
            yAxis.maxInterval = unity.toDecimal((data.MarkLine.Max - data.MarkLine.Min) / 5, 3);

        }
        //console.log(userarray);
        //console.log(datearray);
        //console.log(seriesarray);
        Line(container, data.ColumnName, userarray, datearray, seriesarray, yAxis);
    }


    if (type == "饼图") {
        if (statitem == 0) {
            var dataarray = [];
            for (var i = 0; i < data.List.length; i++) {
                var seldata = unity.fliterdata(function (e) { return e.name == data.List[i].Data; }, dataarray);
                if (seldata == null) {
                    var newdata = {};
                    newdata.name = data.List[i].Data;
                    newdata.value = 1;
                    dataarray.push(newdata);
                } else {
                    seldata.value = seldata.value + 1;
                }
            }
            //排序
            dataarray = dataarray.sort();
            Pie(container, data.ColumnName, dataarray);
        } else {
            var dataarray = [];
            for (var i = 0; i < data.List.length; i++) {
                var seldata = unity.fliterdata(function (e) { return e.name == data.List[i].StatData; }, dataarray);
                if (seldata == null) {
                    var newdata = {};
                    newdata.name = data.List[i].StatData;
                    newdata.value = 1;
                    dataarray.push(newdata);
                } else {
                    seldata.value = seldata.value + 1;
                }
            }
            //排序
            dataarray = dataarray.sort();
            Pie(container, data.ColumnName, dataarray);
        }
    }


    if (type == "柱状图") {
        //按时间排序，获取内所包含的天数形成枚举，再进行处理
        //有线的添加mark线
        var colorL = ['#01D8DB', '#FFBB44', '#3EA8FE', '#7BCE4C', 'rgb(0 ,0 ,255)', 'rgb(70 ,130 ,180)', 'rgb(0 ,139 ,139)', 'rgb(0, 255, 255)', 'rgb(78 ,238 ,148)', 'rgb(0,191 ,255)', 'rgb(46 ,139 ,87)'];
        var datearray = [];
        var userarray = [];
        var seriesarray = [];
        //console.log(data);
        if (statitem == 0) {
            for (var i = 0; i < data.List.length; i++) {
                var usercount = unity.flitercount(function (e) { return e == data.List[i].UserName; }, userarray);
                if (usercount == -1)
                    userarray.push(data.List[i].UserName);

                var datecount = unity.flitercount(function (e) { return e == data.List[i].CreateDate.split('T')[0]; }, datearray);
                if (datecount == -1)
                    datearray.push(data.List[i].CreateDate.split('T')[0]);
            }
        } else {
            for (var i = 0; i < data.List.length; i++) {
                var usercount = unity.flitercount(function (e) { return e == data.List[i].StatData; }, userarray);
                if (usercount == -1)
                    userarray.push(data.List[i].StatData);

                var datecount = unity.flitercount(function (e) { return e == data.List[i].CreateDate.split('T')[0]; }, datearray);
                if (datecount == -1)
                    datearray.push(data.List[i].CreateDate.split('T')[0]);
            }
        }

        for (var i = 0; i < userarray.length; i++) {
            var seriesdata = {};
            seriesdata.itemStyle = {};
            seriesdata.itemStyle.normal = {};
            seriesdata.itemStyle.normal.color = colorL[i % colorL.length];
            //seriesdata.smooth = true;
            seriesdata.name = userarray[i];
            seriesdata.type = "bar";
            seriesdata.data = [];;
            if (statitem == 0) {
                //var selarray = unity.fliter(function (e) { return e.UserName == userarray[i] }, data.List);
                //for (var j = 0; j < selarray.length; j++) {
                //    seriesdata.data.push(selarray[j].Data);
                //}
                for (var j = 0; j < datearray.length; j++) {
                    var seldata = unity.fliterdata(function (e) { return e.UserName == userarray[i] && e.CreateDate.split('T')[0] == datearray[j]; }, data.List);
                    if (seldata != null) {
                        seriesdata.data.push(seldata.Data);
                    } else {
                        seriesdata.data.push(null);
                    }
                }
                seriesarray.push(seriesdata);
            } else {
                for (var j = 0; j < datearray.length; j++) {
                    var seldata = unity.fliterdata(function (e) { return e.StatData == userarray[i] && e.CreateDate.split('T')[0] == datearray[j]; }, data.List);
                    if (seldata != null) {
                        seriesdata.data.push(seldata.Data);
                    } else {
                        seriesdata.data.push(null);
                    }
                }
                seriesarray.push(seriesdata);
            }
        }

        var yAxis = {};
        yAxis.type = "value";
        yAxis.axisLine = {};
        yAxis.axisLine.lineStyle = {};
        yAxis.axisLine.lineStyle.color = "#ccc";

        //标线
        if (data.MarkLine.Max != null && data.MarkLine.Max != "") {
            var markline = {};

            markline.type = "line";

            markline.markLine = {};

            markline.markLine.precision = 3;
            markline.markLine.data = [];
            markline.markLine.label = {};
            markline.markLine.label.position = "middle";
            var max = {};
            max.name = "最大值";
            max.yAxis = data.MarkLine.Max;
            max.lineStyle = {};
            max.lineStyle.color = "#f44444";

            var min = {};
            min.name = "最小值";
            min.yAxis = data.MarkLine.Min;
            min.lineStyle = {};
            min.lineStyle.color = "#f44444";

            markline.markLine.data.push(max);
            markline.markLine.data.push(min);

            seriesarray.push(markline);

            yAxis.min = unity.toDecimal(data.MarkLine.Min - (data.MarkLine.Max - data.MarkLine.Min) / 2, 3);
            yAxis.max = unity.toDecimal(data.MarkLine.Max + (data.MarkLine.Max - data.MarkLine.Min) / 2, 3);
            yAxis.maxInterval = unity.toDecimal((data.MarkLine.Max - data.MarkLine.Min) / 10, 3);

        }
        Bar(container, data.ColumnName, userarray, datearray, seriesarray, yAxis);
    }

    var containercount = unity.flitercount(function (e) { return e == container }, containerarray);
    containerchangearray[containercount] = "false";
    isrefresh = true;
}

//var myChart = null;
function Line(container, title, userarray, datearray, seriesarray, yAxis) {
    //var colorL = ['#01D8DB', '#FFBB44', '#3EA8FE', '#7BCE4C', 'rgb(0 ,0 ,255)', 'rgb(70 ,130 ,180)', 'rgb(0 ,139 ,139)', 'rgb(0, 255, 255)', 'rgb(78 ,238 ,148)', 'rgb(0,191 ,255)', 'rgb(46 ,139 ,87)'];
    //if (myChart != null && myChart != "" && myChart != undefined) {
    //    myChart.dispose();
    //}
    var myChart = echarts.init(document.getElementById(container));
    //console.log(datearray);
    var app = {};
    option = null;
    option = {
        title: {
            text: title,
            left: 'center',
            textStyle: {
                color: '#00D7DB'
            },
            triggerEvent: true
        },
        legend: {
            top: '10%',
            data: userarray,
            textStyle: {
                color: '#ccc'
            }
        },
        tooltip: {
            trigger: 'item',
            precision: 3,
            //formatter:"{a}：{c}<br />时间:",
            formatter: function (params) {

                //console.log(params.seriesIndex + "---" + params.dataIndex);
                return params.seriesName + ":" + params.value + "<br />" + "时间:" + datearray[params.seriesIndex][params.dataIndex];
            }
        },
        grid: {
            top: '25%',
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: {
            type: 'category',
            boundaryGap: false,
            axisLine: {
                lineStyle: {
                    color: '#ccc' //控制颜色
                }
            },
        },
        yAxis: yAxis,
        series: seriesarray
    };

    if (option && typeof option === "object") {
        myChart.setOption(option, true);

        myChart.on('click', 'title', function (params) {
            //console.log("click:" + params.componentType);
            Change(container);
        });

    }
}

function Pie(container, title, dataarray) {
    var dom = document.getElementById(container);
    var myChart = echarts.init(dom);
    var app = {};
    option = null;
    option = {
        //backgroundColor: '#fff',

        title: {
            text: title,
            left: 'center',
            textStyle: {
                color: '#00D7DB',
            },
            triggerEvent: true
        },

        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },

        visualMap: {
            show: false,
            min: 80,
            max: 600,
            inRange: {
                colorLightness: [0, 1]
            }
        },

        series: [
            {
                name: title,
                type: 'pie',
                radius: '70%',
                center: ['50%', '50%'],
                data: dataarray.sort(function (a, b) { return a.value - b.value; }),
                roseType: 'radius',
                label: {
                    normal: {
                        textStyle: {
                            color: '#00D7DB'
                        }
                    }
                },
                labelLine: {
                    normal: {
                        lineStyle: {
                            color: '#00D7DB'
                        },
                        smooth: 0.2,
                        length: 10,
                        length2: 20
                    }
                },
                itemStyle: {
                    normal: {
                        color: '#3EA8FE',
                        shadowBlur: 50,
                        shadowColor: 'rgba(0, 0, 0, 0.7)'
                    }
                },

                animationType: 'scale',
                animationEasing: 'elasticOut',
                animationDelay: function (idx) {
                    return Math.random() * 200;
                }
            }
        ]
    };;
    if (option && typeof option === "object") {
        myChart.setOption(option, true);
        myChart.on('click', 'title', function (params) {
            //console.log("click:" + params.componentType);
            Change(container);
        });
    }
}

function Bar(container, title, userarray, datearray, seriesarray, yAxis) {
    var dom = document.getElementById(container);
    var myChart = echarts.init(dom);
    var app = {};
    //var colorL = ['#01D8DB', '#FFBB44', '#3EA8FE', '#7BCE4C', 'rgb(0 ,0 ,255)', 'rgb(70 ,130 ,180)', 'rgb(0 ,139 ,139)', 'rgb(0, 255, 255)', 'rgb(78 ,238 ,148)', 'rgb(0,191 ,255)', 'rgb(46 ,139 ,87)'];
    option = null;
    app.title = title;
    option = {
        title: {
            text: title,
            left: 'center',
            textStyle: {
                color: '#00D7DB'
            },
            triggerEvent: true
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
            }
        },
        legend: {
            top: '10%',
            data: userarray,
            textStyle: {
                color: '#8ec4ed',
            }
        },
        grid: {
            top: '30%',
            left: '3%',
            right: '4%',
            bottom: '3%',
            containLabel: true
        },
        xAxis: [
            {
                type: 'category',
                data: datearray,
                axisLine: {
                    lineStyle: {
                        color: '#00D7DB' //控制颜色
                    }
                },
            }
        ],
        yAxis: [
              {
                  type: 'value',
                  axisLine: {
                      lineStyle: {
                          color: '#ccc' //控制颜色
                      }
                  },
              }
        ],
        series: seriesarray
    };


    if (option && typeof option === "object") {
        myChart.setOption(option, true);
        myChart.on('click', 'title', function (params) {
            //console.log("click:" + params.componentType);
            Change(container);
        });
    }
}

function Change(selecttype) {
    layer.closeAll();
    layer.open({
        type: 2,
        title: '选择表单',
        //shadeClose: true,
        shade: true,
        maxmin: true, //开启最大化最小化按钮
        area: ['80%', '80%'],
        content: 'select.html?type=' + selecttype
    });
}