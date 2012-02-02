Imports Microsoft.VisualBasic
Imports Telerik.Web.UI
Imports Telerik.OpenAccess
Imports System.Reflection

Public Class FF_GLOBALS


    Public Const PAGE_CREATE_MAIN_MENU As String = "Production Schedule"
    Public Const PAGE_PRODUCTION_SCHEDULE_LIST As String = "Production Schedule List"
    Public Const PAGE_CREATE_EDIT_JOB As String = "Create Job"
    Public Const PAGE_EDIT_JOB_STATES As String = "Edit Job States"
    Public Const PAGE_OPTIONS As String = "Options"
    Public Const PAGE_CREATE_EDIT_CUSTOMER As String = "Create Edit Customer"
    Public Const PAGE_CREATE_EDIT_SCHEDULED_JOB As String = "Manage Scheduled Jobs"
    Public Const PAGE_CLIPBOARD As String = "Clipboard"
    Public Const PAGE_NEW_JOB_FROM_CLIPBOARD As String = "New Job From Clipboard"
    Public Const PAGE_CLEAR_CLIPBOARD As String = "Clear Clipboard"
    Public Const PAGE_ADD_RESOURCES As String = "Resource Management"
    Public Const PAGE_ADD_Group As String = "Add Group"
    Public Const PAGE_ADD_Role As String = "Add Role"
    Public Const PAGE_REPORTS As String = "Reports"
    Public Const PAGE_USER_ROLE_ASSIGNMENT As String = "User Information"
    Public Const PAGE_TEAM_MANAGEMENT As String = "Team Management"
    Public Const PAGE_ADD_QUOTATION As String = "Add Quotation"
    Public Const PAGE_HELP_REFERENCE As String = "HelpReference"

    Public Const PS_USER_NAME_PREFIX As String = "SPRINTPDC\"

    Public Const PAGE_TITLE_TEXT As String = "Production Schedule"

    Public Const ROLE_GROUP_NAME As String = "Production Schedule"
    Public Const ROLE_GROUP_ID As Integer = 0

    Public Const ROLE_SUPER_USER As String = "PS_Superuser"
    Public Const ROLE_MANAGER As String = "PS_Manager"
    Public Const ROLE_USER As String = "PS_User"
    Public Const ROLE_ACCOUNT_HANDLER As String = "PS_AccountHandler"

    Public Const PS_EMAIL_NOTIFIER As String = "portal.notifier@gmail.com"
    Public Const PS_EMAIL_PROPERTY As String = "RPSEmail"
    Public Const PS_PRIVILEGE_PROPERTY As String = "Privilege"

    Public Const PRIVILEGE_ACCOUNT_HANDLER As String = "PS_AccountHandler"

    Public Const JOB_STATE_STARTED As String = "Started"
    Public Const JOB_STATE_COMPLETED As String = "Completed"

    Public Const JOB_STATE_MARK_COMPLETED As String = "C"
    Public Const JOB_STATE_MARK_UNCOMPLETED As String = "U"

    Public Const SELECT_ALL As String = "- Select All -"

    Public Const COUNTRY_KEY_UK As Integer = 222

    Public Const MODE_CREATE As String = "create"
    Public Const MODE_COPY As String = "copy"
    Public Const MODE_EDIT As String = "edit"
    Public Const MODE_TEMPLATE As String = "template"
    Public Const MODE_IMPORT As String = "import"

    Public Const COPY_JOB_TO_CLIPBOARD As String = "JobFromClipboard"
    Public Const SESSION_JOB_ID As String = "JobId"

    Public Const REPORT_JOB As String = "Job"
    Public Const REPORT_JOB_LIST As String = "JobList"

    Public Const DB_INSERT As String = "Insert"
    Public Const DB_UPDATE As String = "Update"
    Public Const DB_DELETE As String = "Delete"

    Public Const RECORD_TYPE_TARIFF As String = "TAR"
    Public Const RECORD_TYPE_QUOTATION As String = "QUO"
    Public Const RECORD_TYPE_COST_MATRIX As String = "CMX"


    '''''''''''''''' Denton Shop '''''''''''''''''
    Public Const RECORD_TYPE_ORDER As String = "O"

    Public Const AUDIT_EVENT_ORDER_UPDATED As String = "Order Updated"

    Public Const ORDER_RECEIVED As Integer = 80
    Public Const ORDER_WAITING_FOR_AUTHORIZATION As Integer = 81
    Public Const ORDER_APPROVE As Integer = 120
    Public Const ORDER_REJECT As Integer = 30
    Public Const ORDER_WAITING_FOR_FULFIMENT As Integer = 130
    Public Const ORDER_FULFILLED As Integer = 140

    Public Const CUSTOMER_ID_KEY As String = "CUSTOMER_ID"

    Public Const ME_PORTAL As String = "ME"
    Public Const UK_PORTAL As String = "UK"
    Public Const USA_PORTAL As String = "US"

    Public Const UK_PORTAL_ID As Integer = 0
    Public Const US_PORTAL_ID As Integer = 5
    Public Const ME_PORTAL_ID As Integer = 6


    Public Const UK_PORTAL_URL As String = "http://denton1.sprintexpress.co.uk/login.aspx"
    Public Const USA_PORTAL_URL As String = "http://denton2.sprintexpress.co.uk/login.aspx"
    Public Const ME_PORTAL_URL As String = "http://denton3.sprintexpress.co.uk/login.aspx"

    Public Const USA_PRODUCT_IDENTIFIER As String = "(US)"
    Public Const UK_PRODUCT_IDENTIFIER As String = "(UK)"
    Public Const ME_PRODUCT_IDENTIFIER As String = "(ME)"




    Public Enum PRODUCTS_TYPE

        UK_PRODUCTS = 0
        USA_PRODUCTS = 1
        ME_PRODUCTS = 2
        
    End Enum

    Public Const PAGE_USER_REGISTRATION As String = "New User Registration"


    '''''''''''''''''' Denton Shop '''''''''''''''''


    Public Const PATH_TO_PS_USER_FILES As String = "ProductionSchedule\"

    Public Const BASE_DATE As String = "1/1/1900"
    Public Const BASE_DATE_MONTH_FORMAT As String = "1-Jan-1900"
    'Public Const BASE_DATE As String = "1-Jan-1900 12:00:00 PM"
    'Public Const BASE_DATE As String = "1-Jan-1900"
    Public Const CLIPBOARD_MODULE_ID As Int32 = 1000

    Public Const AUDIT_TYPE_JOB As String = "J"
    Public Const AUDIT_TYPE_CUSTOMER As String = "C"
    Public Const AUDIT_TYPE_CUSTOMER_CONTACT As String = "T"
    Public Const AUDIT_TYPE_JOB_STAGE As String = "S"
    Public Const AUDIT_TYPE_RESOURCE_JOB_MAPPING As String = "M"

    Public Const JOB_FILES_PATH As String = "ProductionSchedule\"

    Public Shared bDebugMode As Boolean

    Public ReadOnly Property BasePage(ByVal Myself As Object) As DotNetNuke.Framework.CDefault
        Get
            Return CType(Myself.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

    'Public Shared Sub PopulateComboboxCountry(ByRef rcbCountry As RadComboBox)
    '    rcbCountry.Items.Clear()
    '    rcbCountry.Items.Add(New RadComboBoxItem("- please select -", 0))
    '    Dim sSQL As String = "SELECT SUBSTRING(CountryName, 1, 25), CountryKey FROM Country WHERE DeletedFlag = 0 ORDER BY CountryName"
    '    Dim oDT As DataTable = SprintDB.Query(sSQL)
    '    For Each dr As DataRow In oDT.Rows
    '        rcbCountry.Items.Add(New RadComboBoxItem(dr(0), dr(1)))
    '    Next
    'End Sub

    'Public Shared Sub PopulateComboboxCountry(ByRef ddlCountry As DropDownList)
    '    ddlCountry.Items.Clear()
    '    ddlCountry.Items.Add(New ListItem("- please select -", 0))
    '    Dim sSQL As String = "SELECT SUBSTRING(CountryName, 1, 25), CountryKey FROM Country WHERE DeletedFlag = 0 ORDER BY CountryName"
    '    Dim oDT As DataTable = SprintDB.Query(sSQL)
    '    For Each dr As DataRow In oDT.Rows
    '        ddlCountry.Items.Add(New ListItem(dr(0), dr(1)))
    '    Next
    'End Sub

    Public Shared Sub PopulateAccountHandlerCombobox(ByRef rcbAccountHandler As RadComboBox, ByVal Myself As Control)

        Dim rci As RadComboBoxItem
        Dim roleCtlr As New DotNetNuke.Security.Roles.RoleController
        Dim sRoleName As String = "Account Handlers"
        Dim objUserRoles As ArrayList = roleCtlr.GetUserRolesByRoleName(DNN.GetPMB(Myself).PortalId, FF_GLOBALS.ROLE_ACCOUNT_HANDLER)
        rcbAccountHandler.Items.Clear()
        rcbAccountHandler.Items.Add(New RadComboBoxItem("- select Acct Handler -", 0))
        For Each uri As UserRoleInfo In objUserRoles
            Dim nUserId As Integer = uri.UserID
            Dim uiCurrentUser As DotNetNuke.Entities.Users.UserInfo = UserController.GetUserById(DNN.GetPMB(Myself).PortalId, nUserId)
            rci = rcbAccountHandler.FindItemByText(uiCurrentUser.Username)
            If rci Is Nothing Then
                rcbAccountHandler.Items.Add(New RadComboBoxItem(uiCurrentUser.Username, uiCurrentUser.UserID))
            End If
            'rcbAccountHandler.Items.Add(New RadComboBoxItem(uiCurrentUser.DisplayName & IIf(FF_GLOBALS.bDebugMode, " (" & uiCurrentUser.UserID & ")", ""), uiCurrentUser.UserID))

        Next
    End Sub

    Public Shared Function sWithinCurrentWeek(ByVal dtDate As DateTime) As String
        sWithinCurrentWeek = String.Empty
        Dim dtTargetDate = dtDate.Date
        Dim nTargetDayOfWeek As Int32 = dtTargetDate.DayOfWeek
        Dim dtToday As DateTime = Date.Today.Date
        Dim nTodayDayOfWeek As Int32 = dtToday.DayOfWeek

        If dtTargetDate > dtToday Then
            If nTargetDayOfWeek <= nTodayDayOfWeek Then
                sWithinCurrentWeek = "Next " & dtTargetDate.ToString("dddd")
            Else
                sWithinCurrentWeek = dtTargetDate.ToString("dddd")
            End If
        Else
            sWithinCurrentWeek = "Last " & dtTargetDate.ToString("dddd")
        End If
    End Function


    Public Shared Function IsValidDate(ByVal dtDate As DateTime) As String

        Dim dt As Date = dtDate.Date
        If dtDate = FF_GLOBALS.BASE_DATE_MONTH_FORMAT Or dtDate = FF_GLOBALS.BASE_DATE Then
            IsValidDate = String.Empty
        Else

            If dt = Date.Today Then
                IsValidDate = "Today"
            ElseIf dt > DateTime.Now.AddDays(7).Date Or dt < DateTime.Now.AddDays(-7).Date Then
                IsValidDate = dt.ToString("d-MMM-yyyy")
            ElseIf dt = DateTime.Now.AddDays(-1).Date Then
                IsValidDate = "Yesterday"
            ElseIf dt = DateTime.Now.AddDays(1).Date Then
                IsValidDate = "Tomorrow"
            Else
                IsValidDate = sWithinCurrentWeek(dtDate)
            End If


        End If

        Return IsValidDate

    End Function
    Public Shared Sub PopulateComboboxCountry(ByRef ddlCountry As DropDownList)
        ddlCountry.Items.Clear()
        ddlCountry.Items.Add(New ListItem("- please select -", 0))
        Dim sSQL As String = "SELECT SUBSTRING(CountryName, 1, 25), CountryKey FROM Country WHERE DeletedFlag = 0 ORDER BY CountryName"
        Dim oDT As DataTable = SprintDB.Query(sSQL)
        For Each dr As DataRow In oDT.Rows
            ddlCountry.Items.Add(New ListItem(dr(0), dr(1)))
        Next
    End Sub

    'Public Shared Function ConvertToDataTable(Of T)(ByVal queryResult As IEnumerable(Of T)) As DataTable

    'Dim dtReturn As New DataTable("Result")

    '' column names 
    'Dim oProps As PropertyInfo() = Nothing

    'If queryResult Is Nothing Then
    '    Return dtReturn
    'End If

    'For Each rec As T In queryResult
    '    ' Use reflection to get property names, to create table, Only first time, others 
    '    'will follow 
    '    If oProps Is Nothing Then
    '        oProps = DirectCast(rec.[GetType](), Type).GetProperties()

    '        For Each pi As PropertyInfo In oProps
    '            Dim colType As Type = pi.PropertyType

    '            If (colType.IsGenericType) AndAlso (colType.GetGenericTypeDefinition() = GetType(Nullable(Of ))) Then
    '                colType = colType.GetGenericArguments()(0)
    '            End If

    '            dtReturn.Columns.Add(New DataColumn(pi.Name, colType))
    '        Next
    '    End If

    '    Dim dr As DataRow = dtReturn.NewRow()

    '    For Each pi As PropertyInfo In oProps
    '        dr(pi.Name) = If(pi.GetValue(rec, Nothing) Is Nothing, DBNull.Value, pi.GetValue(rec, Nothing))
    '    Next

    '    dtReturn.Rows.Add(dr)
    'Next

    '    Return dtReturn
    'End Function



End Class
