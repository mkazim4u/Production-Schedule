'	phdcc.CodeModule programmable module for DotNetNuke-based systems
'	http://www.phdcc.com/phdcc.CodeModule/
'	Copyright © 2007-2009 PHD Computer Consultants Ltd, PHDCC

'	Version history:
'	3/3/09		01.01.00	Chris Cant	Import/Export support

Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Data
Imports System.IO
Imports System.XML
Imports System.Web
Imports System.Collections
Imports System.Collections.Generic
Imports DotNetNuke
Imports DotNetNuke.Services.Search
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Entities.Modules

Namespace phdcc.CodeModule

	Public Class CodeController
		Implements Entities.Modules.IPortable

		Public Function ExportModule(ByVal ModuleID As Integer) As String Implements DotNetNuke.Entities.Modules.IPortable.ExportModule

			Dim strXML As String = ""

			Dim CodeModuleDirectory As String = HttpContext.Current.Server.MapPath("~/DesktopModules/phdcc.CodeModule/")

			Dim objModules As ModuleController = New ModuleController()
			Dim Settings As Hashtable = objModules.GetModuleSettings(ModuleID)

			Dim ControlName As String = Settings("control")
			strXML += Constants.vbCrLf + "<phdcc_CodeModule>" + Constants.vbCrLf
			strXML += "<control>" + ControlName + "</control>" + Constants.vbCrLf

			'Try
			'Dim ControlContents As String = File.ReadAllText(CodeModuleDirectory + ControlName)
			'strXML += "<controlContents>" + XMLEncode(ControlContents) + "</controlContents>" + Constants.vbCrLf
			'Catch ex As Exception
			'End Try

			strXML += "</phdcc_CodeModule>" + Constants.vbCrLf

			Return strXML

		End Function

		Public Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements DotNetNuke.Entities.Modules.IPortable.ImportModule

			Dim xmlCodeModule As XmlNode = GetContent(Content, "phdcc_CodeModule")
			Dim ControlName As String = GetNodeValue(xmlCodeModule, "control")

			Dim objModules As ModuleController = New ModuleController()
			objModules.UpdateModuleSetting(ModuleID, "control", ControlName)

			'Dim ControlContents As String = GetNodeValue(xmlCodeModule, "controlContents")
			'Dim CodeModuleDirectory As String = HttpContext.Current.Server.MapPath("~/DesktopModules/phdcc.CodeModule/")

		End Sub

	End Class
End Namespace


