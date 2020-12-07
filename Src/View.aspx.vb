Imports System.IO
Partial Public Class View
	Inherits app_page

	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		Dim fileName As String
		Dim temp_id As String
		Dim lang As String

		If Not IsPostBack Then
			If Me.Language = LanguageType.French Then
				lang = "fr"
			Else
				lang = "en"
			End If


			If Not Request.QueryString("temp_id") Is Nothing Then
				temp_id = Request.QueryString("temp_id")
				fileName = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings(strServerName + "_doc_file_path")) & lang & "\" & temp_id & ".htm"
				Read_File(fileName)
			End If
		End If
	End Sub

	Sub Read_File(ByVal filename As String)
		'Open a file for reading

		'Get a StreamReader class that can be used to read the file
		Dim objStreamReader As StreamReader
		objStreamReader = File.OpenText(filename)

		'Now, read the entire file into a string
		Dim contents As String = objStreamReader.ReadToEnd()

		'Set the text of the file to a Web control
		divContent.InnerHtml = contents
		objStreamReader.Close()
		objStreamReader.Dispose()
	End Sub


End Class