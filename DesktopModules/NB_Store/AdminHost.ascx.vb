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

Imports System.Web.UI
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Data
Imports System.Xml

Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports NEvoWeb.Modules.NB_Store.ThumbFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Class AdminHost
        Inherits BaseAdminModule

        Private _ShowUpgrade As Boolean = False
        Private _DoUpgrade As Boolean = False

#Region "Event Handlers"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' Page_Load runs when the control is loaded
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try

                DisplayMsgText(PortalId, PlaceHolder1, "adminhost.text", "")

                If (UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrators")) Then
                    Dim blnOverwrite As Boolean = False

                    If Not Page.IsPostBack Then

                        pnlCheckBoxLists.Visible = False

                        'get if overwrite option has been set
                        blnOverwrite = GetStoreSettingBoolean(PortalId, "settings.overwrite", "None")
                        chkOverwriteSettings.Checked = blnOverwrite

                        PopulatePortalList()

                    End If

                    If _ShowUpgrade Then
                        cmdUpgradeAll.Visible = True
                    Else
                        cmdUpgradeAll.Visible = False
                    End If

                    cmdClearStore.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdClearStoreMsg", LocalResourceFile) & "');")
                    cmdImportDefault.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdImportDefaultMsg", LocalResourceFile) & "');")
                    cmdClearSettings.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdClearSettingsMsg", LocalResourceFile) & "');")
                    cmdCreateThumbs.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdCreateThumbsMsg", LocalResourceFile) & "');")
                    cmdPurgeFiles.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdPurgeFilesMsg", LocalResourceFile) & "');")
                    pnlHost.Visible = True
                Else
                    pnlHost.Visible = False
                End If

            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub



#End Region

#Region "Methods"



        Private Sub PopulatePortalList()
            If UserInfo.IsSuperUser Then
                Dim aryList As New ArrayList
                Dim aryPList As ArrayList
                Dim objPoCtrl As New DotNetNuke.Entities.Portals.PortalController
                Dim objPoInfo As DotNetNuke.Entities.Portals.PortalInfo
                Dim vSetting As String

                aryPList = objPoCtrl.GetPortals

                For Each objPoInfo In aryPList
                    vSetting = GetStoreSetting(objPoInfo.PortalID, "version", "None")
                    If vSetting <> "" Then
                        objPoInfo.Version = vSetting
                        aryList.Add(objPoInfo)
                    End If
                Next

                dgPortals.Visible = True
                dgPortals.DataSource = aryList
                dgPortals.DataBind()

            Else
                dgPortals.Visible = False
            End If

        End Sub

        Private Sub populateImportCheckBox(ByVal ImpFileName As String)
            Dim objSCtrl As New SettingsController
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim li As ListItem
            Dim xmlDoc As New XmlDataDocument
            Dim objImpCtrl As New ManagerMenuController
            Dim nodList As XmlNodeList
            Dim nod As XmlNode

            If ImpFileName <> "" Then
                xmlDoc.Load(Server.MapPath(StoreInstallPath & "templates\" & ImpFileName))
            End If


            pnlCheckBoxLists.Visible = True
            lblLang.Text = Localization.GetString("lblLang", LocalResourceFile)
            lblchkSettings.Text = Localization.GetString("lblchkSettings", LocalResourceFile)
            lblTemplates.Text = Localization.GetString("lblTemplates", LocalResourceFile)
            chkSelectAll.Text = Localization.GetString("chkSelectAll", LocalResourceFile)
            chkSelectAllT.Text = Localization.GetString("chkSelectAll", LocalResourceFile)
            cmdSaveSettings.Visible = False
            fupTemplate.Visible = False
            chkOverwriteSettings.Enabled = False
            cmdClearSettings.Visible = False
            dgPortals.Visible = False

            'languages
            chkLlang.Items.Clear()
            li = New ListItem
            li.Text = Localization.GetString("Default", LocalResourceFile)
            li.Value = "None"
            li.Selected = True
            chkLlang.Items.Add(li)
            For Each Lang As String In supportedLanguages
                li = New ListItem
                li.Text = Lang
                li.Value = Lang
                li.Selected = True
                chkLlang.Items.Add(li)
            Next

            'settings
            chkLsettings.Items.Clear()

            nodList = xmlDoc.SelectNodes("content/storecontent/settings/*")
            For Each nod In nodList
                li = New ListItem
                If Trim(nod.Attributes("Lang").InnerXml) <> "None" Then
                    li.Text = nod.Name & nod.Attributes("Lang").InnerXml
                Else
                    li.Text = nod.Name
                End If
                li.Value = nod.Name
                li.Selected = chkSelectAll.Checked
                chkLsettings.Items.Add(li)
            Next

            'templates
            chkLtemplates.Items.Clear()

            nodList = xmlDoc.SelectNodes("content/storecontent/settingstext/*")
            For Each nod In nodList
                li = New ListItem
                If Trim(nod.Attributes("Lang").InnerXml) <> "None" Then
                    li.Text = nod.Name & " : " & nod.Attributes("Lang").InnerXml
                Else
                    li.Text = nod.Name
                End If
                li.Value = nod.Name
                li.Selected = chkSelectAllT.Checked
                chkLtemplates.Items.Add(li)
            Next

        End Sub


        Private Sub populateExportCheckBox()
            Dim objSCtrl As New SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim aryList As ArrayList
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            Dim li As ListItem

            pnlCheckBoxLists.Visible = True
            lblLang.Text = Localization.GetString("lblLang", LocalResourceFile)
            lblchkSettings.Text = Localization.GetString("lblchkSettings", LocalResourceFile)
            lblTemplates.Text = Localization.GetString("lblTemplates", LocalResourceFile)
            chkSelectAll.Text = Localization.GetString("chkSelectAll", LocalResourceFile)
            chkSelectAllT.Text = Localization.GetString("chkSelectAll", LocalResourceFile)

            cmdImportDefault.Visible = False
            fupTemplate.Visible = False
            chkOverwriteSettings.Visible = False
            cmdClearSettings.Visible = False
            dgPortals.Visible = False

            'languages
            chkLlang.Items.Clear()
            li = New ListItem
            li.Text = Localization.GetString("Default", LocalResourceFile)
            li.Value = "None"
            li.Selected = True
            chkLlang.Items.Add(li)
            For Each Lang As String In supportedLanguages
                li = New ListItem
                li.Text = Lang
                li.Value = Lang
                li.Selected = True
                chkLlang.Items.Add(li)
            Next

            'settings
            chkLsettings.Items.Clear()
            aryList = objSCtrl.GetSettingList(PortalId, "", True, "")
            For Each objSInfo In aryList
                li = New ListItem
                If objSInfo.Lang.Trim <> "None" Then
                    li.Text = objSInfo.SettingName & " : " & objSInfo.Lang.Trim
                Else
                    li.Text = objSInfo.SettingName
                End If
                li.Value = objSInfo.SettingName
                li.Selected = chkSelectAll.Checked
                chkLsettings.Items.Add(li)
            Next

            'templates
            chkLtemplates.Items.Clear()
            aryList = objSCtrl.GetSettingsTexts(PortalId, "", True, "")
            For Each objSTInfo In aryList
                li = New ListItem
                If objSTInfo.Lang.Trim <> "None" Then
                    li.Text = objSTInfo.SettingName & " : " & objSTInfo.Lang.Trim
                Else
                    li.Text = objSTInfo.SettingName
                End If
                li.Value = objSTInfo.SettingName
                li.Selected = chkSelectAllT.Checked
                chkLtemplates.Items.Add(li)
            Next


        End Sub


        Private Sub DoDefaultImport(ByVal ImpFileName As String)
            Dim xmlDoc As New XmlDataDocument
            Dim objImpCtrl As New ManagerMenuController

            If ImpFileName <> "" Then
                xmlDoc.Load(Server.MapPath(StoreInstallPath & "templates\" & ImpFileName))
                objImpCtrl.ImportModuleData(ModuleId, xmlDoc.SelectSingleNode("content").InnerXml, "1", UserId, chkLsettings, chkLtemplates, chkLlang)
            End If

            Dim objU As New Upgrade
            objU.setStoreVersionToCurrent(PortalId, StoreInstallPath)

        End Sub

#End Region


#Region "Events"

        Private Sub cmdClearStore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClearStore.Click
            If txtPass.Text = Users.UserController.GetPassword(UserInfo, "") Then
                lblInvalidPass.Visible = False
                DataProvider.Instance().ClearDownStore(PortalId)
            Else
                lblInvalidPass.Visible = True
            End If
        End Sub

        Private Sub cmdValidation_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdValidation.Click
            Dim objPCtrl As New ProductController
            Dim strMSG As String = ""

            'do product image validation

            CreateDir(PortalSettings, PRODUCTDOCSFOLDER)
            CreateDir(PortalSettings, PRODUCTIMAGESFOLDER)
            CreateDir(PortalSettings, PRODUCTTHUMBSFOLDER)

            'resync DNN files
            FileSystemUtils.SynchronizeFolder(PortalId, PortalSettings.HomeDirectoryMapPath & PRODUCTDOCSFOLDER, PortalSettings.HomeDirectory & PRODUCTDOCSFOLDER, True, True, True)
            FileSystemUtils.SynchronizeFolder(PortalId, PortalSettings.HomeDirectoryMapPath & PRODUCTIMAGESFOLDER, PortalSettings.HomeDirectory & PRODUCTIMAGESFOLDER, True, True, True)
            FileSystemUtils.SynchronizeFolder(PortalId, PortalSettings.HomeDirectoryMapPath & PRODUCTTHUMBSFOLDER, PortalSettings.HomeDirectory & PRODUCTTHUMBSFOLDER, True, True, True)

            strMSG = "<br /><br />Product Images: "
            strMSG &= objPCtrl.ImageValidation(PortalId, MapPath("\"), chkValidation.Checked)
            strMSG &= "<br />Product Docs: "
            strMSG &= objPCtrl.DocValidation(PortalId, PortalSettings.HomeDirectoryMapPath & PRODUCTDOCSFOLDER, chkValidation.Checked)
            If chkValidation.Checked Then
                strMSG &= "<br />Product Languages: "
                strMSG &= objPCtrl.ProductLangValidation(PortalId)
                strMSG &= "<br />Category Languages: "
                strMSG &= objPCtrl.CategoryLangValidation(PortalId)
            Else
                strMSG &= "<br />Product Languages: NOT CHECKED"
                strMSG &= "<br />Category Languages: NOT CHECKED"
            End If

            If strMSG <> "" Then
                lblValidationMsg.Text = strMSG
                lblValidationMsg.Visible = True
            End If

        End Sub

        Private Sub cmdImportDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdImportDefault.Click
            If pnlCheckBoxLists.Visible Then
                DoDefaultImport("ImportedSettings.xml")
                removeLangCache(PortalId, "nbstoreadmintemplatetreemenu" & UserId.ToString)
                removeLangCache(PortalId, "nbstoreadminsettingtreemenu" & UserId.ToString)
                Response.Redirect(EditUrl("AdminHost"), True)
            Else
                If fupTemplate.FileName <> "" Then
                    fupTemplate.SaveAs(Server.MapPath(StoreInstallPath & "templates\ImportedSettings.xml"))
                    If chkOverwriteSettings.Checked Then
                        SetStoreSetting(PortalId, "settings.overwrite", 1, "None", True)
                    Else
                        SetStoreSetting(PortalId, "settings.overwrite", 0, "None", True)
                    End If
                    populateImportCheckBox("ImportedSettings.xml")
                End If
            End If

        End Sub


        Private Sub cmdClearTempUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClearTempUpload.Click
            RemoveFiles(PortalSettings, "temp")
        End Sub

        Private Sub cmdSaveSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSaveSettings.Click
            If pnlCheckBoxLists.Visible Then
                Dim objImpCtrl As New ManagerMenuController
                Dim strExport As String = ""

                strExport = "<content type=""" & ModuleConfiguration.DesktopModule.ModuleName & """ version=""" & ModuleConfiguration.DesktopModule.Version & """>"
                strExport &= objImpCtrl.ExportModuleData(ModuleId, chkLsettings, chkLtemplates, chkLlang, False, False)
                strExport &= "</content>"

                ForceStringDownload(Response, "NB_Store_ExportSettings.xml", strExport)

            Else
                populateExportCheckBox()
            End If

        End Sub

        Private Sub cmdClearSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClearSettings.Click
            Dim objSCtrl As New SettingsController
            Dim aryList As ArrayList
            Dim objSInfo As NB_Store_SettingsInfo
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim objUpg As New Upgrade

            aryList = objSCtrl.GetSettingList(PortalId, "", True, "")
            For Each objSInfo In aryList
                objSCtrl.DeleteSetting(PortalId, objSInfo.SettingName, objSInfo.Lang)
            Next


            aryList = objSCtrl.GetSettingsTexts(PortalId, "", True, "")
            For Each objSTInfo In aryList
                objSCtrl.DeleteSettingsText(PortalId, objSTInfo.SettingName, objSTInfo.Lang)
            Next

            removeLangCache(PortalId, "nbstoreadmintemplatetreemenu" & UserId.ToString)
            removeLangCache(PortalId, "nbstoreadminsettingtreemenu" & UserId.ToString)

            objUpg.DoUpgrade(PortalId, StoreInstallPath, UserId)

        End Sub

        Private Sub cmdRestart_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdRestart.Click
            Dim obj As Object
            obj = New Config
            obj.Touch()
            ' Use late binding to get rid of early binding dependancy, DNN function changed in DNN5.6
            'Config.Touch()
            Response.Redirect(NavigateURL(), True)
        End Sub

        Private Sub cmdCalcSale_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCalcSale.Click
            Dim objCtrl As New PromoController
            objCtrl.createSalePriceTable(PortalId)
        End Sub

        Private Sub cmdExportReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExportReport.Click
            Dim strExport As String = ""
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim Exp As New Export

            strExport = "<content type=""" & ModuleConfiguration.DesktopModule.ModuleName & """ version=""" & ModuleConfiguration.DesktopModule.Version & """>"
            strExport &= Exp.GetSQLReports(PS.PortalId)
            strExport &= "</content>"

            ForceStringDownload(Response, "NB_Store_ExportReports.xml", strExport)

        End Sub

        Private Sub cmdImportReports_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdImportReports.Click
            If fupReports.FileName <> "" Then
                fupReports.SaveAs(Server.MapPath(StoreInstallPath & "templates\ImportedReports.xml"))
                Dim objImpCtrl As New Import
                objImpCtrl.ImportSQLReports(PortalId, Server.MapPath(StoreInstallPath & "templates\ImportedReports.xml"), chkOverwriteReport.Checked)
            End If
        End Sub

        Private Sub dgPortals_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataGridItemEventArgs) Handles dgPortals.ItemDataBound
            Dim item As DataGridItem = e.Item

            If item.ItemType = ListItemType.Item Or _
                item.ItemType = ListItemType.AlternatingItem Or _
                item.ItemType = ListItemType.SelectedItem Then

                Dim pID As Integer
                Dim ver As String
                Dim objUCtrl As New Upgrade

                pID = CInt(item.Cells(0).Text)
                ver = item.Cells(2).Text

                If ver <> objUCtrl.getCurrentStoreVersion(StoreInstallPath) Then
                    _ShowUpgrade = True
                End If


                If _DoUpgrade Then
                    Dim dglblMsg As Label = DirectCast(e.Item.FindControl("lblMsg"), Label)

                    If objUCtrl.DoUpgrade(pID, StoreInstallPath, UserId) = "" Then
                        dglblMsg.Text = Localization.GetString("UpgradeOK", LocalResourceFile)
                    Else
                        dglblMsg.Text = Localization.GetString("UpgradeFAIL", LocalResourceFile)
                    End If

                    item.Cells(2).Text = GetStoreSetting(pID, "version", "None")

                End If



            End If

        End Sub

        Private Sub cmdUpgradeAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpgradeAll.Click
            _DoUpgrade = True
            PopulatePortalList()
        End Sub

        Private Sub chkSelectAll_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged, chkSelectAllT.CheckedChanged
            If cmdImportDefault.Visible Then
                populateImportCheckBox("ImportedSettings.xml")
            Else
                populateExportCheckBox()
            End If
        End Sub

        Private Sub cmdCreateThumbs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCreateThumbs.Click
            If GetStoreSettingBoolean(PortalId, "diskthumbnails.flag") Then
                Dim ThumbSizes As String = GetStoreSetting(PortalId, "diskthumbnails.size")
                If ThumbSizes <> "" Then
                    Dim objThumbs As New ThumbFunctions

                    CreateDir(PortalSettings, PRODUCTTHUMBSFOLDER)

                    If IsNumeric(GetStoreSetting(PortalId, "image.quality")) Then
                        objThumbs._ImageQuality = CInt(GetStoreSetting(PortalId, "image.quality"))
                    End If
                    objThumbs.CreateAllThumbsOnDisk(PortalId, GetCurrentCulture, ThumbSizes)
                End If
            End If
        End Sub

        Private Sub cmdPurgeFiles_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdPurgeFiles.Click
            PurgeAllFiles(PortalSettings)
        End Sub


#End Region


    End Class

End Namespace
