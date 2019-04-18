//==去除空格
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}

//==根据ID获取对象
var $id = function (id) {
    return typeof id == "string" ? document.getElementById(id) : id;
};

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

//======打印
function preview(mess) {
    //获取要打印的内容
    bdhtml = window.document.body.innerHTML;
    sprnstr = "<!--" + mess + "startprint-->";
    eprnstr = "<!--" + mess + "endprint-->";
    prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 21);
    prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
    if (mess == "page") {
        str1 = prnhtml.substring(0, prnhtml.indexOf("<!--btnstart-->"));
        str2 = prnhtml.substr(prnhtml.indexOf("<!--btnend-->") + 13);
        prnhtml = str1 + str2;
    }
    prnhtml = prnhtml.replace("form1 content1", "form content");
    prnhtml = "<table width='100%'><tr><td>" + prnhtml;
    prnhtml = prnhtml + "</td></tr></table>";
    //document.write(prnhtml)
    //寻找插件
    try {
        LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        if ((LODOP != null) && (typeof (LODOP.VERSION) != "undefined")) {
            var strBodyStyle = "<style>table.content {border-top: 1px solid #333; border-right: 1px solid #333;}.form th {height:30px; line-height:30px;font-size:13px; font-weight:bold; border-bottom: 1px solid #333; border-left: 1px solid #333;}.form tr {text-align:center }.form td { line-height:26px;border-left: 1px solid #333;border-bottom: 1px solid #333;font-size:12px;}</style>";
            prnhtml = strBodyStyle + prnhtml;
            LODOP.SET_PREVIEW_WINDOW(0, 0, 0, 860, 640, "");
            LODOP.ADD_PRINT_TABLE("1%", "0.5cm", "RightMargin:0.5cm", 600, prnhtml);
            LODOP.PREVIEW();
            return false;
        }
        if (!LODOP) {
            return false;
        }
    } catch (err) { }
}

function print() {
    bdhtml = window.document.body.innerHTML;
    sprnstr = "<!--pagestartprint-->";
    eprnstr = "<!--pageendprint-->";
    prnhtml = bdhtml.substr(bdhtml.indexOf(sprnstr) + 21);
    prnhtml = prnhtml.substring(0, prnhtml.indexOf(eprnstr));
    prnhtml = "<table width='100%'><tr><td>" + prnhtml;
    prnhtml = prnhtml + "</td></tr></table>";

    try {
        LODOP = getLodop(document.getElementById('LODOP_OB'), document.getElementById('LODOP_EM'));
        if ((LODOP != null) && (typeof (LODOP.VERSION) != "undefined")) {
            jQuery.get('../js/XMLWordStyle.xml', function (xml) {
                prnhtml = "<style>" + jQuery(xml).find("wordstyle").text() + "</style>" + prnhtml;
                LODOP.SET_PREVIEW_WINDOW(0, 0, 0, 860, 640, "");
                LODOP.ADD_PRINT_TABLE("0", "-30", "0", 600, prnhtml);
                LODOP.PREVIEW();
            });
            return false;
        }
        if (!LODOP) {
            return false;
        }
    } catch (err) { }
}

//==根据ID获取金额
var $money = function (id) {
    if (typeof id == "string") {
        try {
            if (document.getElementById(id).value != null) {
                return document.getElementById(id).value.replace(/[,]/g, "").Trim();
            }
            else {
                return document.getElementById(id).innerHTML.replace(/[,]/g, "").Trim();
            }
        }
        catch (Error) {
            alert("ID错误！");
        }
    }
    else if (typeof id == "object") {
        try {
            if (id.value != null) {
                return id.value.replace(/[,]/g, "").Trim();
            }
            else {
                return id.innerHTML.replace(/[,]/g, "").Trim();
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

//==检查输入的是否为数字
function check() {
    if (!((window.event.keyCode >= 48 && window.event.keyCode <= 57) || window.event.keyCode == 46)) {
        window.event.keyCode = 0
    }
}

//==关闭窗口
function winclose(ucid) {
    var uc = "";
    if (arguments.length > 0) {
        uc = ucid + "_";
    }
    if (parent.document.getElementById(uc + "btn_Query")) {
        parent.document.getElementById(uc + "btn_Query").click();
    }
    else {
        parent.location.reload();
    }
}
function Refresh() {
    if (parent.document.getElementById("btn_Query")) {
        parent.document.getElementById("btn_Query").click();
    }
    else {
        parent.location.reload();
    }
}

//==删除信息提示框
function delmessage(mess) {
    return confirm("系统提示：您确认删除" + mess + "吗？");
}

//==获取选中的checkbox的ID值
function getckid(vessel, hfile) {
    var ids = "";
    var ckbox = document.getElementById(vessel).getElementsByTagName("input");
    for (var i = 0; i < ckbox.length; i++) {
        if (ckbox[i].type == "checkbox" && ckbox[i].checked) {
            ids = ids + ckbox[i].id + ",";
        }
    }
    $id(hfile).value = ids;
}

//==设置附件个数
function getfile() {
    //附件
    var hfcount = $id("hfcount");
    var upfile = jQuery("#more").find("input[type='file']");
    hfcount.value = upfile.length;
    //附件类型
    var hftype = $id("hftype");
    hftype.value = "";
    var type = jQuery("#more").find("select");
    for (var i = 0; i < type.length; i++) {
        if (upfile[i].value != "")
            hftype.value += type[i].value + ",";
    }
}

//==添加删除按钮
function adddelnum(d) {
    var im = document.createElement("img");
    im.setAttribute("src", "../images/sq.png");
    im.style.cssText = "margin-left:4px;margin-bottom:-4px;cursor:pointer";
    im.onclick = function () {
        return delfile(this, "DIV");
    }
    d.appendChild(im);
}

//==公用删除
function delfile(f, name) {
    while (f.tagName != name)
        f = f.parentNode;
    f.parentNode.removeChild(f);
    return false;
}

var num = 1;


//======添加附件上传控件
function addfile(id) {
    var d = document.createElement("div");
    var f = document.createElement("input");
    f.setAttribute("type", "file");
    f.setAttribute("name", "upfile");
    f.setAttribute("style", "margin-top:3px;margin-bottom:3px;");
    f.onchange = function () {
        if (this.value) judge(this.value, this);
    }
    d.appendChild(f);

    var im = document.createElement("img");
    im.setAttribute("src", "../images/delbtn.gif");
    //im.setAttribute("style", "margin-left:3px;cursor:pointer");
    im.style.cssText = "margin-left:3px;cursor:pointer; margin-bottom:-3px";
    im.onclick = function () {
        return delfile(this, "DIV");
    }
    d.appendChild(im);
    document.getElementById(id).appendChild(d);
    return false;
}

//==动态绑定数据
function BindData(id, eflag) {
    var sele = document.getElementById(id);
    $.ajax({
        url: "../sysdatamanage/EnumDataAjax.aspx?eflag=" + encodeURI(eflag),
        type: "post",
        async: false,
        dataType: "html",
        success: function (data) {
            if (data == "-1") {
                alert("EFlag参数错误！");
            }
            else if (data == "") {
                alert("未加载到数据！");
            }
            else {
                if (typeof data == "string") {
                    xml = new ActiveXObject("Microsoft.XMLDOM");
                    xml.async = false;
                    xml.loadXML(data);
                } else {
                    xml = data;
                }
                sele.length = 0;
                $(xml).find("Data").each(function () {
                    var text = $(this).text();
                    var value = $(this).attr("ID");
                    sele.options.add(new Option(text, value));
                    if (text == "其他")
                        sele.options.selected = true;
                });
                $(sele).find('option').each(function () {
                    this.selected = (this.text == "其他");

                });
            }
        },
        error: function () {
            alert("数据加载失败！");
        }
    });
}

//==判断doc和xls文件
function judge(file, node) {

    var typelist = ["txt", "doc", "xls", "ppt", "docx", "xlsx", "pptx", "pdf", "jpeg", "jpg", "png", "bmp", "gif", , "rar", "zip", "dwg"];
    if (file) {
        var match = 0;
        var suffix = file.split(".");
        var num = suffix.length - 1;
        var name = suffix[num].toLowerCase();
        for (var i = 0; i < typelist.length; i++) {
            if (name == typelist[i]) {
                match = 1;
                break;
            }
        }
        if (match != 1) {
            alert("暂不支持上传该类型的文件，请重新选择！");
            node.outerHTML = node.outerHTML;
        }
    }
    getFileSize(node);
}

//==判断doc和docx文件
function judgedoc(file, node) {
    if (file) {
        var last = file.substring(file.lastIndexOf(".") + 1);
        if (last != "doc"&&la !="docx") {
            alert("上传类型错误，只支持.doc和docx文件！");
            node.outerHTML = node.outerHTML;
        }
    }
}


//==判断xls文件
function judgexls(file, node) {
    if (file) {
        var last = file.substring(file.lastIndexOf(".") + 1);
        if (last != "xls") {
            alert("上传类型错误，只支持.xls文件！");
            node.outerHTML = node.outerHTML;
        }
    }
}

//==判断rar文件
function judgezip(file, node) {
    if (file) {
        var last = file.substring(file.lastIndexOf(".") + 1);
        if (last != "zip") {
            alert("上传类型错误，只支持.zip文件！");
            node.outerHTML = node.outerHTML;
        }
    }
}

//==判断图片文件
function judgepic(file, node) {
    if (file) {
        var suffix = file.split(".");
        var num = suffix.length - 1;
        var name = suffix[num].toLowerCase();
        if (name != "jpeg" && name != "jpg" && name != "gif" && name != "bmp" && name != "png") {
            alert("上传类型错误，暂只支持.jpeg|.jpg|.gif|.bmp|.png的图片格式！");
            node.outerHTML = node.outerHTML;
        }
    }
}

//==判断上传文件大小
function getFileSize(obj) {
    var size = 0;
    if (navigator.userAgent.indexOf("MSIE") > 0) {
        try {
            //注意，在IE中需要将网站添加到受信任的站点中（Internet选项-安全-受信任的站点-站点-添加）
            var fso = new ActiveXObject('Scripting.FileSystemObject'); //获取上传文件的对象
            var file = fso.GetFile(obj.value);
            size = file.Size;
        }
        catch (err) {
            size = 0;
        }
    }
    else {
        size = obj.files[0].size;
    }
    if ((size / 1048576) > 30) {
        alert("上传失败，上传文件不能大于30M！");
        obj.outerHTML = obj.outerHTML;
    }
}