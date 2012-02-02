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


Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Services.Exceptions
Imports DotNetNuke.Services.Localization
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.Xml

Namespace NEvoWeb.Modules.NB_Store

    Partial Public Class CheckOutSettings
        Inherits BaseModuleSettings

        Public Overrides Sub LoadSettings()
            Try

                If (Page.IsPostBack = False) Then

                    If ModuleSettings.Count = 0 Then
                        DoReset()
                    End If


                    populatedGatewayDisplay()

                    populateGatewayList()
                    populateChequeGatewayList()

                    populateTabsList(lstTabs, PortalSettings, "")
                    populateTabsList(lstTabContShop, PortalSettings, "")
                    populateTabsList(ddlSmoothLoginTab, PortalSettings, TabId.ToString)

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

                'save encap list
                Dim li As ListItem
                Dim strData As String = ""
                For Each li In chkLEncapGateway.Items
                    If li.Selected Then
                        strData &= li.Value & ";"
                    End If
                Next
                strData = strData.Trim(";"c)
                objModules.UpdateModuleSetting(ModuleId, "encapsulatedproviders", strData)

                'save gateway list
                strData = ""
                For Each li In chkGateway.Items
                    If li.Selected Then
                        strData &= li.Value & ";"
                    End If
                Next
                strData = strData.Trim(";"c)
                objModules.UpdateModuleSetting(ModuleId, "gatewayproviders", strData)

                'update store setting to add gateway provider to "gateway.provider"
                UpdateGatewaySetting()
                UpdateChequeGatewaySetting()

            Catch exc As Exception           'Module failed to load
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub

        Private Sub populatedGatewayDisplay()
            Dim li As ListItem

            rblGatewayDisplay.Items.Clear()
            For lp As Integer = 1 To 3
                li = New ListItem
                li.Value = lp.ToString
                li.Text = Localization.GetString("GatewayDisplay" & lp.ToString, LocalResourceFile)
                rblGatewayDisplay.Items.Add(li)
            Next

            rblGatewayDisplay.SelectedValue = "1"
            Dim SelectedV As String = CType(Settings("rblGatewayDisplay"), String)
            If Not rblGatewayDisplay.Items.FindByValue(SelectedV) Is Nothing Then
                rblGatewayDisplay.SelectedValue = SelectedV
            End If


        End Sub

        Private Sub populateGatewayList()
            Try
                Dim ProviderXML As String
                Dim li As ListItem
                Dim xmlDoc As New XmlDataDocument
                Dim xmlNod As XmlNode

                ProviderXML = GetStoreSetting(PortalId, "gatewayproviders.xml", "None", True)
                xmlDoc.LoadXml(ProviderXML)

                For Each xmlNod In xmlDoc.SelectNodes("root/gateways/gateway")
                    li = New ListItem
                    li.Value = xmlNod.Attributes("ref").InnerText
                    li.Text = xmlNod.SelectSingleNode("name").InnerText
                    chkGateway.Items.Add(li)
                Next

                Dim strIN As String
                Dim strList As String()

                strIN = CType(Settings("gatewayproviders"), String)
                If Not strIN Is Nothing Then
                    strList = strIN.Split(";"c)

                    For lp As Integer = 0 To strList.GetUpperBound(0)
                        If Not chkGateway.Items.FindByValue(strList(lp)) Is Nothing Then
                            chkGateway.Items.FindByValue(strList(lp)).Selected = True
                        End If
                    Next
                End If


            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub populateChequeGatewayList()
            Try
                Dim ProviderXML As String
                Dim li As ListItem
                Dim xmlDoc As New XmlDataDocument
                Dim xmlNod As XmlNode

                ProviderXML = GetStoreSetting(PortalId, "encapsulatedproviders.xml", "None", True)
                xmlDoc.LoadXml(ProviderXML)

                For Each xmlNod In xmlDoc.SelectNodes("root/gateways/gateway")
                    li = New ListItem
                    li.Value = xmlNod.Attributes("ref").InnerText
                    li.Text = xmlNod.SelectSingleNode("name").InnerText
                    chkLEncapGateway.Items.Add(li)
                Next

                Dim strIN As String
                Dim strList As String()

                strIN = CType(Settings("encapsulatedproviders"), String)
                If Not strIN Is Nothing Then
                    strList = strIN.Split(";"c)

                    For lp As Integer = 0 To strList.GetUpperBound(0)
                        If Not chkLEncapGateway.Items.FindByValue(strList(lp)) Is Nothing Then
                            chkLEncapGateway.Items.FindByValue(strList(lp)).Selected = True
                        End If
                    Next
                End If


            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try
        End Sub


        Private Sub UpdateGatewaySetting()
            'update store setting to add gateway provider to "gateway.provider"
            Try
                Dim ProviderXML As String
                Dim xmlDoc As New XmlDataDocument
                Dim xmlNod As XmlNode
                Dim strData As String

                ProviderXML = GetStoreSetting(PortalId, "gatewayproviders.xml", "None", True)
                xmlDoc.LoadXml(ProviderXML)

                Dim li As ListItem
                strData = ""
                For Each li In chkGateway.Items
                    If li.Selected Then

                        xmlNod = xmlDoc.SelectSingleNode("root/gateways/gateway[@ref='" & li.Value & "']")

                        If Not xmlNod Is Nothing Then
                            strData &= xmlNod.SelectSingleNode("assembly").InnerText & "," & xmlNod.SelectSingleNode("class").InnerText & ";"
                        End If

                    End If
                Next

                strData = strData.TrimEnd(";"c)
                SetStoreSetting(PortalId, "gateway.provider", strData, "None", True)

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub

        Private Sub UpdateChequeGatewaySetting()
            'update store setting to add cheque gateway provider to "cheque.provider"
            Try
                Dim ProviderXML As String
                Dim xmlDoc As New XmlDataDocument
                Dim xmlNod As XmlNode
                Dim strData As String

                ProviderXML = GetStoreSetting(PortalId, "encapsulatedproviders.xml", "None", True)
                xmlDoc.LoadXml(ProviderXML)

                Dim li As ListItem
                strData = ""
                For Each li In chkLEncapGateway.Items
                    If li.Selected Then

                        xmlNod = xmlDoc.SelectSingleNode("root/gateways/gateway[@ref='" & li.Value & "']")

                        If Not xmlNod Is Nothing Then
                            strData &= xmlNod.SelectSingleNode("assembly").InnerText & "," & xmlNod.SelectSingleNode("class").InnerText & ";"
                        End If

                    End If
                Next

                If strData <> "" Then
                    strData = strData.TrimEnd(";"c)
                    SetStoreSetting(PortalId, "encapsulated.provider", strData, "None", True)
                End If

            Catch exc As Exception
                ProcessModuleLoadException(Me, exc)
            End Try

        End Sub


    End Class

End Namespace
