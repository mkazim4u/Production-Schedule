Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Security.PortalSecurity
Imports DotNetNuke.Security.Roles
Imports Telerik.Web.UI
Imports System.IO
Imports System.Collections.Generic


Partial Class PS_CreateEditJob
    Inherits System.Web.UI.UserControl

    ' remove lnkbtnCreateNormal_Click, lnkbtnCreateTemplate_Click, lnkbtnCreateRecurring_Click
    ' if customer is changed, remove basket and stock item listing

    ' FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
    ' FF_Customer.GetRCBICustomerLocalKey(rcbCustomer.SelectedItem)

    Private gffJob As FF_Job
    Private gdtBasket As DataTable
    Private gdvBasketView As DataView
    Private gffCustomer As FF_Customer
    Private gffCustomerContact As FF_CustomerContact
    Private rc As New DotNetNuke.Security.Roles.RoleController
    Private rgi As New DotNetNuke.Security.Roles.RoleGroupInfo
    Private uc As New DotNetNuke.Entities.Users.UserController
    Private userRoleInfo As New DotNetNuke.Security.Roles.RoleInfo
    Private userInfo As DotNetNuke.Entities.Users.UserInfo
    Dim gnContacts() As Integer

    Private dbContext As New FFDataLayer.EntitiesModel
    'Private ffTariffItem As New FFDataLayer.FF_TariffItem
    Private ffItem As New FFDataLayer.FF_Item
    Private ffGroup As New FFDataLayer.FF_Group
    Private ffQuotation As New FFDataLayer.FF_Tariff




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then

            If IsInRole(FF_GLOBALS.ROLE_ACCOUNT_HANDLER) Then
                lnkbtnAccountHandlerMe.Visible = True
            Else
                lnkbtnAccountHandlerMe.Visible = False
            End If

            Call SetJobMode()
            Call InitialiseControls()
            RadUpload.TargetPhysicalFolder = GetJobFilePath()

            Call BindJobFiles()
            Call LoadUsersInStages(pnJobID)
            Call LoadRights()



            'Call ShowCustomerWindow()

        End If
    End Sub

    Protected Sub LoadRights()

        If Not IsHighPrivilege() Then

            btnUpdateJob.Visible = False
            btnCancelJob.Visible = False
            btnUpdateCustomer.Visible = False
            lnkbtnEditJobStates.Visible = False
            lnkbtnEditJobResources.Visible = False

        Else
            btnUpdateJob.Visible = True
            btnCancelJob.Visible = True
            btnUpdateCustomer.Visible = True
            lnkbtnEditJobStates.Visible = True
            lnkbtnEditJobResources.Visible = True
        End If

    End Sub

    Protected Function IsHighPrivilege() As Boolean
        IsHighPrivilege = False
        Dim nUserId As Integer = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        If Not (DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.IsSuperUser) Then
            Dim alRole As ArrayList = rc.GetUserRoles(DNN.GetPMB(Me).PortalId, nUserId)
            For Each roleInfo As DotNetNuke.Security.Roles.RoleInfo In alRole
                If roleInfo.RoleName = FF_GLOBALS.ROLE_MANAGER Or roleInfo.RoleName = FF_GLOBALS.ROLE_SUPER_USER Or roleInfo.RoleName = FF_GLOBALS.ROLE_ACCOUNT_HANDLER Then
                    IsHighPrivilege = True
                End If
            Next
        Else
            IsHighPrivilege = True
        End If

    End Function

    'Protected Sub rapCreateEditJob_AjaxRequest(ByVal sender As Object, ByVal e As Telerik.Web.UI.AjaxRequestEventArgs)
    '    If e.Argument IsNot Nothing Then
    '        Dim str As String = e.Argument.ToString
    '        If e.Argument = "JobEventLog" Then
    '            gvJobEventLog.DataSource = FF_AuditTrail.GetEventsForJob(pnJobID)
    '            gvJobEventLog.DataBind()
    '            'rwm.Windows(0).Visible = True
    '            rwJobEventLog.Visible = True

    '        End If
    '    End If

    'End Sub

    Protected Sub SetJobMode()
        If pnJobID = 0 Then
            Dim sJobIDOrGUID As String = TryGetJobIDOrGUID()
            If IsTemplate() Then
                psMode += FF_GLOBALS.MODE_TEMPLATE
                'psMode += FF_GLOBALS.MODE_COPY
            End If

            If sJobIDOrGUID <> String.Empty Then
                If IsNumeric(sJobIDOrGUID) Then
                    pnJobID = CInt(sJobIDOrGUID)
                Else
                    pnJobID = FF_Job.GetJobIDFromGUID(sJobIDOrGUID)
                End If
                gffJob = New FF_Job(pnJobID)
                psMode += FF_GLOBALS.MODE_EDIT
                If gffJob.IsTemplate Then
                    psMode += FF_GLOBALS.MODE_TEMPLATE
                    gffJob.IsTemplate = True
                End If
            ElseIf IsClipBoard() Then
                Dim cbjobId As Integer = Convert.ToInt64(Session(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD))
                gffJob = New FF_Job(cbjobId)
                gffJob.IsTemplate = False
                gffJob.JobGUID = Guid.NewGuid.ToString
                pnJobID = gffJob.Add()
                'Call FF_JobState.FetchJobStages(pnJobID)
                Call CreateJobFolder(pnJobID.ToString)
                psMode = FF_GLOBALS.MODE_CREATE

            ElseIf IsTemplateFromJob() Then

                Dim nJobID As Integer = Convert.ToInt64(Request.Params("jobId"))
                gffJob = New FF_Job(nJobID)
                gffJob.IsTemplate = True
                gffJob.JobGUID = Guid.NewGuid.ToString
                pnJobID = gffJob.Add()
                Call CreateJobFolder(pnJobID.ToString)
                psMode += FF_GLOBALS.MODE_TEMPLATE
                psMode += FF_GLOBALS.MODE_CREATE

            Else
                gffJob = New FF_Job
                If psMode = FF_GLOBALS.MODE_TEMPLATE Then
                    gffJob.IsCreated = True
                    gffJob.IsTemplate = True
                    gffJob.JobGUID = Guid.NewGuid.ToString
                End If
                pnJobID = gffJob.Add()
                Call CreateJobFolder(pnJobID.ToString)
                Call FF_JobState.FetchJobStages(pnJobID)
                psMode += FF_GLOBALS.MODE_CREATE
            End If
            Call SetTitles()
        Else
            gffJob = New FF_Job(pnJobID)
        End If
    End Sub

    Private Function GetJobGuid() As String

        Dim ff_job As New FF_Job(pnJobID)
        Dim strJobGuid As String = ff_job.JobGUID
        Return strJobGuid

    End Function


    Protected Sub ShowCustomerWindow()

        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), "key", "showWindow();", True)
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "key1", script)

        'Dim script As String = "<script language='javascript' type='text/javascript'>Sys.Application.add_load(showWindow);</script>"

        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "key1", "Sys.Application.add_load(function() {showWindow()});", True)
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "key2", "<script language='javascript' type='text/javascript'>showWindow();</script>", False)

    End Sub

    Protected Sub SetTitles()
        Dim sTitle As String = String.Empty

        If psMode.Contains(FF_GLOBALS.MODE_CREATE) Then
            sTitle = "Create"
            SetAddEditPageControls()
        End If
        If psMode.Contains(FF_GLOBALS.MODE_EDIT) Then
            sTitle = "Edit"
            SetAddEditPageControls()
        End If
        If psMode.Contains(FF_GLOBALS.MODE_TEMPLATE) Then
            sTitle += " Template"
            SetTemplatePageControls()
        Else
            sTitle += " Job"
            SetAddEditPageControls()
        End If
        If FF_GLOBALS.bDebugMode Then
            sTitle += " (" & pnJobID.ToString & ")"
        End If
        lblCreateOrEditJob.Text = sTitle
        Me.BasePage.Title = FF_GLOBALS.PAGE_TITLE_TEXT + " - " + sTitle
    End Sub

    Protected Sub SetAddEditPageControls()

        lblLegendJobName.Text = "Job Name"
        btnUpdateJob.Text = "Update Job"
        btnCancelJob.Visible = True
        lnkbtnCopyJobToClipboard.Visible = True
        lnkbtnViewJobEventLog.Visible = True

    End Sub

    Protected Sub SetTemplatePageControls()

        lblLegendJobName.Text = "Template Name"
        btnUpdateJob.Text = "Update Template"
        btnCancelJob.Visible = False
        lnkbtnCopyJobToClipboard.Visible = False
        lnkbtnViewJobEventLog.Visible = False

    End Sub


    Protected Sub InitialiseControls()
        btnUpdateJob.Visible = True
        tbJobName.Focus()
        'btnUpdateCustomer.Attributes.Add("onclick", "return close();")
        Call FF_Customer.PopulateCustomerDropdown(rcbCustomer, bIncludeExternalCode:=True)
        lblCreateOrEditJob.Text = lblCreateOrEditJob.Text + " - " + pnJobID.ToString()
        divMaterialsFromStock_Outer.Visible = False
        rttCustomer.Visible = False
        divCustomerContact.Visible = False
        Call PopulateAccountHandlerDropdown()
        Call BindJobStateGridView()
        If psMode.Contains(FF_GLOBALS.MODE_CREATE) Then
            Dim sTemplate As String = TryGetTemplate()
            If sTemplate <> String.Empty Then
                If Not IsNumeric(sTemplate) Then
                    Throw New Exception("Expected a numeric value for template!")
                Else
                    Call CloneFromExistingJobOrTemplate(CInt(sTemplate))
                End If
            Else
                Call PopulateJobTemplateDropdown()
                divJobTemplate.Visible = True
            End If

            If IsTemplateFromJob() Then

                Dim nJobID As Integer = Convert.ToInt64(Convert.ToInt64(Request.Params("jobId")))
                Call CloneFromExistingJobOrTemplate(nJobID)
                divJobTemplate.Visible = False

            End If

            tbNotes.ReadOnly = False
            divAddNote.Visible = False
            rttInstructionsNotes.Enabled = False
            rttInstructionsNotes.Visible = False
        ElseIf psMode.Contains(FF_GLOBALS.MODE_EDIT) Then
            Dim nLocalCustomerKey As Integer = gffJob.CustomerKey
            Call SetCustomerDropDown(nLocalCustomerKey)
            Call PopulateCustomerContact(nLocalCustomerKey)
            Call SetCustomerContactDropDown(nLocalCustomerKey, gffJob.CustomerContactKey)
            Call SetAccountHandlerDropDown(gffJob.AccountHandlerKey)
            tbJobName.Text = gffJob.JobName
            tbMaterialsSupplied.Text = gffJob.MaterialFromExternalSource
            rntbDistributionCost.Text = gffJob.DistributionCost
            rntbProductionCost.Text = gffJob.ProductionCost
            tbInstructions.Text = gffJob.Instructions
            pbIsInitialisingFromPersistedBasket = True
            Call GetSelectedUnpickedStockItems()
            pbIsInitialisingFromPersistedBasket = False
            divJobTemplate.Visible = False

            Call LoadTransientFormFromJobObject()
            If tbNotes.Text <> String.Empty Then
                tbNotes.ReadOnly = True
                divAddNote.Visible = True
            Else
                tbNotes.ReadOnly = False
                divAddNote.Visible = False
            End If
            rttInstructionsNotes.Enabled = True
            rttInstructionsNotes.Visible = True

            tbCustRef1.Text = gffJob.CustRef1
            tbCustRef2.Text = gffJob.CustRef2
            If gffJob.ItemCount > 0 Then
                rntbNumberOfItems.Value = gffJob.ItemCount
            End If
        End If
    End Sub

    Protected Sub LoadTransientFormFromJobObject()

        ' commented 4 some reason

        If gffJob.CollateralDueOn > FF_GLOBALS.BASE_DATE Then
            rdpCollateralDue.SelectedDate = gffJob.CollateralDueOn
        End If
        If gffJob.DeadlineOn > FF_GLOBALS.BASE_DATE Then
            rdpDeadlineOn.SelectedDate = gffJob.DeadlineOn
        End If
        tbNotes.Text = FF_Note.GetAllNotes(pnJobID, Me)
    End Sub

    Protected Sub CloneFromExistingJobOrTemplate(ByVal nExistingJobOrTemplateId As Int32)

        Call FF_JobState.CloneJobStateList(nExistingJobOrTemplateId, pnJobID)
        Call FF_UserJobMapping.CloneUserJobMapping(nExistingJobOrTemplateId, pnJobID, DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID)
        'Call BindJobStateGridView()
        tbNotes.Text = FF_Note.GetAllNotes(nExistingJobOrTemplateId, Me)

        Dim ffExistingJob As New FF_Job(nExistingJobOrTemplateId)
        ''''''' We don't Need to set this ''''''''''''''''''''''''''''''''''''''''

        rdpCollateralDue.SelectedDate = Nothing
        rdpDeadlineOn.SelectedDate = Nothing
        rntbNumberOfItems.Text = 0

        'tbJobName.Text = "Copy of " + ffExistingJob.JobName
        'If ffExistingJob.CollateralDueOn <> FF_GLOBALS.BASE_DATE Then
        '    rdpCollateralDue.SelectedDate = ffExistingJob.CollateralDueOn
        'End If

        'If ffExistingJob.DeadlineOn <> FF_GLOBALS.BASE_DATE Then
        '    rdpDeadlineOn.SelectedDate = ffExistingJob.DeadlineOn
        'End If

        'rntbNumberOfItems.Text = ffExistingJob.ItemCount

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        tbInstructions.Text = ffExistingJob.Instructions
        tbCustRef1.Text = ffExistingJob.CustRef1
        tbCustRef2.Text = ffExistingJob.CustRef2
        tbMaterialsSupplied.Text = ffExistingJob.MaterialFromExternalSource
        rntbDistributionCost.Text = ffExistingJob.DistributionCost
        rntbProductionCost.Text = ffExistingJob.ProductionCost
        ''''''''''''''''''''''''''''''''''''''''''''''''''''''
        Call SetCustomerDropDown(ffExistingJob.CustomerKey)
        Call PopulateCustomerContact(ffExistingJob.CustomerKey)
        Call SetCustomerContactDropDown(ffExistingJob.CustomerKey, ffExistingJob.CustomerContactKey)
        Call SetAccountHandlerDropDown(ffExistingJob.AccountHandlerKey)
        Call CloneUnpickedStockItems(nExistingJobOrTemplateId)
        pbIsInitialisingFromPersistedBasket = True
        Call GetSelectedUnpickedStockItems()
        pbIsInitialisingFromPersistedBasket = False
        divJobTemplate.Visible = False
        Call BindJobStateGridView()
        Call ReadJobFolder(ffExistingJob.ID)
        'Call SetJobStateGridView(pnJobID)
        'Call LoadUsersInStages(nExistingJobOrTemplateId)
        Call BindJobFiles()

        ' NEED TO ADD MATERIALS, STOCK, ETC.
    End Sub

    Protected Sub CloneUnpickedStockItems(ByVal nJobId As Int32)
        Dim nExternalCustomerKey As Int32 = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
        If nExternalCustomerKey > 0 AndAlso FF_Basket.GetTotalUnpickedItems(nJobId, nExternalCustomerKey) > 0 Then
            Dim dtPersistedBasket As DataTable = FF_Basket.GetUnpickedItems(nJobId, nExternalCustomerKey)
            For Each dr As DataRow In dtPersistedBasket.Rows
                Dim ffItem As New FF_Basket
                ffItem.CustomerKey = nExternalCustomerKey
                ffItem.JobId = pnJobID
                ffItem.LogisticProductKey = dr("LogisticProductKey")
                ffItem.Qty = dr("QtyToPick")
                ffItem.Add()
            Next
        End If
    End Sub

    Protected Sub GetSelectedUnpickedStockItems()
        Dim nExternalCustomerKey As Int32 = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
        If nExternalCustomerKey > 0 AndAlso FF_Basket.GetTotalUnpickedItems(pnJobID, nExternalCustomerKey) > 0 Then
            Dim dtPersistedBasket As DataTable = FF_Basket.GetUnpickedItems(pnJobID, nExternalCustomerKey)
            Call CreateBasketDataTable()
            Dim dr As DataRow
            For Each drPersistedBasketRow As DataRow In dtPersistedBasket.Rows
                Dim drProduct As DataRow
                If FF_SprintDBMode.GetDebugMode Then
                    drProduct = SprintDB_Test.Query(BuildSelectProductClause() & " WHERE lp.LogisticProductKey = " & drPersistedBasketRow("LogisticProductKey"))(0)
                Else
                    drProduct = SprintDB.Query(BuildSelectProductClause() & " WHERE lp.LogisticProductKey = " & drPersistedBasketRow("LogisticProductKey"))(0)
                End If

                dr = gdtBasket.NewRow()
                dr("LogisticProductKey") = drProduct("LogisticProductKey")
                dr("ProductCode") = drProduct("ProductCode")
                dr("ProductDescription") = drProduct("ProductDescription")
                dr("QtyAvailable") = drProduct("QtyAvailable")
                dr("QtyToPick") = drPersistedBasketRow("Qty")
                gdtBasket.Rows.Add(dr)
            Next
            Try
                gvStockItems.DataSource = gdtBasket
                gvStockItems.DataBind()
            Catch ex As Exception
            End Try
            gvStockItems.Visible = True
        End If
    End Sub

    Protected Sub SaveFormToJobObject()
        gffJob.JobName = tbJobName.Text
        gffJob.AccountHandlerKey = rcbAccountHandler.SelectedValue
        gffJob.MaterialFromExternalSource = tbMaterialsSupplied.Text
        If rntbDistributionCost.Text.Trim() <> String.Empty Then
            gffJob.ProductionCost = Decimal.Parse(rntbProductionCost.Text)
        End If
        If rntbProductionCost.Text.Trim() <> String.Empty Then
            gffJob.DistributionCost = Decimal.Parse(rntbDistributionCost.Text)
        End If
        gffJob.CustomerKey = FF_Customer.GetRCBICustomerLocalKey(rcbCustomer.SelectedItem)

        gffJob.Instructions = tbInstructions.Text

        'changed by M.K

        If rdpCollateralDue.SelectedDate IsNot Nothing Then
            gffJob.CollateralDueOn = rdpCollateralDue.SelectedDate
        Else
            gffJob.CollateralDueOn = FF_GLOBALS.BASE_DATE
        End If
        If rdpDeadlineOn.SelectedDate IsNot Nothing Then
            gffJob.DeadlineOn = rdpDeadlineOn.SelectedDate
        Else
            gffJob.DeadlineOn = FF_GLOBALS.BASE_DATE
        End If

        'changed by M.K

        If gffJob.CustomerKey > 0 Then
            If FF_CustomerContact.GetContactCount(gffJob.CustomerKey) > 1 Then
                gffJob.CustomerContactKey = rcbCustomerContact.SelectedValue
            Else
                gffJob.CustomerContactKey = pnContactID
            End If
        End If
        gffJob.CustRef1 = tbCustRef1.Text
        gffJob.CustRef2 = tbCustRef2.Text
        If IsNumeric(rntbNumberOfItems.Value) Then
            gffJob.ItemCount = rntbNumberOfItems.Value
        End If
        'gffJob.Instructions = tbNotes.Text ' ????????????
    End Sub

    Protected Sub PopulateJobTemplateDropdown()
        Dim sSQL As String = "SELECT JobName, [ID] FROM FF_Job WHERE IsTemplate = 1 AND IsCreated = 1 AND IsCancelled=0 AND IsDeleted = 0  ORDER BY JobName"

        Dim odt As DataTable = DNNDB.Query(sSQL)

        rcbJobTemplate.DataSource = odt
        rcbJobTemplate.DataBind()

        'rcbJobTemplate.Items.Clear()

        'If odt IsNot Nothing And odt.Rows.Count <> 0 Then
        '    For Each dr As DataRow In odt.Rows
        '        If dr(0).ToString() <> String.Empty Then
        '            Dim rcbItem As New RadComboBoxItem(dr(0), dr(1))
        '            rcbJobTemplate.Items.Add(rcbItem)
        '            'Dim lblJobTemplate As Label = rcbItem.FindControl("lblJobTemplate")
        '            Dim chkStages As CheckBox = rcbItem.FindControl("chkStages")
        '            'lblJobTemplate.Text = dr(0).ToString()
        '            chkStages.Text = dr(0).ToString() + " Only Stages"
        '        End If
        '    Next
        'End If

        'Dim rcbItem_select As New RadComboBoxItem("- please select -", -1)
        'rcbJobTemplate.Items.Insert(0, rcbItem_select)
        'rcbJobTemplate.FindControl("").r()
        'Dim lblSelectJobTemplate As Label = rcbItem_select.FindControl("lblJobTemplate")
        'lblSelectJobTemplate.Text = "- please select -"

    End Sub

    Protected Sub PopulateAccountHandlerDropdown()
        Dim rci As RadComboBoxItem
        Dim roleCtlr As New Security.Roles.RoleController
        Dim sRoleName As String = "Account Handlers"
        Dim objUserRoles As ArrayList = roleCtlr.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, FF_GLOBALS.ROLE_ACCOUNT_HANDLER)
        rcbAccountHandler.Items.Clear()
        rcbAccountHandler.Items.Add(New RadComboBoxItem("- please select -", 0))
        For Each uri As UserRoleInfo In objUserRoles
            Dim nUserId As Integer = uri.UserID
            Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)
            rci = rcbAccountHandler.FindItemByText(uiCurrentUser.Username)
            If rci Is Nothing Then
                rcbAccountHandler.Items.Add(New RadComboBoxItem(uiCurrentUser.Username, uiCurrentUser.UserID))
            End If
            'rcbAccountHandler.Items.Add(New RadComboBoxItem(uiCurrentUser.DisplayName & IIf(FF_GLOBALS.bDebugMode, " (" & uiCurrentUser.UserID & ")", ""), uiCurrentUser.UserID))

        Next
    End Sub

    Protected Function TryGetJobIDOrGUID() As String
        TryGetJobIDOrGUID = String.Empty
        If Request.Params.Count > 0 Then
            Try
                TryGetJobIDOrGUID = Request.Params("job")
            Catch
            End Try
        End If
    End Function
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
    Protected Function TryGetTemplate() As String
        TryGetTemplate = String.Empty
        If Request.Params.Count > 0 Then
            Try
                TryGetTemplate = Request.Params("template")
            Catch
            End Try
        End If
    End Function

    Protected Function IsTemplate() As Boolean
        IsTemplate = False
        Dim sType As String = String.Empty
        If Request.Params.Count > 0 Then
            Try
                sType = Request.Params("type")
            Catch
            End Try
            IsTemplate = sType = "template"
        End If
    End Function

    Protected Function IsClipBoard() As Boolean
        IsClipBoard = False
        Dim sType As String = String.Empty
        If Request.Params.Count > 0 Then
            Try
                sType = Request.Params("type")
            Catch
            End Try
            IsClipBoard = sType = "Clipboard"
        End If
    End Function

    Protected Function IsTemplateFromJob() As Boolean
        IsTemplateFromJob = False
        Dim sType As String = String.Empty
        If Request.Params.Count > 0 Then
            Try
                sType = Request.Params("type")
            Catch
            End Try
            IsTemplateFromJob = sType = "TemplateFromJob"
        End If
    End Function


#Region "Helper functions (HideAllPanels, ...)"

    Protected Sub HideAllPanels()

    End Sub

    Public ReadOnly Property BasePage As DotNetNuke.Framework.CDefault
        Get
            Return CType(Me.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

#End Region

#Region "Properties"


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

    Property psMode() As String
        Get
            Dim o As Object = ViewState("Mode")
            If o Is Nothing Then
                Return ""
            End If
            Return CStr(o)
        End Get
        Set(ByVal Value As String)
            ViewState("Mode") = Value
        End Set
    End Property

    Property pnJobID() As Integer
        Get
            Dim o As Object = ViewState("JobID")
            If o Is Nothing Then
                Return 0
            End If
            Return CInt(o)
        End Get
        Set(ByVal Value As Integer)
            ViewState("JobID") = Value
        End Set
    End Property

    Property pbIsInitialisingFromPersistedBasket() As Boolean
        Get
            Dim o As Object = ViewState("IsInitialisingFromPersistedBasket")
            If o Is Nothing Then
                Return False
            End If
            Return CBool(o)
        End Get
        Set(ByVal Value As Boolean)
            ViewState("IsInitialisingFromPersistedBasket") = Value
        End Set
    End Property

#End Region

    'Protected Sub lnkbtnCreateNormal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnCreateNormal.Click
    '    Call SetJobTemplateComboBox("NORMAL")
    'End Sub

    'Protected Sub lnkbtnCreateTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnCreateTemplate.Click
    '    Call SetJobTemplateComboBox("TEMPLATE")
    'End Sub

    'Protected Sub lnkbtnCreateRecurring_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnCreateRecurring.Click
    '    Call SetJobTemplateComboBox("RECURRING")
    'End Sub

    'Protected Sub SetJobTemplateComboBox(ByVal sValue As String)
    '    For i As Int32 = 0 To rcbJobTemplate.Items.Count - 1
    '        Dim li As RadComboBoxItem = rcbJobTemplate.Items(i)
    '        If li.Value = sValue Then
    '            rcbJobTemplate.SelectedIndex = i
    '            Exit For
    '        End If
    '    Next
    'End Sub

    Protected Sub SetAccountHandlerDropDown(ByVal sValue As String)

        Dim li As RadComboBoxItem = rcbAccountHandler.FindItemByValue(sValue)
        If li IsNot Nothing Then
            rcbAccountHandler.SelectedValue = li.Value
        End If
        'For i As Int32 = 0 To rcbAccountHandler.Items.Count - 1
        '    Dim li As RadComboBoxItem = rcbAccountHandler.Items(i)
        '    If li.Value = sValue Then
        '        rcbAccountHandler.SelectedIndex = i
        '        Exit For
        '    End If
        'Next
    End Sub

    Protected Sub SetCustomerDropDown(ByVal sValue As String)
        For i As Int32 = 0 To rcbCustomer.Items.Count - 1
            If FF_Customer.GetRCBICustomerLocalKey(rcbCustomer.Items(i)) = sValue Then
                rcbCustomer.SelectedIndex = i
                Exit For
            End If
        Next
        Call SetCustomerTooltipData(sValue)
        If FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem) > 0 Then
            divMaterialsFromStock_Outer.Visible = True
        End If
    End Sub

    Protected Sub SetCustomerContactDropDown(ByVal nCustomerKey As Int32, ByVal nCustomerContactKey As Int32)
        Dim nContactCount As Int32 = FF_CustomerContact.GetContactCount(nCustomerKey)
        If nContactCount > 1 Then
            For i As Int32 = 0 To rcbCustomerContact.Items.Count - 1
                If rcbCustomerContact.Items(i).Value = nCustomerContactKey Then
                    rcbCustomerContact.SelectedIndex = i
                    If nCustomerContactKey > 0 Then
                        Call SetCustomerContactTooltipData(nCustomerContactKey)
                    End If
                    Exit For
                End If
            Next
        End If
    End Sub

    Protected Sub lnkbtnAccountHandlerMe_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnAccountHandlerMe.Click
        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetCurrentUserInfo
        Dim sCurrentUserDisplayName As String = uiCurrentUser.Username
        Dim rci As RadComboBoxItem
        rci = rcbAccountHandler.FindItemByText(sCurrentUserDisplayName)
        If rci IsNot Nothing Then
            rcbAccountHandler.SelectedValue = uiCurrentUser.UserID
        Else
            rcbAccountHandler.SelectedValue = "0"
        End If
    End Sub

    Protected Sub btnUpdateJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateJob.Click
        Call UpdateJob()
    End Sub



    Protected Sub UpdateJob()

        UpdateEmailPreference()
        If IsDate(rdpCollateralDue.SelectedDate) AndAlso IsDate(rdpDeadlineOn.SelectedDate) Then
            If rdpCollateralDue.SelectedDate > rdpDeadlineOn.SelectedDate Then
                WebMsgBox.Show("Collateral Due date is after Deadline date. Please correct.")
                Exit Sub
            End If
        End If
        If tbJobName.Text.Trim = String.Empty Then
            WebMsgBox.Show("Warning - this job or template has no name.")
        End If
        gffJob = New FF_Job(pnJobID)
        Call SaveFormToJobObject()
        Call SaveUnpickedStockItems()
        gffJob.IsCreated = True
        If psMode.Contains(FF_GLOBALS.MODE_CREATE) Then
            Dim ffAuditEvent As New FF_AuditTrail(gffJob, Me)
        Else
            Dim ffAuditEvent As New FF_AuditTrail(New FF_Job(pnJobID), gffJob, Me)
        End If
        gffJob.Update(pnJobID)
        tbNotes.Text = tbNotes.Text.Trim
        If tbNotes.ReadOnly = False AndAlso tbNotes.Text <> String.Empty Then
            Dim ffNote As New FF_Note
            ffNote.JobID = pnJobID
            ffNote.Note = tbNotes.Text
            ffNote.CreatedBy = DNN.GetPMB(Me).UserId
            ffNote.CreatedOn = Date.Now
            ffNote.Add()
        End If

        Call NavigateTo(FF_GLOBALS.PAGE_PRODUCTION_SCHEDULE_LIST)
    End Sub

    Protected Sub UpdateEmailPreference()

        Dim nIsEmail As Int16
        For Each jobStaterow As GridViewRow In gvJobStates.Rows
            If jobStaterow.RowType = DataControlRowType.DataRow Then
                Dim gvUserJobMapping As GridView = jobStaterow.FindControl("gvUserJobMapping")
                If gvUserJobMapping IsNot Nothing Then
                    For Each userJobMappingRow As GridViewRow In gvUserJobMapping.Rows
                        If userJobMappingRow.RowType = DataControlRowType.DataRow Then
                            Dim hdJobStateID As HiddenField
                            hdJobStateID = userJobMappingRow.FindControl("hidIsJobStateID")
                            Dim hdJobID As HiddenField
                            hdJobID = userJobMappingRow.FindControl("hidIsJobID")
                            Dim hidIsUserID As HiddenField
                            hidIsUserID = userJobMappingRow.FindControl("hidIsUserID")
                            Dim check As CheckBox = userJobMappingRow.FindControl("cbIsEmail")
                            If check IsNot Nothing Then
                                If check.Checked Then
                                    nIsEmail = 1
                                Else
                                    nIsEmail = 0
                                End If
                            End If

                            FF_UserJobMapping.UpdateUserEmailPreference(hdJobID.Value, hidIsUserID.Value, nIsEmail, hdJobStateID.Value)
                        End If
                    Next
                End If

            End If
        Next


    End Sub

    Protected Sub SaveUnpickedStockItems()
        If gvStockItems.Rows.Count > 0 Then
            Call LoadBasketDataTable()
            If gdtBasket.Rows.Count > 0 Then
                Dim dictStockItems As New Dictionary(Of Int32, Int32)
                For Each dr As DataRow In gdtBasket.Rows
                    dictStockItems.Add(dr("LogisticProductKey"), CInt(dr("QtyToPick")))
                Next
                FF_Basket.RefreshBasket(pnJobID, FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem), dictStockItems)
            End If
        End If
    End Sub

    Protected Sub btnCancelJob_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelJob.Click

        'rapCreateEditJob.Alert("Are you sure you want to cancel this job")
        gffJob = New FF_Job(pnJobID)
        gffJob.IsCancelled = True
        Dim ffAuditEvent As New FF_AuditTrail(New FF_Job(pnJobID), gffJob, Me)
        gffJob.Update(pnJobID)
        WebMsgBox.Show("Job cancelled.")
        Call NavigateTo(FF_GLOBALS.PAGE_PRODUCTION_SCHEDULE_LIST)
    End Sub

    Protected Sub lnkbtnEditJobStates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnEditJobStates.Click
        gffJob = New FF_Job(pnJobID)
        Call SaveFormToJobObject()
        gffJob.Update(pnJobID)
        Dim sQueryParams(1) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_EDIT_JOB_STATES, sQueryParams)
    End Sub
    Protected Sub lnkbtnAddResources_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnEditJobResources.Click
        gffJob = New FF_Job(pnJobID)
        Call SaveFormToJobObject()
        gffJob.Update(pnJobID)
        Dim sQueryParams(0) As String
        sQueryParams(0) = "job=" & pnJobID
        Call NavigateTo(FF_GLOBALS.PAGE_ADD_RESOURCES, sQueryParams)
    End Sub

    Protected Sub NavigateTo(ByVal sPageName As String, Optional ByVal sQueryParams() As String = Nothing)
        Dim tabctrl As New DotNetNuke.Entities.Tabs.TabController
        Dim pi As New DotNetNuke.Entities.Portals.PortalInfo
        Dim tInfo As DotNetNuke.Entities.Tabs.TabInfo = tabctrl.GetTabByName(sPageName, pi.PortalID)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams), False)
        Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(tInfo.TabID, "", sQueryParams), False)
    End Sub

    Protected Sub ReadJobFolder(ByVal sFolderName As String)

        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        Dim sourceFolder As String = sFolderName
        Dim destinationFolder As String = pnJobID.ToString()
        'Dim difFiles As DirectoryInfo = New DirectoryInfo(ps.HomeDirectoryMapPath + FF_GLOBALS.PATH_TO_PS_USER_FILES + sourceFolder)

        For Each fName As String In Directory.GetFiles(ps.HomeDirectoryMapPath + FF_GLOBALS.PATH_TO_PS_USER_FILES + sourceFolder)
            If File.Exists(fName) Then
                Dim dFile As String = String.Empty
                dFile = Path.GetFileName(fName)
                File.Copy(fName, ps.HomeDirectoryMapPath + FF_GLOBALS.PATH_TO_PS_USER_FILES + destinationFolder + "\" + dFile, True)
            End If
        Next
        'difFiles.GetFiles("*.gif")
        'Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        'Dim sDestinationFoder As String = pnJobID.ToString()
        'Dim sSourceFolder As String = ps.HomeDirectoryMapPath & FF_GLOBALS.PATH_TO_PS_USER_FILES & sFolderName
        'Dim oFolderController As New DotNetNuke.Services.FileSystem.FolderController
        'Dim objFileController As New Services.FileSystem.FileController
        'Dim objFolderInfo As Services.FileSystem.FolderInfo = oFolderController.GetFolder(ps.PortalId, sSourceFolder, True)
        'If objFolderInfo IsNot Nothing Then
        '    Dim dtFiles As DataTable = objFileController.GetAllFiles()
        '    If dtFiles IsNot Nothing Then
        '        For Each file As Services.FileSystem.FileInfo In dtFiles.Rows
        '            Dim objFileInfo As Services.FileSystem.FileInfo = objFileController.GetFile(file.FileName, ps.PortalId, objFolderInfo.FolderID)
        '            objFileInfo.Folder = sDestinationFoder
        '            If objFileInfo Is Nothing Then
        '                Dim filedId As Integer = objFileController.AddFile(objFileInfo)
        '            End If

        '        Next

        '    End If
        'End If

    End Sub

    Protected Sub CreateJobFolder(ByVal sFolderName As String)


        'Dim pmb As PortalModuleBase = DNN.GetPMB(Me)
        Dim ps As PortalSettings = DNN.GetPMB(Me).PortalSettings
        'Dim sFolderRoot As String = ps.HomeDirectoryMapPath & "ProductionSchedule/"
        Dim sFolderRoot As String = ps.HomeDirectoryMapPath & FF_GLOBALS.PATH_TO_PS_USER_FILES
        If Not Directory.Exists(sFolderRoot & sFolderName) Then
            Dim oFolderController As New DotNetNuke.Services.FileSystem.FolderController 'add folder and permissions
            'oFolderController.
            Dim nFolderid As Integer = oFolderController.AddFolder(ps.PortalId, sFolderName)
            Dim oPermissionController As New DotNetNuke.Security.Permissions.PermissionController
            Dim arrlst As ArrayList = oPermissionController.GetPermissionByCodeAndKey("SYSTEM_FOLDER", "")
            For Each oPermissionInfo As DotNetNuke.Security.Permissions.PermissionInfo In arrlst
                If oPermissionInfo.PermissionKey = "READ" Then ' add READ permissions to the All Users Role
                    FileSystemUtils.SetFolderPermission(ps.PortalId, nFolderid, oPermissionInfo.PermissionID, Integer.Parse(glbRoleAllUsers), "")
                End If
                If oPermissionInfo.PermissionKey = "WRITE" Then ' add READ permissions to the All Users Role
                    FileSystemUtils.SetFolderPermission(ps.PortalId, nFolderid, oPermissionInfo.PermissionID, Integer.Parse(glbRoleAllUsers), "")
                End If
            Next
        End If
        Dim sNewDir As String = sFolderRoot & sFolderName
        MkDir(sNewDir)

    End Sub

    Protected Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Call BindJobFiles()
        'FileSystemUtils.Synchronize(DNN.GetPMB(Me).PortalId, -1, DNN.GetPMB(Me).PortalSettings.HomeDirectory & pnJobID.ToString, True)
        'GC.Collect()
    End Sub

    Protected Sub rcbJobTemplate_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbJobTemplate.SelectedIndexChanged

        'Dim rcb As RadComboBox = o
        'Dim chkStages As CheckBox = rcb.NamingContainer.FindControl("chkStages")
        'Dim nTemplateId As Integer = e.Value
        'If rcb.SelectedIndex > 0 And chkStages.Checked Then
        '    Call FF_JobState.CloneJobStateList(nTemplateId, pnJobID)
        'ElseIf rcb.SelectedIndex > 0 Then
        '    Call CloneFromExistingJobOrTemplate(nTemplateId)
        'End If

    End Sub

    Protected Sub chkTemplate_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkTemplate As CheckBox = sender
        Dim rcbJobTemplateItem As RadComboBoxItem = chkTemplate.NamingContainer
        Dim nTemplateId As Integer = rcbJobTemplateItem.Value
        If nTemplateId > 0 Then
            Call CloneFromExistingJobOrTemplate(nTemplateId)
            divJobTemplate.Visible = False
            Call BindJobStateGridView()

        End If

    End Sub

    Protected Sub chkStages_OnCheckedChanged(ByVal sender As Object, ByVal e As EventArgs)

        Dim chkStages As CheckBox = sender
        Dim rcbJobTemplateItem As RadComboBoxItem = chkStages.NamingContainer
        Dim nTemplateId As Integer = rcbJobTemplateItem.Value
        If nTemplateId > 0 Then
            Call FF_JobState.CloneJobStateList(nTemplateId, pnJobID)
            divJobTemplate.Visible = False
            Call BindJobStateGridView()

        End If

    End Sub

    Protected Sub BindJobStateGridView()
        Dim sSQL As String = "SELECT [id] 'JobStateKey', JobStateName, IsCompleted FROM FF_JobState WHERE JobID = " & pnJobID & " ORDER BY Position"
        Dim dt As DataTable = DNNDB.Query(sSQL)
        If dt IsNot Nothing Then
            If dt.Rows.Count <> 0 Then
                gvJobStates.DataSource = dt
                gvJobStates.DataBind()
                'Else
                'FF_JobState.InitJobStateList(pnJobID)
                'SetJobStateGridView(pnJobID)
            End If
        End If
    End Sub

    Protected Sub SetJobStateGridView(ByVal nJobId As Integer)

        Dim sqlParamJobId(0) As SqlParameter
        sqlParamJobId(0) = New System.Data.SqlClient.SqlParameter("@JobId", SqlDbType.BigInt)
        sqlParamJobId(0).Value = nJobId
        gvJobStates.DataSource = DNNDB.ExecuteStoredProcedure("FF_GetAllStatesByJobID", sqlParamJobId)
        gvJobStates.DataBind()

    End Sub

    Protected Sub cbIsEmail_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim cb As CheckBox = sender
        Dim nJobStateKey As Integer = cb.ValidationGroup
        Dim ffJobState As New FF_JobState(nJobStateKey)
        If cb.Checked Then
            If ffJobState.JobStateName = FF_GLOBALS.JOB_STATE_COMPLETED Then
                Call FF_JobState.SetAllStagesCompleted(pnJobID)
                Call BindJobStateGridView()
                ' NEED TO LOG THIS EVENT
            End If
        End If
        ffJobState.IsCompleted = cb.Checked
        Dim ffAuditEvent As New FF_AuditTrail(ffJobState, Me)
        ffJobState.Update(nJobStateKey)
        Call SetJobStateTextBold(nJobStateKey, cb.Checked)
    End Sub

    Protected Sub SetJobStateTextBold(ByVal nJobStateKey As Integer, ByVal bChecked As Boolean)
        Dim gvr As GridViewRow
        Dim hid As HiddenField
        Dim lbl As Label
        For Each gvr In gvJobStates.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                hid = gvr.FindControl("hidJobStateKey")
                If hid.Value = nJobStateKey Then
                    lbl = gvr.FindControl("lblJobStateName")
                    lbl.Font.Bold = bChecked
                    Exit For
                End If
            End If
        Next
    End Sub
    Public Shared Function FindControlInHeirarchy(ByVal root As Control, ByVal controlId As String) As Control
        If root.ID = controlId Then
            Return root
        End If

        For Each control As Control In root.Controls
            Dim fintCtl As Control = FindControlInHeirarchy(control, controlId)
            If fintCtl IsNot Nothing Then
                Return fintCtl
            End If
        Next

        Return Nothing
    End Function

    Protected Sub gvJobStates_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvJobStates.RowDataBound

        Dim gvr As GridViewRow = e.Row

        If gvr.RowType = DataControlRowType.DataRow Then

            Dim nJobStateId As Integer
            nJobStateId = CType(e.Row.DataItem, DataRowView).Row("JobStateKey")
            Dim gvUserJobMapping As GridView
            gvUserJobMapping = gvr.FindControl("gvUserJobMapping")
            Dim radToolTipUsers As RadToolTip
            radToolTipUsers = gvr.FindControl("RadToolTipUsers")
            If gvUserJobMapping IsNot Nothing Then
                Dim dt As DataTable = FF_UserJobMapping.FetchUserInJobStage(pnJobID, nJobStateId)
                If dt IsNot Nothing And dt.Rows.Count <> 0 Then

                    'For Each dr As datarow In dt.rows
                    '    Dim sUserName As String = dr("UserName")
                    '    If rc.GetRoleByName(DNN.GetPMB(Me).PortalId, sUserName) IsNot Nothing Then
                    '        Dim usersList As ArrayList = rc.GetUserRolesByRoleName(DNN.GetPMB(Me).PortalId, sUserName)
                    '        If usersList.Count <> 0 Then
                    '            For Each user As DotNetNuke.Entities.Users.UserRoleInfo In usersList

                    '            Next
                    '        End If
                    '    End If

                    'Next


                    gvUserJobMapping.DataSource = dt
                    gvUserJobMapping.DataBind()
                Else
                    radToolTipUsers.Text = "No Resources Assigned To This Stage"
                End If
            End If
        End If

    End Sub

    Protected Sub btnSaveNote_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveNote.Click
        tbNewNote.Text = tbNewNote.Text.Trim
        If tbNewNote.Text <> String.Empty Then
            Dim ffNote As New FF_Note
            ffNote.JobID = pnJobID
            ffNote.Note = tbNewNote.Text
            ffNote.CreatedBy = DNN.GetPMB(Me).UserId
            ffNote.CreatedOn = Date.Now
            ffNote.Add()
            tbNotes.Text = FF_Note.GetAllNotes(pnJobID, Me)
            tbNewNote.Text = String.Empty
        End If
    End Sub

    Protected Sub lnkbtnIncrementBy1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnIncrementBy1.Click
        rntbNumberOfItems.IncrementSettings.Step = 1
        lblCurrentIncrement.Text = "1"
        rntbNumberOfItems.Focus()
    End Sub

    Protected Sub lnkbtnIncrementBy10_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnIncrementBy10.Click
        rntbNumberOfItems.IncrementSettings.Step = 10
        lblCurrentIncrement.Text = "10"
        rntbNumberOfItems.Focus()
    End Sub

    Protected Sub lnkbtnIncrementBy100_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnIncrementBy100.Click
        rntbNumberOfItems.IncrementSettings.Step = 100
        lblCurrentIncrement.Text = "100"
        rntbNumberOfItems.Focus()
    End Sub

    Protected Sub lnkbtnIncrementBy1000_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnIncrementBy1000.Click
        rntbNumberOfItems.IncrementSettings.Step = 1000
        lblCurrentIncrement.Text = "1000"
        rntbNumberOfItems.Focus()
    End Sub

    Protected Sub lnkbtnClearNumberOfItemsField_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnClearNumberOfItemsField.Click
        rntbNumberOfItems.Value = 0
        rntbNumberOfItems.Focus()
    End Sub

    Protected Sub btnSearchGo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchGo.Click
        Call SearchGo()
    End Sub

    Protected Function BuildSelectProductClause() As String
        Dim sbSQL As New StringBuilder
        sbSQL.Append("SELECT DISTINCT lp.LogisticProductKey, ProductCode + ' - ' + ISNULL(ProductDate,'') 'ProductCode', ProductDescription, ")
        sbSQL.Append("QtyAvailable = CASE ISNUMERIC((SELECT sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey)) WHEN 0 THEN 0 ELSE (SELECT sum(LogisticProductQuantity) FROM LogisticProductLocation AS lpl WHERE lpl.LogisticProductKey = lp.LogisticProductKey) END ")
        sbSQL.Append(", QtyToPick = ''")
        sbSQL.Append("FROM LogisticProduct AS lp LEFT OUTER JOIN LogisticProductLocation AS lpl ON lp.LogisticProductKey = lpl.LogisticProductKey ")
        BuildSelectProductClause = sbSQL.ToString
    End Function

    Protected Sub SearchGo()
        Dim nExternalCustomerKey As Integer = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
        rtbSearchForStockItems.Text = rtbSearchForStockItems.Text.Trim
        Dim sbSQL As New StringBuilder
        sbSQL.Append(BuildSelectProductClause)
        sbSQL.Append(" WHERE CustomerKey = " & nExternalCustomerKey & " AND DeletedFlag = 'N' AND ArchiveFlag = 'N' ")
        rtbSearchForStockItems.Text = rtbSearchForStockItems.Text.Trim
        If rtbSearchForStockItems.Text <> String.Empty Then
            sbSQL.Append(" AND (")
            Dim sSearchTerms() As String = rtbSearchForStockItems.Text.Split("|")
            Dim bSubsequentSearchTerm As Boolean = False
            For Each s As String In sSearchTerms
                Dim sSearchTerm = s.Trim
                If bSubsequentSearchTerm Then
                    sbSQL.Append(" OR ")
                End If
                sbSQL.Append(" ProductCode LIKE '%" & sSearchTerm & "%' OR ProductDescription LIKE '%" & sSearchTerm & "%' ")
                bSubsequentSearchTerm = True
            Next
            sbSQL.Append(" ) ")
        End If

        Dim oDT As DataTable
        If FF_SprintDBMode.GetDebugMode Then
            oDT = SprintDB_Test.Query(sbSQL.ToString)
        Else
            oDT = SprintDB.Query(sbSQL.ToString)
        End If

        gvStockItems.DataSource = oDT
        gvStockItems.DataBind()
        gvStockItems.Visible = True
        lblLegendStockItems.Visible = True
    End Sub

    Protected Sub HideAllRadWindows()

        rwCustomer.Visible = False
        rwJobEventLog.Visible = False
        rwBasket.Visible = False

    End Sub

    Protected Sub rcbCustomer_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbCustomer.SelectedIndexChanged
        Dim rcb As RadComboBox = o
        Dim nCustomerKey As Int32 = FF_Customer.GetRCBICustomerLocalKey(rcb.SelectedItem)
        'If rcb.SelectedIndex > 0 Then
        '    Call PopulateCustomerContact(nCustomerKey)
        '    divCustomerContact.Visible = True
        'Else
        '    divCustomerContact.Visible = False
        'End If
        Call PopulateCustomerContact(nCustomerKey)
        Call SetCustomerTooltipData(nCustomerKey)
        gvStockItems.DataSource = Nothing
        gvStockItems.DataBind()
        gvStockItems.Visible = False
        lblLegendStockItems.Visible = False
        rwBasket.Visible = False
        ' REMOVE ANY UNPICKED ITEMS FROM BASKET
        If FF_Customer.GetRCBICustomerExternalKey(rcb.SelectedItem) > 0 Then
            divMaterialsFromStock_Outer.Visible = True
        Else
            divMaterialsFromStock_Outer.Visible = False
        End If


        Call HideAllRadWindows()

    End Sub


    Protected Sub SetCustomerTooltipData(ByVal nCustomerKey As Int32)
        If nCustomerKey > 0 Then
            rttCustomer.Visible = True
            Dim ffCustomer As New FF_Customer(nCustomerKey)
            Dim sbDescription As New StringBuilder
            sbDescription.Append("<table>")

            sbDescription.Append("<tr>")
            sbDescription.Append("<td>")
            sbDescription.Append("Customer Name")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomer.CustomerName)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")

            sbDescription.Append("<td>")
            sbDescription.Append("Addr 1")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomer.CustomerAddr1)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")

            If ffCustomer.CustomerAddr2.Trim <> String.Empty Then
                sbDescription.Append("<td>")
                sbDescription.Append("Addr 2")
                sbDescription.Append("</td>")
                sbDescription.Append("<td>")
                sbDescription.Append(ffCustomer.CustomerAddr2)
                sbDescription.Append("</td>")
                sbDescription.Append("</tr>")
            End If

            If ffCustomer.CustomerAddr3.Trim <> String.Empty Then
                sbDescription.Append("<td>")
                sbDescription.Append("Addr 3")
                sbDescription.Append("</td>")
                sbDescription.Append("<td>")
                sbDescription.Append(ffCustomer.CustomerAddr3)
                sbDescription.Append("</td>")
                sbDescription.Append("</tr>")
            End If

            sbDescription.Append("<td>")
            sbDescription.Append("Town")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomer.CustomerTown)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")

            sbDescription.Append("<td>")
            sbDescription.Append("Post code")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomer.CustomerPostcode)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")

            sbDescription.Append("</table>")
            lblCustomerTooltipData.Text = sbDescription.ToString
        Else
            lblCustomerTooltipData.Text = String.Empty
            rttCustomer.Visible = False
        End If
    End Sub
    Protected Sub lnkbtnNonUKAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnNonUKAddress.Click
        trCountry.Visible = True
        lnkbtnNonUKAddress.Visible = False
        Call FF_GLOBALS.PopulateComboboxCountry(ddlCountry)
    End Sub

    Protected Sub lnkRwBasketNonUKAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkRwBasketNonUKAddress.Click
        trRwBasketCountry.Visible = True
        lnkRwBasketNonUKAddress.Visible = False
        Call FF_GLOBALS.PopulateComboboxCountry(ddlRwBasketCountry)
    End Sub

    Protected Sub ddlRwBasketCountry_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ddlRwBasketShortcut.SelectedIndexChanged

        If ddlRwBasketShortcut.SelectedValue <> 0 Then

            Dim nRecipientid As Integer = Convert.ToInt64(ddlRwBasketShortcut.SelectedValue)
            Dim sqlParamRecipientId(0) As SqlParameter
            sqlParamRecipientId(0) = New System.Data.SqlClient.SqlParameter("@RecipientId", SqlDbType.BigInt)
            sqlParamRecipientId(0).Value = nRecipientid
            Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetRecipientById", sqlParamRecipientId)

            If dt IsNot Nothing And dt.Rows.Count <> 0 Then

                Dim dr As DataRow = dt.Rows(0)

                txtRwBasketAddr1.Text = dr("CneeAddr1").ToString()
                txtRwBasketAddr2.Text = dr("CneeAddr2").ToString()
                txtRwBasketCustomerName.Text = dr("CneeCtcName").ToString()
                txtRwBasketName.Text = dr("CneeName").ToString()
                txtRwBasketState.Text = dr("CneeState").ToString()
                txtRwBasketTown.Text = dr("CneeTown").ToString()
                ddlRwBasketCountry.SelectedValue = dr("CneeCountryKey").ToString()
                txtRwbasketPostCode.Text = dr("CneePostCode").ToString()

            End If


        End If

    End Sub

    Protected Sub PopulateRecipients()

        Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("FF_GetAllRecipients")
        If dt IsNot Nothing And dt.Rows.Count <> 0 Then

            ddlRwBasketShortcut.DataSource = dt
            ddlRwBasketShortcut.DataTextField = "shortcut"
            ddlRwBasketShortcut.DataValueField = "id"
            ddlRwBasketShortcut.DataBind()

        End If

        ddlRwBasketShortcut.Items.Insert(0, New RadComboBoxItem("- please select -", "0"))



    End Sub

    Protected Sub PopulateCustomerContact(ByVal nCustomerKey As Int32)
        If nCustomerKey > 0 Then
            Dim nContactCount As Int32 = FF_CustomerContact.GetContactCount(nCustomerKey)
            tbContact.Visible = False
            rcbCustomerContact.Visible = False
            Select Case nContactCount
                Case 0
                    tbContact.Text = "no contacts found"
                    tbContact.Visible = True
                    lblCustomerContactTooltipData.Text = String.Empty
                    rttCustomerContact.Enabled = False
                Case 1
                    Dim ffContact As New FF_CustomerContact
                    ffContact.Load(FF_CustomerContact.GetFirstContactID(nCustomerKey))
                    tbContact.Text = ffContact.Name
                    tbContact.Visible = True
                    SetCustomerContactTooltipData(FF_CustomerContact.GetFirstContactID(nCustomerKey))
                    rttCustomerContact.TargetControlID = "tbContact"
                    rttCustomerContact.Enabled = True
                Case Else
                    rcbCustomerContact.Items.Clear()
                    Dim rcbi As New RadComboBoxItem("- please select -", 0)
                    rcbCustomerContact.Items.Add(rcbi)
                    Dim dt As DataTable = FF_CustomerContact.GetAllContacts(nCustomerKey)
                    For Each dr As DataRow In dt.Rows
                        If dr("name").ToString <> String.Empty Then
                            rcbCustomerContact.Items.Add(New RadComboBoxItem(dr("name"), dr("id")))
                        End If
                    Next
                    rcbCustomerContact.Visible = True
                    rttCustomerContact.TargetControlID = "rcbCustomerContact"
                    lblCustomerContactTooltipData.Text = String.Empty
                    rttCustomerContact.Enabled = True
                    ' NEED TO COPE WITH CONTACT BEING REMOVED BUT STILL REFERENCED IN JOB
            End Select
            divCustomerContact.Visible = True
        Else
            divCustomerContact.Visible = False
        End If
    End Sub

    Protected Sub SetCustomerContactTooltipData(ByVal nCustomerContactKey As Int32)
        Dim ffCustomerContact As New FF_CustomerContact(nCustomerContactKey)
        Dim sbDescription As New StringBuilder
        sbDescription.Append("<table>")

        sbDescription.Append("<tr>")
        sbDescription.Append("<td>")
        sbDescription.Append("Name")
        sbDescription.Append("</td>")
        sbDescription.Append("<td>")
        sbDescription.Append(ffCustomerContact.Name)
        sbDescription.Append("</td>")
        sbDescription.Append("</tr>")

        If ffCustomerContact.Telephone.Trim <> String.Empty Then
            sbDescription.Append("<td>")
            sbDescription.Append("Telephone")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomerContact.Telephone)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")
        End If

        If ffCustomerContact.Mobile.Trim <> String.Empty Then
            sbDescription.Append("<td>")
            sbDescription.Append("Mobile")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomerContact.Mobile)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")
        End If

        If ffCustomerContact.Notes.Trim <> String.Empty Then
            sbDescription.Append("<td>")
            sbDescription.Append("Notes")
            sbDescription.Append("</td>")
            sbDescription.Append("<td>")
            sbDescription.Append(ffCustomerContact.Notes)
            sbDescription.Append("</td>")
            sbDescription.Append("</tr>")
        End If

        sbDescription.Append("</table>")
        lblCustomerContactTooltipData.Text = sbDescription.ToString
    End Sub

    Protected Sub rcbCustomerContact_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles rcbCustomerContact.SelectedIndexChanged
        Dim rcb As RadComboBox = o
        If rcb.SelectedIndex > 0 Then
            rttCustomerContact.Enabled = True
            Call SetCustomerContactTooltipData(rcb.SelectedValue)
        Else
            lblCustomerContactTooltipData.Text = String.Empty
            rttCustomerContact.Enabled = False
        End If

        Call HideAllRadWindows()
    End Sub

    Protected Sub btnBasket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnShowBasket.Click
        rwBasket.Visible = True
        ShowOneRadWindow(rwBasket)
        Call FillBasket("ProductCode")
    End Sub

    Protected Sub btnRefreshBasket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefreshBasket.Click
        Call FillBasket("ProductCode")
    End Sub

    Protected Sub CreateBasketDataTable()
        gdtBasket = Nothing
        gdtBasket = New DataTable()
        gdtBasket.Columns.Add(New DataColumn("LogisticProductKey", GetType(String)))
        gdtBasket.Columns.Add(New DataColumn("ProductCode", GetType(String)))
        'gdtBasket.Columns.Add(New DataColumn("ProductDate", GetType(String)))
        gdtBasket.Columns.Add(New DataColumn("ProductDescription", GetType(String)))
        gdtBasket.Columns.Add(New DataColumn("QtyAvailable", GetType(Int32)))
        gdtBasket.Columns.Add(New DataColumn("QtyToPick", GetType(Int32)))
    End Sub

    Protected Sub LoadBasketDataTable()
        Dim dr As DataRow
        Dim rntb As RadNumericTextBox
        Call CreateBasketDataTable()
        'gdtBasket.Rows.Clear()
        Dim hid As HiddenField = Nothing
        For Each gvr As GridViewRow In gvStockItems.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                rntb = gvr.FindControl("rntbPickQty")
                If IsNumeric(rntb.Value) AndAlso rntb.Value > 0 Then
                    hid = gvr.FindControl("hidLogisticProductkey")
                    rntb = gvr.FindControl("rntbPickQty")
                    dr = gdtBasket.NewRow()
                    dr("LogisticProductKey") = hid.Value
                    dr("ProductCode") = gvr.Cells(0).Text & ""
                    dr("ProductDescription") = gvr.Cells(1).Text & ""
                    dr("QtyAvailable") = gvr.Cells(2).Text & ""
                    dr("QtyToPick") = rntb.Value
                    gdtBasket.Rows.Add(dr)
                End If
            End If
        Next
    End Sub

    Protected Sub FillBasket(ByVal SortField As String)
        Call LoadBasketDataTable()
        If gdtBasket IsNot Nothing And gdtBasket.Rows.Count <> 0 Then
            tblPickRecipients.Visible = True
            Call PopulateRecipients()
        Else
            tblPickRecipients.Visible = False
        End If

        gvBasket.DataSource = gdtBasket
        gvBasket.DataBind()
        gvBasket.Visible = "True"

    End Sub

    Protected Sub PopulateCustomerObjectFromForm()
        gffCustomer.CustomerCode = tbCustomerCode.Text
        gffCustomer.CustomerName = tbCustomerName.Text
        gffCustomer.CustomerAddr1 = tbAddr1.Text
        gffCustomer.CustomerAddr2 = tbAddr2.Text
        gffCustomer.CustomerAddr3 = tbAddr3.Text
        gffCustomer.CustomerTown = tbTown.Text
        gffCustomer.CustomerPostcode = tbPostcode.Text
        If trCountry.Visible = False Then
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
    Protected Sub InitContactFieldsFromContactObject()
        tbContactName.Text = gffCustomerContact.Name
        tbContactTelephone.Text = gffCustomerContact.Telephone
        tbContactMobile.Text = gffCustomerContact.Mobile
        tbContactEmailAddr.Text = gffCustomerContact.EmailAddr
        tbContactNotes.Text = gffCustomerContact.Notes
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
    Protected Sub btnUpdateCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdateCustomer.Click
        Call UpdateCustomer()
        Call ClearRWAddCustomerControls()
        rwCustomer.Visible = False
    End Sub

    Protected Sub UpdateCustomer()
        tbCustomerCode.Text = tbCustomerCode.Text.Trim
        Dim ffAudit As New FF_AuditTrail()
        Dim ffOldCustomerContact As New FF_CustomerContact()
        'If psMode.Contains(FF_GLOBALS.MODE_CREATE) Then
        If rcbCustomer.SelectedValue = 0 Then
            If FF_Customer.IsUniqueCustomerCode(tbCustomerCode.Text) Then
                gffCustomer = New FF_Customer
                'pnCustomerID = gffCustomer.Add()
                Call PopulateCustomerObjectFromForm()
                gffCustomer.IsActive = True
                pnCustomerID = gffCustomer.Add()
                '''''''''''' I am making bhand here  hope not creating a problem ''''''''''''''''''''''
                gffCustomer.id = pnCustomerID
                ffAudit.CustomerAudit(gffCustomer, FF_GLOBALS.DB_INSERT)
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
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
                        trMultipleContacts.Visible = True
                    End If
                End If
                psMode = FF_GLOBALS.MODE_EDIT
            Else
                WebMsgBox.Show("There is already a customer with this customer code. The customer code must be unique.")
            End If
            'Call NavigateTo(FF_GLOBALS.PAGE_OPTIONS)
        Else
            pnCustomerID = FF_Customer.GetRCBICustomerLocalKey(rcbCustomer.SelectedItem)
            gffCustomer = New FF_Customer(pnCustomerID)
            Dim oldffCustomer As FF_Customer    'for audit trial 
            oldffCustomer = gffCustomer

            Call PopulateCustomerObjectFromForm()
            If FF_Customer.IsUniqueCustomerCode(tbCustomerCode.Text, pnCustomerID) Then
                gffCustomer.Update(pnCustomerID)
                ffAudit.CustomerAudit(gffCustomer, FF_GLOBALS.DB_UPDATE, oldffCustomer)
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
                        gffCustomerContact.id = pnContactID
                        ffAudit.ContactAudit(gffCustomerContact, FF_GLOBALS.DB_INSERT)
                    Else
                        ffOldCustomerContact = New FF_CustomerContact(pnContactID)
                        ffAudit.ContactAudit(gffCustomerContact, FF_GLOBALS.DB_UPDATE, ffOldCustomerContact)
                        gffCustomerContact.Update(pnContactID)
                    End If
                    btnAddAnotherContact.Visible = True
                    If FF_CustomerContact.GetAllContactIDs(pnCustomerID).Length > 1 Then
                        trMultipleContacts.Visible = True
                    End If
                End If
            Else
                WebMsgBox.Show("There is already a customer with this customer code. The customer code must be unique.")
            End If
            'Page.regis
        End If
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "closewindow", "Close();", True)
        Call FF_Customer.PopulateCustomerDropdown(rcbCustomer)
        Call PopulateCustomerContact(pnCustomerID)
        Call SetCustomerTooltipData(pnCustomerID)
        If pnContactID > 0 Then
            divMaterialsFromStock_Outer.Visible = True
        Else
            divMaterialsFromStock_Outer.Visible = False
        End If

        rcbCustomer.SelectedValue = pnCustomerID

    End Sub
    Protected Sub ClearRWAddCustomerControls()

        tbCustomerCode.Text = String.Empty
        tbCustomerName.Text = String.Empty
        tbAddr1.Text = String.Empty
        tbAddr2.Text = String.Empty
        tbAddr3.Text = String.Empty
        tbPostcode.Text = String.Empty
        'tbContact.Text = String.Empty
        tbContactEmailAddr.Text = String.Empty
        tbContactMobile.Text = String.Empty
        tbContactName.Text = String.Empty
        tbContactNotes.Text = String.Empty
        tbContactTelephone.Text = String.Empty
        tbTown.Text = String.Empty
        trMultipleContacts.Visible = False
        btnAddAnotherContact.Visible = False
        trCountry.Visible = False


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

    Protected Function GetSprintAccountHandlerKey() As Int32
        If FF_SprintDBMode.GetDebugMode Then
            GetSprintAccountHandlerKey = SprintDB_Test.Query("SELECT TOP 1 * FROM UserProfile WHERE Status = 'Active' And CustomerKey = " & FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem) & " AND UserPermissions & 1 = 1").Rows(0)(0)
        Else
            GetSprintAccountHandlerKey = SprintDB.Query("SELECT TOP 1 * FROM UserProfile WHERE Status = 'Active' And  CustomerKey = " & FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem) & " AND UserPermissions & 1 = 1").Rows(0)(0)
        End If

    End Function


    '!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    ' NEED TO CHECK ACCOUNT HANDLER IS SELECTED AS AH iS RECIPIENT OF PICK !!!!!!!!!!!!!!!!!!!!!!!!!!!!
    '!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    Protected Function PickItems() As Int32
        PickItems = 0
        Dim nBookingKey As Int32
        Dim nConsignmentKey As Int32
        Dim BookingFailed As Boolean
        Dim oConn As New SqlConnection
        Dim nSprintAccountHandlerKey As Int32 = GetSprintAccountHandlerKey()
        If FF_SprintDBMode.GetDebugMode Then
            oConn.ConnectionString = SprintDB_Test.GetStockSystemConnectionString()
        Else
            oConn.ConnectionString = SprintDB.GetStockSystemConnectionString()
        End If

        Dim oTrans As SqlTransaction
        Dim oCmdAddBooking As SqlCommand = New SqlCommand("spASPNET_StockBooking_Add3", oConn)
        oCmdAddBooking.CommandType = CommandType.StoredProcedure

        Dim param1 As SqlParameter = New SqlParameter("@UserProfileKey", SqlDbType.Int, 4)
        param1.Value = nSprintAccountHandlerKey
        oCmdAddBooking.Parameters.Add(param1)

        Dim param2 As SqlParameter = New SqlParameter("@CustomerKey", SqlDbType.Int, 4)
        param2.Value = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
        oCmdAddBooking.Parameters.Add(param2)

        Dim param2a As SqlParameter = New SqlParameter("@BookingOrigin", SqlDbType.NVarChar, 20)
        param2a.Value = "WEB_BOOKING"
        oCmdAddBooking.Parameters.Add(param2a)

        Dim param3 As SqlParameter = New SqlParameter("@BookingReference1", SqlDbType.NVarChar, 25)
        param3.Value = "Fulfilment Job # " & pnJobID.ToString
        oCmdAddBooking.Parameters.Add(param3)

        Dim param4 As SqlParameter = New SqlParameter("@BookingReference2", SqlDbType.NVarChar, 25)
        param4.Value = tbJobName.Text.Trim()
        oCmdAddBooking.Parameters.Add(param4)

        Dim param5 As SqlParameter = New SqlParameter("@BookingReference3", SqlDbType.NVarChar, 50)
        param5.Value = txtRwBasketAIMSCustRef1.Text.Trim()
        oCmdAddBooking.Parameters.Add(param5)

        Dim param6 As SqlParameter = New SqlParameter("@BookingReference4", SqlDbType.NVarChar, 50)
        param6.Value = txtRwBasketAIMSCustRef2.Text.Trim()
        oCmdAddBooking.Parameters.Add(param6)

        Dim param6a As SqlParameter = New SqlParameter("@ExternalReference", SqlDbType.NVarChar, 50)
        param6a.Value = GetJobGuid()
        oCmdAddBooking.Parameters.Add(param6a)

        Dim param7 As SqlParameter = New SqlParameter("@SpecialInstructions", SqlDbType.NVarChar, 1000)
        param7.Value = tbSpecialInstructions.Text
        oCmdAddBooking.Parameters.Add(param7)

        Dim param8 As SqlParameter = New SqlParameter("@PackingNoteInfo", SqlDbType.NVarChar, 1000)
        param8.Value = String.Empty
        oCmdAddBooking.Parameters.Add(param8)

        Dim param9 As SqlParameter = New SqlParameter("@ConsignmentType", SqlDbType.NVarChar, 20)
        param9.Value = "STOCK ITEM"
        oCmdAddBooking.Parameters.Add(param9)

        Dim param10 As SqlParameter = New SqlParameter("@ServiceLevelKey", SqlDbType.Int, 4)
        param10.Value = -1
        oCmdAddBooking.Parameters.Add(param10)
        Dim param11 As SqlParameter = New SqlParameter("@Description", SqlDbType.NVarChar, 250)
        param11.Value = "PRINTED MATTER - FREE DOMICILE"
        oCmdAddBooking.Parameters.Add(param11)

        Dim param13 As SqlParameter = New SqlParameter("@CnorName", SqlDbType.NVarChar, 50)
        param13.Value = "Sprint International"
        oCmdAddBooking.Parameters.Add(param13)

        Dim param14 As SqlParameter = New SqlParameter("@CnorAddr1", SqlDbType.NVarChar, 50)
        param14.Value = "UNIT 3 MERCURY CENTRE"
        oCmdAddBooking.Parameters.Add(param14)

        Dim param15 As SqlParameter = New SqlParameter("@CnorAddr2", SqlDbType.NVarChar, 50)
        param15.Value = "CENTRAL WAY"
        oCmdAddBooking.Parameters.Add(param15)

        Dim param16 As SqlParameter = New SqlParameter("@CnorAddr3", SqlDbType.NVarChar, 50)
        param16.Value = ""
        oCmdAddBooking.Parameters.Add(param16)

        Dim param17 As SqlParameter = New SqlParameter("@CnorTown", SqlDbType.NVarChar, 50)
        param17.Value = "FELTHAM"
        oCmdAddBooking.Parameters.Add(param17)

        Dim param18 As SqlParameter = New SqlParameter("@CnorState", SqlDbType.NVarChar, 50)
        param18.Value = "MIDDLESEX"
        oCmdAddBooking.Parameters.Add(param18)

        Dim param19 As SqlParameter = New SqlParameter("@CnorPostCode", SqlDbType.NVarChar, 50)
        param19.Value = "TW14 0RN"
        oCmdAddBooking.Parameters.Add(param19)

        Dim param20 As SqlParameter = New SqlParameter("@CnorCountryKey", SqlDbType.Int, 4)
        param20.Value = 222
        oCmdAddBooking.Parameters.Add(param20)

        Dim param21 As SqlParameter = New SqlParameter("@CnorCtcName", SqlDbType.NVarChar, 50)
        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetCurrentUserInfo
        param21.Value = uiCurrentUser.DisplayName
        oCmdAddBooking.Parameters.Add(param21)

        Dim param22 As SqlParameter = New SqlParameter("@CnorTel", SqlDbType.NVarChar, 50)
        param22.Value = ""
        oCmdAddBooking.Parameters.Add(param22)

        Dim param23 As SqlParameter = New SqlParameter("@CnorEmail", SqlDbType.NVarChar, 50)
        param23.Value = ""
        oCmdAddBooking.Parameters.Add(param23)

        Dim param24 As SqlParameter = New SqlParameter("@CnorPreAlertFlag", SqlDbType.Bit)
        param24.Value = 0
        oCmdAddBooking.Parameters.Add(param24)

        Dim param25 As SqlParameter = New SqlParameter("@CneeName", SqlDbType.NVarChar, 50)
        param25.Value = txtRwBasketName.Text.Trim()
        oCmdAddBooking.Parameters.Add(param25)

        Dim param26 As SqlParameter = New SqlParameter("@CneeAddr1", SqlDbType.NVarChar, 50)
        param26.Value = txtRwBasketAddr1.Text.Trim()
        oCmdAddBooking.Parameters.Add(param26)

        Dim param27 As SqlParameter = New SqlParameter("@CneeAddr2", SqlDbType.NVarChar, 50)
        param27.Value = txtRwBasketAddr2.Text.Trim()
        oCmdAddBooking.Parameters.Add(param27)

        Dim param28 As SqlParameter = New SqlParameter("@CneeAddr3", SqlDbType.NVarChar, 50)
        param28.Value = ""
        oCmdAddBooking.Parameters.Add(param28)

        Dim param29 As SqlParameter = New SqlParameter("@CneeTown", SqlDbType.NVarChar, 50)
        param29.Value = txtRwBasketTown.Text.Trim()
        oCmdAddBooking.Parameters.Add(param29)

        Dim param30 As SqlParameter = New SqlParameter("@CneeState", SqlDbType.NVarChar, 50)
        param30.Value = txtRwBasketState.Text.Trim()
        oCmdAddBooking.Parameters.Add(param30)

        Dim param31 As SqlParameter = New SqlParameter("@CneePostCode", SqlDbType.NVarChar, 50)
        param31.Value = txtRwbasketPostCode.Text.Trim()
        oCmdAddBooking.Parameters.Add(param31)

        Dim param32 As SqlParameter = New SqlParameter("@CneeCountryKey", SqlDbType.Int, 4)
        If ddlRwBasketCountry.SelectedItem Is Nothing Or ddlRwBasketCountry.SelectedValue = "0" Then
            param32.Value = 222
            oCmdAddBooking.Parameters.Add(param32)
        Else
            param32.Value = Convert.ToInt64(ddlRwBasketCountry.SelectedValue)
            oCmdAddBooking.Parameters.Add(param32)
        End If

        Dim param33 As SqlParameter = New SqlParameter("@CneeCtcName", SqlDbType.NVarChar, 50)
        'param33.Value = (DNN.GetUserInfo(Me, rcbAccountHandler.SelectedValue)).DisplayName
        param33.Value = ddlRwBasketShortcut.SelectedItem.Text()
        oCmdAddBooking.Parameters.Add(param33)

        Dim param34 As SqlParameter = New SqlParameter("@CneeTel", SqlDbType.NVarChar, 50)
        param34.Value = ""
        oCmdAddBooking.Parameters.Add(param34)
        Dim param35 As SqlParameter = New SqlParameter("@CneeEmail", SqlDbType.NVarChar, 50)
        param35.Value = ""
        oCmdAddBooking.Parameters.Add(param35)
        Dim param36 As SqlParameter = New SqlParameter("@CneePreAlertFlag", SqlDbType.Bit)
        param36.Value = 0
        oCmdAddBooking.Parameters.Add(param36)
        Dim param37 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
        param37.Direction = ParameterDirection.Output
        oCmdAddBooking.Parameters.Add(param37)
        Dim param38 As SqlParameter = New SqlParameter("@ConsignmentKey", SqlDbType.Int, 4)
        param38.Direction = ParameterDirection.Output
        oCmdAddBooking.Parameters.Add(param38)
        Try
            BookingFailed = False
            oConn.Open()
            oTrans = oConn.BeginTransaction(IsolationLevel.ReadCommitted, "AddBooking")
            oCmdAddBooking.Connection = oConn
            oCmdAddBooking.Transaction = oTrans
            oCmdAddBooking.ExecuteNonQuery()
            nBookingKey = CLng(oCmdAddBooking.Parameters("@LogisticBookingKey").Value.ToString)
            nConsignmentKey = CInt(oCmdAddBooking.Parameters("@ConsignmentKey").Value.ToString)
            If nBookingKey > 0 Then
                Call LoadBasketDataTable()
                For Each dr As DataRow In gdtBasket.Rows
                    Dim nProductKey As Int32 = dr("LogisticProductKey")
                    Dim nPickQuantity As Int32 = dr("QtyToPick")
                    Dim oCmdAddStockItem As SqlCommand = New SqlCommand("spASPNET_LogisticMovement_Add", oConn)
                    oCmdAddStockItem.CommandType = CommandType.StoredProcedure

                    Dim param51 As SqlParameter = New SqlParameter("@UserKey", SqlDbType.Int, 4)
                    param51.Value = nSprintAccountHandlerKey
                    oCmdAddStockItem.Parameters.Add(param51)

                    Dim param52 As SqlParameter = New SqlParameter("@CustomerKey", SqlDbType.Int, 4)
                    param52.Value = CLng(Session("CustomerKey"))
                    oCmdAddStockItem.Parameters.Add(param52)

                    Dim param53 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
                    param53.Value = nBookingKey
                    oCmdAddStockItem.Parameters.Add(param53)

                    Dim param54 As SqlParameter = New SqlParameter("@LogisticProductKey", SqlDbType.Int, 4)
                    param54.Value = nProductKey
                    oCmdAddStockItem.Parameters.Add(param54)

                    Dim param55 As SqlParameter = New SqlParameter("@LogisticMovementStateId", SqlDbType.NVarChar, 20)
                    param55.Value = "PENDING"
                    oCmdAddStockItem.Parameters.Add(param55)

                    Dim param56 As SqlParameter = New SqlParameter("@ItemsOut", SqlDbType.Int, 4)
                    param56.Value = nPickQuantity
                    oCmdAddStockItem.Parameters.Add(param56)

                    Dim param57 As SqlParameter = New SqlParameter("@ConsignmentKey", SqlDbType.Int, 8)
                    param57.Value = nConsignmentKey
                    oCmdAddStockItem.Parameters.Add(param57)

                    oCmdAddStockItem.Connection = oConn
                    oCmdAddStockItem.Transaction = oTrans
                    oCmdAddStockItem.ExecuteNonQuery()
                Next
                Dim oCmdCompleteBooking As SqlCommand = New SqlCommand("spASPNET_LogisticBooking_Complete", oConn)
                oCmdCompleteBooking.CommandType = CommandType.StoredProcedure

                Dim param71 As SqlParameter = New SqlParameter("@LogisticBookingKey", SqlDbType.Int, 4)
                param71.Value = nBookingKey

                oCmdCompleteBooking.Parameters.Add(param71)
                oCmdCompleteBooking.Connection = oConn
                oCmdCompleteBooking.Transaction = oTrans
                oCmdCompleteBooking.ExecuteNonQuery()
            Else
                BookingFailed = True
                WebMsgBox.Show("Booking key = 0")
            End If
            If Not BookingFailed Then
                oTrans.Commit()
                PickItems = nConsignmentKey
                ConsignmentEventLog(nConsignmentKey)
            Else
                oTrans.Rollback("AddBooking")
            End If
        Catch ex As SqlException
            WebMsgBox.Show("Failed to submit order: " & ex.ToString)
            oTrans.Rollback("AddBooking")
        Finally
            oConn.Close()
        End Try
    End Function

    Protected Sub ConsignmentEventLog(ByVal nConsignmentKey As Integer)

        Dim ffConsignment As New FFDataLayer.FF_Consignment
        ffConsignment.JobNo = pnJobID
        ffConsignment.ConsignmentNo = nConsignmentKey
        ffConsignment.CreatedOn = DateTime.Now
        ffConsignment.CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        ffConsignment.Description = "Consingnment " + nConsignmentKey.ToString() + " Picked By " + DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username + " for Job No " + pnJobID.ToString()
        dbContext.Add(ffConsignment)
        dbContext.SaveChanges()

    End Sub

    Protected Sub btnPickStockItems_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPickStockItems.Click
        Call PickStockItems()
    End Sub

    Protected Sub PickStockItems()
        Call LoadBasketDataTable()
        Dim nBasketGridViewRowCount As Int32 = gvBasket.Rows.Count
        If nBasketGridViewRowCount <> gdtBasket.Rows.Count Then
            WebMsgBox.Show("The number of items with non-zero quantity (" & gdtBasket.Rows.Count & ") is different from the number of items shown in the basket (" & nBasketGridViewRowCount.ToString & "). Please refresh your basket before picking these items.")
            Exit Sub
        End If
        If GetSprintAccountHandlerKey() = 0 Then
            'WebMsgBox.Show("Please select account handler before picking items - the Account Handler is the recipient of these items.")
            WebMsgBox.Show("Account handler for this account could not be determined - cannot proceed with pick.")
            Exit Sub
        End If
        Dim nBasketCount As Int32 = gvBasket.Rows.Count
        If nBasketCount = 0 Then
            WebMsgBox.Show("No items selected for picking.")
            Exit Sub
        End If
        Dim nConsignmentCode As Int32 = PickItems()
        If nConsignmentCode > 0 Then
            For Each dr As DataRow In gdtBasket.Rows
                Dim nProductKey As Int32 = dr("LogisticProductKey")
                Dim nPickQuantity As Int32 = dr("QtyToPick")
                Dim ffBasket As New FF_Basket
                ffBasket.LogisticProductKey = nProductKey
                ffBasket.Qty = nPickQuantity
                ffBasket.IsPicked = True
                ffBasket.JobId = pnJobID
                ffBasket.CustomerKey = FF_Customer.GetRCBICustomerExternalKey(rcbCustomer.SelectedItem)
                ffBasket.PickDateTime = DateTime.Now
                ffBasket.Add()
            Next
            WebMsgBox.Show(nBasketCount.ToString & " items successfully picked. The consignment number is " & nConsignmentCode.ToString & ".")

        Else
            WebMsgBox.Show("Some of all of the items could not be picked.")
        End If
        gvBasket.DataSource = Nothing
        gvBasket.DataBind()
        tblPickRecipients.Visible = False

        Dim rntb As RadNumericTextBox
        For Each gvr As GridViewRow In gvStockItems.Rows
            If gvr.RowType = DataControlRowType.DataRow Then
                rntb = gvr.FindControl("rntbPickQty")
                If rntb IsNot Nothing Then
                    ' ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
                    ' rntb.Value = "" ' CRASHED HERE AFTER TRYING TO PICK WHEN JOB HAD NOT BEEN SAVED AND ACCOUNT HANDLER ONLY JUST SELECTED
                    ' need to handle rntb
                    rntb.Value = 0 ' TEMPORARILY
                End If
            End If
        Next
        tbSpecialInstructions.Text = String.Empty
    End Sub
    Sub LoadUsersInStages(ByVal jobID As Integer)
        ''Dim dtUsersInStages As DataTable = FF_UserJobMapping.FetchUserJobMapping(pnJobID) '''' users in stages
        'gvUserJobMapping.DataSource = dtUsersInStages
        'gvUserJobMapping.DataBind()
        'gvHelper.SetSuppressGroup("jobstatename")
        'gvHelper.RegisterGroup("jobstatename", True, True)
        'CustomizeGroupByExpression()
    End Sub
    Protected Sub gvStockItems_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStockItems.RowDataBound
        If pbIsInitialisingFromPersistedBasket Then
            Dim gvr As GridViewRow = e.Row
            If gvr.RowType = DataControlRowType.DataRow Then
                Dim rntb As RadNumericTextBox
                Dim hid As HiddenField
                hid = gvr.FindControl("hidLogisticProductkey")
                rntb = gvr.FindControl("rntbPickQty")
                Dim dv As New DataView(gdtBasket)
                dv.Sort = "LogisticProductKey"
                Dim nIndex As Int32 = dv.Find(hid.Value)
                If nIndex >= 0 Then
                    'Dim sQtyToPick As String = dv(nIndex)("QtyToPick")
                    'If IsNumeric(sQtyToPick) AndAlso CInt(sQtyToPick) > 0 Then
                    rntb.Value = dv(nIndex)("QtyToPick")
                    'End If
                End If
            End If
        End If
    End Sub

    'Protected Sub btnJobEventLogCloseWindow_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnJobEventLogCloseWindow.Click
    '    rwJobEventLog.Visible = False
    'End Sub

    Protected Sub PopulateStockSystemCustomerDropdown()
        Dim sSQL As String
        Dim oDT As DataTable
        sSQL = "SELECT CustomerAccountCode, CustomerName, CustomerKey FROM Customer WHERE DeletedFlag = 'N' AND CustomerStatusId = 'ACTIVE' AND ISNULL(AccountHandlerKey,0) > 0 ORDER BY CustomerAccountCode"
        If FF_SprintDBMode.GetDebugMode Then
            oDT = SprintDB_Test.Query(sSQL)
        Else
            oDT = SprintDB.Query(sSQL)
        End If
        RadComboBoxStockSystemCustomer.Items.Clear()
        RadComboBoxStockSystemCustomer.Items.Add(New RadComboBoxItem("- please select -", 0))
        For Each dr As DataRow In oDT.Rows
            RadComboBoxStockSystemCustomer.Items.Add(New RadComboBoxItem(dr(0) & " - " & dr(1), dr(2)))
        Next
    End Sub

    Protected Sub RadComboBoxStockSystemCustomer_SelectedIndexChanged(ByVal o As Object, ByVal e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs) Handles RadComboBoxStockSystemCustomer.SelectedIndexChanged
        Dim rcb As RadComboBox = o
        Dim dr As DataRow
        If rcb.SelectedValue > 0 Then
            Dim sSQL As String = "SELECT CustomerKey, CustomerAccountCode, CustomerName, CustomerAddr1, ISNULL(CustomerAddr2,'') 'CustomerAddr2', ISNULL(CustomerAddr3,'') 'CustomerAddr3', CustomerTown, ISNULL(CustomerPostCode,'') 'CustomerPostCode', CustomerCountryKey, ISNULL(DefaultContact,'') 'DefaultContact', ISNULL(DefaultTelephone,'') 'DefaultTelephone', ISNULL(DefaultEmail,'') 'DefaultEmail', AccountHandlerKey FROM Customer WHERE CustomerKey = " & rcb.SelectedValue
            If FF_SprintDBMode.GetDebugMode Then
                dr = SprintDB_Test.Query(sSQL).Rows(0)
            Else
                dr = SprintDB.Query(sSQL).Rows(0)
            End If

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
        tblImportCustomer.Visible = False
        tblAddCustomer.Visible = True

    End Sub
    Protected Sub lnkbtnCreateTemplate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        trCountry.Visible = True
        lnkbtnNonUKAddress.Visible = False
        Call FF_GLOBALS.PopulateComboboxCountry(ddlCountry)
    End Sub

    Protected Sub btnAddAnotherContact_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddAnotherContact.Click
        Call AddAnotherContact()
    End Sub

    Protected Sub ShowOneRadWindow(ByRef rw As RadWindow)

        If rw Is rwCustomer Then
            rwBasket.Visible = False
            rwJobEventLog.Visible = False
        ElseIf rw Is rwBasket Then
            rwCustomer.Visible = False
            rwJobEventLog.Visible = False
        ElseIf rw Is rwJobEventLog Then
            rwCustomer.Visible = False
            rwBasket.Visible = False
        End If

    End Sub

    Protected Sub lnkbtnViewJobEventLog_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnViewJobEventLog.Click
        gvJobEventLog.DataSource = FF_AuditTrail.GetEventsForJob(pnJobID)
        gvJobEventLog.DataBind()

        'rwCustomer.VisibleOnPageLoad = False
        'rwJobEventLog.VisibleOnPageLoad = True
        rwJobEventLog.Visible = True
        ShowOneRadWindow(rwJobEventLog)
    End Sub

    Protected Sub lbAddCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbAddCustomer.Click


        If rcbCustomer.SelectedValue = "0,0" Then
            btnAddAnotherContact.Visible = False
            trMultipleContacts.Visible = False
            tblImportCustomer.Visible = False
            tblAddCustomer.Visible = True
            tdbtnAddCustomer.Visible = False
            tdbtnImportCustomer.Visible = True
            tbCustomerCode.Focus()
            rwCustomer.Title = "Create Customer"
            btnUpdateCustomer.Text = "Create Customer"

            Call ClearRWAddCustomerControls()
            'rwCustomer.Height = "500"
            'rwCustomer.Width = "510"
            'rwCustomer.Left = "150px"
            'rwCustomer. 
            'rwCustomer.VisibleOnPageLoad = True
            rwCustomer.Visible = True
            ShowOneRadWindow(rwCustomer)
            'Call ShowCustomerWindow()


        Else
            Call ClearRWAddCustomerControls()
            pnCustomerID = FF_Customer.GetRCBICustomerLocalKey(rcbCustomer.SelectedItem)
            'PopulateCustomerContact()
            If rcbCustomerContact.SelectedValue <> String.Empty Then
                pnContactID = Convert.ToInt64(rcbCustomerContact.SelectedValue)
            End If
            'f()
            gffCustomer = New FF_Customer(pnCustomerID)
            PopulateFormFromCustomerObject()
            If gnContacts.Length > 1 Then
                trMultipleContacts.Visible = True
            End If

            If gnContacts.Length > 0 Then
                btnAddAnotherContact.Visible = True
            End If

            tblImportCustomer.Visible = False
            tblAddCustomer.Visible = True
            tdbtnAddCustomer.Visible = False
            tdbtnImportCustomer.Visible = True


            rwCustomer.Title = "Update Customer"
            btnUpdateCustomer.Text = "Update Customer"

            'rwCustomer.VisibleOnPageLoad = True
            rwCustomer.Visible = True
            ShowOneRadWindow(rwCustomer)
            'Call ShowCustomerWindow()
            'rwCustomer.VisibleOnPageLoad = True


        End If






    End Sub

    Protected Sub btnImportCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImportCustomer.Click

        tblAddCustomer.Visible = False
        tblImportCustomer.Visible = True
        tdbtnAddCustomer.Visible = True
        tdbtnImportCustomer.Visible = False
        rwCustomer.Title = "Import Customer"
        Call PopulateStockSystemCustomerDropdown()
        RadComboBoxStockSystemCustomer.Focus()

    End Sub

    Protected Sub btnAddCustomer_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCustomer.Click

        tblAddCustomer.Visible = True
        tblImportCustomer.Visible = False
        rwCustomer.Title = "Add Customer"
        tdbtnImportCustomer.Visible = True
        tdbtnAddCustomer.Visible = False
        ClearRWAddCustomerControls()
        'Call PopulateStockSystemCustomerDropdown()
        tbCustomerCode.Focus()

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
            trAdd3.Visible = True
            lnkbtnAddAddr3.Visible = False
        End If
        tbTown.Text = gffCustomer.CustomerTown
        tbPostcode.Text = gffCustomer.CustomerPostcode
        If gffCustomer.CustomerCountryKey = FF_GLOBALS.COUNTRY_KEY_UK Then
            trCountry.Visible = False
        Else
            trCountry.Visible = True
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

            If rcbCustomerContact.SelectedValue = String.Empty Or rcbCustomerContact.SelectedValue = "0" Then
                pnContactID = gnContacts(0)
                gffCustomerContact = New FF_CustomerContact(pnContactID)
            ElseIf rcbCustomerContact.SelectedValue <> String.Empty Then
                pnContactID = Convert.ToInt64(rcbCustomerContact.SelectedValue)
                gffCustomerContact = New FF_CustomerContact(pnContactID)
            End If

            'gffCustomerContact = New FF_CustomerContact(pnContactID)


            Call InitContactFieldsFromContactObject()
        End If
    End Sub

    Protected Sub lnkbtnAddAddr3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnAddAddr3.Click
        trAdd3.Visible = True
        lnkbtnAddAddr3.Visible = False
    End Sub



    Protected Sub lnkbtnCopyJobToClipboard_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnCopyJobToClipboard.Click
        'DotNetNuke.Services.Personalization.Personalization.SetProfile(FF_GLOBALS.CLIPBOARD_MODULE_ID, "Clipboard", pnJobID.ToString)
        'DotNetNuke.Services.Personalization.Personalization.SetProfile(DNN.GetModuleID(Me).ToString(), "Clipboard", pnJobID.ToString)
        'lblShowCopyJobToolTip.Text = "Job Copied To ClipBoard"
        Session.Add(FF_GLOBALS.COPY_JOB_TO_CLIPBOARD, pnJobID)
        'rttJobCopied.Show()
        ''''''''''' commented for testing
        'rapCreateEditJob.Alert("Job Copied To Clipboard")
        '''''''''''''''''''''''''''''''''''''''''
        'Dim sQueryParams(0) As String
        'sQueryParams(0) = "type=" + FF_GLOBALS.MODE_COPY
        'sQueryParams(0) = "template=" + FF_GLOBALS.MODE_COPY
        'sQueryParams(1) = DotNetNuke.Services.Personalization.Personalization.GetProfile(FF_GLOBALS.CLIPBOARD_MODULE_ID, "Clipboard")
        'Call NavigateTo(FF_GLOBALS.PAGE_CREATE_EDIT_JOB, sQueryParams)


        'WebMsgBox.Show("Job copied to clipboard.")
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
    End Sub

    Protected Function GetJobFilePath() As String
        GetJobFilePath = DNN.GetPMB(Me).PortalSettings.HomeDirectoryMapPath & FF_GLOBALS.JOB_FILES_PATH & pnJobID.ToString
    End Function

    Protected Function GetFileList() As DataTable

        Try

            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("Filename", GetType(String)))
            dt.Columns.Add(New DataColumn("Length", GetType(Int32)))
            dt.Columns.Add(New DataColumn("DateUploaded", GetType(DateTime)))

            Dim di As New DirectoryInfo(GetJobFilePath)
            Dim o As Object
            If di IsNot Nothing Then
                If di.EnumerateFiles() IsNot Nothing Then
                    o = di.EnumerateFiles
                    If o IsNot Nothing Then
                        For Each s As FileInfo In o
                            Dim sFileName As String = Path.GetFileName(s.FullName)
                            Dim dr As DataRow = dt.NewRow
                            dr("Filename") = sFileName
                            dr("Length") = s.Length
                            dr("DateUploaded") = s.CreationTime
                            dt.Rows.Add(dr)
                        Next
                    End If

                End If
            End If
            GetFileList = dt
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

    Protected Sub lnkbtnJobFileDownload_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkbtn As LinkButton = sender
        Dim sFilename As String = lnkbtn.CommandArgument
        'Dim sFilePath As String = GetJobFilePath() & "\" & sFilename
        Response.ContentType = "application/octet-stream"
        Response.AppendHeader("Content-Disposition", "attachment; filename=""" & sFilename & """")
        Dim sTransmitFilePath As String = "~\" & "Portals\" & DNN.GetPMB(Me).PortalId.ToString & "\" & FF_GLOBALS.JOB_FILES_PATH & pnJobID.ToString & "\" & sFilename
        Response.TransmitFile(Server.MapPath(sTransmitFilePath))
        Response.End()
    End Sub

    Protected Sub lnkbtnJobFileDelete_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim lnkbtn As LinkButton = sender
        Dim sFilename As String = GetJobFilePath() & "\" & lnkbtn.CommandArgument
        Dim fi As FileInfo = New FileInfo(sFilename)
        Try
            fi.Delete()
            WebMsgBox.Show("Deleted file " & lnkbtn.CommandArgument)
        Catch ex As Exception
            WebMsgBox.Show("Error deleting file " & lnkbtn.CommandArgument & ": " & ex.Message)
        End Try
        Call BindJobFiles()
    End Sub

    Protected Sub BindJobFiles()
        Dim dt As DataTable = GetFileList()
        gvJobFiles.DataSource = dt
        gvJobFiles.DataBind()
        If dt Is Nothing Then
            divDownload.Visible = False
            shDownloadJobFiles.Visible = False
        ElseIf dt IsNot Nothing And dt.Rows.Count = 0 Then
            divDownload.Visible = False
            shDownloadJobFiles.Visible = False
        Else
            divDownload.Visible = True
            shDownloadJobFiles.Visible = True
            shDownloadJobFiles.Text = "Download job files (" & dt.Rows.Count & ")"
            shJobFiles.Text = "Job files (" & dt.Rows.Count & ")"
        End If
    End Sub

    Protected Sub RadUpload_FileExists(ByVal sender As Object, ByVal e As Telerik.Web.UI.Upload.UploadedFileEventArgs) Handles RadUpload.FileExists
        WebMsgBox.Show("A file with the same name has already been uploaded.\n\nEither remove the uploaded file or rename the file you are trying to upload.")
    End Sub

    Protected Sub GetQuotations()

        Dim IQuotation = From Tariff In dbContext.FF_Tariffs
                         Where Tariff.JobID = pnJobID And
                         Tariff.RecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION And
                         Tariff.IsCreated = True And
                         Tariff.IsDeleted = False
                         Select Tariff

        rgQuotation.DataSource = IQuotation

    End Sub

    Protected Sub rgQuotation_ItemDataBound(ByVal source As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles rgQuotation.ItemDataBound

        If e.Item.ItemType = GridItemType.AlternatingItem Or e.Item.ItemType = GridItemType.Item Then

            Dim lblCreatedBy As Label = e.Item.FindControl("lblCreatedBy")
            Dim chkLockedDate As CheckBox = e.Item.FindControl("chkLockedDate")
            Dim lblLockedDate As Label = e.Item.FindControl("lblLockedDate")


            Dim nUserId As Integer = Convert.ToInt64(lblCreatedBy.Text)

            Dim userInfo As DotNetNuke.Entities.Users.UserInfo = DotNetNuke.Entities.Users.UserController.GetUserById(DNN.GetPMB(Me).PortalId, nUserId)

            lblCreatedBy.Text = userInfo.Username
            'chkLockedDate.Text = FF_GLOBALS.IsValidDate(chkLockedDate.Text)
            lblLockedDate.Text = FF_GLOBALS.IsValidDate(lblLockedDate.Text)

            If lblLockedDate.Text = String.Empty Then

                chkLockedDate.Checked = False
            Else

                chkLockedDate.Checked = True

            End If


        End If

    End Sub

    Protected Sub rgQuotation_ItemCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles rgQuotation.ItemCommand

        If e.CommandName = "InitInsert" Then

            Dim sQueryParams(0) As String
            sQueryParams(0) = "job=" & pnJobID
            Call NavigateTo(FF_GLOBALS.PAGE_ADD_QUOTATION, sQueryParams)

        End If

    End Sub

    Protected Sub chkLockedDate_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim chkLockedDate As CheckBox = sender

        Dim hidQuotationID As HiddenField = chkLockedDate.NamingContainer.FindControl("hidQuotationID")
        Dim nQuotationID As Int64 = hidQuotationID.Value
        ffQuotation = dbContext.GetObjectByKey(New Telerik.OpenAccess.ObjectKey(ffQuotation.GetType().Name, nQuotationID))

        If chkLockedDate.Checked Then

            ffQuotation.LockedDateTime = DateTime.Now

        Else

            ffQuotation.LockedDateTime = FF_GLOBALS.BASE_DATE

        End If

        dbContext.SaveChanges()
        rgQuotation.Rebind()

    End Sub

    Protected Sub rgQuotation_NeedDataSource(ByVal source As Object, ByVal e As Telerik.Web.UI.GridNeedDataSourceEventArgs) Handles rgQuotation.NeedDataSource

        Dim IQuotations = (From Tariff In dbContext.FF_Tariffs
                         Where Tariff.JobID = pnJobID And
                         Tariff.RecordType = FF_GLOBALS.RECORD_TYPE_QUOTATION And
                         Tariff.IsCreated = True And
                         Tariff.IsDeleted = False And
                         Tariff.JobID = pnJobID
                         Select Tariff).ToList


        rgQuotation.DataSource = IQuotations


    End Sub


End Class