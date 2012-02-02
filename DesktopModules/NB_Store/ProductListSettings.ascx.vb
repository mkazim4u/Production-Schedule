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

Imports DotNetNuke
Imports DotNetNuke.Services.Exceptions
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class ProductListSettings
        Inherits BaseModuleSettings


        Public Overrides Sub LoadSettings()
            Try

                If (Page.IsPostBack = False) Then

                    If ModuleSettings.Count = 0 Then
                        DoReset()
                    End If

                    populateTemplateList(PortalId, ddlTemplate, ".template")

                    populateTemplateList(PortalId, ddlAlterTemplate, ".template")

                    populateTemplateList(PortalId, ddlDetailTemplate, ".template")

                    populateTabsList(lstTabs, PortalSettings, TabId.ToString)

                    populateTabsList(lstProductTabs, PortalSettings, TabId.ToString)

                    populateTabsList(ddlTabDefaultRedirect, PortalSettings, TabId.ToString)

                    populateTemplateList(PortalId, ddlHeaderText, ".text", "-1", "", "-1")

                    populateTemplateList(PortalId, ddlListHeaderText, ".text", "-1", "", "-1")

                    populateTemplateList(PortalId, ddlDetailHeaderText, ".text", "-1", "", "-1")


                    'product option settings

                    populateCategoryList(PortalId, ddlDefaultCategory, -1, "All Categories", "")

                    LocalizeDDL(ddlDefaultOrder, Localization.GetString("DefaultOrderList", LocalResourceFile))

                    LoadBaseSettings()

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Overrides Sub UpdateSettings()
            Try

                UpdateBaseSettings()

                Dim objModules As New Entities.Modules.ModuleController
                objModules.UpdateModuleSetting(ModuleId, "lstBrowseMode", lstBrowseMode.SelectedValue)
                Select Case lstBrowseMode.SelectedValue
                    Case "0" 'enabled
                        objModules.UpdateModuleSetting(ModuleId, "chkFeatured", False)
                        objModules.UpdateModuleSetting(ModuleId, "chkBrowseCategory", True)
                    Case "1" 'disabled
                        objModules.UpdateModuleSetting(ModuleId, "chkFeatured", False)
                        objModules.UpdateModuleSetting(ModuleId, "chkBrowseCategory", False)
                    Case "2" 'featured only, browse enabled
                        objModules.UpdateModuleSetting(ModuleId, "chkFeatured", True)
                        objModules.UpdateModuleSetting(ModuleId, "chkBrowseCategory", True)
                    Case "3" 'featured only, browse disabled
                        objModules.UpdateModuleSetting(ModuleId, "chkFeatured", True)
                        objModules.UpdateModuleSetting(ModuleId, "chkBrowseCategory", False)
                    Case "4" 'init featured, browse all
                End Select

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub



    End Class

End Namespace
