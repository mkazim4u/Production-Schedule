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


Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.Xml

Namespace NEvoWeb.Modules.NB_Store

    'TODO: Re-write to get better admin interface.

    Partial Public Class AdminSettings
        Inherits BaseAdminModule


#Region "Private Members"

        Protected TemID As String = ""
        Protected EditTemplateMode As Boolean = False

#End Region

#Region "Event Handlers"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                selectlang.DisplayNoLang = True

                ' Determine ItemId of NB_Store to Update
                If Not (Request.QueryString("TemId") Is Nothing) Then
                    TemID = Request.QueryString("TemId")
                Else
                    TemID = ""
                End If

                'Work out if we're editing templates..
                If Not (Request.QueryString("etmode") Is Nothing) Then
                    If Request.QueryString("etmode") = "1" Then
                        EditTemplateMode = True
                    Else
                        EditTemplateMode = False
                    End If
                Else
                    EditTemplateMode = False
                End If

                ' If this is the first visit to the page, bind the role data to the datalist
                If Not Page.IsPostBack Then

                    If selectlang.EditedLang = "" Then
                        selectlang.SelectedLang = "None"
                    End If
                    selectlang.EditedLang = ""

                    cmdDelete.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("DeleteItem", LocalResourceFile) & "');")

                    If TemID = "" Then
                        pnlEdit.Visible = False
                    Else
                        populateEdit()
                    End If

                    ShowTreeMenu()

                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdCancel.Click
            Try
                If EditTemplateMode Then
                    Response.Redirect(EditUrl("etmode", "1", "AdminSetting"), True)
                Else
                    Response.Redirect(EditUrl("etmode", "0", "AdminSetting"), True)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdUpdate.Click
            Try
                Dim objInfo As NB_Store_SettingsTextInfo
                objInfo = updateSettings()
                If Not objInfo Is Nothing Then
                    selectlang.EditedLang = selectlang.SelectedLang
                    If EditTemplateMode Then
                        Response.Redirect(EditUrl("TemId", objInfo.SettingName.ToString, "AdminSetting", "etmode=1"), True)
                    Else
                        Response.Redirect(EditUrl("TemId", objInfo.SettingName.ToString, "AdminSetting", "etmode=0"), True)
                    End If
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdDelete.Click
            Try
                Dim objCtrl As New SettingsController
                Dim objInfo As NB_Store_SettingsTextInfo

                If EditTemplateMode Then
                    objInfo = objCtrl.GetSettingsTextNotCached(PortalId, TemID, selectlang.SelectedLang)
                    objCtrl.DeleteSettingsText(PortalId, TemID, selectlang.SelectedLang)
                Else
                    objInfo = objCtrl.GetSettingObjNotCached(PortalId, TemID, selectlang.SelectedLang)
                    objCtrl.DeleteSetting(PortalId, TemID, selectlang.SelectedLang)
                End If
                removeLangCache(PortalId, "nbstoreadminsettingtreemenu" & UserId.ToString)
                removeLangCache(PortalId, "nbstoreadmintemplatetreemenu" & UserId.ToString)
                If Not objInfo Is Nothing Then
                    UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
                    If objInfo.Lang.Trim = "None" Then
                        If EditTemplateMode Then
                            Response.Redirect(EditUrl("etmode", "1", "AdminSetting"), True)
                        Else
                            Response.Redirect(EditUrl("etmode", "0", "AdminSetting"), True)
                        End If
                    End If
                End If
                If EditTemplateMode Then
                    Response.Redirect(EditUrl("TemId", objInfo.SettingName.ToString, "AdminSetting", "etmode=1"), True)
                Else
                    Response.Redirect(EditUrl("TemId", objInfo.SettingName.ToString, "AdminSetting", "etmode=0"), True)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            Try
                If EditTemplateMode Then
                    Response.Redirect(EditUrl("TemId", "0", "AdminSetting", "etmode=1"), True)
                Else
                    Response.Redirect(EditUrl("TemId", "0", "AdminSetting", "etmode=0"), True)
                End If

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub selectlang_AfterChange(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal PreviousEditLang As String) Handles selectlang.AfterChange
            Select Case e.CommandName
                Case "Change"
                    ShowTreeMenu()
                    populateEdit()
            End Select
        End Sub

        Private Sub selectlang_BeforeChange(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs, ByVal NewEditLang As String) Handles selectlang.BeforeChange
            Select Case e.CommandName
                Case "Change"
                    'Dim objInfo As New NB_Store_SettingsTextInfo
                    'objInfo = updateSettings()
                    'If TemID = "0" Then
                    '    'new category created, so reset the url
                    '    If EditTemplateMode Then
                    '        Response.Redirect(EditUrl("TemID", objInfo.SettingName, "AdminSetting", "etmode=1"), True)
                    '    Else
                    '        Response.Redirect(EditUrl("TemID", objInfo.SettingName, "AdminSetting", "etmode=0"), True)
                    '    End If
                    'End If
            End Select
        End Sub

        Private Sub chkTextBoxEdit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkTextBoxEdit.CheckedChanged
            Try
                SetStoreSetting(PortalId, "textboxedit.flag", chkTextBoxEdit.Checked.ToString, "None", False)
                If EditTemplateMode Then
                    Response.Redirect(EditUrl("TemID", TemID, "AdminSetting", "etmode=1"), True)
                Else
                    Response.Redirect(EditUrl("TemID", TemID, "AdminSetting", "etmode=0"), True)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub chkUseFileSystem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkUseFileSystem.CheckedChanged
            Try
                SetStoreSetting(PortalId, "filesystemtemplates.flag", chkUseFileSystem.Checked.ToString, "None", False)

                If chkUseFileSystem.Checked Then
                    'filesystem turned on, so take DB and update the setting to the filesystem
                    Dim objInfo As NB_Store_SettingsTextInfo
                    objInfo = updateSettings()
                    If Not objInfo Is Nothing Then
                        selectlang.EditedLang = selectlang.SelectedLang
                    End If
                End If

                If EditTemplateMode Then
                    Response.Redirect(EditUrl("TemID", TemID, "AdminSetting", "etmode=1"), True)
                Else
                    Response.Redirect(EditUrl("TemID", TemID, "AdminSetting", "etmode=0"), True)
                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


#End Region

#Region "Update/View methods"

        Private Function updateSettings() As NB_Store_SettingsTextInfo
            Dim objCtrl As New SettingsController
            Dim objInfo As New NB_Store_SettingsTextInfo

            If EditTemplateMode Then
                objInfo = objCtrl.GetSettingsTextNotCached(PortalId, TemID, selectlang.SelectedLang)
            Else
                objInfo = objCtrl.GetSettingObjNotCached(PortalId, TemID, selectlang.SelectedLang)
            End If

            If objInfo Is Nothing Then
                objInfo = New NB_Store_SettingsTextInfo
            End If

            objInfo.Lang = selectlang.SelectedLang
            objInfo.PortalID = PortalId
            objInfo.SettingName = txtTemplName.Text

            If EditTemplateMode Then
                If txtMessage.Visible Then
                    objInfo.SettingText = System.Web.HttpUtility.HtmlEncode(txtMessage.Text)
                Else
                    objInfo.SettingText = CType(txtEditor, DotNetNuke.UI.UserControls.TextEditor).Text
                End If
            Else
                If txtMessage.Visible Then
                    objInfo.SettingValue = txtMessage.Text
                End If

                If rblSettingValue.Visible Then
                    objInfo.SettingValue = rblSettingValue.SelectedValue
                End If

                If ddlTabs.Visible Then
                    objInfo.SettingValue = ddlTabs.SelectedValue
                End If
            End If

            objInfo.GroupRef = ddlGroups.SelectedValue

            If rblCtrlType.Visible Then
                objInfo.CtrlType = rblCtrlType.SelectedValue
            End If

            objInfo.HostOnly = chkHostOnly.Checked

            If EditTemplateMode Then
                objCtrl.UpdateObjSettingsText(objInfo)
            Else
                objCtrl.UpdateObjSettingObj(objInfo)
            End If

            If Not objInfo Is Nothing Then
                UpdateLog(UserId, Me.ControlName & " - " & System.Reflection.MethodBase.GetCurrentMethod.Name & " - " & DotNetNuke.Common.Utilities.XmlUtils.Serialize(objInfo))
            End If
            removeStoreTemplateCache(PortalId)
            removeLangCache(PortalId, "nbstoreadminsettingtreemenu" & UserId.ToString)
            removeLangCache(PortalId, "nbstoreadmintemplatetreemenu" & UserId.ToString)

            Return objInfo
        End Function


        Private Sub populateEdit()
            Dim objCtrl As New SettingsController
            Dim li As ListItem
            Dim objInfo As NB_Store_SettingsTextInfo

            pnlEdit.Visible = True

            selectlang.DisplayNoLang = True

            If Page.IsPostBack Then
                selectlang.Refresh()
            End If

            txtMessage.Visible = False
            rblSettingValue.Visible = False
            txtEditor.Visible = False
            chkTextBoxEdit.Visible = False
            chkUseFileSystem.Visible = False
            ddlTabs.Visible = False
			
            If EditTemplateMode Then
                rblCtrlType.Visible = False
            End If


            PopulateGroupDropdownlist(ddlGroups)

            If EditTemplateMode Then
                objInfo = objCtrl.GetSettingsTextNotCached(PortalId, TemID, selectlang.SelectedLang)
            Else
                objInfo = objCtrl.GetSettingObjNotCached(PortalId, TemID, selectlang.SelectedLang)
            End If

            If Not objInfo Is Nothing Then

                If objInfo.GroupRef Is Nothing Then objInfo.GroupRef = ""

                If Not ddlGroups.Items.FindByValue(objInfo.GroupRef) Is Nothing Then
                    ddlGroups.ClearSelection()
                    ddlGroups.Items.FindByValue(objInfo.GroupRef).Selected = True
                End If

                txtHelp.Enabled = False

                If EditTemplateMode Then
                    'template edit only
                    Dim blnTextBox As Boolean
                    blnTextBox = GetStoreSettingBoolean(PortalId, "textboxedit.flag", "None")

                    chkTextBoxEdit.Visible = True
                    chkTextBoxEdit.Checked = blnTextBox

                    chkUseFileSystem.Visible = True
                    If GetStoreSettingBoolean(PortalId, "debug.mode", "None") Then
                        chkUseFileSystem.Enabled = True
                        chkUseFileSystem.Checked = GetStoreSettingBoolean(PortalId, "filesystemtemplates.flag", "None")
                    Else
                        chkUseFileSystem.Enabled = False
                        chkUseFileSystem.Checked = False
                    End If

                    If blnTextBox Or (GetStoreSettingBoolean(PortalId, "filesystemtemplates.flag", "None") And GetStoreSettingBoolean(PortalId, "debug.mode", "None")) Then
                        txtEditor.Visible = False
                        txtMessage.Visible = True
                        txtMessage.TextMode = TextBoxMode.MultiLine
                        txtMessage.Height = 400
                        txtMessage.Wrap = False
                        txtMessage.Text = System.Web.HttpUtility.HtmlDecode(objInfo.SettingText)
                    Else
                        txtEditor.Visible = True
                        txtMessage.Visible = False
                        CType(txtEditor, DotNetNuke.UI.UserControls.TextEditor).Text = objInfo.SettingText
                    End If

                    If GetStoreSettingBoolean(PortalId, "filesystemtemplates.flag", "None") And GetStoreSettingBoolean(PortalId, "debug.mode", "None") Then
                        txtEditor.Visible = False
                        txtMessage.Enabled = False
                    End If

                Else
                    'setting edit
                    If objInfo.CtrlType = "" Then
                        rblCtrlType.Visible = True
                        txtMessage.Visible = True
                        txtMessage.Visible = True
                        txtMessage.MaxLength = 2000
                        txtMessage.TextMode = TextBoxMode.MultiLine
                        txtMessage.Height = 400
                        txtMessage.Wrap = False
                        txtMessage.Text = objInfo.SettingValue
                    Else
                        rblCtrlType.Visible = False
                        Select Case objInfo.CtrlType.ToLower
                            Case "textbox"
                                txtMessage.Visible = True
                                txtMessage.MaxLength = 2000
                                txtMessage.TextMode = TextBoxMode.SingleLine
                                txtMessage.Text = objInfo.SettingValue
                            Case "boolean"
                                rblSettingValue.Visible = True
                                If rblSettingValue.Items.Count < 2 Then
                                    li = New ListItem
                                    li.Value = "1"
                                    li.Text = Localization.GetString("True", LocalResourceFile)
                                    rblSettingValue.Items.Add(li)
                                    li = New ListItem
                                    li.Value = "0"
                                    li.Text = Localization.GetString("False", LocalResourceFile)
                                    rblSettingValue.Items.Add(li)
                                End If

                                Try
                                    If CBool(objInfo.SettingValue) Then
                                        rblSettingValue.SelectedValue = "1"
                                    Else
                                        rblSettingValue.SelectedValue = "0"
                                    End If
                                Catch ex As Exception
                                    rblSettingValue.SelectedValue = "0"
                                End Try
                            Case "websitetab"
                                populateTabsList(ddlTabs, PortalSettings, "")

                                If Not ddlTabs.Items.FindByValue(objInfo.SettingValue) Is Nothing Then
                                    ddlTabs.ClearSelection()
                                    ddlTabs.Items.FindByValue(objInfo.SettingValue).Selected = True
                                End If
                                ddlTabs.Visible = True
                            Case Else
                                txtMessage.Visible = True
                                txtMessage.MaxLength = 2000
                                txtMessage.TextMode = TextBoxMode.MultiLine
                                txtMessage.Height = 400
                                txtMessage.Wrap = False
                                txtMessage.Text = objInfo.SettingValue
                        End Select
                    End If
                End If

                txtTemplName.Text = objInfo.SettingName
                chkHostOnly.Checked = objInfo.HostOnly

                txtHelp.Text = Localization.GetString(objInfo.SettingName, LocalResourceFile)

                If UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrator") Then
                    cmdAdd.Visible = True
                Else
                    cmdAdd.Visible = False
                End If


                If (UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrators")) Then
                    chkHostOnly.Enabled = True
                Else
                    chkHostOnly.Enabled = False
                    If objInfo.HostOnly Then
                        txtMessage.Enabled = False
                        txtTemplName.Enabled = False
                        cmdDelete.Visible = False
                        cmdUpdate.Visible = False
                    End If
                End If


            End If

        End Sub

#End Region

#Region "GroupList"

        Private Sub PopulateGroupDropdownlist(ByRef ddlGroups As DropDownList)
            Dim strXML As String = ""
            Dim xmlDoc As New XmlDataDocument

            If EditTemplateMode Then
                strXML = GetStoreSetting(PortalId, "admintemplates.groups", GetCurrentCulture)
            Else
                strXML = GetStoreSetting(PortalId, "adminsettings.groups", GetCurrentCulture)
            End If

            Try
                xmlDoc.LoadXml(strXML)
            Catch ex As Exception
                ddlGroups.Items.Add("ERROR - Invalid xml setting")
                Exit Sub
            End Try

            BuildGroupList(ddlGroups, xmlDoc, "root", "")

        End Sub

        Private Sub BuildGroupList(ByRef ddlGroups As DropDownList, ByVal xmlDoc As XmlDataDocument, ByVal xPath As String, ByVal LevelIndent As String)
            Dim xmlNodList As XmlNodeList
            Dim li As ListItem

            xmlNodList = xmlDoc.SelectSingleNode(xPath).ChildNodes()

            For Each xmlNod As XmlNode In xmlNodList
                li = New ListItem
                li.Text = LevelIndent & xmlNod.Attributes("text").InnerText
                li.Value = xPath & "/" & xmlNod.Name
                ddlGroups.Items.Add(li)
                BuildGroupList(ddlGroups, xmlDoc, xPath & "/" & xmlNod.Name, LevelIndent & ".")
            Next

        End Sub

#End Region


#Region "TreeView"
        Private Sub ShowTreeMenu()
            If EditTemplateMode Then
                ShowTreeMenu("nbstoreadmintemplatetreemenu" & UserId.ToString)
            Else
                ShowTreeMenu("nbstoreadminsettingtreemenu" & UserId.ToString)
            End If
        End Sub

        Private Sub ShowTreeMenu(ByVal CacheName As String)

            Dim objCtrl As New SettingsController
            Dim aryList As ArrayList
            Dim strHtml As String = ""
            Dim DebugMode As Boolean = GetStoreSettingBoolean(PortalId, "debug.mode", "None")
            Dim colMenuList As New Collection

            If getLangCache(PortalId, CacheName, GetCurrentCulture) Is Nothing Or DebugMode Then

                Dim strXML As String = ""
                Dim xmlDoc As New XmlDataDocument
                Dim blnErr As Boolean = False


                If EditTemplateMode Then
                    strXML = GetStoreSetting(PortalId, "admintemplates.groups", GetCurrentCulture)
                Else
                    strXML = GetStoreSetting(PortalId, "adminsettings.groups", GetCurrentCulture)
                End If

                Try
                    xmlDoc.LoadXml(strXML)
                Catch ex As Exception
                    phTreeMenu.Controls.Add(New LiteralControl("Invalid xml setting - ""adminsettings.groups"" "))
                    blnErr = True
                End Try


                Dim hFlagList As Hashtable
                If EditTemplateMode Then
                    aryList = objCtrl.GetSettingsTexts(PortalId, "None", (UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrators")), "")
                    hFlagList = BuildFlags("nbstoreadmintemplateflaglist" & UserId.ToString, aryList, TemID)
                Else
                    aryList = objCtrl.GetSettingListObj(PortalId, "None", (UserInfo.IsSuperUser Or UserInfo.IsInRole("Administrators")), "")
                    hFlagList = BuildFlags("nbstoreadminsettingflaglist" & UserId.ToString, aryList, TemID)
                End If

                For Each objInfo As NB_Store_SettingsTextInfo In aryList
                    colMenuList.Add("<div class=""NBright_SettingMenuListItem"">" & getSettingLink(objInfo, hFlagList) & "</div>")
                Next

                If Not blnErr Then
                    strHtml = BuildTreeMenu(xmlDoc, aryList, "root", hFlagList)
                End If
                setLangCache(PortalId, CacheName, GetCurrentCulture, strHtml)
                If EditTemplateMode Then
                    setLangCache(PortalId, "nbstoreadmintemplatelistmenu" & UserId.ToString, GetCurrentCulture, colMenuList)
                Else
                    setLangCache(PortalId, "nbstoreadminsettinglistmenu" & UserId.ToString, GetCurrentCulture, colMenuList)
                End If
            Else
                strHtml = getLangCache(PortalId, CacheName, GetCurrentCulture).ToString
                If EditTemplateMode Then
                    If Not getLangCache(PortalId, "nbstoreadmintemplatelistmenu" & UserId.ToString, GetCurrentCulture) Is Nothing Then
                        colMenuList = getLangCache(PortalId, "nbstoreadmintemplatelistmenu" & UserId.ToString, GetCurrentCulture)
                    End If
                Else
                    If Not getLangCache(PortalId, "nbstoreadminsettinglistmenu" & UserId.ToString, GetCurrentCulture) Is Nothing Then
                        colMenuList = getLangCache(PortalId, "nbstoreadminsettinglistmenu" & UserId.ToString, GetCurrentCulture)
                    End If
                End If
            End If


            phTreeMenu.Controls.Add(New LiteralControl(strHtml))

            'display listmenu if no edit
            If pnlEdit.Visible Then
                dlListMenu.Visible = False
            Else
                dlListMenu.DataSource = colMenuList
                dlListMenu.DataBind()
            End If


        End Sub

        Private Function BuildTreeMenu(ByVal xmlDoc As XmlDataDocument, ByRef aryList As ArrayList, ByVal xPath As String, ByVal hFlagList As Hashtable) As String
            Dim strHeader As String = ""
            Dim strFooter As String = "</ul>"
            Dim xmlNodList As XmlNodeList
            Dim htmlText As String = ""

            If xPath = "root" Then
                strHeader &= "<ul id=""AdminTreeMenu"" class=""NBright_TreeView"">"
            Else
                strHeader &= "<ul>"
            End If

            xmlNodList = xmlDoc.SelectSingleNode(xPath).ChildNodes()

            For Each xmlNod As XmlNode In xmlNodList

                htmlText &= "<li>"
                htmlText &= "<font class=""NBright_AdminTree"">" & xmlNod.Attributes("text").InnerText & "</font>"
                htmlText &= BuildAdminSettingLink(aryList, xPath & "/" & xmlNod.Name, hFlagList)
                htmlText &= BuildTreeMenu(xmlDoc, aryList, xPath & "/" & xmlNod.Name, hFlagList)
                htmlText &= "</li>"

            Next

            If xPath = "root" Then
                If aryList.Count > 0 Then

                    htmlText &= "<li>"
                    htmlText &= "<font class=""NBright_AdminTree"">Unknown</font>"
                    htmlText &= BuildAdminSettingLink(aryList, "", hFlagList)
                    htmlText &= "</li>"

                End If
            End If


            If htmlText <> "" Then
                htmlText = strHeader & htmlText & strFooter
            End If

            Return htmlText
        End Function

        Private Function BuildFlags(ByVal CacheName As String, ByVal aryList As ArrayList, Optional ByVal SettingName As String = "") As Hashtable
            Dim hFlagList As New Hashtable
            Dim objCtrl As New SettingsController
            Dim aryLangList As ArrayList
            Dim strHtml As String = ""

            If SettingName <> "" And Not getLangCache(PortalId, CacheName, GetCurrentCulture) Is Nothing Then

                hFlagList = getLangCache(PortalId, CacheName, GetCurrentCulture)

                If hFlagList.ContainsKey(SettingName) Then
                    hFlagList.Remove(SettingName)
                End If

                If EditTemplateMode Then
                    aryLangList = objCtrl.GetSettingsTexts(PortalId, "", True, SettingName)
                Else
                    aryLangList = objCtrl.GetSettingListObj(PortalId, "", True, SettingName)
                End If
                strHtml = "&nbsp;"
                For Each itemInfo2 As NB_Store_SettingsTextInfo In aryLangList
                    If itemInfo2.Lang.Trim <> "None" Then
                        strHtml &= "<img src=""" & DotNetNuke.Common.ResolveUrl("~/images/flags/" & itemInfo2.Lang.Trim & ".gif") & """ height=""10"" width=""15"" border=""0"" />"
                    End If
                Next
                hFlagList.Add(SettingName, strHtml)
            Else
                If getLangCache(PortalId, CacheName, GetCurrentCulture) Is Nothing Then

                    For Each itemInfo As NB_Store_SettingsTextInfo In aryList
                        If EditTemplateMode Then
                            aryLangList = objCtrl.GetSettingsTexts(PortalId, "", True, itemInfo.SettingName)
                        Else
                            aryLangList = objCtrl.GetSettingListObj(PortalId, "", True, itemInfo.SettingName)
                        End If

                        strHtml = "&nbsp;"
                        For Each itemInfo2 As NB_Store_SettingsTextInfo In aryLangList
                            If itemInfo2.Lang.Trim <> "None" Then
                                strHtml &= "<img src=""" & DotNetNuke.Common.ResolveUrl("~/images/flags/" & itemInfo2.Lang.Trim & ".gif") & """ height=""10"" width=""15"" border=""0"" />"
                            End If
                        Next
                        hFlagList.Add(itemInfo.SettingName, strHtml)
                    Next
                    setLangCache(PortalId, CacheName, GetCurrentCulture, hFlagList)
                Else
                    hFlagList = getLangCache(PortalId, CacheName, GetCurrentCulture)
                End If
            End If

            Return hFlagList
        End Function

        Private Function BuildAdminSettingLink(ByRef aryList As ArrayList, ByVal GroupName As String, ByVal hFlagList As Hashtable) As String
            Dim itemInfo As NB_Store_SettingsTextInfo
            Dim strHtmlText As String
            Dim aryRemove As New ArrayList

            strHtmlText = "<ul>"

            For Each itemInfo In aryList
                If itemInfo.GroupRef Is Nothing Then itemInfo.GroupRef = ""
                If itemInfo.GroupRef = GroupName Or GroupName = "" Then

                    strHtmlText &= "<li>"

                    strHtmlText &= getSettingLink(itemInfo, hFlagList)

                    aryRemove.Add(itemInfo)

                    strHtmlText &= "</li>"
                End If
            Next

            strHtmlText &= "</ul>"

            'remove allocated settings
            For Each itemInfo In aryRemove
                aryList.Remove(itemInfo)
            Next

            Return strHtmlText

        End Function

        Private Function getSettingLink(ByVal itemInfo As NB_Store_SettingsTextInfo, ByVal hFlagList As Hashtable) As String
            Dim strHtmlLink As String = ""
            Dim strHtmlText As String = ""

            If EditTemplateMode Then
                strHtmlLink = EditUrl("TemID", itemInfo.SettingName, "AdminSetting", "etmode=1")
            Else
                strHtmlLink = EditUrl("TemID", itemInfo.SettingName, "AdminSetting", "etmode=0")
            End If

            strHtmlText &= "<a class=""NBright_AdminTreeSetting"" href=""" & strHtmlLink & """ >" & itemInfo.SettingName & "</a>" & hFlagList(itemInfo.SettingName)

            If itemInfo.HostOnly Then
                strHtmlText &= "<img src=""" & Me.ResolveUrl("img/bullet_key.png") & """ border=""0"" />"
            Else
                strHtmlText &= "<img src=""" & Me.ResolveUrl("img/bullet_wrench.png") & """ border=""0"" />"
            End If


            Return strHtmlText
        End Function

#End Region


    End Class

End Namespace
