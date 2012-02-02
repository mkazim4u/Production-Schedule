Imports Telerik.OpenAccess
Imports FFDataLayer
Imports System.DirectoryServices


Partial Class PS_DynamicRoleAssignment
    Inherits System.Web.UI.UserControl

    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo
    'Dim config As New DotNetNuke.Authentication.ActiveDirectory.Configuration
    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffUser As New FFDataLayer.FF_User

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            AssignRoleToUser()

        End If

    End Sub

    Protected Sub AssignRoleToUser()

        Try

            Dim sUserName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
            Dim nID As Int64
            Dim sFirstTimeNavigationUrl As String = String.Empty
            Dim sNavigationUrl As String = String.Empty


            Dim IUsers = From Users In dbContext.FF_Users
                         Select Users

            For Each user As FFDataLayer.FF_User In IUsers

                'WebMsgBox.Show(user.ToString() & sUserName)

                If String.Compare(user.UserName, sUserName, True) = 0 Then

                    Dim role As DotNetNuke.Security.Roles.RoleInfo = rc.GetRole(user.RoleId, DNN.GetPMB(Me).PortalId)

                    If role IsNot Nothing Then

                        rc.AddUserRole(DNN.GetPMB(Me).PortalId, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID, role.RoleID, DotNetNuke.Common.Utilities.Null.NullDate)
                        'Dim ri As DotNetNuke.Security.Roles.RoleInfo = rc.GetRoleByName(DNN.GetPMB(Me).PortalId, role.RoleName)

                    End If

                    'pbIsFirstTime = user.IsFirstTime

                    Dim tc As New DotNetNuke.Entities.Tabs.TabController

                    Dim firstTimetabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTab(user.RedirectToFirstTime, DNN.GetPMB(Me).PortalId, True)

                    Dim redirectTotabInfo As DotNetNuke.Entities.Tabs.TabInfo = tc.GetTab(user.RedirectTo, DNN.GetPMB(Me).PortalId, True)

                    If firstTimetabInfo IsNot Nothing Then

                        sFirstTimeNavigationUrl = firstTimetabInfo.TabName

                    End If

                    If redirectTotabInfo IsNot Nothing Then

                        sNavigationUrl = redirectTotabInfo.TabName

                    End If


                    If sFirstTimeNavigationUrl <> String.Empty Then

                        nID = user.ID
                        ffUser = dbContext.GetObjectByKey(New ObjectKey(ffUser.GetType().Name, nID))
                        ffUser.RedirectToFirstTime = -1
                        dbContext.SaveChanges()

                    End If


                End If

            Next


            If sFirstTimeNavigationUrl <> String.Empty Then

                NavigateTo(sFirstTimeNavigationUrl, Nothing)


            ElseIf sNavigationUrl <> String.Empty Then

                NavigateTo(sNavigationUrl, Nothing)

            End If

        Catch ex As Exception

            WebMsgBox.Show(ex.Message.ToString())

        End Try




    End Sub

   

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub



End Class
