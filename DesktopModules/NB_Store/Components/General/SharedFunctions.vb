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
Imports System.Xml.Serialization
Imports System.Text
Imports System.Globalization

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports DotNetNuke.Entities
Imports System.Reflection
Imports System.Reflection.Assembly

Namespace NEvoWeb.Modules.NB_Store

    Public Class SharedFunctions

        Public Const DSEDITLOCAL As String = "DSEditLocale"
        Public Const PRODUCTIMAGESFOLDER As String = "productimages" 'TODO: Move to settings
        Public Const PRODUCTDOCSFOLDER As String = "productdocs" 'TODO: Move to settings
        Public Const PRODUCTTHUMBSFOLDER As String = "productthumbs" 'TODO: Move to settings
        Public Const ORDERUPLOADFOLDER As String = "orderuploads" 'TODO: Move to settings
        Public Const NBSTOREAPPNAME As String = "NB_Store"
        Public Const TEMPLATEFOLDER As String = "NB_Store_Templates"
        Public Const NBSTORERESX As String = "/DesktopModules/NB_Store/App_LocalResources/NB_StoreGlobal"

        Public Shared Sub SendEmailToAdministrator(ByVal Portalid As Integer, ByVal SubjectText As String, ByVal EmailBody As String)
            DotNetNuke.Services.Mail.Mail.SendMail(GetStoreEmail(Portalid), GetAdministratorEmail(Portalid), "", SubjectText, EmailBody, "", "TEXT", "", "", "", "")
        End Sub

        Public Shared Sub SendEmailToManager(ByVal Portalid As Integer, ByVal SubjectText As String, ByVal EmailTemplateName As String)
            SendEmailToManager(Portalid, SubjectText, Nothing, EmailTemplateName)
        End Sub
        Public Shared Sub SendEmailToManager(ByVal Portalid As Integer, ByVal SubjectText As String, ByVal objOrderInfo As NB_Store_OrdersInfo, ByVal EmailTemplateName As String)
            Dim ManagerEmail As String = ""
            ManagerEmail = GetMerchantEmail(Portalid)
            SendStoreEmail(Portalid, ManagerEmail, SubjectText, objOrderInfo, EmailTemplateName, GetMerchantCulture(Portalid))
        End Sub

        Public Shared Sub SendEmailToClient(ByVal Portalid As Integer, ByVal ClientEmail As String, ByVal SubjectText As String, ByVal EmailTemplateName As String, ByVal Lang As String)
            SendEmailToClient(Portalid, ClientEmail, SubjectText, Nothing, EmailTemplateName, Lang)
        End Sub

        Public Shared Sub SendEmailToClient(ByVal Portalid As Integer, ByVal ClientEmail As String, ByVal SubjectText As String, ByVal objOrderInfo As NB_Store_OrdersInfo, ByVal EmailTemplateName As String, ByVal Lang As String)
            If Not objOrderInfo Is Nothing Then
                If objOrderInfo.Email <> "" Then
                    ClientEmail = objOrderInfo.Email
                End If
            End If
            SendStoreEmail(Portalid, ClientEmail, SubjectText, objOrderInfo, EmailTemplateName, Lang)
        End Sub

        Public Shared Sub SendStoreEmail(ByVal Portalid As Integer, ByVal ClientEmail As String, ByVal SubjectText As String, ByVal objOrderInfo As NB_Store_OrdersInfo, ByVal EmailTemplateName As String, ByVal Lang As String)
            Dim objSCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim StoreEmail As String = ""
            Dim EmailText As String = ""

            StoreEmail = GetStoreEmail(Portalid)

            objSTInfo = objSCtrl.GetSettingsText(Portalid, EmailTemplateName, Lang)
            If Not objSTInfo Is Nothing Then
                EmailText = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText)
            End If

            SubjectText = System.Web.HttpUtility.HtmlDecode(GetStoreSettingText(Portalid, EmailTemplateName & "subject", Lang))

            If Not objOrderInfo Is Nothing Then
                Dim objTR As New TokenStoreReplace(objOrderInfo, Lang)
                EmailText = objTR.DoTokenReplace(EmailText, True)
                SubjectText = objTR.DoTokenReplace(SubjectText)
            End If

            If GetStoreSetting(Portalid, "debug.email") <> "" Then
                ClientEmail = GetStoreSetting(Portalid, "debug.email")
            End If

            DotNetNuke.Services.Mail.Mail.SendMail(StoreEmail, ClientEmail, "", SubjectText, EmailText, "", "HTML", "", "", "", "")

        End Sub


        Public Shared Function GetClientEmail(ByVal portalid As Integer, ByVal objOInfo As NB_Store_OrdersInfo) As String
            Dim objUCtrl As New Users.UserController
            Dim objUinfo As Users.UserInfo
            Dim rtnEmail As String = ""
            If objOInfo.Email = "" Then
                objUinfo = objUCtrl.GetUser(portalid, objOInfo.UserID)
                If Not objUinfo Is Nothing Then
                    rtnEmail = objUinfo.Email
                End If
            Else
                rtnEmail = objOInfo.Email
            End If
            Return rtnEmail
        End Function

        Public Shared Function GetClientLang(ByVal portalid As Integer, ByVal objOInfo As NB_Store_OrdersInfo) As String
            Return GetClientLang(portalid, objOInfo.UserID)
        End Function

        Public Shared Function GetClientLang(ByVal portalid As Integer, ByVal Userid As Integer) As String
            Dim rtnPreferredLocale As String = GetCurrentCulture()
            Dim objUCtrl As New Users.UserController
            Dim objUinfo As Users.UserInfo
            objUinfo = objUCtrl.GetUser(portalid, Userid)
            If Not objUinfo Is Nothing Then
                rtnPreferredLocale = objUinfo.Profile.PreferredLocale
            End If
            If rtnPreferredLocale = "" Then rtnPreferredLocale = GetCurrentCulture()
            Return rtnPreferredLocale
        End Function


        Public Shared Function GetMerchantCountryCode(ByVal PortalID As Integer) As String
            Return Right(GetMerchantCulture(PortalID), 2)
        End Function

        Public Shared Function GetStoreEmail(ByVal PortalID As Integer) As String
            Dim objCtrl As New SettingsController
            Dim objInfo As NB_Store_SettingsInfo
            objInfo = objCtrl.GetSetting(PortalID, "store.email", "None")
            If objInfo Is Nothing Then
                objInfo = objCtrl.GetSetting(PortalID, "merchant.email", "None")
                If objInfo Is Nothing Then
                    Return ""
                Else
                    Return objInfo.SettingValue
                End If
            Else
                Return objInfo.SettingValue
            End If
        End Function

        Public Shared Function GetMerchantEmail(ByVal PortalID As Integer) As String
            Dim objCtrl As New SettingsController
            Dim objInfo As NB_Store_SettingsInfo
            objInfo = objCtrl.GetSetting(PortalID, "merchant.email", "None")
            If objInfo Is Nothing Then
                objInfo = objCtrl.GetSetting(PortalID, "store.email", "None")
                If objInfo Is Nothing Then
                    Return ""
                Else
                    Return objInfo.SettingValue
                End If
            Else
                Return objInfo.SettingValue
            End If
        End Function

        Public Shared Function GetAdministratorEmail(ByVal PortalID As Integer) As String
            Dim objCtrl As New SettingsController
            Dim objInfo As NB_Store_SettingsInfo
            objInfo = objCtrl.GetSetting(PortalID, "administrator.email", "None")
            If objInfo Is Nothing Then
                Return ""
            Else
                Return objInfo.SettingValue
            End If
        End Function

        Public Shared Function GetMerchantCulture(ByVal PortalID As Integer) As String
            Dim objCtrl As New SettingsController
            Dim objInfo As NB_Store_SettingsInfo
            objInfo = objCtrl.GetSetting(PortalID, "merchant.culture", "None")
            If objInfo Is Nothing Then
                Return GetCurrentCulture()
            Else
                Return objInfo.SettingValue
            End If
        End Function

        Public Shared Function GetCurrentCulture() As String
            Return System.Threading.Thread.CurrentThread.CurrentCulture.ToString()
        End Function

        Public Shared Function GetCurrentCountryCode() As String
            Return System.Threading.Thread.CurrentThread.CurrentCulture.Name.Substring(3, 2)
        End Function

        Public Shared Function GetCurrentLangCode() As String
            Return System.Threading.Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2)
        End Function

        Public Shared Function GetDefaultShipMethod(ByVal PortalID As Integer) As Integer
            Dim strCacheKey As String = "DefaultShipMethod" & PortalID.ToString
            If DataCache.GetCache(strCacheKey) Is Nothing Then
                Dim objCtrl As New ShipController
                Dim objInfo As NB_Store_ShippingMethodInfo
                Dim aryList As ArrayList
                aryList = objCtrl.GetShippingMethodList(PortalID)
                If aryList.Count > 0 Then
                    objInfo = aryList.Item(0)
                Else
                    Return -1
                End If
                DataCache.SetCache(strCacheKey, objInfo.ShipMethodID, DateAdd(DateInterval.Hour, 1, Now))
                Return objInfo.ShipMethodID
            Else
                Return CInt(DataCache.GetCache(strCacheKey))
            End If
        End Function

        Public Shared Sub PopulateAvailableLanguages(ByVal ddlLocales As DropDownList, ByVal Request As System.Web.HttpRequest)
            PopulateAvailableLanguages(ddlLocales, Request, "", "")
        End Sub

        Public Shared Sub PopulateAvailableLanguages(ByVal ddlLocales As DropDownList, ByVal Request As System.Web.HttpRequest, ByVal DefaultSelectValue As String, ByVal DefaultSelectText As String)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim i As Integer
            ddlLocales.Items.Clear()
            For i = 0 To supportedLanguages.Count - 1
                Dim info As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(CType(supportedLanguages(i).Value, Locale).Code)
                Dim item As New ListItem
                item.Value = CType(supportedLanguages(i).Value, Locale).Code

                item.Text = info.NativeName & " (" & CType(supportedLanguages(i).Value, Locale).Code & ")"
                ddlLocales.Items.Add(item)
            Next

            If DefaultSelectValue <> "" Then
                Dim li As New ListItem
                li.Value = DefaultSelectValue
                li.Text = DefaultSelectText
                ddlLocales.Items.Insert(0, li)
            End If

            Dim cl As String = GetCurrentCulture()
            If Not Request(DSEDITLOCAL) Is Nothing Then
                cl = Convert.ToString(Request(DSEDITLOCAL))
            End If
            If Not ddlLocales.Items.FindByValue(cl) Is Nothing Then
                ddlLocales.ClearSelection()
                ddlLocales.Items.FindByValue(cl).Selected = True
            End If

        End Sub

#Region "Cache function"

        Public Shared Sub removeStoreTemplateCache(ByVal PortalID As Integer)
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim aryList As ArrayList

            aryList = objSCtrl.GetSettingsTexts(PortalID, GetCurrentCulture, True, "")
            For Each objSTInfo In aryList
                removeLangCache(PortalID, "TX_" & objSTInfo.SettingName)
            Next
            aryList = objSCtrl.GetSettingList(PortalID, GetCurrentCulture, True, "")
            For Each objSInfo In aryList
                removeLangCache(PortalID, objSInfo.SettingName)
            Next
        End Sub


        Public Shared Sub removeLangCache(ByVal PortalID As Integer, ByVal CacheKey As String)
            Try
                Dim supportedLanguages As LocaleCollection = GetValidLocales()
                Dim i As Integer
                For i = 0 To supportedLanguages.Count - 1
                    DataCache.RemoveCache(CType(supportedLanguages(i).Value, Locale).Code.ToLower & CacheKey.ToLower & PortalID.ToString)
                Next
            Catch ex As Exception
                'will not work if ran from scheduler.
            End Try
            DataCache.RemoveCache("none" & CacheKey.ToLower & PortalID.ToString)
        End Sub

        Public Shared Sub setLangCache(ByVal PortalID As Integer, ByVal CacheKey As String, ByVal Lang As String, ByVal objObject As Object, ByVal CahceMins As Integer)
            DataCache.SetCache(Lang.ToLower & CacheKey.ToLower & PortalID.ToString, objObject, DateAdd(DateInterval.Minute, CahceMins, Now))
        End Sub

        Public Shared Sub setLangCache(ByVal PortalID As Integer, ByVal CacheKey As String, ByVal Lang As String, ByVal objObject As Object)
            setLangCache(PortalID, CacheKey, Lang, objObject, 60)
        End Sub

        Public Shared Sub setLangCache(ByVal PortalID As Integer, ByVal CacheKey As String, ByVal Lang As String, ByVal objS As NB_Store_SettingsTextInfo)
            'strongtyped version - To support Recurring templates
            If objS.SettingText.Contains("[") Then 'Let us not even bother if essential token information isn't there. Speed is important!
                Dim objTR As New TokenStoreReplace(PortalID, Lang)
                objS.SettingText = objTR.replaceTemplates(objS.SettingText, Lang)
            End If
            setLangCache(PortalID, CacheKey, Lang, objS, 60)
        End Sub
        Public Shared Sub setLangCache(ByVal PortalID As Integer, ByVal CacheKey As String, ByVal Lang As String, ByVal objS As NB_Store_SettingsInfo)
            'strongtyped version - To support Recurring templates
            If objS.SettingValue.Contains("[") Then 'Let us not even bother if essential token information isn't there. Speed is important!
                Dim objTR As New TokenStoreReplace(PortalID, Lang)
                objS.SettingValue = objTR.replaceTemplates(objS.SettingValue, Lang)
            End If
            setLangCache(PortalID, CacheKey, Lang, objS, 60)
        End Sub

        Public Shared Function getLangCache(ByVal PortalID As Integer, ByVal CacheKey As String, ByVal Lang As String) As Object
            Return DataCache.GetCache(Lang.ToLower & CacheKey.ToLower & PortalID.ToString)
        End Function

#End Region


        Public Shared Function getStatusList(ByVal Lang As String) As Hashtable
            Dim objOCtrl As New OrderController
            Dim arylist As ArrayList
            Dim objOSInfo As NB_Store_OrderStatusInfo
            Dim htab As New Hashtable

            arylist = objOCtrl.GetOrderStatusList(Lang)

            For Each objOSInfo In arylist
                htab.Add(objOSInfo.OrderStatusID, objOSInfo.OrderStatusText)
            Next
            Return htab
        End Function

        Public Shared Sub populateStatusList(ByVal ddlSearch As DropDownList, ByVal DefaultSelectValue As String, ByVal DefaultSelectText As String, ByVal SelectedValue As String)
            Dim objOCtrl As New OrderController
            Dim arylist As ArrayList
            Dim objOSInfo As NB_Store_OrderStatusInfo
            Dim li As ListItem

            arylist = objOCtrl.GetOrderStatusList(GetCurrentCulture)

            ddlSearch.Items.Clear()
            For Each objOSInfo In arylist
                li = New ListItem
                li.Value = objOSInfo.OrderStatusID
                li.Text = objOSInfo.OrderStatusText
                ddlSearch.Items.Add(li)
            Next

            If DefaultSelectValue <> "" Then
                li = New ListItem
                li.Value = DefaultSelectValue
                li.Text = DefaultSelectText
                ddlSearch.Items.Insert(0, li)
            End If

            If Not ddlSearch.Items.FindByValue(SelectedValue) Is Nothing Then
                ddlSearch.ClearSelection()
                ddlSearch.Items.FindByValue(SelectedValue).Selected = True
            End If

        End Sub

        Public Shared Sub populateShipMethodList(ByVal ddlShipMethod As DropDownList, ByVal PortalID As Integer, Optional ByVal SelectedValue As String = "")
            Dim objCtrl As New ShipController
            Dim arylist As ArrayList
            Dim objInfo As NB_Store_ShippingMethodInfo
            Dim li As ListItem

            arylist = objCtrl.GetShippingMethodList(PortalID)

            ddlShipMethod.Items.Clear()
            For Each objInfo In arylist
                li = New ListItem
                li.Value = objInfo.ShipMethodID
                li.Text = objInfo.MethodName
                ddlShipMethod.Items.Add(li)
            Next

            If Not ddlShipMethod.Items.FindByValue(SelectedValue) Is Nothing Then
                ddlShipMethod.ClearSelection()
                ddlShipMethod.Items.FindByValue(SelectedValue).Selected = True
            End If

        End Sub


        Public Shared Sub populateCategoryList(ByVal PortalId As Integer, ByVal ddlCategories As DropDownList)
            populateCategoryList(PortalId, ddlCategories, "", "", "")
        End Sub

        Public Shared Sub populateDropDownList(ByVal ddl As DropDownList, ByVal CSVList As String)
            Dim li As ListItem
            Dim ValueList() As String

            ddl.Items.Clear()

            ValueList = Split(CSVList, ","c)

            For lp As Integer = 0 To ValueList.GetUpperBound(0)
                li = New ListItem
                li.Value = lp.ToString
                li.Text = ValueList(lp)
                ddl.Items.Add(li)
            Next

        End Sub

        Public Shared Sub populateCategoryList(ByVal PortalId As Integer, ByVal ddlCategories As DropDownList, ByVal DefaultSelectValue As String, ByVal DefaultSelectText As String, ByVal SelectedValue As String)
            Dim objCtrl As New CategoryController
            Dim li As ListItem
            Dim aryList As ArrayList

            ddlCategories.Items.Clear()

            aryList = objCtrl.GetCategories(PortalId, GetCurrentCulture)

            BuildCategoryList(aryList, ddlCategories, 0, "")

            If DefaultSelectValue <> "" Then
                li = New ListItem
                li.Value = DefaultSelectValue
                li.Text = DefaultSelectText
                ddlCategories.Items.Insert(0, li)
            End If

            If Not ddlCategories.Items.FindByValue(SelectedValue) Is Nothing Then
                ddlCategories.ClearSelection()
                ddlCategories.Items.FindByValue(SelectedValue).Selected = True
            End If

        End Sub

        Public Shared Sub BuildCategoryList(ByVal aryList As ArrayList, ByVal ddlCategories As DropDownList, ByVal ParentID As Integer, ByVal Prefix As String)
            Dim objCInfo As NB_Store_CategoriesInfo
            Dim li As ListItem
            For Each objCInfo In aryList
                If objCInfo.ParentCategoryID = ParentID Then
                    li = New ListItem
                    li.Value = objCInfo.CategoryID.ToString
                    li.Text = Prefix & objCInfo.CategoryName
                    ddlCategories.Items.Add(li)
                    BuildCategoryList(aryList, ddlCategories, objCInfo.CategoryID, Prefix & ".")
                End If
            Next
        End Sub

        Public Shared Sub populateTemplateList(ByVal PortalId As Integer, ByVal ddlTemplates As DropDownList, Optional ByVal FilterExt As String = "", Optional ByVal DefaultSelectValue As String = "", Optional ByVal DefaultSelectText As String = "", Optional ByVal SelectedValue As String = "")
            Dim objCtrl As New SettingsController
            Dim htCategories As Hashtable
            Dim li As ListItem

            htCategories = objCtrl.GetTemplateTable(PortalId)

            Dim keys As ICollection = htCategories.Keys
            Dim keysArray(htCategories.Count - 1) As String
            keys.CopyTo(keysArray, 0)
            Array.Sort(keysArray)

            ddlTemplates.Items.Clear()
            For Each key As String In keysArray
                li = New ListItem
                li.Value = key.ToString
                li.Text = key.ToString
                If FilterExt <> "" Then
                    If li.Value.ToLower.EndsWith(FilterExt.ToLower) Then
                        ddlTemplates.Items.Add(li)
                    End If
                Else
                    ddlTemplates.Items.Add(li)
                End If
            Next

            If DefaultSelectValue <> "" Then
                li = New ListItem
                li.Value = DefaultSelectValue
                li.Text = DefaultSelectText
                ddlTemplates.Items.Insert(0, li)
            End If

            If Not ddlTemplates.Items.FindByValue(SelectedValue) Is Nothing Then
                ddlTemplates.ClearSelection()
                ddlTemplates.Items.FindByValue(SelectedValue).Selected = True
            End If

        End Sub

        Public Shared Sub populateCountryList(ByVal PortalID As Integer, ByVal ddlCountry As DropDownList)
            populateCountryList(PortalID, ddlCountry, "", "", "")
        End Sub

        Public Shared Sub populateCountryList(ByVal PortalID As Integer, ByVal ddlCountry As DropDownList, ByVal DefaultValue As String)
            populateCountryList(PortalID, ddlCountry, DefaultValue, "", "")
        End Sub

        Public Shared Sub populateCountryList(ByVal PortalID As Integer, ByVal ddlCountry As DropDownList, ByVal DefaultValue As String, ByVal NoneValue As String, ByVal NoneText As String)
            populateCountryList(PortalID, ddlCountry, DefaultValue, NoneValue, NoneText, False)
        End Sub

        Public Shared Sub populateCountryList(ByVal PortalID As Integer, ByVal ddlCountry As DropDownList, ByVal DefaultValue As String, ByVal NoneValue As String, ByVal NoneText As String, ByVal PrefixCode As Boolean)
            Dim objCtrl As New CategoryController
            Dim li As ListItem
            Dim strCountryList As String = ""

            Dim ctlEntry As New CountryLists

            Dim entryCollection As DotNetNuke.Common.Lists.ListEntryInfoCollection
            entryCollection = ctlEntry.getCountryList(GetCurrentCulture)

            If PortalID >= 0 Then
                'get portal level counties allowed
                Dim objSCtrl As New SettingsController
                Dim objSInfo As NB_Store_SettingsInfo
                objSInfo = objSCtrl.GetSetting(PortalID, "country.list", GetCurrentCulture)
                If Not objSInfo Is Nothing Then
                    strCountryList = objSInfo.SettingValue
                End If
            End If


            ddlCountry.Items.Clear()
            For Each lstInfo As DotNetNuke.Common.Lists.ListEntryInfo In entryCollection
                li = New ListItem
                li.Value = lstInfo.Value
                If PrefixCode Then
                    li.Text = lstInfo.Value & " - " & lstInfo.Text
                Else
                    li.Text = lstInfo.Text
                End If
                If strCountryList = "" Then
                    ddlCountry.Items.Add(li)
                Else
                    If strCountryList.Contains(lstInfo.Text) Then
                        ddlCountry.Items.Add(li)
                    End If
                End If
            Next

            If NoneValue <> "" Then
                li = New ListItem
                li.Value = NoneValue
                li.Text = NoneText
                ddlCountry.Items.Insert(0, li)
            End If

            If Not ddlCountry.Items.FindByValue(DefaultValue) Is Nothing Then
                ddlCountry.SelectedValue = DefaultValue
            End If

        End Sub

        Public Shared Function GetCountryByCode(ByVal CountryCode As String) As String
            Dim ctlEntry As New Lists.ListController
            Dim entryCollection As Lists.ListEntryInfoCollection = ctlEntry.GetListEntryInfoCollection("Country", CountryCode)
            If entryCollection.Count = 0 Then
                Return ""
            Else
                Return entryCollection.Item(0).Text
            End If
        End Function

        Public Shared Sub populateTabsList(ByVal lstTabs As DropDownList, ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal DefaultValue As String)
            lstTabs.DataSource = GetPortalTabs(PortalSettings.DesktopTabs, False, True, False, True)
            lstTabs.DataTextField = "TabName"
            lstTabs.DataValueField = "TabId"
            lstTabs.DataBind()

            If DefaultValue <> "" Then
                If Not lstTabs.Items.FindByValue(DefaultValue) Is Nothing Then
                    lstTabs.SelectedValue = DefaultValue
                End If
            End If
        End Sub

        Public Shared Sub populateFileList(ByVal lstFiles As DropDownList, ByVal DirectoryPath As String, ByVal Pattern As String)
            Dim lp As Integer
            Dim lst As String()
            Dim li As ListItem

            lst = System.IO.Directory.GetFiles(DirectoryPath, Pattern)

            lstFiles.Items.Clear()

            For lp = 0 To lst.GetUpperBound(0)
                li = New ListItem
                li.Value = System.IO.Path.GetFileName(lst(lp))
                li.Text = System.IO.Path.GetFileName(lst(lp))
                lstFiles.Items.Add(li)
            Next

        End Sub

        Public Shared Sub populateFileList(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal lstFiles As DropDownList, ByVal DirectoryPath As String, ByVal EndsWith As String)
            Dim li As ListItem
            Dim aryList As ArrayList
            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo

            folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, DirectoryPath)
            If Not folderInfo Is Nothing Then
                aryList = FileSystemUtils.GetFilesByFolder(PortalSettings.PortalId, folderInfo.FolderID)
                lstFiles.Items.Clear()
                For Each fileInfo In aryList
                    If fileInfo.FileName.EndsWith(EndsWith) Then
                        li = New ListItem
                        li.Value = System.IO.Path.GetFileName(fileInfo.FileName)
                        li.Text = System.IO.Path.GetFileName(fileInfo.FileName)
                        lstFiles.Items.Add(li)
                    End If
                Next
            End If
        End Sub

        Public Shared Sub CreateDir(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal FolderName As String)
            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim blnCreated As Boolean = False

            'try normal test (doesn;t work on medium trust, but avoids waiting for GetFolder.)
            Try
                blnCreated = System.IO.Directory.Exists(PortalSettings.HomeDirectoryMapPath & FolderName)
            Catch ex As Exception
                blnCreated = False
            End Try

            If Not blnCreated Then
                folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, FolderName)
                If folderInfo Is Nothing And FolderName <> "" Then
                    Dim objFolderCtrl As New DotNetNuke.Services.FileSystem.FolderController
                    'add folder and permissions
                    FileSystemUtils.AddFolder(PortalSettings, PortalSettings.HomeDirectoryMapPath, FolderName)
                    folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, FolderName)
                    If Not folderInfo Is Nothing Then
                        Dim folderid As Integer = folderInfo.FolderID
                        Dim objPermissionController As New DotNetNuke.Security.Permissions.PermissionController
                        Dim arr As ArrayList = objPermissionController.GetPermissionByCodeAndKey("SYSTEM_FOLDER", "")
                        For Each objpermission As DotNetNuke.Security.Permissions.PermissionInfo In arr
                            If objpermission.PermissionKey = "WRITE" Then
                                ' add READ permissions to the All Users Role
                                FileSystemUtils.SetFolderPermission(PortalSettings.PortalId, folderid, objpermission.PermissionID, Integer.Parse(glbRoleAllUsers), "")
                            End If
                        Next
                    End If
                End If
            End If
        End Sub

        Public Shared Function IsImageFile(ByVal strExtension As String) As Boolean
            If LCase(strExtension) = ".jpg" Or _
            LCase(strExtension) = ".jpeg" Or _
            LCase(strExtension) = ".gif" Or _
            LCase(strExtension) = ".png" Or _
            LCase(strExtension) = ".tiff" Or _
            LCase(strExtension) = ".bmp" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Sub RemoveFiles(ByVal PortalSettings As DotNetNuke.Entities.Portals.PortalSettings, ByVal sourceDir As String)
            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo
            Dim aryList As ArrayList

            folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, sourceDir)
            If Not folderInfo Is Nothing Then
                aryList = FileSystemUtils.GetFilesByFolder(PortalSettings.PortalId, folderInfo.FolderID)
                For Each fileInfo In aryList
                    FileSystemUtils.DeleteFile(PortalSettings.HomeDirectoryMapPath & sourceDir & "\" & fileInfo.FileName, PortalSettings, True)
                Next
            End If
        End Sub

        Public Shared Function ReplaceFileExt(ByVal FileName As String, ByVal NewExt As String) As String
            Dim strOut As String = ""
            strOut = System.IO.Path.GetDirectoryName(FileName) & "\" & System.IO.Path.GetFileNameWithoutExtension(FileName) & NewExt
            Return strOut
        End Function

        Public Shared Function GetOptCodeInfo(ByVal PortalID As Integer, ByVal OptCode As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As OptCodeInfo
            Return GetOptCodeInfo(PortalID, OptCode, UserInfo, "", "")
        End Function

        Public Shared Function GetOptCodeInfo(ByVal PortalID As Integer, ByVal OptCode As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal TextOptions As String, ByVal TextOptionsXML As String) As OptCodeInfo
            Dim optC As String()
            Dim lp As Integer
            Dim objCtrl As New ProductController
            Dim objOVInfo As NB_Store_OptionValueInfo
            Dim objMInfo As NB_Store_ModelInfo
            Dim objPInfo As NB_Store_ProductsInfo
            Dim rtnAmount As OptCodeInfo = Nothing
            Dim SalePrice As Double
            Dim objPromoCtrl As New PromoController
            Dim OptSeperator As String = GetStoreSettingText(PortalID, "optionseperator.text", GetCurrentCulture, False, True)
            If OptSeperator = "" Then OptSeperator = "/"

            'check if we have a cartlevel CartCalcProvider
            If Not CalcCartInterface.Instance() Is Nothing Then
                rtnAmount = CalcCartInterface.Instance.GetOptCodeInfo(PortalID, OptCode, UserInfo, TextOptions, TextOptionsXML)
            End If
            If rtnAmount Is Nothing Then

                rtnAmount = New OptCodeInfo
                rtnAmount.ItemDesc = ""
                rtnAmount.UnitCost = 0
                rtnAmount.OptCode = ""

                optC = OptCode.Split("-"c)

                For lp = 0 To optC.GetUpperBound(0)
                    If IsNumeric(optC(lp)) Then
                        If lp = 0 Then
                            'first id in list is modelid
                            objMInfo = objCtrl.GetModel(CInt(optC(lp)), GetCurrentCulture)
                            If Not objMInfo Is Nothing Then
                                objPInfo = objCtrl.GetProduct(objMInfo.ProductID, GetCurrentCulture)
                                If objPInfo Is Nothing Then
                                    rtnAmount.ItemDesc = objMInfo.ModelName & "."
                                Else
                                    rtnAmount.ItemDesc = objPInfo.ProductName & " " & Replace(objMInfo.ModelName, objPInfo.ProductName, "") & "." & objMInfo.ModelRef & "."
                                    rtnAmount.ItemDesc = Trim(rtnAmount.ItemDesc)
                                End If
                                rtnAmount.UnitCost = rtnAmount.UnitCost + objMInfo.UnitCost
                                SalePrice = objPromoCtrl.GetSalePrice(objMInfo, UserInfo)
                                If IsDealer(objMInfo.PortalID, UserInfo, GetCurrentCulture) Then
                                    If objMInfo.DealerCost > 0 Then
                                        If (SalePrice > -1) And (SalePrice < objMInfo.DealerCost) Then
                                            rtnAmount.Discount = SalePrice - rtnAmount.UnitCost
                                        Else
                                            rtnAmount.Discount = objMInfo.DealerCost - rtnAmount.UnitCost
                                        End If
                                    Else
                                        If SalePrice > -1 Then
                                            rtnAmount.Discount = SalePrice - rtnAmount.UnitCost
                                        Else
                                            rtnAmount.Discount = 0
                                        End If
                                    End If
                                Else
                                    If SalePrice > -1 Then
                                        rtnAmount.Discount = SalePrice - rtnAmount.UnitCost
                                    Else
                                        rtnAmount.Discount = 0
                                    End If
                                End If
                                rtnAmount.OptCode &= optC(lp) & "-"
                            End If
                        Else
                            If lp = 1 Then
                                'second loop, so must be options, change the . to a / on end of desc
                                rtnAmount.ItemDesc = rtnAmount.ItemDesc.TrimEnd("."c) & OptSeperator
                            End If
                            objOVInfo = objCtrl.GetOptionValue(CInt(optC(lp)), GetCurrentCulture)
                            If Not objOVInfo Is Nothing Then
                                rtnAmount.UnitCost = rtnAmount.UnitCost + objOVInfo.AddedCost
                                rtnAmount.ItemDesc &= objOVInfo.OptionValueDesc & OptSeperator
                                rtnAmount.OptCode &= optC(lp) & "-"
                                'NOTE: Store Level Discount amounts only apply to model price, not to added option cost.
                            End If
                        End If
                    End If
                Next
                rtnAmount.ItemDesc = rtnAmount.ItemDesc.TrimEnd("."c)
                If rtnAmount.ItemDesc.EndsWith(OptSeperator) Then
                    rtnAmount.ItemDesc = rtnAmount.ItemDesc.Substring(0, (rtnAmount.ItemDesc.Length - OptSeperator.Length))
                End If
                rtnAmount.OptCode = rtnAmount.OptCode.TrimEnd("-"c)
                If TextOptions = "" Then
                    If rtnAmount.ItemDesc.EndsWith(OptSeperator) Then
                        rtnAmount.ItemDesc = rtnAmount.ItemDesc.Substring(0, (rtnAmount.ItemDesc.Length - OptSeperator.Length))
                    End If
                Else
                    rtnAmount.ItemDesc = rtnAmount.ItemDesc & OptSeperator & TextOptions
                End If

            End If
            Return rtnAmount
        End Function

        Public Shared Function GetMsgText(ByVal PortalId As String, ByVal TextTemplateID As String, ByVal DefaultMsg As String) As String
            Dim objSCtrl As New NB_Store.SettingsController
            Dim objInfo As NB_Store_SettingsTextInfo
            Dim MsgText As String = DefaultMsg

            objInfo = objSCtrl.GetSettingsText(PortalId, TextTemplateID, GetCurrentCulture)
            If Not objInfo Is Nothing Then
                If objInfo.SettingText <> "" Then
                    MsgText = objInfo.SettingText

                    'do link replaces for login
                    MsgText = Replace(MsgText, "[TAG:Login]", "javascript:__doPostBack('dnn$dnnLOGIN$cmdLogin','')")
                    MsgText = Replace(MsgText, "[TAG:Register]", "javascript:__doPostBack('dnn$dnnUSER$cmdRegister','')")
                    MsgText = Replace(MsgText, "[TAG:SkipLogin]", NavigateURL("", "stg=2", "logmsg=0"))

                    'do link replace for store email
                    MsgText = Replace(MsgText, "[TAG:STOREEMAIL]", GetStoreEmail(PortalId))

                    'if there are anymore tokens then called the full token replace system
                    If InStr(MsgText, "[") > 0 Then
                        Dim objTR As New TokenStoreReplace(PortalId, GetCurrentCulture)
                        MsgText = objTR.DoTokenReplace(MsgText)
                    End If
                End If
            End If
            Return MsgText
        End Function


        Public Shared Sub DisplayMsgText(ByVal PortalId As String, ByVal plHolder As PlaceHolder, ByVal TextTemplateID As String, ByVal DefaultMsg As String)
            Dim MsgText As String = GetMsgText(PortalId, TextTemplateID, DefaultMsg)
            plHolder.Controls.Add(New LiteralControl(System.Web.HttpUtility.HtmlDecode(MsgText)))
        End Sub

        Public Shared Function HTTPPOSTEncode(ByVal postString As String) As String
            postString = postString.Replace("\", "")
            postString = System.Web.HttpUtility.UrlEncode(postString)
            postString = postString.Replace("%2f", "/")
            Return postString
        End Function

        Public Shared Function IsOnlyManagerLite(ByVal Portalid As Integer, ByVal Usrinfo As Users.UserInfo) As Boolean
            If Usrinfo Is Nothing Then
                Return False
            Else
                Dim ManagerLiteRole As String = ""
                ManagerLiteRole = GetStoreSetting(Portalid, "managerlite.role", GetCurrentCulture)
                If Usrinfo.IsInRole(ManagerLiteRole) Then
                    If Usrinfo.IsSuperUser Or Usrinfo.IsInRole("Administrator") Or Usrinfo.IsInRole(GetStoreSetting(Portalid, "manager.role", GetCurrentCulture)) Then
                        Return False
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            End If
        End Function


        Public Shared Function IsManager(ByVal Portalid As Integer, ByVal Usrinfo As Users.UserInfo) As Boolean
            If Usrinfo Is Nothing Then
                Return False
            Else
                Dim ManagerRole As String = ""
                Dim ManagerLiteRole As String = ""

                ManagerLiteRole = GetStoreSetting(Portalid, "managerlite.role", GetCurrentCulture)
                ManagerRole = GetStoreSetting(Portalid, "manager.role", GetCurrentCulture)

                If Usrinfo.IsInRole("Administrators") Or Usrinfo.IsInRole(ManagerRole) Or Usrinfo.IsInRole(ManagerLiteRole) Or Usrinfo.IsSuperUser Then
                    Return True
                Else
                    Return False
                End If
            End If
        End Function

        Public Shared Function IsEditor(ByVal Portalid As Integer, ByVal Usrinfo As Users.UserInfo) As Boolean
            If Usrinfo Is Nothing Then
                Return False
            Else
                Dim EditorRole As String = ""

                If IsManager(Portalid, Usrinfo) Then
                    Return True
                Else
                    EditorRole = GetStoreSetting(Portalid, "editor.role", GetCurrentCulture)
                    If Usrinfo.IsInRole(EditorRole) Then
                        Return True
                    Else
                        Return False
                    End If
                End If

            End If
        End Function

        Public Shared Sub UpdateLog(ByVal LogMsg As String)
            UpdateLog(-1, LogMsg)
        End Sub


        Public Shared Sub UpdateLog(ByVal Userid As Integer, ByVal LogMsg As String)
            If LogMsg <> "" Then
                Dim usrName As String = ""
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                If Userid > -1 Then
                    Dim objUInfo As Users.UserInfo
                    objUInfo = Users.UserController.GetUserById(PS.PortalId, Userid)
                    usrName = objUInfo.Username
                End If
                Dim FileLog As New LogText
                FileLog.FileName = PS.HomeDirectoryMapPath & "LogFiles\NB_Store" & "_" & Today.Year & Format(Today.Month, "00") & Format(Today.Day, "00") & ".Log"
                FileLog.Log(Now.ToString & " - " & usrName & " - " & LogMsg)
            End If
        End Sub

#Region "XSL functions"

        Public Shared Function XSLTransByTemplate(ByVal xmlData As String, ByVal XslTemplateName As String) As String
            ' This function looks for an xsl file/template with the name ,
            ' if it exists it then does an xsl translate 
            Dim DebugMode As Boolean = False
            Dim xslData As New XmlDocument
            Dim xslDataStr As String = ""
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim XSLFName As String = XslTemplateName
            DebugMode = GetStoreSettingBoolean(PS.PortalId, "debug.mode", "None")

            'check for the xsl in the template settings.
            xslDataStr = System.Web.HttpUtility.HtmlDecode(GetStoreSettingText(PS.PortalId, XSLFName, GetCurrentCulture))
            If xslDataStr = "" Then
                ' Not in settings so load XSLT file
                Try
                    xslData.Load(PS.HomeDirectoryMapPath & XSLFName)
                    xslDataStr = xslData.OuterXml
                Catch ex As Exception
                    xslDataStr = ""
                    'may not exist, so ignore
                End Try
            End If

            If DebugMode Then
                xslData = New XmlDocument
                Try
                    xslData.LoadXml(xmlData)
                    xslData.Save(PS.HomeDirectoryMapPath & "debug_" & Replace(XslTemplateName, ".xsl", "") & ".xml")
                Catch ex As Exception
                    'xml may not be valid, ignore.
                End Try
            End If

            If xslDataStr = "" Then
                Return "NO XSL FOUND: " & XslTemplateName
            Else
                Try
                    Return XSLTransInMemory(xmlData, xslDataStr)
                Catch ex As Exception
                    UpdateLog("XML ERROR on " & XslTemplateName & " : " & ex.ToString)
                    Return ""
                End Try
            End If

        End Function

        Public Shared Function XSLTrans(ByVal xmlData As String, ByVal XslFilePath As String) As String
            Try

                Dim xmlDoc As New XmlDataDocument

                xmlDoc.LoadXml(xmlData)

                Dim xslt As New System.Xml.Xsl.XslCompiledTransform()

                xslt.Load(XslFilePath)

                Dim MyWriter As New System.IO.StringWriter
                xslt.Transform(xmlDoc, Nothing, MyWriter)

                Return MyWriter.ToString
            Catch ex As Exception
                Return ex.ToString
            End Try
        End Function


        Public Shared Function XSLTransInMemory(ByVal xmlData As String, ByVal XslData As String) As String
            Try

                Dim xmlDoc As New XmlDataDocument

                xmlDoc.LoadXml(xmlData)

                Dim bytes As Byte() = System.Text.Encoding.UTF8.GetBytes(XslData)
                Dim xslStream As New System.IO.MemoryStream(bytes)
                xslStream.Position = 0

                Dim xslStylesheet As System.Xml.XmlReader
                xslStylesheet = New System.Xml.XmlTextReader(xslStream)

                Dim xslt As New System.Xml.Xsl.XslCompiledTransform()
                xslt.Load(xslStylesheet)

                Dim MyWriter As New System.IO.StringWriter
                xslt.Transform(xmlDoc, Nothing, MyWriter)

                Return MyWriter.ToString
            Catch ex As Exception
                Return ex.ToString
            End Try
        End Function

#End Region

        Public Shared Function GetStoreSettingBoolean(ByVal Portalid As Integer, ByVal SettingName As String) As Boolean
            Return GetStoreSettingBoolean(Portalid, SettingName, "None")
        End Function

        Public Shared Function GetStoreSettingBoolean(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String) As Boolean
            Try
                Return CBool(GetStoreSetting(Portalid, SettingName, Lang, False))
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Function GetStoreSettingInt(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String) As Integer
            Dim strTemp As String = GetStoreSetting(Portalid, SettingName, Lang, False)
            If IsNumeric(strTemp) Then
                Return CInt(strTemp)
            Else
                Return 0
            End If

        End Function

        Public Shared Function GetStoreSetting(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As String
            SettingName = getSettingName(Portalid, SettingName, UserInfo)
            Return GetStoreSetting(Portalid, SettingName, Lang, False)
        End Function

        Public Shared Function GetStoreSetting(ByVal Portalid As Integer, ByVal SettingName As String) As String
            Return GetStoreSetting(Portalid, SettingName, "None", False)
        End Function

        Public Shared Function GetStoreSetting(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String) As String
            Return GetStoreSetting(Portalid, SettingName, Lang, False)
        End Function

        Public Shared Function GetStoreSetting(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal NotCached As Boolean) As String
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            If NotCached Then
                objSInfo = objSCtrl.GetSettingNotCached(Portalid, SettingName, Lang)
            Else
                objSInfo = objSCtrl.GetSetting(Portalid, SettingName, Lang)
            End If
            If objSInfo Is Nothing Then
                Return ""
            Else
                Return objSInfo.SettingValue
            End If
        End Function


        Public Shared Function GetStoreSettingText(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal NotCached As Boolean, ByVal HtmlDecode As Boolean) As String
            If HtmlDecode Then
                Return System.Web.HttpUtility.HtmlDecode(GetStoreSettingText(Portalid, SettingName, Lang, NotCached))
            Else
                Return GetStoreSettingText(Portalid, SettingName, Lang, NotCached)
            End If
        End Function

        Public Shared Function GetStoreSettingText(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As String
            SettingName = getSettingTextName(Portalid, SettingName, UserInfo)
            Return GetStoreSettingText(Portalid, SettingName, Lang, False)
        End Function

        Public Shared Function GetStoreSettingText(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String) As String
            Return GetStoreSettingText(Portalid, SettingName, Lang, False)
        End Function

        Public Shared Function GetStoreSettingText(ByVal Portalid As Integer, ByVal SettingName As String, ByVal Lang As String, ByVal NotCached As Boolean) As String
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsTextInfo
            If NotCached Then
                objSInfo = objSCtrl.GetSettingsTextNotCached(Portalid, SettingName, Lang)
            Else
                objSInfo = objSCtrl.GetSettingsText(Portalid, SettingName, Lang)
            End If
            If objSInfo Is Nothing Then
                Return ""
            Else
                Return objSInfo.SettingValue
            End If
        End Function

        Public Shared Sub SetStoreSetting(ByVal Portalid As Integer, ByVal SettingName As String, ByVal SettingValue As String, ByVal Lang As String, ByVal IsHostOnly As Boolean)
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            objSInfo = objSCtrl.GetSetting(Portalid, SettingName, Lang)
            If objSInfo Is Nothing Then
                objSInfo = New NB_Store_SettingsInfo
                objSInfo.SettingName = SettingName
                objSInfo.SettingValue = SettingValue
                objSInfo.Lang = Lang
                objSInfo.HostOnly = IsHostOnly
                objSInfo.PortalID = Portalid
                If SettingName.EndsWith(".email") Then
                    objSInfo.GroupRef = "root/notifications"
                ElseIf SettingName.EndsWith(".emailsubject") Then
                    objSInfo.GroupRef = "root/notifications"
                ElseIf SettingName.EndsWith(".plugin") Then
                    objSInfo.GroupRef = "root/system/backoffice"
                Else
                    objSInfo.GroupRef = "root/system/misc"
                End If
            Else
                objSInfo.HostOnly = IsHostOnly
                objSInfo.SettingValue = SettingValue
            End If
            objSCtrl.UpdateObjSetting(objSInfo)
        End Sub

        Public Shared Sub LocalizeDDL(ByVal ddl As DropDownList, ByVal NameList As String)
            Dim aryStr As String()
            Dim lp As Integer = 0
            aryStr = NameList.Split(","c)
            If aryStr.GetUpperBound(0) < ddl.Items.Count Then
                For lp = 0 To aryStr.GetUpperBound(0)
                    ddl.Items(lp).Text = aryStr(lp)
                Next
            End If
        End Sub

        Public Shared Function ParseGateway(ByVal GatewayParams As String) As Hashtable
            Dim tableOUT As New Hashtable

            Try
                'Is XML format 
                Dim xmlDoc As New Xml.XmlDataDocument
                Dim xmlNList As Xml.XmlNodeList
                Dim xmlNod As Xml.XmlNode

                xmlDoc.LoadXml(Trim(GatewayParams))

                xmlNList = xmlDoc.SelectNodes("/root/*")

                For Each xmlNod In xmlNList
                    tableOUT.Add(xmlNod.Name, xmlNod.InnerXml)
                Next


            Catch ex As Exception

                'Is old Text Format 
                Dim lp As Integer
                Dim lp2 As Integer
                Dim SetParams() As String
                Dim SetLine() As String
                Dim strValue As String = ""

                SetParams = Split(GatewayParams, vbCrLf)

                For lp = 0 To SetParams.GetUpperBound(0)
                    SetLine = Split(SetParams(lp), "=")
                    If SetLine.GetUpperBound(0) >= 1 Then
                        strValue = ""
                        For lp2 = 1 To SetLine.GetUpperBound(0)
                            If lp2 = 1 Then
                                strValue &= SetLine(lp2)
                            Else
                                strValue &= "=" & SetLine(lp2)
                            End If
                        Next
                        tableOUT.Add(SetLine(0), strValue)
                    End If
                Next


            End Try

            Return tableOUT


        End Function

        Public Shared Sub DoModuleImport(ByVal ModuleID As Integer, ByVal ImpFileName As String, ByVal usrID As Integer)

            If ImpFileName <> "" Then
                Dim xmlDoc As New XmlDataDocument
                Dim objImpCtrl As New BasePortController
                Dim objManagerMenuCtrl As New ManagerMenuController

                xmlDoc.Load(ImpFileName)
                If Not xmlDoc.SelectSingleNode("content/modulecontent") Is Nothing Then
                    objImpCtrl.ImportModule(ModuleID, xmlDoc.SelectSingleNode("content/modulecontent").OuterXml, "1", usrID)
                End If
                If Not xmlDoc.SelectSingleNode("content/storecontent") Is Nothing Then
                    objManagerMenuCtrl.ImportModule(ModuleID, xmlDoc.SelectSingleNode("content/storecontent").OuterXml, "1", usrID)
                End If
            End If
        End Sub

        Public Shared Function populateCategoryDualList(ByVal PortalId As Integer, ByVal EntryID As Integer, ByVal dlCategories As DotNetNuke.UI.UserControls.DualListControl) As Integer
            Dim arrAssigned As New ArrayList
            Dim arrUnAssigned As New ArrayList
            Dim arrCats As ArrayList
            Dim arrCatsAssigned As ArrayList
            Dim objECInfo As NB_Store_ProductCategoryInfo
            Dim objCCtrl As New CategoryController
            Dim objCtrl As New ProductController
            Dim strCats As String = ""


            arrCats = objCCtrl.GetCategories(PortalId, GetCurrentCulture)
            If arrCats.Count = 0 Then
                Return False
            Else
                arrCatsAssigned = objCtrl.GetCategoriesAssigned(EntryID)

                For Each objECInfo In arrCatsAssigned
                    strCats &= "*" & objECInfo.CategoryID & ";"
                Next

                Dim aryRtn As New ArrayList
                BuildCategoryList(arrCats, 0, "", aryRtn)

                Dim li As ListItem
                For Each li In aryRtn
                    If strCats.IndexOf("*" & li.Value & ";") <> -1 Then
                        arrAssigned.Add(li)
                    Else
                        arrUnAssigned.Add(li)
                    End If
                Next

                dlCategories.Assigned = arrAssigned
                dlCategories.Available = arrUnAssigned

                Return (arrAssigned.Count + arrUnAssigned.Count)
            End If

        End Function

        Public Shared Sub BuildCategoryList(ByVal aryList As ArrayList, ByVal ParentID As Integer, ByVal Prefix As String, ByVal aryRtn As ArrayList)
            Dim objCInfo As NB_Store_CategoriesInfo
            Dim li As ListItem
            Dim cName As String = ""

            For Each objCInfo In aryList
                If objCInfo.ParentCategoryID = ParentID Then
                    li = New ListItem
                    li.Value = objCInfo.CategoryID.ToString
                    li.Text = Prefix & objCInfo.CategoryName
                    aryRtn.Add(li)
                    If objCInfo.CategoryName.Length > 20 Then
                        cName = objCInfo.CategoryName.Substring(0, 18) & ".."
                    Else
                        cName = objCInfo.CategoryName
                    End If
                    BuildCategoryList(aryList, objCInfo.CategoryID, Prefix & cName & ">", aryRtn)
                End If
            Next
        End Sub

        Public Shared Function assignByReflection(ByVal obj As Object, ByVal TagXMLString As String) As Object
            Return assignByReflection(obj, getTokenPropXMLnode(TagXMLString))
        End Function

        Public Shared Function getTokenPropXMLnode(ByVal TagXMLString As String) As XmlNode
            Dim xmlDoc As New XmlDataDocument
            Dim xmlNod As XmlNode
            Dim strXML As String

            strXML = System.Web.HttpUtility.HtmlDecode(TagXMLString)
            strXML = "<root>" & strXML & "</root>"

            xmlDoc.LoadXml(strXML)
            xmlNod = xmlDoc.SelectSingleNode("root/prop")
            Return xmlNod
        End Function

        Public Shared Function assignByReflection(ByVal obj As Object, ByVal xmlNod As XmlNode) As Object
            Dim xmlAtt As XmlAttribute

            For Each xmlAtt In xmlNod.Attributes
                obj = assignByReflection(obj, xmlAtt.Name, xmlAtt.InnerText)
            Next

            Return obj
        End Function

        Public Shared Function assignByReflection(ByVal obj As Object, ByVal PropName As String, ByVal PropValue As String) As Object
            Try
                Dim typ As Type = obj.[GetType]()
                Dim prop As PropertyInfo = typ.GetProperty(PropName, BindingFlags.IgnoreCase + BindingFlags.Public + BindingFlags.Instance)
                If Not prop Is Nothing Then
                    Select Case prop.PropertyType.Name
                        Case "Int32", "Integer", "Int16", "Int64"
                            prop.SetValue(obj, CInt(PropValue), Nothing)
                        Case "Boolean"
                            prop.SetValue(obj, CBool(PropValue), Nothing)
                        Case "String"
                            prop.SetValue(obj, PropValue, Nothing)
                        Case "Unit"
                            prop.SetValue(obj, System.Web.UI.WebControls.Unit.Parse(PropValue), Nothing)
                        Case Else
                            If [Enum].IsDefined(prop.PropertyType, PropValue) Then
                                prop.SetValue(obj, [Enum].Parse(prop.PropertyType, PropValue, True), Nothing)
                            Else
                                If IsNumeric(PropValue) Then
                                    prop.SetValue(obj, CInt(PropValue), Nothing)
                                Else
                                    prop.SetValue(obj, PropValue, Nothing)
                                End If
                            End If
                    End Select
                End If
            Catch ex As Exception
                'don't do anything, failure expected in some cases
            End Try
            Return obj
        End Function

        Public Shared Function DocHasBeenPurchasedByDocID(ByVal UserID As Integer, ByVal DocID As Integer) As Boolean
            Dim objCtrl As New ProductController
            Dim objDoc As NB_Store_ProductDocInfo
            objDoc = objCtrl.GetProductDoc(DocID, GetCurrentCulture)
            Return DocHasBeenPurchased(UserID, objDoc.ProductID)
        End Function

        Public Shared Function DocHasBeenPurchased(ByVal UserID As Integer, ByVal ProductID As Integer) As Boolean
            Dim rtnValue As Boolean = False
            If UserID >= 0 Then
                Dim objCtrl As New ProductController

                If objCtrl.CheckIfProductPurchased(ProductID, UserID) > 0 Then
                    Return True
                Else
                    Return False
                End If
            End If
            Return rtnValue
        End Function

        Public Shared Function GetAvailableModelList(ByVal PortalID As Integer, ByVal ProductID As Integer, ByVal Lang As String, ByVal IsDealer As Boolean) As ArrayList
            Dim objCtrl As New ProductController
            Dim aryStock As ArrayList

            If GetStoreSettingBoolean(PortalID, "outofstockmodels.flag") Then
                aryStock = objCtrl.GetModelList(PortalID, ProductID, Lang, IsDealer)
                'remove any deleted models
                For lp As Integer = (aryStock.Count - 1) To 0 Step -1
                    If CType(aryStock.Item(lp), NB_Store_ModelInfo).Deleted Then
                        aryStock.RemoveAt(lp)
                    End If
                Next
            Else
                aryStock = objCtrl.GetModelInStockList(ProductID, Lang, IsDealer)
                'adjust to allow for stock in cart
                aryStock = CurrentCart.AdjustCartStockInModels(PortalID, aryStock)
            End If

            Return aryStock
        End Function

        Public Shared Sub DisplayXMLData(ByVal objInfo As Object, ByVal dlXMLData As DataList)
            Dim aryList As New ArrayList

            aryList.Add(objInfo)

            dlXMLData.DataSource = aryList
            dlXMLData.DataBind()

        End Sub

        Public Shared Function FormatToSave(ByVal inpData As String, Optional ByVal DataTyp As System.TypeCode = TypeCode.String, Optional ByVal SaveLang As String = "en-GB") As String
            'always save in en-GB format to the XML
            If inpData = "" Then Return inpData
            Dim SaveCulture As New CultureInfo(SaveLang, False)
            Select Case DataTyp
                Case TypeCode.Double
                    Dim num As Double
                    If IsNumeric(inpData) Then
                        num = CDbl(inpData)
                        Return num.ToString("N", SaveCulture)
                    Else
                        Return 0
                    End If
                Case TypeCode.DateTime
                    Dim dte As DateTime
                    If IsDate(dte) Then
                        dte = CDate(inpData)
                        Return dte.ToString("s")
                    Else
                        Return ""
                    End If
                Case Else
                    Return inpData
            End Select
        End Function

        Public Shared Function FormatFromSave(ByVal inpData As String, Optional ByVal DataTyp As System.TypeCode = TypeCode.String, Optional ByVal SaveLang As String = "en-GB") As String
            'always saved in en-GB format to the XML
            If inpData = "" Then Return inpData
            Dim SaveCulture As New CultureInfo("en-GB", False)
            Select Case DataTyp
                Case TypeCode.Double
                    Return Double.Parse(inpData, SaveCulture).ToString
                Case TypeCode.DateTime
                    Return Date.Parse(inpData).ToString("d")
                Case Else
                    Return inpData
            End Select
        End Function


        Public Shared Function getGenXML(ByVal dlGenXML As DataList, Optional ByVal RowIndex As Integer = 0, Optional ByVal Lang As String = "en-GB") As String
            Return getGenXML(dlGenXML, "", "", RowIndex, Lang)
        End Function

        Public Shared Function getGenXML(ByVal dlGenXML As DataList, ByVal OriginalXML As String, Optional ByVal FolderMapPath As String = "", Optional ByVal RowIndex As Integer = 0, Optional ByVal Lang As String = "en-GB") As String
            Dim Ctrl As Control
            Dim ddlCtrls As New Collection
            Dim rblCtrls As New Collection
            Dim chkCtrls As New Collection
            Dim txtCtrls As New Collection
            Dim hidCtrls As New Collection
            Dim fupCtrls As New Collection
            Dim chkboxCtrls As New Collection
            Dim teCtrls As New Collection
            Dim intQty As Integer = 1
            Dim DLItem As DataListItem

            'check row exists (0 based)
            If dlGenXML.Items.Count <= RowIndex Then
                Return ""
            End If

            'only do if entry already created
            If dlGenXML.Items.Count >= 1 Then

                DLItem = dlGenXML.Items(RowIndex)

                'build list of controls
                For Each Ctrl In DLItem.Controls
                    If TypeOf Ctrl Is DropDownList Then
                        ddlCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is CheckBoxList Then
                        chkCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is CheckBox Then
                        chkboxCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is TextBox Or TypeOf Ctrl Is DateEditControl Then
                        If TypeOf Ctrl Is DateEditControl Then
                            txtCtrls.Add(DirectCast(Ctrl, DateEditControl).dateField)
                        Else
                            txtCtrls.Add(Ctrl)
                        End If
                    End If
                    If TypeOf Ctrl Is RadioButtonList Then
                        rblCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is GenTextEditor Then
                        teCtrls.Add(Ctrl)
                    End If
                Next

                'load original XML for update  
                Dim xmlDoc As New XmlDataDocument
                If OriginalXML <> "" Then
                    xmlDoc.LoadXml(OriginalXML)
                End If

                'Create XML
                Dim strXML As String = "<genxml>"

                Dim txtCtrl As TextBox
                strXML &= "<textbox>"
                For Each txtCtrl In txtCtrls
                    If OriginalXML <> "" Then
                        ReplaceXMLNode(xmlDoc, "genxml/textbox/" & txtCtrl.ID.ToLower, FormatToSave(txtCtrl.Text))
                    Else
                        Dim DataTyp As String = ""
                        If Not txtCtrl.Attributes("datatype") Is Nothing Then
                            DataTyp = txtCtrl.Attributes("datatype")
                        End If
                        If DataTyp.ToLower = "double" Then
                            strXML &= "<" & txtCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """><![CDATA["
                            strXML &= FormatToSave(txtCtrl.Text, TypeCode.Double)
                        ElseIf DataTyp.ToLower = "date" Then
                            strXML &= "<" & txtCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """><![CDATA["
                            strXML &= FormatToSave(txtCtrl.Text, TypeCode.DateTime)
                        Else
                            strXML &= "<" & txtCtrl.ID.ToLower & "><![CDATA["
                            strXML &= txtCtrl.Text
                        End If
                        strXML &= "]]></" & txtCtrl.ID.ToLower & ">"
                    End If
                Next
                strXML &= "</textbox>"

                Dim chkboxCtrl As CheckBox
                strXML &= "<checkbox>"
                For Each chkboxCtrl In chkboxCtrls
                    If OriginalXML <> "" Then
                        ReplaceXMLNode(xmlDoc, "genxml/checkbox/" & chkboxCtrl.ID.ToLower, chkboxCtrl.Checked.ToString)
                    Else
                        strXML &= "<" & chkboxCtrl.ID.ToLower & "><![CDATA["
                        strXML &= chkboxCtrl.Checked.ToString
                        strXML &= "]]></" & chkboxCtrl.ID.ToLower & ">"
                    End If
                Next
                strXML &= "</checkbox>"

                Dim ddlCtrl As DropDownList
                strXML &= "<dropdownlist>"
                For Each ddlCtrl In ddlCtrls
                    If OriginalXML <> "" Then
                        ReplaceXMLNode(xmlDoc, "genxml/dropdownlist/" & ddlCtrl.ID.ToLower, FormatToSave(ddlCtrl.SelectedValue))
                    Else
                        Dim DataTyp As String = ""
                        If Not ddlCtrl.Attributes("datatype") Is Nothing Then
                            DataTyp = ddlCtrl.Attributes("datatype")
                        End If
                        If DataTyp.ToLower = "double" Then
                            strXML &= "<" & ddlCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """><![CDATA["
                            strXML &= FormatToSave(ddlCtrl.SelectedValue, TypeCode.Double)
                        ElseIf DataTyp.ToLower = "date" Then
                            strXML &= "<" & ddlCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """><![CDATA["
                            strXML &= FormatToSave(ddlCtrl.SelectedValue, TypeCode.DateTime)
                        Else
                            strXML &= "<" & ddlCtrl.ID.ToLower & "><![CDATA["
                            strXML &= ddlCtrl.SelectedValue
                        End If
                        strXML &= "]]></" & ddlCtrl.ID.ToLower & ">"
                    End If
                Next
                strXML &= "</dropdownlist>"

                Dim chkCtrl As CheckBoxList
                strXML &= "<checkboxlist>"
                Dim LItem As ListItem
                Dim lp As Integer = 0
                For Each chkCtrl In chkCtrls

                    Dim DataTyp As String = ""
                    If Not chkCtrl.Attributes("datatype") Is Nothing Then
                        DataTyp = chkCtrl.Attributes("datatype")
                    End If
                    If DataTyp.ToLower = "double" Then
                        strXML &= "<" & chkCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """>"
                    ElseIf DataTyp.ToLower = "date" Then
                        strXML &= "<" & chkCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """>"
                    Else
                        strXML &= "<" & chkCtrl.ID.ToLower & ">"
                    End If
                    For Each LItem In chkCtrl.Items
                        If OriginalXML <> "" Then
                            ReplaceXMLatt(xmlDoc, "genxml/checkboxlist/" & chkCtrl.ID.ToLower & "/chk[.='" & FormatToSave(LItem.Text) & "']", LItem.Selected)
                        Else
                            strXML &= "<chk value=""" & LItem.Selected & """>"
                            If DataTyp.ToLower = "double" Then
                                strXML &= "<![CDATA[" & FormatToSave(LItem.Text, TypeCode.Double) & "]]>"
                            ElseIf DataTyp.ToLower = "date" Then
                                strXML &= "<![CDATA[" & FormatToSave(LItem.Text, TypeCode.DateTime) & "]]>"
                            Else
                                strXML &= "<![CDATA[" & LItem.Text & "]]>"
                            End If
                            strXML &= "</chk>"
                        End If
                    Next
                    strXML &= "</" & chkCtrl.ID.ToLower & ">"
                Next
                strXML &= "</checkboxlist>"

                Dim rblCtrl As RadioButtonList
                strXML &= "<radiobuttonlist>"
                For Each rblCtrl In rblCtrls
                    If OriginalXML <> "" Then
                        ReplaceXMLNode(xmlDoc, "genxml/radiobuttonlist/" & rblCtrl.ID.ToLower, FormatToSave(rblCtrl.SelectedValue))
                    Else
                        Dim DataTyp As String = ""
                        If Not rblCtrl.Attributes("datatype") Is Nothing Then
                            DataTyp = rblCtrl.Attributes("datatype")
                        End If
                        If DataTyp.ToLower = "double" Then
                            strXML &= "<" & rblCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """><![CDATA["
                            strXML &= FormatToSave(rblCtrl.SelectedValue, TypeCode.Double)
                        ElseIf DataTyp.ToLower = "date" Then
                            strXML &= "<" & rblCtrl.ID.ToLower & " datatype=""" & DataTyp.ToLower & """><![CDATA["
                            strXML &= FormatToSave(rblCtrl.SelectedValue, TypeCode.DateTime)
                        Else
                            strXML &= "<" & rblCtrl.ID.ToLower & "><![CDATA["
                            strXML &= rblCtrl.SelectedValue
                        End If
                        strXML &= "]]></" & rblCtrl.ID.ToLower & ">"
                    End If
                Next
                strXML &= "</radiobuttonlist>"

                Dim gteCtrl As GenTextEditor
                Dim teCtrl As DotNetNuke.UI.UserControls.TextEditor
                strXML &= "<edt>"
                For Each gteCtrl In teCtrls
                    teCtrl = DirectCast(gteCtrl.Controls(0), DotNetNuke.UI.UserControls.TextEditor)
                    If OriginalXML <> "" Then
                        ReplaceXMLNode(xmlDoc, "genxml/edt/" & teCtrl.ID.ToLower, teCtrl.Text)
                    Else
                        strXML &= "<" & teCtrl.ID.ToLower & "><![CDATA["
                        Try
                            strXML &= teCtrl.Text
                        Catch ex As Exception
                            'do nothing, fails if updating from datalist created in-line code.
                        End Try
                        strXML &= "]]></" & teCtrl.ID.ToLower & ">"
                    End If
                Next
                strXML &= "</edt>"


                strXML &= "</genxml>"

                If OriginalXML <> "" Then
                    strXML = xmlDoc.OuterXml
                End If

                Return strXML
            Else
                Return ""
            End If


        End Function

        Public Shared Sub ReplaceXMLNode(ByVal xmlDoc As XmlDataDocument, ByVal XPath As String, ByVal NewValue As String, Optional ByVal cdata As Boolean = True)
            'CtrlType: textbox,checkbox,dropdownlist
            Dim nod As XmlNode
            nod = xmlDoc.SelectSingleNode(XPath)
            If Not nod Is Nothing Then
                If cdata Then
                    nod.InnerXml = "<![CDATA[" & NewValue & "]]>"
                Else
                    nod.InnerXml = NewValue
                End If
            End If
        End Sub


        Public Shared Sub ReplaceXMLatt(ByVal xmlDoc As XmlDataDocument, ByVal XPath As String, ByVal NewValue As String)
            Dim nod As XmlNode
            nod = xmlDoc.SelectSingleNode(XPath)
            If Not nod Is Nothing Then
                nod.Attributes("value").InnerText = NewValue
            End If
        End Sub

        Public Shared Function getGenXMLvalue(ByVal DataXML As String, ByVal XPath As String) As String
            Dim xmlNod As XmlNode
            xmlNod = getGenXMLnode(DataXML, XPath)
            If xmlNod Is Nothing Then
                Return ""
            Else
                If Not xmlNod.Attributes("datatype") Is Nothing Then
                    Select Case xmlNod.Attributes("datatype").InnerText.ToLower
                        Case "double"
                            Return FormatFromSave(xmlNod.InnerText, TypeCode.Double)
                        Case "date"
                            Return FormatFromSave(xmlNod.InnerText, TypeCode.DateTime)
                        Case "html"
                            Return xmlNod.InnerXml
                        Case Else
                            Return xmlNod.InnerText
                    End Select
                Else
                    Return xmlNod.InnerText
                End If
            End If

        End Function

        Public Shared Function getGenXMLnode(ByVal ctrlID As String, ByVal CtrlType As String, ByVal DataXML As String) As XmlNode
            If ctrlID Is Nothing Or CtrlType Is Nothing Or DataXML Is Nothing Then
                Return Nothing
            Else
                Return getGenXMLnode(DataXML, "genxml/" & CtrlType & "/" & ctrlID.ToLower)
            End If
        End Function

        Public Shared Function getGenXMLnode(ByVal DataXML As String, ByVal XPath As String) As XmlNode
            Dim strXML As String = ""
            Dim xmlNod As XmlNode
            Dim xmlDoc As New XmlDocument

            Try
                xmlDoc.LoadXml(DataXML)
                xmlNod = xmlDoc.SelectSingleNode(XPath.ToLower)
                Return xmlNod
            Catch ex As Exception
                Return Nothing
            End Try

        End Function

        Public Shared Function getGridViewValueDbl(ByVal row As GridViewRow, ByVal CtrlID As String) As Double
            Dim getValue As String = ""
            getValue = getGridViewValue(row, CtrlID)
            If IsNumeric(getValue) Then
                Return CDbl(getValue)
            Else
                Return 0
            End If
        End Function


        Public Shared Function getGridViewValueInt(ByVal row As GridViewRow, ByVal CtrlID As String) As Integer
            Dim getValue As String = ""
            getValue = getGridViewValue(row, CtrlID)
            If IsNumeric(getValue) Then
                Return CInt(getValue)
            Else
                Return 0
            End If
        End Function

        Public Shared Function getGridViewValueDate(ByVal row As GridViewRow, ByVal CtrlID As String) As Date
            Dim getValue As String = ""
            getValue = getGridViewValue(row, CtrlID)
            If IsDate(getValue) Then
                Return CDate(getValue)
            Else
                Return Null.NullDate
            End If
        End Function

        Public Shared Function getGridViewValueBool(ByVal row As GridViewRow, ByVal CtrlID As String) As Boolean
            Dim getValue As String = "False"
            getValue = getGridViewValue(row, CtrlID)
            Return CBool(getValue)
        End Function

        Public Shared Function getGridViewValue(ByVal row As GridViewRow, ByVal CtrlID As String) As String
            Dim rtnValue As String = ""
            Dim ctrl As Control

            ctrl = row.FindControl(CtrlID)

            If TypeOf ctrl Is TextBox Then
                rtnValue = DirectCast(ctrl, TextBox).Text
            ElseIf TypeOf ctrl Is CheckBox Then
                rtnValue = DirectCast(ctrl, CheckBox).Checked.ToString
            ElseIf TypeOf ctrl Is DropDownList Then
                rtnValue = DirectCast(ctrl, DropDownList).SelectedValue
            ElseIf TypeOf ctrl Is HiddenField Then
                rtnValue = DirectCast(ctrl, HiddenField).Value
            End If

            Return rtnValue
        End Function

        Public Shared Sub setGridViewEnabled(ByVal row As GridViewRow, ByVal CtrlIdAsCSVlist As String, ByVal blnVisible As Boolean)
            Dim rtnValue As String = ""
            Dim ctrl As Control
            Dim strList As String()

            strList = CtrlIdAsCSVlist.Split(","c)
            For lp As Integer = 0 To strList.GetUpperBound(0)
                ctrl = row.FindControl(strList(lp))

                If Not ctrl Is Nothing Then
                    If TypeOf ctrl Is TextBox Then
                        DirectCast(ctrl, TextBox).Enabled = blnVisible
                    ElseIf TypeOf ctrl Is CheckBox Then
                        DirectCast(ctrl, CheckBox).Enabled = blnVisible
                    ElseIf TypeOf ctrl Is DropDownList Then
                        DirectCast(ctrl, DropDownList).Enabled = blnVisible
                    ElseIf TypeOf ctrl Is RadioButton Then
                        DirectCast(ctrl, RadioButton).Enabled = blnVisible
                    ElseIf TypeOf ctrl Is RadioButtonList Then
                        DirectCast(ctrl, RadioButtonList).Enabled = blnVisible
                    End If
                End If
            Next

        End Sub

        Public Shared Sub setGridViewVisible(ByVal row As GridViewRow, ByVal CtrlIdAsCSVlist As String, ByVal blnVisible As Boolean)
            Dim rtnValue As String = ""
            Dim ctrl As Control
            Dim strList As String()

            strList = CtrlIdAsCSVlist.Split(","c)
            For lp As Integer = 0 To strList.GetUpperBound(0)
                ctrl = row.FindControl(strList(lp))
                If Not ctrl Is Nothing Then
                    ctrl.Visible = blnVisible
                End If
            Next

        End Sub


#Region "Javascript methods"



        Public Shared Sub AddCSSLink(ByVal LinkFile As String, ByVal Page As System.Web.UI.Page)
            Dim oLink As New HtmlLink
            If Not LinkFile.EndsWith("/") Then
                oLink.Attributes("rel") = "stylesheet"
                oLink.Attributes("media") = "screen"
                oLink.Attributes("type") = "text/css"
                oLink.Attributes("href") = LinkFile
                Dim oCSS As Control = Page.FindControl("CSS")
                If Not oCSS Is Nothing Then
                    oCSS.Controls.Add(oLink)
                End If
            End If
        End Sub

        Public Shared Sub IncludeScripts(ByVal PortalID As Integer, ByVal StoreInstallPath As String, ByVal Page As System.Web.UI.Page, ByVal JSIncludeKey As String, ByVal JSStartUpIncludeKey As String, ByVal CSSIncludeKey As String)
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim DNNJQuery As String = ""
            Dim major As Integer, minor As Integer, build As Integer, revision As Integer
            Dim injectLib As Boolean = False
            If SafeDNNVersion(major, minor, revision, build) Then
                Select Case major
                    Case 5
                        'todo: all versions of 5?
                        injectLib = False
                        Exit Select
                    Case Else
                        injectLib = True
                        Exit Select
                End Select
            Else
                injectLib = True
            End If

            DNNJQuery = GetStoreSetting(PortalID, "usednnjquery.flag", "None")
            If DNNJQuery = "" Then DNNJQuery = "0"

            If injectLib Then
                'DNN 4 so load NB_Store JQuery
                If Not Page.ClientScript.IsClientScriptIncludeRegistered("jquery") Then
                    If Not Page.ClientScript.IsStartupScriptRegistered("jquery") Then
                        If CBool(DNNJQuery) Then
                            Page.ClientScript.RegisterClientScriptInclude("jquery", "/Resources/Shared/scripts/jquery/jquery.min.js")
                        Else
                            Page.ClientScript.RegisterClientScriptInclude("jquery", StoreInstallPath & "js/jquery.js")
                        End If
                        Page.ClientScript.RegisterClientScriptBlock(GetType(String), "jQueryFix", "var j = jQuery.noConflict();", True)
                    End If
                End If
            Else
                'DNN 5 use DNN version
                Try
                    Dim objHandle As Runtime.Remoting.ObjectHandle = Activator.CreateInstance("DotNetNuke", "DotNetNuke.Framework.jQuery")
                    Dim obj As Object = objHandle.Unwrap

                    obj.RequestRegistration()
                Catch ex As Exception
                    LogException(ex) 'something went wrong, lets log the error...
                End Try
            End If

            objSInfo = objSCtrl.GetSetting(PortalID, JSIncludeKey, GetCurrentCulture)
            If Not objSInfo Is Nothing Then
                'custom js includes
                If objSInfo.SettingValue.Trim <> "" Then
                    Dim jsList As String() = objSInfo.SettingValue.Split(","c)

                    For lp As Integer = 0 To jsList.GetUpperBound(0)
                        RegisterJS(jsList(lp), jsList(lp), StoreInstallPath, Page)
                    Next
                End If
            End If

            objSInfo = objSCtrl.GetSetting(PortalID, JSStartUpIncludeKey, GetCurrentCulture)
            If Not objSInfo Is Nothing Then
                'custom js includes
                If objSInfo.SettingValue.Trim <> "" Then
                    Dim jsList As String() = objSInfo.SettingValue.Split(","c)

                    For lp As Integer = 0 To jsList.GetUpperBound(0)
                        LoadJQueryCode(PortalID, jsList(lp), Page)
                    Next
                End If
            End If

            objSInfo = objSCtrl.GetSetting(PortalID, CSSIncludeKey, GetCurrentCulture)
            If Not objSInfo Is Nothing Then
                If objSInfo.SettingValue.Trim <> "" Then
                    'custom css
                    Dim cssList As String() = objSInfo.SettingValue.Split(","c)

                    For lp As Integer = 0 To cssList.GetUpperBound(0)
                        If cssList(lp).StartsWith("/") Then
                            AddCSSLink(cssList(lp), Page)
                        Else
                            AddCSSLink(StoreInstallPath & "js/" & cssList(lp), Page)
                        End If
                    Next
                End If
            End If
        End Sub

        Public Shared Sub RegisterJS(ByVal RegName As String, ByVal JSFileName As String, ByVal StoreInstallPath As String, ByVal Page As System.Web.UI.Page)
            If Not Page.ClientScript.IsClientScriptIncludeRegistered(RegName) Then
                If JSFileName.StartsWith("/") Then
                    Page.ClientScript.RegisterClientScriptInclude(RegName, JSFileName)
                Else
                    Page.ClientScript.RegisterClientScriptInclude(RegName, StoreInstallPath & "js/" & JSFileName)
                End If
            End If
        End Sub

        Public Shared Sub LoadJQueryCode(ByVal PortalID As Integer, ByVal SettingsKey As String, ByVal Page As System.Web.UI.Page)
            Dim objSTCtrl As New SettingsController
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim jCode As String = ""

            objSTInfo = objSTCtrl.GetSettingsText(PortalID, SettingsKey, GetCurrentCulture)
            If Not objSTInfo Is Nothing Then
                If objSTInfo.SettingText.ToLower.StartsWith("jquery") Then
                    jCode = "<script language=""javascript"" type=""text/javascript"">" & System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText) & "</script>"
                Else
                    jCode = System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText)
                End If

                Page.ClientScript.RegisterStartupScript(Page.GetType, SettingsKey, jCode)
            End If

        End Sub

        Public Shared Function SafeDNNVersion(ByRef major As Integer, ByRef minor As Integer, ByRef revision As Integer, ByRef build As Integer) As Boolean
            'Based on code by Bruce Chapman (ifinity)
            Dim ver As System.Version = System.Reflection.Assembly.GetAssembly(GetType(DotNetNuke.Common.Globals)).GetName().Version
            If ver IsNot Nothing Then
                major = ver.Major
                minor = ver.Minor
                build = ver.Build
                revision = ver.Revision
                Return True
            Else
                major = 0
                minor = 0
                build = 0
                revision = 0
                Return False
            End If
        End Function

        Public Shared Function IsDNN4() As Boolean
            Dim major As Integer, minor As Integer, build As Integer, revision As Integer
            Dim rtnFlag As Boolean = False
            If SafeDNNVersion(major, minor, revision, build) Then
                If major = 4 Then
                    rtnFlag = True
                End If
            End If
            Return rtnFlag
        End Function


#End Region


        Public Shared Function FormatToStoreCurrency(ByVal Value As Double) As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Return FormatToStoreCurrency(PS.PortalId, Value)
        End Function

        Public Shared Function FormatToStoreCurrency(ByVal PortalID As Integer, ByVal Value As Double) As String
            Dim CurrencyCultureCode As String = GetStoreSetting(PortalID, "currency.culture", GetCurrentCulture())
            If CurrencyCultureCode = "" Then CurrencyCultureCode = GetMerchantCulture(PortalID)
            Dim rtnValue As String = ""
            If CurrencyCultureCode.StartsWith("""") Then
                rtnValue = Value.ToString(Replace(CurrencyCultureCode, """", ""))
            Else
                Try
                    rtnValue = Value.ToString("c", New Globalization.CultureInfo(CurrencyCultureCode, False))
                Catch ex As Exception
                    rtnValue = Value.ToString
                End Try
            End If
            Return rtnValue
        End Function

        Public Shared Function RoundToStoreCurrency(ByVal Value As Double) As Decimal
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Return RoundToStoreCurrency(PS.PortalId, Value)
        End Function

        Public Shared Function RoundToStoreCurrency(ByVal PortalID As Integer, ByVal Value As Double) As Decimal
            Return CurrencyStringToDecimal(PortalID, FormatToStoreCurrency(PortalID, Value))
        End Function

        Public Shared Function CurrencyStringToDecimal(ByVal PortalID As Integer, ByVal Value As String) As Decimal
            If Value.Length = 0 Then
                Return 0
            Else
                Dim CurrencyCultureCode As String = GetStoreSetting(PortalID, "currency.culture", GetCurrentCulture())
                If CurrencyCultureCode.StartsWith("""") Then
                    Return CType(Value, Double).ToString(Replace(CurrencyCultureCode, """", ""))
                Else
                    If CurrencyCultureCode = "" Then CurrencyCultureCode = GetMerchantCulture(PortalID)
                    Dim g As New Globalization.CultureInfo(CurrencyCultureCode, False)
                    Return Decimal.Parse(Value, NumberStyles.Any, g.NumberFormat)
                End If
            End If
        End Function


        Public Shared Function getCurrencySymbol() As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Return getCurrencySymbol(PS.PortalId)
        End Function

        Public Shared Function getCurrencySymbol(ByVal PortalID As Integer) As String
            Dim CurrencyCultureCode As String = GetStoreSetting(PortalID, "currency.culture", GetCurrentCulture())
            Dim rtnValue As String = ""
            If CurrencyCultureCode = "" Or CurrencyCultureCode.StartsWith("""") Then
                rtnValue = GetStoreSetting(PortalID, "currency.symbol", GetCurrentCulture())
            End If

            If rtnValue = "" Then
                CurrencyCultureCode = GetMerchantCulture(PortalID)

                Try
                    rtnValue = New Globalization.CultureInfo(CurrencyCultureCode, False).NumberFormat.CurrencySymbol
                Catch ex As Exception
                End Try
            End If

            Return rtnValue
        End Function

        Public Shared Function getCurrencyISOCode() As String
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Return getCurrencyISOCode(PS.PortalId)
        End Function

        Public Shared Function getCurrencyISOCode(ByVal PortalID As Integer) As String
            Dim CurrencyCultureCode As String = GetStoreSetting(PortalID, "currency.culture", GetCurrentCulture())
            If CurrencyCultureCode = "" Then CurrencyCultureCode = GetMerchantCulture(PortalID)
            Dim rtnValue As String = ""
            Try
                rtnValue = New Globalization.RegionInfo(CurrencyCultureCode).ISOCurrencySymbol
            Catch ex As Exception
                rtnValue = ""
            End Try

            Return rtnValue
        End Function


        Public Shared Sub removeStoreCookie(ByVal PortalID As Integer, ByVal CookieName As String)
            Dim FullCookieName As String = "NB_Store_" & CookieName & "_Portal" & PortalID.ToString
            Dim FoundCookie As HttpCookie = HttpContext.Current.Request.Cookies(FullCookieName)
            If Not FoundCookie Is Nothing Then
                FoundCookie.Expires = DateTime.Now.AddYears(-30)
                HttpContext.Current.Response.Cookies.Add(FoundCookie)
            End If
        End Sub

        Public Shared Function getStoreCookie(ByVal PortalID As Integer, ByVal CookieName As String) As HttpCookie
            Dim FullCookieName As String = "NB_Store_" & CookieName & "_Portal" & PortalID.ToString
            Dim FoundCookie As HttpCookie = HttpContext.Current.Request.Cookies(FullCookieName)
            If FoundCookie Is Nothing Then
                FoundCookie = New HttpCookie(FullCookieName)
            End If
            Return FoundCookie
        End Function

        Public Shared Function getStoreCookieValue(ByVal objCookie As HttpCookie, ByVal ValueID As String) As String
            If Not (objCookie Is Nothing) Then
                If Not objCookie(ValueID) Is Nothing Then
                    Return objCookie(ValueID)
                Else
                    Return ""
                End If
            Else
                Return ""
            End If
        End Function

        Public Shared Function getAdminCookieValue(ByVal PortalID As Integer, ByVal ValueID As String) As String
            Return getStoreCookieValue(PortalID, "Admin", ValueID)
        End Function

        Public Shared Function getStoreCookieValue(ByVal PortalID As Integer, ByVal CookieName As String, ByVal ValueID As String) As String
            Dim FullCookieName As String = "NB_Store_" & CookieName & "_Portal" & PortalID.ToString
            Dim FoundCookie As HttpCookie = HttpContext.Current.Request.Cookies(FullCookieName)
            Return getStoreCookieValue(FoundCookie, ValueID)
        End Function

        Public Shared Sub setAdminCookieValue(ByVal PortalID As Integer, ByVal ValueID As String, ByVal Value As String)
            setStoreCookieValue(PortalID, "Admin", ValueID, Value, 0)
        End Sub

        Public Shared Sub setStoreCookieValue(ByVal PortalID As Integer, ByVal CookieName As String, ByVal ValueID As String, ByVal Value As String, Optional ByVal ExpireDays As Double = 30)
            Dim FullCookieName As String = "NB_Store_" & CookieName & "_Portal" & PortalID.ToString
            Dim FoundCookie As HttpCookie = HttpContext.Current.Request.Cookies(FullCookieName)
            If FoundCookie Is Nothing Then
                FoundCookie = New HttpCookie(FullCookieName)
            End If
            If Value Is Nothing Then
                FoundCookie(ValueID) = ""
            Else
                FoundCookie(ValueID) = Value
            End If
            If ExpireDays = "0" Then
                FoundCookie.Expires = Nothing
            Else
                FoundCookie.Expires = DateAdd(DateInterval.Day, ExpireDays, Today)
            End If
            HttpContext.Current.Response.Cookies.Add(FoundCookie)

        End Sub

        Public Shared Function GetCacheKey(ByVal KeyID As String, ByVal PortalID As Integer) As String
            Return GetCacheKey(KeyID, PortalID, -1)
        End Function

        Public Shared Function GetCacheKey(ByVal KeyID As String, ByVal PortalID As Integer, ByVal ProductID As Integer) As String
            Dim strCacheKey As String = ""
            strCacheKey = NBSTOREAPPNAME & KeyID & "_" & PortalID.ToString & "_" & ProductID.ToString

            'create list of product cahce keys, so they can be clear on product update
            If ProductID >= 0 Then
                Dim CacheKeyProdList As String = NBSTOREAPPNAME & "ProdKeyList" & "_" & PortalID.ToString
                Dim hCacheKeyProdList As Hashtable
                Dim hPKeys As Hashtable

                hCacheKeyProdList = DataCache.GetCache(CacheKeyProdList)
                If hCacheKeyProdList Is Nothing Then
                    'new
                    hCacheKeyProdList = New Hashtable
                    hPKeys = New Hashtable
                    hPKeys.Add(strCacheKey, strCacheKey)
                    hCacheKeyProdList.Add(ProductID, hPKeys)
                    DataCache.SetCache(CacheKeyProdList, hCacheKeyProdList)
                Else
                    If hCacheKeyProdList.ContainsKey(ProductID) Then
                        hPKeys = DirectCast(hCacheKeyProdList(ProductID), Hashtable)
                        If Not hPKeys.ContainsKey(strCacheKey) Then
                            hPKeys.Add(strCacheKey, strCacheKey)
                            hCacheKeyProdList.Remove(ProductID)
                            hCacheKeyProdList.Add(ProductID, hPKeys)
                            DataCache.SetCache(CacheKeyProdList, hCacheKeyProdList)
                        End If
                    Else
                        hPKeys = New Hashtable
                        hPKeys.Add(strCacheKey, strCacheKey)
                        hCacheKeyProdList.Add(ProductID, hPKeys)
                        DataCache.SetCache(CacheKeyProdList, hCacheKeyProdList)
                    End If
                End If
            End If

            Return strCacheKey
        End Function

        Public Shared Sub ClearProductCache(ByVal PortalID As Integer, ByVal ProductID As Integer)
            Dim CacheKeyProdList As String = NBSTOREAPPNAME & "ProdKeyList" & "_" & PortalID.ToString
            Dim hCacheKeyProdList As Hashtable
            Dim hPKeys As Hashtable
            hCacheKeyProdList = DataCache.GetCache(CacheKeyProdList)
            If Not hCacheKeyProdList Is Nothing Then
                If hCacheKeyProdList.ContainsKey(ProductID) Then
                    hPKeys = DirectCast(hCacheKeyProdList(ProductID), Hashtable)
                    For Each de As DictionaryEntry In hPKeys
                        DataCache.RemoveCache(de.Value)
                    Next
                End If
            End If

        End Sub

        Public Shared Function IsDealer(ByVal PortalID As Integer, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo, ByVal Lang As String) As Boolean
            Dim DealerRole As String = GetStoreSetting(PortalID, "dealer.role", Lang)
            If UserInfo.IsInRole(DealerRole) And DealerRole <> "" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Shared Function VersionCompare(ByVal TestVersion As String, ByVal CompareVersion As String) As Integer
            'test if version is different, return 0 for same version, -1 for older version, +1 for newer version.
            If TestVersion = "" Then TestVersion = "0.0.0" ' must be new installation.
            If CompareVersion = "" Then CompareVersion = "0.0.0"
            Try
                Dim Tver As String()
                Tver = Split(TestVersion, "."c)
                Dim Cver As String()
                Cver = Split(CompareVersion, "."c)
                If Tver.GetUpperBound(0) = 2 And Cver.GetUpperBound(0) = 2 Then
                    If CInt(Tver(0)) = CInt(Cver(0)) And CInt(Tver(1)) = CInt(Cver(1)) And CInt(Tver(2)) = CInt(Cver(2)) Then
                        Return 0
                    ElseIf CInt(Tver(0)) < CInt(Cver(0)) Then
                        Return -1
                    ElseIf CInt(Tver(0)) > CInt(Cver(0)) Then
                        Return 1
                    ElseIf CInt(Tver(1)) < CInt(Cver(1)) Then
                        Return -1
                    ElseIf CInt(Tver(1)) > CInt(Cver(1)) Then
                        Return 1
                    ElseIf CInt(Tver(2)) < CInt(Cver(2)) Then
                        Return -1
                    ElseIf CInt(Tver(2)) > CInt(Cver(2)) Then
                        Return 1
                    Else
                        Return 0
                    End If
                Else
                    'test version strings not valid, return same version.
                    Return 0
                End If
            Catch ex As Exception
                Return 0
            End Try
        End Function

        'copy of DNN function.  Quick way to get functionaility not exposed.
        Public Shared Function GetHostSettingAsBoolean(ByVal key As String, ByVal defaultValue As Boolean) As Boolean
            Dim retValue As Boolean
            Try
                Dim setting As String = DotNetNuke.Entities.Host.HostSettings.GetHostSetting(key)
                If String.IsNullOrEmpty(setting) Then
                    retValue = defaultValue
                Else
                    retValue = (setting.ToUpperInvariant().StartsWith("Y") OrElse setting.ToUpperInvariant = "TRUE")
                End If
            Catch ex As Exception
                'we just want to trap the error as we may not be installed so there will be no Settings
            End Try
            Return retValue
        End Function

        Public Shared Sub ForceStringDownload(ByVal Response As HttpResponse, ByVal FileName As String, ByVal FileData As String)
            Response.AppendHeader("content-disposition", "attachment; filename=" & FileName)
            Response.ContentType = "application/octet-stream"
            Response.Write(FileData)
            Response.End()
        End Sub

        Public Shared Function GetValidLocales() As LocaleCollection
            'TODO: Change this to DNN5 portal localization when we goto DNN5 only
            Dim supportedLanguages As LocaleCollection = DotNetNuke.Services.Localization.Localization.GetEnabledLocales()
            If supportedLanguages.Count = 0 Then
                ' the getenabledlocales doesn;t work correct in DNN5, so use this as a fallback
                supportedLanguages = DotNetNuke.Services.Localization.Localization.GetSupportedLocales()
            End If

            Return supportedLanguages
        End Function

        Public Shared Function EmbeddedImage(ByVal Name As String) As System.Drawing.Bitmap
            Return New System.Drawing.Bitmap(GetExecutingAssembly.GetManifestResourceStream(Name))
        End Function

#Region "Clean up folders and files"

        Public Shared Sub PurgeAllFiles(ByVal PortalSettings As Portals.PortalSettings)
            Dim hList As New Hashtable

            If Not PortalSettings Is Nothing Then
                '------------- Purge Image Folder --------------------
                hList = getImageFileNamesInDB(PortalSettings.PortalId)
                PurgeFiles(PortalSettings, PRODUCTIMAGESFOLDER, hList)

                '------------- Purge Doc Folder --------------------
                hList = getDocsFileNamesInDB(PortalSettings.PortalId)
                PurgeFiles(PortalSettings, PRODUCTDOCSFOLDER, hList)

                '------------- Purge Uploaded Folder --------------------
                hList = getDocsFileNamesInDB(PortalSettings.PortalId)
                PurgeFiles(PortalSettings, ORDERUPLOADFOLDER, hList)

                '------------- Purge Log Folder --------------------
                hList = getDocsFileNamesInDB(PortalSettings.PortalId)
                PurgeLogFiles(PortalSettings)

            End If

        End Sub

        Public Shared Sub PurgeFiles(ByVal PortalSettings As Portals.PortalSettings, ByVal PortalFolderName As String, ByVal FileNamesInDB As Hashtable)
            Dim folderInfo As DotNetNuke.Services.FileSystem.FolderInfo
            Dim fileInfo As DotNetNuke.Services.FileSystem.FileInfo
            Dim aryList As ArrayList
            Dim blnDelete As Boolean = False

            folderInfo = FileSystemUtils.GetFolder(PortalSettings.PortalId, PortalFolderName)
            If Not folderInfo Is Nothing Then
                aryList = FileSystemUtils.GetFilesByFolder(PortalSettings.PortalId, folderInfo.FolderID)
                For Each fileInfo In aryList
                    blnDelete = True

                    If FileNamesInDB.ContainsKey(fileInfo.FileName) Then
                        blnDelete = False
                    End If

                    If blnDelete Then
                        Try
                            FileSystemUtils.DeleteFile(PortalSettings.HomeDirectoryMapPath & PortalFolderName & "\" & fileInfo.FileName, PortalSettings, True)
                        Catch ex As Exception
                            'ignore if locked.
                        End Try
                    End If
                Next
            End If

        End Sub

        Public Shared Sub PurgeLogFiles(ByVal PortalSettings As Portals.PortalSettings)

            If GetStoreSetting(PortalSettings.PortalId, "version", "None") <> "" Then ' portal may not have a store.
                Dim PurgeDate As Date
                Dim PurgeDays As String = GetStoreSetting(PortalSettings.PortalId, "purgelogfiles.days")
                If IsNumeric(PurgeDays) Then
                    PurgeDate = DateAdd(DateInterval.Day, CInt(PurgeDays) * -1, Now)

                    Dim blnDelete As Boolean = False

                    Dim folderInfo As New DirectoryInfo(PortalSettings.HomeDirectoryMapPath & "LogFiles")
                    Dim Files As FileInfo() = folderInfo.GetFiles("*.log")

                    For Each LogF As FileInfo In Files
                        If DateTime.Compare(Today, LogF.LastWriteTime.AddDays(CInt(PurgeDays))) >= 0 Then
                            LogF.Delete()
                        End If
                    Next

                End If

            End If

        End Sub

        Public Shared Function getImageFileNamesInDB(ByVal PortalID As Integer) As Hashtable
            Dim aryList As ArrayList
            Dim hList As New Hashtable
            Dim objCtrl As New ProductController
            Dim objInfo As New NB_Store_ProductImageInfo
            Dim objCCtrl As New CategoryController
            Dim objCInfo As New NB_Store_CategoriesInfo

            '--- Products ---
            aryList = objCtrl.GetProductImageExportList(PortalID)

            For Each objInfo In aryList
                If Not hList.ContainsKey(Path.GetFileName(objInfo.ImagePath)) Then
                    hList.Add(System.IO.Path.GetFileName(objInfo.ImagePath), System.IO.Path.GetFileName(objInfo.ImagePath))
                End If
            Next

            '--- Categories ---
            aryList = objCCtrl.GetCategories(PortalID, GetCurrentCulture())

            For Each objCInfo In aryList
                If Not hList.ContainsKey(Path.GetFileName(objCInfo.ImageURL)) Then
                    hList.Add(System.IO.Path.GetFileName(objCInfo.ImageURL), System.IO.Path.GetFileName(objCInfo.ImageURL))
                End If
            Next


            Return hList
        End Function

        Public Shared Function getDocsFileNamesInDB(ByVal PortalID As Integer) As Hashtable
            Dim aryList As ArrayList
            Dim hList As New Hashtable
            Dim objCtrl As New ProductController
            Dim objInfo As New NB_Store_ProductDocInfo

            aryList = objCtrl.GetProductDocExportList(PortalID)

            For Each objInfo In aryList
                If Not hList.ContainsKey(Path.GetFileName(objInfo.DocPath)) Then
                    hList.Add(System.IO.Path.GetFileName(objInfo.DocPath), System.IO.Path.GetFileName(objInfo.DocPath))
                End If
            Next

            Return hList
        End Function

        Public Shared Function getUploadedFileNamesInDB(ByVal PortalID As Integer) As Hashtable
            Dim aryList As ArrayList
            Dim hList As New Hashtable
            Dim xmlDoc As XmlDataDocument
            Dim xmlNodList As XmlNodeList
            Dim xmlNod As XmlNode

            Dim objCtrl As New OrderController
            Dim objInfo As NB_Store_OrderDetailsInfo

            aryList = objCtrl.GetOrderDetailList(-1) '-1 orderid passes back all order details

            For Each objInfo In aryList
                Try
                    xmlDoc = New XmlDataDocument
                    xmlDoc.LoadXml(objInfo.CartXMLInfo)
                    xmlNodList = xmlDoc.SelectNodes("root/*")
                    For Each xmlNod In xmlNodList
                        If Not hList.ContainsKey(Path.GetFileName(xmlNod.InnerXml)) And xmlNod.Name.StartsWith("fu") Then
                            hList.Add(Path.GetFileName(xmlNod.InnerXml), Path.GetFileName(xmlNod.InnerXml))
                        End If
                    Next
                Catch ex As Exception
                    'not xml so ignore.
                End Try
            Next

            Return hList

        End Function

#End Region

#Region " Urls "

        Public Shared Sub setUrlCookieInfo(ByVal PortalID As Integer, ByVal Request As HttpRequest)

            setCookieURLparam(PortalID, "CatID", "CatID=" & Request.QueryString("CatID"))

            setCookieURLparam(PortalID, "RtnTab", "RtnTab=" & Request.QueryString("RtnTab"))

            setCookieURLparam(PortalID, "PageIndex", "PageIndex=" & Request.QueryString("PageIndex"))

            setCookieURLparam(PortalID, "orderby", "orderby=" & Request.QueryString("orderby"))

            setCookieURLparam(PortalID, "currentpage", "currentpage=" & Request.QueryString("currentpage"))

            setCookieURLparam(PortalID, "wishlist", "wishlist=" & Request.QueryString("wishlist"))

            setCookieURLparam(PortalID, "Search", "Search=" & Request.QueryString("Search"))

        End Sub

        Private Shared Sub setCookieURLparam(ByVal PortalID As Integer, ByVal ValueID As String, ByVal Value As String)
            If Value.EndsWith("=") Then Value = ""
            setStoreCookieValue(PortalID, "UrlCookieInfo", ValueID, Value, 0)
        End Sub

        Public Shared Function getCookieURLparam(ByVal PortalID As Integer, ByVal ValueID As String)
            Return getStoreCookieValue(PortalID, "UrlCookieInfo", ValueID)
        End Function

        Public Shared Function getUrlCookieInfo(ByVal PortalID As Integer, ByVal ParamArray AdditionalParameters() As String) As String()

            Dim params(AdditionalParameters.Length + 7) As String

            params(0) = getCookieURLparam(PortalID, "orderby")
            params(1) = getCookieURLparam(PortalID, "CatID")
            params(2) = getCookieURLparam(PortalID, "RtnTab")
            params(3) = getCookieURLparam(PortalID, "PageIndex")
            params(4) = getCookieURLparam(PortalID, "currentpage")
            params(5) = getCookieURLparam(PortalID, "wishlist")
            params(6) = getCookieURLparam(PortalID, "Search")

            For i As Integer = 0 To AdditionalParameters.Length - 1
                params(i + 8) = AdditionalParameters(i)
            Next

            Return params
        End Function

        Public Shared Function GetProductUrl(ByVal PortalID As Integer, ByVal TabID As Integer, ByVal objPInfo As NB_Store_ProductsInfo, ByVal CatID As Integer, ByVal CanonicalLink As Boolean) As String
            Dim strUrlName As String = GetStoreSetting(PortalID, "urlname.column", "None")
            Try
                If strUrlName <> "" Then
                    strUrlName = DataBinder.Eval(objPInfo, strUrlName)
                Else
                    strUrlName = objPInfo.SEOName
                End If
            Catch ex As Exception
                strUrlName = ""
            End Try

            Return GetProductUrlByProductID(PortalID, TabID, objPInfo.ProductID, CatID, strUrlName, CanonicalLink, objPInfo.Lang)
        End Function


        Public Shared Function GetProductUrl(ByVal PortalID As Integer, ByVal TabID As Integer, ByVal objPInfo As ProductListInfo, ByVal CatID As Integer, ByVal CanonicalLink As Boolean) As String
            Dim strUrlName As String = GetStoreSetting(PortalID, "urlname.column", "None")
            Try
                If strUrlName <> "" Then
                    strUrlName = DataBinder.Eval(objPInfo, strUrlName)
                Else
                    strUrlName = objPInfo.SEOName
                End If
            Catch ex As Exception
                strUrlName = ""
            End Try

            Return GetProductUrlByProductID(PortalID, TabID, objPInfo.ProductID, CatID, strUrlName, CanonicalLink, objPInfo.Lang)
        End Function


        Public Shared Function GetProductUrlByProductID(ByVal PortalID As Integer, ByVal TabID As Integer, ByVal ProductID As Integer, ByVal CatID As Integer, ByVal SEOName As String, ByVal CanonicalLink As Boolean, ByVal Lang As String) As String

            If SEOName <> "" Then
                SEOName = Replace(SEOName, " ", "_")
                SEOName = Regex.Replace(SEOName, "[\W]", "")
            End If

            If CanonicalLink Or CatID = -1 Then
                If GetHostSettingAsBoolean("UseFriendlyUrls", False) And SEOName <> "" Then
                    Return GetSEOLink(PortalID, TabID, "", SEOName & ".aspx", "ProdID=" & ProductID.ToString, "Language=" & Lang)
                Else
                    Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                    Return NavigateURL(TabID, False, PS, "", Lang, "ProdID=" & ProductID.ToString)
                End If
            Else
                If GetHostSettingAsBoolean("UseFriendlyUrls", False) And SEOName <> "" Then
                    Return GetSEOLink(PortalID, TabID, "", SEOName & ".aspx", "ProdID=" & ProductID.ToString, "Language=" & Lang, "CatID=" & CatID)
                Else
                    Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                    Return NavigateURL(TabID, False, PS, "", Lang, "ProdID=" & ProductID.ToString, "CatID=" & CatID)
                End If
            End If


        End Function

        Public Shared Function GetProductListUrlByCatID(ByVal PortalID As Integer, ByVal TabID As Integer, ByVal CatID As Integer, ByVal SEOName As String, ByVal Lang As String) As String

            If SEOName <> "" Then
                SEOName = Replace(SEOName, " ", "_")
                SEOName = Regex.Replace(SEOName, "[\W]", "")
            End If

            If CatID = -1 Then
                If GetHostSettingAsBoolean("UseFriendlyUrls", False) And SEOName <> "" Then
                    Return GetSEOLink(PortalID, TabID, "", SEOName & ".aspx", "Language=" & Lang)
                Else
                    Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                    Return NavigateURL(TabID, False, PS, "", Lang)
                End If
            Else
                If GetHostSettingAsBoolean("UseFriendlyUrls", False) And SEOName <> "" Then
                    Return GetSEOLink(PortalID, TabID, "", SEOName & ".aspx", "CatID=" & CatID.ToString, "Language=" & Lang)
                Else
                    Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                    Return NavigateURL(TabID, False, PS, "", Lang, "CatID=" & CatID)
                End If
            End If


        End Function



        Public Shared Sub IncludeCanonicalLink(ByVal page As Page, ByVal href As String)
            If href <> "" Then
                Dim cLink As HtmlLink
                cLink = New HtmlLink()
                cLink.Attributes.Add("rel", "canonical")
                cLink.Href = href
                page.Header.Controls.Add(cLink)
            End If
        End Sub

        Public Shared Function GetSEOLink(ByVal PortalId As Integer, ByVal TabID As Integer, ByVal ControlKey As String, ByVal Title As String, ByVal ParamArray AdditionalParameters() As String) As String

            Dim TabInfo As DotNetNuke.Entities.Tabs.TabInfo = (New DotNetNuke.Entities.Tabs.TabController).GetTab(TabID, PortalId, False)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()

            If Not TabInfo Is Nothing Then

                Dim Path As String = "~/default.aspx?tabid=" & TabInfo.TabID
                For Each p As String In AdditionalParameters

                    If supportedLanguages.Count > 1 Then
                        Path &= "&" & p
                    Else
                        'only one langauge so don't add the langauge param.
                        If Not p.ToLower.StartsWith("language") Then
                            Path &= "&" & p
                        End If
                    End If
                Next
                If String.IsNullOrEmpty(Title) Then Title = "Default.aspx"
                Return DotNetNuke.Common.Globals.FriendlyUrl(TabInfo, Path, Title)
            Else
                Return ""
            End If

        End Function

        Public Shared Sub Redirect301(ByVal Response As HttpResponse, ByVal redirectURL As String)
            Response.StatusCode = 301
            Response.Status = "301 Moved Permanently"
            Response.RedirectLocation = redirectURL
        End Sub

        Public Shared Sub Display404Error(ByVal PortalID As Integer, ByVal Response As System.Web.HttpResponse, ByVal Server As System.Web.HttpServerUtility)
            Dim strURL As String = GetStoreSetting(PortalID, "err404.url", GetCurrentCulture)
            If strURL <> "" Then
                Response.StatusCode = 404
                Server.Transfer(strURL)
                Response.End()
            End If
        End Sub

        Public Shared Function getURLParam(ByVal context As HttpContext, ByVal ParamKey As String) As String
            If Not (context.Request.QueryString(ParamKey) Is Nothing) Then
                Return context.Request.QueryString(ParamKey)
            Else
                Return ""
            End If
        End Function

        Public Shared Function ProductPageName(ByVal ProductListInfo As ProductListInfo, ByVal CategoryName As String, ByVal PortalName As String) As String

            Dim strText As String = GetStoreSettingText(ProductListInfo.PortalID, "ProductPageName.template", ProductListInfo.Lang)
            If strText = "" Then
                strText = ProductListInfo.ProductName
            Else
                strText = Replace(strText, "[TAG:PRODUCTREF]", ProductListInfo.ProductRef)
                strText = Replace(strText, "[TAG:PRODUCTNAME]", ProductListInfo.ProductName)
                strText = Replace(strText, "[TAG:MANUFACTURER]", ProductListInfo.Manufacturer)
                strText = Replace(strText, "[TAG:SUMMARY]", ProductListInfo.Summary)
                strText = Replace(strText, "[TAG:TAGWORDS]", ProductListInfo.TagWords)
                strText = Replace(strText, "[TAG:SEONAME]", ProductListInfo.SEOName)
                strText = Replace(strText, "[TAG:CATEGORYNAME]", CategoryName)
                strText = Replace(strText, "[TAG:PORTALNAME]", PortalName)
            End If

            Return strText
        End Function


#End Region

#Region "Templating method"

        Public Shared Function populateGenObject(ByVal dlGenXML As DataList, ByVal obj As Object, Optional ByVal RowIndex As Integer = 0) As Object
            Dim Ctrl As Control
            Dim ddlCtrls As New Collection
            Dim rblCtrls As New Collection
            Dim chkCtrls As New Collection
            Dim txtCtrls As New Collection
            Dim hidCtrls As New Collection
            Dim chkboxCtrls As New Collection
            Dim intQty As Integer = 1
            Dim DLItem As DataListItem

            'check row exists (0 based)
            If dlGenXML.Items.Count <= RowIndex Then
                Return obj
            End If

            'only do if entry already created
            If dlGenXML.Items.Count >= 1 Then

                DLItem = dlGenXML.Items(RowIndex)

                'build list of controls
                For Each Ctrl In DLItem.Controls
                    'hidden fields do not have an attribute to do databind
                    If TypeOf Ctrl Is DropDownList Then
                        ddlCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is CheckBox Then
                        chkboxCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is RadioButtonList Then
                        rblCtrls.Add(Ctrl)
                    End If
                    If TypeOf Ctrl Is TextBox Then
                        txtCtrls.Add(Ctrl)
                    End If
                Next

                Dim txtCtrl As TextBox
                For Each txtCtrl In txtCtrls
                    If Not txtCtrl.Attributes.Item("databind") Is Nothing Then
                        obj = assignByReflection(obj, txtCtrl.Attributes.Item("databind"), txtCtrl.Text)
                    End If
                Next

                Dim chkboxCtrl As CheckBox
                For Each chkboxCtrl In chkboxCtrls
                    If Not chkboxCtrl.Attributes.Item("databind") Is Nothing Then
                        obj = assignByReflection(obj, chkboxCtrl.Attributes.Item("databind"), chkboxCtrl.Checked)
                    End If
                Next

                Dim ddlCtrl As DropDownList
                For Each ddlCtrl In ddlCtrls
                    If Not ddlCtrl.Attributes.Item("databind") Is Nothing Then
                        obj = assignByReflection(obj, ddlCtrl.Attributes.Item("databind"), ddlCtrl.SelectedValue)
                    End If
                Next

                Dim rblCtrl As RadioButtonList
                For Each rblCtrl In rblCtrls
                    If Not rblCtrl.Attributes.Item("databind") Is Nothing Then
                        obj = assignByReflection(obj, rblCtrl.Attributes.Item("databind"), rblCtrl.SelectedValue)
                    End If
                Next

            End If

            Return obj

        End Function

        Public Shared Function RenderDataList(ByVal PortalID As Integer, ByVal TemplateName As String, ByVal Lang As String) As String
            Dim objInfo As New Object
            Return RenderDataList(PortalID, TemplateName, Lang, objInfo)
        End Function

        Public Shared Function RenderDataList(ByVal PortalID As Integer, ByVal TemplateName As String, ByVal Lang As String, ByVal objInfo As Object) As String
            Dim arylist As New ArrayList
            Dim dlGen As New System.Web.UI.WebControls.DataList
            Dim TemplateText As String

            TemplateText = System.Web.HttpUtility.HtmlDecode(GetStoreSettingText(PortalID, TemplateName, Lang))

            dlGen.ItemTemplate = New GenXMLTemplate(TemplateText)

            arylist.Add(objInfo)

            dlGen.DataSource = arylist
            dlGen.DataBind()

            'Get the rendered HTML
            Dim SB As New StringBuilder()
            Dim SW As New StringWriter(SB)
            Dim htmlTW As New System.Web.UI.HtmlTextWriter(SW)
            dlGen.RenderControl(htmlTW)

            Return SB.ToString()
        End Function

        Public Shared Function getSettingTextName(ByVal Portalid As Integer, ByVal SettingName As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As String
            'this function allows an interface to choose the setting name to be returned
            If Not EventInterface.Instance() Is Nothing Then
                SettingName = EventInterface.Instance.getSettingTextName(Portalid, SettingName, UserInfo)
            End If
            Return SettingName
        End Function

        Public Shared Function getSettingName(ByVal Portalid As Integer, ByVal SettingName As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo) As String
            'this function allows an interface to choose the setting name to be returned
            If Not EventInterface.Instance() Is Nothing Then
                SettingName = EventInterface.Instance.getSettingName(Portalid, SettingName, UserInfo)
            End If
            Return SettingName
        End Function

#End Region

#Region "Gateway Functions"

        Public Shared Function GetAvailableGatewaysTable(ByVal PortalID As Integer) As Hashtable
            Dim hTable As New Hashtable
            Dim objGInfo As NB_Store_GatewayInfo
            Dim aryList As ArrayList

            aryList = GetAvailableGateways(PortalID)

            For Each objGInfo In aryList
                hTable.Add(objGInfo.ref, objGInfo)
            Next

            Return hTable
        End Function

        Public Shared Function GetAvailableGateways(ByVal PortalID As Integer) As ArrayList
            Dim aryList As New ArrayList
            Dim objGInfo As NB_Store_GatewayInfo

            Dim ProviderClassList As String

            ProviderClassList = GetStoreSetting(PortalID, "gateway.provider", "XX")

            Dim ProviderXML As String
            Dim xmlDoc As New XmlDataDocument
            Dim xmlNod As XmlNode

            ProviderXML = GetStoreSetting(PortalID, "gatewayproviders.xml", GetCurrentCulture, True)
            xmlDoc.LoadXml(ProviderXML)

            For Each xmlNod In xmlDoc.SelectNodes("root/gateways/gateway")
                If Not xmlNod.SelectSingleNode("assembly") Is Nothing Then
                    If InStr(ProviderClassList, xmlNod.SelectSingleNode("assembly").InnerText) >= 0 Then
                        objGInfo = New NB_Store_GatewayInfo
                        objGInfo.ref = xmlNod.Attributes("ref").InnerText
                        objGInfo.name = xmlNod.SelectSingleNode("name").InnerText
                        objGInfo.assembly = xmlNod.SelectSingleNode("assembly").InnerText
                        objGInfo.classname = xmlNod.SelectSingleNode("name").InnerText
                        If Not xmlNod.SelectSingleNode("gatewaymsg") Is Nothing Then
                            objGInfo.gatewaymsg = xmlNod.SelectSingleNode("gatewaymsg").InnerText
                        Else
                            objGInfo.gatewaymsg = xmlNod.SelectSingleNode("name").InnerText
                        End If
                        objGInfo.gatewaytype = "PRO"
                        aryList.Add(objGInfo)
                    End If
                End If
            Next


            ProviderClassList = GetStoreSetting(PortalID, "encapsulated.provider", "XX")

            ProviderXML = GetStoreSetting(PortalID, "encapsulatedproviders.xml", GetCurrentCulture, True)
            xmlDoc = New XmlDataDocument
            xmlDoc.LoadXml(ProviderXML)

            For Each xmlNod In xmlDoc.SelectNodes("root/gateways/gateway")
                If Not xmlNod.SelectSingleNode("assembly") Is Nothing Then
                    If InStr(ProviderClassList, xmlNod.SelectSingleNode("assembly").InnerText) >= 0 Then
                        objGInfo = New NB_Store_GatewayInfo
                        objGInfo.ref = xmlNod.Attributes("ref").InnerText
                        objGInfo.name = xmlNod.SelectSingleNode("name").InnerText
                        objGInfo.assembly = xmlNod.SelectSingleNode("assembly").InnerText
                        objGInfo.classname = xmlNod.SelectSingleNode("name").InnerText
                        If Not xmlNod.SelectSingleNode("gatewaymsg") Is Nothing Then
                            objGInfo.gatewaymsg = xmlNod.SelectSingleNode("gatewaymsg").InnerText
                        Else
                            objGInfo.gatewaymsg = xmlNod.SelectSingleNode("name").InnerText
                        End If
                        objGInfo.gatewaytype = "CHQ"
                        aryList.Add(objGInfo)
                    End If
                End If
            Next

            Return aryList

        End Function

#End Region

#Region "Stock functions"

        Public Shared Function getProductStockLevels(ByVal container As DataListItem) As ProductStockLevels
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Return getProductStockLevels(PS.PortalId, container)
        End Function

        Public Shared Function getProductStockLevels(ByVal PortalID As Integer, ByVal container As DataListItem) As ProductStockLevels
            Dim rtnLevels As New ProductStockLevels
            Dim CartQty As Integer = 0
            Dim QtyR As Integer = DataBinder.Eval(container.DataItem, "QtyRemaining")
            Dim QtySet As Integer = DataBinder.Eval(container.DataItem, "QtyStockSet")
            Dim LockStockOnCart As Boolean = GetStoreSettingBoolean(PortalID, "lockstockoncart")

            If QtyR < 0 Then
                'stock control turned off.
                Return rtnLevels
            Else
                rtnLevels.StockOn = True
            End If


            rtnLevels.Qty = QtyR
            rtnLevels.MaxQty = QtySet

            If LockStockOnCart Then
                'adjust for amount in cart
                Dim objCCtrl As New CartController
                Dim objPCtrl As New ProductController
                Dim ProdID As Integer = CInt(DataBinder.Eval(container.DataItem, "ProductID"))
                Dim Lang As String = DataBinder.Eval(container.DataItem, "Lang")
                Dim aryList As ArrayList
                aryList = objPCtrl.GetModelList(PortalID, ProdID, Lang, True)

                For Each objInfo As NB_Store_ModelInfo In aryList
                    If objInfo.QtyRemaining >= 0 Then
                        CartQty += objCCtrl.GetCartModelQty(PortalID, objInfo.ModelID)
                    End If
                Next
                rtnLevels.CartQty = CartQty
            End If


            If rtnLevels.MaxQty > 0 And rtnLevels.Qty > 0 Then

                'stock percent left (not including cart)
                rtnLevels.Percent = CInt(((100 / (rtnLevels.MaxQty)) * (rtnLevels.Qty - rtnLevels.CartQty)))
                If rtnLevels.Percent < 0 Then rtnLevels.Percent = 0
                If rtnLevels.Percent = 0 And (rtnLevels.Qty - rtnLevels.CartQty) > 0 Then
                    rtnLevels.Percent = 1 ' if any product left set to 1%
                End If

                'stock percent actual
                rtnLevels.PercentActual = CInt(((100 / rtnLevels.MaxQty) * rtnLevels.Qty))

                'stock percent sold 
                rtnLevels.PercentSold = 100 - CInt(((100 / rtnLevels.MaxQty) * rtnLevels.Qty))
                If rtnLevels.PercentSold = 100 Then
                    rtnLevels.PercentSold = 99
                End If

                'stock in progress
                'rtnLevels.PercentInProgess = CInt((100 / rtnLevels.MaxQty) * rtnLevels.CartQty)
                rtnLevels.PercentInProgess = 100 - (rtnLevels.Percent + rtnLevels.PercentSold)
            Else
                rtnLevels.PercentSold = 100
                rtnLevels.PercentInProgess = 0
                rtnLevels.PercentActual = 0
                rtnLevels.Percent = 0
            End If

            Return rtnLevels
        End Function

#End Region

#Region "Feeder Functions"

        Public Shared Function GetFeederReportText(ByVal context As System.Web.HttpContext, ByVal FormatAsXML As Boolean) As String
            Dim strText As String = ""
            Dim key As String = getURLParam(context, "key")
            Dim ProdID As String = getURLParam(context, "ProdID")
            Dim ProdRef As String = getURLParam(context, "ProdRef")
            Dim CatID As String = getURLParam(context, "CatID")
            Dim Lang As String = getURLParam(context, "Lang")
            Dim Search As String = getURLParam(context, "Search")
            Dim Pass As String = getURLParam(context, "pass")
            Dim StoreTabID As String = getURLParam(context, "TabID")
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim PortalID As Integer = PS.PortalId

            Dim strCacheKey As String = key & Pass & ProdID & CatID & FormatAsXML

            Dim debugmode As Boolean = GetStoreSettingBoolean(CInt(PortalID), "debug.mode", "None")

            If Lang = "" Then Lang = GetMerchantCulture(PortalID)

            If FormatAsXML Then
                strText = "<root></root>" ' create a default xml
            End If

            If getLangCache(CInt(PortalID), strCacheKey, Lang) Is Nothing Or debugmode Then
                Dim objInfo As FeederSetInfo
                Dim htSet As Hashtable
                htSet = populateFeederSettings(CInt(PortalID))

                If htSet Is Nothing Then
                    strText = "Invalid ""feeder.settings"" setting"
                Else
                    If htSet.Contains(key) Then
                        objInfo = CType(htSet.Item(key), FeederSetInfo)

                        If objInfo.reportref <> "" And Pass = objInfo.password Then
                            Dim objRInfo As NB_Store_SQLReportInfo
                            Dim objRCtrl As New SQLReportController

                            objRInfo = objRCtrl.GetSQLReportByRef(CInt(PortalID), objInfo.reportref)
                            If Not objRInfo Is Nothing Then
                                If objRInfo.SQL <> "" Then
                                    objRInfo.SQL = objRCtrl.insertParams(objRInfo.ReportID, -1, CInt(PortalID), context.Request, objRInfo.SQL, GetCurrentCulture)
                                    If FormatAsXML Then
                                        strText = objRCtrl.runXSL(objRInfo)
                                        If strText = "" Then
                                            'no xsl output, so run stright SQL
                                            Try
                                                strText = objRCtrl.ExecuteSQLReportXml(objRInfo.SQL)
                                            Catch ex As Exception
                                                strText = "ERROR runnig ExecuteSQLReportXml:   " & ex.ToString
                                            End Try
                                        End If
                                    Else
                                        Try
                                            strText = objRCtrl.ExecuteSQLReportText(objRInfo.SQL, objRInfo.FieldDelimeter, objRInfo.FieldQualifier, objRInfo.AllowDisplay)
                                        Catch ex As Exception
                                            strText = "ERROR runnig ExecuteSQL:   " & ex.ToString
                                        End Try
                                    End If

                                    If CInt(objInfo.cachemins) > 0 And Not strText.StartsWith("ERROR") Then
                                        If FormatAsXML Then
                                            strText = "<root>" & strText & "</root>"
                                        End If
                                        setLangCache(CInt(PortalID), strCacheKey, Lang, strText, CInt(objInfo.cachemins))
                                    End If

                                End If
                            End If

                        Else
                            If debugmode And objInfo.functionkey = "" Then
                                strText = "Invalid Password for Report: " & objInfo.Key
                            End If
                        End If

                        If objInfo.functionkey <> "" Then

                            Dim objCtrl As New ProductController
                            Dim objExp As New Export

                            If objInfo.functionkey = "xmlGetProductsInCat" And IsNumeric(PortalID) And IsNumeric(CatID) Then
                                strText = objCtrl.xmlGetProductsInCat(CInt(PortalID), CInt(CatID), Lang)
                            ElseIf objInfo.functionkey = "xmlGetProduct" Then
                                strText = objExp.GetProductXML(CInt(ProdID), Lang)
                            ElseIf objInfo.functionkey = "xmlGetRSS" And IsNumeric(PortalID) And IsNumeric(CatID) Then
                                If CatID >= 0 Then ' never send back entire store, could overload server on big sites.
                                    Dim aryList As ArrayList
                                    aryList = objCtrl.GetProductList(CInt(PortalID), CInt(CatID), Lang, False)
                                    strText = getRSSoutput(aryList, StoreTabID)
                                End If
                            End If

                            setLangCache(CInt(PortalID), strCacheKey, Lang, strText, CInt(objInfo.cachemins))

                        End If
                    End If
                End If
            Else
                strText = getLangCache(CInt(PortalID), strCacheKey, Lang).ToString
            End If

            Return strText
        End Function


        Public Shared Function populateFeederSettings(ByVal PortalId As Integer) As Hashtable
            Dim strSettings As String
            Dim xmlSettings As XmlDataDocument
            Dim htSet As New Hashtable
            Dim objInfo As FeederSetInfo

            strSettings = GetStoreSetting(PortalId, "feeder.settings", "None")

            xmlSettings = New XmlDataDocument

            Try
                xmlSettings.LoadXml(strSettings)

                For Each xmlNod As XmlNode In xmlSettings.SelectNodes("root/item")
                    objInfo = New FeederSetInfo
                    objInfo.Key = xmlNod.Attributes(0).InnerXml
                    objInfo.password = xmlNod.SelectSingleNode("./password").InnerXml
                    objInfo.reportref = xmlNod.SelectSingleNode("./reportref").InnerXml
                    objInfo.functionkey = xmlNod.SelectSingleNode("./functionkey").InnerXml
                    objInfo.cachemins = xmlNod.SelectSingleNode("./cachemins").InnerXml

                    htSet.Add(objInfo.Key, objInfo)
                Next
                Return htSet
            Catch ex As Exception
                Return Nothing
            End Try
        End Function

        Public Shared Function getRSSoutput(ByVal aryList As ArrayList, ByVal StoreTabID As String) As String
            Dim strRtn As String = ""
            Dim strHeader As String = ""
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

            'get header
            strHeader = GetStoreSettingText(PS.PortalId, "rss.template", GetCurrentCulture, False, True)

            'get items
            For Each objP As ProductListInfo In aryList
                strRtn &= getRSSItem(objP, StoreTabID)
            Next

            strRtn = Replace(strHeader, "[Product:rssitem]", strRtn)

            Return strRtn
        End Function

        Public Shared Function getRSSItem(ByVal objInfo As ProductListInfo, ByVal StoreTabID As String) As String
            Dim strRtn As String = ""
            Dim objT As New TokenStoreReplace(objInfo)
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings

            strRtn = GetStoreSettingText(objInfo.PortalID, "rssitem.template", objInfo.Lang, False, True)
            strRtn = Replace(strRtn, "<![CDATA[", "**CDATASTART**")
            strRtn = Replace(strRtn, "]]>", "**CDATAEND**")
            If Not IsNumeric(StoreTabID) Then
                StoreTabID = GetStoreSetting(objInfo.PortalID, "store.tab", objInfo.Lang)
            End If
            If IsNumeric(StoreTabID) Then
                strRtn = Replace(strRtn, "[Product:Link]", DotNetNuke.Common.ResolveUrl(GetProductUrl(objInfo.PortalID, CInt(StoreTabID), objInfo, -1, True)))
            Else
                strRtn = Replace(strRtn, "[Product:Link]", "NO STORE TABID")
            End If

            strRtn = objT.DoTokenReplace(strRtn)

            strRtn = Replace(strRtn, "**CDATASTART**", "<![CDATA[")
            strRtn = Replace(strRtn, "**CDATAEND**", "]]>")

            Return strRtn
        End Function




#End Region

    End Class


End Namespace
