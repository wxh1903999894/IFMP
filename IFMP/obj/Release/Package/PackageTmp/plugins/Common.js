
//转化日期格式
Date.prototype.Format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,
        "d+": this.getDate(),
        "h+": this.getHours(),
        "m+": this.getMinutes(),
        "s+": this.getSeconds(),
        "q+": Math.floor((this.getMonth() + 3) / 3),
        "S": this.getMilliseconds()
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};


var unity = {
    dealWithDate: function (date) {
        return date.replace("/Date(", "").replace(")/", "");
    },
    stringToDate: function (str) {
        return new Date(Date.parse(str.replace(/-/g, "/")));
    },
    getRequest: function () { //获取url中"?"符后的字串
        var url = location.search;
        var thisRequest = [];
        if (url.indexOf("?") != -1) {
            var str = url.substr(1);
            strs = str.split("&");
            for (var i = 0; i < strs.length; i++) {
                thisRequest[strs[i].split("=")[0]] = decodeURI(strs[i].split("=")[1]);
            }
        }
        return thisRequest;
    },
    getURL: function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    },
    InitNode: function (nodeName, nodeClass) {
        var node = document.createElement(nodeName);
        if (nodeClass != "" && nodeClass != null)
            node.className = nodeClass;
        return node;
    },
    fliter: function (func, arr) {
        var r = [];
        for (var i = 0; i < arr.length; i++) {
            if (func(arr[i], i, arr)) {
                r.push(arr[i]);
            }
        }
        return r;
    },
    fliterdata: function (func, arr) {
        for (var i = 0; i < arr.length; i++) {
            if (func(arr[i], i, arr)) {
                return arr[i];
            }
        }
        return null;
    },
    flitercount: function (func, arr) {
        for (var i = 0; i < arr.length; i++) {
            if (func(arr[i], i, arr)) {
                return i;
            }
        }
        return -1;
    },
    toDecimal: function (x, number) {
        var f = parseFloat(x);
        if (isNaN(f)) {
            return;
        }
        var k = 1;
        for (var i = 0; i < number; i++) {
            k = k * 10;
        }
        f = Math.round(x * k) / k;
        return f;
    }
};