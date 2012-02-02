'	phdcc.CodeModule programmable module for DotNetNuke-based systems
'	http://www.phdcc.com/phdcc.CodeModule/
'	Copyright © 2007-2009 PHD Computer Consultants Ltd, PHDCC

'	Version history:
'	13/5/08		01.00.03	Chris Cant	Info section added
'	3/3/09		01.01.00	Chris Cant	Convert from TabModule to Module settings - to make Import/Export easier
'	15/5/09		01.01.01	Chris Cant	Only show admin header if IsEditable
'	19/5/09		01.01.02	Chris Cant	Re-version as retail
'	25/5/09		01.01.04	Chris Cant	Cope if control name empty
'	27/5/09		01.02.00	Chris Cant	Support skin object token CODEMODULE with ControlFile property

'	ASCX and LocalResources Version history
'	13/5/08		01.00.03	Chris Cant	Info section added
'	15/5/09		01.01.01	Chris Cant	lblNoSettings and btnSettings moved into divAdmin

Imports System.Collections.Generic
Imports System.IO
Imports System.Reflection
Imports System.Web.UI

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules

Namespace phdcc.CodeModule

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The View class shows the normal module output, 
	''' and handles Setting button press (and future Edit module action)
	''' </summary>
	''' -----------------------------------------------------------------------------
	Partial Class View
		Inherits Entities.Modules.PortalModuleBase
		Implements Entities.Modules.IActionable

		' 01.02.00..
		Private _ControlFile As String
		''' <summary>
		''' This property receives the ControlFile token value set in the skin.xml file
		''' </summary>
		Public Property ControlFile() As String
			Get
				Return _ControlFile
			End Get
			Set(ByVal value As String)
				_ControlFile = value
			End Set
		End Property
		' ..01.02.00

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Page_Load runs when the control is loaded.
		''' If control name not set and Admin then show Admin block.
		''' Otherwise, load control and add to our ViewControl PlaceMarker
		''' The control's page events are then called by ASP.NET as normal
		''' </summary>
		''' -----------------------------------------------------------------------------
		Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
			Try
				' 01.00.03..
				If Request.IsAuthenticated And EditMode And IsEditable Then	' EditMode true if an Administrator	' 01.01.01
					divAdmin.Visible = True
				End If
				' ..01.00.03..

				' 01.01.00..
				' Convert Tab-Module setting to Module, so it can be seen easily in Import/Export
				Dim objModules As ModuleController = New ModuleController()
				Dim TabSettingControl As String = objModules.GetTabModuleSettings(TabModuleId)("control")
				If Not TabSettingControl Is Nothing Then
					objModules.DeleteTabModuleSetting(TabModuleId, "control")
					objModules.UpdateModuleSetting(ModuleId, "control", TabSettingControl)
				End If
				' ..01.01.00

				' Find control name, either from skin token setting or standard module setting	' 01.02.00..
				Dim ControlName = ControlFile
				If ControlName Is Nothing Then
					ControlName = CStr(Settings("control"))
				End If																			' ..01.02.00
				If ControlName Is Nothing Or ControlName = "" Then	' 01.01.04
					lblNoSettings.Visible = True
					btnSettings.Visible = True
				Else
					lblInfo.Text = ControlName	' 01.00.03
					' 01.01.00..
					' Load control and add to page
					Dim CodeModuleDirectory As String = HttpContext.Current.Server.MapPath("~/DesktopModules/phdcc.CodeModule/")
					If File.Exists(CodeModuleDirectory + ControlName) Then
						ViewControl.Controls.Add(LoadControl(ControlName))
					Else
						lblError.InnerText = "Control file does not exist: " + CodeModuleDirectory + ControlName
						lblError.Visible = True
					End If
					' ..01.01.00

				End If

			Catch exc As Exception		  'Module failed to load
				lblError.InnerText = exc.Message + exc.StackTrace
				lblError.Visible = True
			End Try
		End Sub

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' Registers the module actions required for interfacing with the portal framework.
		''' This will handle the Edit option if this is needed in future
		''' </summary>
		''' <value></value>
		''' -----------------------------------------------------------------------------
		Public ReadOnly Property ModuleActions() As Entities.Modules.Actions.ModuleActionCollection Implements Entities.Modules.IActionable.ModuleActions
			Get
				Dim Actions As New Entities.Modules.Actions.ModuleActionCollection
				'Actions.Add(GetNextActionID, Localization.GetString(Entities.Modules.Actions.ModuleActionType.AddContent, LocalResourceFile), Entities.Modules.Actions.ModuleActionType.AddContent, "", "", EditUrl(), False, Security.SecurityAccessLevel.Edit, True, False)
				Return Actions
			End Get
		End Property
		''' -----------------------------------------------------------------------------
		''' Handle Settings button press by redirecting to module settings
		''' -----------------------------------------------------------------------------
		Protected Sub btnSettings_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSettings.Click
			Response.Redirect(DotNetNuke.Common.Globals.NavigateURL(PortalSettings.ActiveTab.TabID, "Module", "moduleid", ModuleId.ToString()), False)
		End Sub
	End Class

End Namespace
