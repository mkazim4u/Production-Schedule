Imports System
Imports System.Collections
Imports System.Data
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Security.Membership.Data
Imports DotNetNuke.Security.Roles
Imports DotNetNuke.Services.Exceptions
Imports System.Linq
Imports System.Reflection
Imports Telerik.Web.UI

Partial Class SNR_AddressBook
    Inherits System.Web.UI.UserControl

    Private dbContext As New SNRDentonDBLayerDataContext
    Private nUserID As Integer = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private uc As New DotNetNuke.Entities.Users.UserController


    Property pnAddressID() As Integer
        Get
            Dim o As Object = ViewState("AddressID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("AddressID") = Value
        End Set
    End Property
    ' if its true then personal address book else global address book
    Property pbMode() As Boolean
        Get
            Dim o As Object = ViewState("Mode")
            If o Is Nothing Then
                Return 1
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("Mode") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            PopulateCountry()

        End If

    End Sub

    Protected Sub btnToggle_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim btnToggle As Button = sender

        If pbMode Then
            pbMode = False
            BindGlobalAddressBook()
            btnToggle.Text = "Personal"
        Else
            pbMode = True
            BindPersonalAddressBook()
            btnToggle.Text = "Shared"
        End If


    End Sub

    Protected Sub btnAddAddress_Click(ByVal sender As Object, ByVal e As EventArgs)

        rwAddress.VisibleOnPageLoad = True
        rwAddress.Visible = True

    End Sub

    Private Sub BindGlobalAddressBook()


        Dim Address = (From add In dbContext.SNR_Addresses Where add.UserID = 0 And add.IsDeleted = False
                       ).ToList

        rlvAddress.DataSource = Address

    End Sub

    Private Sub BindPersonalAddressBook()


        Dim Address = (From add In dbContext.SNR_Addresses Where add.UserID = nUserID And add.IsDeleted = False
                       ).ToList

        rlvAddress.DataSource = Address

    End Sub

    Protected Sub rlvAddress_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.RadListViewNeedDataSourceEventArgs) Handles rlvAddress.NeedDataSource

        If pbMode Then
            BindPersonalAddressBook()
        Else
            BindGlobalAddressBook()
        End If



    End Sub

    Protected Sub rlvAddress_ItemCommand(ByVal sender As Object, ByVal e As RadListViewCommandEventArgs) Handles rlvAddress.ItemCommand

        If e.CommandName = "Insert" Then

            Insert(e)

        ElseIf e.CommandName = "Edit" Then

            Edit(e)

        ElseIf e.CommandName = "Delete" Then

            Delete(e)


        End If

    End Sub

    Protected Sub rlvAddress_ItemDataBound(ByVal sender As Object, ByVal e As RadListViewItemEventArgs) Handles rlvAddress.ItemDataBound

        If e.Item.ItemType = RadListViewItemType.DataItem Or e.Item.ItemType = RadListViewItemType.AlternatingItem Then


            Dim item As RadListViewItem = e.Item

            Dim lblCountry As Label = item.FindControl("lblCountry")
            Dim nCountryKey As Integer = Convert.ToInt64(lblCountry.Text)

            Dim country = (From con In dbContext.SNR_Countries
                          Where con.CountryKey = nCountryKey
                          Select con).SingleOrDefault

            If country IsNot Nothing Then

                lblCountry.Text = country.CountryName

            Else

                lblCountry.Text = String.Empty

            End If



            If pbMode Then

            Else
                Dim nPortalID As Integer = DNN.GetPMB(Me).PortalId

                Dim al As New ArrayList

                al = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, nUserID)

                Dim userrole = (From role As RoleInfo In al
                               Where role.RoleName = "Manager"
                               Select role).SingleOrDefault

                Dim lnkbtnDelete As LinkButton = item.FindControl("lnkbtnDelete")
                Dim lnkbtnEdit As LinkButton = item.FindControl("lnkbtnEdit")

                If userrole Is Nothing Then

                    lnkbtnDelete.Visible = False
                    lnkbtnEdit.Visible = False

                Else

                    lnkbtnDelete.Visible = True
                    lnkbtnEdit.Visible = True
                End If

            End If


        End If

    End Sub

    Protected Sub Delete(ByVal e As RadListViewCommandEventArgs)

        Dim item As RadListViewItem = e.ListViewItem

        Dim lnkbtnDelete As LinkButton = item.FindControl("lnkbtnDelete")

        Dim nAddressID As Integer = Convert.ToInt64(lnkbtnDelete.CommandArgument)

        Dim Address = (From add In dbContext.SNR_Addresses
                      Where add.ID = nAddressID
                      Select add).SingleOrDefault

        Address.IsDeleted = True

        dbContext.SubmitChanges()

        If pbMode Then
            BindPersonalAddressBook()
        Else
            BindGlobalAddressBook()
        End If

    End Sub

    Private Sub ClearFields()

        txtContactName.Text = String.Empty
        txtAddress1.Text = String.Empty
        txtAddress2.Text = String.Empty
        txtCity.Text = String.Empty
        txtRegion.Text = String.Empty
        txtPostalCode.Text = String.Empty
        txtCompanyName.Text = String.Empty
        txtPhone.Text = String.Empty
        txtMobile.Text = String.Empty


    End Sub

    Protected Sub Insert(ByVal e As RadListViewCommandEventArgs)

        If e.ListViewItem.ItemType = RadListViewItemType.InsertItem Then

            ClearFields()

            'Dim snrAddress As New SNR_Address

            'txtContactName.Text = String.Empty
            'txtAddress1.Text = String.Empty
            'txtAddress2.Text = String.Empty
            'txtCity.Text = String.Empty
            'txtRegion.Text = String.Empty
            'txtPostalCode.Text = String.Empty
            'txtCompanyName.Text = String.Empty
            'txtPhone.Text = String.Empty
            'txtMobile.Text = String.Empty
            rwAddress.Visible = True
            rwAddress.VisibleOnPageLoad = True
            rwAddress.Title = "Add New Address"
            btnUpdate.Text = "Insert"

            pnAddressID = -1

        End If




    End Sub

    Protected Sub Edit(ByVal e As RadListViewCommandEventArgs)

        'If e.ListViewItem.ItemType = RadListViewItemType.EditItem Then

        Dim item As RadListViewItem = e.ListViewItem

        Dim lnkbtnEdit As LinkButton = item.FindControl("lnkbtnEdit")

        Dim nAddressID As Integer = Convert.ToInt64(lnkbtnEdit.CommandArgument)

        pnAddressID = nAddressID

        Dim Address = (From add In dbContext.SNR_Addresses Where add.ID = pnAddressID).SingleOrDefault

        txtContactName.Text = Address.ContactName
        txtAddress1.Text = Address.Address1
        txtAddress2.Text = Address.Address2
        txtCity.Text = Address.City
        txtRegion.Text = Address.Region
        txtPostalCode.Text = Address.PostCode
        txtCompanyName.Text = Address.CompanyName
        txtPhone.Text = Address.Phone
        txtMobile.Text = Address.Mobile
        txtEmail.Text = Address.Email

        If pbMode Then

            chkIsGlobal.Visible = False

        Else

            chkIsGlobal.Visible = True
        End If

        If Address.UserID = 0 Then
            chkIsGlobal.Checked = True
        Else
            chkIsGlobal.Checked = False
        End If

        rwAddress.Visible = True
        rwAddress.VisibleOnPageLoad = True
        rwAddress.Title = "Edit Address"
        btnUpdate.Text = "Update"


        'End If



    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnUpdate.Click

        Dim btnUpdate As Button = sender

        Dim chkIsGlobal As CheckBox = btnUpdate.NamingContainer.FindControl("chkIsGlobal")

        If pnAddressID > 0 Then

            Dim Address = (From add In dbContext.SNR_Addresses
                          Where add.ID = pnAddressID
                          Select add).SingleOrDefault

            Address.ContactName = txtContactName.Text.Trim
            Address.Address1 = txtAddress1.Text.Trim
            Address.Address2 = txtAddress2.Text.Trim
            Address.City = txtCity.Text.Trim
            Address.Region = txtRegion.Text.Trim
            Address.PostCode = txtPostalCode.Text.Trim
            Address.CompanyName = txtCompanyName.Text.Trim
            Address.Phone = txtPhone.Text.Trim
            Address.Mobile = txtMobile.Text.Trim
            Address.CountryKey = Convert.ToInt64(rcbCountry.SelectedValue)
            Address.Email = txtEmail.Text.Trim
            Address.PortalID = DNN.GetPMB(Me).PortalId
            Address.CreatedOn = DateTime.Now
            Address.CreatedBy = nUserID
            Address.IsDeleted = False

            If chkIsGlobal.Checked Then
                Address.UserID = 0
            Else
                Address.UserID = nUserID
            End If

            dbContext.SubmitChanges()


        Else

            Dim Address As New SNR_Address

            Address.ContactName = txtContactName.Text.Trim
            Address.Address1 = txtAddress1.Text.Trim
            Address.Address2 = txtAddress2.Text.Trim
            Address.City = txtCity.Text.Trim
            Address.CountryKey = Convert.ToInt64(rcbCountry.SelectedValue)
            Address.Region = txtRegion.Text.Trim
            Address.PostCode = txtPostalCode.Text.Trim
            Address.CompanyName = txtCompanyName.Text.Trim
            Address.Phone = txtPhone.Text.Trim
            Address.Mobile = txtMobile.Text.Trim
            Address.Mobile = txtMobile.Text.Trim
            Address.PortalID = DNN.GetPMB(Me).PortalId
            Address.CreatedOn = DateTime.Now
            Address.CreatedBy = nUserID
            Address.UserID = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
            Address.IsDeleted = False
            Address.CopyFromGlobal = False
            dbContext.SNR_Addresses.InsertOnSubmit(Address)
            dbContext.SubmitChanges()

        End If


        rwAddress.Visible = False
        rwAddress.VisibleOnPageLoad = False

        If pbMode Then
            BindPersonalAddressBook()
        Else
            BindGlobalAddressBook()
        End If

        ClearFields()




    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancel.Click

        rwAddress.VisibleOnPageLoad = False
        rwAddress.Visible = False

        ClearFields()

    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As EventArgs)

        Dim btnSearch As Button = sender
        Dim txtSearch As TextBox = btnSearch.NamingContainer.FindControl("txtSearch")
        Dim search As String = txtSearch.Text.Trim
        rlvAddress.DataSource = dbContext.SNR_DENTON_ADDRESSBOOK_SEARCH(search)
        rlvAddress.DataBind()

    End Sub

    Private Sub PopulateCountry()

        Dim country = (From con In dbContext.SNR_Countries
                       Select con).ToList

        rcbCountry.DataSource = country
        rcbCountry.DataBind()

    End Sub



End Class

'Protected Sub rgAddress_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgAddress.NeedDataSource

'    Dim Address = (From add In dbContext.SNR_AddressBooks
'                  Select add).ToList

'    rgAddress.DataSource = Address

'End Sub

'Protected Sub rgAddress_ItemCommand(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgAddress.ItemCommand

'    If e.CommandName = "PerformInsert" Then

'        Insert(e)

'    ElseIf e.CommandName = "Update" Then

'        Update(e)

'    End If


'End Sub



'Protected Sub Insert(ByVal e As Telerik.Web.UI.GridCommandEventArgs)

'    If TypeOf e.Item Is GridEditFormInsertItem And e.Item.IsInEditMode Then

'        Dim txtFirstName As TextBox = e.Item.FindControl("txtFirstName")
'        Dim txtLastName As TextBox = e.Item.FindControl("txtLastName")
'        Dim txtAddress As TextBox = e.Item.FindControl("txtAddress")
'        Dim txtCity As TextBox = e.Item.FindControl("txtCity")
'        Dim txtRegion As TextBox = e.Item.FindControl("txtRegion")
'        Dim txtPostalCode As TextBox = e.Item.FindControl("txtPostalCode")
'        Dim txtCompanyName As TextBox = e.Item.FindControl("txtCompanyName")
'        Dim txtPhone As TextBox = e.Item.FindControl("txtPhone")
'        Dim txtMobile As TextBox = e.Item.FindControl("txtMobile")

'        'Dim snrAddress As New SNR_Address

'        Dim nPortalID As Integer = DNN.GetPMB(Me).PortalId
'        Dim nUserID As Integer = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID

'        snrAddress.AddressName = txtFirstName.Text.Trim
'        snrAddress.Address1 = txtLastName.Text.Trim
'        snrAddress.Address2 = txtAddress.Text.Trim
'        snrAddress.City = txtCity.Text.Trim
'        snrAddress.RegionCode = txtRegion.Text.Trim
'        snrAddress.PostalCode = txtPostalCode.Text.Trim
'        snrAddress.CompanyName = txtCompanyName.Text.Trim
'        snrAddress.Phone1 = txtPhone.Text.Trim
'        snrAddress.Phone2 = txtMobile.Text.Trim
'        snrAddress.PortalID = DNN.GetPMB(Me).PortalId
'        snrAddress.CreatedDate = DateTime.Now
'        snrAddress.CreatedByUser = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
'        snrAddress.UserID = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
'        snrAddress.PrimaryAddress = False

'        dbContext.SNR_Addresses.InsertOnSubmit(snrAddress)
'        dbContext.SubmitChanges()

'        dbContext.NEvoweb_NB_Store_Address_Update(-1, nPortalID, nUserID, "", txtFirstName.Text.Trim, txtLastName.Text.Trim, txtAddress.Text.Trim, txtCity.Text.Trim, txtRegion.Text.Trim, "", txtPostalCode.Text.Trim, txtPhone.Text.Trim, txtMobile.Text.Trim, False, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username, DateTime.Now, DNN.GetPMB(Me).PortalId, txtCompanyName.Text.Trim, "", "", "", "")




'    End If

'End Sub
