Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class ShowSelectLang
        Inherits System.Web.UI.UserControl

#Region "Methods"


        Public Sub Refresh()
            buildMenu()
        End Sub

        Private Sub buildMenu()
            If Localization.GetEnabledLocales.Count > 1 Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                Dim AdminLang = getStoreCookieValue(PS.PortalId, "AdminLang", "AdminSelectedCulture")
                If AdminLang = "" Then AdminLang = GetCurrentCulture()
                If AdminLang <> "None" Then
                    phFlag.Controls.Add(New LiteralControl("<img src=""" & DotNetNuke.Common.ResolveUrl("~/images/flags/" & AdminLang & ".gif") & """ height=""12"" width=""18"" border=""0"" />"))
                End If
            End If
        End Sub

#End Region

    End Class

End Namespace
