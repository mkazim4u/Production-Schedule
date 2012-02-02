Imports Telerik.Web.UI
Imports DotNetNuke.Services.Personalization

Partial Class PS_Report
    Inherits System.Web.UI.UserControl

    Private rptJobListCost As New FFReporting.rptJobListCost
    Const USER_REPORTS As String = "User_Reports"

    'Public Property palReports() As Hashtable
    '    Get
    '        Dim o As Object = ViewState("palReports")
    '        If o Is Nothing Then
    '            Return Nothing
    '        End If
    '        Return DirectCast(o, Hashtable)
    '    End Get
    '    Set(ByVal Value As Hashtable)
    '        ViewState("palReports") = Value
    '    End Set
    'End Property

    Public Property palReports() As ArrayList
        Get
            Dim o As Object = ViewState("palReports")
            If o Is Nothing Then
                Return Nothing
            End If
            Return DirectCast(o, ArrayList)
        End Get
        Set(ByVal Value As ArrayList)
            ViewState("palReports") = Value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            'LoadFavourites()

        End If

    End Sub

    Protected Sub LoadFavourites()

        LoadUserSettings()

    End Sub


    Protected Sub rtvReports_NodeClick(ByVal sender As Object, ByVal e As RadTreeNodeEventArgs)

        Dim sNodeName As String = e.Node.Text

        Select Case sNodeName

            Case "Total Cost Report"
                divCostReport.Visible = True
                Exit Select

            Case "Help"
                divCostReport.Visible = False
                Exit Select
            Case "Support"
                divCostReport.Visible = False
                Exit Select

        End Select

    End Sub

    Private Sub SetParametersForJobListReport()

        If rdpFromDate.SelectedDate IsNot Nothing Then
            rptJobListCost.ReportParameters("DateFrom").Value = rdpFromDate.SelectedDate
        End If

        If rdpToDate.SelectedDate IsNot Nothing Then
            rptJobListCost.ReportParameters("DateTo").Value = rdpToDate.SelectedDate
        End If

        rptJobListCost.ReportParameters("UserName").Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username

        rptViewer.Report = rptJobListCost




    End Sub

    Protected Sub btnGo_Click(ByVal sender As Object, ByVal e As EventArgs)

        SetParametersForJobListReport()
        'PrinterSettings(rptJobListCost)

    End Sub

    Protected Sub LoadUserSettings()

        'Personalization.RemoveProfile(DNN.GetModuleID(Me).ToString(), USER_REPORTS)

        palReports = Personalization.GetProfile(DNN.GetModuleID(Me).ToString(), USER_REPORTS)

        If palReports IsNot Nothing Then

            Dim favNode As RadTreeNode = rtvReports.Nodes.FindNodeByText("Favourite Reports")

            For Each sNodeName As String In palReports

                favNode.Nodes.Add(New RadTreeNode(sNodeName, sNodeName))

            Next

        End If


    End Sub

    Protected Sub SetUserSettings(ByVal strReportName As String)

        Dim al As ArrayList


        If palReports Is Nothing Then
            palReports = New ArrayList
        End If

        If Not palReports.Contains(strReportName) Then
            palReports.Add(strReportName)
            Personalization.SetProfile(DNN.GetModuleID(Me).ToString(), USER_REPORTS, palReports)
            CopyToFavourites(strReportName)
        End If


    End Sub

    Protected Sub CopyToFavourites(ByVal sNodeName As String)

        For Each node As RadTreeNode In rtvReports.GetAllNodes

            If node.Text = "Favourite Reports" Then

                'Dim favNode As RadTreeNode = rtvReports.Nodes.FindNodeByText("Favourite Reports")

                node.Nodes.Add(New RadTreeNode(sNodeName, sNodeName))

                'favNode.Nodes.Add(New RadTreeNode(sNodeName, sNodeName))


            End If


        Next


    End Sub

    Protected Sub PrinterSettings(ByVal report As Object)

        Dim printerSettings As New System.Drawing.Printing.PrinterSettings

        ' The standard print controller comes with no UI
        Dim standardPrintController As New System.Drawing.Printing.StandardPrintController

        ' Print the report using the custom print controller
        Dim reportProcessor As New Telerik.Reporting.Processing.ReportProcessor
        reportProcessor.PrintController = standardPrintController
        'reportProcessor.PrintReport(CType(report, Telerik.Reporting.Report), printerSettings)

    End Sub

    Protected Sub rtvReports_ContextMenuItemClick(ByVal sender As Object, ByVal e As Telerik.Web.UI.RadTreeViewContextMenuEventArgs)
        Select Case e.MenuItem.Value
            Case "Copy"
                If e.Node.Level = 2 Then

                    SetUserSettings(e.Node.Text)

                End If
                Exit Select
            Case "Remove"
                If e.Node.Level = 2 Then
                    e.Node.Remove()

                End If
                Exit Select
        End Select
    End Sub


End Class


'Dim paramDateFrom As New Telerik.Reporting.ReportParameter()
'paramDateFrom.Name = "DateFrom"
'paramDateFrom.Type = Telerik.Reporting.ReportParameterType.DateTime
'paramDateFrom.AllowBlank = False
'paramDateFrom.AllowNull = False

'paramDateFrom.Value = rdpFromDate.SelectedDate

'rptJobListCost.ReportParameters.Add(paramDateFrom)

'Dim paramDateTo As New Telerik.Reporting.ReportParameter()
'paramDateTo.Name = "DateTo"
'paramDateTo.Type = Telerik.Reporting.ReportParameterType.DateTime
'paramDateTo.AllowBlank = False
'paramDateTo.AllowNull = False

'paramDateTo.Value = rdpToDate.SelectedDate

'rptJobListCost.ReportParameters.Add(paramDateTo)

'rptViewer.Report = rptJobListCost

'Dim paramUserName As New Telerik.Reporting.ReportParameter()
'        paramUserName.Name = "UserName"
'        paramUserName.Type = Telerik.Reporting.ReportParameterType.String
'        paramUserName.AllowBlank = False
'        paramUserName.AllowNull = False
'        paramUserName.Value = DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.Username
'        rptJobListCost.ReportParameters.Add(paramUserName)
