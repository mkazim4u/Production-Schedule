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
Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports System.IO

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The AdminReportEdit class is used to manage content
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------

    Partial Class AdminReportEdit
        Inherits Framework.UserControlBase

#Region "Private Members"
        Protected _RepID As Integer = -1
#End Region

        Public Event DeleteButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event UpdateButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event CancelButton(ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event AddParamButton(ByVal sender As Object, ByVal e As System.EventArgs)

        Public Property RepID() As Integer
            Get
                Return _RepID
            End Get
            Set(ByVal value As Integer)
                _RepID = value
            End Set
        End Property

#Region "Event Handlers"

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                RaiseEvent CancelButton(sender, e)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                SaveReport(_RepID)
                ' Redirect back to the portal home page
                RaiseEvent UpdateButton(sender, e)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try
                ' Only attempt to delete the item if it exists already7

                If _RepID >= 0 Then
                    Dim objSQLReports As New SQLReportController
                    objSQLReports.DeleteSQLReport(_RepID)
                End If

                ' Redirect back to the portal home page
                RaiseEvent DeleteButton(sender, e)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdAddParam_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAddParam.Click
            Try
                Dim objCtrl As New SQLReportController
                Dim objInfo As NB_Store_SQLReportParamInfo
                Dim lp As Integer

                _RepID = SaveReport(_RepID)

                If IsNumeric(txtAddParams.Text) Then
                    For lp = 1 To CInt(txtAddParams.Text)
                        objInfo = New NB_Store_SQLReportParamInfo
                        objInfo.ParamName = ""
                        objInfo.ParamType = "nvarchar(50)"
                        objInfo.ParamValue = ""
                        objInfo.ReportID = _RepID
                        objInfo.ReportParamID = -1
                        objInfo.ParamSource = 1
                        objCtrl.UpdateObjSQLReportParam(objInfo)
                    Next

                    RaiseEvent AddParamButton(sender, e)

                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub




        Private Sub dgParam_DeleteCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataGridCommandEventArgs) Handles dgParam.DeleteCommand
            Try

                Dim ItemId As Integer = Int32.Parse(e.CommandArgument.ToString)
                Dim objCtrl As New SQLReportController
                objCtrl.DeleteSQLReportParam(ItemId)
                PopulateParams(RepID)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub



        Public Sub populateEdit(ByVal ReportID As Integer)
            Dim objCtrl As New SQLReportController
            Dim objRinfo As NB_Store_SQLReportInfo

            objRinfo = objCtrl.GetSQLReport(ReportID)
            If Not objRinfo Is Nothing Then
                chkAllowDisplay.Checked = objRinfo.AllowDisplay
                chkAllowExport.Checked = objRinfo.AllowExport
                chkDisplayInLine.Checked = objRinfo.DisplayInLine
                txtEmailFrom.Text = objRinfo.EmailFrom
                chkEmailResults.Checked = objRinfo.EmailResults
                txtEmailTo.Text = objRinfo.EmailTo
                txtReportName.Text = objRinfo.ReportName
                chkSchedulerFlag.Checked = objRinfo.SchedulerFlag
                ddlSchReRunMins.SelectedValue = objRinfo.SchReRunMins
                ddlSchStartHour.SelectedValue = objRinfo.SchStartHour
                ddlSchStartMins.SelectedValue = objRinfo.SchStartMins
                txtSQLText.Text = objRinfo.SQL
                chkShowSQL.Checked = objRinfo.ShowSQL

                If objRinfo.ReportRef = "" Then
                    txtReportRef.Text = "R" & objRinfo.ReportID.ToString
                Else
                    txtReportRef.Text = objRinfo.ReportRef
                End If

                txtReportTitle.Text = objRinfo.ReportTitle
                txtFieldDelimeter.Text = objRinfo.FieldDelimeter
                If txtFieldDelimeter.Text = "" Then txtFieldDelimeter.Text = ","
                txtFieldQualifier.Text = objRinfo.FieldQualifier

                PopulateParams(ReportID)


            Else
                cmdDelete.Visible = False
            End If


        End Sub

        Private Function SaveReport(ByVal ReportID As Integer) As Integer
            Dim objCtrl As New SQLReportController
            Dim objRinfo As NB_Store_SQLReportInfo



            Dim rtnRepID As Integer


            objRinfo = objCtrl.GetSQLReport(ReportID)
            If objRinfo Is Nothing Then
                objRinfo = New NB_Store_SQLReportInfo
            End If

            objRinfo.AllowDisplay = chkAllowDisplay.Checked
            objRinfo.AllowExport = chkAllowExport.Checked
            objRinfo.DisplayInLine = chkDisplayInLine.Checked
            objRinfo.EmailFrom = txtEmailFrom.Text
            objRinfo.EmailResults = chkEmailResults.Checked
            objRinfo.EmailTo = txtEmailTo.Text
            objRinfo.LastRunTime = Today
            objRinfo.PortalID = PortalSettings.PortalId
            objRinfo.ReportID = ReportID
            objRinfo.ReportName = txtReportName.Text
            objRinfo.SchedulerFlag = chkSchedulerFlag.Checked
            objRinfo.SchReRunMins = ddlSchReRunMins.SelectedValue
            objRinfo.SchStartHour = ddlSchStartHour.SelectedValue
            objRinfo.SchStartMins = ddlSchStartMins.SelectedValue
            objRinfo.SQL = txtSQLText.Text
            objRinfo.ShowSQL = chkShowSQL.Checked
            objRinfo.AllowPaging = False
            objRinfo.ReportRef = txtReportRef.Text
            objRinfo.ReportTitle = txtReportTitle.Text
            objRinfo.FieldDelimeter = txtFieldDelimeter.Text
            If objRinfo.FieldDelimeter = "" Then objRinfo.FieldDelimeter = ","
            objRinfo.FieldQualifier = txtFieldQualifier.Text
            rtnRepID = objCtrl.UpdateObjSQLReport(objRinfo)


            'save params
            updateParams()



            Return rtnRepID
        End Function

        Private Sub updateParams()
            Dim objCtrl As New SQLReportController
            Dim objInfo As NB_Store_SQLReportParamInfo
            Dim i As DataGridItem
            Dim ParamID As Integer

            For Each i In dgParam.Items
                ParamID = CInt(i.Cells(0).Text)
                objInfo = objCtrl.GetSQLReportParam(ParamID)
                If Not objInfo Is Nothing Then
                    objInfo.ParamName = CType(i.FindControl("txtParamName"), TextBox).Text
                    objInfo.ParamSource = CType(i.FindControl("ddlSource"), DropDownList).SelectedValue
                    objInfo.ParamType = CType(i.FindControl("ddlType"), DropDownList).SelectedValue
                    objInfo.ParamValue = CType(i.FindControl("txtParamValue"), TextBox).Text
                    objCtrl.UpdateObjSQLReportParam(objInfo)
                End If
            Next
        End Sub

        Private Sub PopulateParams(ByVal ReportID As Integer)
            Dim objCtrl As New SQLReportController
            Dim aryList As ArrayList

            aryList = objCtrl.GetSQLReportParamList(ReportID)

            dgParam.DataSource = aryList
            dgParam.DataBind()

        End Sub



#End Region

    End Class

End Namespace