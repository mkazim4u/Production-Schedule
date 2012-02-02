Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions

Partial Class PS_AddRole
    Inherits System.Web.UI.UserControl
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

        End If
    End Sub
    Protected Sub CreatRole()
        Dim DP As DataProvider
        DP = DataProvider.Instance()
        DP.AddRoleGroup(DNN.GetPMB(Me).PortalId, txtRoleName.Text.Trim(), txtRoleDescription.Text.Trim(), 0)
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        CreatRole()
    End Sub

    


End Class


