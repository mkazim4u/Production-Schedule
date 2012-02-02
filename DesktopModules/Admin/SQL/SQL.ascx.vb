'
' DotNetNuke� - http://www.dotnetnuke.com
' Copyright (c) 2002-2010
' by DotNetNuke Corporation
'
' Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated 
' documentation files (the "Software"), to deal in the Software without restriction, including without limitation 
' the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and 
' to permit persons to whom the Software is furnished to do so, subject to the following conditions:
'
' The above copyright notice and this permission notice shall be included in all copies or substantial portions 
' of the Software.
'
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED 
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL 
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER 
' DEALINGS IN THE SOFTWARE.
'
Imports DotNetNuke.Services.Log.EventLog

Namespace DotNetNuke.Modules.Admin.SQL

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The SQL PortalModuleBase is used run SQL Scripts on the Database
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
    '''                       and localisation
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class SQL
        Inherits DotNetNuke.Entities.Modules.PortalModuleBase

#Region "Controls"

        Protected WithEvents lblRunAsScript As System.Web.UI.WebControls.Label

#End Region

#Region "Private Methods"
        Private Sub CheckSecurity()
            ' Verify that the current user has access to access this page
            If Not UserInfo.IsSuperUser Then
                Response.Redirect(NavigateURL("Access Denied"), True)
            End If
        End Sub
#End Region

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded.
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        '''     [VMasanas]  9/28/2004   Changed redirect to Access Denied
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                CheckSecurity()
                If Not Page.IsPostBack Then
                    Dim colConnections As ConnectionStringSettingsCollection = ConfigurationManager.ConnectionStrings
                    For Each objConnection As ConnectionStringSettings In colConnections
                        If objConnection.Name.ToLower <> "localmysqlserver" And objConnection.Name.ToLower <> "localsqlserver" Then
                            cboConnection.Items.Add(objConnection.Name)
                        End If
                    Next
                    cboConnection.SelectedIndex = 0
                    cmdExecute.ToolTip = Services.Localization.Localization.GetString("cmdExecute.ToolTip", Me.LocalResourceFile)
                    chkRunAsScript.ToolTip = Services.Localization.Localization.GetString("chkRunAsScript.ToolTip", Me.LocalResourceFile)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' cmdExecute_Click runs when the Execute button is clicked
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' 	[cnurse]	9/28/2004	Updated to reflect design changes for Help, 508 support
        '''                       and localisation
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub cmdExecute_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExecute.Click
            Try
                CheckSecurity()
                If txtQuery.Text <> "" Then
                    Dim connectionstring As String = Config.GetConnectionString(cboConnection.SelectedValue)
                    If chkRunAsScript.Checked Then
                        Dim strError As String = DataProvider.Instance.ExecuteScript(connectionstring, txtQuery.Text)
                        If strError = Null.NullString Then
                            lblMessage.Text = Services.Localization.Localization.GetString("QuerySuccess", Me.LocalResourceFile)
                        Else
                            lblMessage.Text = strError
                        End If
                    Else
                        Dim dr As IDataReader = DataProvider.Instance().ExecuteSQLTemp(connectionstring, txtQuery.Text)
                        If Not dr Is Nothing Then
                            gvResults.DataSource = dr
                            gvResults.DataBind()
                            dr.Close()
                        Else
                            lblMessage.Text = Services.Localization.Localization.GetString("QueryError", Me.LocalResourceFile)
                        End If
                    End If

                    RecordAuditEventLog(txtQuery.Text)
                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub RecordAuditEventLog(ByVal query As String)
            Dim props As New LogProperties
            props.Add(New LogDetailInfo("User", UserInfo.Username))
            props.Add(New LogDetailInfo("SQL Query", query))

            Dim elc As New EventLogController
            elc.AddLog(props, PortalSettings, UserId, EventLogController.EventLogType.HOST_SQL_EXECUTED.ToString, True)
        End Sub

#End Region

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub

        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Protected Sub cmdUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpload.Click
            CheckSecurity()
            If Page.IsPostBack Then
                If uplSqlScript.PostedFile.FileName <> "" Then
                    Dim scriptFile As New System.IO.StreamReader(uplSqlScript.PostedFile.InputStream)
                    txtQuery.Text = scriptFile.ReadToEnd()
                End If
            End If
        End Sub
    End Class

End Namespace
