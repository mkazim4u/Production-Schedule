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


Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class ManagerMenuController
        Inherits BasePortController

        Public Overrides Function ExportModule(ByVal ModuleID As Integer) As String
            Return ExportModuleData(ModuleID, Nothing, Nothing, Nothing, True, True)
        End Function

        Public Function ExportModuleData(ByVal ModuleID As Integer, ByVal chkSettingsList As CheckBoxList, ByVal chkTemplateList As CheckBoxList, ByVal chkLangList As CheckBoxList, ByVal ExportStatusCodes As Boolean, ByVal ExportReports As Boolean) As String
            Dim strXML As String = ""
            Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
            Dim Exp As New Export

            Dim LangList As New Hashtable
            If chkLangList Is Nothing Then
                LangList.Add(GetCurrentCulture, GetCurrentCulture)
            Else
                For Each li As ListItem In chkLangList.Items
                    If li.Selected Then
                        LangList.Add(li.Value, li.Value)
                    End If
                Next
            End If

            strXML += "<storecontent>"
            strXML += Exp.GetSettingsXML(PS.PortalId, chkSettingsList, LangList)
            strXML += Exp.GetSettingsTextXML(PS.PortalId, chkTemplateList, LangList)
            If ExportStatusCodes Then
                strXML += Exp.GetStatusXML()
            End If
            If ExportReports Then
                strXML += Exp.GetSQLReports(PS.PortalId)
            End If
            strXML += "</storecontent>"

            Return strXML

        End Function

        Public Overrides Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer)
            ImportModuleData(ModuleID, Content, Version, UserId, Nothing, Nothing, Nothing)
        End Sub

        Public Sub ImportModuleData(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer, ByVal chkSettingsList As CheckBoxList, ByVal chkTemplateList As CheckBoxList, ByVal chkLangList As CheckBoxList)
            Dim xmlText As XmlNode = GetContent(Content, "storecontent")
            Dim xmlSettings As XmlNode
            Dim xmlSetNod As XmlNode
            Dim objOCtrl As New OrderController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim objOSInfo As NB_Store_OrderStatusInfo
            Dim PortalID As Integer
            Dim blnForcedOverwrite As Boolean = False

            Dim objModules As New Entities.Modules.ModuleController
            Dim objModInfo As Entities.Modules.ModuleInfo

            objModInfo = objModules.GetModule(ModuleID, -1)
            If objModInfo Is Nothing Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                PortalID = PS.PortalId
            Else
                PortalID = objModInfo.PortalID
            End If

            Dim LangList As New Hashtable
            If chkLangList Is Nothing Then
                LangList.Add(GetCurrentCulture, GetCurrentCulture)
            Else
                For Each li As ListItem In chkLangList.Items
                    If li.Selected Then
                        LangList.Add(li.Value, li.Value)
                    End If
                Next
            End If

            xmlSettings = xmlText.SelectSingleNode("settings")

            If Not xmlSettings Is Nothing Then
                For Each xmlSetNod In xmlSettings.SelectNodes("*")
                    objSInfo = New NB_Store_SettingsInfo
                    objSInfo.PortalID = PortalID
                    objSInfo.Lang = xmlSetNod.Attributes("Lang").Value
                    objSInfo.HostOnly = xmlSetNod.Attributes("HostOnly").Value
                    objSInfo.SettingName = xmlSetNod.Name
                    objSInfo.SettingValue = xmlSettings.SelectSingleNode(xmlSetNod.Name & "[@Lang=""" & xmlSetNod.Attributes("Lang").Value & """]").InnerText

                    If Not xmlSetNod.Attributes("CtrlType") Is Nothing Then
                        objSInfo.CtrlType = xmlSetNod.Attributes("CtrlType").Value
                    Else
                        objSInfo.CtrlType = ""
                    End If

                    If Not xmlSetNod.Attributes("GroupRef") Is Nothing Then
                        objSInfo.GroupRef = xmlSetNod.Attributes("GroupRef").Value
                    Else
                        objSInfo.GroupRef = ""
                    End If

                    If Not xmlSetNod.Attributes("HostOnly") Is Nothing Then
                        objSInfo.HostOnly = xmlSetNod.Attributes("HostOnly").Value
                    Else
                        objSInfo.HostOnly = ""
                    End If

                    If Not xmlSetNod.Attributes("Overwrite") Is Nothing Then
                        blnForcedOverwrite = CBool(xmlSetNod.Attributes("Overwrite").Value)
                    Else
                        blnForcedOverwrite = False
                    End If

                    If chkSettingsList Is Nothing Then
                        updateSettingData(PortalID, objSInfo, blnForcedOverwrite)
                    Else
                        If Not chkSettingsList.Items.FindByValue(objSInfo.SettingName) Is Nothing Then
                            If chkSettingsList.Items.FindByValue(objSInfo.SettingName).Selected And LangList.Contains(objSInfo.Lang.Trim) Then
                                updateSettingData(PortalID, objSInfo, blnForcedOverwrite)
                            End If
                        End If
                    End If

                Next
            End If

            xmlSettings = xmlText.SelectSingleNode("settingstext")

            If Not xmlSettings Is Nothing Then
                For Each xmlSetNod In xmlSettings.SelectNodes("*")
                    objSTInfo = New NB_Store_SettingsTextInfo
                    objSTInfo.PortalID = PortalID
                    objSTInfo.Lang = xmlSetNod.Attributes("Lang").Value
                    objSTInfo.SettingName = xmlSetNod.Name
                    objSTInfo.SettingText = xmlSettings.SelectSingleNode(xmlSetNod.Name & "[@Lang=""" & xmlSetNod.Attributes("Lang").Value & """]").InnerText

                    If Not xmlSetNod.Attributes("CtrlType") Is Nothing Then
                        objSTInfo.CtrlType = xmlSetNod.Attributes("CtrlType").Value
                    Else
                        objSTInfo.CtrlType = ""
                    End If

                    If Not xmlSetNod.Attributes("GroupRef") Is Nothing Then
                        objSTInfo.GroupRef = xmlSetNod.Attributes("GroupRef").Value
                    Else
                        objSTInfo.GroupRef = ""
                    End If

                    If Not xmlSetNod.Attributes("HostOnly") Is Nothing Then
                        objSTInfo.HostOnly = xmlSetNod.Attributes("HostOnly").Value
                    Else
                        objSTInfo.HostOnly = False
                    End If

                    If Not xmlSetNod.Attributes("Overwrite") Is Nothing Then
                        blnForcedOverwrite = CBool(xmlSetNod.Attributes("Overwrite").Value)
                    Else
                        blnForcedOverwrite = False
                    End If

                    If chkTemplateList Is Nothing Then
                        updateSettingTextData(PortalID, objSTInfo, blnForcedOverwrite)
                    Else
                        If Not chkTemplateList.Items.FindByValue(objSTInfo.SettingName) Is Nothing Then
                            If chkTemplateList.Items.FindByValue(objSTInfo.SettingName).Selected And LangList.Contains(objSTInfo.Lang.Trim) Then
                                updateSettingTextData(PortalID, objSTInfo, blnForcedOverwrite)
                            End If
                        End If
                    End If

                Next
            End If

            xmlSettings = xmlText.SelectSingleNode("status")

            If Not xmlSettings Is Nothing Then
                For Each xmlSetNod In xmlSettings.SelectNodes("*")
                    objOSInfo = New NB_Store_OrderStatusInfo
                    objOSInfo.OrderStatusID = CInt(Replace(xmlSetNod.Name, "ID", ""))
                    objOSInfo.Lang = xmlSetNod.Attributes("Lang").Value
                    objOSInfo.OrderStatusText = xmlSettings.SelectSingleNode(xmlSetNod.Name & "[@Lang=""" & xmlSetNod.Attributes("Lang").Value & """]").InnerText
                    objOSInfo.ListOrder = xmlSetNod.Attributes("ListOrder").Value
                    objOCtrl.UpdateObjOrderStatus(objOSInfo)
                Next
            End If

            'load up any default reports found in settings file.
            xmlSettings = xmlText.SelectSingleNode("SQLReports")
            If Not xmlSettings Is Nothing Then
                Dim objImp As New Import
                objImp.ImportSQLReports(PortalID, xmlSettings, False)
            End If

        End Sub


        Private Sub updateSettingData(ByVal PortalID As Integer, ByVal objSInfo As NB_Store_SettingsInfo, ByVal ForcedOverwrite As Boolean)
            Dim objSCtrl As New SettingsController
            Dim objSInfo2 As NB_Store_SettingsInfo

            If GetStoreSettingBoolean(PortalID, "settings.overwrite", "None") Or ForcedOverwrite Then
                objSCtrl.UpdateObjSetting(objSInfo)
            Else
                objSInfo2 = objSCtrl.GetSetting(PortalID, objSInfo.SettingName, objSInfo.Lang)
                If objSInfo2 Is Nothing Then
                    objSCtrl.UpdateObjSetting(objSInfo)
                Else
                    If objSInfo2.Lang.Trim(" ") <> objSInfo.Lang.Trim(" ") Then
                        objSCtrl.UpdateObjSetting(objSInfo)
                    End If
                End If
            End If

        End Sub


        Private Sub updateSettingTextData(ByVal PortalID As Integer, ByVal objSTInfo As NB_Store_SettingsTextInfo, ByVal ForcedOverwrite As Boolean)
            Dim objSCtrl As New SettingsController
            Dim objSTInfo2 As NB_Store_SettingsTextInfo

            If GetStoreSettingBoolean(PortalID, "settings.overwrite", "None") Or ForcedOverwrite Then
                objSCtrl.UpdateObjSettingsText(objSTInfo)
            Else
                objSTInfo2 = objSCtrl.GetSettingsText(PortalID, objSTInfo.SettingName, objSTInfo.Lang)
                If objSTInfo2 Is Nothing Then
                    objSCtrl.UpdateObjSettingsText(objSTInfo)
                Else
                    If objSTInfo2.Lang.Trim(" ") <> objSTInfo.Lang.Trim(" ") Then
                        objSCtrl.UpdateObjSettingsText(objSTInfo)
                    End If
                End If
            End If

        End Sub
    End Class

End Namespace
