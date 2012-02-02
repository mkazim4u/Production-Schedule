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

    Public Class Import

        Private _ProdTransTab As Hashtable
        Private _ModelTransTab As Hashtable
        Private _ImageTransTab As Hashtable
        Private _OptionTransTab As Hashtable
        Private _OptionVTransTab As Hashtable
        Private _CatTransTab As Hashtable
        Private _DocTransTab As Hashtable
        Private _RelatedTransTab As Hashtable
        Private _ShipMethodTransTab As Hashtable


        Public Function ImportProductImages(ByVal PS As Portals.PortalSettings, ByVal ImportZipFile As String) As String
            CreateDir(PS, PRODUCTIMAGESFOLDER)
            Return UnZipImportFile(ImportZipFile, PS.HomeDirectoryMapPath & PRODUCTIMAGESFOLDER & "\", PS)
        End Function

        Public Function ImportProductDocs(ByVal PS As Portals.PortalSettings, ByVal ImportZipFile As String) As String
            CreateDir(PS, PRODUCTDOCSFOLDER)
            Return UnZipImportFile(ImportZipFile, PS.HomeDirectoryMapPath & PRODUCTDOCSFOLDER & "\", PS)
        End Function

        Public Function ImportShipping(ByVal PS As Portals.PortalSettings, ByVal ImportFile As String) As String
            Dim xmlDoc As New Xml.XmlDataDocument
            Dim xmlSNod1 As Xml.XmlNode
            Dim xmlSNod2 As Xml.XmlNode
            Dim xmlSNodList1 As Xml.XmlNodeList
            Dim xmlSNodList2 As Xml.XmlNodeList
            Dim strMSG As String = "<br/>Shipping Updated : "
            Dim objSCtrl As New ShipController
            Dim objSRInfo As New NB_Store_ShippingRatesInfo
            Dim objSMInfo As New NB_Store_ShippingMethodInfo
            Dim c As Integer = 0
            Dim NewShipMethodID As Integer
            Dim OldShipMethodID As Integer
            Dim strID As String

            xmlDoc.Load(ImportFile)


            'clear down current shipping data
            objSCtrl.ClearAllShippingMethods(PS.PortalId)

            'create Import Translation table
            _ShipMethodTransTab = New Hashtable
            xmlSNodList1 = xmlDoc.SelectNodes("root/shipping/method/NB_Store_ShippingMethodInfo")
            For Each xmlNod In xmlSNodList1
                strID = xmlNod.SelectSingleNode("ShipMethodID").InnerXml
                If Not _ShipMethodTransTab.ContainsKey(strID) Then
                    _ShipMethodTransTab.Add(strID, "-1")
                End If
            Next

            'Import all types found
            xmlSNodList1 = xmlDoc.SelectNodes("root/shipping/method")
            For Each xmlSNod1 In xmlSNodList1
                'do method data
                objSMInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlSNod1.SelectSingleNode("NB_Store_ShippingMethodInfo").OuterXml, objSMInfo.GetType)
                OldShipMethodID = objSMInfo.ShipMethodID
                objSMInfo.ShipMethodID = -1
                objSMInfo.PortalID = PS.PortalId
                NewShipMethodID = objSCtrl.UpdateObjShippingMethod(objSMInfo)
                _ShipMethodTransTab(OldShipMethodID) = NewShipMethodID

                'do rate data
                xmlSNodList2 = xmlSNod1.SelectNodes("rate/NB_Store_ShippingRatesInfo[ShipType!='PRD']")
                For Each xmlSNod2 In xmlSNodList2
                    objSRInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlSNod2.OuterXml, objSRInfo.GetType)
                    objSRInfo.ItemId = -1
                    objSRInfo.ObjectId = -1
                    objSRInfo.PortalID = PS.PortalId
                    objSRInfo.ShipMethodID = NewShipMethodID
                    objSCtrl.UpdateObjShippingRate(objSRInfo)
                    c = c + 1
                Next
            Next

            Return strMSG & c.ToString
        End Function

        Public Function ImportProducts(ByVal PS As Portals.PortalSettings, ByVal ImportFile As String, ByVal UpdateExistingProductsFlg As Boolean, ByVal CreateCategoriesFlg As Boolean) As String
            Dim strMsg As String = ""
            Try
                Dim xmlDoc As New Xml.XmlDataDocument
                Dim xmlPNod As Xml.XmlNode
                Dim xmlPNodList As Xml.XmlNodeList
                Dim xmlMNod As Xml.XmlNode
                Dim xmlMNodList As Xml.XmlNodeList
                Dim xmlONod As Xml.XmlNode
                Dim xmlONodList As Xml.XmlNodeList
                Dim xmlOVNod As Xml.XmlNode
                Dim xmlOVNodList As Xml.XmlNodeList
                Dim xmlINod As Xml.XmlNode
                Dim xmlINodList As Xml.XmlNodeList
                Dim objPInfo As New NB_Store_ProductsInfo
                Dim objPInfoDB As NB_Store_ProductsInfo
                Dim objMInfo As New NB_Store_ModelInfo
                Dim objMInfoDB As NB_Store_ModelInfo
                Dim objIInfo As New NB_Store_ProductImageInfo
                Dim objDInfo As New NB_Store_ProductDocInfo
                Dim objOInfo As New NB_Store_OptionInfo
                Dim objOVInfo As New NB_Store_OptionValueInfo
                Dim objSRCtrl As New ShipController
                Dim objSRInfo As New NB_Store_ShippingRatesInfo
                Dim objRInfo As New NB_Store_ProductRelatedInfo
                Dim strXML As String = ""
                Dim objPCtrl As New ProductController
                'Dim supportedLanguages As LocaleCollection = GetValidLocales()
                Dim supportedLanguages As LocaleCollection = GetValidLocales()
                Dim ImpProdID As Integer
                Dim ImpModelID As Integer
                Dim ImpImageID As Integer
                Dim ImpDocID As Integer
                Dim ImpRelatedID As Integer
                Dim ImpOptionID As Integer
                Dim ImpOptionVID As Integer
                Dim AlreadyClearedFlg As Boolean

                xmlDoc.Load(ImportFile)

                'build Translation table
                buildTransTables(PS.PortalId, xmlDoc)

                'create categories
                If CreateCategoriesFlg Then
                    strMsg = ImportCategories(PS, ImportFile)
                    If strMsg <> "" Then
                        Return strMsg
                    End If
                End If

                Dim FirstLang As String = ""

                For Each Lang As String In supportedLanguages

                    xmlPNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P")
                    For Each xmlPNod In xmlPNodList
                        Try
                            '----------------------------------------------------
                            'validate models by forcing exception before any updates
                            xmlMNodList = xmlPNod.SelectNodes("M/NB_Store_ModelInfo")
                            For Each xmlMNod In xmlMNodList
                                objMInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlMNod.OuterXml, objMInfo.GetType)
                            Next
                            '----------------------------------------------------

                            If FirstLang = "" Then FirstLang = Lang
                            'Update Product
                            objPInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlPNod.SelectSingleNode("NB_Store_ProductsInfo").OuterXml, objPInfo.GetType)
                            objPInfo.PortalID = PS.PortalId
                            ImpProdID = objPInfo.ProductID
                            If UpdateExistingProductsFlg And objPInfo.ProductRef <> "" Then
                                objPInfoDB = objPCtrl.GetProductByRef(PS.PortalId, objPInfo.ProductRef, Lang)
                                If Not objPInfoDB Is Nothing Then
                                    objPInfo.ProductID = objPInfoDB.ProductID
                                Else
                                    objPInfo.ProductID = _ProdTransTab(ImpProdID.ToString)
                                End If
                            Else
                                objPInfo.ProductID = _ProdTransTab(ImpProdID.ToString)
                            End If

                            objPInfo = objPCtrl.UpdateObjProduct(objPInfo)

                            _ProdTransTab(ImpProdID.ToString) = objPInfo.ProductID.ToString

                            If CreateCategoriesFlg Then
                                UpdateCategoryXref(objPInfo, xmlPNod.SelectNodes("C/NB_Store_ProductCategoryInfo"))
                            End If

                            'Update Models
                            xmlMNodList = xmlPNod.SelectNodes("M/NB_Store_ModelInfo")
                            For Each xmlMNod In xmlMNodList
                                objMInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlMNod.OuterXml, objMInfo.GetType)
                                objMInfo.ProductID = objPInfo.ProductID
                                objMInfo.PortalID = PS.PortalId
                                ImpModelID = objMInfo.ModelID
                                If UpdateExistingProductsFlg Then
                                    objMInfoDB = objPCtrl.GetModelByRef(objPInfo.ProductID, objMInfo.ModelRef, Lang)
                                    If Not objMInfoDB Is Nothing Then
                                        objMInfo.ModelID = objMInfoDB.ModelID
                                    Else
                                        objMInfo.ModelID = _ModelTransTab(ImpModelID.ToString)
                                    End If
                                Else
                                    objMInfo.ModelID = _ModelTransTab(ImpModelID.ToString)
                                End If
                                _ModelTransTab(ImpModelID.ToString) = objPCtrl.UpdateObjModel(objMInfo).ToString
                                objMInfo.ModelID = _ModelTransTab(ImpModelID.ToString)

                                'Update shipping
                                Dim objShip As NB_Store_ShippingRatesInfo
                                Dim SCost As Decimal = 0
                                objShip = objSRCtrl.GetShippingRateByObjID(objMInfo.PortalID, objMInfo.ModelID, "PRD", -1)
                                If Not objShip Is Nothing Then
                                    objShip.ProductWeight = objMInfo.Weight
                                    objShip.ProductHeight = objMInfo.Height
                                    objShip.ProductLength = objMInfo.Length
                                    objShip.ProductWidth = objMInfo.Width
                                Else
                                    objShip = New NB_Store_ShippingRatesInfo
                                    objShip.Description = ""
                                    objShip.Disable = False
                                    objShip.ItemId = -1
                                    objShip.ProductWeight = objMInfo.Weight
                                    objShip.ProductHeight = objMInfo.Height
                                    objShip.ProductLength = objMInfo.Length
                                    objShip.ProductWidth = objMInfo.Width
                                    objShip.ObjectId = objMInfo.ModelID
                                    objShip.Range1 = 0
                                    objShip.Range2 = 0
                                    objShip.PortalID = objMInfo.PortalID
                                    objShip.ShipCost = 0
                                    objShip.ShipMethodID = -1
                                    objShip.ShipType = "PRD"
                                End If

                                objSRCtrl.UpdateObjShippingRate(objShip)

                            Next

                            'update Images
                            xmlINodList = xmlPNod.SelectNodes("I/NB_Store_ProductImageInfo")
                            AlreadyClearedFlg = False
                            For Each xmlINod In xmlINodList
                                objIInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlINod.OuterXml, objIInfo.GetType)
                                ImpImageID = objIInfo.ImageID

                                If UpdateExistingProductsFlg And Not AlreadyClearedFlg Then
                                    'Is new image to be create, if so clear down old images
                                    If _ImageTransTab(ImpImageID.ToString) = "-1" Then
                                        'remove existing image data
                                        Dim aryList As ArrayList
                                        aryList = objPCtrl.GetProductImageList(objPInfo.ProductID, Lang)
                                        For Each objIInfo2 As NB_Store_ProductImageInfo In aryList
                                            objPCtrl.DeleteProductImage(objIInfo2.ImageID)
                                        Next
                                        AlreadyClearedFlg = True
                                    End If
                                End If

                                objIInfo.ProductID = objPInfo.ProductID
                                objIInfo.ImageID = _ImageTransTab(ImpImageID.ToString)
                                objIInfo.ImagePath = PS.HomeDirectoryMapPath & PRODUCTIMAGESFOLDER & "\" & Path.GetFileName(objIInfo.ImagePath)
                                objIInfo.ImageURL = PS.HomeDirectory & PRODUCTIMAGESFOLDER & "/" & Path.GetFileName(objIInfo.ImagePath)
                                If Path.GetFileNameWithoutExtension(objIInfo.ImagePath) <> "" Then
                                    _ImageTransTab(ImpImageID.ToString) = objPCtrl.UpdateObjProductImage(objIInfo).ToString
                                End If
                            Next

                            'update Docs
                            xmlINodList = xmlPNod.SelectNodes("D/NB_Store_ProductDocInfo")
                            AlreadyClearedFlg = False
                            For Each xmlINod In xmlINodList
                                objDInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlINod.OuterXml, objDInfo.GetType)
                                ImpDocID = objDInfo.DocID

                                If UpdateExistingProductsFlg And Not AlreadyClearedFlg Then
                                    'Is new doc to be create, if so clear down old images
                                    If _DocTransTab(ImpDocID.ToString) = "-1" Then
                                        'remove existing doc data
                                        Dim aryList As ArrayList
                                        aryList = objPCtrl.GetProductDocList(objPInfo.ProductID, Lang)
                                        For Each objDInfo2 As NB_Store_ProductDocInfo In aryList
                                            objPCtrl.DeleteProductDoc(objDInfo2.DocID)
                                        Next
                                        AlreadyClearedFlg = True
                                    End If
                                End If

                                objDInfo.ProductID = objPInfo.ProductID
                                objDInfo.DocID = _DocTransTab(ImpDocID.ToString)
                                objDInfo.DocPath = PS.HomeDirectoryMapPath & PRODUCTDOCSFOLDER & "\" & Path.GetFileName(objDInfo.DocPath)
                                If Path.GetFileNameWithoutExtension(objDInfo.DocPath) <> "" Then
                                    _DocTransTab(ImpDocID.ToString) = objPCtrl.UpdateObjProductDoc(objDInfo).ToString
                                End If
                            Next


                            'update options
                            xmlONodList = xmlPNod.SelectNodes("options/O")
                            AlreadyClearedFlg = False
                            For Each xmlONod In xmlONodList
                                objOInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlONod.SelectSingleNode("NB_Store_OptionInfo").OuterXml, objOInfo.GetType)
                                ImpOptionID = objOInfo.OptionID

                                If UpdateExistingProductsFlg And Not AlreadyClearedFlg Then
                                    'Is new option to be create, if so clear down old options
                                    If _OptionTransTab(ImpOptionID.ToString) = "-1" Then
                                        'remove existing option data
                                        Dim aryList As ArrayList
                                        aryList = objPCtrl.GetOptionList(objPInfo.ProductID, Lang)
                                        For Each objOInfo2 As NB_Store_OptionInfo In aryList
                                            objPCtrl.DeleteOption(objOInfo2.OptionID)
                                        Next
                                        AlreadyClearedFlg = True
                                    End If
                                End If

                                objOInfo.ProductID = objPInfo.ProductID
                                objOInfo.OptionID = _OptionTransTab(ImpOptionID.ToString)
                                objOInfo = objPCtrl.UpdateObjOption(objOInfo)
                                _OptionTransTab(ImpOptionID.ToString) = objOInfo.OptionID.ToString
                                xmlOVNodList = xmlONod.SelectNodes("OV/NB_Store_OptionValueInfo")
                                For Each xmlOVNod In xmlOVNodList
                                    objOVInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlOVNod.OuterXml, objOVInfo.GetType)
                                    ImpOptionVID = objOVInfo.OptionValueID
                                    objOVInfo.OptionID = objOInfo.OptionID
                                    objOVInfo.OptionValueID = _OptionVTransTab(ImpOptionVID.ToString)
                                    _OptionVTransTab(ImpOptionVID.ToString) = objPCtrl.UpdateObjOptionValue(objOVInfo).ToString
                                Next
                            Next
                        Catch ex As Exception
                            UpdateLog("ERROR on Import: " & ex.ToString)
                            strMsg &= "Product Ref: """ & xmlPNod.SelectSingleNode("NB_Store_ProductsInfo/ProductRef").InnerText & """ failed to import.<br/>"
                        End Try

                    Next
                Next

                If FirstLang = "" Then
                    FirstLang = GetMerchantCulture(PS.PortalId)
                End If

                'update Related - this must be done after the full product install,
                '                 so we can link the products via the new ids.
                xmlPNodList = xmlDoc.SelectNodes("root/products/" & FirstLang & "/P")
                For Each xmlPNod In xmlPNodList
                    Try
                        xmlINodList = xmlPNod.SelectNodes("R/NB_Store_ProductRelatedInfo")
                        AlreadyClearedFlg = False
                        For Each xmlINod In xmlINodList
                            objRInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlINod.OuterXml, objRInfo.GetType)
                            ImpRelatedID = objRInfo.RelatedID

                            If UpdateExistingProductsFlg And Not AlreadyClearedFlg Then
                                'Is new doc to be create, if so clear down old images
                                If _RelatedTransTab(ImpRelatedID.ToString) = "-1" Then
                                    'remove existing related data
                                    objPCtrl.DeleteProductRelatedByProduct(objPInfo.ProductID)
                                    AlreadyClearedFlg = True
                                End If
                            End If

                            objRInfo.ProductID = _ProdTransTab(objRInfo.ProductID.ToString)
                            objRInfo.RelatedProductID = _ProdTransTab(objRInfo.RelatedProductID.ToString)

                            objRInfo.RelatedID = _RelatedTransTab(ImpRelatedID.ToString)
                            _RelatedTransTab(ImpRelatedID.ToString) = objPCtrl.UpdateObjProductRelated(objRInfo)
                        Next

                    Catch ex As Exception
                        UpdateLog("ERROR on Import: " & ex.ToString)
                        strMsg &= "Related Products for Product Ref: """ & xmlPNod.SelectSingleNode("NB_Store_ProductsInfo/ProductRef").InnerText & """ failed to import.<br/>"
                    End Try
                Next

                objPCtrl.ProductLangValidation(PS.PortalId, FirstLang)

                Return BuildReport(strMsg)
            Catch ex As Exception
                strMsg &= ex.ToString
            End Try
            Return strMsg
        End Function

        Public Sub UpdateCategoryXref(ByVal objPInfo As NB_Store_ProductsInfo, ByVal xmlNodList As Xml.XmlNodeList)
            'update CategoryXref
            Dim objCInfo As New NB_Store_ProductCategoryInfo
            Dim objPCtrl As New ProductController
            objPCtrl.DeleteProductCategory(objPInfo.ProductID)
            For Each xmlINod In xmlNodList
                objCInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlINod.OuterXml, objCInfo.GetType)
                objCInfo.CategoryID = CInt(_CatTransTab(objCInfo.CategoryID.ToString))
                objCInfo.ProductID = objPInfo.ProductID
                objPCtrl.UpdateObjProductCategory(objCInfo)
            Next
        End Sub

        Public Function ImportCategories(ByVal PS As Portals.PortalSettings, ByVal ImportFile As String) As String
            Dim strMsg As String = ""
            Try
                Dim xmlDoc As New Xml.XmlDataDocument
                Dim xmlNodList As Xml.XmlNodeList
                Dim xmlNod As Xml.XmlNode
                'Dim supportedLanguages As LocaleCollection = GetValidLocales()
                Dim supportedLanguages As LocaleCollection = DotNetNuke.Services.Localization.Localization.GetSupportedLocales()
                Dim objImpCatInfo As NB_Store_CategoriesInfo = Nothing
                Dim ImpCatArray As CategoryArrayList
                Dim DBCatArray As CategoryArrayList
                Dim QName As String
                Dim objPCtrl As New ProductController
                Dim objDBCatInfo As NB_Store_CategoriesInfo = Nothing

                xmlDoc.Load(ImportFile)

                ImpCatArray = New CategoryArrayList(xmlDoc)
                DBCatArray = New CategoryArrayList(PS.PortalId)

                Dim FirstLang As String = ""

                For Each Lang As String In supportedLanguages
                    xmlNodList = xmlDoc.SelectNodes("root/categories/" & Lang & "/NB_Store_CategoriesInfo")
                    For Each xmlNod In xmlNodList
                        If FirstLang = "" Then FirstLang = Lang
                        QName = ImpCatArray.GetCatQualifiedName(xmlNod("CategoryID").InnerXml, Lang)
                        If QName <> "" Then
                            objDBCatInfo = DBCatArray.GetCatByName(QName, Lang)
                            If objDBCatInfo Is Nothing Then
                                objImpCatInfo = createCategoryByName(PS.PortalId, ImpCatArray, QName, Lang)
                            Else
                                objImpCatInfo = ImpCatArray.GetCatByName(QName, Lang)
                                _CatTransTab(objImpCatInfo.CategoryID.ToString) = objDBCatInfo.CategoryID.ToString
                            End If
                        End If
                    Next
                Next

                If FirstLang = "" Then
                    FirstLang = GetMerchantCulture(PS.PortalId)
                End If

                objPCtrl.CategoryLangValidation(PS.PortalId, FirstLang)

            Catch ex As Exception
                strMsg = ex.ToString
            End Try
            Return strMsg
        End Function

        Public Sub ImportSQLReports(ByVal PortalID As Integer, ByVal ImportFile As String, ByVal blnOverwrite As Boolean)
            Dim xmlDoc As New Xml.XmlDataDocument
            xmlDoc.Load(ImportFile)
            Dim xmlSQLReports As Xml.XmlNode = GetContent(xmlDoc.OuterXml, "content/SQLReports")
            ImportSQLReports(PortalID, xmlSQLReports, blnOverwrite)
        End Sub

        Public Sub ImportSQLReports(ByVal PortalID As Integer, ByVal XmlInput As Xml.XmlNode, ByVal blnOverwrite As Boolean)
            Dim xmlSQLReport As Xml.XmlNode
            Dim xmlSQLReportParams As Xml.XmlNode
            Dim xmlSQLReports As Xml.XmlNode = XmlInput
            Dim objSQLReport As NB_Store_SQLReportInfo
            Dim objRInfo As NB_Store_SQLReportInfo
            Dim objRPInfo As NB_Store_SQLReportParamInfo
            Dim objXInfo As NB_Store_SQLReportXSLInfo
            Dim objCtrl As New SQLReportController

            Dim ReportID As Integer

            If Not xmlSQLReports.SelectNodes("SQLReport") Is Nothing Then

                For Each xmlSQLReport In xmlSQLReports.SelectNodes("SQLReport")

                    If Not xmlSQLReport.SelectSingleNode("NB_Store_SQLReportInfo").OuterXml Is Nothing Then
                        objSQLReport = New NB_Store_SQLReportInfo
                        objSQLReport = CType(DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlSQLReport.SelectSingleNode("NB_Store_SQLReportInfo").OuterXml, objSQLReport.GetType), NB_Store_SQLReportInfo)

                        objRInfo = objCtrl.GetSQLReportByRef(PortalID, objSQLReport.ReportRef)
                        If Not objRInfo Is Nothing Then
                            objSQLReport.ReportID = objRInfo.ReportID
                        Else
                            objSQLReport.ReportID = -1
                        End If

                        If objSQLReport.ReportID = -1 Or blnOverwrite Then

                            If objSQLReport.ReportID >= 0 Then
                                objCtrl.DeleteSQLReport(objSQLReport.ReportID)
                            End If

                            objSQLReport.PortalID = PortalID
                            ReportID = objCtrl.UpdateObjSQLReport(objSQLReport)

                            For Each xmlSQLReportParams In xmlSQLReport.SelectNodes("SQLReportParams/*")
                                If Not xmlSQLReportParams.SelectSingleNode(".") Is Nothing Then
                                    objRPInfo = New NB_Store_SQLReportParamInfo
                                    objRPInfo = CType(DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlSQLReportParams.SelectSingleNode(".").OuterXml, objRPInfo.GetType), NB_Store_SQLReportParamInfo)
                                    objRPInfo.ReportID = ReportID
                                    objRPInfo.ReportParamID = -1
                                    objCtrl.UpdateObjSQLReportParam(objRPInfo)
                                End If
                            Next

                            For Each xmlSQLReportParams In xmlSQLReport.SelectNodes("SQLReportXSL/*")
                                If Not xmlSQLReportParams.SelectSingleNode(".") Is Nothing Then
                                    objXInfo = New NB_Store_SQLReportXSLInfo
                                    objXInfo = CType(DotNetNuke.Common.Utilities.XmlUtils.Deserialize(xmlSQLReportParams.SelectSingleNode(".").OuterXml, objXInfo.GetType), NB_Store_SQLReportXSLInfo)
                                    objXInfo.ReportID = ReportID
                                    objXInfo.ReportXSLID = -1
                                    objCtrl.UpdateObjSQLReportXSL(objXInfo)
                                End If
                            Next

                        End If
                    End If
                Next
            End If
        End Sub


        Private Function BuildReport(Optional ByVal strMsg As String = "") As String
            Dim strRep As String = ""
            strRep &= "Imported: " & _ProdTransTab.Count & " Products<br/>"
            strRep &= "Imported: " & _CatTransTab.Count & " Categories<br/>"
            If strMsg <> "" Then
                strRep = strRep & "<br/><br/><br/>" & strMsg
            End If
            Return strRep
        End Function

        Private Sub buildTransTables(ByVal PortalID As Integer, ByVal xmlDoc As Xml.XmlDataDocument)
            Dim xmlNodList As Xml.XmlNodeList
            Dim xmlNod As Xml.XmlNode
            'Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim supportedLanguages As LocaleCollection = DotNetNuke.Services.Localization.Localization.GetSupportedLocales()
            Dim strID As String
            Dim objDefImpCatInfo As NB_Store_CategoriesInfo = Nothing

            _ProdTransTab = New Hashtable
            _ModelTransTab = New Hashtable
            _ImageTransTab = New Hashtable
            _OptionTransTab = New Hashtable
            _OptionVTransTab = New Hashtable
            _CatTransTab = New Hashtable
            _DocTransTab = New Hashtable
            _RelatedTransTab = New Hashtable
            _ShipMethodTransTab = New Hashtable

            For Each Lang As String In supportedLanguages
                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/NB_Store_ProductsInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("ProductID").InnerXml
                    If Not _ProdTransTab.ContainsKey(strID) Then
                        _ProdTransTab.Add(strID, "-1")
                    End If
                Next

                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/M/NB_Store_ModelInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("ModelID").InnerXml
                    If Not _ModelTransTab.ContainsKey(strID) Then
                        _ModelTransTab.Add(strID, "-1")
                    End If
                Next

                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/I/NB_Store_ProductImageInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("ImageID").InnerXml
                    If Not _ImageTransTab.ContainsKey(strID) Then
                        _ImageTransTab.Add(strID, "-1")
                    End If
                Next

                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/D/NB_Store_ProductDocInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("DocID").InnerXml
                    If Not _DocTransTab.ContainsKey(strID) Then
                        _DocTransTab.Add(strID, "-1")
                    End If
                Next

                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/R/NB_Store_ProductRelatedInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("RelatedID").InnerXml
                    If Not _RelatedTransTab.ContainsKey(strID) Then
                        _RelatedTransTab.Add(strID, "-1")
                    End If
                Next

                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/options/O/NB_Store_OptionInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("OptionID").InnerXml
                    If Not _OptionTransTab.ContainsKey(strID) Then
                        _OptionTransTab.Add(strID, "-1")
                    End If
                Next

                xmlNodList = xmlDoc.SelectNodes("root/products/" & Lang & "/P/options/O/OV/NB_Store_OptionValueInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("OptionValueID").InnerXml
                    If Not _OptionVTransTab.ContainsKey(strID) Then
                        _OptionVTransTab.Add(strID, "-1")
                    End If
                Next

                'create default Import category ("Imported"), if it doesnot exist
                If objDefImpCatInfo Is Nothing Then
                    objDefImpCatInfo = CreateDefaultImportCategory(PortalID, Lang)
                End If

                xmlNodList = xmlDoc.SelectNodes("root/categories/" & Lang & "/NB_Store_CategoriesInfo")
                For Each xmlNod In xmlNodList
                    strID = xmlNod.SelectSingleNode("CategoryID").InnerXml
                    If Not _CatTransTab.ContainsKey(strID) Then
                        _CatTransTab.Add(strID, objDefImpCatInfo.CategoryID.ToString)
                    End If
                Next



            Next
        End Sub

        Private Function createCategoryByName(ByVal PortalID As Integer, ByVal CatImpArray As CategoryArrayList, ByVal QualifiedName As String, ByVal Lang As String) As NB_Store_CategoriesInfo
            Dim objCatCtrl As New CategoryController
            Dim objCatInfo As NB_Store_CategoriesInfo = Nothing
            Dim objImpCatInfo As NB_Store_CategoriesInfo = Nothing
            Dim CatList As String() = QualifiedName.Split("/"c)
            Dim CatQName As String = ""
            'Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim supportedLanguages As LocaleCollection = DotNetNuke.Services.Localization.Localization.GetSupportedLocales()
            Dim ImpCatID As Integer
            Dim PCatID As Integer = 0
            Dim CatDBArray As New CategoryArrayList(PortalID)

            For lp As Integer = 0 To CatList.GetUpperBound(0)
                CatQName = CatQName & "/" & CatList(lp).ToString
                CatQName = CatQName.TrimStart("/"c)
                objImpCatInfo = CatImpArray.GetCatByName(CatQName, Lang)
                If Not objImpCatInfo Is Nothing Then
                    ImpCatID = objImpCatInfo.CategoryID
                    objCatInfo = CatDBArray.GetCatByName(CatQName, Lang)
                    If objCatInfo Is Nothing Then
                        objCatInfo = New NB_Store_CategoriesInfo
                        objCatInfo.CategoryID = -1
                        objCatInfo.PortalID = PortalID
                        objCatInfo.Archived = objImpCatInfo.Archived
                        objCatInfo.CategoryDesc = objImpCatInfo.CategoryDesc
                        objCatInfo.CategoryName = objImpCatInfo.CategoryName
                        objCatInfo.CreatedByUser = 0
                        objCatInfo.CreatedDate = Now
                        objCatInfo.Lang = Lang
                        objCatInfo.ListOrder = objImpCatInfo.ListOrder
                        objCatInfo.Message = objImpCatInfo.Message
                        objCatInfo.ParentCategoryID = PCatID
                        objCatInfo.ParentName = objImpCatInfo.ParentName
                        objCatInfo.ProductCount = 0
                        objCatInfo = objCatCtrl.UpdateObjCategories(objCatInfo)
                        'create in all languages
                        For Each Lang2 As String In supportedLanguages
                            objImpCatInfo = CatImpArray.GetCat(ImpCatID, Lang2)
                            If Not objImpCatInfo Is Nothing Then
                                objCatInfo.Lang = Lang2
                                objCatInfo.CategoryName = objImpCatInfo.CategoryName
                                objCatInfo.CategoryDesc = objImpCatInfo.CategoryDesc
                                objCatInfo.Message = objImpCatInfo.Message
                                objCatInfo = objCatCtrl.UpdateObjCategories(objCatInfo)
                            End If
                        Next
                        CatDBArray = New CategoryArrayList(PortalID)
                    End If
                    PCatID = objCatInfo.CategoryID
                    _CatTransTab(ImpCatID.ToString) = objCatInfo.CategoryID.ToString
                End If

            Next

            Return objCatInfo
        End Function

        Private Function CreateDefaultImportCategory(ByVal PortalID As Integer, ByVal Lang As String) As NB_Store_CategoriesInfo
            Dim objCatCtrl As New CategoryController
            Dim objCatInfo As NB_Store_CategoriesInfo

            Dim DBCats As New CategoryArrayList(PortalID)

            objCatInfo = DBCats.GetCatByName("Imported", Lang)

            If objCatInfo Is Nothing Then
                objCatInfo = New NB_Store_CategoriesInfo
                objCatInfo.Archived = True
                objCatInfo.CategoryDesc = "Products Imported"
                objCatInfo.CategoryID = -1
                objCatInfo.CategoryName = "Imported"
                objCatInfo.CreatedByUser = 0
                objCatInfo.CreatedDate = Now
                objCatInfo.Lang = Lang
                objCatInfo.ListOrder = 1
                objCatInfo.Message = ""
                objCatInfo.ParentCategoryID = 0
                objCatInfo.ParentName = ""
                objCatInfo.PortalID = PortalID
                objCatInfo.ProductCount = 0
                objCatInfo.Hide = True
                objCatInfo = objCatCtrl.UpdateObjCategories(objCatInfo)
                objCatCtrl.CopyToLanguages(objCatInfo)
            End If

            Return objCatInfo

        End Function

        Private Function UnZipImportFile(ByVal ZipFileName As String, ByVal DestFolder As String, ByVal PS As DotNetNuke.Entities.Portals.PortalSettings) As String

            Dim sourceFileName As String = System.IO.Path.GetFileName(ZipFileName)

            Dim objZipEntry As ZipEntry
            Dim strMessage As String = ""
            Dim strFileName As String = ""
            Dim strExtension As String
            Dim outStream As FileStream
            Dim buff(2047) As Byte
            Dim bytes As Integer
            Dim imgCount As Integer = 0

            'create the Zip Input Stream and parse it for the files
            Dim objZipInputStream As New ZipInputStream(File.OpenRead(ZipFileName))
            objZipEntry = objZipInputStream.GetNextEntry
            While Not objZipEntry Is Nothing
                If Not objZipEntry.IsDirectory Then
                    strFileName = Path.GetFileName(objZipEntry.Name)
                    If strFileName <> "" Then
                        strExtension = Path.GetExtension(strFileName)
                        If InStr(1, "," & PS.HostSettings("FileExtensions").ToString.ToUpper, "," & Replace(strExtension.ToUpper, ".", "")) <> 0 Or IsImageFile(strExtension) Then
                            Try
                                Dim folderPath As String = System.IO.Path.GetDirectoryName(DestFolder & Replace(objZipEntry.Name, "/", "\"))
                                Dim Dinfo As New DirectoryInfo(folderPath)
                                If Dinfo.Exists Then
                                    Dim zipEntryFileName As String = DestFolder & Replace(objZipEntry.Name, "/", "\")
                                    outStream = File.Create(zipEntryFileName, 2048)
                                    Do While True
                                        bytes = objZipInputStream.Read(buff, 0, 2048)
                                        If bytes = 0 Then
                                            Exit Do
                                        End If
                                        outStream.Write(buff, 0, bytes)
                                    Loop
                                    outStream.Close()
                                    imgCount = imgCount + 1
                                End If
                            Catch ex As Exception
                                If Not objZipInputStream Is Nothing Then
                                    objZipInputStream.Close()
                                End If
                                Return ex.Message
                            End Try
                        End If
                    End If
                End If

                objZipEntry = objZipInputStream.GetNextEntry
            End While

            objZipInputStream.Close()

            strMessage = "<br/>Imported Zip Items: " & imgCount.ToString
            Return strMessage

        End Function



    End Class

End Namespace
