﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EmployeeEdit.aspx.cs" Inherits="IFMP.sysmanage.EmployeeEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/editinfor.js" type="text/javascript"></script>
    <script src="../plugins/My97/WdatePicker.js"></script>
    <script src="../plugins/Validform_v5.3.2.js" type="text/javascript"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery("#form1").Validform();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="4" align="left">用户信息</th>
                    </tr>
                    <tr>
                        <td align="right" width="100px">用户名：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_SysUserName"></asp:Literal>
                        </td>
                        <td align="right" width="100px">员工编号：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_UserNumber"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">姓名：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_RealName"></asp:Literal>
                        </td>
                        <td align="right">性别：</td>
                        <td align="left">
                            <asp:DropDownList runat="server" ID="ddl_Sex" datatype="ddl" errormsg="请选择性别"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">手机号码：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_CellPhone"></asp:Literal>
                        </td>
                        <td align="right">职务：</td>
                        <td align="left">
                            <asp:TextBox runat="server" ID="txt_JobName"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">部门：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_DepID" datatype="ddl" errormsg="请选择部门"></asp:DropDownList>
                            <span style="color: red;">*</span>
                        </td>
                        <td align="right">岗位：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_PostID"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">政治面貌：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_Polity"></asp:DropDownList>
                        </td>
                        <td align="right">民族：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_Nationality"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">生日：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_BirthDate" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                        <td align="right">入职日期：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_HireDate" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">试用时间：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_ProbationDays" Text="0"></asp:TextBox>
                        </td>
                        <td align="right">转正日期：</td>
                        <td>
                            <asp:TextBox runat="server" ID="txt_QualifiedDate" onclick="WdatePicker({skin:'whyGreen'})"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">状态：</td>
                        <td align="left">
                            <asp:Literal runat="server" ID="ltl_UserState"></asp:Literal>
                        </td>
                        <td align="right">员工分类：</td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddl_UserType"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">照片：</td>
                        <td colspan="3">
                            <asp:Image ID="img" Width="100px" Height="100px" Visible="false" runat="server" />
                            <div id="divicon">
                                <asp:FileUpload ID="fl_UpFile" runat="server" onchange="if(this.value)judgepic(this.value,this);" />
                            </div>
                            <asp:HiddenField ID="hf_UpFile" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right">家庭住址：
                        </td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txt_Address" TextMode="MultiLine" Width="60%" Height="80px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button runat="server" ID="btn_Submit" lay-submit="" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

