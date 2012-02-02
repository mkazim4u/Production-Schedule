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


Partial Class PS_AddGroup
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffGroup As New FFDataLayer.FF_Group


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Call BindGrid()

        End If
    End Sub
    Protected Sub BindGrid()



    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Protected Sub rgGroup_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgGroup.NeedDataSource

        'Dim ffGroup As FFDAL.FFDAL.FF_Group
        Dim query = dbContext.GetAll(Of FFDataLayer.FF_Group)()
        rgGroup.DataSource = query.ToList

    End Sub
    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim txtGroupName As TextBox = e.Item.FindControl("txtGroupName")
            Dim groupName As String = txtGroupName.Text

            ffGroup.GroupName = groupName
            ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffGroup.CreatedOn = DateTime.Now
            dbContext.Add(ffGroup)
            dbContext.SaveChanges()
            'rgGroup.Rebind()


        End If


    End Sub
    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim hidFFUserID As HiddenField = e.Item.FindControl("hidFFUserID")

            Dim nID As Int64 = hidFFUserID.Value


            Dim data As GridEditFormItem = e.Item
            nID = Convert.ToInt64(data.ParentItem("ID").Text)

            Dim txtGroupName As TextBox = e.Item.FindControl("txtGroupName")
            Dim groupName As String = txtGroupName.Text


            ffGroup = dbContext.GetObjectByKey(New ObjectKey(ffGroup.GetType().Name, nID))
            ffGroup.GroupName = groupName
            ffGroup.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffGroup.CreatedOn = DateTime.Now
            dbContext.SaveChanges()


        End If
    End Sub
    Protected Sub rgGroup_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgGroup.ItemCommand

        If e.CommandName = "PerformInsert" Then
            Insert(e)
        Else
            Update(e)
        End If


    End Sub

    Protected Function ToggleButtons() As String

        Dim sText As String
        If rgGroup.MasterTableView.IsItemInserted Then
            sText = "Insert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

    Protected Function ToggleCommand() As String

        Dim sText As String
        If rgGroup.MasterTableView.IsItemInserted Then
            sText = "PerformInsert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

End Class



