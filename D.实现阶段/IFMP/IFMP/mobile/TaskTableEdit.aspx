<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskTableEdit.aspx.cs" Inherits="IFMP.mobile.TaskTableEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../plugins/layui/css/layui.css" rel="stylesheet">
    <link href="../css/iconfont.css" rel="stylesheet">
    <link href="../css/mobilemain.css" rel="stylesheet" />
    <script src="../plugins/jquery-3.3.1.js"></script>
    <script src="../plugins/layui/layui.js"></script>
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script>
        jQuery(document).ready(function () {
            //jQuery("#form1").Validform();
        });

        function back() {
            window.history.back(-1);
        }

        function getrownumber(i) {
            if (i > 10) {
                return i;
            }
            else {
                return '0' + i;
            }
        }

        function GetData() {

            var repeaterId = "<%=rp_List.ClientID %>";//Repeater的客户端ID
            var rows = "<%=rp_List.Items.Count%>";//Repeater的行数
            for (var i = 0; i < rows; i++) {
                var hf_HintDictionaryID = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_HintDictionaryID");

                if (hf_HintDictionaryID != null && hf_HintDictionaryID.value != null && hf_HintDictionaryID.value != "")//有提示字典
                {
                    var result = false;
                    var hf_RegexData = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_RegexData");
                    var txt_TextName = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_txt_TextName");
                    var hf_RegexType = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_RegexType");
                    var displaytype = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_DType").getAttribute("value");

                    var columnname = txt_TextName.parentElement.parentElement.parentElement.firstElementChild.innerText;
                    var regexalert = txt_TextName.nextElementSibling.innerText;
                    //console.log(txt_TextName.nextElementSibling.innerText);

                    if (displaytype == 1) {
                        //填写                        
                        var regdata = hf_RegexData.value.split('|');

                        if (hf_RegexType.value == 92) {//特殊的一组字符
                            for (var j = 0; j < regdata.length; j++) {
                                var a = regdata[j];
                                if (txt_TextName.value == regdata[j]) {
                                    result = true;
                                }
                            }
                        }
                        else if (hf_RegexType.value == 15 || hf_RegexType.value == 22) {//有范围的数字
                            var begindata = regdata[0].split('°')[0];
                            var enddata = regdata[1].split('°')[0];
                            if (begindata > enddata) {
                                begindata = begindata + enddata;
                                enddata = begindata - enddata;
                                begindata = begindata - enddata;
                            }
                            //console.log(begindata);
                            //console.log(enddata);
                            console.log(txt_TextName.value.split('°')[0] >= begindata && txt_TextName.value.split('°')[0] <= enddata);
                            if (txt_TextName.value.split('°')[0] >= begindata && txt_TextName.value.split('°')[0] <= enddata) {
                                result = true;
                            }
                        }
                    } else {
                        //选择
                        //var regdata = hf_RegexData.value.split('|');
                        if (document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_DefaultData").value != null && document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_DefaultData").value != "") {
                            var defaultdata = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_DefaultData").value.split('|');
                            var radio = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_rdo_Name");
                            var seldata = 0;
                            for (var j = 0; j < radio.children.length; j++) {

                                if (radio.children[j].getAttribute("type") == "radio") {

                                    if (radio.children[j].checked) {
                                        seldata = radio.children[j].nextElementSibling.innerText;
                                    }

                                }
                            }

                            if (defaultdata.length > 0) {
                                for (var j = 0; j < defaultdata.length; j++) {
                                    if (seldata == defaultdata[j]) {
                                        result = true;
                                        break;
                                    }
                                }
                            } else {
                                result = true;
                            }
                        }

                    }


                    if (result == false) {
                        if (confirm(columnname + '的输入信息{' + txt_TextName.value + '}不在' + regexalert + '内，确定要提交吗？')) {
                            return true;
                        }
                        else {
                            return false;
                        }
                    }
                }
            }
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
        <div style="padding: 20px; background-color: #F2F2F2;">
            <div class="layui-row layui-col-space20">
                <div class="layui-col-sm12">
                    <div class="layui-card card-child">
                        <div class="layui-card-header">
                            <asp:Literal runat="server" ID="ltl_TableType"></asp:Literal>
                        </div>
                        <div class="layui-card-body">
                            <asp:Repeater runat="server" ID="rp_List" OnItemDataBound="rp_List_ItemDataBound">
                                <ItemTemplate>
                                    <div class="layui-form-item layui-form-text">
                                        <label class="layui-form-label"><%#Eval("ColumnName") %></label>
                                        <div class="layui-input-block">
                                            <span style="" class="layui-form-mid">
                                                <asp:TextBox runat="server" AUTOCOMPLETE="OFF" AutoCompleteType="Disabled" ID="txt_TextName" placeholder='<%#Eval("DefaultData") %>'></asp:TextBox>
                                                <span style="color: red;">
                                                    <asp:Literal runat="server" ID="ltl_RegexData" Visible="false"></asp:Literal></span>
                                                <%--<asp:DropDownList runat="server" ID="ddl_Name"></asp:DropDownList>--%>
                                                <asp:RadioButtonList runat="server" ID="rdo_Name" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab"></asp:RadioButtonList>
                                                <asp:HiddenField runat="server" ID="hf_DictionaryID" Value='<%#Eval("DictionaryID") %>' />
                                                <asp:HiddenField runat="server" ID="hf_HintDictionaryID" Value='<%#Eval("HintDictionaryID") %>' />
                                                <asp:HiddenField runat="server" ID="hf_DType" Value='<%#Eval("DType") %>' />
                                                <asp:HiddenField runat="server" ID="hf_TableColumnID" Value='<%#Eval("ID") %>' />
                                                <asp:HiddenField runat="server" ID="hf_RegexData" Value='<%#Eval("RegexData") %>' />
                                                <asp:HiddenField runat="server" ID="hf_RegexType" Value='<%#Eval("RegexType") %>' />
                                                <asp:HiddenField runat="server" ID="hf_DefaultData" Value='<%#Eval("DefaultData") %>' />
                                            </span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                            <div class="layui-form-item layui-form-text">
                                <label class="layui-form-label">填写人</label>
                                <div class="layui-input-block">
                                    <span style="" class="layui-form-mid">
                                        <asp:DropDownList runat="server" ID="ddl_User"></asp:DropDownList>
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <button style="background-color: red" type="button" class="layui-btn layui-btn-normal layui-btn-radius" onclick='back()'>返回</button>
        <asp:Button runat="server" ID="btn_Submit" CssClass="layui-btn layui-btn-normal layui-btn-radius" Text="提交" OnClientClick="return GetData();" OnClick="btn_Submit_Click" />
    </form>
</body>
</html>
