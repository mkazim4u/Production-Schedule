Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports FFDataLayer
Imports Telerik.Web.UI
Imports Telerik.OpenAccess
Imports System.Web.Script.Services

Partial Class PS_Timer
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffGroup As New FFDataLayer.FF_Group

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

        End If

    End Sub

    'Protected Sub TimerAutoSave_Tick(sender As Object, e As System.EventArgs) Handles TimerAutoSave.Tick

    '    Dim sGroupName As String = txtGroupName.Text.Trim()


    '    If sGroupName <> String.Empty Then

    '        ffGroup.GroupName = sGroupName
    '        ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
    '        ffGroup.CreatedOn = DateTime.Now
    '        dbContext.Add(ffGroup)
    '        dbContext.SaveChanges()
    '        btnSave.Text = "Record Saved"

    '    End If


    'End Sub

    Protected Sub Save()

        Dim sGroupName As String = txtGroupName.Text.Trim()

        If sGroupName <> String.Empty Then

            ffGroup.GroupName = sGroupName
            ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffGroup.CreatedOn = DateTime.Now
            dbContext.Add(ffGroup)
            dbContext.SaveChanges()
            btnSave.Text = "Record Saved"

        End If


    End Sub
    <System.Web.Services.WebMethod()>
    <ScriptMethod(ResponseFormat:=ResponseFormat.Json)>
    Public Shared Function IsExist() As String


        'Dim sGroupName As String = txtGroupName.Text.Trim()

        Return Date.Now.ToString()

        'Dim sql As String = "select * from ff_group where groupname = 'Group 2'"
        'Dim dt As DataTable = DNNDB.Query(sql)
        'If dt IsNot Nothing And dt.Rows.Count <> 0 Then

        'Else
        '    Save()
        'End If

    End Function

End Class
