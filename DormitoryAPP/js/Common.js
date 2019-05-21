//$(function () {
//    var id = $.cookie('UserID');
//    if (id == null) {
//        var href = window.location.href;
//        var url = href.split("/");
//        var rurl = encodeURI(encodeURI(url[url.length - 1]));
//        window.location.href = '../DDLogin.aspx?rurl=' + rurl;
//    }
//});


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
    cutFile: function (file, cutSize) {
        var count = file.size / cutSize | 0, fileArr = [];
        for (var i = 0; i < count ; i++) {
            fileArr.push({
                name: file.name + ".part" + (i + 1),
                file: file.slice(cutSize * i, cutSize * (i + 1))
            });
        };
        fileArr.push({
            name: file.name + ".part" + (count + 1),
            file: file.slice(cutSize * count, file.size)
        });
        return fileArr;
    },
    photoCompress: function (file, w, objDiv) {
        var ready = new FileReader();
        /*开始读取指定的Blob对象或File对象中的内容. 当读取操作完成时,readyState属性的值会成为DONE,如果设置了onloadend事件处理程序,则调用之.同时,result属性中将包含一个data: URL格式的字符串以表示所读取文件的内容.*/
        ready.readAsDataURL(file);
        ready.onload = function () {
            var re = this.result;
            unity.canvasDataURL(re, w, objDiv)
        }
    },
    canvasDataURL: function (path, obj, callback) {
        var img = new Image();
        img.src = path;
        img.onload = function () {
            var that = this;
            // 默认按比例压缩
            var w = that.width,
                h = that.height,
                scale = w / h;
            w = obj.width || w;
            h = obj.height || (w / scale);
            var quality = 0.7;  // 默认图片质量为0.7
            //生成canvas
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext('2d');
            // 创建属性节点
            var anw = document.createAttribute("width");
            anw.nodeValue = w;
            var anh = document.createAttribute("height");
            anh.nodeValue = h;
            canvas.setAttributeNode(anw);
            canvas.setAttributeNode(anh);
            ctx.drawImage(that, 0, 0, w, h);
            // 图像质量
            if (obj.quality && obj.quality <= 1 && obj.quality > 0) {
                quality = obj.quality;
            }
            // quality值越小，所绘制出的图像越模糊
            var base64 = canvas.toDataURL('image/jpeg', quality);
            // 回调函数返回base64的值
            callback(base64);
        }
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
    html_encode: function (str) {
        var s = "";
        if (str.length == 0) return "";
        s = str.replace(/&/g, "&gt;");
        s = s.replace(/</g, "&lt;");
        s = s.replace(/>/g, "&gt;");
        s = s.replace(/ /g, "&nbsp;");
        s = s.replace(/\'/g, "&#39;");
        s = s.replace(/\"/g, "&quot;");
        s = s.replace(/\n/g, "<br>");
        return s;
    },
    checkNum: function (value) {
        var patrn = /^(-)?\d+(\.\d+)?$/;
        if (patrn.exec(value) == null || value == "") {
            return false
        } else {
            return true
        }
    },
    InitNode: function (nodeName, nodeClass) {
        var node = document.createElement(nodeName);
        if (nodeClass != "" && nodeClass != null)
            node.className = nodeClass;
        return node;
    },
    objectEqual: function (a, b) {
        var propsA = Object.getOwnPropertyNames(a),
            propsB = Object.getOwnPropertyNames(b);
        if (propsA.length != propsB.length) {
            return false;
        }
        for (var i = 0; i < propsA.length; i++) {
            var propName = propsA[i];
            //如果对应属性对应值不相等，则返回false

            if (a[propName] !== b[propName]) {
                //console.log(propName + ":" + a[propName] + "--" + b[propName]);
                return false;
            }
        }
        return true;
    }
};