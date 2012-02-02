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
Imports System.Data
Imports System.Xml
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store


    Public Class BaseModuleSettings
        Inherits Entities.Modules.ModuleSettingsBase

        Protected WithEvents fupSettings As New Global.System.Web.UI.WebControls.FileUpload
        Dim cmdSave As New LinkButton
        Dim cmdLoad As New LinkButton
        Dim cmdReset As New LinkButton

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            AddHandler cmdSave.Click, AddressOf cmdSaveClick
            AddHandler cmdLoad.Click, AddressOf cmdLoadClick
            AddHandler cmdReset.Click, AddressOf cmdResetClick
        End Sub

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            cmdSave.Text = Localization.GetString("cmdSave", NBSTORERESX)
            cmdLoad.Text = Localization.GetString("cmdLoad", NBSTORERESX)
            cmdReset.Text = Localization.GetString("cmdReset", NBSTORERESX)

            cmdReset.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdResetMsg", NBSTORERESX) & "');")
            cmdLoad.Attributes.Add("onClick", "javascript:return confirm('" & Localization.GetString("cmdLoadMsg", NBSTORERESX) & "');")

            cmdSave.CssClass = "NBright_CommandButton"
            cmdLoad.CssClass = "NBright_CommandButton"
            cmdReset.CssClass = "NBright_CommandButton"

            Me.Controls.Add(New LiteralControl("<hr/><div class=""normalbold"">" & Localization.GetString("settingsheader", NBSTORERESX) & "</div>"))
            Me.Controls.Add(New LiteralControl("<table><tr><td>"))
            Me.Controls.Add(cmdReset)
            Me.Controls.Add(New LiteralControl("</td><td>"))
            Me.Controls.Add(cmdSave)
            Me.Controls.Add(New LiteralControl("</td><td>"))
            Me.Controls.Add(fupSettings)
            Me.Controls.Add(New LiteralControl("&nbsp;"))
            Me.Controls.Add(cmdLoad)
            Me.Controls.Add(New LiteralControl("</td></tr></table>"))
            Me.Controls.Add(New LiteralControl("<br/>TabID : " & TabId.ToString))

        End Sub

        Private Sub cmdSaveClick()
            Dim objImpCtrl As New BasePortController
            Dim strExport As String = ""

            UpdateSettings()

            strExport = "<content type=""" & ModuleConfiguration.DesktopModule.ModuleName & """ version=""" & ModuleConfiguration.DesktopModule.Version & """>"
            strExport &= objImpCtrl.ExportModule(ModuleId)
            strExport &= "</content>"

            ForceStringDownload(Response, "NB_Store_" & Replace(ModuleConfiguration.DesktopModule.ModuleName, " ", "_") & ".xml", strExport)

        End Sub

        Private Sub cmdLoadClick()
            Try
                Dim objImpCtrl As New BasePortController

                If fupSettings.FileName <> "" Then
                    fupSettings.SaveAs(PortalSettings.HomeDirectoryMapPath & "ImportedSettings.xml")
                    DoModuleImport(ModuleId, PortalSettings.HomeDirectoryMapPath & "ImportedSettings.xml", UserId)
                    Response.Redirect(NavigateURL("Module", "ModuleId=" & ModuleId))
                End If
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub cmdResetClick()
            DoReset()
        End Sub

        Public Sub DoReset()
            Try
                DoModuleImport(ModuleId, Server.MapPath(ControlPath & "templates\" & ModuleConfiguration.DesktopModule.ModuleName & ".xml"), UserId)
                Response.Redirect(NavigateURL("Module", "ModuleId=" & ModuleId))
            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub UpdateBaseSettings()
            Try
                UpdateCtrls(Me)
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Public Sub LoadBaseSettings()
            Try
                If (Page.IsPostBack = False) Then
                    LoadCtrls(Me)
                End If
            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub LoadCtrls(ByVal ObjCtrl As Control)
            Dim strValue As String
            For Each ctrl As Object In ObjCtrl.Controls
                If TypeOf ctrl Is TextBox Then
                    ctrl.Text = CType(Settings(ctrl.ID.ToString), String)
                ElseIf TypeOf ctrl Is CheckBox Then
                    ctrl.Checked = CType(Settings(ctrl.ID.ToString), Boolean)
                ElseIf TypeOf ctrl Is DropDownList Then
                    strValue = CType(Settings(ctrl.ID.ToString), String)
                    If Not ctrl.Items.FindByValue(strValue) Is Nothing Then
                        ctrl.ClearSelection()
                        ctrl.Items.FindByValue(strValue).Selected = True
                    End If
                ElseIf TypeOf ctrl Is RadioButtonList Then
                    strValue = CType(Settings(ctrl.ID.ToString), String)
                    If Not ctrl.Items.FindByValue(strValue) Is Nothing Then
                        ctrl.ClearSelection()
                        ctrl.Items.FindByValue(strValue).Selected = True
                    End If
                ElseIf ctrl.Controls.Count > 0 Then
                    LoadCtrls(ctrl)
                End If
            Next
        End Sub

        Private Sub UpdateCtrls(ByVal ObjCtrl As Control)
            Dim objModules As New Entities.Modules.ModuleController

            For Each ctrl As Object In ObjCtrl.Controls
                If TypeOf ctrl Is TextBox Then
                    objModules.UpdateModuleSetting(ModuleId, ctrl.ID.ToString, ctrl.Text)
                ElseIf TypeOf ctrl Is CheckBox Then
                    objModules.UpdateModuleSetting(ModuleId, ctrl.ID.ToString, ctrl.Checked.ToString)
                ElseIf TypeOf ctrl Is DropDownList Then
                    objModules.UpdateModuleSetting(ModuleId, ctrl.ID.ToString, ctrl.SelectedValue)
                ElseIf TypeOf ctrl Is RadioButtonList Then
                    objModules.UpdateModuleSetting(ModuleId, ctrl.ID.ToString, ctrl.SelectedValue)
                ElseIf ctrl.Controls.Count > 0 Then
                    UpdateCtrls(ctrl)
                End If
            Next
        End Sub

    End Class

End Namespace

