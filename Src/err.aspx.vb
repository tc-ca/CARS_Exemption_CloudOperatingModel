
Partial Class err
  Inherits app_page

  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Me.PageTitle = GetLocalResourceObject("PageTitle").ToString
    Me.AddStyleSheetReferences()
    Me.ltError.Text = Request.QueryString("err")


    End Sub
End Class
