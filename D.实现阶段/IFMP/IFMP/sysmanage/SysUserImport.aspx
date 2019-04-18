<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SysUserImport.aspx.cs" Inherits="IFMP.sysmanage.SysUserImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../css/green_formcss.css" rel="stylesheet" />
    <script src="../plugins/jquery-1.8.2.min.js"></script>
    <script src="../plugins/Validform_v5.3.2.js"></script>
    <script src="../plugins/editinfor.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="listcent pad0">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="listinfo">
                <tbody>
                    <tr>
                        <th colspan="2" align="left">用户导入信息</th>
                    </tr>
                    <tr>
                        <td align="right">用户信息：</td>
                        <td align="left">
                            <asp:FileUpload ID="fuimport" onchange="if(this.value)judgexls(this.value,this);"
                                runat="server" />&nbsp;&nbsp;<asp:LinkButton ID="lbtn_example" Style="color: blue" runat="server"
                                    OnClick="lbtn_example_Click">[模板下载]</asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">导入说明：</td>
                        <td align="left">
                            <span style="color: red;">1.请先下载用户信息导入模板并严格按照用户信息导入模板录入数据进行导入；<br />
                                2.表头标红数据均不能为空，以免导入失败；<br />
                                3.日期请按照以下格式填写如（2016-08-08）；<br />
                                4.部门、岗位信息请按照系统中已有数据进行填写，以免导入失败；
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btn_Sumbit" runat="server" Text="提交" CssClass="submit" OnClick="btn_Sumbit_Click" />
                            <input type="button" name="button" id="cancell" value="取消" class="editor" onclick=' $.close("A_id");' />
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </form>
</body>
</html>
