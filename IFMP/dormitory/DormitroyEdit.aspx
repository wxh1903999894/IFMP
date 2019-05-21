<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DormitroyEdit.aspx.cs" Inherits="IFMP.dormitory.DormitroyEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <style>
        .textbox.combo {
            margin: 5px 0;
        }

        .textbox-addon.textbox-addon-right {
            margin-right: 3px;
        }

        .combo .combo-arrow {
            height: 34px !important;
            margin-top: 2px !important;
            border-radius: 5px;
        }
    </style>
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/AddOption.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script type="text/javascript">
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hf_tflag" runat="server" />
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tr>
                    <td align="right">宿舍名称：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_User" datatype="*1-30" nullmsg="请输入宿舍名称" CssClass="searchbg" runat="server">
                        </asp:TextBox>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">备注：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_memo" runat="server">
                        </asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td align="right">宿舍编号：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_code" datatype="*" nullmsg="请输入宿舍编号" CssClass="searchbg" runat="server">
                        </asp:TextBox>
                        <span style="color: Red">*</span>
                    </td>
                </tr>
                <tr>
                    <td align="right">是否点检：
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbl_IsCheck" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab">
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td align="right" width="100px">宿舍人员：
                    </td>
                    <td>
                        <asp:TextBox ID="txt_SysID" cascadeCheck="false" runat="server" name="txt_SysID" multiline="true" multiple="true" onlyLeafCheck="true" url="../ashx/GetBaseDate.ashx?method=GetUserTxt&deptype=1" CssClass="easyui-combotree"></asp:TextBox>
                        <%--<div id="UserSelect" class="layui-input-block"></div>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" align="center">
                        <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                        <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>

<%--<script src="../plugins/jquery-3.3.1.js"></script>
<script src="../plugins/Common.js"></script>
<script src="../plugins/echarts.js"></script>
<script src="../plugins/layui/layui.js"></script>--%>
<%--<script>
    var fulluserdata = "";

    $(document).ready(function () {
        InitFullUser();
        InitTaskShift();
    })

    layui.config({
        base: '../plugins/layui/'
    }).extend({
        formSelects: 'formSelects-v4'
    });

    function InitFullUser() {
        $.ajax({
            url: "../ashx/GetBaseDate.ashx",
            type: "GET",
            dataType: "json",
            async: false,
            data: "method=GetUserByDepLayUI",
            success: function (data) {
                fulluserdata = data;
            },
            error: function () {
                alert("当前网络可能有错误");
            }
        });
    }

    function InitTaskShift() {
        $.ajax({
            url: "../ashx/GetBaseDate.ashx",
            type: "GET",
            dataType: "json",
            async: false,
            data: "method=GetDorUser",
            success: function (data) {
                if (data.result == "success") {
                    var UserSelect = document.getElementById("UserSelect");
                    var UserArray = [];
                    var DefaultUserArray = [];
                    for (var i = 0; i < data.User.length; i++) {
                        var userdata = unity.InitNode("lable", "");
                        userdata.innerText = data.User[i].Name;
                        userdata.id = data.User[i].UserID;
                        UserSelect.appendChild(userdata);

                        var userselect = unity.InitNode("select", "");
                        userselect.id = "full-" + data.User[i].UserID;
                        userselect.name = "full-" + data.User[i].UserID;
                        userselect.setAttribute("xm-select", "full-" + data.User[i].UserID);
                        userselect.setAttribute("xm-select-search-type", "dl");
                        userselect.setAttribute("xm-select-radio", "");
                        userselect.setAttribute("xm-select-search", "");
                        UserSelect.appendChild(userselect);
                        UserArray.push(userselect.id);
                        DefaultUserArray.push(userdata.id);
                    }

                    layui.use(['formSelects'], function () {
                        for (var i = 0; i < UserArray.length; i++) {
                            layui.formSelects.data(UserArray[i], 'local', {
                                arr: fulluserdata
                            });

                            var seluserdata = [];
                            seluserdata.push(DefaultUserArray[i]);
                            layui.formSelects.value(UserArray[i], seluserdata);
                        }
                    })
                } else {
                    alert("获取数据失败，请联系系统管理员");
                }
            },
            error: function () {
                alert("当前网络有错误");
            }
        });
    }
</script>--%>

