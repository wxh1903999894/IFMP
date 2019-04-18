<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TaskTableEdit.aspx.cs" Inherits="IFMP.taskflow.TaskTableEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <link href="../css/easyui.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/jquery.min.js"></script>
    <script src="../plugins/jquery.easyui.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });

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
                //console.log(hf_HintDictionaryID.value);
                if (hf_HintDictionaryID.value != null && hf_HintDictionaryID.value != "")//有提示字典
                {
                    var result = false;
                    var hf_RegexData = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_RegexData");
                    var txt_TextName = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_txt_TextName");
                    var hf_RegexType = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_RegexType");
                    var displaytype = document.getElementById(repeaterId + "_ctl" + getrownumber(i) + "_hf_DType").getAttribute("value");

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
                        else if (hf_RegexType.value == 15) {//有范围的数字
                            var begindata = regdata[0];
                            var enddata = regdata[1];
                            if (begindata > enddata) {
                                begindata = begindata + enddata;
                                enddata = begindata - enddata;
                                begindata = begindata - enddata;
                            }
                            if (txt_TextName.value > begindata && txt_TextName.value < enddata) {
                                result = true;
                            }
                        }
                    }
                    else {
                        //选择
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
                        if (confirm('输入信息不在数据范围内，确定要提交吗？')) {
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
        <%--<asp:HiddenField runat="server" ID="hf_Value" />--%>
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="2" align="left">
                            <asp:Literal runat="server" ID="ltl_TableType"></asp:Literal></th>
                    </tr>
                    <asp:Repeater runat="server" ID="rp_List" OnItemDataBound="rp_List_ItemDataBound">
                        <ItemTemplate>
                            <tr>
                                <td colspan="4" style="margin: 0; padding: 0;">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                                        <tbody>
                                            <tr>
                                                <td width="350px" align="right">
                                                    <%#Eval("ColumnName") %>：
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" ID="txt_TextName" placeholder='<%#Eval("DefaultData") %>'></asp:TextBox>
                                                    <span style="color: red;">
                                                        <asp:Literal runat="server" ID="ltl_RegexData" Visible="false"></asp:Literal></span>
                                                    <asp:RadioButtonList runat="server" ID="rdo_Name" RepeatDirection="Horizontal" RepeatLayout="Flow" CssClass="edilab"></asp:RadioButtonList>
                                                    <asp:HiddenField runat="server" ID="hf_DictionaryID" Value='<%#Eval("DictionaryID") %>' />
                                                    <asp:HiddenField runat="server" ID="hf_HintDictionaryID" Value='<%#Eval("HintDictionaryID") %>' />
                                                    <asp:HiddenField runat="server" ID="hf_DType" Value='<%#Eval("DType") %>' />
                                                    <asp:HiddenField runat="server" ID="hf_TableColumnID" Value='<%#Eval("ID") %>' />

                                                    <asp:HiddenField runat="server" ID="hf_hf_DefaultData" Value='<%#Eval("DefaultData") %>' />
                                                    <asp:HiddenField runat="server" ID="hf_RegexData" Value='<%#Eval("RegexData") %>' />
                                                    <asp:HiddenField runat="server" ID="hf_RegexType" Value='<%#Eval("RegexType") %>' />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    <tr id="trvis" runat="server">
                        <td align="center">
                            <asp:Button runat="server" ID="btn_Submit" CssClass="submit" Text="提交" OnClientClick="return GetData();" OnClick="btn_Submit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
