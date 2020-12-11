'Imports civav_corelib.dates
Imports Microsoft.VisualBasic
Imports System.Threading.Thread
Imports System.Globalization

Public Class app_page
    'Inherits TCLib.CLFPage
    Inherits System.Web.UI.Page
    Protected _lang As String
    Public show_errors As Boolean = False
    Public show_pnl1 As Boolean = True
    Public show_pnl2 As Boolean = False
    Private m_pageTitle As String

    Public Property PageTitle() As String
        Get
            Return m_pageTitle
        End Get
        Set(ByVal value As String)
            m_pageTitle = value
        End Set
    End Property

    Public Property lang() As String
        Get
            Return _lang
        End Get
        Set(ByVal value As String)
            _lang = value
        End Set
    End Property

    Private Sub LoadCLFPageHelperTags()
        SetLangURL()

        CLFPage.TagHelper.AddVersionTag(System.Configuration.ConfigurationManager.AppSettings("CLFVersion"))

        If Me.lang = "fra" Then
            CLFPage.TagHelper.AddLanguageTag(CLFPage.Languages.French)
            CLFPage.TagHelper.AddMetaTag("dc.title", "Recherche pour exemptions")
            CLFPage.TagHelper.AddMetaTag("dc.language", "fra")
            CLFPage.TagHelper.AddMetaTag("dc.subject", "Recherche pour exemptions")
            CLFPage.TagHelper.AddMetaTag("dc.creator", "Développement des applications et Gestion de la technologie")
            CLFPage.TagHelper.AddMetaTag("dc.keywords", "exemptions, authorizations, Règlement de l'aviation canadien, RAC")

            CLFPage.TagHelper.AddBreadCrumbTag("Aviation", "https://www.tc.gc.ca/fr/services/aviation.html")
            CLFPage.TagHelper.AddBreadCrumbTag("S&#233;curit&#233; a&#233;rienne", "http://www.tc.gc.ca/aviationcivile/securiteaerienne/menu.htm")
            CLFPage.TagHelper.AddBreadCrumbTag("Services de r&#232;glementation", "http://www.tc.gc.ca/aviationcivile/Servreg/menu.htm")

            CLFPage.TagHelper.AddBreadCrumbTag("Transport et infrastructure", "https://www.canada.ca/fr/services/transport.html")
            CLFPage.TagHelper.AddBreadCrumbTag("Aviation", "https://www.tc.gc.ca/fr/services/aviation.html")
            CLFPage.TagHelper.AddBreadCrumbTag("Recherche pour exemptions", "e_s.aspx?lang=fra")

            CLFPage.MenuHelper.Initialize("Browse by Mode") _
                .AddMenuItem("Aviation", ResolveUrl("https://www.tc.gc.ca/fr/services/aviation.html")) _
                .AddMenuItem("Transport maritime", ResolveUrl("https://www.tc.gc.ca/fr/services/maritime.html")) _
                .AddMenuItem("Transport ferroviaire", ResolveUrl("https://www.tc.gc.ca/fr/services/ferroviaire.html")) _
                .AddMenuItem("Transport routier", ResolveUrl("https://www.tc.gc.ca/fr/services/routier.html")) _
                .Create()
        Else
            CLFPage.TagHelper.AddLanguageTag(CLFPage.Languages.English)
            CLFPage.TagHelper.AddMetaTag("dc.title", "CARS Exemptions")
            CLFPage.TagHelper.AddMetaTag("dc.language", "eng")
            CLFPage.TagHelper.AddMetaTag("dc.subject", "CARS Exemptions")
            CLFPage.TagHelper.AddMetaTag("dc.creator", "Application Development and Technology Management")
            CLFPage.TagHelper.AddMetaTag("dc.keywords", "exemptions, authorizations, canadian aviation regulations, CARs")

            CLFPage.TagHelper.AddBreadCrumbTag("Transport and infrastructure", "https://www.canada.ca/en/services/transport.html")
            CLFPage.TagHelper.AddBreadCrumbTag("Aviation", "https://www.tc.gc.ca/en/services/aviation.html")
            CLFPage.TagHelper.AddBreadCrumbTag("Exemptions Search", "e_s.aspx")

            CLFPage.MenuHelper.Initialize("Exemptions Search") _
                .AddMenuItem("Exemptions Search", ResolveUrl("e_s.aspx")) _
                .Create()
            CLFPage.MenuHelper.Initialize("Browse by Mode") _
                .AddMenuItem("Aviation", ResolveUrl("https://www.tc.gc.ca/en/services/aviation.html")) _
                .AddMenuItem("Marine Transportation", ResolveUrl("https://www.tc.gc.ca/en/services/marine.html")) _
                .AddMenuItem("Rail Transportation", ResolveUrl("https://www.tc.gc.ca/en/services/rail.html")) _
                .AddMenuItem("Road Transportation", ResolveUrl("https://www.tc.gc.ca/en/services/road.html")) _
                .Create()
        End If
        ' Common Page Items
        CLFPage.TagHelper.AddMetaTag("dcterms.modified", "2012-03-01")
        CLFPage.TagHelper.AddMetaTag("dcterms.issued", "2012-03-01")
        ' Set the last modified date - we don't set the page title here, it is individually in each page
        CLFPage.TagHelper.AddConfigsTag(System.Configuration.ConfigurationManager.AppSettings("DateAppLastModified"), False, False)

    End Sub


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If lang = "fra" Then
            PageTitle = "Recherche pour exemptions"
        Else
            PageTitle = "Exemptions Search"
        End If
        Me.Title = PageTitle
        ' This code prevents the CLFPageHelpers from executing for async post backs request.
        ' Without this logic, using CLFPageHelpers with async post backs may cause some issues
        If ScriptManager.GetCurrent(Page) Is Nothing Then
            LoadCLFPageHelperTags()
        Else
            If Not ScriptManager.GetCurrent(Page).IsInAsyncPostBack Then
                LoadCLFPageHelperTags()
            End If
        End If

    End Sub

    Protected Overrides Sub InitializeCulture()
        SetLang()
        CurrentThread.CurrentCulture = New System.Globalization.CultureInfo(ConvertLangCodeToISO(Me.lang))
        CurrentThread.CurrentUICulture = CurrentThread.CurrentCulture
        MyBase.InitializeCulture()
    End Sub

    Private Sub SetLang()
        '
        ' The Treasury board CLF Page 2.0 standard states that the language code in the
        ' url must use the three-letter ISO 639-2/T language code , which is fra for French 
        ' and eng for English
        '
        Dim sLangUrl As String = ""
        If IsNothing(Request.QueryString("lang")) Then ' If lang query is found use that
            'If session is found, use that
            If Not (Session.Item("language") Is Nothing) Then
                _lang = CStr(Session.Item("language"))
            Else ' If no query string or session, default to english
                _lang = "eng"
            End If
        Else
            _lang = Request.QueryString("lang").ToString
        End If

        'For some reason the querystring appears sometimes in double. This handles that problem when it occurs.
        If _lang = "eng,eng" Then
            Response.Redirect("s_r.aspx?lang=eng")
        ElseIf _lang = "fra,fra" Then
            Response.Redirect("s_r.aspx?lang=fra")
        End If

        Session.Item("language") = _lang
    End Sub
    Private Sub SetLangURL()
        Dim sLangUrl As String = ""
        If IsNothing(Request.QueryString("lang")) Then ' If lang query is found use that
            ' Set the Language URL
            If Request.Url.ToString.IndexOf("?") = -1 Then
                sLangUrl = Request.Url.AbsoluteUri + "?lang=" + _lang
            Else
                sLangUrl = Request.Url.AbsoluteUri + "&lang=" + _lang
            End If
        Else
            sLangUrl = Request.Url.AbsoluteUri
        End If
        ' Switch French to English and vice versa
        If _lang = "eng" Then
            sLangUrl = sLangUrl.ToString.Replace("lang=eng", "lang=fra")
        Else
            sLangUrl = sLangUrl.ToString.Replace("lang=fra", "lang=eng")
        End If
        ' Encode URL to ensure W3C compliance
        CLFPage.TagHelper.AddActionsTag(HttpUtility.HtmlEncode(sLangUrl))
    End Sub

    Public Shared Function ConvertLangCodeToISO(ByVal lang_in As String) As String
        Select Case lang_in
            Case "fra"
                Return "fr-CA"
            Case Else
                Return "en-CA"
        End Select
        Return "en-CA"
    End Function
    Public Shared Function ConvertLangCodeToSingleChar(ByVal lang_in As String) As String
        Select Case lang_in
            Case "fra"
                Return "f"
            Case Else
                Return "e"
        End Select
        Return "e"
    End Function

    Public Shared Function CleanUpQueryString(ByVal qs As String) As String
        qs = Replace(qs, "&lang=eng", "")
        qs = Replace(qs, "&lang=fra", "")
        qs = Replace(qs, "&lang=", "")
        qs = Replace(qs, "lang=eng", "")
        qs = Replace(qs, "lang=fra", "")
        qs = Replace(qs, "lang=", "")

        Dim i As Integer
        Dim key_values() As String = Split(qs, "&")
        Dim temp_qs As String = ""

        For i = 0 To key_values.GetUpperBound(0)
            If key_values(i).Trim <> "" Then
                temp_qs = temp_qs & key_values(i)
                If i <> key_values.GetUpperBound(0) Then
                    temp_qs = temp_qs & "&amp;"
                End If
            End If
        Next

        Return "&amp;" & temp_qs
    End Function

    Public Sub AddStyleSheetReferences()
        ' used to be able to do this in Page_Load
        'Me.AddStyleSheetReference("../css/civ_av_style.css")
    End Sub

    'Private Sub AddMetaTags()
    '    If Me.Language = LanguageType.French Then
    '        Me.AddMetaTag("dc.title", "Recherche pour exemptions")
    '        Me.AddMetaTag("title", "Recherche pour exemptions")
    '        Me.AddMetaTag("dc.creator", "Gouvernement du Canada; Transports Canada; S&eacute;curit&eacute; et s&ucirc;ret&eacute;; Aviation civile; Services de r&eacute;glementation")
    '        Me.AddMetaTag("dc.contributor", "Nicole Girard (AARBH)")
    '        Me.AddMetaTag("dc.language", "fre")
    '        Me.AddMetaTag("dc.date", civav_corelib.dates.FormatTCDate(Now.Date.ToString))
    '        Me.AddMetaTag("dc.date.created", "2003-08-14")
    '        Me.AddMetaTag("dc.date.modified", "2003-08-14")
    '        Me.AddMetaTag("dc.date.valid", "2004-08-14")
    '        Me.AddMetaTag("dc.subject", "transport a&#233;rien, legislation")
    '        Me.AddMetaTag("keywords", "exemptions, autorisations, r&#232;glement de l'aviation canadien, rac")
    '        Me.AddMetaTag("dc.description", "Ceci est la page de recherche pour les exemptions des exigences r&#233;glementaires.")
    '        Me.AddMetaTag("description", "Ceci est la page de recherche pour les exemptions des exigences r&#233;glementaires.")
    '        Me.AddMetaTag("dc.subject.topic", "A&#233;rien")
    '        Me.AddMetaTag("dc.publisher", "Civil Aviation Web Team (AARC)")
    '    Else
    '        Me.AddMetaTag("dc.title", "Exemptions Search")
    '        Me.AddMetaTag("title", "Exemptions Search")
    '        Me.AddMetaTag("dc.creator", "Government of Canada; Transport Canada; Safety and Security; Civil Aviation; Regulatory Services")
    '        Me.AddMetaTag("dc.contributor", "Nicole Girard (AARBH)")
    '        Me.AddMetaTag("dc.language", "eng")
    '        Me.AddMetaTag("dc.date", civav_corelib.dates.FormatTCDate(Now.Date.ToString))
    '        Me.AddMetaTag("dc.date.created", "2003-08-14")
    '        Me.AddMetaTag("dc.date.modified", "2003-08-14")
    '        Me.AddMetaTag("dc.date.valid", "2004-08-14")
    '        Me.AddMetaTag("dc.subject", "air transport, legislation")
    '        Me.AddMetaTag("keywords", "exemptions, authorizations, canadian aviation regulations, cars")
    '        Me.AddMetaTag("dc.description", "This is a search page for exemptions from regulatory requirements.")
    '        Me.AddMetaTag("description", "This is a search page for exemptions from regulatory requirements.")
    '        Me.AddMetaTag("dc.subject.topic", "Air")
    '        Me.AddMetaTag("dc.publisher", "Civil Aviation Web Team (AARC)")
    '    End If


    'End Sub

    Protected Function GetDocPaths() As String
        'Return System.Configuration.ConfigurationManager.AppSettings("doc_file_path_" & Left(_lang, 2))
        Return Server.MapPath(ConfigurationManager.AppSettings("doc_file_path_" & Left(_lang, 2)))
    End Function

    Protected Function GetHtmlUrl() As String
        Return System.Configuration.ConfigurationManager.AppSettings("doc_url_path_" & Left(_lang, 2))
    End Function


    Public Shared Sub OpenConnection(ByRef cn As System.Data.OleDb.OleDbConnection, Optional ByVal lang As String = "")
        Try
            cn = New System.Data.OleDb.OleDbConnection(ConfigurationManager.ConnectionStrings("APP_DATABASE").ToString())
            cn.Open()
        Catch ex As Exception
            ' connection failed, redirect user to error page
            HttpContext.Current.Response.Clear()
            HttpContext.Current.Response.Redirect("err.aspx?lang=" & lang & "&err=" & HttpContext.Current.Server.UrlEncode(ex.Message))
        End Try
    End Sub


    Public Shared Sub CloseConnection(ByRef cn As System.Data.OleDb.OleDbConnection)
        If Not IsNothing(cn) Then
            cn.Close()
            cn.Dispose()
        End If
    End Sub
End Class
