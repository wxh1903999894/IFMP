<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WebPager.ascx.cs" Inherits="IFMP.webcontrols.WebPager" %>
<div class="fy">
    <asp:Panel runat="server" DefaultButton="ibtnGo">
        共<span class="number"><asp:Literal ID="ltlRecordCount" runat="server" Text="0"></asp:Literal></span>条记录 每页<span class="number">
            <%--<asp:Literal ID="ltlPageSize" runat="server" Text="10"></asp:Literal>--%>
            <asp:DropDownList runat="server" ID="ddl_PageSize" OnSelectedIndexChanged="ddl_PageSize_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem Value="10">10</asp:ListItem>
                <asp:ListItem Value="15">15</asp:ListItem>
                <asp:ListItem Value="20">20</asp:ListItem>
                <asp:ListItem Value="30">30</asp:ListItem>
                <asp:ListItem Value="40">40</asp:ListItem>
                <asp:ListItem Value="50">50</asp:ListItem>
            </asp:DropDownList>
        </span>条 共<span class="number">
            <asp:Literal ID="ltlPageCount" runat="server"></asp:Literal></span>页
        <asp:LinkButton ID="lbtnPrevious" runat="server" CausesValidation="false" OnCommand="ToggleCommon_Click"
            CommandArgument="previous">上一页</asp:LinkButton>

        <asp:LinkButton ID="lbtnNext" runat="server" CausesValidation="false" OnCommand="ToggleCommon_Click"
            CommandArgument="next">下一页</asp:LinkButton>

        <asp:LinkButton ID="lbtnFirst" runat="server" CausesValidation="false" OnCommand="ToggleCommon_Click"
            CommandArgument="first">首页</asp:LinkButton>

        <asp:LinkButton ID="lbtnLast" runat="server" CausesValidation="false" OnCommand="ToggleCommon_Click"
            CommandArgument="last">末页</asp:LinkButton>
        跳转到
           <asp:TextBox ID="txtCurrentPage" CssClass="tzbg" runat="server" MaxLength="4" Width="30px"></asp:TextBox>页     
        <asp:Button ID="ibtnGo" runat="server" CausesValidation="false" Text="跳转" OnCommand="ToggleCommon_Click"
            CommandArgument="go" CssClass="tzbt" />
        <asp:RegularExpressionValidator ID="revCurrentPage" runat="server" ControlToValidate="txtCurrentPage"
            ValidationExpression="\d*" SetFocusOnError="true"></asp:RegularExpressionValidator>
        <asp:RangeValidator ID="rvCurrentPage" runat="server" ControlToValidate="txtCurrentPage"
            SetFocusOnError="true" Type="Integer"></asp:RangeValidator>
    </asp:Panel>
</div>
