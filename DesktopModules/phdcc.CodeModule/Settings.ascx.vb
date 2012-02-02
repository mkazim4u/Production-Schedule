'	phdcc.CodeModule programmable module for DotNetNuke-based systems
'	http://www.phdcc.com/phdcc.CodeModule/
'	Copyright © 2007-2009 PHD Computer Consultants Ltd, PHDCC

'	Version history:
'	3/3/09		01.01.00	Chris Cant	Convert from TabModule to Module settings - to make Import/Export easier

'	ASCX and LocalResources Version history

Imports System.Web.UI

Imports DotNetNuke
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Services.Exceptions


Namespace phdcc.CodeModule

	''' -----------------------------------------------------------------------------
	''' <summary>
	''' The Settings class manages Module Settings
	''' </summary>
	''' <remarks>
	''' </remarks>
	''' <history>
	''' </history>
	''' -----------------------------------------------------------------------------
	Partial Class Settings
		Inherits Entities.Modules.ModuleSettingsBase

#Region "Base Method Implementations"

		''' -----------------------------------------------------------------------------
		''' <summary>
		''' LoadSettings loads the settings from the Database and displays them
		''' </summary>
		''' <remarks>
		''' </remarks>
		''' <history>
		''' </history>
		''' -----------------------------------------------------------------------------
		Public Overrides Sub LoadSettings()
			Try
				Dim objModules As ModuleController = New ModuleController()
				Dim mi As ModuleInfo = objModules.GetModule(ModuleId, TabId)
				lblModuleVersion.Text = mi.Version

				If Not Page.IsPostBack Then
					Dim ControlName = Settings("control")	' 01.01.00
					txtControlName.Text = ControlName
				End If
			Catch exc As Exception			 'Module failed to load
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
		''' </history>
		''' -----------------------------------------------------------------------------
		Public Overrides Sub UpdateSettings()
			Try
				Dim objModules As New Entities.Modules.ModuleController

				objModules.DeleteTabModuleSetting(TabModuleId, "control")					' 01.01.00
				objModules.UpdateModuleSetting(ModuleId, "control", txtControlName.Text)	' 01.01.00

				' refresh cache
				SynchronizeModule()
			Catch exc As Exception			 'Module failed to load
				ProcessModuleLoadException(Me, exc)
			End Try
		End Sub

#End Region

	End Class

End Namespace

