<%@ Page Language="VB" AutoEventWireup="false" Inherits="exemptions._Default" Codebehind="Default.aspx.vb" %>
<%
    
    Dim clfVersion As String = System.Configuration.ConfigurationManager.AppSettings("SplashPageCLFNewVersion")
    
    
  %>

<clf:splash language="English" renderbutton="true" securehttp="true">
        <clf:title english="Transport Canada - Exemptions Search" french="Transports Canada - Recherche pour exemptions" />
        <clf:version value="<% =clfVersion%>" />
        <clf:link english="e_s.aspx?lang=eng"  french="e_s.aspx?lang=fra" />
</clf:splash>
