Imports DotNetNuke.Authentication
Imports System.DirectoryServices
Partial Class PS_AssignRoleToUser
    Inherits System.Web.UI.UserControl

    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            AssignRoleToUser()

            'Dim adUserInfo As New DotNetNuke.Authentication.ActiveDirectory.Configuration()

            'Dim defaultDomain As String = adUserInfo.DefaultDomain



        End If

    End Sub

    Protected Sub AssignRoleToUser()

        Dim sUserName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
        Dim sRoleName As String

        Dim sql As String = "select username, rolename from ff_users"

        Dim dt As DataTable = DNNDB.Query(sql)

        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            For Each dr As DataRow In dt.Rows

                sRoleName = dr("rolename")

                If dr("username") = sUserName Then

                    Dim ri As DotNetNuke.Security.Roles.RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, sRoleName)
                    rc.AddUserRole(DNN.GetPMB(Me).PortalId, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID, ri.RoleID, DotNetNuke.Common.Utilities.Null.NullDate)

                End If

            Next

        End If

    End Sub

    Protected Sub chkIsFirstTime_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)



    End Sub

End Class
