Imports Telerik.Web.UI
Imports Microsoft.VisualBasic

Partial Public Class FF_AuditTrail

    Const AUDIT_EVENT As String = ""
    Const AUDIT_EVENT_JOB_CREATED As String = "Job Created"
    Const AUDIT_EVENT_JOB_UPDATED As String = "Job Updated"
    Const AUDIT_EVENT_JOB_DELETED As String = "Job Deleted"

    Const AUDIT_EVENT_JOB_STAGE_UPDATED As String = "Job Stage Updated"
    Const AUDIT_EVENT_JOB_STAGE_CREATED As String = "Job Stage Created"
    Const AUDIT_EVENT_JOB_STAGE_DELETED As String = "Job Stage Deleted"

    Const AUDIT_EVENT_CUSTOMER_CREATED As String = "Customer Created"
    Const AUDIT_EVENT_CUSTOMER_UPDATED As String = "Customer Updated"

    Const AUDIT_EVENT_CUSTOMER_CONTACT_CREATED As String = "Customer Contact Created"
    Const AUDIT_EVENT_CUSTOMER_CONTACT_UPDATED As String = "Customer Contact Updated"

    'Const AUDIT_EVENT_CUSTOMER_INSERTED As String = "Customer Inserted"



    Const AUDIT_EVENT_RESOURCE_MAPPING_UPDATED As String = "Resource Mapping Updated"

    Private gsNewLine As String = "<br />" & Environment.NewLine
    Private gsChangedFrom As String = " changed from "
    Private gsTo As String = " to "
    Private gsChangedTo As String = " changed to "
    Private gsDeletedBy As String = " deleted by "
    Private gsCreatedBy As String = " created by "

    Private sUserName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
    Private nUserId As Integer = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID

    'Private gffAuditTrail As FF_AuditTrail

    Public Sub New(ByVal ffJob As FF_Job, ByVal Myself As Control)
        Call Init()
        m_SourceId = ffJob.ID
        m_AuditEvent = AUDIT_EVENT_JOB_CREATED
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_JOB
        m_CreatedBy = DNN.GetPMB(Myself).UserId
        Call Add()
    End Sub

    Public Sub New(ByVal ffjob As FF_Job, ByVal operation As String)

        If operation = FF_GLOBALS.DB_DELETE Then

            Call Init()
            m_SourceId = ffjob.ID
            m_AuditEvent = AUDIT_EVENT_JOB_DELETED
            If ffjob.IsTemplate = True Then
                m_ChangeDetail = "Template" & gsDeletedBy & sUserName
            Else
                m_ChangeDetail = "Job" & gsDeletedBy & sUserName
            End If
            m_RecordType = FF_GLOBALS.AUDIT_TYPE_JOB
            m_CreatedBy = nUserId
            Call Add()

        End If

    End Sub

    Public Sub New(ByVal ffJobOld As FF_Job, ByVal ffJobNew As FF_Job, ByVal Myself As Control)
        Call CompareJobs(ffJobOld, ffJobNew, DNN.GetPMB(Myself).UserId)
    End Sub

    Public Sub New(ByVal ffJobStage As FF_JobState, ByVal Myself As Control)
        Call Init()
        m_SourceId = ffJobStage.id
        m_AuditEvent = AUDIT_EVENT_JOB_STAGE_UPDATED
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_JOB_STAGE
        m_ChangeDetail = "Stage " & ffJobStage.JobStateName & " completion status set to " & ffJobStage.IsCompleted.ToString
        m_CreatedBy = DNN.GetPMB(Myself).UserId
        Call Add()
    End Sub

    Public Sub New(ByVal ffJobStage As FF_JobState, ByVal operation As String)
        Call Init()
        m_SourceId = ffJobStage.id
        'Dim sUserName As String
        'Dim nUserId As Integer
        'sUserName = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
        'nUserId = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID

        If operation = FF_GLOBALS.DB_INSERT Then
            m_AuditEvent = AUDIT_EVENT_JOB_STAGE_CREATED
            m_ChangeDetail = "Stage " & ffJobStage.JobStateName & " Added By " & sUserName
        ElseIf operation = FF_GLOBALS.DB_UPDATE Then
            m_AuditEvent = AUDIT_EVENT_JOB_STAGE_UPDATED
            m_ChangeDetail = "Stage " & ffJobStage.JobStateName & " Updated By " & sUserName
        ElseIf operation = FF_GLOBALS.DB_UPDATE Then
            m_AuditEvent = AUDIT_EVENT_JOB_STAGE_DELETED
            m_ChangeDetail = "Stage " & ffJobStage.JobStateName & " Deleted By " & sUserName
        End If
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_JOB_STAGE
        m_CreatedBy = nUserId
        Call Add()
    End Sub


    Private Sub CreateJobComparisonEntry(ByVal ffJob As FF_Job, ByVal sChangeDetail As String, ByVal nUserId As Int32)
        Call Init()
        m_SourceId = ffJob.ID
        m_AuditEvent = AUDIT_EVENT_JOB_UPDATED
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_JOB
        m_ChangeDetail = sChangeDetail
        m_CreatedBy = nUserId
        Call Add()
    End Sub

    Public Sub New(ByVal ffUjm As FF_UserJobMapping, ByVal sChangeDetail As String, ByVal sourceId As Integer)
        Call Init()
        m_SourceId = sourceId
        m_AuditEvent = AUDIT_EVENT_RESOURCE_MAPPING_UPDATED
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_RESOURCE_JOB_MAPPING
        m_ChangeDetail = sChangeDetail
        m_CreatedBy = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        Call Add()
    End Sub

    Private Sub CompareCustomer()

    End Sub



    Public Sub CustomerAudit(ByVal ffCustomer As FF_Customer, ByVal operation As String, Optional ByVal oldffCustomer As FF_Customer = Nothing)

        Call Init()
        'Dim sUserName As String = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
        'Dim nUserId As Integer = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.UserID
        Dim sChangeDetail As New StringBuilder


        m_SourceId = ffCustomer.id
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_CUSTOMER
        m_CreatedBy = nUserId

        If operation = FF_GLOBALS.DB_INSERT Then

            m_AuditEvent = AUDIT_EVENT_CUSTOMER_CREATED
            m_ChangeDetail = ffCustomer.CustomerCode & " Added By " & sUserName
            Call Add()

        ElseIf operation = FF_GLOBALS.DB_UPDATE Then

            If ffCustomer.CustomerCode <> oldffCustomer.CustomerCode Then
                sChangeDetail.Append("Customer Code ")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerCode)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerCode)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerName <> oldffCustomer.CustomerName Then
                sChangeDetail.Append("Customer Name ")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerName)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerName)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerAddr1 <> oldffCustomer.CustomerAddr1 Then
                sChangeDetail.Append("Customer Addr1")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerAddr1)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerAddr1)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerAddr2 <> oldffCustomer.CustomerAddr2 Then
                sChangeDetail.Append("Customer Addr2")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerAddr2)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerAddr2)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerAddr3 <> oldffCustomer.CustomerAddr3 Then
                sChangeDetail.Append("Customer Addr3")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerAddr3)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerAddr3)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerTown <> oldffCustomer.CustomerTown Then
                sChangeDetail.Append("Customer Town")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerTown)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerTown)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerPostcode <> oldffCustomer.CustomerPostcode Then
                sChangeDetail.Append("Customer Post Code")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerPostcode)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerPostcode)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            ElseIf ffCustomer.CustomerCountryKey <> oldffCustomer.CustomerCountryKey Then
                sChangeDetail.Append("Customer Country Key")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomer.CustomerCountryKey)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomer.CustomerCountryKey)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_UPDATED
                Call Add()
            End If
        End If

        'Call Add()


    End Sub
    Public Sub ContactAudit(ByVal ffCustomerContact As FF_CustomerContact, ByVal operation As String, Optional ByVal oldffCustomerContact As FF_CustomerContact = Nothing)
        Call Init()
        Dim sChangeDetail As New StringBuilder
        m_SourceId = ffCustomerContact.id
        m_RecordType = FF_GLOBALS.AUDIT_TYPE_CUSTOMER_CONTACT
        m_CreatedBy = nUserId

        If operation = FF_GLOBALS.DB_INSERT Then

            m_AuditEvent = AUDIT_EVENT_CUSTOMER_CONTACT_CREATED
            m_ChangeDetail = ffCustomerContact.Name & " Added By " & sUserName
            Call Add()

        ElseIf operation = FF_GLOBALS.DB_UPDATE Then

            If ffCustomerContact.Name <> oldffCustomerContact.Name Then
                sChangeDetail.Append("Customer Contact Name")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomerContact.Name)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomerContact.Name)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_CONTACT_UPDATED
                Call Add()
            ElseIf ffCustomerContact.Telephone <> oldffCustomerContact.Telephone Then
                sChangeDetail.Append("Customer Contact Telephone")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomerContact.Telephone)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomerContact.Telephone)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_CONTACT_UPDATED
                Call Add()

            ElseIf ffCustomerContact.Mobile <> oldffCustomerContact.Mobile Then
                sChangeDetail.Append("Customer Contact Mobile")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomerContact.Mobile)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomerContact.Mobile)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_CONTACT_UPDATED
                Call Add()

            ElseIf ffCustomerContact.EmailAddr <> oldffCustomerContact.EmailAddr Then
                sChangeDetail.Append("Customer Contact EmailAddr")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomerContact.EmailAddr)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomerContact.EmailAddr)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_CONTACT_UPDATED
                Call Add()
            ElseIf ffCustomerContact.Notes <> oldffCustomerContact.Notes Then
                sChangeDetail.Append("Customer Contact Telephone")
                sChangeDetail.Append(gsChangedFrom)
                sChangeDetail.Append(oldffCustomerContact.Notes)
                sChangeDetail.Append(gsChangedTo)
                sChangeDetail.Append(ffCustomerContact.Notes)
                m_AuditEvent = AUDIT_EVENT_CUSTOMER_CONTACT_UPDATED
                Call Add()
            End If

        End If



    End Sub

    Private Sub CompareJobs(ByVal ffJobOld As FF_Job, ByVal ffJobNew As FF_Job, ByVal nUserId As Int32)
        Dim sbComparison As New StringBuilder
        If ffJobOld.CollateralDueOn <> ffJobNew.CollateralDueOn Then
            sbComparison.Append("Collateral due on")
            sbComparison.Append(gsChangedTo)
            sbComparison.Append(ffJobNew.CollateralDueOn.ToString("d-MMM-yyyy"))
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.DeadlineOn <> ffJobNew.DeadlineOn Then
            sbComparison.Append("Deadline on")
            sbComparison.Append(gsChangedTo)
            sbComparison.Append(ffJobNew.DeadlineOn.ToString("d-MMM-yyyy"))
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.JobName <> ffJobNew.JobName Then
            sbComparison.Append("Job name")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(ffJobOld.JobName)
            sbComparison.Append(gsTo)
            sbComparison.Append(ffJobNew.JobName)
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.CustomerKey <> ffJobNew.CustomerKey Then
            sbComparison.Append("Job customer")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(FF_Customer.GetCustomerCode(ffJobOld.CustomerKey))
            sbComparison.Append(gsTo)
            sbComparison.Append(FF_Customer.GetCustomerCode(ffJobNew.CustomerKey))
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.CustomerContactKey <> ffJobNew.CustomerContactKey Then
            sbComparison.Append("Job customer contact")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(FF_CustomerContact.GetContactName(ffJobOld.CustomerContactKey))
            sbComparison.Append(gsTo)
            sbComparison.Append(FF_CustomerContact.GetContactName(ffJobNew.CustomerContactKey))
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.AccountHandlerKey <> ffJobNew.AccountHandlerKey Then
            sbComparison.Append("Job account handler")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(GetNameFromUserId(ffJobOld.AccountHandlerKey))
            sbComparison.Append(gsTo)
            sbComparison.Append(GetNameFromUserId(ffJobNew.AccountHandlerKey))
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.ItemCount <> ffJobNew.ItemCount Then
            sbComparison.Append("Job item count")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(ffJobOld.ItemCount)
            sbComparison.Append(gsTo)
            sbComparison.Append(ffJobNew.ItemCount)
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.CustRef1 <> ffJobNew.CustRef1 Then
            sbComparison.Append("Customer Reference #1")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(ffJobOld.CustRef1)
            sbComparison.Append(gsTo)
            sbComparison.Append(ffJobNew.CustRef1)
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.CustRef2 <> ffJobNew.CustRef2 Then
            sbComparison.Append("Customer Reference #2")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(ffJobOld.CustRef2)
            sbComparison.Append(gsTo)
            sbComparison.Append(ffJobNew.CustRef2)
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.MaterialFromExternalSource <> ffJobNew.MaterialFromExternalSource Then
            sbComparison.Append("Material from external source")
            sbComparison.Append(gsChangedFrom)
            sbComparison.Append(ffJobOld.MaterialFromExternalSource)
            sbComparison.Append(gsTo)
            sbComparison.Append(ffJobNew.MaterialFromExternalSource)
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If

        If ffJobOld.IsCancelled <> ffJobNew.IsCancelled Then
            sbComparison.Append("Job CANCELLED")
            Call CreateJobComparisonEntry(ffJobNew, sbComparison.ToString, nUserId)
            sbComparison.Length = 0
        End If
    End Sub

    Private Function GetNameFromUserId(ByVal nUserId As Int32) As String
        GetNameFromUserId = String.Empty
        Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(0, nUserId)
        If uiCurrentUser IsNot Nothing Then
            GetNameFromUserId = uiCurrentUser.DisplayName & IIf(FF_GLOBALS.bDebugMode, " (" & uiCurrentUser.UserID & ")", "")
        End If
        Return GetNameFromUserId

    End Function

    Public Shared Function GetEventsForJob(ByVal nJobId As Int32) As DataTable
        Dim sSQL As String = "SELECT CreatedOn 'Date/Time', AuditEvent 'Event', ChangeDetail 'Description', DisplayName 'User' FROM FF_AuditTrail ffat LEFT OUTER JOIN Users u ON ffat.CreatedBy = u.UserId WHERE RecordType = 'J' AND SourceId = " & nJobId & " ORDER BY [id]"
        GetEventsForJob = DNNDB.Query(sSQL)
    End Function



End Class
