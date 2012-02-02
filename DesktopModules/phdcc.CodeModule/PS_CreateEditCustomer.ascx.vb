Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports Telerik.Web.UI
Imports System.IO
Imports System.Collections.Generic

Partial Class PS_CreateEditCustomer
    Inherits System.Web.UI.UserControl

    'Const MODE_CREATE As String = "create"
    'Const MODE_EDIT As String = "edit"
    Private gffCustomer As FF_Customer
    Private gffCustomerContact As FF_CustomerContact
    Dim gnFirstContact As Integer
    Dim gnContacts() As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Call SetModeAndGetCustomer()
            Call InitialiseControls()
            tbCustomerCode.Focus()
        End If
    End Sub

    Public ReadOnly Property BasePage As DotNetNuke.Framework.CDefault
        Get
            Return CType(Me.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

    Protected Sub SetModeAndGetCustomer()
        If pnCustomerID = 0 Then
            Dim sCustomerID As String = TryGetCustomerID()
            If sCustomerID <> String.Empty Then
                If IsNumeric(sCustomerID) Then
                    pnCustomerID = CInt(sCustomerID)
                Else
                    pnCustomerID = FF_Job.GetJobIDFromGUID(sCustomerID)
                End If
                gffCustomer = New FF_Customer(pnCustomerID)

                Call PopulateFormFromCustomerObject()

                psMode = FF_GLOBALS.MODE_EDIT
                Me.BasePage.Title = "Edit Customer"
                fslgndMain.InnerText = "Edit Customer"
            Else
                gffCustomer = New FF_Customer
                pnCustomerID = 0
                psMode = FF_GLOBALS.MODE_CREATE
                Me.BasePage.Title = "Create Customer"
                fslgndMain.InnerText = "Create Customer"
            End If
            If IsCustomerImport() Then
                psMode += FF_GLOBALS.MODE_IMPORT
            End If
        Else
            gffCustomer = New FF_Customer(pnCustomerID)
        End If
    End Sub

    Protected Sub InitialiseControls()
        fslgndMain.Visible = True
        fsImport.Visible = False
        btnAddAnotherContact.Visible = False
        divMultipleContacts.Visible = False
        If psMode.Contains(FF_GLOBALS.MODE_EDIT) Then
            'gnContacts = FF_CustomerContact.GetAllContacts(pnCustomerID)
            If gnContacts.Length > 0 Then
                btnAddAnotherContact.Visible = True
                If gnContacts.Length > 1 Then
                    divMultipleContacts.Visible = True
                End If
            End If
        End If
        If psMode.Contains(FF_GLOBALS.MODE_IMPORT) Then
            fsMain.Visible = False
            fsImport.Visible = True
            Call PopulateStockSystemCustomerDropdown()
            RadComboBoxStockSystemCustomer.Focus()
        End If
    End Sub

    Protected Function TryGetCustomerID() As String
        TryGetCustomerID = String.Empty
        If Request.Params.Count > 0 Then
            Try
                TryGetCustomerID = Request.Params("customer")
            Catch
            End Try
        End If
    End Function

    Protected Function IsCustomerImport() As Boolean
        IsCustomerImport = False
        Dim sType As String = String.Empty
        If Request.Params.Count > 0 Then
            Try
                sType = Request.Params("mode")
            Catch
            End Try
            IsCustomerImport = sType = "import"
        End If
    End Function


#Region "Helper functions (HideAllPanels, ...)"

    Protected Sub HideAllPanels()

    End Sub

#End Region


#Region "Properties"

    Property psMode() As String
        Get
            Dim o As Object = ViewState("Mode")
            If o Is Nothing Then
                Return FF_GLOBALS.MODE_CREATE
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("Mode") = Value
        End Set
    End Property

    Property pnCustomerID() As Integer
        Get
            Dim o As Object = ViewState("CustomerID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("CustomerID") = Value
        End Set
    End Property

    Property pnContactID() As Integer
        Get
            Dim o As Object = ViewState("ContactID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("ContactID") = Value
        End Set
    End Property


#End Region

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams))
    End Sub

    Protected Sub lnkbtnAddAddr3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnAddAddr3.Click
        divAddr3.Visible = True
        lnkbtnAddAddr3.Visible = False
    End Sub

    Protected Sub lnkbtnNonUKAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnNonUKAddress.Click
        divCountry.Visible = True
        lnkbtnNonUKAddress.Visible = False
        Call FF_GLOBALS.PopulateComboboxCountry(ddlCountry)
    End Sub

    Protected Sub btnUpdateCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateCustomer.Click
        Call UpdateCustomer()
    End Sub

    Protected Sub UpdateCustomer()
        tbCustomerCode.Text = tbCustomerCode.Text.Trim
        If psMode.Contains(FF_GLOBALS.MODE_CREATE) Then
            If FF_Customer.IsUniqueCustomerCode(tbCustomerCode.Text) Then
                gffCustomer = New FF_Customer
                'pnCustomerID = gffCustomer.Add()
                Call PopulateCustomerObjectFromForm()
                gffCustomer.IsActive = True
                pnCustomerID = gffCustomer.Add()

                tbContactName.Text = tbContactName.Text.Trim
                If tbContactName.Text <> String.Empty Then
                    gffCustomerContact = New FF_CustomerContact
                    gffCustomerContact.Name = tbContactName.Text
                    gffCustomerContact.Telephone = tbContactTelephone.Text
                    gffCustomerContact.Mobile = tbContactMobile.Text
                    gffCustomerContact.EmailAddr = tbContactEmailAddr.Text
                    gffCustomerContact.Notes = tbContactNotes.Text
                    gffCustomerContact.CustomerKey = pnCustomerID
                    pnContactID = gffCustomerContact.Add()
                    btnAddAnotherContact.Visible = True
                    If FF_CustomerContact.GetAllContactIDs(pnCustomerID).Length > 1 Then
                        divMultipleContacts.Visible = True
                    End If
                End If
                psMode = FF_GLOBALS.MODE_EDIT
            Else
                WebMsgBox.Show("There is already a customer with this customer code. The customer code must be unique.")
            End If
            Call NavigateTo(FF_GLOBALS.PAGE_OPTIONS)
        Else
            gffCustomer = New FF_Customer(pnCustomerID)
            Call PopulateCustomerObjectFromForm()
            If FF_Customer.IsUniqueCustomerCode(tbCustomerCode.Text, pnCustomerID) Then
                gffCustomer.Update(pnCustomerID)
                If pnContactID = -1 Or pnContactID > 0 Then
                    gffCustomerContact = New FF_CustomerContact
                    gffCustomerContact.Name = tbContactName.Text
                    gffCustomerContact.Telephone = tbContactTelephone.Text
                    gffCustomerContact.Mobile = tbContactMobile.Text
                    gffCustomerContact.EmailAddr = tbContactEmailAddr.Text
                    gffCustomerContact.Notes = tbContactNotes.Text
                    gffCustomerContact.CustomerKey = pnCustomerID
                    If pnContactID = -1 Then
                        pnContactID = gffCustomerContact.Add
                    Else
                        gffCustomerContact.Update(pnContactID)
                    End If
                    btnAddAnotherContact.Visible = True
                    If FF_CustomerContact.GetAllContactIDs(pnCustomerID).Length > 1 Then
                        divMultipleContacts.Visible = True
                    End If
                End If
            Else
                WebMsgBox.Show("There is already a customer with this customer code. The customer code must be unique.")
            End If

            Call NavigateTo(FF_GLOBALS.PAGE_OPTIONS)
        End If
    End Sub

    Protected Sub PopulateCustomerObjectFromForm()
        gffCustomer.CustomerCode = tbCustomerCode.Text
        gffCustomer.CustomerName = tbCustomerName.Text
        gffCustomer.CustomerAddr1 = tbAddr1.Text
        gffCustomer.CustomerAddr2 = tbAddr2.Text
        gffCustomer.CustomerAddr3 = tbAddr3.Text
        gffCustomer.CustomerTown = tbTown.Text
        gffCustomer.CustomerPostcode = tbPostcode.Text
        If divCountry.Visible = False Then
            gffCustomer.CustomerCountryKey = FF_GLOBALS.COUNTRY_KEY_UK
        Else
            gffCustomer.CustomerCountryKey = ddlCountry.SelectedValue
        End If
        If IsNumeric(hidExternalCustomerKey.Value) AndAlso hidExternalCustomerKey.Value > 0 Then
            gffCustomer.ExternalCustomerKey = hidExternalCustomerKey.Value
        Else
            gffCustomer.ExternalCustomerKey = 0
        End If
    End Sub

    Protected Sub PopulateFormFromCustomerObject()
        If gffCustomer.ExternalCustomerKey > 0 Then
            hidExternalCustomerKey.Value = gffCustomer.ExternalCustomerKey
        Else
            hidExternalCustomerKey.Value = 0
        End If
        tbCustomerCode.Text = gffCustomer.CustomerCode
        tbCustomerName.Text = gffCustomer.CustomerName
        tbAddr1.Text = gffCustomer.CustomerAddr1
        tbAddr2.Text = gffCustomer.CustomerAddr2
        If gffCustomer.CustomerAddr3.Trim <> String.Empty Then
            tbAddr3.Text = gffCustomer.CustomerAddr3
            divAddr3.Visible = True
            lnkbtnAddAddr3.Visible = False
        End If
        tbTown.Text = gffCustomer.CustomerTown
        tbPostcode.Text = gffCustomer.CustomerPostcode
        If gffCustomer.CustomerCountryKey = FF_GLOBALS.COUNTRY_KEY_UK Then
            divCountry.Visible = False
        Else
            divCountry.Visible = True
            lnkbtnNonUKAddress.Visible = False
            Call FF_GLOBALS.PopulateComboboxCountry(ddlCountry)
            For i As Integer = 0 To ddlCountry.Items.Count - 1
                If gffCustomer.CustomerCountryKey = ddlCountry.Items(i).Value Then
                    ddlCountry.SelectedIndex = i
                    Exit For
                End If
            Next
        End If
        'tbTelephone.Text = gffCustomer.CustomerTelephone
        'tbEmailAddr.Text = gffCustomer.CustomerEmail
        gnContacts = FF_CustomerContact.GetAllContactIDs(pnCustomerID)
        If gnContacts.Length > 0 Then
            pnContactID = gnContacts(0)
            gffCustomerContact = New FF_CustomerContact(gnContacts(0))
            Call InitContactFieldsFromContactObject()
        End If
    End Sub

    Protected Sub PopulateStockSystemCustomerDropdown()
        Dim sSQL As String
        sSQL = "SELECT CustomerAccountCode, CustomerName, CustomerKey FROM Customer WHERE DeletedFlag = 'N' AND CustomerStatusId = 'ACTIVE' AND ISNULL(AccountHandlerKey,0) > 0 ORDER BY CustomerAccountCode"
        Dim oDT As DataTable = SprintDB.Query(sSQL)
        RadComboBoxStockSystemCustomer.Items.Clear()
        RadComboBoxStockSystemCustomer.Items.Add(New RadComboBoxItem("- please select -", 0))
        For Each dr As DataRow In oDT.Rows
            RadComboBoxStockSystemCustomer.Items.Add(New RadComboBoxItem(dr(0) & " - " & dr(1), dr(2)))
        Next
    End Sub

    Protected Sub InitContactFieldsFromContactObject()
        tbContactName.Text = gffCustomerContact.Name
        tbContactTelephone.Text = gffCustomerContact.Telephone
        tbContactMobile.Text = gffCustomerContact.Mobile
        tbContactEmailAddr.Text = gffCustomerContact.EmailAddr
        tbContactNotes.Text = gffCustomerContact.Notes
    End Sub

    Protected Sub btnAddAnotherContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAnotherContact.Click
        Call AddAnotherContact()
    End Sub

    Protected Sub AddAnotherContact()
        pnContactID = -1
        tbContactName.Text = String.Empty
        tbContactTelephone.Text = String.Empty
        tbContactMobile.Text = String.Empty
        tbContactEmailAddr.Text = String.Empty
        tbContactNotes.Text = String.Empty
        btnAddAnotherContact.Visible = False
        tbContactName.Focus()
    End Sub

    Protected Sub lnkbtnPrevContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnPrevContact.Click
        pnContactID = GetPrevContactId()
        gffCustomerContact = New FF_CustomerContact(pnContactID)
        Call InitContactFieldsFromContactObject()
    End Sub

    Protected Sub lnkbtnNextContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnNextContact.Click
        pnContactID = GetNextContactId()
        gffCustomerContact = New FF_CustomerContact(pnContactID)
        Call InitContactFieldsFromContactObject()
    End Sub

    Protected Function GetPrevContactId() As Integer
        GetPrevContactId = 0
        gnContacts = FF_CustomerContact.GetAllContactIDs(pnCustomerID)
        Dim i As Integer
        For i = gnContacts.Length - 1 To 0 Step -1
            If gnContacts(i) = pnContactID Then
                If i > 0 Then
                    GetPrevContactId = gnContacts(i - 1)
                Else
                    GetPrevContactId = gnContacts(gnContacts.Length - 1)
                End If
                Exit For
            End If
        Next
    End Function

    Protected Function GetNextContactId() As Integer
        GetNextContactId = 0
        gnContacts = FF_CustomerContact.GetAllContactIDs(pnCustomerID)
        Dim i As Integer
        For i = 0 To gnContacts.Length - 1
            If gnContacts(i) = pnContactID Then
                If i < gnContacts.Length - 1 Then
                    GetNextContactId = gnContacts(i + 1)
                Else
                    GetNextContactId = gnContacts(0)
                End If
                Exit For
            End If
        Next
    End Function

    Protected Sub RadComboBoxStockSystemCustomer_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxStockSystemCustomer.SelectedIndexChanged
        Dim rcb As RadComboBox = o
        If rcb.SelectedValue > 0 Then
            Dim sSQL As String = "SELECT CustomerKey, CustomerAccountCode, CustomerName, CustomerAddr1, ISNULL(CustomerAddr2,'') 'CustomerAddr2', ISNULL(CustomerAddr3,'') 'CustomerAddr3', CustomerTown, ISNULL(CustomerPostCode,'') 'CustomerPostCode', CustomerCountryKey, ISNULL(DefaultContact,'') 'DefaultContact', ISNULL(DefaultTelephone,'') 'DefaultTelephone', ISNULL(DefaultEmail,'') 'DefaultEmail', AccountHandlerKey FROM Customer WHERE CustomerKey = " & rcb.SelectedValue
            Dim dr As DataRow = SprintDB.Query(sSQL).Rows(0)
            gffCustomer = New FF_Customer
            gffCustomer.ExternalCustomerKey = dr("CustomerKey")
            gffCustomer.CustomerCode = dr("CustomerAccountCode")
            gffCustomer.CustomerName = dr("CustomerName")
            gffCustomer.CustomerAddr1 = dr("CustomerAddr1")
            gffCustomer.CustomerAddr2 = dr("CustomerAddr2")
            gffCustomer.CustomerAddr3 = dr("CustomerAddr3")
            gffCustomer.CustomerTown = dr("CustomerTown")
            gffCustomer.CustomerPostcode = dr("CustomerPostCode")
            gffCustomer.CustomerCountryKey = dr("CustomerCountryKey")
            Call PopulateFormFromCustomerObject()
            If Not String.IsNullOrEmpty(dr("DefaultContact")) Then
                tbContactName.Text = dr("DefaultContact")
                tbContactTelephone.Text = dr("DefaultTelephone")
                tbContactEmailAddr.Text = dr("DefaultEmail")
            End If
        End If
        fsImport.Visible = False
        fsMain.Visible = True
    End Sub

End Class