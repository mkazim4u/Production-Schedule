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
Imports System.Configuration
Imports System.Data
Imports System.Xml
Imports System.Web
Imports System.Collections.Generic
Imports System.IO

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class ProductController


#Region "NB_Store_Products Public Methods"

        Public Sub CopyProductToLanguages(ByVal objInfo As NB_Store_ProductsInfo, Optional ByVal ForceOverwrite As Boolean = True)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim originalLang As String = objInfo.Lang
            For Each L As String In supportedLanguages
                If originalLang <> L Then
                    CopyProductToLanguages(objInfo, L, originalLang, ForceOverwrite)
                End If
            Next
        End Sub

        Public Sub CopyProductToLanguages(ByVal objInfo As NB_Store_ProductsInfo, ByVal Lang As String, ByVal originalLang As String, Optional ByVal ForceOverwrite As Boolean = True)
            Dim blnDoCopy As Boolean = True
            Dim objDummy As NB_Store_ProductsInfo

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = GetProduct(objInfo.ProductID, Lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            If blnDoCopy Then

                'copy products
                objInfo.Lang = Lang
                UpdateObjProduct(objInfo)
            End If

            Dim arylist As ArrayList

            'copy models -----------------------------------
            Dim objModelInfo As NB_Store_ModelInfo
            'get new models
            arylist = GetModelList(objInfo.PortalID, objInfo.ProductID, originalLang, True)
            For Each objModelInfo In arylist
                CopyModelToLanguages(objModelInfo, Lang, ForceOverwrite)
            Next
            '------------------------------

            'copy options -----------------------------------
            Dim objOptionInfo As NB_Store_OptionInfo
            'get new options
            Dim arylist2 As ArrayList
            Dim objOptionVInfo As NB_Store_OptionValueInfo
            arylist = GetOptionList(objInfo.ProductID, originalLang)
            For Each objOptionInfo In arylist
                CopyOptionToLanguages(objOptionInfo, Lang, ForceOverwrite)

                'copy option values 
                arylist2 = GetOptionValueList(objOptionInfo.OptionID, originalLang)
                For Each objOptionVInfo In arylist2
                    CopyOptionValueToLanguages(objOptionVInfo, Lang, ForceOverwrite)
                Next

            Next
            '------------------------------

            'copy images ---------------------
            Dim objPIInfo As NB_Store_ProductImageInfo
            arylist = GetProductImageList(objInfo.ProductID, originalLang)
            For Each objPIInfo In arylist
                CopyProductImageToLanguages(objPIInfo, Lang, ForceOverwrite)
            Next
            '------------------------------

            'copy docs ---------------------
            Dim objDInfo As NB_Store_ProductDocInfo
            arylist = GetProductDocList(objInfo.ProductID, originalLang)
            For Each objDInfo In arylist
                CopyProductDocToLanguages(objDInfo, Lang, ForceOverwrite)
            Next
            '------------------------------

        End Sub

        Public Function CopyProduct(ByVal ProductID As Integer) As Integer
            Dim objPInfo As NB_Store_ProductsInfo = Nothing
            Dim objPInfoNew As NB_Store_ProductsInfo
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim i As Integer
            Dim Lang As String = ""
            Dim LangLoop As String = ""
            Dim NewProductID As Integer = -1
            'TODO: Messy copied code, review to make it cleaner somehow!

            If supportedLanguages.Count > 0 Then
                'get the first language to create a new product as that lang
                For i = 0 To supportedLanguages.Count - 1
                    Lang = CType(supportedLanguages(i).Value, Locale).Code
                    'create a product (need to create one so we can get the NewProductID)
                    objPInfo = GetProduct(ProductID, Lang)
                    If Not objPInfo Is Nothing Then
                        Exit For
                    End If
                Next
                If Not objPInfo Is Nothing Then

                    objPInfo.ProductID = -1
                    objPInfo.ProductName = objPInfo.ProductName & " [Copy]"
                    objPInfoNew = UpdateObjProduct(objPInfo)
                    If Not objPInfoNew Is Nothing Then
                        NewProductID = objPInfoNew.ProductID
                        'create product other langs
                        For i = 0 To supportedLanguages.Count - 1
                            LangLoop = CType(supportedLanguages(i).Value, Locale).Code
                            If LangLoop <> Lang Then
                                objPInfo = GetProduct(ProductID, LangLoop)
                                If Not objPInfo Is Nothing Then
                                    'create new product for language
                                    objPInfo.ProductID = NewProductID
                                    objPInfo.ProductName = objPInfo.ProductName & " [Copy]"
                                    UpdateObjProduct(objPInfo)
                                End If
                            End If
                        Next

                        CopyModels(ProductID, Lang, NewProductID, supportedLanguages)

                        CopyOptions(ProductID, Lang, NewProductID, supportedLanguages)

                        CopyProductImages(ProductID, Lang, NewProductID, supportedLanguages)

                        CopyProductDocs(ProductID, Lang, NewProductID, supportedLanguages)

                        CopyProductCategories(ProductID, NewProductID)

                    End If

                End If

            End If
            Return NewProductID
        End Function

        Private Sub CopyProductDocs(ByVal ProductID As Integer, ByVal Lang As String, ByVal NewProductID As Integer, ByVal supportedLanguages As LocaleCollection)
            'create Products
            Dim objProductInfo As NB_Store_ProductDocInfo
            Dim objNewProductInfo As NB_Store_ProductDocInfo
            Dim aryProductList As ArrayList
            Dim aryNewProductList As ArrayList
            Dim LangLoop As String = ""

            aryProductList = GetProductDocList(ProductID, Lang)
            For Each objProductInfo In aryProductList
                objProductInfo.DocID = -1
                objProductInfo.ProductID = NewProductID
                UpdateObjProductDoc(objProductInfo)
            Next

            'create Products  other langs
            For i = 0 To supportedLanguages.Count - 1
                LangLoop = CType(supportedLanguages(i).Value, Locale).Code
                aryNewProductList = GetProductDocList(NewProductID, LangLoop)
                aryProductList = GetProductDocList(ProductID, LangLoop)

                For lp = 0 To (aryProductList.Count - 1)
                    objProductInfo = CType(aryProductList(lp), NB_Store_ProductDocInfo)
                    objNewProductInfo = CType(aryNewProductList(lp), NB_Store_ProductDocInfo)
                    If objProductInfo.DocDesc <> "" Then
                        objNewProductInfo.DocDesc = objProductInfo.DocDesc
                        objNewProductInfo.Lang = objProductInfo.Lang
                        UpdateObjProductDoc(objNewProductInfo)
                    End If
                Next
            Next
        End Sub

        Private Sub CopyProductImages(ByVal ProductID As Integer, ByVal Lang As String, ByVal NewProductID As Integer, ByVal supportedLanguages As LocaleCollection)
            'create Products
            Dim objProductInfo As NB_Store_ProductImageInfo
            Dim objNewProductInfo As NB_Store_ProductImageInfo
            Dim aryProductList As ArrayList
            Dim aryNewProductList As ArrayList
            Dim LangLoop As String = ""

            aryProductList = GetProductImageList(ProductID, Lang)
            For Each objProductInfo In aryProductList
                objProductInfo.ImageID = -1
                objProductInfo.ProductID = NewProductID
                UpdateObjProductImage(objProductInfo)
            Next

            'create Products  other langs
            For i = 0 To supportedLanguages.Count - 1
                LangLoop = CType(supportedLanguages(i).Value, Locale).Code
                aryNewProductList = GetProductImageList(NewProductID, LangLoop)
                aryProductList = GetProductImageList(ProductID, LangLoop)

                For lp = 0 To (aryProductList.Count - 1)
                    objProductInfo = CType(aryProductList(lp), NB_Store_ProductImageInfo)
                    objNewProductInfo = CType(aryNewProductList(lp), NB_Store_ProductImageInfo)
                    If objProductInfo.ImageDesc <> "" Then
                        objNewProductInfo.ImageDesc = objProductInfo.ImageDesc
                        objNewProductInfo.Lang = objProductInfo.Lang
                        UpdateObjProductImage(objNewProductInfo)
                    End If
                Next
            Next
        End Sub

        Private Sub CopyModels(ByVal ProductID As Integer, ByVal Lang As String, ByVal NewProductID As Integer, ByVal supportedLanguages As LocaleCollection)
            'create models
            Dim objModelInfo As NB_Store_ModelInfo
            Dim objNewModelInfo As NB_Store_ModelInfo
            Dim aryModelList As ArrayList
            Dim aryNewModelList As ArrayList
            Dim LangLoop As String = ""
            Dim objSRInfo As NB_Store_ShippingRatesInfo
            Dim objSRCtrl As New ShipController
            Dim ModelID As Integer

            aryModelList = GetModelList(-1, ProductID, Lang, True)
            For Each objModelInfo In aryModelList
                'get model shipping info
                objSRInfo = objSRCtrl.GetShippingRateByObjID(objModelInfo.PortalID, objModelInfo.ModelID, "PRD", -1)

                objModelInfo.ModelID = -1
                objModelInfo.ProductID = NewProductID
                ModelID = UpdateObjModel(objModelInfo)

                If Not objSRInfo Is Nothing Then
                    'copy model shipping info
                    objSRInfo.ObjectId = ModelID
                    objSRInfo.ItemId = -1
                    objSRCtrl.UpdateObjShippingRate(objSRInfo)
                End If

            Next



            'create models  other langs
            For i = 0 To supportedLanguages.Count - 1
                LangLoop = CType(supportedLanguages(i).Value, Locale).Code
                aryNewModelList = GetModelList(-1, NewProductID, LangLoop, True)
                aryModelList = GetModelList(-1, ProductID, LangLoop, True)

                For lp = 0 To (aryModelList.Count - 1)
                    objModelInfo = CType(aryModelList(lp), NB_Store_ModelInfo)
                    objNewModelInfo = CType(aryNewModelList(lp), NB_Store_ModelInfo)
                    If objModelInfo.ModelName <> "" Then
                        objNewModelInfo.ModelName = objModelInfo.ModelName
                        objNewModelInfo.Lang = objModelInfo.Lang
                        UpdateObjModel(objNewModelInfo)
                    End If
                Next
            Next
        End Sub

        Private Sub CopyOptions(ByVal ProductID As Integer, ByVal Lang As String, ByVal NewProductID As Integer, ByVal supportedLanguages As LocaleCollection)
            'create options
            Dim objOptionInfo As NB_Store_OptionInfo
            Dim objNewOptionInfo As NB_Store_OptionInfo
            Dim aryOptionList As ArrayList
            Dim aryNewOptionList As ArrayList
            Dim LangLoop As String = ""

            aryOptionList = GetOptionList(ProductID, Lang)
            For Each objOptionInfo In aryOptionList
                objOptionInfo.OptionID = -1
                objOptionInfo.ProductID = NewProductID
                UpdateObjOption(objOptionInfo)
            Next


            'create options  other langs
            For i = 0 To supportedLanguages.Count - 1
                LangLoop = CType(supportedLanguages(i).Value, Locale).Code
                aryNewOptionList = GetOptionList(NewProductID, LangLoop)
                aryOptionList = GetOptionList(ProductID, LangLoop)

                For lp = 0 To (aryOptionList.Count - 1)
                    objOptionInfo = CType(aryOptionList(lp), NB_Store_OptionInfo)
                    objNewOptionInfo = CType(aryNewOptionList(lp), NB_Store_OptionInfo)

                    If objOptionInfo.OptionDesc <> "" Then
                        objNewOptionInfo.OptionDesc = objOptionInfo.OptionDesc
                        objNewOptionInfo.Lang = objOptionInfo.Lang
                        UpdateObjOption(objNewOptionInfo)
                    End If
                Next
            Next

            'create option values 
            aryNewOptionList = GetOptionList(NewProductID, Lang)
            aryOptionList = GetOptionList(ProductID, Lang)
            For lp = 0 To (aryOptionList.Count - 1)
                objOptionInfo = CType(aryOptionList(lp), NB_Store_OptionInfo)
                objNewOptionInfo = CType(aryNewOptionList(lp), NB_Store_OptionInfo)
                'copy option values
                CopyOptionValues(objOptionInfo.OptionID, Lang, objNewOptionInfo.OptionID, supportedLanguages)
            Next

        End Sub

        Private Sub CopyOptionValues(ByVal OptionID As Integer, ByVal Lang As String, ByVal NewOptionID As Integer, ByVal supportedLanguages As LocaleCollection)
            'create options
            Dim objOptionInfo As NB_Store_OptionValueInfo
            Dim objNewOptionInfo As NB_Store_OptionValueInfo
            Dim aryOptionList As ArrayList
            Dim aryNewOptionList As ArrayList
            Dim LangLoop As String = ""

            aryOptionList = GetOptionValueList(OptionID, Lang)
            For Each objOptionInfo In aryOptionList
                objOptionInfo.OptionValueID = -1
                objOptionInfo.OptionID = NewOptionID
                UpdateObjOptionValue(objOptionInfo)
            Next

            'create options  other langs
            For i = 0 To supportedLanguages.Count - 1
                LangLoop = CType(supportedLanguages(i).Value, Locale).Code
                aryNewOptionList = GetOptionValueList(NewOptionID, LangLoop)
                aryOptionList = GetOptionValueList(OptionID, LangLoop)

                If aryNewOptionList.Count <> aryOptionList.Count Then
                    'mismatch on lanague otions, so use the original
                    aryNewOptionList = aryOptionList.Clone
                End If

                For lp = 0 To (aryOptionList.Count - 1)
                    objOptionInfo = CType(aryOptionList(lp), NB_Store_OptionValueInfo)
                    objNewOptionInfo = CType(aryNewOptionList(lp), NB_Store_OptionValueInfo)
                    If objOptionInfo.OptionValueDesc <> "" Then
                        objNewOptionInfo.OptionValueDesc = objOptionInfo.OptionValueDesc
                        objNewOptionInfo.Lang = objOptionInfo.Lang
                        UpdateObjOptionValue(objNewOptionInfo)
                    End If
                Next
            Next

        End Sub

        Private Sub CopyProductCategories(ByVal ProductID As Integer, ByVal NewProductID As Integer)
            'create options
            Dim objPCInfo As NB_Store_ProductCategoryInfo
            Dim aryPCList As ArrayList

            aryPCList = GetCategoriesAssigned(ProductID)
            For Each objPCInfo In aryPCList
                UpdateProductCategory(NewProductID, objPCInfo.CategoryID)
            Next

        End Sub

        Public Sub DeleteProduct(ByVal ProductID As Integer)
            Dim aryList As ArrayList

            'Remove all entry images (no cascade delete on images)
            aryList = GetProductImageList(ProductID, GetCurrentCulture)
            For Each objInfo As NB_Store_ProductImageInfo In aryList
                DeleteProductImage(objInfo.ImageID)
            Next

            'Remove all entry docs (no cascade delete on docs)
            aryList = GetProductDocList(ProductID, GetCurrentCulture)
            For Each objInfo As NB_Store_ProductDocInfo In aryList
                DeleteProductDoc(objInfo.DocID)
            Next

            'Remove all Related Products
            DeleteProductRelatedByProduct(ProductID)

            DataProvider.Instance().DeleteNB_Store_Products(ProductID)
        End Sub

        Public Sub PurgeProducts(ByVal PortalID As Integer)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each Lang As String In supportedLanguages
                PurgeProducts(PortalID, Lang)
            Next
        End Sub

        Public Sub PurgeProducts(ByVal PortalID As Integer, ByVal Lang As String)
            Dim aryList As ArrayList
            Dim objPInfo As NB_Store_ProductsInfo
            aryList = GetProductExportList(PortalID, Lang, True)
            For Each objPInfo In aryList
                If GetProductInOrders(objPInfo.ProductID) = 0 Then
                    DeleteProduct(objPInfo.ProductID)
                End If
            Next
        End Sub

        Public Sub PurgeModels(ByVal PortalID As Integer)
            Dim aryList As ArrayList
            Dim objMInfo As NB_Store_ModelInfo
            aryList = GetModelDeletedList(PortalID)
            For Each objMInfo In aryList
                If GetModelInOrders(objMInfo.ModelID) = 0 Then
                    DeleteModel(PortalID, objMInfo.ModelID)
                End If
            Next
        End Sub

        Public Sub PurgeImages(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal ImageDirectory As String)
            Dim aryList As ArrayList
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim DBFileList As New Hashtable
            Dim objCCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo

            aryList = GetProductImageExportList(PortalSettings.PortalId)
            For Each objPIInfo In aryList
                If Not DBFileList.ContainsValue(objPIInfo.ImagePath.ToLower) Then
                    DBFileList.Add(objPIInfo.ImageID, objPIInfo.ImagePath.ToLower)
                End If
            Next

            aryList = objCCtrl.GetCategories(PortalSettings.PortalId, GetCurrentCulture, -1, True, True)
            For Each objCInfo In aryList
                If objCInfo.ImageURL <> "" Then
                    If Not DBFileList.ContainsValue(System.Web.Hosting.HostingEnvironment.MapPath(objCInfo.ImageURL).ToLower) Then
                        If objCInfo.ImageURL <> "" Then
                            DBFileList.Add("CAT" & objCInfo.CategoryID.ToString, System.Web.Hosting.HostingEnvironment.MapPath(objCInfo.ImageURL).ToLower)
                        End If
                    End If
                End If
            Next

            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo

            folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, ImageDirectory)
            If Not folderInfo Is Nothing Then
                aryList = FileSystemUtils.GetFilesByFolder(PortalSettings.PortalId, folderInfo.FolderID)
                For Each fileInfo In aryList
                    If Not DBFileList.ContainsValue((PortalSettings.HomeDirectoryMapPath & ImageDirectory & "\" & fileInfo.FileName).ToLower) Then
                        FileSystemUtils.DeleteFile(PortalSettings.HomeDirectoryMapPath & ImageDirectory & "\" & fileInfo.FileName, PortalSettings, True)
                    End If
                Next
            End If

        End Sub

        Public Sub PurgeDocs(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal DocDirectory As String)
            Dim aryList As ArrayList
            Dim objDInfo As NB_Store_ProductDocInfo
            Dim DBFileList As New Hashtable

            aryList = GetProductDocExportList(PortalSettings.PortalId)
            For Each objDInfo In aryList
                If Not DBFileList.ContainsValue(objDInfo.DocPath.ToLower) Then
                    DBFileList.Add(objDInfo.DocID, objDInfo.DocPath.ToLower)
                End If
            Next

            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo

            folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, DocDirectory)
            If Not folderInfo Is Nothing Then
                aryList = FileSystemUtils.GetFilesByFolder(PortalSettings.PortalId, folderInfo.FolderID)
                For Each fileInfo In aryList
                    If Not DBFileList.ContainsValue((PortalSettings.HomeDirectoryMapPath & DocDirectory & "\" & fileInfo.FileName).ToLower) Then
                        FileSystemUtils.DeleteFile(PortalSettings.HomeDirectoryMapPath & DocDirectory & "\" & fileInfo.FileName, PortalSettings, True)
                    End If
                Next
            End If

        End Sub

        Public Function ImageValidation(ByVal PortalID As Integer, ByVal WebMapPath As String, ByVal DoFix As Boolean) As String
            Dim aryList As ArrayList
            Dim objPIInfo As NB_Store_ProductImageInfo
            Dim objPCtrl As New ProductController
            Dim errCount As Integer = 0
            Dim fixCount As Integer = 0

            aryList = GetProductImageExportList(PortalID)
            For Each objPIInfo In aryList
                If WebMapPath.TrimEnd("\"c) & Replace(objPIInfo.ImageURL, "/", "\") <> objPIInfo.ImagePath Then
                    errCount = errCount + 1
                    If DoFix Then
                        objPIInfo.ImagePath = WebMapPath.TrimEnd("\"c) & Replace(objPIInfo.ImageURL, "/", "\")
                        If File.Exists(objPIInfo.ImagePath) Then
                            objPCtrl.UpdateObjProductImageOnly(objPIInfo)
                            If Not objPIInfo Is Nothing Then
                                UpdateLog("Update Image - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objPIInfo))
                            End If
                        Else
                            If Not objPIInfo Is Nothing Then
                                UpdateLog("Delete Image - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objPIInfo))
                            End If
                            objPCtrl.DeleteProductImage(objPIInfo.ImageID)
                        End If
                        fixCount = fixCount + 1
                    End If
                Else
                    If Not File.Exists(objPIInfo.ImagePath) Then
                        errCount = errCount + 1
                        If DoFix Then
                            UpdateLog("Delete Image - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objPIInfo))
                            objPCtrl.DeleteProductImage(objPIInfo.ImageID)
                            fixCount = fixCount + 1
                        End If
                    End If
                End If
            Next
            Return "ERRORS: " & errCount & "  FIX: " & fixCount
        End Function

        Public Function DocValidation(ByVal PortalID As Integer, ByVal DocMapPath As String, ByVal DoFix As Boolean) As String
            Dim aryList As ArrayList
            Dim objDInfo As NB_Store_ProductDocInfo
            Dim objPCtrl As New ProductController
            Dim errCount As Integer = 0
            Dim fixCount As Integer = 0

            aryList = GetProductDocExportList(PortalID)
            For Each objDInfo In aryList
                If Not objDInfo.DocPath.ToLower.StartsWith(DocMapPath.ToLower) Then
                    errCount = errCount + 1
                    If DoFix Then
                        objDInfo.DocPath = DocMapPath & "\" & System.IO.Path.GetFileName(objDInfo.DocPath)
                        If File.Exists(objDInfo.DocPath) Then
                            objDInfo.Lang = ""
                            objPCtrl.UpdateObjProductDoc(objDInfo)
                            If Not objDInfo Is Nothing Then
                                UpdateLog("Update Doc - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objDInfo))
                            End If
                        Else
                            If Not objDInfo Is Nothing Then
                                UpdateLog("Delete Doc - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objDInfo))
                                objPCtrl.DeleteProductDoc(objDInfo.DocID)
                            End If
                        End If
                        fixCount = fixCount + 1
                    End If
                Else
                    If Not File.Exists(objDInfo.DocPath) Then
                        errCount = errCount + 1
                        If DoFix Then
                            UpdateLog("Delete Doc - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objDInfo))
                            objPCtrl.DeleteProductDoc(objDInfo.DocID)
                            fixCount = fixCount + 1
                        End If
                    End If
                End If
            Next
            Return "ERRORS: " & errCount & "  FIX: " & fixCount
        End Function

        Public Function ProductLangValidation(ByVal PortalID As Integer, ByVal Lang As String) As String
            Dim aryList As ArrayList
            Dim objPInfo As NB_Store_ProductsInfo
            Dim fixCount As Integer = 0

            aryList = GetProductExportList(PortalID, Lang, False)
            For Each objPInfo In aryList
                fixCount = fixCount + 1
                CopyProductToLanguages(objPInfo, False)
            Next
            Return "Products Checked based on Merchant Culture of " & Lang & " : " & fixCount
        End Function


        Public Function ProductLangValidation(ByVal PortalID As Integer) As String
            Return ProductLangValidation(PortalID, GetMerchantCulture(PortalID))
        End Function

        Public Function CategoryLangValidation(ByVal PortalID As Integer) As String
            Return CategoryLangValidation(PortalID, GetMerchantCulture(PortalID))
        End Function


        Public Function CategoryLangValidation(ByVal PortalID As Integer, ByVal Lang As String) As String
            Dim aryList As ArrayList
            Dim objCtrl As New CategoryController
            Dim objCInfo As NB_Store_CategoriesInfo
            Dim fixCount As Integer = 0

            aryList = objCtrl.GetCategories(PortalID, Lang)
            For Each objCInfo In aryList
                fixCount = fixCount + 1
                objCtrl.CopyToLanguages(objCInfo, False)
            Next
            Return "Categories Checked based on Merchant Culture of " & Lang & " : " & fixCount
        End Function


        Public Function GetProductByRef(ByVal PortalID As Integer, ByVal ProductRef As String, ByVal Lang As String) As NB_Store_ProductsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetProductByRef(PortalID, ProductRef, Lang), GetType(NB_Store_ProductsInfo)), NB_Store_ProductsInfo)
        End Function

        Public Function GetProductListInfo(ByVal ProductID As Integer, ByVal Lang As String) As ProductListInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Products(ProductID, Lang), GetType(ProductListInfo)), ProductListInfo)
        End Function

        Public Function GetProduct(ByVal ProductID As Integer, ByVal Lang As String) As NB_Store_ProductsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Products(ProductID, Lang), GetType(NB_Store_ProductsInfo)), NB_Store_ProductsInfo)
        End Function

        Public Function GetProductInOrders(ByVal ProductID As Integer) As Integer
            Return CType(DataProvider.Instance().GetProductInOrders(ProductID), Integer)
        End Function

        Public Function CheckIfProductPurchased(ByVal ProductID As Integer, ByVal UserID As Integer) As Integer
            Return CType(DataProvider.Instance().CheckIfProductPurchased(ProductID, UserID), Integer)
        End Function

        Public Function GetProductCount(ByVal PortalID As Integer) As Integer
            Return CType(DataProvider.Instance().GetProductCount(PortalID), Integer)
        End Function

        Public Function GetProductInArray(ByVal ProductID As Integer, ByVal Lang As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Products(ProductID, Lang), GetType(ProductListInfo))
        End Function

        Public Function GetProductExportList(ByVal PortalID As Integer, ByVal Lang As String, ByVal DeletedOnly As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetProductExportList(PortalID, Lang, DeletedOnly), GetType(NB_Store_ProductsInfo))
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, "", False, False, "", False, 0, 1, 99999, False, isDealer)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, SearchText, False, False, "", False, 0, 1, 99999, SearchDescription, isDealer)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, SearchText, GetArchived, False, "", False, 0, 1, 99999, SearchDescription, isDealer)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, "", False, False, "", False, 0, PageIndex, Pagesize, False, isDealer)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, SearchText, False, False, "", False, 0, PageIndex, Pagesize, SearchDescription, isDealer)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, SearchText, GetArchived, False, "", False, 0, PageIndex, Pagesize, SearchDescription, isDealer)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal OrderBY As String, ByVal OrderDESC As Boolean, ByVal ReturnLimit As Integer, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean) As ArrayList
            Return GetProductList(PortalID, CategoryID, Lang, SearchText, GetArchived, FeaturedOnly, OrderBY, OrderDESC, ReturnLimit, PageIndex, Pagesize, SearchDescription, isDealer, "", False)
        End Function

        Public Function GetProductList(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal OrderBY As String, ByVal OrderDESC As Boolean, ByVal ReturnLimit As Integer, ByVal PageIndex As Integer, ByVal Pagesize As Integer, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean, ByVal CategoryList As String, ByVal ExcludeFeatured As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Productss(PortalID, CategoryID, Lang, SearchText, GetArchived, FeaturedOnly, OrderBY, OrderDESC, ReturnLimit, PageIndex, Pagesize, SearchDescription, isDealer, CategoryList, ExcludeFeatured), GetType(ProductListInfo))
        End Function

        Public Function UpdateObjProduct(ByVal objInfo As NB_Store_ProductsInfo) As NB_Store_ProductsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().UpdateNB_Store_Products(objInfo.ProductID, objInfo.PortalID, objInfo.TaxCategoryID, objInfo.Featured, objInfo.Archived, objInfo.CreatedByUser, objInfo.CreatedDate, objInfo.IsDeleted, objInfo.ProductRef, objInfo.Lang, objInfo.Summary, objInfo.Description, objInfo.Manufacturer, objInfo.ProductName, objInfo.XMLData, objInfo.SEOName, objInfo.TagWords, objInfo.IsHidden), GetType(NB_Store_ProductsInfo)), NB_Store_ProductsInfo)
        End Function

        Public Function GetProductListSize(ByVal PortalID As Integer, ByVal CategoryID As Integer, ByVal Lang As String, ByVal SearchText As String, ByVal GetArchived As Boolean, ByVal FeaturedOnly As Boolean, ByVal SearchDescription As Boolean, ByVal isDealer As Boolean, ByVal CategoryList As String, ByVal ExcludeFeatured As Boolean) As Integer
            Return CType(DataProvider.Instance().GetProductListSize(PortalID, CategoryID, Lang, SearchText, GetArchived, FeaturedOnly, SearchDescription, isDealer, CategoryList, ExcludeFeatured), Integer)
        End Function


#End Region

#Region "NB_Store_Model Public Methods"

        Public Sub CopyModelToLanguages(ByVal objInfo As NB_Store_ModelInfo, ByVal ForceOverwrite As Boolean)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopyModelToLanguages(objInfo, L, ForceOverwrite)
            Next
        End Sub

        Public Sub CopyModelToLanguages(ByVal objInfo As NB_Store_ModelInfo, ByVal Lang As String, ByVal ForceOverwrite As Boolean)
            Dim objDummy As NB_Store_ModelInfo
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = GetModel(objInfo.ModelID, Lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            If blnDoCopy Then
                objInfo.Lang = Lang
                UpdateObjModel(objInfo)
            End If

        End Sub

        Public Sub DeleteModel(ByVal PortalID As Integer, ByVal ModelID As Integer)
            DataProvider.Instance().DeleteNB_Store_Model(ModelID)
            'remove shipping rates record for model
            Dim objSRCtrl As New ShipController
            Dim objSRInfo As NB_Store_ShippingRatesInfo
            Dim aryList As ArrayList

            aryList = objSRCtrl.GetShippingMethodList(PortalID)
            For Each objSMInfo As NB_Store_ShippingMethodInfo In aryList
                objSRInfo = objSRCtrl.GetShippingRateByObjID(PortalID, ModelID, "PRD", objSMInfo.ShipMethodID)
                If Not objSRInfo Is Nothing Then
                    DataProvider.Instance().DeleteNB_Store_ShippingRates(objSRInfo.ItemId)
                End If
            Next

        End Sub

        Public Function GetModelByRef(ByVal ModelRef As String, ByVal Lang As String) As NB_Store_ModelInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetModelByRef(-1, ModelRef, Lang), GetType(NB_Store_ModelInfo)), NB_Store_ModelInfo)
        End Function

        Public Function GetModelByRef(ByVal ProductID As Integer, ByVal ModelRef As String, ByVal Lang As String) As NB_Store_ModelInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetModelByRef(ProductID, ModelRef, Lang), GetType(NB_Store_ModelInfo)), NB_Store_ModelInfo)
        End Function

        Public Function GetModel(ByVal ModelID As Integer, ByVal Lang As String) As NB_Store_ModelInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Model(ModelID, Lang), GetType(NB_Store_ModelInfo)), NB_Store_ModelInfo)
        End Function

        Public Function GetModelList(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Models(PortalID, ProductID, Lang, IsDealer), GetType(NB_Store_ModelInfo))
        End Function

        Public Function GetModelInStockList(ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ModelsInStock(ProductID, Lang, IsDealer), GetType(NB_Store_ModelInfo))
        End Function

        Public Function GetModelList(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Models(PortalID, -1, Lang, IsDealer), GetType(NB_Store_ModelInfo))
        End Function

        Public Function GetModelStockList(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal PageIndex As Integer, ByVal PageSize As Integer, ByVal IsDealer As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ModelStockList(PortalID, Filter, Lang, CategoryID, PageIndex, PageSize, IsDealer), GetType(NB_Store_ModelInfo))
        End Function

        Public Function GetModelStockList(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal IsDealer As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ModelStockList(PortalID, Filter, Lang, CategoryID, 1, 0, IsDealer), GetType(NB_Store_ModelInfo))
        End Function

        Public Function GetModelStockListSize(ByVal PortalID As Integer, ByVal Filter As String, ByVal Lang As String, ByVal CategoryID As Integer, ByVal IsDealer As Boolean) As Integer
            Return CType(DataProvider.Instance().GetNB_Store_ModelStockListSize(PortalID, Filter, Lang, CategoryID, IsDealer), Integer)
        End Function

        Public Function GetModelDeletedList(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_DeletedModels(PortalID), GetType(NB_Store_ModelInfo))
        End Function

        Public Function UpdateObjModel(ByVal objInfo As NB_Store_ModelInfo) As Integer
            Return DataProvider.Instance().UpdateNB_Store_Model(objInfo.ModelID, objInfo.ProductID, objInfo.ListOrder, objInfo.UnitCost, objInfo.Barcode, objInfo.ModelRef, objInfo.Lang, objInfo.ModelName, objInfo.QtyRemaining, objInfo.QtyTrans, objInfo.QtyTransDate, objInfo.Deleted, objInfo.QtyStockSet, objInfo.DealerCost, objInfo.PurchaseCost, objInfo.XMLData, objInfo.Extra, objInfo.DealerOnly, objInfo.Allow)
        End Function

        Public Sub UpdateStockLevel(ByVal OrderID As Integer)
            Dim objOCtrl As New OrderController
            Dim aryList As ArrayList
            Dim objInfo As NB_Store_OrderDetailsInfo

            aryList = objOCtrl.GetOrderDetailList(OrderID)
            For Each objInfo In aryList
                UpdateStockLevel(objInfo.ModelID, objInfo.Quantity)
            Next
        End Sub

        Public Sub UpdateStockLevel(ByVal ModelID As Integer, ByVal Qty As Integer)
            Dim objInfo As NB_Store_ModelInfo

            objInfo = GetModel(ModelID, GetCurrentCulture)
            If Not objInfo Is Nothing Then
                'check if stock is turned off (-1)
                If objInfo.QtyRemaining > 0 Then
                    If objInfo.QtyRemaining > 0 Then
                        objInfo.QtyRemaining = objInfo.QtyRemaining - Qty
                        objInfo.QtyTrans = objInfo.QtyTrans - Qty
                    End If
                    If objInfo.QtyRemaining < 0 Then
                        objInfo.QtyRemaining = 0
                        'TODO: Send email of stock level eror
                    End If
                    If objInfo.QtyTrans < 0 Then
                        objInfo.QtyTrans = 0
                    End If

                    UpdateObjModel(objInfo)
                End If
            End If
        End Sub

        Public Sub RemoveModelQtyTrans(ByVal OrderID As Integer)
            Dim objOCtrl As New OrderController
            Dim aryList As ArrayList
            Dim objInfo As NB_Store_OrderDetailsInfo

            aryList = objOCtrl.GetOrderDetailList(OrderID)
            For Each objInfo In aryList
                UpdateModelQtyTrans(objInfo.ModelID, objInfo.Quantity * -1)
            Next

        End Sub

        Public Sub UpdateModelQtyTrans(ByVal OrderID As Integer)
            Dim objOCtrl As New OrderController
            Dim aryList As ArrayList
            Dim objInfo As NB_Store_OrderDetailsInfo

            aryList = objOCtrl.GetOrderDetailList(OrderID)
            For Each objInfo In aryList
                UpdateModelQtyTrans(objInfo.ModelID, objInfo.Quantity)
            Next

        End Sub

        Public Sub UpdateModelQtyTrans(ByVal ModelID As Integer, ByVal QtyTrans As Integer)
            Dim objInfo As NB_Store_ModelInfo

            objInfo = GetModel(ModelID, GetCurrentCulture)
            If Not objInfo Is Nothing Then
                If Now < DateAdd(DateInterval.Minute, 10, objInfo.QtyTransDate) Then
                    objInfo.QtyTrans = QtyTrans + objInfo.QtyTrans
                Else
                    objInfo.QtyTrans = QtyTrans
                End If
                objInfo.QtyTransDate = Now

                If objInfo.QtyTrans < 0 Then
                    objInfo.QtyTrans = 0
                End If

                UpdateObjModel(objInfo)
            End If
        End Sub

        Public Sub DeleteProductModels(ByVal objProductInfo As NB_Store_ProductsInfo)
            Dim aryList As ArrayList
            aryList = GetModelList(objProductInfo.PortalID, objProductInfo.ProductID, objProductInfo.Lang, True)
            Dim objModelInfo As NB_Store_ModelInfo
            For Each objModelInfo In aryList
                DeleteModel(objProductInfo.PortalID, objModelInfo.ModelID)
            Next
        End Sub

        Public Sub AddNewModel(ByVal objProductInfo As NB_Store_ProductsInfo)

            Dim objInfo As New NB_Store_ModelInfo
            objInfo.ModelID = -1
            objInfo.Barcode = ""
            objInfo.Lang = objProductInfo.Lang
            objInfo.ListOrder = 1
            objInfo.ModelName = objProductInfo.ProductName
            objInfo.ModelRef = objProductInfo.ProductRef
            objInfo.ProductID = objProductInfo.ProductID
            objInfo.QtyRemaining = -1
            objInfo.QtyTrans = 0
            objInfo.QtyTransDate = Now
            objInfo.UnitCost = 0
            UpdateObjModel(objInfo)
        End Sub

        Public Function GetModelInOrders(ByVal ModelID As Integer) As Integer
            Return CType(DataProvider.Instance().GetModelInOrders(ModelID), Integer)
        End Function

#End Region

#Region "NB_Store_ProductImage Public Methods"


        Public Sub CopyProductImageToLanguages(ByVal objInfo As NB_Store_ProductImageInfo, ByVal ForceOverwrite As Boolean)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopyProductImageToLanguages(objInfo, L, ForceOverwrite)
            Next
        End Sub

        Public Sub CopyProductImageToLanguages(ByVal objInfo As NB_Store_ProductImageInfo, ByVal Lang As String, ByVal ForceOverwrite As Boolean)
            Dim objDummy As NB_Store_ProductImageInfo
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = GetProductImage(objInfo.ImageID, Lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            If blnDoCopy Then
                objInfo.Lang = Lang
                UpdateObjProductImage(objInfo)
            End If
        End Sub

        Public Sub DeleteProductImage(ByVal ImageID As Integer)
            DataProvider.Instance().DeleteNB_Store_ProductImage(ImageID)
        End Sub

        Public Function GetProductImage(ByVal ImageID As Integer, ByVal Lang As String) As NB_Store_ProductImageInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_ProductImage(ImageID, Lang), GetType(NB_Store_ProductImageInfo)), NB_Store_ProductImageInfo)
        End Function

        Public Function GetProductImageList(ByVal ProductID As Integer, ByVal Lang As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductImages(ProductID, Lang), GetType(NB_Store_ProductImageInfo))
        End Function

        Public Function GetProductImageExportList(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetProductExportImages(PortalID), GetType(NB_Store_ProductImageInfo))
        End Function

        Public Function UpdateObjProductImage(ByVal objInfo As NB_Store_ProductImageInfo) As Integer
            Return DataProvider.Instance().UpdateNB_Store_ProductImage(objInfo.ImageID, objInfo.ProductID, objInfo.ImagePath, objInfo.ListOrder, objInfo.Hidden, objInfo.Lang, objInfo.ImageDesc, objInfo.ImageURL)
        End Function

        Public Sub UpdateObjProductImageOnly(ByVal objInfo As NB_Store_ProductImageInfo)
            DataProvider.Instance().UpdateNB_Store_ProductImageOnly(objInfo.ImageID, objInfo.ProductID, objInfo.ImagePath, objInfo.ListOrder, objInfo.Hidden, objInfo.ImageURL)
        End Sub

        Public Sub AddNewImage(ByVal ProductID As Integer, ByVal Lang As String, ByVal ImagePathFile As String, ByVal ImageURL As String, ByVal Hidden As Boolean)
            Dim objInfo As New NB_Store_ProductImageInfo
            objInfo.Hidden = Hidden
            objInfo.ImageDesc = ""
            objInfo.ImageID = -1
            objInfo.ImagePath = ImagePathFile
            objInfo.Lang = Lang
            objInfo.ListOrder = 1
            objInfo.ProductID = ProductID
            objInfo.ImageURL = ImageURL
            UpdateObjProductImage(objInfo)
        End Sub


#End Region

#Region "NB_Store_Option Public Methods"

        Public Sub CopyOptionToLanguages(ByVal objInfo As NB_Store_OptionInfo, ByVal ForceOverwrite As Boolean)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopyOptionToLanguages(objInfo, L, ForceOverwrite)
            Next
        End Sub

        Public Sub CopyOptionToLanguages(ByVal objInfo As NB_Store_OptionInfo, ByVal Lang As String, ByVal ForceOverwrite As Boolean)
            Dim objDummy As NB_Store_OptionInfo
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = GetOption(objInfo.OptionID, Lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            If blnDoCopy Then
                objInfo.Lang = Lang
                UpdateObjOption(objInfo)
            End If

        End Sub

        Public Sub DeleteOption(ByVal OptionID As Integer)
            DataProvider.Instance().DeleteNB_Store_Option(OptionID)
        End Sub

        Public Function GetOption(ByVal OptionID As Integer, ByVal Lang As String) As NB_Store_OptionInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Option(OptionID, Lang), GetType(NB_Store_OptionInfo)), NB_Store_OptionInfo)
        End Function

        Public Function GetOptionList(ByVal ProductID As Integer, ByVal Lang As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Options(ProductID, Lang), GetType(NB_Store_OptionInfo))
        End Function

        Public Function UpdateObjOption(ByVal objInfo As NB_Store_OptionInfo) As NB_Store_OptionInfo
            Return CType(CBO.FillObject(DataProvider.Instance().UpdateNB_Store_Option(objInfo.OptionID, objInfo.ProductID, objInfo.ListOrder, objInfo.Lang, objInfo.OptionDesc, objInfo.Attributes), GetType(NB_Store_OptionInfo)), NB_Store_OptionInfo)
        End Function

        Public Sub DeleteProductOptions(ByVal objProductInfo As NB_Store_ProductsInfo)
            Dim aryList As ArrayList
            aryList = GetOptionList(objProductInfo.ProductID, objProductInfo.Lang)
            Dim objOptionInfo As NB_Store_OptionInfo
            For Each objOptionInfo In aryList
                DeleteOption(objOptionInfo.OptionID)
            Next
        End Sub

        Public Sub AddNewOption(ByVal objProductInfo As NB_Store_ProductsInfo)
            Dim objInfo As New NB_Store_OptionInfo
            objInfo.OptionID = -1
            objInfo.Lang = objProductInfo.Lang
            objInfo.ListOrder = 1
            objInfo.ProductID = objProductInfo.ProductID
            objInfo.OptionDesc = ""
            UpdateObjOption(objInfo)
        End Sub


#End Region

#Region "NB_Store_OptionValue Public Methods"

        Public Sub CopyOptionValueToLanguages(ByVal objInfo As NB_Store_OptionValueInfo, ByVal ForceOverwrite As Boolean)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopyOptionValueToLanguages(objInfo, L, ForceOverwrite)
            Next
        End Sub

        Public Sub CopyOptionValueToLanguages(ByVal objInfo As NB_Store_OptionValueInfo, ByVal Lang As String, ByVal ForceOverwrite As Boolean)
            Dim objDummy As NB_Store_OptionValueInfo
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = GetOptionValue(objInfo.OptionValueID, Lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If

            If blnDoCopy Then
                objInfo.Lang = Lang
                UpdateObjOptionValue(objInfo)
            End If
        End Sub

        Public Sub DeleteOptionValue(ByVal OptionValueID As Integer)
            DataProvider.Instance().DeleteNB_Store_OptionValue(OptionValueID)
        End Sub

        Public Function GetOptionValue(ByVal OptionValueID As Integer, ByVal Lang As String) As NB_Store_OptionValueInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_OptionValue(OptionValueID, Lang), GetType(NB_Store_OptionValueInfo)), NB_Store_OptionValueInfo)
        End Function

        Public Function GetOptionValueList(ByVal OptionID As Integer, ByVal Lang As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_OptionValues(OptionID, Lang), GetType(NB_Store_OptionValueInfo))
        End Function


        Public Function GetOptionValueListWithInterface(ByVal OptionID As Integer, ByVal Lang As String) As ArrayList
            Dim aryList As ArrayList

            aryList = GetOptionValueList(OptionID, Lang)

            If Not OptionInterface.Instance() Is Nothing Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                aryList = OptionInterface.Instance.GetOptionValueList(PS.PortalId, OptionID, Lang, aryList)
            End If

            Return aryList
        End Function

        Public Function UpdateObjOptionValue(ByVal objInfo As NB_Store_OptionValueInfo) As Integer
            Return DataProvider.Instance().UpdateNB_Store_OptionValue(objInfo.OptionValueID, objInfo.OptionID, objInfo.AddedCost, objInfo.ListOrder, objInfo.Lang, objInfo.OptionValueDesc)
        End Function

        Public Sub CreateProductOptionValues(ByVal objOptionInfo As NB_Store_OptionInfo, ByVal CategoryOptionID As Integer)
            'search for category level defaults
            Dim aryList As ArrayList
            aryList = GetOptionValueList(CategoryOptionID, objOptionInfo.Lang)
            If Not aryList Is Nothing Then
                If aryList.Count > 0 Then
                    'add category level option values
                    Dim objOptionVInfo As NB_Store_OptionValueInfo
                    For Each objOptionVInfo In aryList
                        objOptionVInfo.OptionValueID = -1
                        objOptionVInfo.OptionID = objOptionInfo.OptionID
                        UpdateObjOptionValue(objOptionVInfo)
                    Next
                End If
            End If
        End Sub

        Public Sub AddNewOptionValue(ByVal objOptionInfo As NB_Store_OptionInfo)
            Dim objInfo As New NB_Store_OptionValueInfo
            objInfo.OptionID = objOptionInfo.OptionID
            objInfo.Lang = objOptionInfo.Lang
            objInfo.ListOrder = 1
            objInfo.OptionValueDesc = ""
            objInfo.AddedCost = 0
            objInfo.OptionValueID = -1
            UpdateObjOptionValue(objInfo)
        End Sub


#End Region

#Region "NB_Store_ProductDoc Public Methods"

        Public Sub CopyProductDocToLanguages(ByVal objInfo As NB_Store_ProductDocInfo, ByVal ForceOverwrite As Boolean)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopyProductDocToLanguages(objInfo, L, ForceOverwrite)
            Next
        End Sub

        Public Sub CopyProductDocToLanguages(ByVal objInfo As NB_Store_ProductDocInfo, ByVal Lang As String, ByVal ForceOverwrite As Boolean)
            Dim objDummy As NB_Store_ProductDocInfo
            Dim blnDoCopy As Boolean = True

            'check if Language exists
            If Not ForceOverwrite Then
                objDummy = GetProductDoc(objInfo.DocID, Lang)
                If objDummy Is Nothing Then
                    blnDoCopy = True
                Else
                    blnDoCopy = False
                End If
            End If
            If blnDoCopy Then
                objInfo.Lang = Lang
                UpdateObjProductDoc(objInfo)
            End If
        End Sub


        Public Sub DeleteProductDoc(ByVal DocID As Integer)
            DataProvider.Instance().DeleteNB_Store_ProductDoc(DocID)
        End Sub

        Public Function GetProductDoc(ByVal DocID As Integer, ByVal Lang As String) As NB_Store_ProductDocInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_ProductDoc(DocID, Lang), GetType(NB_Store_ProductDocInfo)), NB_Store_ProductDocInfo)
        End Function

        Public Function GetProductDocList(ByVal ProductID As Integer, ByVal Lang As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductDocList(ProductID, Lang), GetType(NB_Store_ProductDocInfo))
        End Function

        Public Function GetProductDocExportList(ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductDocExportList(PortalID), GetType(NB_Store_ProductDocInfo))
        End Function

        Public Function GetProductSelectDocList(ByVal Lang As String, ByVal FilterText As String, ByVal PortalID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductSelectDocList(Lang, FilterText, PortalID), GetType(NB_Store_ProductSelectDocInfo))
        End Function

        Public Function UpdateObjProductDoc(ByVal objInfo As NB_Store_ProductDocInfo) As Integer
            Return DataProvider.Instance().UpdateNB_Store_ProductDoc(objInfo.DocID, objInfo.ProductID, objInfo.DocPath, objInfo.ListOrder, objInfo.Hidden, System.Web.HttpUtility.UrlPathEncode(objInfo.FileName), objInfo.FileExt, objInfo.Lang, objInfo.DocDesc)
        End Function

#End Region

#Region "ProductCategory Public Methods"

        Public Function GetCategoriesAssigned(ByVal ProductID As Integer) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductCategoriesAssigned(ProductID), GetType(NB_Store_ProductCategoryInfo))
        End Function

        Public Sub DeleteProductCategory(ByVal ProductID As Integer, ByVal CategoryID As Integer)
            DataProvider.Instance().DeleteProductCategory(ProductID, CategoryID)
        End Sub

        Public Sub UpdateProductCategory(ByVal ProductID As Integer, ByVal CategoryID As Integer)
            DataProvider.Instance().UpdateProductCategory(ProductID, CategoryID)
        End Sub

        Public Sub UpdateObjProductCategory(ByVal objInfo As NB_Store_ProductCategoryInfo)
            DataProvider.Instance().UpdateProductCategory(objInfo.ProductID, objInfo.CategoryID)
        End Sub

        Public Sub DeleteProductCategory(ByVal ProductID As Integer)
            DeleteProductCategory(ProductID, -1)
        End Sub

#End Region

#Region "NB_Store_ProductRelated Public Methods"

        Public Sub DeleteProductRelated(ByVal RelatedID As Integer)
            DataProvider.Instance().DeleteNB_Store_ProductRelated(RelatedID)
        End Sub

        Public Function GetProductRelated(ByVal RelatedID As Integer) As NB_Store_ProductRelatedInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_ProductRelated(RelatedID), GetType(NB_Store_ProductRelatedInfo)), NB_Store_ProductRelatedInfo)
        End Function

        Public Function GetProductRelatedList(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal RelatedType As Integer, ByVal GetAll As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductRelatedList(PortalID, ProductID, Lang, RelatedType, GetAll), GetType(NB_Store_ProductRelatedListInfo))
        End Function

        Public Function GetProductRelatedArray(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal RelatedType As Integer, ByVal GetAll As Boolean) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_ProductRelatedList(PortalID, ProductID, Lang, RelatedType, GetAll), GetType(NB_Store_ProductRelatedInfo))
        End Function

        Public Function UpdateObjProductRelated(ByVal objInfo As NB_Store_ProductRelatedInfo) As Integer
            Return DataProvider.Instance().UpdateNB_Store_ProductRelated(objInfo.RelatedID, objInfo.PortalID, objInfo.ProductID, objInfo.RelatedProductID, objInfo.DiscountAmt, objInfo.DiscountPercent, objInfo.ProductQty, objInfo.MaxQty, objInfo.RelatedType, objInfo.Disabled, objInfo.NotAvailable, objInfo.BiDirectional)
        End Function

        Public Sub NotAvailableProductRelated(ByVal ProductID As Integer, ByVal Flag As Boolean)
            DataProvider.Instance().NotAvailableProductRelated(ProductID, Flag)
        End Sub

        Public Sub DeleteProductRelatedByProduct(ByVal ProductID As Integer)
            DataProvider.Instance().DeleteNB_Store_ProductRelatedByProduct(ProductID)
        End Sub

#End Region

#Region "General Functions"

        Public Function IsInCategory(ByVal ProductID As Integer, ByVal CategoryID As String) As Boolean
            Dim aryList As ArrayList
            Dim rtnBool As Boolean = False

            aryList = GetCategoriesAssigned(ProductID)
            For Each objC As NB_Store_ProductCategoryInfo In aryList
                If CategoryID = objC.CategoryID Then
                    rtnBool = True
                    Exit For
                End If
            Next
            Return rtnBool
        End Function

        Public Function GetFromPrice(ByVal PortalID As Integer, ByVal ProductID As Integer) As Decimal
            Dim objModelInfo As NB_Store_ModelInfo
            Dim FromPrice As Decimal = -1
            Dim aryList As ArrayList
            aryList = GetModelList(PortalID, ProductID, GetCurrentCulture, True)
            For Each objModelInfo In aryList
                If FromPrice < 0 Then
                    FromPrice = objModelInfo.UnitCost
                Else
                    If FromPrice > objModelInfo.UnitCost Then
                        FromPrice = objModelInfo.UnitCost
                    End If
                End If
            Next
            Return FromPrice
        End Function

        Public Function GetFromPriceCurrency(ByVal PortalID As Integer, ByVal ProductID As Integer) As String
            Dim strFromPrice As String
            strFromPrice = FormatToStoreCurrency(CType(GetFromPrice(PortalID, ProductID), Double))
            Return strFromPrice
        End Function

#End Region

#Region "General Method"

        Public Sub assignImageByProductRef(ByVal ProductID As Integer, ByVal Lang As String, ByVal ImagesFolder As String, ByVal HomeDirectory As String)

            'only do this assign if no images exists
            If GetProductImageList(ProductID, Lang).Count = 0 Then
                Dim objPInfo As NB_Store_ProductsInfo = GetProduct(ProductID, Lang)
                If Not objPInfo Is Nothing Then
                    Dim HiddenFlag As Boolean = False
                    Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
                    Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo
                    Dim aryList As ArrayList

                    'add multiple product images based on productref
                    folderInfo = FileSystemUtils.GetFolder(objPInfo.PortalID, ImagesFolder)
                    If Not folderInfo Is Nothing Then
                        aryList = FileSystemUtils.GetFilesByFolder(objPInfo.PortalID, folderInfo.FolderID)
                        For Each fileInfo In aryList
                            If fileInfo.FileName.ToLower.EndsWith(objPInfo.ProductRef.ToLower & ".jpg") Then
                                AddNewImage(ProductID, Lang, fileInfo.FileName, HomeDirectory & PRODUCTIMAGESFOLDER & "/" & System.IO.Path.GetFileName(fileInfo.FileName), HiddenFlag)
                            End If
                        Next
                    End If
                End If

            End If


        End Sub

#End Region

#Region "XML returns"


        Public Function getProductLinkXML(ByVal objPInfo As ProductListInfo, ByVal ProductTabID As Integer, ByVal CatID As Integer) As String
            Dim strXML As String = ""
            strXML &= "<link>"
            strXML &= GetProductUrl(objPInfo.PortalID, ProductTabID, objPInfo, CatID, False)
            strXML &= "</link>"
            Return strXML
        End Function

        Public Function getProductLinkXML(ByVal objPInfo As NB_Store_ProductsInfo, ByVal ProductTabID As Integer, ByVal CatID As Integer) As String
            Dim strXML As String = ""
            strXML &= "<link>"
            strXML &= GetProductUrl(objPInfo.PortalID, ProductTabID, objPInfo, CatID, False)
            strXML &= "</link>"
            Return strXML
        End Function

        Public Function xmlGetProductsInCat(ByVal PortalID As Integer, ByVal CatID As Integer, ByVal Lang As String, Optional ByVal ProductTabID As Integer = -1) As String
            Dim aryList As ArrayList
            Dim objExp As New Export
            Dim strXML As String = ""

            If CatID >= 0 Then ' don't do this for all categories, could be massive

                aryList = GetProductList(PortalID, CatID, Lang, 1, 50, False)

                strXML &= "<root>"
                For Each objInfo As ProductListInfo In aryList
                    strXML &= "<item>"
                    If ProductTabID >= 0 Then
                        strXML &= getProductLinkXML(objInfo, ProductTabID, CatID)
                    End If
                    strXML &= objExp.GetProductXML(objInfo.ProductID, Lang)
                    strXML &= "</item>"
                Next
                strXML &= "</root>"
            Else
                strXML &= "<root></root>"
            End If

            Return strXML
        End Function

        Public Function xmlGetProduct(ByVal ProdID As Integer, ByVal Lang As String, Optional ByVal ProductTabID As Integer = -1, Optional ByVal CatID As Integer = -1) As String
            Dim aryList As ArrayList
            Dim objExp As New Export

            aryList = GetProductInArray(ProdID, Lang)

            Dim strXML As String = ""
            strXML &= "<root>"
            For Each objInfo As ProductListInfo In aryList
                If ProductTabID >= 0 Then
                    strXML &= getProductLinkXML(objInfo, ProductTabID, CatID)
                End If
                strXML &= objExp.GetProductXML(objInfo.ProductID, Lang)
            Next
            strXML &= "</root>"

            Return strXML

        End Function

#End Region

    End Class

End Namespace
