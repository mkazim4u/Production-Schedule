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

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class SettingsController

#Region "NB_Store_Settings Public Methods"

        Public Sub CopySettingToLanguages(ByVal objInfo As NB_Store_SettingsInfo)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopySettingToLanguages(objInfo, L)
            Next
        End Sub

        Public Sub CopySettingToLanguages(ByVal objInfo As NB_Store_SettingsInfo, ByVal Lang As String)
            objInfo.Lang = Lang
            UpdateObjSetting(objInfo)
        End Sub

        Public Sub DeleteSetting(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String)
            DataProvider.Instance().DeleteNB_Store_Settings(PortalID, SettingName, Lang)
            removeLangCache(PortalID, SettingName.ToLower)
        End Sub

        Public Function GetSetting(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As NB_Store_SettingsInfo
            Dim objS As NB_Store_SettingsInfo

            If getLangCache(PortalID, SettingName.ToLower, Lang) Is Nothing Then
                objS = CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Settings(PortalID, SettingName, Lang), GetType(NB_Store_SettingsInfo)), NB_Store_SettingsInfo)
                If Not objS Is Nothing Then
                    setLangCache(PortalID, SettingName.ToLower, Lang, objS)
                End If
            Else
                objS = CType(getLangCache(PortalID, SettingName.ToLower, Lang), NB_Store_SettingsInfo)
            End If
            Return objS
        End Function

        Public Function GetSettingObjNotCached(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As NB_Store_SettingsTextInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Settings(PortalID, SettingName, Lang), GetType(NB_Store_SettingsTextInfo)), NB_Store_SettingsTextInfo)
        End Function

        Public Function GetSettingNotCached(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As NB_Store_SettingsInfo
            Return CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_Settings(PortalID, SettingName, Lang), GetType(NB_Store_SettingsInfo)), NB_Store_SettingsInfo)
        End Function

        Public Function GetSettingListObj(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Settingss(PortalID, Lang, IsHost, SettingName), GetType(NB_Store_SettingsTextInfo))
        End Function

        Public Function GetSettingList(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_Settingss(PortalID, Lang, IsHost, SettingName), GetType(NB_Store_SettingsInfo))
        End Function

        Public Sub UpdateObjSetting(ByVal objInfo As NB_Store_SettingsInfo)
            DataProvider.Instance().UpdateNB_Store_Settings(objInfo.PortalID, objInfo.SettingName, objInfo.Lang, objInfo.SettingValue, objInfo.HostOnly, objInfo.GroupRef, objInfo.CtrlType)
            removeLangCache(objInfo.PortalID, objInfo.SettingName.ToLower)
        End Sub

        Public Sub UpdateObjSettingObj(ByVal objInfo As NB_Store_SettingsTextInfo)
            DataProvider.Instance().UpdateNB_Store_Settings(objInfo.PortalID, objInfo.SettingName, objInfo.Lang, objInfo.SettingValue, objInfo.HostOnly, objInfo.GroupRef, objInfo.CtrlType)
            removeLangCache(objInfo.PortalID, objInfo.SettingName.ToLower)
        End Sub

#End Region


#Region "NB_Store_SettingsText Public Methods"

        Public Sub CopyToLanguages(ByVal objInfo As NB_Store_SettingsTextInfo)
            Dim supportedLanguages As LocaleCollection = GetValidLocales()
            For Each L As String In supportedLanguages
                CopyToLanguages(objInfo, L)
            Next
        End Sub

        Public Sub CopyToLanguages(ByVal objInfo As NB_Store_SettingsTextInfo, ByVal Lang As String)
            objInfo.Lang = Lang
            UpdateObjSettingsText(objInfo)
        End Sub


        Public Sub DeleteSettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String)
            DataProvider.Instance().DeleteNB_Store_SettingsText(PortalID, SettingName, Lang)
            removeLangCache(PortalID, "TX_" & SettingName.ToLower)

            Dim DebugMode As Boolean = GetStoreSettingBoolean(PortalID, "debug.mode", "None")
            Dim FSTemplates As Boolean = GetStoreSettingBoolean(PortalID, "filesystemtemplates.flag", "None")
            If DebugMode And FSTemplates Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                Try
                    If Lang.Trim.ToLower = "none" Then
                        FileSystemUtils.DeleteFile(PS.HomeDirectoryMapPath & TEMPLATEFOLDER & "\" & SettingName & "")
                    Else
                        FileSystemUtils.DeleteFile(PS.HomeDirectoryMapPath & TEMPLATEFOLDER & "\" & SettingName & "_" & Lang & "")
                    End If
                Catch ex As Exception
                    'file delete may fail because of file lock..ignore.
                End Try
            End If
        End Sub

        Public Function GetSettingsText(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As NB_Store_SettingsTextInfo
            Dim objS As NB_Store_SettingsTextInfo
            Dim DebugMode As Boolean = GetStoreSettingBoolean(PortalID, "debug.mode", "None")

            If getLangCache(PortalID, "TX_" & SettingName.ToLower, Lang) Is Nothing Or DebugMode Then
                objS = CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_SettingsText(PortalID, SettingName, Lang), GetType(NB_Store_SettingsTextInfo)), NB_Store_SettingsTextInfo)
                If Not objS Is Nothing Then
                    objS = GetFileSystemTemplate(objS)
                    setLangCache(PortalID, "TX_" & SettingName.ToLower, Lang, objS)
                End If
            Else
                objS = CType(getLangCache(PortalID, "TX_" & SettingName.ToLower, Lang), NB_Store_SettingsTextInfo)
            End If
            Return objS
        End Function

        Public Function GetSettingsTextNotCached(ByVal PortalID As Integer, ByVal SettingName As String, ByVal Lang As String) As NB_Store_SettingsTextInfo
            Dim objS As NB_Store_SettingsTextInfo

            objS = CType(CBO.FillObject(DataProvider.Instance().GetNB_Store_SettingsText(PortalID, SettingName, Lang), GetType(NB_Store_SettingsTextInfo)), NB_Store_SettingsTextInfo)
            If Not objS Is Nothing Then
                objS = GetFileSystemTemplate(objS)
            End If

            Return objS
        End Function

        Public Function GetSettingsTexts(ByVal PortalID As Integer, ByVal Lang As String, ByVal IsHost As Boolean, ByVal SettingName As String) As ArrayList
            Return CBO.FillCollection(DataProvider.Instance().GetNB_Store_SettingsTexts(PortalID, Lang, IsHost, SettingName), GetType(NB_Store_SettingsTextInfo))
        End Function

        Public Sub UpdateObjSettingsTextNoCache(ByVal objInfo As NB_Store_SettingsTextInfo)
            DataProvider.Instance().UpdateNB_Store_SettingsText(objInfo.PortalID, objInfo.SettingName, objInfo.Lang, objInfo.SettingText, objInfo.HostOnly, objInfo.GroupRef, objInfo.CtrlType)
        End Sub

        Public Sub UpdateObjSettingsText(ByVal objInfo As NB_Store_SettingsTextInfo)
            DataProvider.Instance().UpdateNB_Store_SettingsText(objInfo.PortalID, objInfo.SettingName, objInfo.Lang, objInfo.SettingText, objInfo.HostOnly, objInfo.GroupRef, objInfo.CtrlType)

            Dim DebugMode As Boolean = GetStoreSettingBoolean(objInfo.PortalID, "debug.mode", "None")
            Dim FSTemplates As Boolean = GetStoreSettingBoolean(objInfo.PortalID, "filesystemtemplates.flag", "None")
            If DebugMode And FSTemplates Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                Dim objF As New FileObj
                CreateDir(PS, TEMPLATEFOLDER)
                If objInfo.Lang.Trim.ToLower = "none" Then
                    objF.SaveTextToFile(System.Web.HttpUtility.HtmlDecode(objInfo.SettingText), PS.HomeDirectoryMapPath & TEMPLATEFOLDER & "\" & objInfo.SettingName & "")
                Else
                    objF.SaveTextToFile(System.Web.HttpUtility.HtmlDecode(objInfo.SettingText), PS.HomeDirectoryMapPath & TEMPLATEFOLDER & "\" & objInfo.SettingName & "_" & objInfo.Lang & "")
                End If
            End If

            removeLangCache(objInfo.PortalID, "TX_" & objInfo.SettingName.ToLower)
        End Sub

        Public Function GetTemplateTable(ByVal PortalID As Integer, ByVal Lang As String) As Hashtable
            Dim aryList As ArrayList
            aryList = GetSettingsTexts(PortalID, "", True, "")
            Return GetTemplateTable(aryList, Lang)
        End Function


        Public Function GetTemplateTable(ByVal PortalID As Integer) As Hashtable
            Return GetTemplateTable(PortalID, "None")
        End Function

        Public Function GetTemplateTable(ByVal aryList As ArrayList, ByVal Lang As String) As Hashtable
            Dim hTable As New Hashtable
            Dim objInfo As NB_Store_SettingsTextInfo
            For Each objInfo In aryList
                If Trim(objInfo.Lang) = Trim(Lang) Then
                    hTable.Add(objInfo.SettingName, objInfo.SettingName & "(" & objInfo.Lang & ")")
                End If
            Next

            Return hTable
        End Function

        Private Function GetFileSystemTemplate(ByVal objInfo As NB_Store_SettingsTextInfo) As NB_Store_SettingsTextInfo
            If Not objInfo Is Nothing Then
                Dim DebugMode As Boolean = GetStoreSettingBoolean(objInfo.PortalID, "debug.mode", "None")
                Dim FSTemplates As Boolean = GetStoreSettingBoolean(objInfo.PortalID, "filesystemtemplates.flag", "None")

                If DebugMode And FSTemplates Then
                    Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                    Dim objF As New FileObj
                    Dim strIN As String = ""
                    If objInfo.Lang.Trim.ToLower = "none" Then
                        strIN = System.Web.HttpUtility.HtmlEncode(objF.GetFileContents(PS.HomeDirectoryMapPath & TEMPLATEFOLDER & "\" & objInfo.SettingName & ""))
                    Else
                        strIN = System.Web.HttpUtility.HtmlEncode(objF.GetFileContents(PS.HomeDirectoryMapPath & TEMPLATEFOLDER & "\" & objInfo.SettingName & "_" & objInfo.Lang & ""))
                    End If
                    If strIN <> "" Then
                        objInfo.SettingText = strIN
                        'update the DB with the filesystem template (don't use cache or filesystem)
                        UpdateObjSettingsTextNoCache(objInfo)
                    End If
                End If
            End If

            Return objInfo
        End Function

#End Region




    End Class

End Namespace
