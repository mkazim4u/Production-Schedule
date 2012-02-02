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

Namespace NEvoWeb.Modules.NB_Store

    ''' -----------------------------------------------------------------------------
    ''' <summary>
    ''' The MenuManager class displays the content
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <history>
    ''' </history>
    ''' -----------------------------------------------------------------------------
    Partial Class MenuManager
        Inherits BaseModule
        Implements Entities.Modules.IPortable

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
                Dim objSCtrl As New SettingsController
                Dim errMsg As String = ""
                Dim objUpg As New Upgrade
                Dim StorePortalVerison As String = GetStoreSetting(PortalId, "version", "None")

                If StorePortalVerison = "" Then
                    'No version so do First time install

                    'check the product images and doc directories exist.
                    CreateDir(PortalSettings, PRODUCTIMAGESFOLDER)
                    CreateDir(PortalSettings, PRODUCTDOCSFOLDER)

                    'copy noimage to portal root
                    Try
                        Dim objR As New ImgReSize
                        'Use this method to try and get over medium trust issues
                        FileSystemUtils.SaveFile(PortalSettings.HomeDirectoryMapPath & "noimage.png", objR.BmpToBytes_MemStream(EmbeddedImage("noimage.png"), 72, 4))
                    Catch ex As Exception
                        'do nothing 
                    End Try

                    errMsg = objUpg.DoUpgrade(PortalId, StoreInstallPath, UserId)
                End If

                If VersionCompare(StorePortalVerison, ModuleConfiguration.DesktopModule.Version) <> 0 Then
                    errMsg = objUpg.DoUpgrade(PortalId, StoreInstallPath, UserId)
                End If

                'Check the store setting were upgraded.
                StorePortalVerison = GetStoreSetting(PortalId, "version", "None")
                If VersionCompare(StorePortalVerison, ModuleConfiguration.DesktopModule.Version) <> 0 Then
                    errMsg = "NB_Store Portal Version " & StorePortalVerison & " mismatch with NB_Store Module Version " & ModuleConfiguration.Version
                End If


                If errMsg <> "" Then
                    PlaceHolder1.Controls.Add(New LiteralControl(System.Web.HttpUtility.HtmlDecode("<br />" & errMsg)))
                End If
                '----------------------------------------------------------

                DisplayMsgText(PortalId, PlaceHolder1, "backoffice.text", "")
                Me.Controls.Add(New AdminMenu)

            Catch exc As Exception        'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub




#End Region


#Region "Optional Interfaces"

        Public Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Function

        Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserID As Integer) Implements Entities.Modules.IPortable.ImportModule
            ' included as a stub only so that the core knows this module Implements Entities.Modules.IPortable
        End Sub

#End Region

    End Class

End Namespace
