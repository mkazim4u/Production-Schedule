Imports FFReporting
Imports Telerik.Web.UI
Imports Telerik.ReportViewer
Imports Telerik.Reporting
Imports Telerik.Reporting.Processing

Partial Class ReportViewer
    Inherits System.Web.UI.UserControl

    Private rptJobReport As New FFReporting.rptJobReport
    Private rptJobList As New FFReporting.rptJobList
    Private rptName As String
    Public ReadOnly Property BasePage As DotNetNuke.Framework.CDefault
        Get
            Return CType(Me.Page, DotNetNuke.Framework.CDefault)
        End Get
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            Call LoadReportFromQueryString()
            Call SetTitles()
        End If

    End Sub
    Private Sub LoadJobCombo()

        'Dim dt As DataTable = DNNDB.ExecuteStoredProcedure("GetAllJobFromFF_Job")
        'cmbJobID.DataSource = dt
        'cmbJobID.DataTextField = "ID"
        'cmbJobID.DataValueField = "ID"
        'cmbJobID.DataBind()


    End Sub



    Protected Sub SetTitles()

        Me.BasePage.Title = FF_GLOBALS.PAGE_TITLE_TEXT + " - " + FF_GLOBALS.PAGE_REPORTS

    End Sub

    Private Sub SetParametersForJobListReport()

        Dim paramCompleted As New Telerik.Reporting.ReportParameter()
        paramCompleted.Name = "Completed"
        paramCompleted.Type = Telerik.Reporting.ReportParameterType.Integer
        paramCompleted.AllowBlank = False
        paramCompleted.AllowNull = False

        If Session("Completed") IsNot Nothing Then
            paramCompleted.Value = Convert.ToInt64(Session("Completed"))
        End If
        rptJobList.ReportParameters.Add(paramCompleted)

        Dim paramUnCompleted As New Telerik.Reporting.ReportParameter()
        paramUnCompleted.Name = "UnCompleted"
        paramUnCompleted.Type = Telerik.Reporting.ReportParameterType.Integer
        paramUnCompleted.AllowBlank = False
        paramUnCompleted.AllowNull = False

        If Session("UnCompleted") IsNot Nothing Then
            paramUnCompleted.Value = Convert.ToInt64(Session("UnCompleted"))
        End If
        rptJobList.ReportParameters.Add(paramUnCompleted)

        Dim paramRecentlyCompleted As New Telerik.Reporting.ReportParameter()
        paramRecentlyCompleted.Name = "RecentlyCompleted"
        paramRecentlyCompleted.Type = Telerik.Reporting.ReportParameterType.Integer
        paramRecentlyCompleted.AllowBlank = False
        paramRecentlyCompleted.AllowNull = False

        If Session("RecentlyCompleted") IsNot Nothing Then
            paramRecentlyCompleted.Value = Convert.ToInt64(Session("RecentlyCompleted"))
        End If
        rptJobList.ReportParameters.Add(paramRecentlyCompleted)

        Dim paramUserId As New Telerik.Reporting.ReportParameter()
        paramUserId.Name = "UserId"
        paramUserId.Type = Telerik.Reporting.ReportParameterType.Integer
        paramUserId.AllowBlank = False
        paramUserId.AllowNull = False
        If Session("UserId") IsNot Nothing Then
            paramUserId.Value = Convert.ToInt64(Session("UserId"))
        End If
        rptJobList.ReportParameters.Add(paramUserId)

        Dim paramUserName As New Telerik.Reporting.ReportParameter()
        paramUserName.Name = "UserName"
        paramUserName.Type = Telerik.Reporting.ReportParameterType.String
        paramUserName.AllowBlank = False
        paramUserName.AllowNull = False
        paramUserName.Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username

        rptJobList.ReportParameters.Add(paramUserName)
        rptViewer.Report = rptJobList

        'PrinterSettings(rptJobList)
        'Dim paramDateFrom As New Telerik.Reporting.ReportParameter()
        'paramDateFrom.Name = "DateFrom"
        'paramDateFrom.Type = Telerik.Reporting.ReportParameterType.DateTime
        'paramDateFrom.AllowBlank = False
        'paramDateFrom.AllowNull = False

        'If Request.QueryString("DateFrom") IsNot Nothing Then
        '    paramDateFrom.Value = CDate(Request.QueryString("DateFrom"))
        'End If
        'rptJobList.ReportParameters.Add(paramDateFrom)

        'Dim paramDateTo As New Telerik.Reporting.ReportParameter()
        'paramDateTo.Name = "DateTo"
        'paramDateTo.Type = Telerik.Reporting.ReportParameterType.DateTime
        'paramDateTo.AllowBlank = False
        'paramDateTo.AllowNull = False

        'If Request.QueryString("DateTo") IsNot Nothing Then
        '    paramDateTo.Value = CDate(Request.QueryString("DateTo"))
        'End If







    End Sub

    Private Sub LoadReportFromQueryString()
        'If Request.Params("ReportName") IsNot Nothing Then
        rptName = Request.Params("ReportName")
        If rptName = FF_GLOBALS.REPORT_JOB Then
            'Call LoadJobCombo()
            Call SetParametersForJobReport()
            Call PrinterSettings(rptJobReport)
            rptViewer.RefreshReport()
            'tdJobIDCombo.Visible = True
        ElseIf rptName = FF_GLOBALS.REPORT_JOB_LIST Then
            Call SetParametersForJobListReport()
            Call PrinterSettings(rptJobList)
            rptViewer.RefreshReport()
            'tdJobIDCombo.Visible = False
        End If
        'End If
    End Sub

    'Private Sub SetParametersForJobReport(ByVal nJobID As Integer)

    '    Dim paramJobId As New Telerik.Reporting.ReportParameter()
    '    paramJobId.Name = "ID"
    '    paramJobId.Type = Telerik.Reporting.ReportParameterType.Integer
    '    paramJobId.AllowBlank = False
    '    paramJobId.AllowNull = False
    '    paramJobId.Value = nJobID
    '    paramJobId.Visible = False
    '    rptJobReport.ReportParameters.Add(paramJobId)
    '    'rptJobReport.ReportParameters("ID").Value = nJobID

    '    Dim paramUserName As New Telerik.Reporting.ReportParameter()
    '    paramUserName.Name = "User"
    '    paramUserName.Type = Telerik.Reporting.ReportParameterType.String
    '    paramUserName.AllowBlank = False
    '    paramUserName.AllowNull = False
    '    paramUserName.Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
    '    rptJobReport.ReportParameters.Add(paramUserName)

    '    reportViewer.Report = rptJobReport

    'End Sub

    Private Sub SetParametersForJobReport()
        Dim nJobID As Integer
        If Session(FF_GLOBALS.SESSION_JOB_ID) IsNot Nothing Then
            nJobID = Convert.ToInt64(Session(FF_GLOBALS.SESSION_JOB_ID))
            If nJobID <> 0 Then
                'cmbJobID.SelectedValue = nJobID
                Dim reportParameter1 As New Telerik.Reporting.ReportParameter()
                reportParameter1.Name = "ID"
                reportParameter1.Type = Telerik.Reporting.ReportParameterType.Integer
                reportParameter1.AllowBlank = False
                reportParameter1.AllowNull = False
                reportParameter1.Value = nJobID
                reportParameter1.Visible = False
                rptJobReport.ReportParameters.Add(reportParameter1)
                rptJobReport.ReportParameters("ID").Value = nJobID

                Dim paramUserName As New Telerik.Reporting.ReportParameter()
                paramUserName.Name = "UserName"
                paramUserName.Type = Telerik.Reporting.ReportParameterType.String
                paramUserName.AllowBlank = False
                paramUserName.AllowNull = False
                paramUserName.Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
                rptJobReport.ReportParameters.Add(paramUserName)



                rptViewer.Report = rptJobReport


            End If
        End If

    End Sub

    Private Sub PrinterSettings(ByVal report As Object)



        'Dim printScript = String.Format("{0}.PrintReport()", Me.reportViewer.ClientID)
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "ReportPrint", printScript, True)
        Dim result As Telerik.Reporting.Processing.RenderingResult = Nothing
        Dim deviceInfo As Hashtable = New Hashtable()
        deviceInfo("JavaScript") = "this.print({bUI: false, bSilent: true, bShrinkToFit: true});"
        Dim reportProcessor As Telerik.Reporting.Processing.ReportProcessor = New ReportProcessor()

        Dim o As Object = report
        If o Is rptJobList Then
            Dim fileName As String = "JobListReport " + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf"
            result = reportProcessor.RenderReport("PDF", rptJobList, deviceInfo)
            'o = rptJobList
        ElseIf o Is rptJobReport Then
            Dim fileName As String = "JobReport " + DateTime.Now.ToString("dd/MM/yyyy") + ".pdf"
            result = reportProcessor.RenderReport("PDF", rptJobReport, deviceInfo)
            'o = rptJobReport

        End If

        Response.Clear()
        Response.ContentType = result.MimeType
        Response.Cache.SetCacheability(HttpCacheability.[Private])
        Response.Expires = -1
        Response.Buffer = True
        Response.BinaryWrite(result.DocumentBytes)
        Response.End()


        'printerSettings(rptJobReport)
        '' Obtain the settings of the default printer
        'Dim printerSettings As New System.Drawing.Printing.PrinterSettings

        '' The standard print controller comes with no UI
        'Dim standardPrintController As New System.Drawing.Printing.StandardPrintController

        '' Print the report using the custom print controller
        'Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor
        'reportProcessor.PrintController = standardPrintController
        'reportProcessor.PrintReport(CType(o, Telerik.Reporting.Report), printerSettings)


        'Dim printScript = String.Format("{0}.PrintReport()", Me.reportViewer.ClientID)

        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "ReportPrint", printScript, True)

        'Dim cstext2 As New StringBuilder()
        'cstext2.Append("<script type=""text/javascript""> function Print() {")
        'cstext2.Append("<%=reportViewer.ClientID %>.PrintReport()} </script>")
        'Page.ClientScript.RegisterStartupScript(Me.GetType(), "Print", cstext2.ToString(), True)



    End Sub



End Class


