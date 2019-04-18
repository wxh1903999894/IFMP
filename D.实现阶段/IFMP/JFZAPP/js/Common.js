$(function () {
    var id = $.cookie('UserID');
    if (id == null)
    {
        var href = window.location.href;
        var url = href.split("/");
        var rurl =encodeURI(encodeURI( url[url.length - 1]));
        window.location.href = '../DDLogin.aspx?rurl=' + rurl;
    }
});