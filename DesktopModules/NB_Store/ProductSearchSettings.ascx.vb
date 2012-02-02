' --- Copyright (c) notice NevoWeb ---
'  Copyright (c) 2008 SARL NevoWeb.  www.nevoweb.com. All rights are reserved.
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
Imports System.Text.RegularExpressions
Imports System.Web
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class ProductSearchSettings
        Inherits Entities.Modules.ModuleSettingsBase


#Region "Base Method Implementations"

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' LoadSettings loads the settings from the Database and displays them
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[dclee]	05/12/2008	Nevoweb - conversion to NB_Store search
        '''		[cnurse]	11/30/2004	converted to SettingsBase
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub LoadSettings()
            Try
                If (Page.IsPostBack = False) Then

                    Dim SearchTabID As String = CType(ModuleSettings("SearchResultsModule"), String)
                    Dim ShowGoImage As String = CType(ModuleSettings("ShowGoImage"), String)
                    Dim ShowSearchImage As String = CType(ModuleSettings("ShowSearchImage"), String)

                    If SearchTabID = "" Then
                        SearchTabID = TabId.ToString
                    End If

                    populateTabsList(cboModule, PortalSettings, SearchTabID)

                    If Not cboModule.Items.FindByValue(SearchTabID) Is Nothing Then
                        cboModule.Items.FindByValue(SearchTabID).Selected = True
                    End If
                    If cboModule.Items.Count > 0 Then
                        txtModule.Text = cboModule.SelectedItem.Text
                    Else
                        txtModule.Text = Localization.GetString("NoSearchModule", LocalResourceFile)
                    End If

                    If Not ShowGoImage Is Nothing Then
                        chkGo.Checked() = CType(ShowGoImage, Boolean)
                    End If

                    If Not ShowSearchImage Is Nothing Then
                        chkSearchImage.Checked() = CType(ShowSearchImage, Boolean)
                    End If

                    txtImagePath.Text = CType(ModuleSettings("txtImagePath"), String)
                    txtSImagePath.Text = CType(ModuleSettings("txtSImagePath"), String)

                End If
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        ''' -----------------------------------------------------------------------------
        ''' <summary>
        ''' UpdateSettings saves the modified settings to the Database
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        ''' <history>
        '''		[cnurse]	11/30/2004	converted to SettingsBase
        ''' </history>
        ''' -----------------------------------------------------------------------------
        Public Overrides Sub UpdateSettings()
            Try
                Dim objModules As New Entities.Modules.ModuleController

                If Not cboModule.SelectedIndex = -1 Then
                    objModules.UpdateModuleSetting(Me.ModuleId, "SearchResultsModule", cboModule.SelectedItem.Value)
                End If

                objModules.UpdateModuleSetting(Me.ModuleId, "ShowGoImage", chkGo.Checked.ToString)
                objModules.UpdateModuleSetting(Me.ModuleId, "ShowSearchImage", chkSearchImage.Checked.ToString)
                objModules.UpdateModuleSetting(Me.ModuleId, "txtImagePath", txtImagePath.Text)
                objModules.UpdateModuleSetting(Me.ModuleId, "txtSImagePath", txtSImagePath.Text)

            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

#End Region


    End Class


End Namespace
