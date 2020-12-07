Imports System.Globalization
Partial Public Class search
    Inherits app_page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Me.IsPostBack = True Then

            If lang = "fra" Then
                Response.Redirect("e_s_r.aspx?lang=fra&ddlKind=" & Server.UrlEncode(Me.ddlKind.SelectedValue) & "&ddlType=" & Server.UrlEncode(Me.ddlType.SelectedValue) & "&txtSearchTerm=" & Server.UrlEncode(Me.txtSearchTerm.Text.Trim))
            Else
                Response.Redirect("e_s_r.aspx?lang=eng&ddlKind=" & Server.UrlEncode(Me.ddlKind.SelectedValue) & "&ddlType=" & Server.UrlEncode(Me.ddlType.SelectedValue) & "&txtSearchTerm=" & Server.UrlEncode(Me.txtSearchTerm.Text.Trim))
            End If

        Else
            'Me.Title = GetLocalResourceObject("PageTitle").ToString
            CLFPage.TagHelper.AddConfigsTag(Nothing, False, False, Me.Title)
        End If

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click





    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' Why is this code necessary?
        '
        ' SPECIAL NOTE: This code is not necessary if we put accents in the 
        ' resource files. i.e. not htmlencoded. Need to decide which way to go.
        '
        ' The asp:dropdownlist control htmlencodes all listitems by default.
        ' As a result any characters represented by an entity number will
        ' not display correctly because the leading & will be encoded to &amp;
        ' which in turn nullifies the entity number.
        '
        ' e.g. Type d'a&#233;ronef is rendered on the page as Type d'a&amp;#233;ronef
        '
        ' Therefore, the text value of each listitem must be htmldecoded before
        ' it gets htmlencoded by the internal mechanism of the control.
        For i As Integer = 0 To Me.ddlType.Items.Count - 1
            Me.ddlType.Items(i).Text = Server.HtmlDecode(Me.ddlType.Items(i).Text)
            Me.ddlType.Items(i).Attributes("title") = Server.HtmlDecode(Me.ddlType.Items(i).Attributes("title"))
        Next
    End Sub
End Class