<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysUserInfo.aspx.cs" Inherits="IFMP.sysmanage.SysUserInfo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/editinfor.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
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
                        <td align="right" width="100px">用户名</td>
                        <td width="45%">
                            <asp:Literal runat="server" ID="ltl_SysUserName"></asp:Literal>
                        </td>
                        <td align="right" width="100px">姓名</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_RealName"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">员工编号</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_EmpCode"></asp:Literal>
                        </td>
                        <td align="right">身份证号</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Identity"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">手机号码</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_CellPhone"></asp:Literal>
                        </td>
                        <td align="right">性别</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Sex"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">部门</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_DepID"></asp:Literal>
                        </td>
                        <td align="right">岗位</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_PostID"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">工作类型分类</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_UserLeaveType"></asp:Literal>
                        </td>
                        <td align="right">职务</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_JobName"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">政治面貌</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Polity"></asp:Literal>
                        </td>
                        <td align="right">民族</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Nationality"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">生日</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Birthdate"></asp:Literal>
                        </td>
                        <td align="right">入职日期</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_Begindate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">试用时间</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_periodDay"></asp:Literal>
                        </td>
                        <td align="right">转正日期</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_CorrectionDate"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">状态</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_UserState"></asp:Literal>
                        </td>
                        <td align="right">员工分类</td>
                        <td>
                            <asp:Literal runat="server" ID="ltl_UserType"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">家庭住址</td>
                        <td colspan="3">
                            <asp:Literal runat="server" ID="ltl_Censusaddr"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">照片</td>
                        <td colspan="3">
                            <asp:Image runat="server" ID="img_Photo" />
                        </td>
                    </tr>
                    <tr>
                        <th colspan="4" align="left">密码信息</th>
                    </tr>
                    <tr>
                        <td align="right">原密码</td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txt_OldPwd" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">新密码</td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txt_NewPwd" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">输入新密码</td>
                        <td colspan="3">
                            <asp:TextBox runat="server" ID="txt_AgainPwd" TextMode="Password"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button runat="server" ID="btn_Submit" lay-submit="" CssClass="submit" Text="提交" OnClick="btn_Submit_Click" />
                            <%--<input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />--%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>

