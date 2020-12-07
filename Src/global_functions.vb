Module global_functions



' Function to convert a null value to empty string
Public Function UnNullStr(ByVal inValue2 As Object) As String
  Dim invalue As String
  If IsDBNull(inValue2) Then
    Return " "
    Exit Function
  End If

  invalue = inValue2.ToString

  If IsNothing(invalue) Then
    UnNullStr = " "
    Exit Function
  End If

  If invalue.Length = 0 Then
    UnNullStr = " "
  Else
    UnNullStr = "" & invalue
  End If
End Function


Public Function FileExists(ByVal path As String) As Boolean

  Dim return_value As Boolean
  return_value = False

  If System.IO.File.Exists(path) = True Then
    return_value = True
  Else

  End If

  Return return_value
End Function


Function CleanString(ByVal string_to_format As String) As String

  If InStr(string_to_format, Chr(34)) > -1 Then
    string_to_format = HttpContext.Current.Server.HtmlEncode(string_to_format)
  End If

  string_to_format = RemoveScriptTags(string_to_format)
  string_to_format = CleanHtmlTags(string_to_format)

  Return string_to_format

End Function

Public Function CleanHtmlTags(ByVal str_ As String) As String

  str_ = str_.Replace("<", "&lt;")
  CleanHtmlTags = str_
End Function


Public Function RemoveScriptTags(ByVal str_ As String) As String
  str_ = str_.Replace("<script>", String.Empty)
  str_ = str_.Replace("</script>", String.Empty)
  RemoveScriptTags = str_
End Function



' function to format a date to yyyy-mm-dd (date_format: (1=2006-01-14  2=January 14, 2006)
Function FormatTCDate(ByVal date_value As String) As String
  If Trim(date_value) = "" Or IsNothing(date_value) = True Then
    Return ""
    Exit Function
  End If

  If IsDate(date_value) = False Then
    Return ""
    Exit Function
  End If


  Dim month_ As String
  Dim day_ As String
  Dim year_ As String

  Dim temp_date As Date
  temp_date = CDate(date_value)

  month_ = temp_date.Month.ToString
  day_ = temp_date.Day.ToString
  year_ = temp_date.Year.ToString

  
  ' put Month & Day in 2 digit format
  If Len(Trim(month_)) = 1 Then
    month_ = "0" & month_
  End If

  If Len(Trim(day_)) = 1 Then
    day_ = "0" & day_
  End If


  Return year_ & "-" & month_ & "-" & day_

End Function



End Module


