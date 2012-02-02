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

Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class AdminReportPopup
        Inherits BaseModule

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim objRInfo As NB_Store_SQLReportInfo
                Dim objRCtrl As New SQLReportController

                lblMessage.Text = Services.Localization.Localization.GetString("QueryError", Me.LocalResourceFile)

                If Request.QueryString("REPID") <> "" Then
                    If IsNumeric(Request.QueryString("REPID").ToString) Then
                        Dim ReportID As Integer = CInt(Request.QueryString("REPID").ToString)
                        'TODO: check if user can view this report
                        objRInfo = objRCtrl.GetSQLReport(ReportID)
                        If Not objRInfo Is Nothing Then
                            If objRInfo.SQL <> "" Then
                                lblMessage.Text = ""
                                objRInfo.SQL = objRCtrl.insertParams(ReportID, UserId, PortalId, Request, objRInfo.SQL, GetCurrentCulture)
                                Dim strXML As String = getStoreCookieValue(PortalId, "SQLFORMREPORT", "XMLDATA")
                                objRInfo.SQL = objRCtrl.insertFormParams(ReportID, objRInfo.SQL, strXML)

                                Dim strXSLOut As String = ""
                                strXSLOut = objRCtrl.runXSL(objRInfo)
                                If strXSLOut <> "" Then
                                    If strXSLOut <> "HIDE_XSL_OUTPUT" Then
                                        'XSL has produced output so use this
                                        lblMessage.Text &= strXSLOut
                                    End If
                                Else
                                    If objRInfo.ShowSQL Then
                                        lblMessage.Text = objRInfo.SQL & "<br />" & "<hr />"
                                    End If
                                    objRCtrl.popDataGridSQL(objRInfo.SQL, dgResults)
                                End If
                            End If
                        End If
                    End If
                End If
            Catch ex As Exception
                lblMessage.Text = ex.ToString & "<hr />"
            End Try

        End Sub

    End Class

End Namespace
