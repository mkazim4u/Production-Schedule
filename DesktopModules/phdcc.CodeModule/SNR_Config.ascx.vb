Imports Telerik.Web.UI

Partial Class SNR_Config
    Inherits System.Web.UI.UserControl

    Private dbContext As New SNRDentonDBLayerDataContext

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Call BindGrid()

        End If
    End Sub

    Protected Sub rgConfiguration_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgConfiguration.NeedDataSource

        BindGrid()

    End Sub

    Protected Sub BindGrid()

        Dim IConfig = From Config In dbContext.SNR_Configurations Order By Config.ID Descending
              Select Config

        rgConfiguration.DataSource = IConfig.ToList


    End Sub


    Protected Sub rgConfiguration_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgConfiguration.ItemCommand

        If e.CommandName = RadGrid.PerformInsertCommandName Then
            Insert(e)
        ElseIf e.CommandName = RadGrid.UpdateCommandName Then
            Update(e)
        ElseIf e.CommandName = "Delete" Then
            Delete(e)
        End If

    End Sub

    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If TypeOf e.Item Is GridEditFormInsertItem AndAlso e.Item.IsInEditMode Then

            If e.Item.OwnerTableView.IsItemInserted Then

                Dim txtKey As TextBox = e.Item.FindControl("txtKey")
                Dim txtValue As TextBox = e.Item.FindControl("txtValue")
                Dim chkIsAdmin As CheckBox = e.Item.FindControl("chkIsAdmin")

                Dim con As New SNR_Configuration
                con.ConfigKey = txtKey.Text.Trim
                con.ConfigValue = txtValue.Text.Trim
                con.CreatedOn = DateTime.Now
                con.CreatedBY = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
                con.IsDeleted = False
                con.IsAdmin = IIf(chkIsAdmin.Checked, True, False)

                dbContext.SNR_Configurations.InsertOnSubmit(con)
                dbContext.SubmitChanges()

            End If

        End If

    End Sub

    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If Not (TypeOf e.Item Is GridEditFormInsertItem) Then

            Dim txtKey As TextBox = e.Item.FindControl("txtKey")
            Dim txtValue As TextBox = e.Item.FindControl("txtValue")
            Dim btnUpdate As Button = e.Item.FindControl("btnUpdate")
            Dim chkIsAdmin As CheckBox = e.Item.FindControl("chkIsAdmin")

            Dim nID As Integer = Convert.ToInt64(btnUpdate.CommandArgument)

            Dim con = (From c In dbContext.SNR_Configurations
                          Where c.ID = nID
                         Select c).SingleOrDefault


            con.ConfigKey = txtKey.Text.Trim
            con.ConfigValue = txtValue.Text.Trim
            con.IsDeleted = False
            con.CreatedOn = DateTime.Now
            con.IsAdmin = IIf(chkIsAdmin.Checked, True, False)
            con.CreatedBY = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID

            dbContext.SubmitChanges()

        End If

    End Sub
    Protected Sub rgConfiguration_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgConfiguration.ItemDataBound

        If (TypeOf e.Item Is GridDataItem) Or (e.Item.ItemType = GridItemType.AlternatingItem) Then

            Dim imgEdit As System.Web.UI.WebControls.Image = e.Item.FindControl("imgEdit")
            Dim imgDelete As System.Web.UI.WebControls.Image = e.Item.FindControl("imgDelete")

            Dim hidCreatedBy As HiddenField = e.Item.FindControl("hidCreatedBy")
            Dim nUserID As Integer = hidCreatedBy.Value

            Dim item As GridDataItem = e.Item

            Dim ui As UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserID)
            item("CreatedBy").Text = ui.Username

            imgEdit.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/Images/Edit.gif"
            imgDelete.ImageUrl = "~/Portals/" + DNN.GetPMB(Me).PortalId.ToString + "/Images/Delete.png"

        ElseIf TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode Then

            If TypeOf e.Item Is GridEditableItem And TypeOf e.Item Is GridEditFormInsertItem Then

                Dim btnInsert As Button = e.Item.FindControl("btnInsert")
                btnInsert.Visible = True

            Else

                Dim btnUpdate As Button = e.Item.FindControl("btnUpdate")
                btnUpdate.Visible = True

            End If

        End If


    End Sub
    Protected Sub Delete(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        Dim lnkDelete As LinkButton = e.Item.FindControl("lnkDelete")
        Dim nID As Integer = Convert.ToInt64(lnkDelete.CommandArgument)

        Dim con = (From c In dbContext.SNR_Configurations
                         Where c.ID = nID
                        Select c).SingleOrDefault

        con.IsDeleted = True

        dbContext.SubmitChanges()

    End Sub

    'Protected Sub cusKey_ServerValidate(ByVal sender As Object, ByVal e As ServerValidateEventArgs)

    '    Dim config = From con In dbContext.SNR_Configurations
    '                 Where con.ConfigKey = e.Value

    '    If config IsNot Nothing Then

    '        e.IsValid = False

    '    End If

    'End Sub



End Class
