' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. BSD License.
' Author: D.C.Lee
' ------------------------------------------------------------------------
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' ------------------------------------------------------------------------
' This copyright notice may NOT be removed, obscured or modified without written consent from the author.
' --- End copyright notice --- 


Imports System.Reflection
Imports DotNetNuke.Services.Scheduling
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.IO
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities
Imports DotNetNuke.UI.Skins
Imports DotNetNuke.Entities.Tabs

Namespace NEvoWeb.Modules.NB_Store

    Public Class CleanUpCarts
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem
        End Sub

        Public Overrides Sub DoWork()
            Try

                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL

                CleanUpCarts()

                'update the result to success since no exception was thrown
                Me.ScheduleHistoryItem.Succeeded = True
                Me.ScheduleHistoryItem.AddLogNote("Store Clean Up Carts Completed.")

            Catch exc As Exception
                Me.ScheduleHistoryItem.Succeeded = False
                Me.ScheduleHistoryItem.AddLogNote("Store Clean Up Carts failed." + exc.ToString)
                Me.ScheduleHistoryItem.Succeeded = False

                'notification that we have errored
                Me.Errored(exc)

                'log the exception
                LogException(exc)
            End Try
        End Sub

        Private Sub CleanUpCarts()
            Dim objCtrl As New CartController
            Dim OrderMins As Integer = 2880
            Dim CartMins As Integer = 10080
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim objPC As New DotNetNuke.Entities.Portals.PortalController
            Dim aryList As ArrayList
            Dim objPInfo As DotNetNuke.Entities.Portals.PortalInfo

            aryList = objPC.GetPortals()
            For Each objPInfo In aryList
                If GetStoreSetting(objPInfo.PortalID, "version", "None") <> "" Then ' portal may not have a store.

                    objSInfo = objSCtrl.GetSetting(objPInfo.PortalID, "purgecartmins", GetCurrentCulture)
                    If Not objSInfo Is Nothing Then
                        CartMins = CInt(objSInfo.SettingValue)
                    End If

                    objSInfo = objSCtrl.GetSetting(objPInfo.PortalID, "purgeordermins", GetCurrentCulture)
                    If Not objSInfo Is Nothing Then
                        OrderMins = CInt(objSInfo.SettingValue)
                    End If

                    objCtrl.DeleteOldCarts(objPInfo.PortalID, CartMins, OrderMins)

                End If
            Next

        End Sub

    End Class


    Public Class StoreOrderReport
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem
        End Sub

        Public Overrides Sub DoWork()
            Try

                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL

                DoReport()

                'update the result to success since no exception was thrown
                Me.ScheduleHistoryItem.Succeeded = True
                Me.ScheduleHistoryItem.AddLogNote("Store Reports Completed.")

            Catch exc As Exception
                Me.ScheduleHistoryItem.Succeeded = False
                Me.ScheduleHistoryItem.AddLogNote("Store Reports failed." + exc.ToString)
                Me.ScheduleHistoryItem.Succeeded = False

                'notification that we have errored
                Me.Errored(exc)

                'log the exception
                LogException(exc)
            End Try
        End Sub


        Private Sub DoReport()
            Dim objSCtrl As New SettingsController
            Dim objPC As New DotNetNuke.Entities.Portals.PortalController
            Dim aryList As ArrayList
            Dim objPInfo As DotNetNuke.Entities.Portals.PortalInfo
            Dim objSInfoXSL As NB_Store_SettingsInfo
            Dim objSInfo As NB_Store_SettingsInfo
            Dim strReport As String = ""
            Dim SubjectText As String = "Orders Report"
            Dim strXSLFilePath As String = ""
            Dim LastRunDay As String

            aryList = objPC.GetPortals()
            For Each objPInfo In aryList
                If GetStoreSetting(objPInfo.PortalID, "version", "None") <> "" Then ' portal may not have a store.
                    LastRunDay = GetStoreSetting(objPInfo.PortalID, "orderreport.lastday", "None")
                    If LastRunDay <> Day(Today).ToString Then

                        'clean up reports
                        strReport = GetReportXML(objPInfo.PortalID, GetCurrentCulture)
                        objSInfoXSL = objSCtrl.GetSetting(objPInfo.PortalID, "ordersreport.xsl", GetMerchantCulture(objPInfo.PortalID))
                        If Not objSInfoXSL Is Nothing Then
                            strXSLFilePath = System.Web.Hosting.HostingEnvironment.MapPath(objSInfoXSL.SettingValue)
                            If File.Exists(strXSLFilePath) Then
                                strReport = XSLTrans(strReport, strXSLFilePath)
                            Else
                                strReport = "ERROR Producing Orders Report. Unable to find xsl required. (" & objSInfoXSL.SettingValue & ")"
                            End If
                        Else
                            strReport = "ERROR Producing Orders Report. Unable to find setting 'ordersreport.xsl'."
                        End If

                        objSInfo = objSCtrl.GetSetting(objPInfo.PortalID, "ordersreport.subject", GetMerchantCulture(objPInfo.PortalID))
                        If Not objSInfo Is Nothing Then
                            SubjectText = System.Web.HttpUtility.HtmlDecode(objSInfo.SettingValue)
                        End If
                        'send email
                        If GetStoreEmail(objPInfo.PortalID) <> "" Then
                            DotNetNuke.Services.Mail.Mail.SendMail(GetStoreEmail(objPInfo.PortalID), GetMerchantEmail(objPInfo.PortalID), "", SubjectText, strReport, "", "HTML", "", "", "", "")
                        End If
                        SetStoreSetting(objPInfo.PortalID, "orderreport.lastday", Day(Today).ToString, "None", True)
                    End If
                End If
            Next



        End Sub

        Private Function GetReportXML(ByVal PortalID As Integer, ByVal Lang As String) As String
            Dim objOCtrl As New OrderController
            Dim objSTAInfo As NB_Store_OrderStatusInfo
            Dim aryList As ArrayList
            Dim strXML = "<root>"

            aryList = objOCtrl.GetOrderStatusList(Lang)
            If aryList.Count = 0 Then
                'get default orderstatus
                aryList = objOCtrl.GetOrderStatusList("XX")
            End If

            For Each objSTAInfo In aryList
                If objSTAInfo.OrderStatusID <> 70 Then
                    strXML &= getOrderXML(PortalID, objSTAInfo.OrderStatusID)
                End If
            Next
            strXML &= "</root>"
            Return strXML
        End Function

        Private Function getOrderXML(ByVal PortalID As Integer, ByVal OrderStatus As Integer) As String
            Dim strXML As String = ""
            Dim objCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo
            Dim aryList As ArrayList

            aryList = objCtrl.GetOrderList(PortalID, -1, DateAdd(DateInterval.Year, -2, Today), Today, OrderStatus, "")

            If aryList.Count = 0 Then
                strXML &= ""
            Else
                For Each objOInfo In aryList
                    strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objOInfo)
                Next
            End If

            Return strXML
        End Function


    End Class


    Public Class CreateSalePrices
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem
        End Sub

        Public Overrides Sub DoWork()
            Try

                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL

                DoSalePrices()

                'update the result to success since no exception was thrown
                Me.ScheduleHistoryItem.Succeeded = True
                Me.ScheduleHistoryItem.AddLogNote("Sale Prices Calculation Completed.")

            Catch exc As Exception
                Me.ScheduleHistoryItem.Succeeded = False
                Me.ScheduleHistoryItem.AddLogNote("Sale Prices Calculation  failed." + exc.ToString)
                Me.ScheduleHistoryItem.Succeeded = False

                'notification that we have errored
                Me.Errored(exc)

                'log the exception
                LogException(exc)
            End Try
        End Sub

        Private Sub DoSalePrices()
            Dim objPC As New DotNetNuke.Entities.Portals.PortalController
            Dim aryList As ArrayList
            Dim objPInfo As DotNetNuke.Entities.Portals.PortalInfo
            Dim LastRunDay As String
            Dim objPromoCtrl As New PromoController

            aryList = objPC.GetPortals()
            For Each objPInfo In aryList
                If GetStoreSetting(objPInfo.PortalID, "version", "None") <> "" Then ' portal may not have a store.
                    LastRunDay = GetStoreSetting(objPInfo.PortalID, "salerates.lastday", "None")
                    If LastRunDay <> Day(Today).ToString Then
                        objPromoCtrl.createSalePriceTable(objPInfo.PortalID)
                        SetStoreSetting(objPInfo.PortalID, "salerates.lastday", Day(Today).ToString, "None", True)
                    End If
                End If
            Next

        End Sub

    End Class

    Public Class SchedulerReport
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem
        End Sub

        Public Overrides Sub DoWork()
            Try

                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL

                DoReport()

                'update the result to success since no exception was thrown
                Me.ScheduleHistoryItem.Succeeded = True
                Me.ScheduleHistoryItem.AddLogNote("Store Reports Completed.")

            Catch exc As Exception
                Me.ScheduleHistoryItem.Succeeded = False
                Me.ScheduleHistoryItem.AddLogNote("Store Reports failed." + exc.ToString)
                Me.ScheduleHistoryItem.Succeeded = False

                'notification that we have errored
                Me.Errored(exc)

                'log the exception
                LogException(exc)
            End Try
        End Sub


        Private Sub DoReport()
            Dim objPC As New DotNetNuke.Entities.Portals.PortalController
            Dim aryList As ArrayList
            Dim objPInfo As DotNetNuke.Entities.Portals.PortalInfo
            Dim strReport As String = ""
            Dim strXSLFilePath As String = ""
            Dim objRCtrl As New SQLReportController
            Dim objRInfo As NB_Store_SQLReportInfo
            Dim aryRepList As ArrayList

            aryList = objPC.GetPortals()
            For Each objPInfo In aryList
                If GetStoreSetting(objPInfo.PortalID, "version", "None") <> "" Then ' portal may not have a store.

                    aryRepList = objRCtrl.GetSQLAdminReportList(objPInfo.PortalID, True, "")

                    For Each objRInfo In aryRepList
                        If objRInfo.SchedulerFlag Then
                            If Now > calcNextRunTime(objRInfo) Then

                                RunReport(objPInfo, objRInfo)

                                objRInfo.LastRunTime = Now
                                objRCtrl.UpdateObjSQLReport(objRInfo)
                            End If
                        End If
                    Next

                End If
            Next
        End Sub

        Private Sub RunReport(ByVal objPInfo As DotNetNuke.Entities.Portals.PortalInfo, ByVal objRinfo As NB_Store_SQLReportInfo)

            Dim objRCtrl As New SQLReportController

            If Not objRinfo Is Nothing Then

                objRinfo.SQL = objRCtrl.insertParams(objRinfo.ReportID, -1, objPInfo.PortalID, Nothing, objRinfo.SQL, GetCurrentCulture)
                Dim strXSLOut As String = ""
                Dim dgResults As New System.Web.UI.WebControls.DataGrid
                Dim dataGridHTML As String = ""

                dataGridHTML = objRCtrl.GetReportOutput(objRinfo)

                If objRinfo.EmailResults And dataGridHTML <> "" And objRinfo.EmailFrom <> "" And objRinfo.EmailTo <> "" Then
                    dataGridHTML = objRCtrl.AddStoreCSSforEmailHTML(dataGridHTML)
                    dataGridHTML = objRinfo.ReportTitle & "<br/>" & dataGridHTML
                    DotNetNuke.Services.Mail.Mail.SendMail(objRinfo.EmailFrom, objRinfo.EmailTo, "", objRinfo.ReportName, dataGridHTML, "", "HTML", "", "", "", "")
                End If
            End If

        End Sub


        Private Function calcNextRunTime(ByVal objRinfo As NB_Store_SQLReportInfo) As DateTime
            Dim NextRunTime As DateTime

            'if re-run time is over a day then make sure the start time is re-set to the hour required.
            If objRinfo.SchReRunMins >= 1440 Then
                objRinfo.LastRunTime = CDate(objRinfo.LastRunTime.ToShortDateString & " " & objRinfo.SchStartHour & ":" & objRinfo.SchStartMins & ":00")
            End If

            If IsNumeric(objRinfo.SchReRunMins) Then
                NextRunTime = DateAdd(DateInterval.Minute, CInt(objRinfo.SchReRunMins), objRinfo.LastRunTime)
            End If
            Return NextRunTime
        End Function

    End Class

    Public Class StoreStatistics
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem
        End Sub

        Public Overrides Sub DoWork()
            Try

                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL

                DoReport()

                'update the result to success since no exception was thrown
                Me.ScheduleHistoryItem.Succeeded = True
                Me.ScheduleHistoryItem.AddLogNote("Store Statistics Completed.")

            Catch exc As Exception
                Me.ScheduleHistoryItem.Succeeded = False
                Me.ScheduleHistoryItem.AddLogNote("Store Statistics failed." + exc.ToString)
                Me.ScheduleHistoryItem.Succeeded = False

                'notification that we have errored
                Me.Errored(exc)

                'log the exception
                LogException(exc)
            End Try
        End Sub


        Private Sub DoReport()
            Dim objPC As New DotNetNuke.Entities.Portals.PortalController
            Dim aryList As ArrayList
            Dim objPInfo As DotNetNuke.Entities.Portals.PortalInfo
            Dim LastRunDay As String

            aryList = objPC.GetPortals()
            For Each objPInfo In aryList
                If GetStoreSetting(objPInfo.PortalID, "version", "None") <> "" Then ' portal may not have a store.
                    LastRunDay = GetStoreSetting(objPInfo.PortalID, "storestatistics.lastday", "None")
                    If LastRunDay <> Day(Today).ToString Then

                        Dim objSCtrl As New StatsController
                        Dim PurgeDate As Date
                        Dim PurgeDays As String = GetStoreSetting(objPInfo.PortalID, "searchwordpurge.days")
                        If IsNumeric(PurgeDays) Then
                            PurgeDate = DateAdd(DateInterval.Day, CInt(PurgeDays) * -1, Now)
                        Else
                            PurgeDate = DateAdd(DateInterval.Day, -60, Now)
                        End If

                        objSCtrl.PurgeSearchWordHits(objPInfo.PortalID, PurgeDate)

                        objSCtrl.ProcessSearchWordHits(objPInfo.PortalID)

                        SetStoreSetting(objPInfo.PortalID, "storestatistics.lastday", Day(Today).ToString, "None", True)
                    End If
                End If
            Next



        End Sub



    End Class


    Public Class PurgeStoreFiles
        Inherits DotNetNuke.Services.Scheduling.SchedulerClient

        Public Sub New(ByVal objScheduleHistoryItem As DotNetNuke.Services.Scheduling.ScheduleHistoryItem)
            MyBase.new()
            Me.ScheduleHistoryItem = objScheduleHistoryItem
        End Sub

        Public Overrides Sub DoWork()
            Try

                'notification that the event is progressing
                Me.Progressing()    'OPTIONAL

                PurgeStore()

                'update the result to success since no exception was thrown
                Me.ScheduleHistoryItem.Succeeded = True
                Me.ScheduleHistoryItem.AddLogNote("Purge Store Files Completed.")

            Catch exc As Exception
                Me.ScheduleHistoryItem.Succeeded = False
                Me.ScheduleHistoryItem.AddLogNote("Purge Store Files failed." + exc.ToString)
                Me.ScheduleHistoryItem.Succeeded = False

                'notification that we have errored
                Me.Errored(exc)

                'log the exception
                LogException(exc)
            End Try
        End Sub

        Private Sub PurgeStore()
            Dim objPC As New DotNetNuke.Entities.Portals.PortalController
            Dim aryList As ArrayList
            Dim objPInfo As DotNetNuke.Entities.Portals.PortalInfo
            Dim objPromoCtrl As New PromoController

            aryList = objPC.GetPortals()
            For Each objPInfo In aryList
                If GetStoreSetting(objPInfo.PortalID, "version", "None") <> "" Then ' portal may not have a store.

                    Dim PortalSettings As New Portals.PortalSettings(objPInfo.PortalID)

                    PurgeLogFiles(PortalSettings)

                    PurgeAllFiles(PortalSettings)

                End If
            Next

        End Sub

    End Class

End Namespace

