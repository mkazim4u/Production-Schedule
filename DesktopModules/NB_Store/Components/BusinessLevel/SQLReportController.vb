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

Imports System
Imports System.Configuration
Imports System.Data
Imports System.Xml
Imports System.Web
Imports System.Collections.Generic

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Services.Search
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.IO

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The Controller class for AdminReport
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Public Class SQLReportController


#Region "SQLReport Public Methods"

        Public Sub DeleteSQLReport(ByVal ReportID As Integer)
            DataProvider.Instance().DeleteSQLReport(ReportID)
        End Sub

        Public Function GetSQLReport(ByVal ReportID As Integer) As NB_Store_SQLReportInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetSQLReport(ReportID), GetType(NB_Store_SQLReportInfo)), NB_Store_SQLReportInfo)
        End Function

        Public Function GetSQLReportByRef(ByVal PortalID As Integer, ByVal ReportRef As String) As NB_Store_SQLReportInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetSQLReportByRef(PortalID, ReportRef), GetType(NB_Store_SQLReportInfo)), NB_Store_SQLReportInfo)
        End Function

        Public Function GetSQLAdminReportList(ByVal PortalID As Integer, ByVal IsEditable As Boolean, ByVal SearchText As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetSQLAdminReportList(PortalID, IsEditable, SearchText), GetType(NB_Store_SQLReportInfo))
        End Function


        Public Function UpdateObjSQLReport(ByVal objInfo As NB_Store_SQLReportInfo) As Integer
            Return DataProvider.Instance().UpdateSQLReport(objInfo.ReportID, objInfo.PortalID, objInfo.ReportName, objInfo.SQL, objInfo.SchedulerFlag, objInfo.SchStartHour, objInfo.SchStartMins, objInfo.SchReRunMins, objInfo.LastRunTime, objInfo.AllowExport, objInfo.AllowDisplay, objInfo.DisplayInLine, objInfo.EmailResults, objInfo.EmailFrom, objInfo.EmailTo, objInfo.ShowSQL, objInfo.ConnectionString, objInfo.ReportRef, objInfo.AllowPaging, objInfo.ReportTitle, objInfo.FieldDelimeter, objInfo.FieldQualifier)
        End Function

        Public Function ExecuteSQLReportXml(ByVal SQLcommand As String) As String
            Return DataProvider.Instance().ExecuteSQLReportXml(SQLcommand)
        End Function

        Public Sub popDataGridSQL(ByVal SQLcommand As String, ByVal GridView As DataGrid)
            DataProvider.Instance().popDataGridSQL(SQLcommand, GridView)
        End Sub

        Public Function ExecuteSQLReportText(ByVal SQLcommand As String, ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal ExportHeader As Boolean) As String
            Return DataProvider.Instance().ExecuteSQLReportText(SQLcommand, FieldDelimeter, FieldQualifier, ExportHeader)
        End Function


        Public Sub CopyReport(ByVal ReportID As Integer)
            Dim objRInfo As NB_Store_SQLReportInfo
            Dim NewReportID As Integer

            objRInfo = GetSQLReport(ReportID)
            objRInfo.ReportID = -1
            objRInfo.ReportName = objRInfo.ReportName & " [Copy]"
            NewReportID = UpdateObjSQLReport(objRInfo)

            'Copy params
            Dim aryList As ArrayList
            Dim objPInfo As NB_Store_SQLReportParamInfo
            aryList = GetSQLReportParamList(ReportID)
            For Each objPInfo In aryList
                objPInfo.ReportID = NewReportID
                objPInfo.ReportParamID = -1
                UpdateObjSQLReportParam(objPInfo)
            Next

            'Copy xsl
            Dim objXInfo As NB_Store_SQLReportXSLInfo
            aryList = GetSQLReportXSLList(ReportID)
            For Each objXInfo In aryList
                objXInfo.ReportID = NewReportID
                objXInfo.ReportXSLID = -1
                UpdateObjSQLReportXSL(objXInfo)
            Next

        End Sub

#End Region

#Region "SQLReportParam Public Methods"

        Public Sub DeleteSQLReportParam(ByVal ReportParamID As Integer)
            DataProvider.Instance().DeleteSQLReportParam(ReportParamID)
        End Sub

        Public Function GetSQLReportParam(ByVal ReportParamID As Integer) As NB_Store_SQLReportParamInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetSQLReportParam(ReportParamID), GetType(NB_Store_SQLReportParamInfo)), NB_Store_SQLReportParamInfo)
        End Function

        Public Function GetSQLReportParamList(ByVal ReportID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetSQLReportParamList(ReportID), GetType(NB_Store_SQLReportParamInfo))
        End Function

        Public Sub UpdateObjSQLReportParam(ByVal objInfo As NB_Store_SQLReportParamInfo)
            DataProvider.Instance().UpdateSQLReportParam(objInfo.ReportParamID, objInfo.ReportID, objInfo.ParamName, objInfo.ParamType, objInfo.ParamValue, objInfo.ParamSource)
        End Sub

        Public Function ExecuteSQL(ByVal SQLcommand As String) As IDataReader
            Return DataProvider.Instance().ExecuteSQL(SQLcommand)
        End Function

#End Region


#Region "SQLReportXSL Public Methods"

        Public Sub DeleteSQLReportXSL(ByVal ReportXSLID As Integer)
            DataProvider.Instance().DeleteSQLReportXSL(ReportXSLID)
        End Sub

        Public Function GetSQLReportXSL(ByVal ReportXSLID As Integer) As NB_Store_SQLReportXSLInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetSQLReportXSL(ReportXSLID), GetType(NB_Store_SQLReportXSLInfo)), NB_Store_SQLReportXSLInfo)
        End Function

        Public Function GetSQLReportXSLList(ByVal ReportID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetSQLReportXSLList(ReportID), GetType(NB_Store_SQLReportXSLInfo))
        End Function

        Public Sub UpdateSQLReportXSL(ByVal ReportXSLID As Integer, ByVal ReportID As Integer, ByVal XMLInput As String, ByVal XSLFile As String, ByVal OutputFile As String, ByVal DisplayResults As Boolean, ByVal SortOrder As Integer)
            DataProvider.Instance().UpdateSQLReportXSL(ReportXSLID, ReportID, XMLInput, XSLFile, OutputFile, DisplayResults, SortOrder)
        End Sub

        Public Sub UpdateObjSQLReportXSL(ByVal objInfo As NB_Store_SQLReportXSLInfo)
            DataProvider.Instance().UpdateSQLReportXSL(objInfo.ReportXSLID, objInfo.ReportID, objInfo.XMLInput, objInfo.XSLFile, objInfo.OutputFile, objInfo.DisplayResults, objInfo.SortOrder)
        End Sub

#End Region

#Region "SQLReport Execution Methods"

        Public Function runXSL(ByVal objRInfo As NB_Store_SQLReportInfo) As String
            Dim objRXInfo As NB_Store_SQLReportXSLInfo
            Dim strReportOUT As String = ""
            Dim aryList As ArrayList
            aryList = GetSQLReportXSLList(objRInfo.ReportID)
            If aryList.Count > 0 Then
                For Each objRXInfo In aryList
                    If objRXInfo.XMLInput = "" Then
                        strReportOUT = XSLTransInMemory(ExecuteSQLReportXml(objRInfo.SQL), objRXInfo.XSLFile)
                    Else
                        strReportOUT = XSLTransInMemory(objRXInfo.XMLInput, objRXInfo.XSLFile)
                    End If
                Next
                If strReportOUT = "" Then strReportOUT = "HIDE_XSL_OUTPUT"
            End If
            Return strReportOUT
        End Function

        Public Function insertParams(ByVal ReportID As Integer, ByVal UserId As Integer, ByVal PortalId As Integer, ByVal Request As System.Web.HttpRequest, ByVal strSQL As String, ByVal Lang As String) As String
            Dim objCtrl As New SQLReportController
            Dim objInfo As NB_Store_SQLReportParamInfo
            Dim aryList As ArrayList
            Dim strParam As String

            aryList = objCtrl.GetSQLReportParamList(ReportID)

            strSQL = "  " & strSQL
            For Each objInfo In aryList
                If Not objInfo.ParamName.StartsWith("@") Then
                    objInfo.ParamName = "@" & objInfo.ParamName
                End If
                strParam = ""
                If objInfo.ParamSource < 8 Then ' source 8 and above is form controls
                    strParam &= " declare " & objInfo.ParamName & " " & objInfo.ParamType & " "
                    Select Case objInfo.ParamSource
                        Case 1
                            strParam &= " set " & objInfo.ParamName & " = '" & objInfo.ParamValue & "' "
                        Case 2
                            If Not Request Is Nothing Then
                                If Not (Request.QueryString(objInfo.ParamValue) Is Nothing) Then
                                    strParam &= " set " & objInfo.ParamName & " = '" & Request.QueryString(objInfo.ParamValue) & "' "
                                End If
                            End If
                        Case 3
                            If Not Request Is Nothing Then
                                If Not (Request.Form(objInfo.ParamValue) Is Nothing) Then
                                    strParam &= " set " & objInfo.ParamName & " = '" & Request.Form(objInfo.ParamValue) & "' "
                                End If
                            End If
                        Case 4
                            strParam &= " set " & objInfo.ParamName & " = getdate() "
                        Case 5
                            strParam &= " set " & objInfo.ParamName & " = '" & PortalId & "' "
                        Case 6
                            strParam &= " set " & objInfo.ParamName & " = '" & UserId & "' "
                        Case 7
                            strParam &= " set " & objInfo.ParamName & " = '" & Lang & "' "
                    End Select
                End If

                strSQL = strParam & " " & strSQL

            Next

            Return strSQL
        End Function


        Public Function insertFormParams(ByVal ReportID As Integer, ByVal strSQL As String, ByVal FormXMLData As String) As String
            Dim objCtrl As New SQLReportController
            Dim objInfo As NB_Store_SQLReportParamInfo
            Dim aryList As ArrayList
            Dim strParam As String
            Dim strParamName As String = ""
            aryList = objCtrl.GetSQLReportParamList(ReportID)

            strSQL = "  " & strSQL
            For Each objInfo In aryList
                strParamName = Replace(objInfo.ParamName, "@", "")
                If Not objInfo.ParamName.StartsWith("@") Then
                    objInfo.ParamName = "@" & objInfo.ParamName
                End If
                strParam = ""
                If objInfo.ParamSource >= 8 Then ' source 8 and above is form controls
                    strParam &= " declare " & objInfo.ParamName & " " & objInfo.ParamType & " "
                    Select Case objInfo.ParamSource
                        Case 8
                            strParam &= " set " & objInfo.ParamName & " = '" & getGenXMLvalue(FormXMLData, "genxml/textbox/" & strParamName) & "' "
                        Case 9
                            Dim strDate As String = getGenXMLvalue(FormXMLData, "genxml/textbox/" & strParamName)
                            If IsDate(strDate) Then
                                If objInfo.ParamType.ToLower = "datetime" Then
                                    strParam &= " set " & objInfo.ParamName & " = convert(datetime,'" & CDate(strDate).Year.ToString & "-" & CDate(strDate).Month.ToString("00") & "-" & CDate(strDate).Day.ToString("00") & "') "
                                Else
                                    strParam &= " set " & objInfo.ParamName & " = '" & CDate(strDate).Year.ToString & "-" & CDate(strDate).Month.ToString("00") & "-" & CDate(strDate).Day.ToString("00") & "' "
                                End If
                            End If
                        Case 10
                            strParam &= " set " & objInfo.ParamName & " = '" & getGenXMLvalue(FormXMLData, "genxml/dropdownlist/" & strParamName) & "' "
                        Case 11
                            strParam &= " set " & objInfo.ParamName & " = '" & getGenXMLvalue(FormXMLData, "genxml/radiobuttonlist/" & strParamName) & "' "
                        Case 12
                            If getGenXMLvalue(FormXMLData, "genxml/checkbox/" & strParamName).ToLower = "true" Then
                                strParam &= " set " & objInfo.ParamName & " = '1' "
                            Else
                                strParam &= " set " & objInfo.ParamName & " = '0' "
                            End If
                    End Select
                End If

                strSQL = strParam & " " & strSQL

            Next

            Return strSQL
        End Function

        Public Function GetReportOutput(ByVal PortalID As Integer, ByVal RepRef As String) As String
            Dim objRInfo As NB_Store_SQLReportInfo
            objRInfo = GetSQLReportByRef(PortalID, RepRef)
            Return GetReportOutput(objRInfo)
        End Function

        Public Function GetReportOutput(ByVal PortalID As Integer, ByVal RepID As Integer) As String
            Dim objRInfo As NB_Store_SQLReportInfo
            objRInfo = GetSQLReportByRef(PortalID, RepID)
            Return GetReportOutput(objRInfo)
        End Function

        Public Function GetReportOutput(ByVal objRinfo As NB_Store_SQLReportInfo) As String
            Dim dataGridHTML As String = ""
            If Not objRinfo Is Nothing Then

                objRinfo.SQL = insertParams(objRinfo.ReportID, -1, objRinfo.PortalID, Nothing, objRinfo.SQL, GetCurrentCulture)
                Dim strXSLOut As String = ""
                Dim dgResults As New System.Web.UI.WebControls.DataGrid

                strXSLOut = runXSL(objRinfo)
                If strXSLOut <> "" Then
                    If strXSLOut <> "HIDE_XSL_OUTPUT" Then
                        'XSL has produced output so use this
                        dataGridHTML = strXSLOut
                    End If
                Else
                    If objRinfo.SQL <> "" Then
                        popDataGridSQL(objRinfo.SQL, dgResults)
                        'Get the rendered HTML
                        Dim SB As New StringBuilder()
                        Dim SW As New StringWriter(SB)
                        Dim htmlTW As New HtmlTextWriter(SW)

                        dgResults.GridLines = GridLines.None
                        dgResults.Width = System.Web.UI.WebControls.Unit.Percentage(100)
                        dgResults.HeaderStyle.CssClass = "NBright_HeaderStyle"
                        dgResults.FooterStyle.CssClass = "NBright_FooterStyle"
                        dgResults.EditItemStyle.CssClass = "NBright_EditItemStyle"
                        dgResults.SelectedItemStyle.CssClass = "NBright_SelectedItemStyle"
                        dgResults.AlternatingItemStyle.CssClass = "NBright_AlternatingItemStyle"
                        dgResults.ItemStyle.CssClass = "NBright_ItemStyle"
                        dgResults.AllowPaging = False

                        dgResults.RenderControl(htmlTW)

                        dataGridHTML = SB.ToString()

                    End If
                End If
            End If
            Return dataGridHTML
        End Function

        Public Function AddStoreCSSforEmailHTML(ByVal dataHTML As String) As String
            ' this functions adds the NB_Store CSS for use on email output.
            Dim ModuleCSSFile As String = System.Web.Hosting.HostingEnvironment.MapPath("\desktopmodules\NB_Store\module.css")

            If File.Exists(ModuleCSSFile) Then
                Dim objF As New FileObj
                dataHTML = "<style type=""text/css"">" & objF.GetFileContents(ModuleCSSFile) & "</style>" & dataHTML
            End If
            Return dataHTML
        End Function

#End Region


        Public Function BuildReportInputForm(ByVal reportID As String) As String
            Dim rtnSTR As String
            Dim objBuild As New FormTemplateBuilder
            Dim arylist As ArrayList
            Dim objR As NB_Store_SQLReportInfo

            objR = GetSQLReport(reportID)

            objBuild.AddTitle(objR.ReportName)

            arylist = GetSQLReportParamList(reportID)

            Dim ctrlid As String = ""
            For Each objP As NB_Store_SQLReportParamInfo In arylist
                ctrlid = Replace(objP.ParamName, "@", "")
                Select Case objP.ParamSource
                    Case 8
                        Dim txt As New NBright_TextBox
                        txt.Text = objP.ParamValue
                        objBuild.AddControl(ctrlid, txt, Replace(objP.ParamName, "@", "") & " : ")
                    Case 9
                        Dim de As New NBright_dateEditControl
                        de.Text = objP.ParamValue
                        objBuild.AddControl(ctrlid, de, Replace(objP.ParamName, "@", "") & " : ")
                    Case 10
                        Dim ddl As New NBright_DropDownList
                        ddl.data = objP.ParamValue
                        ddl.datavalue = objP.ParamValue
                        objBuild.AddControl(ctrlid, ddl, Replace(objP.ParamName, "@", "") & " : ")
                    Case 11
                        Dim rbl As New NBright_RadioButtonList
                        rbl.data = objP.ParamValue
                        rbl.datavalue = objP.ParamValue
                        objBuild.AddControl(ctrlid, rbl, Replace(objP.ParamName, "@", "") & " : ")
                    Case 12
                        Dim chk As New NBright_CheckBox
                        If objP.ParamValue <> "" Then
                            Try
                                chk.Checked = CBool(objP.ParamValue)
                            Catch ex As Exception
                            End Try
                        End If
                        objBuild.AddControl(ctrlid, chk, Replace(objP.ParamName, "@", "") & " : ")
                End Select

            Next



            rtnSTR = objBuild.GetFormTemplate()

            Return rtnSTR
        End Function

    End Class

    Public Class StatsController

#Region "NB_Store_SearchWordHits Public Methods"

        Public Sub ProcessSearchWordHits(ByVal PortalID As Integer)
            DataProvider.Instance().ProcessSearchWords(PortalID)
        End Sub

        Public Sub PurgeSearchWordHits(ByVal PortalID As Integer, ByVal PurgeBeforeDate As Date)
            DataProvider.Instance().PurgeSearchWord(PortalID, PurgeBeforeDate)
        End Sub

        Public Sub UpdateSearchWordHits(ByVal PortalID As Integer, ByVal SearchWord As String, ByVal WordPosition As Integer)
            DataProvider.Instance().UpdateSearchWord(PortalID, SearchWord, WordPosition)
        End Sub

#End Region
    End Class

End Namespace

