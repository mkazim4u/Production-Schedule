Imports Telerik.OpenAccess
Imports FFDataLayer
Imports Telerik.Web.UI

Partial Class PS_AddRecipients
    Inherits System.Web.UI.UserControl

    Private dbContext As New FFDataLayer.EntitiesModel
    Private ffRecipients As New FF_PickRecipient


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            Call BindGrid()

        End If
    End Sub
    Protected Sub BindGrid()



    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)

    End Sub
    Protected Sub rgRecipients_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgRecipients.NeedDataSource

        'Dim ffGroup As FFDAL.FFDAL.FF_Group
        Dim IRecipients = (From Recipients In dbContext.FF_PickRecipients
                          Select Recipients).ToList
        rgRecipients.DataSource = IRecipients

    End Sub
    Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If TypeOf e.Item Is GridEditFormInsertItem And e.Item.IsInEditMode Then

            Dim rtbShortcut As RadTextBox = e.Item.FindControl("rtbShortcut")
            Dim rtbCtcName As RadTextBox = e.Item.FindControl("rtbCtcName")
            Dim rtbCneeName As RadTextBox = e.Item.FindControl("rtbCneeName")
            Dim rtbCneeAddr1 As RadTextBox = e.Item.FindControl("rtbCneeAddr1")
            Dim rtbCneeAddr2 As RadTextBox = e.Item.FindControl("rtbCneeAddr2")
            Dim rtbState As RadTextBox = e.Item.FindControl("rtbState")
            Dim rtbTown As RadTextBox = e.Item.FindControl("rtbTown")
            Dim rtbPostCode As RadTextBox = e.Item.FindControl("rtbPostCode")

            Dim rcbCountry As RadComboBox = e.Item.FindControl("rcbCountry")

            ffRecipients.Shortcut = rtbShortcut.Text.Trim()
            ffRecipients.CneeCtcName = rtbCtcName.Text.Trim()
            ffRecipients.CneeName = rtbCneeName.Text.Trim()
            ffRecipients.CneeAddr1 = rtbCneeAddr1.Text.Trim()
            ffRecipients.CneeAddr2 = rtbCneeAddr2.Text.Trim()
            ffRecipients.CneeState = rtbState.Text.Trim()
            ffRecipients.CneeTown = rtbTown.Text.Trim()
            ffRecipients.CneePostCode = rtbPostCode.Text.Trim()
            ffRecipients.CneeCountryKey = Convert.ToInt32(rcbCountry.SelectedValue)
            ffRecipients.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffRecipients.CreatedOn = DateTime.Now

            dbContext.Add(ffRecipients)
            dbContext.SaveChanges()

        End If


    End Sub
    Protected Sub Update(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

        If e.Item.IsInEditMode Then

            Dim rtbShortcut As RadTextBox = e.Item.FindControl("rtbShortcut")
            Dim rtbCtcName As RadTextBox = e.Item.FindControl("rtbCtcName")
            Dim rtbCneeName As RadTextBox = e.Item.FindControl("rtbCneeName")
            Dim rtbCneeAddr1 As RadTextBox = e.Item.FindControl("rtbCneeAddr1")
            Dim rtbCneeAddr2 As RadTextBox = e.Item.FindControl("rtbCneeAddr2")
            Dim rtbState As RadTextBox = e.Item.FindControl("rtbState")
            Dim rtbTown As RadTextBox = e.Item.FindControl("rtbTown")
            Dim rtbPostCode As RadTextBox = e.Item.FindControl("rtbPostCode")

            Dim rcbCountry As RadComboBox = e.Item.FindControl("rcbCountry")
            Dim hidFFUserID As HiddenField = e.Item.FindControl("hidFFUserID")

            ffRecipients = dbContext.GetObjectByKey(New ObjectKey(ffRecipients.GetType().Name, hidFFUserID.Value))

            ffRecipients.Shortcut = rtbShortcut.Text.Trim()
            ffRecipients.CneeCtcName = rtbCtcName.Text.Trim()
            ffRecipients.CneeName = rtbCneeName.Text.Trim()
            ffRecipients.CneeAddr1 = rtbCneeAddr1.Text.Trim()
            ffRecipients.CneeAddr2 = rtbCneeAddr2.Text.Trim()
            ffRecipients.CneeState = rtbState.Text.Trim()
            ffRecipients.CneeTown = rtbTown.Text.Trim()
            ffRecipients.CneePostCode = rtbPostCode.Text.Trim()
            ffRecipients.CneeCountryKey = Convert.ToInt32(rcbCountry.SelectedValue)
            ffRecipients.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            ffRecipients.CreatedOn = DateTime.Now

            dbContext.SaveChanges()


        End If
    End Sub
    Protected Sub BindCountry(ByVal e As Telerik.Web.UI.GridItemEventArgs)

        Dim ddlCountry As DropDownList = e.Item.FindControl("ddlCountry")

        Call FF_GLOBALS.PopulateComboboxCountry(ddlCountry)

    End Sub
    Protected Sub rgRecipients_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgRecipients.ItemDataBound

        If TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode Then

            If e.Item.OwnerTableView.IsItemInserted Then

                BindCountry(e)

            Else

                BindCountry(e)

                Dim rtbShortcut As RadTextBox = e.Item.FindControl("rtbShortcut")
                Dim rtbCtcName As RadTextBox = e.Item.FindControl("rtbCtcName")
                Dim rtbCneeName As RadTextBox = e.Item.FindControl("rtbCneeName")
                Dim rtbCneeAddr1 As RadTextBox = e.Item.FindControl("rtbCneeAddr1")
                Dim rtbCneeAddr2 As RadTextBox = e.Item.FindControl("rtbCneeAddr2")
                Dim rtbState As RadTextBox = e.Item.FindControl("rtbState")
                Dim rtbTown As RadTextBox = e.Item.FindControl("rtbTown")
                Dim rtbPostCode As RadTextBox = e.Item.FindControl("rtbPostCode")

                Dim rcbCountry As RadComboBox = e.Item.FindControl("rcbCountry")

                rtbShortcut.Text = ffRecipients.Shortcut
                rtbCtcName.Text = ffRecipients.CneeCtcName
                rtbCneeName.Text = ffRecipients.CneeName
                rtbCneeAddr1.Text = ffRecipients.CneeAddr1
                rtbCneeAddr2.Text = ffRecipients.CneeAddr2
                rtbState.Text = ffRecipients.CneeState
                rtbTown.Text = ffRecipients.CneeTown
                rtbPostCode.Text = ffRecipients.CneePostCode
                rcbCountry.SelectedValue = ffRecipients.CneeCountryKey

            End If

        ElseIf (TypeOf e.Item Is GridDataItem) Or (e.Item.ItemType = GridItemType.AlternatingItem) Then

            Dim lblCneeCountryKey As Label = e.Item.FindControl("lblCneeCountryKey")
            Dim nCountryKey As Integer = Convert.ToInt64(lblCneeCountryKey.Text)
            Dim sSQL As String = "SELECT SUBSTRING(CountryName, 1, 25), CountryKey FROM Country WHERE DeletedFlag = 0 and CountryKey = " + nCountryKey
            Dim oDT As DataTable = SprintDB.Query(sSQL)

            If oDT IsNot Nothing And oDT.Rows.Count <> 0 Then

                lblCneeCountryKey.Text = oDT.Rows(0)("CountryName")

            End If

        End If

    End Sub
    Protected Sub rgRecipients_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgRecipients.ItemCommand

        If e.CommandName = "PerformInsert" Then
            Insert(e)
        Else
            Update(e)
        End If


    End Sub


End Class
