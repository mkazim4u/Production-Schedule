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

Namespace NEvoWeb.Modules.NB_Store

    Public Class ImportCSV

        Private strMsg As String = ""

#Region "Products"

        Public Function ImportCSVProducts(ByVal PS As Portals.PortalSettings, ByVal ImportFile As String, ByVal UpdateExistingProductsFlg As Boolean, ByVal CreateCategoriesFlg As Boolean, Optional ByVal RowDelimeter As String = vbCrLf, Optional ByVal FieldDelimeter As String = ",", Optional ByVal FieldQualifier As String = """", Optional ByVal FieldEscapedQualifier As String = """""") As String
            strMsg = ""
            Try
                If RowDelimeter = "" Then RowDelimeter = vbCrLf
                If FieldDelimeter = "" Then FieldDelimeter = ","
                If String.IsNullOrEmpty(FieldQualifier) Then FieldQualifier = """"
                If String.IsNullOrEmpty(FieldEscapedQualifier) Then FieldEscapedQualifier = """"""

                Dim ImportXMLFile As String = ""
                Dim CSVdata As String = ""
                Dim CSVlines As String()
                Dim strXML As String = ""
                Dim strCatXML As String = ""
                Dim xmlDoc As New Xml.XmlDataDocument
                Dim objFile As New FileObj
                Dim xmlDocProdMap As New Xml.XmlDataDocument
                Dim strTemp As String = ""

                CSVdata = objFile.GetFileContents(ImportFile)

                CSVlines = SplitCSVdata(PS.PortalId, CSVdata, RowDelimeter, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)

                strCatXML = buildCSVCategoryXML(PS, CSVlines, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)

                strXML = "<root version=""1.0"">"

                Dim ProdIDXRef As New Hashtable

                strXML &= buildCSVProductXML(PS, CSVlines, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)
                strXML &= strCatXML
                strXML &= "<shipping></shipping>"
                strXML &= "<tax></tax>"
                strXML &= "</root>"

                xmlDoc = New Xml.XmlDataDocument
                xmlDoc.LoadXml(strXML)
                ImportXMLFile = Replace(ImportFile, ".csv", ".xml")
                xmlDoc.Save(ImportXMLFile)


                'run normal import process on created xml file.
                Dim objImpCtrl As New Import
                strMsg &= objImpCtrl.ImportProducts(PS, ImportXMLFile, UpdateExistingProductsFlg, CreateCategoriesFlg)
            Catch ex As Exception
                strMsg &= ex.ToString
            End Try
            Return strMsg
        End Function

        Private Function buildCSVProductXML(ByVal PS As Portals.PortalSettings, ByVal CSVlines As String(), ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal FieldEscapedQualifier As String) As String
            Dim strCSVline() As String
            Dim lp As Integer
            Dim lp2 As Integer
            Dim strProdXML As String
            Dim xmlDoc As New Xml.XmlDataDocument
            Dim objSCtrl As New SettingsController
            Dim xmlDocCatMap As New Xml.XmlDataDocument
            Dim xmlDocProdMap As New Xml.XmlDataDocument
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim strTemp As String = ""
            Dim intCatCol As Integer = -1
            Dim intParCatCol As Integer = -1
            Dim intLangCol As Integer = -1
            Dim intPRefCol As Integer = -1
            Dim supportedLanguages As LocaleCollection = GetValidLocales()

            objSTInfo = objSCtrl.GetSettingsTextNotCached(PS.PortalId, "categoryCSV.ImportMapping", "None")
            xmlDocCatMap.LoadXml(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText))
            objSTInfo = objSCtrl.GetSettingsTextNotCached(PS.PortalId, "productCSV.ImportMapping", "None")
            xmlDocProdMap.LoadXml(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText))

            'get the product ref column.  NOTE: Product Ref must be included for multiple lang import.
            Dim strPRefCol As String = xmlDocProdMap.SelectSingleNode("P/NB_Store_ProductsInfo/ProductRef").InnerText
            If strPRefCol.StartsWith("[COL:") Then
                intPRefCol = CInt(Replace(Replace(strPRefCol, "[COL:", ""), "]", ""))
            End If

            'check if the Language records are included in the csv
            Dim strLangCol As String = xmlDocProdMap.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText
            If strLangCol.StartsWith("[COL:") Then
                intLangCol = CInt(Replace(Replace(strLangCol, "[COL:", ""), "]", ""))
            End If

            'get category column
            Dim strCatCol As String = xmlDocCatMap.SelectSingleNode("NB_Store_CategoriesInfo/CategoryName").InnerText
            If strCatCol.StartsWith("[COL:") Then
                intCatCol = CInt(Replace(Replace(strCatCol, "[COL:", ""), "]", ""))
            End If

            'get parent category column
            Dim strParCatCol As String = xmlDocCatMap.SelectSingleNode("NB_Store_CategoriesInfo/ParentName").InnerText
            If strParCatCol.StartsWith("[COL:") Then
                intParCatCol = CInt(Replace(Replace(strParCatCol, "[COL:", ""), "]", ""))
            End If

            'put csv into an arrayList for processing.
            ' and build xml Import file

            'build category xref, so cats can be inserted
            Dim CatXRef As New Hashtable
            CatXRef = getCSVcategoryXref(CSVlines, xmlDocCatMap.OuterXml, intPRefCol, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)

            'get category xml and category xref
            Dim ProdXRef As New Hashtable
            Dim ProdProdXref As New Hashtable
            Dim UnqID As Integer = 0
            strProdXML = "<products>"
            If intPRefCol > 0 Then
                If intLangCol > 0 Then
                    If intPRefCol > 0 Then
                        For Each Lang As String In supportedLanguages
                            UnqID = 0 ' reset unqid so the language id match.
                            strProdXML &= "<" & Lang & ">"
                            For lp = 0 To CSVlines.GetUpperBound(0)
                                If CSVlines(lp) <> "" Then
                                    strCSVline = SplitCustom(CSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)
                                    strTemp = xmlDocProdMap.OuterXml
                                    For lp2 = 0 To strCSVline.GetUpperBound(0)
                                        strTemp = Replace(strTemp, "[COL:" & (lp2 + 1).ToString & "]", "<![CDATA[" & strCSVline(lp2) & "]]>")
                                    Next
                                    xmlDoc = New Xml.XmlDataDocument
                                    xmlDoc.LoadXml(strTemp)
                                    strLangCol = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/UnitCost").InnerText

                                    strLangCol = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText
                                    If strLangCol = Lang Then
                                        strPRefCol = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductRef").InnerText
                                        If Not ProdXRef.Contains(strPRefCol & "_" & Lang) And strPRefCol <> "" Then
                                            Try
                                                Dim ProdID As String
                                                If ProdProdXref.Contains(strCSVline(intPRefCol - 1)) Then
                                                    ProdID = ProdProdXref(strCSVline(intPRefCol - 1))
                                                Else
                                                    ProdID = (ProdXRef.Count + 1).ToString
                                                End If
                                                xmlDoc = GetProductXML(PS.PortalId, ProdID, xmlDoc, CatXRef, UnqID, Lang)

                                                ProdXRef.Add(strPRefCol & "_" & Lang, xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText)
                                                strProdXML &= xmlDoc.OuterXml
                                            Catch ex As Exception
                                                strMsg &= "Product Ref: """ & strPRefCol & """ failed to import.<br/>"
                                            End Try
                                        End If
                                        If Not ProdProdXref.Contains(strCSVline(intPRefCol - 1)) Then
                                            ProdProdXref.Add(strCSVline(intPRefCol - 1), ProdXRef(strPRefCol & "_" & Lang))
                                        End If
                                    End If
                                End If
                            Next
                            strProdXML &= "</" & Lang & ">"
                            strProdXML = Replace(strProdXML, "[LANG]", Lang)
                        Next
                    End If
                Else
                    strProdXML &= "<" & GetCurrentCulture() & ">"
                    For lp = 0 To CSVlines.GetUpperBound(0)
                        If CSVlines(lp) <> "" Then
                            strCSVline = SplitCustom(CSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)
                            strTemp = xmlDocProdMap.OuterXml
                            For lp2 = 0 To strCSVline.GetUpperBound(0)
                                strTemp = Replace(strTemp, "[COL:" & (lp2 + 1).ToString & "]", "<![CDATA[" & strCSVline(lp2) & "]]>")
                            Next
                            xmlDoc = New Xml.XmlDataDocument
                            xmlDoc.LoadXml(strTemp)
                            strPRefCol = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductRef").InnerText
                            If Not ProdXRef.Contains(strPRefCol) And strPRefCol <> "" Then

                                Try
                                    xmlDoc = GetProductXML(PS.PortalId, (ProdXRef.Count + 1).ToString, xmlDoc, CatXRef, UnqID, GetCurrentCulture())
                                    ProdXRef.Add(strPRefCol, xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText)
                                    strProdXML &= xmlDoc.OuterXml
                                Catch ex As Exception
                                    strMsg &= "Product Ref: """ & strPRefCol & """ failed to import.<br/>"
                                End Try

                            End If
                        End If
                    Next
                    strProdXML &= "</" & GetCurrentCulture() & ">"
                    strProdXML = Replace(strProdXML, "[LANG]", GetCurrentCulture())
                End If
            Else
                strMsg = "ERROR - NO Product Ref Column in CSV file."
            End If
            strProdXML &= "</products>"
            Return strProdXML
        End Function

        Private Function GetProductXML(ByVal PortalID As Integer, ByVal ProductID As String, ByVal xmlDoc As Xml.XmlDataDocument, ByVal CatXRef As Hashtable, ByRef UnqID As Integer, ByVal Lang As String) As Xml.XmlDataDocument
            Dim xmlNodList As Xml.XmlNodeList
            Dim xmlNod As Xml.XmlNode
            Dim strTemp As String

            xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText = ProductID
            xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/PortalID").InnerText = PortalID.ToString
            xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText = Lang

            'do categories
            xmlNodList = xmlDoc.SelectNodes("P/C/NB_Store_ProductCategoryInfo")
            For Each xmlNod In xmlNodList
                xmlNod.SelectSingleNode("ProductID").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText
                xmlNod.SelectSingleNode("CategoryID").InnerText = CatXRef(xmlDoc.SelectSingleNode("P/C/NB_Store_ProductCategoryInfo/CategoryID").InnerText)
            Next

            'do models
            xmlNodList = xmlDoc.SelectNodes("P/M/NB_Store_ModelInfo")
            For Each xmlNod In xmlNodList
                xmlNod.SelectSingleNode("ProductID").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText
                UnqID += 1
                xmlNod.SelectSingleNode("ModelID").InnerText = UnqID
                xmlNod.SelectSingleNode("QtyTransDate").InnerText = getFormatToday()
                xmlNod.SelectSingleNode("QtyTrans").InnerText = "0"
                xmlNod.SelectSingleNode("PortalID").InnerText = PortalID.ToString

                If xmlNod.SelectSingleNode("ListOrder").InnerText = "" Then
                    xmlNod.SelectSingleNode("ListOrder").InnerText = "1"
                End If
                If xmlNod.SelectSingleNode("QtyRemaining").InnerText = "" Then
                    xmlNod.SelectSingleNode("QtyRemaining").InnerText = "-1"
                End If
                xmlNod.SelectSingleNode("Lang").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText
            Next

            'do images
            xmlNodList = xmlDoc.SelectNodes("P/I/NB_Store_ProductImageInfo")
            For Each xmlNod In xmlNodList
                xmlNod.SelectSingleNode("ProductID").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText
                UnqID += 1
                xmlNod.SelectSingleNode("ImageID").InnerText = UnqID
                If xmlNod.SelectSingleNode("ListOrder").InnerText = "" Then
                    xmlNod.SelectSingleNode("ListOrder").InnerText = "1"
                End If
                'replace image ext with jpg
                strTemp = xmlNod.SelectSingleNode("ImagePath").InnerText
                Try
                    xmlNod.SelectSingleNode("ImagePath").InnerText = System.IO.Path.GetDirectoryName(strTemp) & "\" & System.IO.Path.GetFileNameWithoutExtension(strTemp) & ".jpg"
                Catch ex As Exception
                    'Invalid image so just make it blank
                    xmlNod.SelectSingleNode("ImagePath").InnerText = ""
                End Try
                strTemp = xmlNod.SelectSingleNode("ImageURL").InnerText
                Try
                    xmlNod.SelectSingleNode("ImageURL").InnerText = System.IO.Path.GetDirectoryName(strTemp) & "\" & System.IO.Path.GetFileNameWithoutExtension(strTemp) & ".jpg"
                Catch ex As Exception
                    'Invalid image so just make it blank
                    xmlNod.SelectSingleNode("ImageURL").InnerText = ""
                End Try
                xmlNod.SelectSingleNode("Lang").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText
            Next

            'do docs
            xmlNodList = xmlDoc.SelectNodes("P/D/NB_Store_ProductDocInfo")
            For Each xmlNod In xmlNodList
                xmlNod.SelectSingleNode("ProductID").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText
                UnqID += 1
                xmlNod.SelectSingleNode("DocID").InnerText = UnqID
                If xmlNod.SelectSingleNode("ListOrder").InnerText = "" Then
                    xmlNod.SelectSingleNode("ListOrder").InnerText = "1"
                End If
                'replace image ext with jpg
                strTemp = xmlNod.SelectSingleNode("DocPath").InnerText
                xmlNod.SelectSingleNode("FileName").InnerText = System.IO.Path.GetFileNameWithoutExtension(strTemp)
                xmlNod.SelectSingleNode("FileExt").InnerText = System.IO.Path.GetExtension(strTemp)
                xmlNod.SelectSingleNode("Lang").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText
            Next

            'do options
            Dim optID As Integer
            Dim xmlNodList2 As Xml.XmlNodeList
            Dim xmlNod2 As Xml.XmlNode

            xmlNodList = xmlDoc.SelectNodes("P/options/O")
            For Each xmlNod In xmlNodList
                xmlNod.SelectSingleNode("NB_Store_OptionInfo/ProductID").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/ProductID").InnerText
                UnqID += 1
                optID = UnqID
                xmlNod.SelectSingleNode("NB_Store_OptionInfo/OptionID").InnerText = UnqID
                If xmlNod.SelectSingleNode("NB_Store_OptionInfo/ListOrder").InnerText = "" Then
                    xmlNod.SelectSingleNode("NB_Store_OptionInfo/ListOrder").InnerText = "1"
                End If
                xmlNod.SelectSingleNode("NB_Store_OptionInfo/Lang").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText

                'do option values
                xmlNodList2 = xmlNod.SelectNodes("OV/NB_Store_OptionValueInfo")
                For Each xmlNod2 In xmlNodList2
                    UnqID += 1
                    xmlNod2.SelectSingleNode("OptionValueID").InnerText = UnqID
                    xmlNod2.SelectSingleNode("OptionID").InnerText = optID
                    If xmlNod2.SelectSingleNode("ListOrder").InnerText = "" Then
                        xmlNod2.SelectSingleNode("ListOrder").InnerText = "1"
                    End If
                    xmlNod2.SelectSingleNode("Lang").InnerText = xmlDoc.SelectSingleNode("P/NB_Store_ProductsInfo/Lang").InnerText
                Next
            Next

            Return xmlDoc
        End Function

#End Region

#Region "Categories"

        Private Function buildCSVCategoryXML(ByVal PS As Portals.PortalSettings, ByVal CSVlines As String(), ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal FieldEscapedQualifier As String) As String
            Dim strCSVline() As String
            Dim lp As Integer
            Dim lp2 As Integer
            Dim strCatXML As String
            Dim xmlDoc As New Xml.XmlDataDocument
            Dim objSCtrl As New SettingsController
            Dim xmlDocCatMap As New Xml.XmlDataDocument
            Dim xmlDocProdMap As New Xml.XmlDataDocument
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim strTemp As String = ""
            Dim intCatCol As Integer = -1
            Dim intParCatCol As Integer = -1
            Dim intLangCol As Integer = -1
            Dim intPRefCol As Integer = -1
            Dim supportedLanguages As LocaleCollection = GetValidLocales()

            objSTInfo = objSCtrl.GetSettingsTextNotCached(PS.PortalId, "categoryCSV.ImportMapping", "None")
            xmlDocCatMap.LoadXml(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText))
            objSTInfo = objSCtrl.GetSettingsTextNotCached(PS.PortalId, "productCSV.ImportMapping", "None")
            xmlDocProdMap.LoadXml(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText))

            'get the product ref column.  NOTE: Product Ref must be included for multiple lang import.
            Dim strPRefCol As String = xmlDocProdMap.SelectSingleNode("P/NB_Store_ProductsInfo/ProductRef").InnerText
            If strPRefCol.StartsWith("[COL:") Then
                intPRefCol = CInt(Replace(Replace(strPRefCol, "[COL:", ""), "]", ""))
            End If

            'check if the Language records are included in the csv
            Dim strLangCol As String = xmlDocCatMap.SelectSingleNode("NB_Store_CategoriesInfo/Lang").InnerText
            If strLangCol.StartsWith("[COL:") Then
                intLangCol = CInt(Replace(Replace(strLangCol, "[COL:", ""), "]", ""))
            End If

            'get category column
            Dim strCatCol As String = xmlDocCatMap.SelectSingleNode("NB_Store_CategoriesInfo/CategoryName").InnerText
            If strCatCol.StartsWith("[COL:") Then
                intCatCol = CInt(Replace(Replace(strCatCol, "[COL:", ""), "]", ""))
            End If

            'get parent category column
            Dim strParCatCol As String = xmlDocCatMap.SelectSingleNode("NB_Store_CategoriesInfo/ParentName").InnerText
            If strParCatCol.StartsWith("[COL:") Then
                intParCatCol = CInt(Replace(Replace(strParCatCol, "[COL:", ""), "]", ""))
            End If

            'put csv into an arrayList for processing.
            ' and build xml Import file

            'build category xref, so parent cats can be inserted
            Dim ParCatXRef As New Hashtable
            If intParCatCol >= 0 Then
                ParCatXRef = getCSVcategoryXref(CSVlines, xmlDocCatMap.OuterXml, intPRefCol, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)
            End If

            'get category xml and category xref
            Dim CatXRef As New Hashtable
            Dim ProdCatXref As New Hashtable
            strCatXML = "<categories>"
            If intPRefCol > 0 Then
                If intLangCol > 0 Then
                    If intPRefCol > 0 Then
                        For Each Lang As String In supportedLanguages
                            strCatXML &= "<" & Lang & ">"

                            For lp = 0 To CSVlines.GetUpperBound(0)
                                If CSVlines(lp) <> "" Then
                                    strCSVline = SplitCustom(CSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)
                                    strTemp = xmlDocCatMap.OuterXml
                                    For lp2 = 0 To strCSVline.GetUpperBound(0)
                                        strTemp = Replace(strTemp, "[COL:" & (lp2 + 1).ToString & "]", "<![CDATA[" & strCSVline(lp2) & "]]>")
                                    Next
                                    xmlDoc = New Xml.XmlDataDocument
                                    xmlDoc.LoadXml(strTemp)
                                    strLangCol = xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/Lang").InnerText
                                    If strLangCol = Lang Then
                                        strCatCol = xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryName").InnerText
                                        strParCatCol = xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ParentName").InnerText
                                        If Not CatXRef.Contains(strCatCol & "_" & Lang) Then
                                            If ProdCatXref.Contains(strCSVline(intPRefCol - 1)) Then
                                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryID").InnerText = ProdCatXref(strCSVline(intPRefCol - 1))
                                            Else
                                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryID").InnerText = (CatXRef.Count + 1).ToString
                                            End If
                                            xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/PortalID").InnerText = PS.PortalId.ToString
                                            xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/Lang").InnerText = Lang
                                            xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ParentCategoryID").InnerText = "0"

                                            If xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ListOrder").InnerText = "" Then
                                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ListOrder").InnerText = "1"
                                            End If
                                            xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ProductCount").InnerText = "0"
                                            xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CreatedByUser").InnerText = "-1"
                                            xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CreatedDate").InnerText = getFormatToday()

                                            If intParCatCol >= 0 Then
                                                If Not ParCatXRef(strParCatCol) Is Nothing Then
                                                    xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ParentCategoryID").InnerText = ParCatXRef(strParCatCol)
                                                End If
                                            End If
                                            CatXRef.Add(strCatCol & "_" & Lang, xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryID").InnerText)
                                            strCatXML &= xmlDoc.OuterXml
                                        End If
                                        If Not ProdCatXref.Contains(strCSVline(intPRefCol - 1)) Then
                                            ProdCatXref.Add(strCSVline(intPRefCol - 1), CatXRef(strCatCol & "_" & Lang))
                                        End If
                                    End If
                                End If
                            Next
                            strCatXML &= "</" & Lang & ">"
                        Next
                    End If
                Else
                    strCatXML &= "<" & GetCurrentCulture() & ">"
                    For lp = 0 To CSVlines.GetUpperBound(0)
                        If CSVlines(lp) <> "" Then
                            strCSVline = SplitCustom(CSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)
                            strTemp = xmlDocCatMap.OuterXml
                            For lp2 = 0 To strCSVline.GetUpperBound(0)
                                strTemp = Replace(strTemp, "[COL:" & (lp2 + 1).ToString & "]", "<![CDATA[" & strCSVline(lp2) & "]]>")
                            Next
                            xmlDoc = New Xml.XmlDataDocument
                            xmlDoc.LoadXml(strTemp)
                            strCatCol = xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryName").InnerText
                            strParCatCol = xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ParentName").InnerText
                            If Not CatXRef.Contains(strCatCol) Then
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryID").InnerText = (CatXRef.Count + 1).ToString
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/PortalID").InnerText = PS.PortalId.ToString
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/Lang").InnerText = GetCurrentCulture()
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ParentCategoryID").InnerText = "0"

                                If xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ListOrder").InnerText = "" Then
                                    xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ListOrder").InnerText = "1"
                                End If
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ProductCount").InnerText = "0"
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CreatedByUser").InnerText = "-1"
                                xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CreatedDate").InnerText = getFormatToday()

                                If intParCatCol >= 0 Then
                                    If Not ParCatXRef(strParCatCol) Is Nothing Then
                                        xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/ParentCategoryID").InnerText = ParCatXRef(strParCatCol)
                                    End If
                                End If
                                CatXRef.Add(strCatCol, (CatXRef.Count + 1).ToString)
                                strCatXML &= xmlDoc.OuterXml
                            End If
                        End If
                    Next
                    strCatXML &= "</" & GetCurrentCulture() & ">"
                End If
            Else
                strMsg = "ERROR - NO Product Ref Column in CSV file."
            End If
            strCatXML &= "</categories>"
            strCatXML = Replace(strCatXML, "[LANG]", GetCurrentCulture())
            Return strCatXML
        End Function

#End Region

#Region "Models"

        Public Function ImportCSVModels(ByVal PS As Portals.PortalSettings, ByVal ImportFile As String, Optional ByVal RowDelimeter As String = vbCrLf, Optional ByVal FieldDelimeter As String = ",", Optional ByVal FieldQualifier As String = """", Optional ByVal FieldEscapedQualifier As String = """""") As String
            strMsg = "Completed.<br/><br/>"
            Try
                If RowDelimeter = "" Then RowDelimeter = vbCrLf
                If FieldDelimeter = "" Then FieldDelimeter = ","
                If String.IsNullOrEmpty(FieldQualifier) Then FieldQualifier = """"
                If String.IsNullOrEmpty(FieldEscapedQualifier) Then FieldEscapedQualifier = """"""

                Dim ImportXMLFile As String = ""
                Dim CSVdata As String = ""
                Dim CSVlines As String()
                Dim strXML As String = ""
                Dim xmlDoc As New Xml.XmlDataDocument
                Dim objFile As New FileObj
                Dim xmlDocModMap As New Xml.XmlDataDocument
                Dim strTemp As String = ""

                CSVdata = objFile.GetFileContents(ImportFile)

                CSVlines = SplitCSVdata(PS.PortalId, CSVdata, RowDelimeter, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)

                DoModelImport(PS, CSVlines, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)
                
            Catch ex As Exception
                strMsg &= ex.ToString
            End Try
            Return strMsg
        End Function

        Private Sub DoModelImport(ByVal PS As Portals.PortalSettings, ByVal CSVlines As String(), ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal FieldEscapedQualifier As String)
            Dim strCSVline() As String
            Dim lp As Integer
            Dim lp2 As Integer
            Dim xmlDoc As New Xml.XmlDataDocument
            Dim objSCtrl As New SettingsController
            Dim xmlDocModMap As New Xml.XmlDataDocument
            Dim strTemp As String = ""
            Dim intMRefCol As Integer = -1
            Dim intIDCol As Integer = -1
            Dim intLangCol As Integer = -1
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim strMapping As String = GetStoreSettingText(PS.PortalId, "modelCSV.ImportMapping", "None", True, True)
            Dim objMInfoDB As NB_Store_ModelInfo = Nothing
            Dim objMInfo As New NB_Store_ModelInfo
            Dim objPCtrl As New ProductController
            Dim intFail As Integer = 0
            Dim intOK As Integer = 0

            If strMapping <> "" Then

                xmlDocModMap.LoadXml(strMapping)

                'get the model ref and Id columns. 
                Dim strMRefCol As String = xmlDocModMap.SelectSingleNode("NB_Store_ModelInfo/ModelRef").InnerText
                If strMRefCol.StartsWith("[COL:") Then
                    intMRefCol = CInt(Replace(Replace(strMRefCol, "[COL:", ""), "]", ""))
                End If
                Dim strMIDCol As String = xmlDocModMap.SelectSingleNode("NB_Store_ModelInfo/ModelID").InnerText
                If strMIDCol.StartsWith("[COL:") Then
                    intIDCol = CInt(Replace(Replace(strMIDCol, "[COL:", ""), "]", ""))
                End If
                Dim strLangCol As String = xmlDocModMap.SelectSingleNode("NB_Store_ModelInfo/Lang").InnerText
                If strLangCol.StartsWith("[COL:") Then
                    intLangCol = CInt(Replace(Replace(strLangCol, "[COL:", ""), "]", ""))
                End If


                'Build import XML
                Dim ProdXRef As New Hashtable
                Dim ProdProdXref As New Hashtable
                Dim UnqID As Integer = 0

                If intMRefCol > 0 Or intIDCol > 0 Then
                    For lp = 0 To CSVlines.GetUpperBound(0)
                        If CSVlines(lp) <> "" Then
                            'inject data into xml structure
                            strCSVline = SplitCustom(CSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)

                            'check if langauge is being passed by CSV, if not then use mapping language.
                            If intLangCol > 0 Then
                                strLangCol = strCSVline(intLangCol - 1)
                            End If

                            'get DB Info
                            objMInfoDB = Nothing
                            If intIDCol > 0 Then
                                'Get data with the ModelID
                                If IsNumeric(strCSVline(intIDCol - 1)) Then
                                    objMInfoDB = objPCtrl.GetModel(strCSVline(intIDCol - 1), strLangCol)
                                End If
                            End If
                            If objMInfoDB Is Nothing And intMRefCol > 0 Then
                                'Get data with the ModelRef
                                objMInfoDB = objPCtrl.GetModelByRef(strCSVline(intMRefCol - 1), strLangCol)
                            End If

                            If Not objMInfoDB Is Nothing Then
                                Dim strTempDB As String
                                Dim xmlDocDB As New Xml.XmlDataDocument

                                Try

                                    strTempDB = DotNetNuke.Common.Utilities.XmlUtils.Serialize(objMInfoDB)

                                    xmlDocDB.LoadXml(strTempDB) ' get the DB obj into a xml format

                                    'merge the mapping data into the DB xml DOC.
                                    Dim xmlNodList As Xml.XmlNodeList
                                    xmlNodList = xmlDocModMap.SelectNodes("NB_Store_ModelInfo/*")

                                    For Each xmlNod As Xml.XmlNode In xmlNodList
                                        If xmlNod.InnerXml.StartsWith("[COL:") Then
                                            ReplaceXMLNode(xmlDocDB, "NB_Store_ModelInfo/" & xmlNod.Name, xmlNod.InnerXml, False)
                                        End If
                                    Next

                                    'replace with CSV data
                                    strTemp = xmlDocDB.OuterXml
                                    For lp2 = 0 To strCSVline.GetUpperBound(0)
                                        strTemp = Replace(strTemp, "[COL:" & (lp2 + 1).ToString & "]", "<![CDATA[" & strCSVline(lp2) & "]]>")
                                    Next

                                    'replace tokens
                                    strTemp = Replace(strTemp, "[LANG]", GetCurrentCulture())
                                    strTemp = Replace(strTemp, "[PORTALID]", PS.PortalId)

                                    objMInfo = DotNetNuke.Common.Utilities.XmlUtils.Deserialize(strTemp, objMInfo.GetType)

                                    objPCtrl.UpdateObjModel(objMInfo)
                                    intOK += 1

                                Catch ex As Exception
                                    UpdateLog("MODEL IMPORT ERROR : " & ex.ToString)
                                    intFail += 1
                                End Try
                            Else
                                intFail += 1
                            End If
                        End If
                    Next

                Else
                    strMsg = "ERROR - NO ModelRef or ModelID CSV Mapping."
                End If
            Else
                strMsg = "ERROR - NO Model Mapping ""modelCSV.ImportMapping"" Template."
            End If

            strMsg &= "Updated: " & intOK.ToString & "<br/>"
            strMsg &= "Failed: " & intFail.ToString & "<br/>"

        End Sub


#End Region


#Region "General Methods"


        'NEW: Custom split function (specify Delimiter, Field text qualifier, escaped qualifier)
        Private Function SplitCustom( _
                ByVal Expression As String, _
                ByVal Delimiter As String, _
                ByVal Qualifier As String, _
                ByVal EscapedQualifier As String, _
                ByVal IgnoreCase As Boolean) As String()

            'Split("'Programmers often write, \'Hello World!\'','\'this works\'',\'this too\',3.14,'25,000.00'", _
            '      ",", _
            '      "'", _
            '      "\'", _
            '      True)
            '
            ''Will split expression into following 5 items:
            '
            '1) Programmers often write, 'Hello World!'
            '2) 'this works'
            '3) 'this too'
            '4) 3.14
            '5) 25,000.00
            '
            'NOTE: when excel saves a worksheet as a CSV file, the defaults it uses are: 
            'delimiter=comma(,)
            'text qualifier=double-quote(")
            'escaped qualifier=two double-quotes("")


            Dim qualifierState As Boolean = False
            Dim startIndex As Integer = 0
            Dim values As New System.Collections.ArrayList
            Dim fieldValue As String = ""

            Dim ReplacementQualifier As Char = Convert.ToChar(7) 'non-printing ASCII character (Bell)

            If Not String.IsNullOrEmpty(EscapedQualifier) Then
                Dim strFields() As String
                strFields = Expression.Split(Delimiter)
                Expression = ""
                For lp As Integer = 0 To strFields.GetUpperBound(0)
                    If strFields(lp) = EscapedQualifier Then
                        ' don;t escape empty fields
                        Expression &= strFields(lp) & Delimiter
                    Else
                        Expression &= strFields(lp).Replace(EscapedQualifier, ReplacementQualifier) & Delimiter
                    End If
                Next
                Expression = Expression.TrimEnd(Delimiter)

            End If

            For charIndex As Integer = 0 To Expression.Length - 1
                If Not Qualifier Is Nothing _
                    AndAlso String.Compare(Expression.Substring(charIndex, Qualifier.Length), Qualifier, IgnoreCase) = 0 Then
                    qualifierState = Not qualifierState

                ElseIf Not qualifierState _
                    AndAlso Not Delimiter Is Nothing _
                    AndAlso String.Compare(Expression.Substring(charIndex, Delimiter.Length), Delimiter, IgnoreCase) = 0 Then
                    'get field value (triming qualifier and replacing escapedqualifier with normal text)
                    fieldValue = Expression.Substring(startIndex, charIndex - startIndex).Trim(Qualifier).Replace(ReplacementQualifier, Qualifier)
                    values.Add(fieldValue)
                    startIndex = charIndex + 1

                End If
            Next

            If startIndex < Expression.Length Then
                'get field value (triming qualifier and replacing escapedqualifier with normal text)
                fieldValue = Expression.Substring(startIndex, Expression.Length - startIndex).Trim(Qualifier).Replace(ReplacementQualifier, Qualifier)
                values.Add(fieldValue)

            End If

            Dim returnValues(values.Count - 1) As String
            values.CopyTo(returnValues)

            Return returnValues

        End Function


        Private Function getCSVcategoryXref(ByVal strCSVlines As String(), ByVal CatMapping As String, ByVal intPRefCol As Integer, ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal FieldEscapedQualifier As String) As Hashtable
            Dim CatXRef As New Hashtable
            Dim xmlDoc As Xml.XmlDataDocument
            Dim strCatCol As String = ""
            Dim strCSVline As String()
            Dim strTemp As String = ""
            Dim ProdCatXref As New Hashtable

            For lp As Integer = 0 To strCSVlines.GetUpperBound(0)
                If strCSVlines(lp) <> "" Then
                    strCSVline = SplitCustom(strCSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)

                    'clear invalid fieldqualifier from empty line
                    For lp3 As Integer = 0 To strCSVline.GetUpperBound(0)
                        strCSVline(lp3) = Replace(strCSVline(lp3), """""""""""", "")
                    Next

                    strTemp = CatMapping
                    For lp2 As Integer = 0 To strCSVline.GetUpperBound(0)
                        strTemp = Replace(strTemp, "[COL:" & (lp2 + 1).ToString & "]", "<![CDATA[" & strCSVline(lp2) & "]]>")
                    Next
                    xmlDoc = New Xml.XmlDataDocument
                    xmlDoc.LoadXml(strTemp)
                    strCatCol = xmlDoc.SelectSingleNode("NB_Store_CategoriesInfo/CategoryName").InnerText
                    If Not CatXRef.Contains(strCatCol) Then
                        If ProdCatXref.Contains(strCSVline(intPRefCol - 1)) And strCSVline(intPRefCol - 1) <> "" Then
                            CatXRef.Add(strCatCol, ProdCatXref(strCSVline(intPRefCol - 1)))
                        Else
                            CatXRef.Add(strCatCol, (CatXRef.Count + 1).ToString)
                        End If
                    End If
                    If Not ProdCatXref.Contains(strCSVline(intPRefCol - 1)) Then
                        ProdCatXref.Add(strCSVline(intPRefCol - 1), CatXRef(strCatCol))
                    End If
                End If
            Next

            Return CatXRef
        End Function

        Private Function getFormatToday() As String
            Return Year(Today) & "-" & Month(Today).ToString("00") & "-" & Day(Today).ToString("00") & "T00:00:00"
        End Function

        Private Function SplitCSVdata(ByVal PortalID As Integer, ByVal CSVdata As String, ByVal RowDelimeter As String, ByVal FieldDelimeter As String, ByVal FieldQualifier As String, ByVal FieldEscapedQualifier As String) As String()
            Dim CSVlines As String()
            Dim xmlDoc As New Xml.XmlDataDocument
            Dim objSCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim xmlNodList As Xml.XmlNodeList
            Dim xmlNod As Xml.XmlNode

            CSVlines = Split(CSVdata, RowDelimeter)

            Try
                objSTInfo = objSCtrl.GetSettingsTextNotCached(PortalID, "replaceCSV.ImportMapping", "None")
                If Not objSTInfo Is Nothing Then
                    Dim col As String = ""
                    Dim str1 As String = ""
                    Dim str2 As String = ""
                    Dim substr As Boolean = False
                    Dim CSVline As String()

                    xmlDoc.LoadXml(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText))

                    xmlNodList = xmlDoc.SelectNodes("root/item")
                    For Each xmlNod In xmlNodList
                        col = xmlNod.Attributes(0).InnerText
                        str1 = xmlNod.Attributes(1).InnerText
                        str2 = xmlNod.Attributes(2).InnerText
                        substr = xmlNod.Attributes(3).InnerText
                        If str1 <> "" Then
                            For lp As Integer = 0 To CSVlines.GetUpperBound(0)
                                CSVline = SplitCustom(CSVlines(lp), FieldDelimeter, FieldQualifier, FieldEscapedQualifier, True)
                                For lp2 As Integer = 0 To CSVline.GetUpperBound(0)
                                    If Not IsNumeric(col) Then
                                        'replace all columns
                                        If substr Then
                                            CSVline(lp2) = Replace(CSVline(lp2), str1, str2)
                                        Else
                                            If CSVline(lp2) = str1 Then
                                                CSVline(lp2) = Replace(CSVline(lp2), str1, str2)
                                            End If
                                        End If
                                    Else
                                        'replace only specified columns
                                        If (lp2 + 1) = CInt(col) Then
                                            If substr Then
                                                CSVline(lp2) = Replace(CSVline(lp2), str1, str2)
                                            Else
                                                If CSVline(lp2) = str1 Then
                                                    CSVline(lp2) = Replace(CSVline(lp2), str1, str2)
                                                End If
                                            End If
                                        End If
                                    End If
                                Next
                                CSVlines(lp) = JoinCustom(CSVline, FieldDelimeter, FieldQualifier, FieldEscapedQualifier)
                            Next
                        End If
                    Next

                End If
            Catch ex As Exception
                strMsg &= "replaceCSV Error: " & ex.ToString
            End Try

            Return CSVlines
        End Function

        Private Function JoinCustom(ByVal CSVLines As String(), ByVal FieldDelimiter As String, ByVal FieldQualifier As String, ByVal FieldEscapedQualifier As String) As String

            Dim sbRet As System.Text.StringBuilder = New Text.StringBuilder()

            For Each itm As String In CSVLines
                If itm Is Nothing Then
                    sbRet = sbRet.Append(String.Format("{2}{0}{2}{1}", "", FieldDelimiter, FieldQualifier))
                Else
                    itm = itm.Replace(FieldQualifier, FieldEscapedQualifier)
                    sbRet = sbRet.Append(String.Format("{2}{0}{2}{1}", itm, FieldDelimiter, FieldQualifier))
                End If
            Next

            Return sbRet.ToString.TrimEnd(FieldDelimiter)

        End Function

#End Region

    End Class

End Namespace

