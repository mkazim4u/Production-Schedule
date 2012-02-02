' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee, Romain Doidy
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 
'
'
Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.IO

Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The ViewSQLReport class displays the content
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class AdminReport
        Inherits BaseAdminModule

        Dim RepID As Integer = -1
        Dim _frm As String = ""

#Region "Event Handlers"

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            RepID = -1
            If Not (Request.QueryString("RepID") Is Nothing) Then
                If IsNumeric(Request.QueryString("RepID")) Then
                    RepID = CInt(Request.QueryString("RepID"))
                End If
            End If

            _frm = ""
            If Not (Request.QueryString("frm") Is Nothing) Then
                _frm = Request.QueryString("frm")
            End If

        End Sub

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try



                AdminReportEdit.RepID = RepID
                AdminReportList.IsEdit = IsEditable

                If Not Page.IsPostBack Then
                    If Not (Request.QueryString("ReportRef") Is Nothing) Then
                        RunByRef(CType(Request.QueryString("ReportRef"), String))
                    Else
                        If RepID >= 0 And _frm = "" Then
                            populateEdit(RepID)
                        Else
                            populateList()
                        End If
                    End If
                End If

            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub AdminReportList_AddButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdminReportList.AddButton
            Response.Redirect(EditUrl("RepID", "0", "AdminReport"))
        End Sub

        Private Sub AdminReportList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles AdminReportList.ItemCommand
            Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
            Select Case e.CommandName
                Case "View"
                    RedirectToForm(ItemId, "view")
                    ViewReport(ItemId)
                Case "Export"
                    RedirectToForm(ItemId, "export")
                    ExportToExcel(ItemId)
                Case "EditRep"
                    Response.Redirect(EditUrl("RepID", ItemId, "AdminReport"))
                Case "Email"
                    RedirectToForm(ItemId, "email")
                    EmailReport(ItemId)
                Case Else
                    Response.Redirect(EditUrl("AdminReport"))
            End Select
        End Sub


#End Region

#Region "Methods"

        Private Sub populateForm(ByVal RepID As String)
            pnlList.Visible = False
            pnlResults.Visible = False
            pnlForm.Visible = True
            pnlEdit.Visible = False
        End Sub

        Private Sub populateEdit(ByVal RepID As String)
            pnlList.Visible = False
            pnlResults.Visible = False
            pnlForm.Visible = False
            pnlEdit.Visible = True
            AdminReportEdit.populateEdit(RepID)
        End Sub
        Private Sub populateList()
            If _frm <> "" Then
                pnlList.Visible = False
                pnlForm.Visible = True
            Else
                pnlList.Visible = True
                pnlForm.Visible = False
            End If
            pnlResults.Visible = False
            pnlEdit.Visible = False
            If UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrator") Then
                AdminReportList.IsEdit = True
            Else
                AdminReportList.IsEdit = False
            End If
            AdminReportList.populateList()
        End Sub

        Private Sub RedirectToForm(ByVal ReportID As String, ByVal ExeType As String)
            'check if we need to insert a form to get input
            Dim aryList As ArrayList
            Dim objRCtrl As New SQLReportController
            Dim DoRedirect As Boolean = False

            aryList = objRCtrl.GetSQLReportParamList(ReportID)
            For Each obj As NB_Store_SQLReportParamInfo In aryList
                If CInt(obj.ParamSource) > 7 Then
                    DoRedirect = True
                End If
            Next

            If DoRedirect Then
                'redirect to display the form
                Response.Redirect(EditUrl("RepID", ReportID, "AdminReport", "frm", ExeType))
            End If

        End Sub


        Private Sub ViewReport(ByVal ReportID As String)
            Dim strSQL As String = "ERROR IN REPORT"
            Try

                Dim objRInfo As NB_Store_SQLReportInfo
                Dim objRCtrl As New SQLReportController

                lblMessage.Text = Services.Localization.Localization.GetString("QueryError", Me.LocalResourceFile)
                pnlResults.Visible = True
                pnlList.Visible = False
                lblMessage.Visible = True



                objRInfo = objRCtrl.GetSQLReport(ReportID)
                If Not objRInfo Is Nothing Then

                    If objRInfo.DisplayInLine Then

                        objRInfo.SQL = objRCtrl.insertParams(ReportID, UserId, PortalId, Request, objRInfo.SQL, GetCurrentCulture)
                        objRInfo.SQL = objRCtrl.insertFormParams(ReportID, objRInfo.SQL, frmReport.XMLData)

                        Dim strXSLOut As String = ""
                        strXSLOut = objRCtrl.runXSL(objRInfo)
                        If strXSLOut <> "" Then
                            If strXSLOut <> "HIDE_XSL_OUTPUT" Then
                                'XSL has produced output so use this
                                lblMessage.Text = strXSLOut
                            Else
                                lblMessage.Text = Localization.GetString("nooutput", LocalResourceFile)
                            End If
                        Else
                            'No XSL output so do SQL report
                            If objRInfo.SQL <> "" Then
                                If objRInfo.ShowSQL Then
                                    lblMessage.Text = objRInfo.SQL & "<br />" & "<hr />"
                                Else
                                    lblMessage.Text = ""
                                End If
                                objRCtrl.popDataGridSQL(objRInfo.SQL, dgResults)
                            End If
                        End If
                    Else
                        'Display in AdminReportPopup
                        If Not frmReport.XMLData Is Nothing Then
                            setStoreCookieValue(PortalId, "SQLFORMREPORT", "XMLDATA", frmReport.XMLData, 0)
                        End If
                        Response.Write("<script>window.open('" & EditUrl("", "", "AdminReportPopup", "REPID=" & ReportID.ToString, "ContainerSrc=" & QueryStringEncode("[G]" & Skins.SkinInfo.RootContainer & "/" & glbHostSkinFolder & "/" & "No Container"), "dnnprintmode=true") & "','_blank')</script>")
                        populateList()
                    End If
                End If
            Catch ex As Exception
                lblMessage.Text = strSQL & "<br />" & "<hr />"
            End Try
        End Sub

        Public Sub ExportToExcel(ByVal ReportID As Integer)
            Dim objRInfo As NB_Store_SQLReportInfo
            Dim objRCtrl As New SQLReportController

            Try

                'GET Data From Database        
                objRInfo = objRCtrl.GetSQLReport(ReportID)
                If Not objRInfo Is Nothing Then
                    'Add Response header 
                    Response.Clear()


                    objRInfo.SQL = objRCtrl.insertParams(ReportID, UserId, PortalId, Request, objRInfo.SQL, GetCurrentCulture)
                    objRInfo.SQL = objRCtrl.insertFormParams(ReportID, objRInfo.SQL, frmReport.XMLData)

                    Dim strXSLOut As String = ""
                    strXSLOut = objRCtrl.runXSL(objRInfo)
                    If strXSLOut <> "" Then
                        Response.AddHeader("content-disposition", "attachment; filename=" & Replace(objRInfo.ReportName, " ", "_"))
                        Response.Charset = ""
                        Response.ContentType = "application/octet-stream"
                        'XSL has produced output so use this
                        If strXSLOut <> "HIDE_XSL_OUTPUT" Then
                            Response.Write(strXSLOut)
                        Else
                            Response.Write(Localization.GetString("nooutput", LocalResourceFile))
                        End If
                        Response.Flush()
                    Else

                        Response.AddHeader("content-disposition", "attachment; filename=" & Replace(objRInfo.ReportName, " ", "_") & ".csv")
                        Response.Charset = ""
                        Response.ContentType = "application/vnd.xls"

                        Dim dr As IDataReader = objRCtrl.ExecuteSQL(objRInfo.SQL)
                        Dim sb As New StringBuilder()
                        '
                        'Add Header
                        '
                        For count As Integer = 0 To dr.FieldCount - 1
                            If dr.GetName(count) IsNot Nothing Then
                                sb.Append(dr.GetName(count))
                            End If
                            If count < dr.FieldCount - 1 Then
                                sb.Append(objRInfo.FieldDelimeter)
                            End If
                        Next
                        Response.Write(sb.ToString() & vbCrLf)
                        Response.Flush()
                        '
                        'Append Data
                        '
                        While dr.Read()
                            sb = New StringBuilder()

                            For col As Integer = 0 To dr.FieldCount - 2
                                If Replace(objRInfo.FieldQualifier, " ", "") = "" Then
                                    If Not dr.IsDBNull(col) Then
                                        sb.Append(dr.GetValue(col).ToString().Replace(objRInfo.FieldDelimeter, " "))
                                    End If
                                Else
                                    sb.Append(objRInfo.FieldQualifier)
                                    sb.Append(dr.GetValue(col).ToString)
                                    sb.Append(objRInfo.FieldQualifier)
                                End If

                                sb.Append(objRInfo.FieldDelimeter)
                            Next
                            If Replace(objRInfo.FieldQualifier, " ", "") = "" Then
                                If Not dr.IsDBNull(dr.FieldCount - 1) Then
                                    sb.Append(dr.GetValue(dr.FieldCount - 1).ToString().Replace(objRInfo.FieldDelimeter, " "))
                                End If
                            Else
                                If Not dr.IsDBNull(dr.FieldCount - 1) Then
                                    sb.Append(objRInfo.FieldQualifier)
                                    sb.Append(dr.GetValue(dr.FieldCount - 1).ToString)
                                    sb.Append(objRInfo.FieldQualifier)
                                End If
                            End If

                            Response.Write(sb.ToString() & vbLf)
                            Response.Flush()
                        End While
                        dr.Dispose()
                    End If
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
            Response.End()
        End Sub


        Private Sub EmailReport(ByVal ReportID As String)
            Try

                Dim objRInfo As NB_Store_SQLReportInfo
                Dim objRCtrl As New SQLReportController

                lblMessage.Text = Services.Localization.Localization.GetString("QueryError", Me.LocalResourceFile)
                pnlResults.Visible = True
                pnlList.Visible = False
                lblMessage.Visible = True

                objRInfo = objRCtrl.GetSQLReport(ReportID)
                If Not objRInfo Is Nothing Then
                    If objRInfo.EmailResults Then
                        objRInfo.SQL = objRCtrl.insertParams(ReportID, UserId, PortalId, Request, objRInfo.SQL, GetCurrentCulture)
                        objRInfo.SQL = objRCtrl.insertFormParams(ReportID, objRInfo.SQL, frmReport.XMLData)

                        Dim strXSLOut As String = ""

                        If objRInfo.ShowSQL Then
                            lblMessage.Text = objRInfo.SQL & "<br />" & "<hr />" & Services.Localization.Localization.GetString("EmailMsg", Me.LocalResourceFile) & objRInfo.EmailTo & "<hr />"
                        Else
                            lblMessage.Text = Services.Localization.Localization.GetString("EmailMsg", Me.LocalResourceFile) & objRInfo.EmailTo & "<hr />"
                        End If

                        Dim dataGridHTML As String = ""


                        strXSLOut = objRCtrl.runXSL(objRInfo)
                        If strXSLOut <> "" Then
                            If strXSLOut <> "HIDE_XSL_OUTPUT" Then
                                'XSL has produced output so use this
                                dataGridHTML = strXSLOut
                            Else
                                dataGridHTML = Localization.GetString("nooutput", LocalResourceFile)
                            End If
                        Else
                            objRCtrl.popDataGridSQL(objRInfo.SQL, dgResults)
                            'Get the rendered HTML
                            Dim SB As New StringBuilder()
                            Dim SW As New StringWriter(SB)
                            Dim htmlTW As New HtmlTextWriter(SW)
                            dgResults.RenderControl(htmlTW)

                            dataGridHTML = SB.ToString()

                            Dim ModuleCSSFile As String = Server.MapPath(StoreInstallPath & "module.css")

                            If File.Exists(ModuleCSSFile) Then
                                Dim objF As New FileObj
                                dataGridHTML = "<style type=""text/css"">" & objF.GetFileContents(ModuleCSSFile) & "</style>" & dataGridHTML
                            End If
                        End If

                        If dataGridHTML <> "" Then
                            dataGridHTML = objRInfo.ReportTitle & "<br/>" & dataGridHTML
                            DotNetNuke.Services.Mail.Mail.SendMail(objRInfo.EmailFrom, objRInfo.EmailTo, "", objRInfo.ReportName, dataGridHTML, "", "HTML", "", "", "", "")
                        End If
                    End If
                End If
            Catch ex As Exception
                lblMessage.Text = ex.ToString & "<hr />"
            End Try
        End Sub

        Private Sub RunByRef(ByVal ReportRef As String)
            Dim objCtrl As New SQLReportController
            Dim objRInfo As NB_Store_SQLReportInfo

            objRInfo = objCtrl.GetSQLReportByRef(PortalId, ReportRef)
            If objRInfo Is Nothing Then
                lblMessage.Text = Services.Localization.Localization.GetString("InvalidReportRef", Me.LocalResourceFile)
            Else
                ViewReport(objRInfo.ReportID)
            End If

        End Sub




#End Region

        Private Sub AdminReportEdit_AddParamButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdminReportEdit.AddParamButton
            Response.Redirect(EditUrl("RepID", RepID, "AdminReport"))
        End Sub


        Private Sub AdminReportEdit_CancelButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdminReportEdit.CancelButton
            Response.Redirect(EditUrl("AdminReport"))
        End Sub

        Private Sub AdminReportEdit_DeleteButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdminReportEdit.DeleteButton
            Response.Redirect(EditUrl("AdminReport"))
        End Sub

        Private Sub AdminReportEdit_UpdateButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdminReportEdit.UpdateButton
            Response.Redirect(EditUrl("AdminReport"))
        End Sub

        Private Sub AdminReportEdit_SearchButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles AdminReportList.SearchButton
            populateList()
        End Sub

        Private Sub cmdFormReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFormReturn.Click
            Response.Redirect(EditUrl("AdminReport"))
        End Sub

        Private Sub cmdResultsReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdResultsReturn.Click
            Response.Redirect(EditUrl("AdminReport"))
        End Sub

        Private Sub cmdFormRun_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdFormRun.Click
            Dim strXML As String = frmReport.XMLData


            If _frm.ToLower = "view" Then
                ViewReport(RepID)
            ElseIf _frm.ToLower = "export" Then
                ExportToExcel(RepID)
            ElseIf _frm.ToLower = "email" Then
                EmailReport(RepID)
            End If


        End Sub

    End Class

End Namespace
