Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class BaseAdmin
        Inherits BaseModule

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            IncludeScripts(PortalId, StoreInstallPath, Page, "adminjs.includes", "adminstartup.includes", "admincss.includes")
        End Sub

        Public Overloads Function EditUrl() As String
            Return Me.EditUrl("", "", "Edit")
        End Function

        Public Overloads Function EditUrl(ByVal ControlKey As String) As String
            Return Me.EditUrl("", "", ControlKey)
        End Function

        Public Overloads Function EditUrl(ByVal KeyName As String, ByVal KeyValue As String) As String
            Return Me.EditUrl(KeyName, KeyValue, "Edit")
        End Function

        Public Overloads Function EditUrl(ByVal KeyName As String, ByVal KeyValue As String, ByVal ControlKey As String) As String

            Dim key As String = ControlKey
            Dim SkinSrc As String = ""

            If Not (Request.QueryString("SkinSrc") Is Nothing) Then
                SkinSrc = QueryStringEncode(Request.QueryString("SkinSrc"))
            End If

            If (key = "") Then
                key = "Edit"
            End If

            If KeyName <> "" And KeyValue <> "" Then
                If SkinSrc = "" Then
                    Return NavigateURL(PortalSettings.ActiveTab.TabID, key, "mid=" & ModuleId.ToString, KeyName & "=" & KeyValue)
                Else
                    Return NavigateURL(PortalSettings.ActiveTab.TabID, key, "mid=" & ModuleId.ToString, KeyName & "=" & KeyValue, "SkinSrc=" & SkinSrc)
                End If
            Else
                If SkinSrc = "" Then
                    Return NavigateURL(PortalSettings.ActiveTab.TabID, key, "mid=" & ModuleId.ToString)
                Else
                    Return NavigateURL(PortalSettings.ActiveTab.TabID, key, "mid=" & ModuleId.ToString, "SkinSrc=" & SkinSrc)
                End If
            End If
        End Function

        Public Overloads Function EditUrl(ByVal KeyName As String, ByVal KeyValue As String, ByVal ControlKey As String, ByVal ParamArray AdditionalParameters As String()) As String
            Dim objBase As New Entities.Modules.PortalModuleBase
            Dim SkinSrc As String = ""
            If Not (Request.QueryString("SkinSrc") Is Nothing) Then
                SkinSrc = QueryStringEncode(Request.QueryString("SkinSrc"))
            End If

            Dim key As String = ControlKey

            If (key = "") Then
                key = "Edit"
            End If

            If KeyName <> "" And KeyValue <> "" Then
                Dim params(AdditionalParameters.Length + 2) As String

                params(0) = "mid=" & ModuleId.ToString
                params(1) = KeyName & "=" & KeyValue
                If SkinSrc <> "" Then
                    params(2) = "SkinSrc=" & SkinSrc
                End If

                For i As Integer = 0 To AdditionalParameters.Length - 1
                    params(i + 3) = AdditionalParameters(i)
                Next

                Return NavigateURL(PortalSettings.ActiveTab.TabID, key, params)
            Else
                Dim params(AdditionalParameters.Length + 1) As String

                params(0) = "mid=" & ModuleId.ToString
                If SkinSrc <> "" Then
                    params(1) = "SkinSrc=" & SkinSrc
                End If

                For i As Integer = 0 To AdditionalParameters.Length - 1
                    params(i + 2) = AdditionalParameters(i)
                Next

                Return NavigateURL(PortalSettings.ActiveTab.TabID, key, params)
            End If
        End Function

    End Class

End Namespace
