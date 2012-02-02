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


Imports System
Imports System.IO

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports ICSharpCode.SharpZipLib.Zip
Imports ICSharpCode.SharpZipLib.Checksums
Imports ICSharpCode.SharpZipLib.GZip

Namespace NEvoWeb.Modules.NB_Store

    Public Class Export

        Public Function ExportImages(ByVal PortalID As Integer, ByVal strBackupFile As String) As String
            Dim objPCtrl As New ProductController
            Dim aryList As ArrayList
            Dim hsTab As New Hashtable
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo

            aryList = objPCtrl.GetProductImageExportList(PortalID)
            For Each objPIInfo In aryList
                If Not hsTab.ContainsValue(objPIInfo.ImagePath) Then
                    hsTab.Add(objPIInfo.ImageID, objPIInfo.ImagePath)
                End If
            Next

            aryList = objCCtrl.GetCategories(PortalID, GetCurrentCulture, -1, True, True)
            For Each objCInfo In aryList
                If objCInfo.ImageURL <> "" Then
                    If Not hsTab.ContainsValue(System.Web.Hosting.HostingEnvironment.MapPath(objCInfo.ImageURL)) Then
                        hsTab.Add("CAT" & objCInfo.CategoryID.ToString, System.Web.Hosting.HostingEnvironment.MapPath(objCInfo.ImageURL))
                    End If
                End If
            Next

            If ZipHashTable(hsTab, strBackupFile) Then
                Return strBackupFile
            Else
                Return ""
            End If

        End Function

        Private Function ZipHashTable(ByVal list As Hashtable, ByVal ZipFileName As String) As Integer
            Dim CompressionLevel As Integer = 0
            Dim GUIDName As String = System.Guid.NewGuid.ToString
            Dim fname As String
            Dim ZipSize As Long
            Dim Item As DictionaryEntry

            'craete zip file
            Dim strmZipFile As FileStream = Nothing
            strmZipFile = File.Create(ZipFileName)
            Dim strmZipStream As ZipOutputStream = Nothing
            Try
                strmZipStream = New ZipOutputStream(strmZipFile)
                strmZipStream.SetLevel(CompressionLevel)
                For Each Item In list
                    fname = Item.Value
                    If File.Exists(fname) Then
                        FileSystemUtils.AddToZip(strmZipStream, fname, System.IO.Path.GetFileName(fname), "")
                    End If
                Next
            Catch ex As Exception
                Return False
            Finally
                If Not strmZipStream Is Nothing Then
                    ZipSize = strmZipStream.Length
                    strmZipStream.Finish()
                    strmZipStream.Close()
                End If
            End Try

            Return True

        End Function


        Public Function ExportDocs(ByVal PortalID As Integer, ByVal strBackupFile As String) As String
            Dim objPCtrl As New ProductController
            Dim aryList As ArrayList
            Dim hsTab As New Hashtable
            Dim objDInfo As NB_Store_ProductDocInfo

            aryList = objPCtrl.GetProductDocExportList(PortalID)
            For Each objDInfo In aryList
                If Not hsTab.ContainsValue(objDInfo.DocPath) Then
                    hsTab.Add(objDInfo.DocID, objDInfo.DocPath)
                End If
            Next

            If ZipHashTable(hsTab, strBackupFile) Then
                Return strBackupFile
            Else
                Return ""
            End If

        End Function


        Public Function ExportProducts(ByVal PortalId As Integer, ByVal strBackupFile As String, ByVal blnExpOrders As Boolean) As String
            Try

                Dim strXML As String = ""
                Dim objFile As New FileObj
                Dim objPCtrl As New ProductController
                Dim objPInfo As NB_Store_ProductsInfo
                Dim objOCtrl As New OrderController
                Dim objOInfo As NB_Store_OrdersInfo
                Dim aryList As ArrayList
                Dim supportedLanguages As LocaleCollection = GetValidLocales()

                strXML &= "<root version=""1.0"">"

                strXML &= "<products>"
                For Each Lang As String In supportedLanguages
                    strXML &= "<" & Lang & ">"
                    aryList = objPCtrl.GetProductExportList(PortalId, Lang, False)
                    For Each objPInfo In aryList
                        strXML &= GetProductXML(objPInfo)
                    Next
                    strXML &= "</" & Lang & ">"
                Next
                strXML &= "</products>"
                strXML &= GetCategoriesXML(PortalId)
                'shipping now has it's own import export
                'strXML &= GetShippingXML(PortalId)
                strXML &= GetTaxXML(PortalId)

                If blnExpOrders Then
                    strXML &= "<orders>"
                    aryList = objOCtrl.GetOrdersExportList(PortalId, -1)
                    For Each objOInfo In aryList
                        strXML &= GetOrderXML(objOInfo)
                    Next
                    strXML &= "</orders>"
                End If

                strXML &= "</root>"

                objFile.SaveTextToFile(strXML, strBackupFile)

                Return strBackupFile
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Function ExportShipping(ByVal PortalId As Integer, ByVal strBackupFile As String) As String
            Try

                Dim strXML As String = ""
                Dim objFile As New FileObj
                Dim objPCtrl As New ProductController
                Dim supportedLanguages As LocaleCollection = GetValidLocales()

                strXML &= "<root version=""1.0"">"
                strXML &= GetShippingXML(PortalId)
                strXML &= "</root>"

                objFile.SaveTextToFile(strXML, strBackupFile)

                Return strBackupFile
            Catch ex As Exception
                Return ""
            End Try
        End Function


        Public Function ExportOrders(ByVal PortalID As Integer, ByVal ArchivedOnly As Boolean, ByVal strBackupFile As String) As String
            Try
                Dim strXML As String = ""
                Dim objFile As New FileObj
                Dim aryList As ArrayList
                Dim supportedLanguages As LocaleCollection = GetValidLocales()
                Dim objOCtrl As New OrderController
                Dim objOInfo As NB_Store_OrdersInfo

                strXML &= "<root version=""1.0"">"

                strXML &= "<orders>"
                If ArchivedOnly Then
                    aryList = objOCtrl.GetOrdersExportList(PortalID, 75)
                Else
                    aryList = objOCtrl.GetOrdersExportList(PortalID, -1)
                End If
                For Each objOInfo In aryList
                    strXML &= GetOrderXML(objOInfo)
                Next
                strXML &= "</orders>"

                strXML &= "</root>"

                objFile.SaveTextToFile(strXML, strBackupFile)

                Return strBackupFile
            Catch ex As Exception
                Return ""
            End Try
        End Function

        Public Function GetCategoriesXML(ByVal PortalId As Integer) As String
            Dim strXML As String = ""
            Dim aryList As ArrayList
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo
            Dim supportedLanguages As LocaleCollection = GetValidLocales()

            strXML &= "<categories>"
            For Each Lang As String In supportedLanguages
                strXML &= "<" & Lang & ">"
                aryList = objCCtrl.GetCategories(PortalId, Lang)
                For Each objCInfo In aryList
                    strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objCInfo)
                Next
                strXML &= "</" & Lang & ">"
            Next
            strXML &= "</categories>"

            Return strXML
        End Function

        Public Function GetShippingXML(ByVal PortalId As Integer) As String
            Dim strXML As String = ""
            Dim aryList As ArrayList
            Dim objSCtrl As New ShipController
            Dim objSInfo As NB_Store_ShippingRatesInfo
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim arySMList As ArrayList

            strXML &= "<shipping>"

            arySMList = objSCtrl.GetShippingMethodList(PortalId)
            For Each objSMInfo As NB_Store_ShippingMethodInfo In arySMList
                strXML &= "<method>"
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objSMInfo)
                strXML &= "<rate>"
                aryList = objSCtrl.GetShippingRateList(PortalId, "", "", "", objSMInfo.ShipMethodID)
                For Each objSInfo In aryList
                    strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objSInfo)
                Next
                strXML &= "</rate>"
                strXML &= "</method>"
            Next

            strXML &= "</shipping>"


            Return strXML
        End Function

        Public Function GetTaxXML(ByVal PortalId As Integer) As String
            Dim strXML As String = ""
            Dim aryList As ArrayList
            Dim objTCtrl As New TaxController
            Dim objTInfo As NB_Store_TaxRatesInfo
            Dim supportedLanguages As LocaleCollection = GetValidLocales()

            strXML &= "<tax>"
            aryList = objTCtrl.GetTaxRatesList(PortalId, "", "", "")
            For Each objTInfo In aryList
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objTInfo)
            Next
            strXML &= "</tax>"

            Return strXML
        End Function


        Public Function GetOrderXML(ByVal OrderID As Integer) As String
            Dim objOCtrl As New OrderController
            Dim objOInfo As NB_Store_OrdersInfo
            objOInfo = objOCtrl.GetOrder(OrderID)
            Return GetOrderXML(objOInfo)
        End Function

        Public Function GetOrderXML(ByVal objOInfo As NB_Store_OrdersInfo) As String
            Dim strXML As String = ""
            Dim objOCtrl As New OrderController
            Dim aryList2 As ArrayList
            Dim objODInfo As NB_Store_OrderDetailsInfo
            Dim objAInfo As NB_Store_AddressInfo

            strXML &= "<O>"
            strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objOInfo)
            strXML &= "<BA>"
            objAInfo = objOCtrl.GetOrderAddress(objOInfo.BillingAddressID)
            If Not objAInfo Is Nothing Then
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objAInfo)
            End If
            strXML &= "</BA>"
            strXML &= "<SA>"
            objAInfo = objOCtrl.GetOrderAddress(objOInfo.ShippingAddressID)
            If Not objAInfo Is Nothing Then
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objAInfo)
            End If
            strXML &= "</SA>"
            strXML &= "<OD>"
            aryList2 = objOCtrl.GetOrderDetailList(objOInfo.OrderID)
            For Each objODInfo In aryList2
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objODInfo)
            Next
            strXML &= "</OD>"
            strXML &= "</O>"

            Return strXML
        End Function

        Public Function GetProductXML(ByVal ProductID As Integer, ByVal Lang As String) As String
            Dim objPCtrl As New ProductController
            Dim objPInfo As NB_Store_ProductsInfo
            objPInfo = objPCtrl.GetProduct(ProductID, Lang)
            If Not objPInfo Is Nothing Then
                Return GetProductXML(objPInfo)
            Else
                Return ""
            End If
        End Function

        Public Function GetProductXML(ByVal objPInfo As NB_Store_ProductsInfo) As String
            Dim strXML As String = ""
            Dim objPCtrl As New ProductController
            Dim aryList2 As ArrayList
            Dim aryList3 As ArrayList
            Dim objMInfo As NB_Store_ModelInfo
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim objDInfo As NB_Store_ProductDocInfo
            Dim objOInfo As NB_Store_OptionInfo
            Dim objOVInfo As NB_Store_OptionValueInfo

            strXML &= "<P>"
            strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objPInfo)
            strXML &= "<M>"
            aryList2 = objPCtrl.GetModelList(objPInfo.PortalID, objPInfo.ProductID, objPInfo.Lang, True)
            For Each objMInfo In aryList2
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objMInfo)
            Next
            strXML &= "</M>"
            strXML &= "<I>"
            aryList2 = objPCtrl.GetProductImageList(objPInfo.ProductID, objPInfo.Lang)
            For Each objPIInfo In aryList2
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objPIInfo)
            Next
            strXML &= "</I>"
            strXML &= "<D>"
            aryList2 = objPCtrl.GetProductDocList(objPInfo.ProductID, objPInfo.Lang)
            For Each objDInfo In aryList2
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objDInfo)
            Next
            strXML &= "</D>"
            strXML &= "<C>"
            aryList2 = objPCtrl.GetCategoriesAssigned(objPInfo.ProductID)
            For Each objCInfo As NB_Store_ProductCategoryInfo In aryList2
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objCInfo)
            Next
            strXML &= "</C>"
            strXML &= "<R>"
            aryList2 = objPCtrl.GetProductRelatedArray(objPInfo.PortalID, objPInfo.ProductID, objPInfo.Lang, -1, True)
            For Each objRInfo As NB_Store_ProductRelatedInfo In aryList2
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objRInfo)
            Next
            strXML &= "</R>"
            strXML &= "<options>"
            aryList2 = objPCtrl.GetOptionList(objPInfo.ProductID, objPInfo.Lang)
            For Each objOInfo In aryList2
                strXML &= "<O>"
                strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objOInfo)
                strXML &= "<OV>"
                aryList3 = objPCtrl.GetOptionValueList(objOInfo.OptionID, objPInfo.Lang)
                For Each objOVInfo In aryList3
                    strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objOVInfo)
                Next
                strXML &= "</OV>"
                strXML &= "</O>"
            Next
            strXML &= "</options>"
            strXML &= "</P>"

            Return strXML
        End Function

        Public Function GetSettingsXML(ByVal PortalId As Integer) As String
            Return GetSettingsXML(PortalId, Nothing, Nothing)
        End Function


        Public Function GetSettingsXML(ByVal PortalId As Integer, ByVal chkList As CheckBoxList, ByVal LanguageList As Hashtable) As String
            Dim strXML As String = ""
            Dim objSCtrl As New SettingsController
            Dim objOCtrl As New OrderController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim aryList As ArrayList

            strXML += "<settings>"

            aryList = objSCtrl.GetSettingList(PortalId, "", True, "")
            For Each objSInfo In aryList
                If chkList Is Nothing Then
                    strXML += "<" & objSInfo.SettingName & " Lang=""" & objSInfo.Lang.Trim & """ CtrlType=""" & objSInfo.CtrlType & """" & " GroupRef=""" & objSInfo.GroupRef & """" & " HostOnly=""" & objSInfo.HostOnly & """><![CDATA[" & objSInfo.SettingValue & "]]></" & objSInfo.SettingName & ">"
                Else
                    If Not chkList.Items.FindByValue(objSInfo.SettingName) Is Nothing Then
                        If chkList.Items.FindByValue(objSInfo.SettingName).Selected And LanguageList.Contains(objSInfo.Lang.Trim) Then
                            strXML += "<" & objSInfo.SettingName & " Lang=""" & objSInfo.Lang.Trim & """ CtrlType=""" & objSInfo.CtrlType & """" & " GroupRef=""" & objSInfo.GroupRef & """" & " HostOnly=""" & objSInfo.HostOnly & """><![CDATA[" & objSInfo.SettingValue & "]]></" & objSInfo.SettingName & ">"
                        End If
                    End If
                End If
            Next

            strXML += "</settings>"
            Return strXML
        End Function

        Public Function GetSettingsTextXML(ByVal PortalId As Integer) As String
            Return GetSettingsTextXML(PortalId, Nothing, Nothing)
        End Function

        Public Function GetSettingsTextXML(ByVal PortalId As Integer, ByVal chkList As CheckBoxList, ByVal LanguageList As Hashtable) As String
            Dim strXML As String = ""
            Dim objSCtrl As New SettingsController
            Dim objOCtrl As New OrderController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim aryList As ArrayList

            strXML += "<settingstext>"

            aryList = objSCtrl.GetSettingsTexts(PortalId, "", True, "")
            For Each objSTInfo In aryList
                If chkList Is Nothing Then
                    strXML += "<" & objSTInfo.SettingName & " Lang=""" & objSTInfo.Lang.Trim & """" & " CtrlType=""" & objSTInfo.CtrlType & """" & " GroupRef=""" & objSTInfo.GroupRef & """" & " HostOnly=""" & objSTInfo.HostOnly & """><![CDATA[" & objSTInfo.SettingText & "]]></" & objSTInfo.SettingName & ">"
                Else
                    If Not chkList.Items.FindByValue(objSTInfo.SettingName) Is Nothing Then
                        If chkList.Items.FindByValue(objSTInfo.SettingName).Selected And LanguageList.Contains(objSTInfo.Lang.Trim) Then
                            strXML += "<" & objSTInfo.SettingName & " Lang=""" & objSTInfo.Lang.Trim & """" & " CtrlType=""" & objSTInfo.CtrlType & """" & " GroupRef=""" & objSTInfo.GroupRef & """" & " HostOnly=""" & objSTInfo.HostOnly & """><![CDATA[" & objSTInfo.SettingText & "]]></" & objSTInfo.SettingName & ">"
                        End If
                    End If
                End If
            Next

            strXML += "</settingstext>"

            Return strXML
        End Function

        Public Function GetStatusXML() As String
            Dim strXML As String = ""
            Dim objSCtrl As New SettingsController
            Dim objOCtrl As New OrderController
            Dim objOSInfo As NB_Store_OrderStatusInfo
            Dim aryList As ArrayList

            strXML += "<status>"

            aryList = objOCtrl.GetOrderStatusList("")
            For Each objOSInfo In aryList
                strXML += "<ID" & objOSInfo.OrderStatusID.ToString & " Lang=""" & objOSInfo.Lang & """ ListOrder=""" & objOSInfo.ListOrder & """><![CDATA[" & objOSInfo.OrderStatusText & "]]></ID" & objOSInfo.OrderStatusID.ToString & ">"
            Next

            strXML += "</status>"

            Return strXML
        End Function


        Public Function GetSQLReports(ByVal PortalID As Integer) As String
            Dim strXML As String = ""
            Dim objCtrl As New SQLReportController
            Dim arySQLReportParams As ArrayList
            Dim arySQLReports As ArrayList = objCtrl.GetSQLAdminReportList(PortalID, True, "")
            Dim objSQLReport As NB_Store_SQLReportInfo
            Dim objRPInfo As NB_Store_SQLReportParamInfo
            Dim objXInfo As NB_Store_SQLReportXSLInfo

            If arySQLReports.Count > 0 Then
                strXML += "<SQLReports>"
                For Each objSQLReport In arySQLReports
                    strXML += "<SQLReport>"
                    strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objSQLReport)
                    strXML += "<SQLReportParams>"
                    arySQLReportParams = objCtrl.GetSQLReportParamList(objSQLReport.ReportID)
                    For Each objRPInfo In arySQLReportParams
                        strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objRPInfo)
                    Next
                    strXML += "</SQLReportParams>"
                    strXML += "<SQLReportXSL>"
                    arySQLReportParams = objCtrl.GetSQLReportXSLList(objSQLReport.ReportID)
                    For Each objXInfo In arySQLReportParams
                        strXML &= DotNetNuke.Common.Utilities.XmlUtils.Serialize(objXInfo)
                    Next
                    strXML += "</SQLReportXSL>"
                    strXML += "</SQLReport>"
                Next

                strXML += "</SQLReports>"
            End If

            Return strXML
        End Function


    End Class

End Namespace
