
//==面加载事件
$(function () {
    //删除
    $("#btn_Delete").click(function () {
        return checkselectone("删除", "信息");
    });

    $("#btn_PwdReset").click(function () {
        return checkselectone("重置", "密码信息");
    });

    $("#btn_Cancel").click(function () {
        return checkselectone("注销", "信息");
    });

    $("#btn_Confirm").click(function () {
        return checkselectone("确认提交", "信息");
    });

    //重置密码
    $("#btn_PassWord").click(function () {
        return checkselectone("重置密码", "用户");
    });

    //归还
    $("#btn_Back").click(function () {
        return checkselectone("归还", "资产");
    });

    //公示
    $("#btn_Public").click(function () {
        return checkselectone("公示", "信息");
    });
    //取消公示
    $("#btn_CancelPublic").click(function () {
        var text = document.getElementById("btn_CancelPublic").value;
        return checkselectone(text, "信息");
    });
    //取消注册
    $("#btn_UnRegister").click(function () {
        var text = document.getElementById("btn_UnRegister").value;
        return checkselectone("取消注册", "信息");
    });
    $("#btn_Report").click(function () {
        return checkselectone("报到", "信息");
    });
    $("#btn_PStateq").click(function () {
        return checkselectone("启用", "信息");
    });
    $("#btn_PStatej").click(function () {
        return checkselectone("禁用", "信息");
    });
});

//==全选框勾选事件
//==单独使用，手动加在CheckBox中，并绑定value值为所关联的Tabla的Class名
function setid(thisid) {
    var checkid = document.getElementById("hf_CheckIDS").value;
    var thische = document.getElementById(thisid);
    if (thische.checked) {
        checkid = checkid + thisid.replace('ck_', '') + ",";
        document.getElementById(thisid + "l").className = "yxz";
    } else {
        document.getElementById(thisid + "l").className = "wxz";
        checkid = checkid.replace(thisid.replace('ck_', '') + ",", '');
    }
    checkid = checkid.replace(thisid + ",", '');
    document.getElementById("hf_CheckIDS").value = checkid;
    //alert(document.getElementById("hf_CheckIDS").value);
}

function setidandname(thisid) {
    //console.log(1);
    var checkid = document.getElementById("hf_CheckIDS").value;
    var checkname = document.getElementById("hf_CheckNames").value;
    var thische = document.getElementById(thisid);
    var thisname = document.getElementById(thisid.replace("ck", "name"));
    if (thische.checked) {
        checkid = checkid + thisid.replace('ck_', '') + ",";
        checkname = checkname + thisname.value + ",";
        document.getElementById(thisid + "l").className = "yxz";
    } else {
        document.getElementById(thisid + "l").className = "wxz";
        checkid = checkid.replace(thisid.replace('ck_', '') + ",", '');
        checkname = checkname.replace(thisname.value + ",", '');
    }
    checkid = checkid.replace(thisid + ",", '');
    checkname = checkname.replace(checkname.value + ",", '');
    document.getElementById("hf_CheckIDS").value = checkid;
    document.getElementById("hf_CheckNames").value = checkname;
    //alert(document.getElementById("hf_CheckIDS").value);
}

///*checkbox样式结束*/
//全选js事件开始
function qx(thisname, thisid) {
    var rche = document.getElementsByName(thisname);
    var rid = document.getElementById(thisid);
    document.getElementById("hf_CheckIDS").value = "";
    var checkid = "";
    var val = null;
    if (rid.checked) {

        for (var i = 0,
        len = rche.length; i < len; i++) {
            var comp = rche[i];
            if (comp.disabled == false) {
                checkid = checkid + comp.id.replace('ck_', '') + ",";
                document.getElementById(comp.id).checked = "checked";
                document.getElementById(comp.id + "l").className = "yxz";
            }
        }
    } else {
        for (var i = 0,
        len = rche.length; i < len; i++) {
            var comp = rche[i];
            checkid = checkid.replace(comp.id.replace('ck_', '') + ",", '');
            document.getElementById(comp.id).checked = "";
            document.getElementById(comp.id + "l").className = "wxz";
        }
    }
    checkid = checkid.replace(thisid + ",", '');
    document.getElementById("hf_CheckIDS").value = checkid;
    //alert(document.getElementById("hf_CheckIDS").value);
}//全选js事件结束

//==至少选择一项
function checkselectone(mess, info) {
    var checkid = document.getElementById("hf_CheckIDS").value;
    var count = 0;
    var strs = new Array();
    strs = checkid.split(",");
    for (i = 0; i < strs.length; i++) {
        if (strs[i] != "") {
            count++;
            break;
        }
    }
    if (count == 0) {
        alert("系统提示：请至少选择一条信息！");
        return false;
    }
    else {
        return confirm("您确认" + mess + "选中的" + info + "吗？");
    }
}

//==至少选择一项
function checkselectones(checkid) {

    var count = 0;
    var strs = new Array();
    strs = checkid.split(",");
    for (i = 0; i < strs.length; i++) {
        if (strs[i] != "") {
            count++;
            break;
        }
    }
    if (count == 0) {
        return false;
    }
    else {
        return true;
    }
}

function checkctselect(mess, info) {
    var type = jQuery("#hftype").val();
    var checkid;
    if (type == "B") {
        checkid = parent.document.getElementById("hf_CheckIDS").value;
    }
    else if (type == "A") {
        checkid = document.getElementById("hf_CheckIDS").value;
    }
    var count = 0;
    var strs = new Array();
    strs = checkid.split(",");
    for (i = 0; i < strs.length; i++) {
        if (strs[i] != "") {
            count++;
            break;
        }
    }
    if (count == 0) {
        alert("系统提示：请至少选择一条信息！");
        return false;
    }
    else {
        return confirm("您确认" + mess + "选中的" + info + "吗？");
    }
}

//==至少选择两项
function checktwo() {
    var checkid = document.getElementById("hf_CheckIDS").value;
    var count = 0;
    var strs = new Array();
    strs = checkid.split(",");
    var idlist = "";
    for (i = 0; i < strs.length; i++) {
        if (strs[i] != "") {
            idlist += strs[i] + "|";
            count++;
        }
    }
    if (count < 2) {
        alert("系统提示：请至少选择两条信息！");
        return false;
    }
    else {
        document.getElementById("hflist").value = idlist;
        return true;
    }
}

//==只选择一项
function onlycheck(mess) {
    var checkid = document.getElementById("hf_CheckIDS").value;
    var flag = IsOnly(checkid);
    if (flag == 0)//未选
    {
        alert("系统提示：请选择一条信息!");
        return false;
    }
    else if (flag == -99)//修改多个
    {
        alert("系统提示：每次只可" + mess + "一条信息，请检查!");
        return false;
    }
    else {
        return true;
        //return confirm("您确认" + mess + "选中的信息吗？");
    }
}

//==弹出asyncbox的调用方法
//==调用方法 return openbox(box的ID，地址,标题,传值(id=1&name='ss')，宽，高,Type(-1为添加，0为修改,其他为查看))
function openbox(idval, urlval, dataval, widthval, heighval, type, isparent) {
    //通过参数个数，判断是flag的值
    if (arguments.length == 6) {
        isparent = 1;
    }
    //盒子标题
    var titleval = "";
    try {

        titleval = document.getElementById("lbl_Menuname").innerText;
    }
    catch (err) {
        //titleval = "<font color='red'>" + err + "</font>";
    }
    //是否存在分页控件
    if (document.getElementById("wucpg") && document.getElementById("lbtn_Add")) {
        try {
            var s = document.getElementById("hf_Page").value;
        }
        catch (err) {
            //            asyncbox.tips("此页面缺少ID为'hf_Page'的隐藏控件", 'error');
            //            return false;
            titleval += "<font color='red'>" + 此页面缺少ID为hf_Page的隐藏控件 + "</font>";
        }
    }
    //ID设置
    var parms = "";
    if (type == 10 || type == 11 || type == 12 || type == 15 || type == 16 || type == 20 || type == 21 || type == 22 || type == 23 || type == 28) {
        $("#hf_Page").val("1");
        var checkid = document.getElementById("hf_CheckIDS").value;
        var flag = IsOnly(checkid);

        if (flag == -99)//修改多个 提示返回
        {
            alert("系统提示：只可操作一条记录，请检查!");
            return false;
        }
        else {
            if (type == 10) {
                titleval += "_评论";
            }
            else if (type == 11) {
                titleval += "_离开定位";
            }
            else if (type == 12) {
                titleval += "_方案申请";
            }
            else if (type == 15) {
                titleval += "_更新状态";
            }
            else if (type == 16) {
                titleval += "_派工单申请";
            }
            else if (type == 20) {
                titleval += "_问题管理";
            }
            else if (type == 21) {
                titleval += "_上传";
            }
            else if (type == 22) {
                titleval += "_是否中标";
            }
            else if (type == 23) {
                titleval += "_会议纪要";
            }
            else if (type == 28) {
                titleval += "_资产借出";
            }
            parms = "&id=" + flag;
        }
    }
    else if (type == -2) {
        $("#hf_Page").val("1");
        var checkid = document.getElementById("hf_CheckIDS").value;
        var flag = checkselectones(checkid);

        if (flag == false)//修改多个 提示返回
        {
            alert("系统提示：至少选择一条记录，请检查!");
            return false;
        }
        else {
            if (type == -2) {
                titleval += "_教师退休";
            }
            parms = "&id=" + flag;
        }
    } else if (type == -3) {
        $("#hf_Page").val("1");
        var checkid = document.getElementById("hf_CheckIDS").value;
        var flag = checkselectones(checkid);

        if (flag == false)//修改多个 提示返回
        {
            alert("系统提示：至少选择一条记录，请检查!");
            return false;
        }
        else {
            titleval += "_审核";

            parms = "&id=" + checkid.split(',')[0];
        }
    }
    else {

        $("#hf_Page").val("");
        titleval = settitle(titleval, type);
    }
    var menu = "&menuname=" + titleval + "&pb=box";
    var datalist = dataval + parms + menu;
    //开启异步盒子
    open_async(idval, urlval, titleval, datalist, widthval, heighval, isparent);
    return false;
}

//==打开信息合并窗口
function openMergerbox(idval, urlval, dataval, widthval, heighval) {
    var titleval;
    try {
        titleval = document.getElementById("lbl_Menuname").innerText;
    }
    catch (err) {
        titleval = "<font color='red'>" + err + "</font>";
    }
    //ID
    var checkid = document.getElementById("hflist").value;
    var datalist = dataval + "&id=" + checkid + "&menuname=" + titleval + "&pb=box";
    //开启异步盒子
    open_async(idval, urlval, titleval, datalist, widthval, heighval, 1);
    return false;
}

//==打开异步盒子
//==etype：1 直接打开，2 从父级窗口打开
function open_async(idval, urlval, titleval, datalist, widthval, heighval, etype) {
    if (etype == 1) {
        asyncbox.open({
            id: idval,
            url: urlval,
            title: titleval,
            modal: false, //遮罩层
            drag: true, //移动
            data: datalist,
            width: widthval,
            height: heighval
        });
    }
    else if (etype == 2) {
        parent.asyncbox.open({
            id: idval,
            url: urlval,
            title: titleval,
            modal: true, //遮罩层
            drag: true, //移动
            data: datalist,
            width: widthval,
            height: heighval
        });
    }
    else {
        asyncbox.open({
            id: idval,
            url: urlval,
            title: titleval,
            modal: true, //遮罩层
            drag: true, //移动
            data: datalist,
            width: widthval,
            height: heighval,
            callback: function (action, opener) {
                if (action == 'close') {
                    try {
                        opener.Refresh();
                    }
                    catch (err) { }
                }
            }
        });
    }
}

//==标题设置
function settitle(title, flag) {
    switch (flag) {
        case -1: title += "_添加"; break;
        case 0: title += "_修改"; break;
        case 1: title += "_查看"; break;
        case 2: title += "调课"; break;
        case 3: title += "_导入"; break;
        case 4: title += "_详情"; break;
        case 5: title += "_参会人员"; break;
        case 6: title += "_会议联系人"; break;
        case 8: title = "选取人员"; break;
        case 9: title += "_教师退休"; break;
        case 13: title += "_退宿"; break;
        case 14: title += "_解除合同"; break;
        case 17: title += "_领用"; break;
        case 18: title += "_借用"; break;
        case 29: title += "_记录"; break;
        case 30: title += "_总结"; break;
        case 31: title += "_评价"; break;
        case 32: title += "_发布"; break;
        case 33: title += "_申请"; break;
        case 34: title += "_审核"; break;
        case 35: title += "_集中实习"; break;
        case 36: title += "_考生安排"; break;
        case 37: title += "_入库"; break;
        case 38: title += "_出库"; break;
        case 39: title += "_采购"; break;
        case 40: title += "_回复"; break;
        case 41: title += "_转发"; break;
        case 42: title = "安排宿舍"; break;
        case 43: title += "_设置管理员"; break;
        case 44: title += "_班主任安排"; break;
        case 45: title += "_选择审核人"; break;
        case 46: title += "_是否公开"; break;
        case 47: title += "_安排补考"; break;
        case 48: title += "_补考学生查看"; break;
        case 49: title = "_调课"; break;
        case 50: title += "_任课教师安排"; break;
        case 51: title += "_开始答题"; break;
        case 52: title += "_成绩查看"; break;
        case 53: title += "_项目添加"; break;
        case 54: title = "物品赔偿情况"; break;
        case 55: title += "_年级毕业"; break;
        case 56: title += "写日志"; break;
        case 57: title += "_选取班级"; break;
        case 58: title += "_分析"; break;
        case 59: title = "空间留言"; break;
        case 60: title += "_完成"; break;
        case 61: title += "_销假"; break;
        case 62: title += "_添加成绩"; break;
        case 63: title += "_选取考场"; break;
        case 64: title += "_科目添加"; break;
        case 65: title += "_报废"; break;
        case 66: title += "_归档"; break;
        case 67: title += "_批转"; break;
        case 68: title += "_缴费"; break;
        case 69: title += "_开始排课"; break;
        case 70: title += "_分配维修人员"; break;
        case 71: title += "_选取资产"; break;
        case 72: title += "_归还"; break;
        case 73: title += "_科目统一安排"; break;
        case 74: title += "_科目安排"; break;
        case 75: title += "_学生导入"; break;
        case 80: title += "_配置教师"; break;
        case 81: title += "_学生变动"; break;
        case 82: title += "_课程点名册"; break;
        case 83: title += "_抢单"; break;
        default: break;
    };
    return title;
}


//==修改判断
//==控件的使用 ：IsOnly(标志)
function IsOnly(checkid) {
    //判断只可选择一条
    var strs = new Array(); //定义一数组
    var strsid = new Array();
    strs = checkid.split(","); //字符分割        
    for (i = 0; i < strs.length; i++) {
        if (strs[i] != "")
            strsid.push(strs[i]);
    }
    if (strsid.length == 0)
        return 0;
    else if (strsid.length > 1)
        return -99;
    else
        return strsid[0];
}

//==提示信息框
function operatemessage(mess) {
    return confirm("系统提示：您确认" + mess + "吗？");
}

//==获取RadioButton选择
function checkradio(obj, flag) {
    var ck = $("table[class='form content']").find("input:" + obj);
    var id = "";
    for (var i = 0; i < ck.length; i++) {
        if (ck[i].checked) {
            document.getElementById("hf_CheckIDS").value = ck[i].value;
            if (flag == 1) {
                document.getElementById("hf_Grade").value = $(ck[i]).next.val();
            }
            else if (flag == 2) {
                document.getElementById("hf_IsOut").value = $(ck[i]).next.val();
                document.getElementById("hf_EqState").value = $(ck[i]).next.next.val();
            }
            break;
        }
    }
}



//==去除空格
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//==检查日期
function checkdate(begin, end) {
    var begindate = $("#" + begin).val().Trim();
    var enddate = $("#" + end).val().Trim();
    if (begindate != "" && enddate != "") {
        if (Date.parse(begindate.replace("-", "/")) > Date.parse(enddate.replace("-", "/"))) {
            alert("起始日期应小于终止日期");
            return false;
        }
    }
}

//==金额判断
function checkmoney(e, mess, flag) {
    var eval = e.value;
    if (eval != "") {
        if (flag == 1) {
            if (!eval.match(/^[0-9]*([.][0-9]{1,2})?$/)) {
                e.value = "";
                alert(mess + "输入不正确");
                e.focus();
            }
        }
        else {
            if (!eval.match(/^[0-9]*([.][0-9])?$/)) {
                e.value = "";
                alert(mess + "输入不正确");
                e.focus();
            }
        }
    }
}

//==关闭窗口
function Refresh() {
    if (parent.document.getElementById("imgbtn_inquiry")) {
        parent.document.getElementById("imgbtn_inquiry").click();
    }
    else {
        parent.location.reload();
    }
}

//==去除空格
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//==根据ID获取对象的值并去除空格
var $val = function (id) {
    if (typeof id == "string") {
        try {
            if (document.getElementById(id).value != null) {
                return document.getElementById(id).value.Trim();
            }
            else {
                return document.getElementById(id).innerHTML.Trim();
            }
        }
        catch (Error) {
            alert("ID错误！");
        }
    }
    else if (typeof id == "object") {
        try {
            if (id.value != null) {
                return id.value.Trim();
            }
            else {
                return id.innerHTML.Trim();
            }
        }
        catch (Error) {
            alert("参数错误！");
        }
    }
    else {
        alert("参数错误！");
    }
};

//==格式化金额
function formrun(objvalue) {
    var objlength = objvalue.length,
        dtmp = new String(objvalue).indexOf(".");
    var inttmp = 0,
        floattmp = -1;
    if (dtmp != -1) {
        inttmp = new String(objvalue).substring(0, dtmp);
        floattmp = new String(objvalue).substring(dtmp + 1, objlength + 1);
    }
    else {
        inttmp = new String(objvalue);
    }
    inttmp = inttmp.replace(/[^0-9]/g, "");
    var tmp = "", str = "0000";
    for (; inttmp.length > 3;) {
        var temp = new String(inttmp / 1000);
        if (temp.indexOf(".") == -1) {
            tmp = ",000" + tmp;
            inttmp = temp;
        }
        else {
            var le = new String(temp).split(".")[1].length;
            tmp = "," + new String(temp).split(".")[1] + str.substring(0, 3 - le) + tmp;
            inttmp = new String(temp).split(".")[0];
        }
    }
    inttmp = inttmp + tmp;
    return inttmp + runing(floattmp);
}

//rili
function SetCanler() {
    var type = document.getElementById("hf_CssFlag").value;
    if (type == "blue") {
        WdatePicker({ skin: 'whyBlue' });

    }
    else if (type == "green") {
        WdatePicker({ skin: 'whyGreen' });


    }
    else {
        WdatePicker({ skin: 'default' });
    }


}

//==整理小数部分
function runing(val) {
    if (val != "-1" && val != "") {
        var valvalue = 0 + "." + val;
        if (val.length >= 2) {
            valvalue = parseFloat(valvalue).toFixed(2);
        }
        var temp = "." + valvalue.split(".")[1];
        return temp;
    }
    else if (val != "0" && val == "") {
        return ".";
    }
    else {
        return "";
    }
}



function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

