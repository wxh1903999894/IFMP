
//var $$ = jQuery.noConflict();
var printtype = 1;
$(document).ready(function () {


    mui("#segmentedControl").on('tap', '.jpqh1', function (event) {
        $('.printed').show();
        $('.noprint').show();
        printtype = 1;
    });

    mui("#segmentedControl").on('tap', '.jpqh2', function (event) {
        $('.printed').show();
        $('.noprint').hide();
        printtype = 2;
    });

    mui("#segmentedControl").on('tap', '.jpqh3', function (event) {
        $('.printed').hide();
        $('.noprint').show();
        printtype = 3;
    });
    //多行文本文字占位点击消失

    //iphone按钮点击active失效：
    document.body.addEventListener('touchstart', function () { })

    // 监听tap事件，解决 a标签 不能跳转页面问题
    mui('nav').on('tap', 'a', function () { document.location.href = this.href; });

    //得到日期  
})

function InitPrint() {
    if (printtype == 1) {
        $('.printed').show();
        $('.noprint').show();
    } else if (printtype == 2) {
        $('.printed').show();
        $('.noprint').hide();
    } else if (printtype == 3) {
        $('.printed').hide();
        $('.noprint').show();
    }

}

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)"); //构造一个含有目标参数的正则表达式对象
    var r = window.location.search.substr(1).match(reg);  //匹配目标参数
    if (r != null) return unescape(r[2]); return null; //返回参数值
}

var unity = {
    photoCompress: function (file, w, objDiv) {
        var ready = new FileReader();
        /*开始读取指定的Blob对象或File对象中的内容. 当读取操作完成时,readyState属性的值会成为DONE,如果设置了onloadend事件处理程序,则调用之.同时,result属性中将包含一个data: URL格式的字符串以表示所读取文件的内容.*/
        ready.readAsDataURL(file);
        ready.onload = function () {
            var re = this.result;
            unity.canvasDataURL(re, w, objDiv);
        }
    },
    canvasDataURL: function (path, obj, callback) {
        var img = new Image();
        img.src = path;
        img.onload = function () {
            var w = this.width,
                h = this.height,
                scale = w / h;
            w = obj.width || w;
            h = obj.height || (w / scale);
            var quality = 0.7;//默认图片质量为0.7
            //生成canvas
            var canvas = document.createElement('canvas');
            var ctx = canvas.getContext('2d');
            //创建属性节点
            var anw = document.createAttribute("width");
            anw.nodeValue = w;
            var anh = document.createAttribute("height");
            anh.nodeValue = h;
            canvas.setAttributeNode(anw);
            canvas.setAttributeNode(anh);
            ctx.drawImage(this, 0, 0, w, h);
            //图片质量
            if (obj.quality && obj.quality <= 1 && obj.quality > 0) {
                quality = obj.quality;
            }
            //quality值越小，所绘制的图像越模糊
            var base64 = canvas.toDataURL('image/jpeg', quality);
            //回调函数返回base64的值
            callback(base64);
        }
    }
}