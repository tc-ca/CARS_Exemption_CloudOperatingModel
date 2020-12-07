Imports System.Reflection

Partial Public Class show_app_settings
	Inherits app_page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

		Dim server_name As String
		Dim doc_path As String
		Dim html_url As String
		Dim settings As String = String.Empty
		Dim objGlobal As Global_asax

		If Me.Language = LanguageType.French Then
			Me.LanguageAction = "show_app_settings.aspx?lang=eng" & CleanUpQueryString(Request.QueryString.ToString)
		Else
			Me.LanguageAction = "show_app_settings.aspx?lang=fra" & CleanUpQueryString(Request.QueryString.ToString)
		End If



		server_name = LCase(HttpContext.Current.Request.ServerVariables("server_name"))
		If server_name = "tc.gc.ca" Or server_name = "www.tc.gc.ca" Or server_name = "tcinfo" Or server_name = "tcinfo.tc.gc.ca" Then
			Response.Redirect("http://www.tc.gc.ca")
		End If

		If txtServerName.Text.Trim <> String.Empty Then
			server_name = txtServerName.Text.Trim.ToLower()
		End If

		objGlobal = New Global_asax()
		doc_path = GetDocPaths(objGlobal.databaseToUse(server_name))
		html_url = GetHtmlUrl(objGlobal.databaseToUse(server_name))

    ' just doug testing Weiguang's code.
		Dim subject As String = String.Empty

		subject = "Exemptions"
		subject = HttpContext.Current.Server.HtmlDecode(subject)

		settings += "<strong>App settings from web.config</strong><br><br>"
		settings += "The document path is:<br />"
		settings += doc_path
		settings += "<br /><br />"
		settings += "The document url path is:<br />"
		settings += html_url
		settings += "<br /><br />"

		settings += "The connection string is:<br />"
		settings += ConfigurationManager.ConnectionStrings("Production").ToString()

		' display a nice re-assuring message to our users know that this functionality will not be available in production
		settings += "<br><br>"
		settings += "<span style='color:Red'>* Note: this page has been coded to ensure that it will not be available in production.<br><br>"
		divSettings.InnerHtml = settings
  End Sub




	Protected Sub butShowConnectionString_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles butShowConnectionString.Click
		'Response.Write(ConfigurationManager.ConnectionStrings("Production").ToString())
	End Sub




End Class