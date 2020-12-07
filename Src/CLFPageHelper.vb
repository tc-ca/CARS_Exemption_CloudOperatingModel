Option Strict Off
Imports System.Collections.Generic
Imports System.Web
Imports System.Text
Imports System.Web.UI.WebControls
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Text.RegularExpressions

Namespace CLFPage
    ' Menus are generated using a fluent interface coding technique
    Public Class MenuHelper
        ' Not using properties in order to fully support .NET 2.0
        Private _builder As StringBuilder
        Private _created As Boolean
        Private _conditions As Stack(Of BoolFunc)

        Private Sub New(title As String)
            ' Validate
            If String.IsNullOrEmpty(title) Then
                Throw New ArgumentNullException("title")
            End If

            _created = False
            _conditions = New Stack(Of BoolFunc)()
            _builder = New StringBuilder()
        End Sub
        Private Sub New(title As String, right As Boolean, condition As BoolFunc)
            Me.New(title)
            Me.OpenMenu(title, right, condition)
        End Sub

        Private Function OpenMenu(title As String, condition As BoolFunc) As MenuHelper
            OpenMenu(title, False, condition)
            Return Me
        End Function
        Private Function OpenMenu(title As String, right As Boolean, condition As BoolFunc) As MenuHelper
            _conditions.Push(condition)
            If CheckCondition(Nothing) Then
                OpenMenu(_builder, title, right)
            End If

            Return Me
        End Function
        Public Function AddMenuItem(title As String, link As String) As MenuHelper
            Return AddMenuItem(title, link, Nothing)
        End Function
        Public Function AddMenuItem(title As String, link As String, condition As BoolFunc) As MenuHelper
            Return AddMenuItem(title, link, Nothing, Nothing, condition)
        End Function
        Public Function AddMenuItem(title As String, link As String, tooltip As String, id As String) As MenuHelper
            Return AddMenuItem(title, link, tooltip, id, Nothing)
        End Function
        Public Function AddMenuItem(title As String, link As String, tooltip As String, id As String, condition As BoolFunc) As MenuHelper
            If CheckCondition(condition) Then
                AddMenuItem(_builder, title, tooltip, id, link)
            End If

            Return Me
        End Function
        Public Function Initialize(title As String, link As String) As MenuHelper
            Return Initialize(title, link, Nothing)
        End Function
        Public Function Initialize(title As String, link As String, condition As BoolFunc) As MenuHelper
            Return Initialize(title, link, Nothing, Nothing, condition)
        End Function
        Public Function Initialize(title As String, link As String, tooltip As String, id As String) As MenuHelper
            Return Initialize(title, link, tooltip, id, Nothing)
        End Function
        Public Function Initialize(title As String, link As String, tooltip As String, id As String, condition As BoolFunc) As MenuHelper
            _conditions.Push(condition)
            If CheckCondition(Nothing) Then
                OpenMenuItem(_builder, title, tooltip, id, link)
            End If

            Return Me
        End Function
        Public Function Create() As MenuHelper
            If _conditions.Count = 0 Then
                Throw New InvalidOperationException()
            End If

            Dim condition = _conditions.Pop()
            If _conditions.Count > 0 Then
                ' Sub menu
                If condition Is Nothing OrElse CheckCondition(condition) Then
                    CloseMenuItem(_builder)
                End If

                Return Me
            Else
                ' Root menu
                If condition Is Nothing OrElse CheckCondition(condition) Then
                    CloseMenu(_builder)
                End If

                ' Write to the head
                If _created Then
                    Throw New NotSupportedException("You can only call .Create() once.")
                End If
                TagHelper.WriteTagToResponse(_builder.ToString())

                _created = True
                Return Me
            End If

        End Function

        Private Function CheckCondition(condition As BoolFunc) As Boolean
            ' Check against all parent conditions
            Dim c As BoolFunc
            For Each c In _conditions
                ' One of the conditions in the hierarchy wasn't meet
                If c IsNot Nothing AndAlso Not c() Then
                    Return False
                End If
            Next

            ' None were invalid, so check the current condition
            Return condition Is Nothing OrElse condition()
        End Function

        Public Shared Function Initialize(title As String) As MenuHelper
            Return Initialize(title, False, DirectCast(Nothing, BoolFunc))
        End Function
        Public Shared Function Initialize(title As String, right As Boolean) As MenuHelper
            Return Initialize(title, right, DirectCast(Nothing, BoolFunc))
        End Function
        Public Shared Function Initialize(title As String, right As Boolean, condition As BoolFunc) As MenuHelper
            Return New MenuHelper(title, right, condition)
        End Function

        Private Shared Sub OpenMenu(builder As StringBuilder, title As String, right As Boolean)
            builder.Append(TagHelper.CreateTag(TagType.Menu, TagMode.Open, New TagAttribute("title", title), New TagAttribute("right", right)))
        End Sub
        Private Shared Sub CloseMenu(builder As StringBuilder)
            builder.Append(TagHelper.CreateTag(TagType.Menu, TagMode.Close))
        End Sub
        Private Shared Sub AddMenuItem(builder As StringBuilder, title As String, tooltip As String, id As String, link As String)
            builder.Append(TagHelper.CreateTag(TagType.Item, TagMode.SelfClose, New TagAttribute("title", title), New TagAttribute("link", link), New TagAttribute("tooltip", tooltip), New TagAttribute("id", id)))
        End Sub
        Private Shared Sub OpenMenuItem(builder As StringBuilder, title As String, tooltip As String, id As String, link As String)
            builder.Append(TagHelper.CreateTag(TagType.Item, TagMode.Open, New TagAttribute("title", title), New TagAttribute("link", link), New TagAttribute("tooltip", tooltip), New TagAttribute("id", id)))
        End Sub
        Private Shared Sub CloseMenuItem(builder As StringBuilder)
            builder.Append(TagHelper.CreateTag(TagType.Item, TagMode.Close))
        End Sub

    End Class

    Public NotInheritable Class TagHelper
        Public Shared Function GetVersionTag(version As String) As String
            Return CreateTag(TagType.Version, TagMode.SelfClose, New TagAttribute("value", version))
        End Function
        Public Shared Function GetLanguageTag(language As Languages) As String
            Return CreateTag(TagType.Language, TagMode.SelfClose, New TagAttribute("value", language))
        End Function
        Public Shared Function GetActionsTag(Optional languageAction As String = Nothing, Optional searchAction As String = Nothing, Optional homeAction As String = Nothing) As String
            Return CreateTag(TagType.Actions, TagMode.SelfClose, New TagAttribute("language", languageAction), New TagAttribute("search", searchAction), New TagAttribute("home", homeAction))
        End Function
        Public Shared Function GetConfigsTag(Optional lastUpdatedDate As String = Nothing, Optional relativePathing As Boolean = False, Optional secureHttp As Boolean = False, Optional bodyTitle As String = Nothing) As String
            Return CreateTag(TagType.Configs, TagMode.SelfClose, New TagAttribute("lastUpdated", lastUpdatedDate), New TagAttribute("relativePathing", relativePathing), New TagAttribute("secureHttp", secureHttp), New TagAttribute("bodyTitle", bodyTitle))
        End Function
        Public Shared Function GetMetaTag(name As String, content As String) As String
            Return CreateTag(TagType.Meta, TagMode.SelfClose, New TagAttribute("name", name), New TagAttribute("content", content))
        End Function
        Public Shared Function GetBreadCrumbTag(title As String, link As String) As String
            Return CreateTag(TagType.Breadcrumb, TagMode.SelfClose, New TagAttribute("title", title), New TagAttribute("link", link))
        End Function
        Public Shared Function GetTransformTag(Optional headerTransform As TransformFunc = Nothing, Optional topTransform As TransformFunc = Nothing, Optional bottomTransform As TransformFunc = Nothing) As String
            Dim headerClass As String = IIf(headerTransform Is Nothing, Nothing, headerTransform.Method.DeclaringType.AssemblyQualifiedName)
            Dim headerFunc As String = IIf(headerTransform Is Nothing, Nothing, headerTransform.Method.Name)
            Dim topClass As String = IIf(topTransform Is Nothing, Nothing, topTransform.Method.DeclaringType.AssemblyQualifiedName)
            Dim topFunc As String = IIf(topTransform Is Nothing, Nothing, topTransform.Method.Name)
            Dim bottomClass As String = IIf(bottomTransform Is Nothing, Nothing, bottomTransform.Method.DeclaringType.AssemblyQualifiedName)
            Dim bottomFunc As String = IIf(bottomTransform Is Nothing, Nothing, bottomTransform.Method.Name)

            Return CreateTag(TagType.Transform, TagMode.SelfClose,
                New TagAttribute("headerClass", headerClass), New TagAttribute("header", headerFunc),
                New TagAttribute("topClass", topClass), New TagAttribute("top", topFunc),
                New TagAttribute("bottomClass", bottomClass), New TagAttribute("bottom", bottomFunc))
        End Function

        Public Shared Sub AddVersionTag(version As String)
            WriteTagToResponse(GetVersionTag(version))
        End Sub
        Public Shared Sub AddLanguageTag(language As Languages)
            WriteTagToResponse(GetLanguageTag(language))
        End Sub
        Public Shared Sub AddActionsTag(Optional languageAction As String = Nothing, Optional searchAction As String = Nothing, Optional homeAction As String = Nothing)
            WriteTagToResponse(GetActionsTag(languageAction, searchAction, homeAction))
        End Sub
        Public Shared Sub AddConfigsTag(Optional lastUpdatedDate As String = Nothing, Optional relativePathing As Boolean = False, Optional secureHttp As Boolean = False, Optional bodyTitle As String = Nothing)
            WriteTagToResponse(GetConfigsTag(lastUpdatedDate, relativePathing, secureHttp, bodyTitle))
        End Sub
        Public Shared Sub AddMetaTag(name As String, content As String)
            WriteTagToResponse(GetMetaTag(name, content))
        End Sub
        Public Shared Sub AddBreadCrumbTag(title As String, link As String)
            WriteTagToResponse(GetBreadCrumbTag(title, link))
        End Sub
        Public Shared Sub AddTransformTag(Optional headerTransform As TransformFunc = Nothing, Optional topTransform As TransformFunc = Nothing, Optional bottomTransform As TransformFunc = Nothing)
            WriteTagToResponse(GetTransformTag(headerTransform, topTransform, bottomTransform))
        End Sub

        Public Shared Sub WriteTagToResponse(tag As String)
            ' Validate
            Dim context As HttpContext = HttpContext.Current
            If context Is Nothing Then
                Throw New NotSupportedException("Unable to retreive the current HttpContext.")
            End If
            Dim response As HttpResponse = context.Response
            If response Is Nothing Then
                Throw New NotSupportedException("Unable to retreive the current HttpResponse.")
            End If

            ' Write the raw tag to the HttpResponse
            response.Write(tag)
        End Sub

        Public Shared Function CreateTag(key As TagType, type As TagMode, ParamArray attribute As TagAttribute()) As String
            Dim tagFormat As String = "<{3}clf:{0}{1}{2}>"
            Dim attFormat As String = " {0}=""{1}"""
            Dim attributes As New StringBuilder()
            For Each att As TagAttribute In attribute
                If att.Value IsNot Nothing Then
                    attributes.AppendFormat(attFormat, att.Key, att.Value)
                End If
            Next

            Return String.Format(tagFormat, key.ToString().ToLower(), attributes.ToString(), If(type = TagMode.SelfClose, "/", String.Empty), If(type = TagMode.Close, "/", String.Empty))
        End Function
    End Class

    Public Class TagAttribute
        Private _key As String
        Private _value As String

        Public Property Key As String
            Get
                Return _key
            End Get
            Set(value As String)
                _key = value
            End Set
        End Property
        Public Property Value As String
            Get
                Return _value
            End Get
            Set(value As String)
                _value = value
            End Set
        End Property

        Public Sub New(key As String, value As String)
            _key = key
            _value = value
        End Sub
        Public Sub New(key As String, value As Object)
            Me.New(key, value.ToString())
        End Sub
    End Class

    Public Delegate Function BoolFunc() As Boolean
    Public Delegate Function TransformFunc(html As String) As String

    Public Enum TagType
        Breadcrumb
        Language
        Actions
        Configs
        Version
        Meta
        Menu
        Item
        Transform
    End Enum
    Public Enum TagMode
        Open
        Close
        SelfClose
    End Enum
    Public Enum Languages
        English
        French
    End Enum
End Namespace
