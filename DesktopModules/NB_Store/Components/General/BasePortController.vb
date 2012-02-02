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

    Public Class BasePortController
        Implements Entities.Modules.IPortable


#Region "Optional Interfaces"
        'Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems

        'End Function

        Public Overridable Function ExportModule(ByVal ModuleID As Integer) As String Implements Entities.Modules.IPortable.ExportModule
            Dim strXML As String = ""
            Dim supportedLanguages As DotNetNuke.Services.Localization.LocaleCollection = DotNetNuke.Services.Localization.Localization.GetSupportedLocales()
            Dim settings As Hashtable
            Dim d As DictionaryEntry

            settings = DotNetNuke.Entities.Portals.PortalSettings.GetModuleSettings(ModuleID)

            strXML += "<modulecontent>"

            strXML += "<settings>"

            For Each d In settings
                If Not IsNumeric(d.Key) Then
                    strXML += "<" & d.Key.ToString() & "><![CDATA[" & d.Value.ToString() & "]]></" & d.Key.ToString() & ">"
                End If
            Next

            strXML += "</settings>"

            strXML += "</modulecontent>"

            Return strXML
        End Function

        Public Overridable Sub ImportModule(ByVal ModuleID As Integer, ByVal Content As String, ByVal Version As String, ByVal UserId As Integer) Implements Entities.Modules.IPortable.ImportModule
            Dim xmlText As XmlNode = GetContent(Content, "modulecontent")
            Dim xmlSettings As XmlNode
            Dim xmlSetNod As XmlNode
            Dim settings As Hashtable
            Dim d As DictionaryEntry

            Dim objModules As New Entities.Modules.ModuleController

            'clear current settings
            settings = DotNetNuke.Entities.Portals.PortalSettings.GetModuleSettings(ModuleID)
            For Each d In settings
                objModules.DeleteModuleSetting(ModuleID, d.Key.ToString)
            Next


            xmlSettings = xmlText.SelectSingleNode("settings")

            For Each xmlSetNod In xmlSettings.SelectNodes("*")
                objModules.UpdateModuleSetting(ModuleID, xmlSetNod.Name, xmlSettings.SelectSingleNode(xmlSetNod.Name).InnerText)
            Next

        End Sub
#End Region

    End Class

End Namespace
