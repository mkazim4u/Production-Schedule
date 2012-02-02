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

    Public Class Upgrade


        Public Function DoUpgrade(ByVal PortalID As Integer, ByVal StoreInstallPath As String, ByVal UserId As Integer) As String
            Dim rtnMsg As String = ""
            Dim objSCtrl As New SettingsController
            Dim StorePortalVerison As String = GetStoreSetting(PortalID, "version", "None")
            Dim StoreCurentVerison As String = getCurrentStoreVersion(StoreInstallPath)

            If VersionCompare(StorePortalVerison, StoreCurentVerison) <> 0 Then

                '---------------------------------------------------------------------------
                'set overwrite to false, so we don't automatically overwrite any settings
                '---------------------------------------------------------------------------
                If GetStoreSettingBoolean(PortalID, "settings.overwrite") Then
                    SetStoreSetting(PortalID, "settings.overwrite", "0", "None", True)
                End If
                '---------------------------------------------------------------------------

                '---------------------------------------------------------------------------
                ' If standard shipping doesn;t exist then create it.
                '---------------------------------------------------------------------------
                If GetDefaultShipMethod(PortalID) = -1 Then
                    'create standard shipping method
                    Dim objSHCtrl As New ShipController
                    Dim objSMInfo As New NB_Store_ShippingMethodInfo
                    objSMInfo.Disabled = False
                    objSMInfo.MethodDesc = "Standard Shipping"
                    objSMInfo.MethodName = "Standard"
                    objSMInfo.SortOrder = 1
                    objSMInfo.PortalID = PortalID
                    objSMInfo.ShipMethodID = -1
                    objSMInfo.URLtracker = ""
                    objSMInfo.TemplateName = ""
                    objSHCtrl.UpdateObjShippingMethod(objSMInfo)
                End If
                '---------------------------------------------------------------------------

                '---------------------------------------------------------------------------
                ' Do the module import
                '---------------------------------------------------------------------------
                Dim objImpCtrl As New ManagerMenuController
                Dim xmlDoc As New XmlDataDocument
                Dim objModules As New Entities.Modules.ModuleController
                Dim objModInfo As Entities.Modules.ModuleInfo
                Dim supportedLanguages As LocaleCollection = GetValidLocales()
                Dim fName As String

                objModInfo = objModules.GetModuleByDefinition(PortalID, "NB_Store_BackOffice")

                xmlDoc.Load(System.Web.Hosting.HostingEnvironment.MapPath(StoreInstallPath & "templates\ManagerMenuDefault.xml"))
                objImpCtrl.ImportModule(objModInfo.ModuleID, xmlDoc.SelectSingleNode("content").InnerXml, "1", UserId)

                'update language files if they exist in template directory.
                If StorePortalVerison = "" Then
                    'Only want to do the import of lanague on new installation.
                    'Otherwise, default settings being used may be susceeded by the language version.
                    For Each Lang As String In supportedLanguages
                        Try
                            fName = System.Web.Hosting.HostingEnvironment.MapPath(StoreInstallPath & "templates\ManagerMenuDefault_" & Lang & ".xml")
                            If System.IO.File.Exists(fName) Then
                                xmlDoc = New XmlDataDocument
                                xmlDoc.Load(fName)
                                objImpCtrl.ImportModule(objModInfo.ModuleID, xmlDoc.SelectSingleNode("content").InnerXml, "1", UserId)
                            End If
                        Catch ex As Exception
                            'file test may fail on medium trust, ignore! Language update can be done manually
                        End Try
                    Next
                End If
                '---------------------------------------------------------------------------

                setStoreVersionToCurrent(PortalID, StoreInstallPath)

            End If

            Return rtnMsg
        End Function

        Public Function getCurrentStoreVersion(ByVal StoreInstallPath As String) As String
            Dim rtnStr As String
            Dim xmlDoc As New XmlDataDocument
            xmlDoc.Load(System.Web.Hosting.HostingEnvironment.MapPath(StoreInstallPath & "templates\ManagerMenuDefault.xml"))
            Try
                rtnStr = xmlDoc.SelectSingleNode("content").Attributes("version").InnerText
            Catch ex As Exception
                rtnStr = "00.00.00"
            End Try
            Return rtnStr
        End Function

        Public Sub setStoreVersionToCurrent(ByVal PortalID As Integer, ByVal StoreInstallPath As String)
            SetStoreSetting(PortalID, "version", getCurrentStoreVersion(StoreInstallPath), "None", True)
        End Sub

    End Class

End Namespace
