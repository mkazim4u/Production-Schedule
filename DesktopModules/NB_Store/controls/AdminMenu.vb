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
Imports System.IO

Namespace NEvoWeb.Modules.NB_Store

    Public Class AdminMenu
        Inherits System.Web.UI.UserControl
        
#Region "Private Members"
        Protected dnnctl As String = ""
        Protected spg As String = ""
        Protected etmode As String = ""
        Protected skinurl As Boolean = False
        Protected _Parent As BaseModule

        Protected plhTabMenu As PlaceHolder
        Protected plhSubTabMenu As PlaceHolder
        Protected plhTitle As PlaceHolder

#End Region

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            plhTabMenu = New PlaceHolder
            plhSubTabMenu = New PlaceHolder
            plhTitle = New PlaceHolder

            Me.Controls.Add(New LiteralControl("<div class=""NBright_MenuDiv"">"))
            Me.Controls.Add(plhTabMenu)
            Me.Controls.Add(plhSubTabMenu)
            Me.Controls.Add(plhTitle)
            Me.Controls.Add(New LiteralControl("</div>"))

        End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Dim objSCtrl As New NB_Store.SettingsController
            Dim xmlDoc As New XmlDataDocument

            _Parent = CType(Me.Parent, BaseModule)

            dnnctl = ""
            If Not (Request.QueryString("ctl") Is Nothing) Then
                dnnctl = Request.QueryString("ctl")
            End If

            spg = ""
            If Not (Request.QueryString("spg") Is Nothing) Then
                spg = Request.QueryString("spg").ToLower
            End If

            etmode = ""
            If Not (Request.QueryString("etmode") Is Nothing) Then
                etmode = Request.QueryString("etmode").ToLower
            End If

            skinurl = False
            If Not (Request.QueryString("skinsrc") Is Nothing) Then
                skinurl = True
            End If

            Dim strCacheKey As String = ""
            strCacheKey = GetCacheKey("MainMenu.xml", _Parent.PortalId)

            If strCacheKey <> "" Then
                If DataCache.GetCache(strCacheKey) Is Nothing Or GetStoreSettingBoolean(_Parent.PortalId, "debug.mode", "None") Then

                    Dim strPortalMenu As String = GetStoreSettingText(_Parent.PortalId, "menu.xml", GetCurrentCulture, False, True)
                    If strPortalMenu <> "" And InStr(strPortalMenu, "ctl=""AdminSetting"" param=""etmode=1""") > 0 Then
                        xmlDoc.LoadXml(strPortalMenu)
                    Else
                        Try
                            xmlDoc.Load(Server.MapPath(_Parent.StoreInstallPath & "xml/MainMenu.xml"))
                        Catch ex As Exception
                            xmlDoc.Load(Server.MapPath("~/DesktopModules/NB_Store/xml/MainMenu.xml"))
                        End Try
                    End If

                    MergePlugins(_Parent.PortalId, xmlDoc)
                    DataCache.SetCache(strCacheKey, xmlDoc, DateAdd(DateInterval.Hour, 1, Now))
                Else
                    xmlDoc = CType(DataCache.GetCache(strCacheKey), XmlDataDocument)
                End If

                Dim SkinSrc As String = ""
                If Not xmlDoc.SelectSingleNode("root/skin").Attributes("path") Is Nothing Then
                    SkinSrc = QueryStringEncode(DotNetNuke.Common.ResolveUrl(xmlDoc.SelectSingleNode("root/skin").Attributes("path").InnerText))
                End If

                'add css file for menu
                AddCSSLink(xmlDoc)

                'check if skin is needed
                If skinurl Or SkinSrc = "" Then
                    buildMenu(xmlDoc)
                Else
                    buildEnterButton(xmlDoc)
                    RegisterJS(_Parent.PortalId, "jquery.nbstorejquerycheck.js", _Parent.StoreInstallPath, _Parent.Page)
                End If

            End If

        End Sub


        Private Sub buildEnterButton(ByVal xmlDoc As XmlDataDocument)
            Dim hyp As HyperLink
            hyp = New HyperLink

            Dim ResxCtrl As String = getAtt(xmlDoc, "root/resx", "name")
            Dim SkinSrc As String = QueryStringEncode(DotNetNuke.Common.ResolveUrl(getAtt(xmlDoc, "root/skin", "path")))
            Dim EnterCss As String = getAtt(xmlDoc, "root/enterbutton", "css")
            Dim EnterCssDiv As String = getAtt(xmlDoc, "root/enterbutton", "cssdiv")
            Dim ResourceKey As String = getAtt(xmlDoc, "root/enterbutton", "resourcekey")
            Dim EnterCtrl As String = getAtt(xmlDoc, "root/enterbutton", "defaultctrl")

            hyp.Text = getLocalTabText(ResourceKey, ResxCtrl)

            'only display button if menu module is being used, menu module should match resx module.
            If hyp.Text <> "" And ResxCtrl.StartsWith(Me.Parent.GetType().BaseType.Name) Then
                hyp.CssClass = EnterCss
                hyp.NavigateUrl = NavigateURL(EnterCtrl, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, "SkinSrc=" & SkinSrc)

                If EnterCssDiv <> "" Then
                    EnterCssDiv = " class=""" & EnterCssDiv & """"
                End If

                plhTitle.Controls.Add(New LiteralControl("<div " & EnterCssDiv & ">"))
                plhTitle.Controls.Add(hyp)
                plhTitle.Controls.Add(New LiteralControl("</div>"))
            End If

        End Sub

        Private Function MergePlugins(ByVal PortalID As Integer, ByVal xmlDoc As XmlDataDocument) As XmlDataDocument
            Dim xmlPlug As New XmlDataDocument
            Dim objCtrl As New NB_Store.SettingsController
            Dim objSInfo As NB_Store_SettingsInfo
            Dim aryList As ArrayList
            Dim xmlNodToAdd As XmlNode
            Dim xmlRoot As XmlNode

            aryList = objCtrl.GetSettingList(PortalID, "None", True, "")

            For Each objSInfo In aryList
                If objSInfo.SettingName.ToLower.EndsWith(".plugin") Then
                    Try
                        Dim TabControlName As String
                        Dim ModControlName As String
                        Dim ModControlSrc As String
                        'Add plugin to xml file.
                        xmlPlug = New XmlDataDocument
                        xmlPlug.LoadXml(objSInfo.SettingValue)
                        'get the nod to be placed under.
                        TabControlName = xmlPlug.SelectSingleNode("root/tabs/tab").Attributes("id").InnerXml
                        ModControlName = xmlPlug.SelectSingleNode("root/tabs/tab/subtab").Attributes("ctl").InnerXml
                        ModControlSrc = xmlPlug.SelectSingleNode("root/tabs/tab/subtab").Attributes("ctlsrc").InnerXml

                        xmlRoot = xmlDoc.SelectSingleNode("root/tabs/tab[@id='" & TabControlName & "']")

                        If Not xmlRoot Is Nothing Then

                            'get nod to add
                            xmlNodToAdd = xmlPlug.SelectSingleNode("root/tabs/tab/subtab")

                            'append to xmldoc
                            xmlRoot.AppendChild(xmlDoc.ImportNode(xmlNodToAdd, True))


                            'add plugin to NB_Store_BackOffice
                            Dim objModInfo As DotNetNuke.Entities.Modules.ModuleControlInfo
                            objModInfo = DotNetNuke.Entities.Modules.ModuleControlController.GetModuleControlByControlKey(ModControlName, _Parent.ModuleConfiguration.ModuleDefID)
                            If objModInfo Is Nothing Then
                                objModInfo = New DotNetNuke.Entities.Modules.ModuleControlInfo
                                objModInfo.ControlKey = ModControlName
                                objModInfo.ControlSrc = ModControlSrc
                                objModInfo.ControlTitle = ModControlName
                                objModInfo.ControlType = SecurityAccessLevel.Edit
                                objModInfo.HelpURL = ""
                                objModInfo.IconFile = ""
                                objModInfo.ModuleControlID = Null.NullInteger
                                objModInfo.ModuleDefID = _Parent.ModuleConfiguration.ModuleDefID
                                objModInfo.SupportsPartialRendering = False
                                objModInfo.ViewOrder = 0

                                DotNetNuke.Entities.Modules.ModuleControlController.AddModuleControl(objModInfo)

                            End If

                        End If


                    Catch ex As Exception
                        ex.HelpLink = "Plugin Failed to Load: " & objSInfo.SettingName
                        LogException(ex) ' plugin failed to load
                    End Try

                End If
            Next

            Return xmlDoc
        End Function

        Private Sub buildMenu(ByVal xmlDoc As XmlDataDocument)
            Dim nod As XmlNode
            Dim nodList As XmlNodeList
            Dim strLink As String
            Dim TabName As String = ""
            Dim SubTabName As String = ""


            'set resx
            Dim ResxCtrl As String = getAtt(xmlDoc, "root/resx", "name")
            Dim SkinSrc As String = QueryStringEncode(DotNetNuke.Common.ResolveUrl(getAtt(xmlDoc, "root/skin", "path")))
            Dim TitleCss As String = getAtt(xmlDoc, "root/css", "titlediv")
            Dim TabCss As String = getAtt(xmlDoc, "root/css", "tabdiv")
            Dim TabulCss As String = getAtt(xmlDoc, "root/css", "tabul")
            Dim TabliCss As String = getAtt(xmlDoc, "root/css", "tabli")
            Dim TabliActiveCss As String = getAtt(xmlDoc, "root/css", "tabliactive")

            Dim SubTabCss As String = getAtt(xmlDoc, "root/css", "subtabdiv")
            Dim SubTabulCss As String = getAtt(xmlDoc, "root/css", "subtabul")
            Dim SubTabliCss As String = getAtt(xmlDoc, "root/css", "sublitabli")
            Dim SubTabliActiveCss As String = getAtt(xmlDoc, "root/css", "sublitabliactive")


            If TabCss <> "" Then TabCss = " class=""" & TabCss & """"
            If TabulCss <> "" Then TabulCss = " class=""" & TabulCss & """"
            If TabliCss <> "" Then TabliCss = " class=""" & TabliCss & """"
            If TabliActiveCss <> "" Then TabliActiveCss = " class=""" & TabliActiveCss & """"
            If SubTabCss <> "" Then SubTabCss = " class=""" & SubTabCss & """"
            If SubTabulCss <> "" Then SubTabulCss = " class=""" & SubTabulCss & """"
            If SubTabliCss <> "" Then SubTabliCss = " class=""" & SubTabliCss & """"
            If SubTabliActiveCss <> "" Then SubTabliActiveCss = " class=""" & SubTabliCss & """"
            If TitleCss <> "" Then TitleCss = " class=""" & TitleCss & """"

            'build tab menu
            nodList = xmlDoc.SelectNodes("root/tabs/tab")
            strLink = "<div  " & TabulCss & ">"
            strLink &= "<table  " & TabCss & "><tr>"

            For Each nod In nodList

                If checkDisplayRoles(nod) Then



                    Dim nList As XmlNodeList
                    Dim ActiveParent As Boolean = False
                    nList = nod.SelectNodes("subtab[@ctl='" & dnnctl & "']")

                    If Not nList Is Nothing Then
                        If nList.Count > 0 Then
                            ActiveParent = True
                        End If
                    End If

                    If dnnctl.ToLower = nod.Attributes("ctl").InnerText.ToLower Then
                        ActiveParent = True
                    End If

                    Dim iconURL As String = nod.Attributes("image").InnerText

                    If Not iconURL.ToLower.StartsWith("~/desk") Then
                        'need to add module path
                        iconURL = _Parent.StoreInstallPath & iconURL
                    Else
                        iconURL = DotNetNuke.Common.ResolveUrl(iconURL)
                    End If


                    If ActiveParent Then
                        TabName = getLocalTabText(nod.Attributes("text").InnerText, ResxCtrl)
                        If SkinSrc = "" Then
                            strLink &= "<td " & TabliActiveCss & " ><a href=""" & NavigateURL(nod.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod.Attributes("param").InnerText) & """ ><img src=""" & iconURL & """ alt="""" border=""0"" align=""absmiddle"" /> " & getLocalTabText(nod.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                        Else
                            strLink &= "<td " & TabliActiveCss & " ><a href=""" & NavigateURL(nod.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod.Attributes("param").InnerText, "SkinSrc=" & SkinSrc) & """ ><img src=""" & iconURL & """ alt="""" border=""0"" align=""absmiddle"" /> " & getLocalTabText(nod.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                        End If
                    Else
                        If nod.Attributes("ctl").InnerText = "EXIT" Then
                            'Exit Menu
                            strLink &= "<td " & TabliCss & " ><a href=""" & NavigateURL() & """ ><img src=""" & iconURL & """ alt="""" border=""0""  align=""absmiddle"" /> " & getLocalTabText(nod.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                        Else

                            If SkinSrc = "" Then
                                strLink &= "<td " & TabliCss & " ><a href=""" & NavigateURL(nod.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod.Attributes("param").InnerText) & """ ><img src=""" & iconURL & """ alt="""" border=""0"" align=""absmiddle"" /> " & getLocalTabText(nod.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                            Else
                                strLink &= "<td " & TabliCss & " ><a href=""" & NavigateURL(nod.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod.Attributes("param").InnerText, "SkinSrc=" & SkinSrc) & """ ><img src=""" & iconURL & """ alt="""" border=""0"" align=""absmiddle"" /> " & getLocalTabText(nod.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                            End If
                        End If
                    End If
                End If
            Next
            strLink &= "</tr></table>"
            strLink &= "</div>"

            'Me.Controls.Add(New LiteralControl(strLink))
            plhTabMenu.Controls.Add(New LiteralControl(strLink))

            'select 1st subtab nod with ctl attribute
            nod = xmlDoc.SelectSingleNode("root/tabs/tab/subtab[@ctl=""" & dnnctl & """]")

            If nod Is Nothing Then
                'dnn throws ctl name is as lower case on login, so just teke fisrt node
                nod = xmlDoc.SelectSingleNode("root/tabs/tab/subtab[1]")
            End If

            'get get children of parent
            strLink = "<div  " & SubTabulCss & ">"
            strLink &= "<table  " & SubTabCss & "><tr>"

            If Not nod Is Nothing Then
                For Each nod2 As XmlNode In nod.ParentNode.ChildNodes
                    If checkDisplayRoles(nod2) Then

                        Dim iconURL As String = nod2.Attributes("image").InnerText

                        If Not iconURL.ToLower.StartsWith("~/desk") Then
                            'need to add module path
                            iconURL = _Parent.StoreInstallPath & iconURL
                        Else
                            iconURL = DotNetNuke.Common.ResolveUrl(iconURL)
                        End If


                        If IsActiveSubTab(nod2.Attributes("ctl").InnerText, dnnctl, nod2.Attributes("param").InnerText) Then
                            SubTabName = getLocalTabText(nod2.Attributes("text").InnerText, ResxCtrl)
                            If SkinSrc = "" Then
                                strLink &= "<td " & SubTabliActiveCss & " ><a href=""" & NavigateURL(nod2.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod2.Attributes("param").InnerText) & """ ><img src=""" & iconURL & """ alt="""" border=""0""  align=""absmiddle""/> " & getLocalTabText(nod2.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                            Else
                                strLink &= "<td " & SubTabliActiveCss & " ><a href=""" & NavigateURL(nod2.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod2.Attributes("param").InnerText, "SkinSrc=" & SkinSrc) & """ ><img src=""" & iconURL & """ alt="""" border=""0""  align=""absmiddle""/> " & getLocalTabText(nod2.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                            End If
                        Else
                            If nod2.Attributes("ctl").InnerText = "HELP" Then
                                'Help Buton
                                strLink &= "<td " & SubTabliCss & " ><a href=""" & _Parent.StoreInstallPath & "documentation/Documentation.html"" target=""_blank"" ><img src=""" & iconURL & """ alt="""" border=""0""  align=""absmiddle"" /> " & getLocalTabText(nod2.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                            Else
                                If SkinSrc = "" Then
                                    strLink &= "<td " & SubTabliCss & " ><a href=""" & NavigateURL(nod2.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod2.Attributes("param").InnerText) & """ ><img src=""" & iconURL & """ alt="""" border=""0""  align=""absmiddle""/> " & getLocalTabText(nod2.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                                Else
                                    strLink &= "<td " & SubTabliCss & " ><a href=""" & NavigateURL(nod2.Attributes("ctl").InnerText, "mid=" & CType(Me.Parent, Entities.Modules.PortalModuleBase).ModuleId, nod2.Attributes("param").InnerText, "SkinSrc=" & SkinSrc) & """ ><img src=""" & iconURL & """ alt="""" border=""0""  align=""absmiddle""/> " & getLocalTabText(nod2.Attributes("text").InnerText, ResxCtrl) & "</a></td>"
                                End If
                            End If
                        End If
                    End If
                Next
            End If
            strLink &= "</tr></table>"
            strLink &= "</div>"

            'Me.Controls.Add(New LiteralControl(strLink))
            plhSubTabMenu.Controls.Add(New LiteralControl(strLink))


            If SubTabName = "" Then
                strLink = "<div " & TitleCss & ">" & TabName & "</div>"
            Else
                strLink = "<div " & TitleCss & ">" & TabName & " - " & SubTabName & "</div>"
            End If

            'Me.Controls.Add(New LiteralControl(strLink))
            plhTitle.Controls.Add(New LiteralControl(strLink))

        End Sub

        Private Function IsActiveSubTab(ByVal MenuCtrl As String, ByVal dnnCtrl As String, ByVal menuaparam As String) As Boolean
            Dim blnActive As Boolean = False

            If spg = "" And etmode = "" Then
                If MenuCtrl = dnnCtrl Then
                    blnActive = True
                End If
            Else
                If spg <> "" Then ' shipping control being used, test param to get active ctrl
                    If MenuCtrl = dnnCtrl And menuaparam.EndsWith(spg) Then
                        blnActive = True
                    End If
                End If

                If etmode <> "" Then ' setting control being used, test param to get active ctrl
                    If MenuCtrl = dnnCtrl And menuaparam.EndsWith(etmode) Then
                        blnActive = True
                    End If
                End If

            End If

            Return blnActive
        End Function

        Private Function getAtt(ByVal xmlDoc As XmlDataDocument, ByVal XPath As String, ByVal AttName As String) As String
            If Not xmlDoc.SelectSingleNode(XPath).Attributes(AttName) Is Nothing Then
                Return xmlDoc.SelectSingleNode(XPath).Attributes(AttName).InnerText
            Else
                Return ""
            End If
        End Function

        Private Function getLocalTabText(ByVal TabText As String, ByVal ResourceCtrl As String) As String
            Dim rtnMsg As String
            rtnMsg = Localization.GetString(Replace(TabText, " ", "_"), Services.Localization.Localization.GetResourceFile(Me.Parent, ResourceCtrl))
            If rtnMsg = "" Then rtnMsg = TabText
            Return rtnMsg
        End Function

        Private Sub AddCSSLink(ByVal xmlDoc As XmlDataDocument)
            Dim LinkFile As String = getAtt(xmlDoc, "root/css", "path")
            If LinkFile <> "" Then
                Dim oLink As New System.Web.UI.HtmlControls.HtmlGenericControl("link")
                oLink.Attributes("rel") = "stylesheet"
                oLink.Attributes("media") = "screen"
                oLink.Attributes("type") = "text/css"
                oLink.Attributes("href") = LinkFile
                Dim oCSS As Control = Me.Page.FindControl("CSS")
                If Not oCSS Is Nothing Then
                    oCSS.Controls.Add(oLink)
                End If
            End If
        End Sub

        Private Function checkDisplayRoles(ByVal nod As XmlNode) As Boolean
            Dim blnAdd As Boolean = False

            'check roles
            If Not nod.Attributes("roles") Is Nothing _
            And Not CType(Me.Parent, Entities.Modules.PortalModuleBase).UserInfo.IsSuperUser _
            And Not CType(Me.Parent, Entities.Modules.PortalModuleBase).UserInfo.IsInRole("Administrators") Then
                Dim ManagerRoles As String()
                ManagerRoles = nod.Attributes("roles").InnerText.Split(","c)
                For lp As Integer = 0 To ManagerRoles.GetUpperBound(0)
                    If CType(Me.Parent, Entities.Modules.PortalModuleBase).UserInfo.IsInRole(ManagerRoles(lp)) Then
                        blnAdd = True
                    End If
                Next

            Else
                blnAdd = True
            End If
            Return blnAdd

        End Function

    End Class

End Namespace

