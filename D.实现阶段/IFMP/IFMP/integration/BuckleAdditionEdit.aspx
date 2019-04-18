<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BuckleAdditionEdit.aspx.cs" Inherits="IFMP.integration.BuckleAdditionEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/jquery-3.3.1.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
        function showboxFirstAduitUser() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelete.aspx', "&flag=5", 920, 585, 1);
        }
        function showboxLastAduitUser() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelete.aspx', "&flag=6", 920, 585, 1);
        }
        function showbox() {
            return parent.openbox('S_id', '../sysmanage/SysUserSelete.aspx', "&flag=1", 1100, 600, 8);
        }
    </script>
    <style>
        .edilab label {
            float: none;
        }

        .edilab input {
            height: 13px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_UID" runat="server" />
        <asp:HiddenField ID="hf_SID" runat="server" />
        <asp:HiddenField ID="hf_Score" runat="server" />
        <asp:HiddenField ID="hf_jkry" runat="server" Value="" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">奖扣信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">事件：
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddl_First" datatype="ddl" errormsg="请选择事件" runat="server" OnSelectedIndexChanged="ddl_First_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_Second" datatype="ddl" errormsg="请选择事件" runat="server" OnSelectedIndexChanged="ddl_Second_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <asp:DropDownList ID="ddl_EventName" datatype="ddl" errormsg="请选择事件" runat="server" OnSelectedIndexChanged="ddl_EventName_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                            <span style="color: Red">*</span></td>
                    </tr>
                    <tr>
                        <td align="right">主题：
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txt_STitle" runat="server" CssClass="searchbg" Width="400px" datatype="*" nullmsg="请录入主题"></asp:TextBox>
                            <span style="color: Red">*</span></td>
                    </tr>
                    <tr>
                        <td align="right">奖扣日期：
                        </td>
                        <td>
                            <asp:TextBox ID="txt_VDate" runat="server" Width="75px" datatype="*" nullmsg="请选择奖扣日期" CssClass="searchbg" onfocus="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                            <span style="color: Red">*</span></td>
                        <td align="right" width="100px">是否有奖票：
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rbl_IsReward" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">奖扣对象：
                        </td>
                        <td colspan="3">
                            <%--<asp:TextBox runat="server" ID="txt_UserID"></asp:TextBox>--%>
                            <img src="../images/checkbtn.gif" id="btn_plancom" runat="server" onclick="showbox()" />
                            <span id="vuser" runat="server"></span>
                            <span style="color: Red">*</span>
                        </td>
                    </tr>
                    <tr id="tr" runat="server">
                        <td align="right">初审人：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_FirstAduitUser" datatype="ddl" errormsg="请选择初审人" runat="server"></asp:DropDownList>
                            <span style="color: Red">*</span>
                        </td>
                        <td align="right">终审人：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_LastAduitUser" datatype="ddl" errormsg="请选择终审人" runat="server"></asp:DropDownList>
                            <span style="color: Red">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">图片： </td>
                        <td colspan="3">
                            <asp:Image ID="img" Width="100px" Height="100px" Visible="false" runat="server" />
                            <div id="divicon">
                                <asp:FileUpload ID="fl_UpFile" runat="server" onchange="if(this.value)judgepic(this.value,this);" />
                            </div>
                            <asp:HiddenField ID="hf_UpFile" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">事件说明：
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txt_EventMark" runat="server" Rows="6" Height="100px" Width="98%" Style="resize: none;" TextMode="MultiLine" CssClass="searchbg"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClientClick="tj()" OnClick="btn_Sumbit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
<script>
    function say(ids) {
        var uid = "";
        var uids = "";
        if (document.getElementById("hf_jkry").value == "") {
            uid = jqzqorzhygzf(ids);
            uids = uid;
        }
        else {
            uid = jqzqorzhygzf(ids) + "," + document.getElementById("hf_jkry").value;
            var arr = uid.split(',');
            for (var i = 0; i < arr.length; i++) {
                if (uids.indexOf(arr[i]) == -1) {
                    uids += arr[i] + ",";
                }
            }
            uids = jqzqorzhygzf(uids);
        }
        document.getElementById("hf_jkry").value = uids;
        var name = "";
        var item = "";
        $.ajax({
            url: "../ashx/GetBaseDate.ashx",
            cache: false,
            async: false,
            type: "get",
            dataType: "json",
            data: "method=GetName&id=" + uids,
            success: function (data) {
                if (data.result == "true") {
                    name = data.data[0].name;
                }
            }
        })
        var arruid = uids.split(',');
        var arrname = name.split(',');
        item += "<br/><label id=\"lbl-0\">快捷设置</label><img id=\"aa\" src=\"../images/allpic.png\" style=\"margin-bottom: -3px; margin-left: 5px; margin-right: 5px;\" onclick=\"jiaorjian(this)\"><input id=\"txt-0\" value=\"1\" style=\"margin-right:5px;width:25px;\" onchange=\"kjsz()\" />";
        for (var i = 0; i < arruid.length; i++) {
            item += "<br/><label id=" + arruid[i] + " style=\"margin:0;\">" + arrname[i] + "</label><img src=\"../images/allpic.png\" style=\"margin-bottom: -3px; margin-left: 5px; margin-right: 5px;\"><input id=\"txt-" + arruid[i] + "\" value=\"1\" style=\"margin-right:5px;width:25px;\"/><img src=\"../images/del.png\"/>";
        }
        jQuery("#vuser").html(item);
    }

    function kjsz(obj) {
        jQuery("#vuser input").each(function () {
            jQuery(this).val(jQuery("#txt-0").val());
        });
    }

    function jiaorjian(obj) {
        if (jQuery(obj).attr("src") == "../images/allpic.png") {
            jQuery("#vuser img").each(function () {
                if (jQuery(this).attr("src") != "../images/del.png" && jQuery(this).attr("id") != "aa") {
                    jQuery(this).attr("src", "../images/allinpic.png")
                }
            });
        }
        else {
            jQuery("#vuser img").each(function () {
                if (jQuery(this).attr("src") != "../images/del.png" && jQuery(this).attr("id") != "aa") {
                    jQuery(this).attr("src", "../images/allpic.png");
                }
            });
        }
    }
    jQuery("#vuser").on("click", "img", function () {
        if (jQuery(this).parent().attr("id") != 0) {
            if (jQuery(this).attr("src") == "../images/del.png") {
                jQuery(this).prev().prev().prev().prev().remove();
                jQuery(this).prev().prev().prev().remove();
                jQuery(this).prev().prev().remove();
                jQuery(this).prev().remove();
                jQuery(this).remove();
                getuidorscore();
            }
            else {
                if (jQuery(this).attr("src") == "../images/allpic.png") {
                    jQuery(this).attr("src", "../images/allinpic.png");
                }
                else {
                    jQuery(this).attr("src", "../images/allpic.png");
                }
            }
        }
    })

    function getuidorscore() {
        jQuery("#hf_Score").val("");
        jQuery("#hf_UID").val("");
        jQuery("#vuser input").each(function () {
            if (jQuery(this).attr("id") != "txt-0") {
                if (jQuery(this).prev().attr("src") == "../images/allpic.png") {
                    jQuery("#hf_Score").val(jQuery("#hf_Score").val() + ',' + jQuery("#" + jQuery(this).attr("id")).val());
                }
                else {
                    jQuery("#hf_Score").val(jQuery("#hf_Score").val() + ',-' + jQuery("#" + jQuery(this).attr("id")).val());
                }
            }
        });
        jQuery("#vuser label").each(function () {

            if (jQuery(this).attr("id") != "lbl-0") {
                jQuery("#hf_UID").val(jQuery("#hf_UID").val() + ',' + jQuery(this).attr("id"));
            }
        });
        if (jQuery("#hf_Score").val() != "") {
            jQuery("#hf_Score").val(jqzqorzhygzf(jQuery("#hf_Score").val()));
        }
        if (jQuery("#hf_UID").val() != "") {
            jQuery("#hf_UID").val(jqzqorzhygzf(jQuery("#hf_UID").val()));
        }
    }
    function tj() {
        getuidorscore();
    }
    function jqzqorzhygzf(ids) {
        if (ids != "") {
            if (ids.substr(0, 1) == ",") {
                ids = ids.substr(1, ids.length - 1);
            }
            if (ids.substr(ids.length - 1, 1) == ",") {
                ids = ids.substr(0, ids.length - 1);
            }
        }
        return ids;
    }
</script>
