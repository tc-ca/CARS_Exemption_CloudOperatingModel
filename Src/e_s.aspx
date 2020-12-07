<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false"  Codebehind="e_s.aspx.vb" Inherits="exemptions.search" MasterPageFile="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row">
        <div class="mrgn-bttm-sm col-md-12 alert alert-info">
            <p>
                <asp:literal id="ltDescription" text="This is a search page for exemptions from regulatory requirements." runat="server" meta:resourcekey="ltDescriptionResource1"></asp:literal>
            </p>
        </div>

        <div class="col-md-11 text-primary h3 mrgn-bttm-md">
            <strong><asp:literal id="ltSearchCriteria" runat="server" text="Search Criteria" meta:resourcekey="ltSearchCriteriaResource1"></asp:literal></strong>
        </div>

        <div class="clearfix"></div>

        <div class="col-md-4 mrgn-bttm-md">
            <asp:label id="lblSelectExemptionType" runat="server" text="Select exemption type:" associatedcontrolid="ddlKind" meta:resourcekey="lblSelectExemptionType"></asp:label>
        </div>
        <div class="col-md-7 mrgn-bttm-md">
            <asp:dropdownlist id="ddlKind" runat="server" tabindex="1" meta:resourcekey="ddlKindResource1">
                <asp:ListItem selected="True" value="1" meta:resourcekey="ListItemResource1">Exemption</asp:ListItem><asp:ListItem value="2" meta:resourcekey="ListItemResource2">Authorization</asp:ListItem>
            </asp:dropdownlist>
        </div>

        <div class="clearfix"></div>

        <div class="col-md-4 mrgn-bttm-md">
            <asp:label id="lblSearchOn" runat="server" text="Search on:" associatedcontrolid="ddlType" meta:resourcekey="lblSearchOn"></asp:label>
        </div>
        <div class="col-md-7 mrgn-bttm-md">
            <asp:dropdownlist id="ddlType" runat="server" tabindex="2" meta:resourcekey="ddlTypeResource1">
                <asp:ListItem selected="True" value="1" meta:resourcekey="ListItemResource3">Registration</asp:ListItem>
                <asp:ListItem value="2" meta:resourcekey="ListItemResource4">AC Type</asp:ListItem>
                <asp:ListItem value="3" meta:resourcekey="ListItemResource5">Provision</asp:ListItem>
                <asp:ListItem value="4" meta:resourcekey="ListItemResource6">Company Name</asp:ListItem>
            </asp:dropdownlist>  
        </div>

        <div class="clearfix"></div>

        <div class="col-md-4 mrgn-bttm-md">
            <asp:label id="lblSearchFor" runat="server" text="Search for:" associatedcontrolid="txtSearchTerm" meta:resourcekey="lblSearchFor"></asp:label>
        </div>
        <div class="col-md-7 mrgn-bttm-md">
            <asp:textbox id="txtSearchTerm" runat="server" tabindex="3" meta:resourcekey="txtSearchTermResource1"></asp:textbox>
        </div>

        <div class="clearfix"></div>

        <div class="col-md-11 mrgn-bttm-md">
            <asp:button id="btnSearch" runat="server" tabindex="4" text="search" meta:resourcekey="btnSearchResource1" />  
        </div>

        <div class="clearfix"></div>
    </div>
</asp:Content>