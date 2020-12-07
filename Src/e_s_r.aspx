<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false" Codebehind="e_s_r.aspx.vb" Inherits="exemptions.results" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<div class="row">
    <div class="mrgn-bttm-sm col-md-12">
        <asp:hyperlink id="hlNewSearch" runat="server" meta:resourcekey="hlNewSearchResource1"	navigateurl="~/e_s.aspx" text="New Search"></asp:hyperlink>
    </div>
    
    <div class="clearfix"></div>

    <div class="mrgn-bttm-sm col-md-12">
        <asp:literal id="ltNoResults" text="Your search returned 0 results." runat="server"	visible="False" meta:resourcekey="ltNoResults1"></asp:literal>
    </div>

    <div class="mrgn-bttm-sm col-md-12">
        <table id="tblSearchResults" border="0" cellpadding="2" cellspacing="2" runat="server">
            <%--See code behind--%>
        </table>
    </div>
</div>
</asp:Content>
