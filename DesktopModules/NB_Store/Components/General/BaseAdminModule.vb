Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class BaseAdminModule
        Inherits BaseAdmin

        Public BackOfficeMenu As AdminMenu

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            BackOfficeMenu = New AdminMenu
            Controls.AddAt(0, BackOfficeMenu)

        End Sub

    End Class

End Namespace
