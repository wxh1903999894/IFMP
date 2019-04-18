var showtext = "河南飞龙（芜湖）汽车零部件有限公司欢迎您！";
var showtime = 20000;
var isalert = false;
var nowtime = 0;
$(document).ready(function () {
    /*显示日期*/
    var date = new Date();                //得到当前日期原始模式
    var newyear = date.getFullYear();     //得到当前日期年份
    var newmonth = date.getMonth() + 1;   //得到当前日期月份（注意： getMonth()方法一月为 0, 二月为 1, 以此类推。）
    var day = date.getDate();            //得到当前某日日期（1-31）
    newmonth = (newmonth < 10 ? "0" + newmonth : newmonth);  //10月以下的月份自动加0
    var newdate = newmonth + "月" + day + "日";
    $(".data").text(newdate);

    /*文字滚动*/
    function time1() {
        //function format(i) {
        //    return (i < 10 ? "0" + i : i);
        //}
        if (isalert) {
            if (nowtime > showtime) {
                isalert = false;
            } else {
                nowtime = nowtime + 100;
            }
            document.getElementById("time").style = "color:red";
        } else {
            nowtime = 0;
            showtext = "河南飞龙（芜湖）汽车零部件有限公司欢迎您！";
            document.getElementById("time").style = "";
        }

        document.getElementById("time").innerHTML = showtext;
    }
    var interval = setInterval(time1, 100);

    /*时钟*/
    time();
    ampm();
    whatDay();
    setInterval(function () {
        time();
        ampm();
        whatDay();
    }, 1000);


    setInterval(function () {

        getAlert();
    }, 1000);

    //时钟函数
    function time() {
        var date = new Date(),
          hours = date.getHours(),
          minutes = date.getMinutes(),
          seconds = date.getSeconds();

        //make clock a 12 hour clock instead of 24 hour clock
        //hours = (hours > 12) ? (hours - 12) : hours;
        //hours = (hours === 0) ? 12 : hours;

        //invokes function to make sure number has at least two digits
        hours = addZero(hours);
        minutes = addZero(minutes);
        seconds = addZero(seconds);

        //changes the html to match results
        document.getElementsByClassName('hours')[0].innerHTML = hours;
        document.getElementsByClassName('minutes')[0].innerHTML = minutes;
        document.getElementsByClassName('seconds')[0].innerHTML = seconds;
    }

    //turns single digit numbers to two digit numbers by placing a zero in front
    function addZero(val) {
        return (val <= 9) ? ("0" + val) : val;
    }

    //lights up either am or pm on clock
    function ampm() {
        var date = new Date(),
          hours = date.getHours(),
          am = document.getElementsByClassName("am")[0].classList,
          pm = document.getElementsByClassName("pm")[0].classList;


        (hours >= 12) ? pm.add("light-on") : am.add("light-on");
        (hours >= 12) ? am.remove("light-on") : pm.remove("light-on");
    }

    //lights up what day of the week it is
    function whatDay() {
        var date = new Date(),
          currentDay = date.getDay(),
          days = document.getElementsByClassName("day");

        //iterates through all divs with a class of "day"
        for (x in days) {
            //list of classes in current div
            var classArr = days[x].classList;

            (classArr !== undefined) && ((x == currentDay) ? classArr.add("light-on") : classArr.remove("light-on"));
        }
    }



    //做一个获取最近的报警
    function getAlert() {
        $.ajax({
            url: "../ashx/BaseData.ashx",
            type: "GET",
            dataType: "json",
            async: false,
            data: {
                "method": "GetAlertMessage",
            },
            success: function (data) {
                console.log(data);
                if (data.result == "success") {
                    if (data.data != null && data.data != "") {
                        console.log(data);
                        showtext = data.data;
                        isalert = true;
                        nowtime = 0;
                    }
                }
            }
        })
    }
})





