Option Strict Off

Imports System.data
Imports System.Data.OleDb
Imports System.Globalization

Partial Class results
  Inherits app_page

Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Me.Title = GetLocalResourceObject("PageTitle").ToString
            CLFPage.TagHelper.AddConfigsTag(Nothing, False, False, Me.Title)
        End If

        ShowSearchResults()
    End Sub

    Private Sub ShowSearchResults()
		Dim conn As OleDbConnection = Nothing
        Dim command As New OleDbCommand
        Dim reader As OleDbDataReader
        Dim sql As String = ""
        Dim exemption_type As String
		Dim exemption_kind As String
        Dim search_term As String = ""
        Dim field_to_search As String   ' this will be our field name in our sql statement below
        'Dim search_term_sql As String
        Dim purpose_field_name As String
		Dim doc_path, html_url As String

        exemption_type = CleanString(Request(Trim(UnNullStr("ddlType"))))
        field_to_search = CleanString(Request(Trim("ddlType"))) ' for some reason our field name is in our select box
        exemption_kind = CleanString(Request(Trim(UnNullStr("ddlKind"))))
        search_term = CleanString(Request(Trim(UnNullStr("txtSearchTerm"))))

        If lang = "fra" Then
            purpose_field_name = "Objet" ' save our purpose db field name to french
        Else
            purpose_field_name = "Purpose" ' save our purpose db field name to english
        End If

        OpenConnection(conn)

        sql = "SELECT ID, [Exempted Provision], [Issuing Office], OPI, [Company Name], [A/C Registration] as ac_registration,"
        sql += " [Issue Date], ExpiryDate, objet, purpose from [exemptions database]"
        sql += " WHERE [exmp-auth] = ?"
        command.Parameters.Add(New OleDbParameter("@p_exemption_kind", exemption_kind))

        If search_term <> "" Then

            Select Case field_to_search
                Case "1"
                    sql = sql & " And [A/C Registration]"
                Case "2"
                    sql = sql & " And [A/C Type]"
                Case "3"
                    sql = sql & " And [Exempted Provision]"
                Case "4"
                    sql = sql & " And [Company Name]"
                Case Else
                    Return
            End Select
            sql = sql & " Like '%' + ? + '%'"
            command.Parameters.Add(New OleDbParameter("@p_search_term", search_term))

        End If
        sql = sql & " ORDER BY [Issue Date] DESC"

        command.Connection = conn
        command.CommandText = sql
        reader = command.ExecuteReader()

        Dim temp_id As String
		Dim temp_exempted_provision As String
		Dim temp_issuing_office As String
		Dim temp_opi As String
		Dim temp_company_name As String
		Dim temp_ac_registration As String
		Dim temp_issue_date As String
		Dim temp_expiry_date As String
		Dim temp_purpose As String
		Dim temp_html_doc_exists As Boolean
		Dim temp_word_doc_exists As Boolean
		Dim temp_html_link As String
		Dim temp_word_link As String
		Dim temp_row As HtmlTableRow
        Dim temp_cell As HtmlTableCell

        If Not reader.HasRows Then
            'If Me.Language = LanguageType.French Then
            '	Response.Write("Votre recherche &#224; retourner 0 r&#233;sultats. ")
            'Else
            '	Response.Write("Your search returned 0 results. ")
            'End If
            ltNoResults.Text = "<br />" & ltNoResults.Text
            ltNoResults.Visible = True
        Else
            Dim di As New IO.DirectoryInfo(GetDocPaths)
            Dim arWordFiles As IO.FileInfo() = di.GetFiles("*.doc")
            Dim arWebFiles As IO.FileInfo() = di.GetFiles("*.htm")

            doc_path = GetDocPaths()
            html_url = GetHtmlUrl()

            Do While reader.Read
                temp_html_doc_exists = False
                temp_word_doc_exists = False
                temp_html_link = ""
                temp_word_link = ""
                temp_id = reader.Item("id")
                temp_exempted_provision = Trim(UnNullStr(reader.Item("exempted provision"))).Replace("&", "&amp;")
                temp_issuing_office = Trim(UnNullStr(reader.Item("issuing office")))
                temp_opi = Trim(UnNullStr(reader.Item("opi")))
                temp_company_name = Trim(UnNullStr(reader.Item("company name"))).Replace("&", "&amp;")
                temp_ac_registration = Trim(UnNullStr(reader.Item("ac_registration"))).Replace("&", "&amp;")
                temp_issue_date = FormatTCDate(Trim(UnNullStr(reader.Item("issue date"))))
                temp_expiry_date = FormatTCDate(Trim(UnNullStr(reader.Item("expirydate"))))
                temp_purpose = Trim(UnNullStr(reader.Item(purpose_field_name))).Replace("&", "&amp;")

                ' create our provision row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltProvisionResource1.text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_exempted_provision & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our issuing office row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltIssuingOfficeResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_issuing_office & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our opi row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltOPIResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_opi & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our company name row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                'temp_cell.NoWrap = True
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltCompanyNameResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_company_name & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our ac registration row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                'temp_cell.NoWrap = True
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltAircraftRegistrationResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_ac_registration & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our issue date row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                'temp_cell.NoWrap = True
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltIssueDateResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_issue_date & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our expiry date row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                'temp_cell.NoWrap = True
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltExpiryDateResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_expiry_date & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our purpose row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                temp_cell.VAlign = "top"
                temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltPurposeResource1.Text").ToString() & "</span>"
                temp_row.Cells.Add(temp_cell)
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_purpose & "</span>"
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)

                ' create our full details row if corresponding doc exists
                temp_html_doc_exists = CheckExists(arWebFiles, temp_id & ".htm") 'FileExists(doc_path & temp_id & ".htm")
                temp_word_doc_exists = CheckExists(arWordFiles, temp_id & ".doc") 'FileExists(doc_path & temp_id & ".htm")'FileExists(doc_path & temp_id & ".doc")

                If temp_html_doc_exists = True Or temp_word_doc_exists = True Then
                    ' create our html link if html doc exists 
                    If temp_html_doc_exists = True Then
                        temp_html_link = "<span class=" & """" & "content" & """" & "><a href=" & """" & html_url & temp_id & ".htm" & """>HTML</a></span>" & "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"
                    End If

                    ' create our word link if html doc exists 
                    If temp_word_doc_exists = True Then
                        temp_word_link = "<span class=" & """" & "content" & """" & "><a href=" & """" & html_url & temp_id & ".doc" & """" & ">MS WORD</a></span>"
                    End If

                    temp_row = New HtmlTableRow
                    temp_cell = New HtmlTableCell
                    'temp_cell.NoWrap = True
                    temp_cell.InnerHtml = "<span class=" & """" & "contentbold" & """" & ">" & GetLocalResourceObject("ltViewDetailsResource1.Text").ToString() & "</span>"
                    temp_row.Cells.Add(temp_cell)
                    temp_cell = New HtmlTableCell
                    temp_cell.InnerHtml = "<span class=" & """" & "content" & """" & ">" & temp_html_link & temp_word_link & "</span>"
                    temp_row.Cells.Add(temp_cell)
                    tblSearchResults.Rows.Add(temp_row)
                End If

                ' create our <hr> row
                temp_row = New HtmlTableRow
                temp_cell = New HtmlTableCell
                temp_cell.InnerHtml = "<hr /><br />"
                temp_cell.ColSpan = 2
                temp_row.Cells.Add(temp_cell)
                tblSearchResults.Rows.Add(temp_row)
            Loop
        End If

        reader.Close()
        command.Dispose()
        CloseConnection(conn)
	End Sub

    Private Function CheckExists(ByRef arFileList As IO.FileInfo(), ByVal sFileName As String) As Boolean
        Dim i As Integer
        Dim RetVal = False

        For i = 0 To arFileList.Length - 1
            If arFileList(i).Name = sFileName Then
                RetVal = True
                Exit For
            End If
        Next

        Return RetVal
    End Function
End Class