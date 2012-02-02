Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports FFDataLayer
Imports Telerik.Web.UI
Imports Telerik.OpenAccess

Partial Class PS_Item
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffItem As New FFDataLayer.FF_Item
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

    Protected Sub rgItem_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgItem.NeedDataSource

        'Dim dbContext As New DBContext
        'Dim ffItem As FFDAL.FFDAL.FF_Item
        Dim query = dbContext.GetAll(Of FFDataLayer.FF_Item)()
        rgItem.DataSource = query.ToList

    End Sub

    Protected Sub rgItem_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgItem.ItemDataBound

        If e.Item.IsInEditMode Then

            BindGroup(e)

        End If

    End Sub


    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim txtItemName As TextBox = e.Item.FindControl("txtItemName")
            Dim txtRate As TextBox = e.Item.FindControl("txtRate")
            Dim txtPosition As TextBox = e.Item.FindControl("txtPosition")
            Dim rcbGroup As RadComboBox = e.Item.FindControl("rcbGroup")
            Dim rntbPrice As RadNumericTextBox = e.Item.FindControl("rntbPrice")

            ffItem.Units = Convert.ToInt64(txtRate.Text.Trim())
            ffItem.CostPrice = rntbPrice.Text.Trim()
            ffItem.ItemName = txtItemName.Text.Trim()
            ffItem.ItemOrder = txtPosition.Text.Trim()
            ffItem.GroupID = Convert.ToInt64(rcbGroup.SelectedValue)
            ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffItem.CreatedOn = DateTime.Now
            dbContext.Add(ffItem)
            dbContext.SaveChanges()
            rgItem.Rebind()

        End If

    End Sub
    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim nID As Int64
            Dim data As GridEditFormItem = e.Item
            Dim txtItemName As TextBox = e.Item.FindControl("txtItemName")
            Dim txtRate As TextBox = e.Item.FindControl("txtRate")
            Dim txtPosition As TextBox = e.Item.FindControl("txtPosition")
            Dim rcbGroup As RadComboBox = e.Item.FindControl("rcbGroup")
            Dim rntbPrice As RadNumericTextBox = e.Item.FindControl("rntbPrice")

            nID = Convert.ToInt64(data.ParentItem("ID").Text)
            ffItem = dbContext.GetObjectByKey(New ObjectKey(ffItem.GetType().Name, nID))

            ffItem.ItemName = txtItemName.Text.Trim()
            ffItem.CostPrice = rntbPrice.Text.Trim()
            ffItem.GroupID = Convert.ToInt64(rcbGroup.SelectedValue)
            ffItem.Units = Convert.ToInt64(txtRate.Text.Trim())
            ffItem.ItemOrder = txtPosition.Text.Trim()
            ffItem.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffItem.CreatedOn = DateTime.Now
            dbContext.SaveChanges()

        End If

    End Sub
    Protected Sub rgItem_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgItem.ItemCommand

        If e.CommandName = "PerformInsert" Then
            Insert(e)
        Else
            Update(e)
        End If


    End Sub

    Protected Function ToggleButtons() As String

        Dim sText As String
        If rgItem.MasterTableView.IsItemInserted Then
            sText = "Insert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

    Protected Function ToggleCommand() As String

        Dim sText As String
        If rgItem.MasterTableView.IsItemInserted Then
            sText = "PerformInsert"
        Else
            sText = "Update"
        End If

        Return sText

    End Function

    Protected Sub BindGroup(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim rcbGroup As RadComboBox = e.Item.FindControl("rcbGroup")


        Dim IGroups As IList = dbContext.FF_Groups.ToList


        For Each item As FFDataLayer.FF_Group In IGroups

            Dim rcbItem As New RadComboBoxItem(item.GroupName, item.ID)
            rcbGroup.Items.Add(rcbItem)

        Next

        rcbGroup.Items.Insert(0, New RadComboBoxItem("- Select -", -1))

    End Sub

End Class
