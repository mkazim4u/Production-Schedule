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

    Partial Public Class ProductListOptions
        Inherits BaseModule


        Private Sub LoadSettings()
            'this section of code is  a duplicate of code in the "productlistsettings" control.
            Try

                If (Page.IsPostBack = False) Then

                    chkRedirectToCart.Checked = CType(Settings("chkRedirectToCart"), Boolean)
                    If Not rblCategoryMessage.Items.FindByValue(CType(Settings("rblCategoryMessage"), String)) Is Nothing Then
                        rblCategoryMessage.SelectedValue = CType(Settings("rblCategoryMessage"), String)
                    Else
                        rblCategoryMessage.SelectedValue = "1"
                    End If

                    txtPageSize.Text = CType(Settings("txtPageSize"), String)
                    chkIncrementCart.Checked = CType(Settings("chkIncrementCart"), Boolean)
                    chkDefaultOrderDESC.Checked = CType(Settings("chkDefaultOrderDESC"), Boolean)
                    chkSkipList.Checked = CType(Settings("chkSkipList"), Boolean)

                    populateCategoryList(PortalId, ddlDefaultCategory, -1, "All Categories", "")

                    If Not ddlDefaultCategory.Items.FindByValue(CType(Settings("ddlDefaultCategory"), String)) Is Nothing Then
                        ddlDefaultCategory.ClearSelection()
                        ddlDefaultCategory.Items.FindByValue(CType(Settings("ddlDefaultCategory"), String)).Selected = True
                    End If


                    LocalizeDDL(ddlDefaultOrder, Localization.GetString("DefaultOrderList", LocalResourceFile))
                    If Not ddlDefaultOrder.Items.FindByValue(CType(Settings("ddlDefaultOrder"), String)) Is Nothing Then
                        ddlDefaultOrder.SelectedValue = CType(Settings("ddlDefaultOrder"), String)
                    End If

                    If Not lstModuleTitle.Items.FindByValue(CType(Settings("lstModuleTitle"), String)) Is Nothing Then
                        lstModuleTitle.ClearSelection()
                        lstModuleTitle.Items.FindByValue(CType(Settings("lstModuleTitle"), String)).Selected = True
                    End If

                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub UpdateSettings()
            'this section of code is  a duplicate of code in the "productlistsettings" control.
            Try
                Dim objModules As New Entities.Modules.ModuleController

                objModules.UpdateModuleSetting(ModuleId, "chkRedirectToCart", chkRedirectToCart.Checked.ToString)
                objModules.UpdateModuleSetting(ModuleId, "rblCategoryMessage", rblCategoryMessage.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "txtPageSize", txtPageSize.Text)
                objModules.UpdateModuleSetting(ModuleId, "ddlDefaultCategory", ddlDefaultCategory.SelectedValue)

                objModules.UpdateModuleSetting(ModuleId, "chkIncrementCart", chkIncrementCart.Checked.ToString)
                objModules.UpdateModuleSetting(ModuleId, "ddlDefaultOrder", ddlDefaultOrder.SelectedValue)
                objModules.UpdateModuleSetting(ModuleId, "chkDefaultOrderDESC", chkDefaultOrderDESC.Checked.ToString)
                objModules.UpdateModuleSetting(ModuleId, "chkSkipList", chkSkipList.Checked.ToString)

                objModules.UpdateModuleSetting(ModuleId, "lstModuleTitle", lstModuleTitle.SelectedValue)

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            LoadSettings()
        End Sub

        Private Sub cmdCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
            Try
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub cmdUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdUpdate.Click
            Try
                UpdateSettings()
                Response.Redirect(NavigateURL(), True)
            Catch exc As Exception    'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub
    End Class

End Namespace
