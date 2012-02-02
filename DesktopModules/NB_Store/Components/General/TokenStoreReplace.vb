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
Imports DotNetNuke.Entities.Host.HostSettings
Imports DotNetNuke.Entities.Modules
Imports DotNetNuke.Entities.Portals
Imports DotNetNuke.Entities.Tabs
Imports DotNetNuke.Entities.Users
Imports DotNetNuke.Entities.Profile
Imports DotNetNuke.Entities.Host
Imports DotNetNuke.Services.Localization
Imports System.Globalization
Imports System.Web
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports NEvoWeb.Modules.NB_Store.SharedFunctions
Imports System.Xml
Imports System.IO

Namespace NEvoWeb.Modules.NB_Store

    ''' <summary>
    ''' The TokenReplace class provides the option to replace tokens formatted 
    ''' [object:property] within a string
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TokenStoreReplace
        Inherits DotNetNuke.Services.Tokens.BaseCustomTokenReplace

        Private _OrderID As Integer
        Private _PortalID As Integer = -1
        Private _Lang As String = ""

#Region "Constructor"
        Public Sub New()
            _PortalID = -1
        End Sub

        Public Sub New(ByVal CTotals As CartTotals, ByVal Lang As String, Optional ByVal PortalID As Integer = -1)
            _PortalID = PortalID
            LoadCartTokens(CTotals, Lang)
        End Sub

        Public Sub New(ByVal objProductInfo As ProductListInfo)
            If Not objProductInfo Is Nothing Then
                Dim params As IDictionary
                _PortalID = objProductInfo.PortalID
                _Lang = objProductInfo.Lang

                params = GetTokenList(objProductInfo, objProductInfo.Lang)
                AddPropertySource(params, "Product")

            End If
        End Sub

        Public Sub New(ByVal objModelInfo As NB_Store_ModelInfo)
            If Not objModelInfo Is Nothing Then
                Dim params As IDictionary
                _PortalID = objModelInfo.PortalID
                _Lang = objModelInfo.Lang

                params = GetTokenList(objModelInfo, objModelInfo.Lang)
                AddPropertySource(params, "Model")

            End If
        End Sub

        Public Sub New(ByVal objOrderInfo As NB_Store_OrdersInfo, ByVal Lang As String)
            If Not objOrderInfo Is Nothing Then
                _OrderID = objOrderInfo.OrderID
                _PortalID = objOrderInfo.PortalID
                _Lang = Lang
                LoadStoreTokens(objOrderInfo, Lang)
            End If
        End Sub

        Public Sub New(ByVal PortalID As Integer)
            _PortalID = PortalID
        End Sub

        Public Sub New(ByVal PortalID As Integer, ByVal Lang As String)
            _PortalID = PortalID
            _Lang = Lang
        End Sub

#End Region

#Region "Public Replace Methods"

        Public Sub AddPropertySource(ByVal Custom As IDictionary, ByVal CustomCaption As String)
            PropertySource.Add(CustomCaption.ToLower, New DotNetNuke.Services.Tokens.DictionaryPropertyAccess(Custom))
        End Sub

        Public Function DoTokenReplace(ByVal strSourceText As String, ByVal ClientBound As Boolean) As String
            Return DoTokenReplace(strSourceText, _Lang, ClientBound)
        End Function

        Public Function DoTokenReplace(ByVal strSourceText As String) As String
            Return DoTokenReplace(strSourceText, _Lang, False)
        End Function

        Public Function DoTokenReplace(ByVal strSourceText As String, ByVal Lang As String) As String
            Return DoTokenReplace(strSourceText, Lang, False)
        End Function

        Public Function DoTokenReplace(ByVal strSourceText As String, ByVal Lang As String, ByVal ClientBound As Boolean) As String
            If _PortalID < 0 Then
                Dim PS As Entities.Portals.PortalSettings = Entities.Portals.PortalController.GetCurrentPortalSettings
                _PortalID = PS.PortalId
            End If

            'use double sqr brqckets as escape char.
            Dim FoundEscapeChar As Boolean = False
            If InStr(strSourceText, "[[") > 0 Or InStr(strSourceText, "]]") > 0 Then
                strSourceText = Replace(strSourceText, "[[", "**SQROPEN**")
                strSourceText = Replace(strSourceText, "]]", "**SQRCLOSE**")
                FoundEscapeChar = True
            End If

            'replace any [Template:*.*] in then source text with the acutal templates.
            If Lang = "" Then Lang = _Lang
            strSourceText = replaceTemplates(strSourceText, Lang)

            If InStr(strSourceText, "[Report:") > 0 Then
                strSourceText = ReplaceReportTokens(_PortalID, strSourceText)
            End If

            If InStr(strSourceText, "[WishList:ItemCount]") > 0 Then
                strSourceText = Replace(strSourceText, "[WishList:ItemCount]", WishList.GetItemCount(_PortalID))
            End If

            If InStr(strSourceText, "[Order:GoogleEcommerceUpdate]") > 0 Then
                strSourceText = Replace(strSourceText, "[Order:GoogleEcommerceUpdate]", getGoogleAnalyticsEcommerceScript(_PortalID))
            End If

            If InStr(strSourceText, "[Setting:") > 0 Then
                strSourceText = ReplaceSettingTokens(_PortalID, strSourceText)
            End If

            If InStr(strSourceText, "[Order:DetailRows]") > 0 Then
                strSourceText = Replace(strSourceText, "[Order:DetailRows]", getCartDetailHtmlRows)
            End If

            If InStr(strSourceText, "[Order:DetailTemplateRows]") > 0 Then
                strSourceText = Replace(strSourceText, "[Order:DetailTemplateRows]", getCartDetailTemplateRows("", ClientBound))
            End If

            If InStr(strSourceText, "[Order:TrackingLink]") > 0 Then
                strSourceText = Replace(strSourceText, "[Order:TrackingLink]", getTrackingLink)
            End If
            If InStr(strSourceText, "[Order:ShippingMethod]") > 0 Then
                strSourceText = Replace(strSourceText, "[Order:ShippingMethod]", getShippingMethod)
            End If

            If InStr(strSourceText, "[Currency:Symbol]") > 0 Then
                strSourceText = Replace(strSourceText, "[Currency:Symbol]", getCurrencySymbol())
            End If

            If InStr(strSourceText, "[Cart:URL]") > 0 Then
                If IsNumeric(GetStoreSetting(_PortalID, "checkout.tab")) Then
                    strSourceText = Replace(strSourceText, "[Cart:URL]", NavigateURL(CInt(GetStoreSetting(_PortalID, "checkout.tab"))))
                End If
            End If

            If InStr(strSourceText, "[Cart:TabID]") > 0 Then
                strSourceText = Replace(strSourceText, "[Cart:TabID]", GetStoreSetting(_PortalID, "checkout.tab"))
            End If

            If InStr(strSourceText, "[Order:AmendedLink]") > 0 Then
                strSourceText = Replace(strSourceText, "[Order:AmendedLink]", NavigateURL(CInt(GetStoreSettingInt(_PortalID, "checkout.tab", GetCurrentCulture)), "", "codeid=[Order:OrderGUID]"))
            End If

            If Not TokenExtraInterface.Instance() Is Nothing Then
                strSourceText = TokenExtraInterface.Instance.DoExtraReplace(_PortalID, strSourceText, _Lang, _OrderID)
            End If

            Dim rtnStr As String = ReplaceTokens(strSourceText)

            If FoundEscapeChar Then
                rtnStr = Replace(rtnStr, "**SQROPEN**", "[")
                rtnStr = Replace(rtnStr, "**SQRCLOSE**", "]")
            End If

            Return rtnStr
        End Function

        Protected Overrides Function ReplaceTokens(ByVal strSourceText As String) As String
            Return MyBase.ReplaceTokens(strSourceText)
        End Function

#End Region

#Region "Private methods"

        Private Sub LoadCartTokens(ByVal CTotals As CartTotals, ByVal Lang As String)
            If Not CTotals Is Nothing Then
                Dim params As IDictionary

                params = GetTokenList(CTotals, Lang)
                AddPropertySource(params, "Cart")
            End If
        End Sub

        Private Sub LoadStoreTokens(ByVal objOrderInfo As NB_Store_OrdersInfo, ByVal Lang As String)
            If Not objOrderInfo Is Nothing Then
                Dim objACtrl As New OrderController
                Dim params As IDictionary
                Dim ctlEntry As New CountryLists
                Dim cList As DotNetNuke.Common.Lists.ListEntryInfo

                params = GetTokenList(objOrderInfo, Lang)
                'replace stg2FormXML and stg3FormXML with xsl rndered html
                params.Item("Stg2FormXML") = XSLTransByTemplate(params.Item("Stg2FormXML"), "Stg2FormXML.xsl")
                params.Item("Stg3FormXML") = XSLTransByTemplate(params.Item("Stg3FormXML"), "Stg3FormXML.xsl")

                AddPropertySource(params, "Order")

                Dim objAInfo As NB_Store_AddressInfo
                objAInfo = objACtrl.GetOrderAddress(objOrderInfo.BillingAddressID)
                If Not objAInfo Is Nothing Then
                    params = GetTokenList(objAInfo, Lang)
                    cList = ctlEntry.getCountryEntryInfo(objAInfo.CountryCode, Lang)
                    If Not cList Is Nothing Then
                        params.Add("Country", cList.Text)
                    End If
                    AddPropertySource(params, "BAddress")
                End If

                objAInfo = objACtrl.GetOrderAddress(objOrderInfo.ShippingAddressID)
                If Not objAInfo Is Nothing Then
                    params = GetTokenList(objAInfo, Lang)
                    cList = ctlEntry.getCountryEntryInfo(objAInfo.CountryCode, Lang)
                    If Not cList Is Nothing Then
                        params.Add("Country", cList.Text)
                    End If
                    AddPropertySource(params, "SAddress")
                End If

                Dim objOSInfo As NB_Store_OrderStatusInfo
                objOSInfo = objACtrl.GetOrderStatus(objOrderInfo.OrderStatusID, Lang)
                If Not objOSInfo Is Nothing Then
                    params = GetTokenList(objOSInfo, Lang)
                    AddPropertySource(params, "OrderStatus")
                End If

                Dim objUsrCtrl As New Users.UserController
                Dim objUsrInfo As UserInfo = objUsrCtrl.GetUser(objOrderInfo.PortalID, objOrderInfo.UserID)
                If Not objUsrInfo Is Nothing Then
                    params = GetTokenList(objUsrInfo, Lang)
                    AddPropertySource(params, "OrderUser")

                    params = GetProfileTokensList(objUsrInfo, Lang)
                    AddPropertySource(params, "OrderUserProfile")
                End If

            End If
        End Sub

        Private Function GetProfileTokensList(ByVal obj As UserInfo, ByVal Lang As String) As IDictionary '#RS#
            Dim params As IDictionary(Of String, String) = New Dictionary(Of String, String)

            For Each pprop As DotNetNuke.Entities.Profile.ProfilePropertyDefinition In obj.Profile.ProfileProperties
                If Not params.ContainsKey(pprop.PropertyName) Then
                    params.Add(pprop.PropertyName, pprop.PropertyValue)
                End If
            Next

            Return params
        End Function

        Private Function GetTokenList(ByVal obj As Object, ByVal Lang As String) As IDictionary
            Dim params As IDictionary(Of String, String) = New Dictionary(Of String, String)
            Dim reg As New Regex("\s*")

            Dim myType As Type = obj.GetType
            Dim properties As System.Reflection.PropertyInfo() = myType.GetProperties()
            Dim ci As New System.Globalization.CultureInfo(Lang)
            If ci Is Nothing Then ci = System.Threading.Thread.CurrentThread.CurrentCulture
            For Each p As System.Reflection.PropertyInfo In properties
                Try
                    Select Case p.PropertyType.Name
                        Case "Decimal"

                            'params.Add(p.Name, CType(p.GetValue(obj, Nothing), Decimal).ToString("F", ci))
                            params.Add(p.Name, FormatToStoreCurrency(CType(p.GetValue(obj, Nothing), Decimal)))
                            params.Add("#" & p.Name, reg.Replace(Replace(FormatToStoreCurrency(CType(p.GetValue(obj, Nothing), Decimal)), getCurrencySymbol(), ""), ""))
                            params.Add("." & p.Name, CType(p.GetValue(obj, Nothing), Decimal).ToString("0.00"))
                            params.Add("=" & p.Name, Replace(CType(p.GetValue(obj, Nothing), Decimal).ToString("0.00"), ".", ""))
                        Case "DateTime"
                            If _PortalID >= 0 Then
                                Dim dteFormat As String = GetStoreSetting(_PortalID, "datedisplay.format", Lang)
                                If dteFormat <> "" Then
                                    params.Add(p.Name, DirectCast(p.GetValue(obj, Nothing), Date).ToString(dteFormat, ci))
                                Else
                                    params.Add(p.Name, DirectCast(p.GetValue(obj, Nothing), Date).ToString("d", ci))
                                End If
                            Else
                                params.Add(p.Name, DirectCast(p.GetValue(obj, Nothing), Date).ToString("d", ci))
                            End If
                        Case Else
                            params.Add(p.Name, p.GetValue(obj, Nothing).ToString)
                    End Select
                Catch ex As Exception
                End Try
            Next

            Return params

        End Function

        Private Function getCartDetailTemplateRows(ByVal TemplateName As String, ByVal ClientBound As Boolean) As String
            Dim strHTML As String = ""
            Dim objCtrl As New OrderController
            Dim objPCtrl As New ProductController
            Dim objODInfo As NB_Store_OrderDetailsInfo
            Dim objMInfo As NB_Store_ModelInfo
            Dim objPInfo As ProductListInfo
            Dim aryList As ArrayList
            Dim objOInfo As NB_Store_OrdersInfo
            Dim rtnCartTemplate As String = ""

            If TemplateName = "" Then TemplateName = "detailrows.template"
            Dim cartTemplate As String = GetStoreSettingText(_PortalID, TemplateName, _Lang)

            If cartTemplate = "" Then cartTemplate = "*** No cart template: detailrows.template ****"

            objOInfo = objCtrl.GetOrder(_OrderID)

            If Not objOInfo Is Nothing Then

                aryList = objCtrl.GetOrderDetailList(_OrderID)
                For Each objODInfo In aryList

                    Dim objToken As New TokenStoreReplace(_PortalID, _Lang)

                    objToken.AddPropertySource(objToken.GetTokenList(objODInfo, _Lang), "Detail")

                    objMInfo = objPCtrl.GetModel(objODInfo.ModelID, _Lang)
                    If Not objMInfo Is Nothing Then
                        objToken.AddPropertySource(objToken.GetTokenList(objMInfo, _Lang), "Model")
                    End If

                    objPInfo = objPCtrl.GetProductListInfo(objMInfo.ProductID, _Lang)
                    If Not objPInfo Is Nothing Then
                        objToken.AddPropertySource(objToken.GetTokenList(objPInfo, _Lang), "Product")
                    End If

                    'set file download link.
                    rtnCartTemplate = cartTemplate
                    If objODInfo.CartXMLInfo <> "" Then
                        Dim uInfo As UserInfo = Entities.Users.UserController.GetCurrentUserInfo
                        If Not uInfo Is Nothing Then
                            If IsEditor(_PortalID, uInfo) And Not ClientBound Then
                                rtnCartTemplate = Replace(cartTemplate, "[Detail:FileLink]", "<a href='/DesktopModules/NB_Store/FileProvider.ashx?did=" & objODInfo.OrderDetailID.ToString & "'><img src='/DesktopModules/NB_Store/img/file.gif'/></a>")
                            End If
                        End If
                    End If
                    rtnCartTemplate = Replace(rtnCartTemplate, "[Detail:FileLink]", "")

                    strHTML &= System.Web.HttpUtility.HtmlDecode(objToken.DoTokenReplace(rtnCartTemplate))


                Next
            End If

            Return strHTML
        End Function

        Private Function getCartDetailHtmlRows() As String
            Dim strHTML As String = ""
            Dim objCtrl As New OrderController
            Dim objODInfo As NB_Store_OrderDetailsInfo
            Dim aryList As ArrayList
            Dim objOInfo As NB_Store_OrdersInfo
            objOInfo = objCtrl.GetOrder(_OrderID)

            If Not objOInfo Is Nothing Then

                Dim blnDoLink As Boolean = GetStoreSettingBoolean(objOInfo.PortalID, "productlinksoncart.flag", GetCurrentCulture)

                aryList = objCtrl.GetOrderDetailList(_OrderID)
                For Each objODInfo In aryList
                    strHTML &= "<tr class=""OrderDetailRow"">"
                    strHTML &= "<td>"
                    If blnDoLink Then
                        strHTML &= "<a href=""" & objODInfo.ProductURL & """ target=""_blank"" >" & objODInfo.ItemDesc & "</a>"
                    Else
                        strHTML &= objODInfo.ItemDesc
                    End If
                    strHTML &= "</td>"
                    strHTML &= "<td align=""center"">"
                    strHTML &= FormatToStoreCurrency(objODInfo.UnitCost)
                    strHTML &= "</td>"
                    strHTML &= "<td align=""center"">"
                    strHTML &= objODInfo.Quantity
                    strHTML &= "</td>"
                    strHTML &= "<td  align=""right"">"
                    strHTML &= FormatToStoreCurrency(objODInfo.UnitCost * objODInfo.Quantity)
                    strHTML &= "</td>"
                    strHTML &= "</tr>"
                Next
            End If

            Return strHTML
        End Function

        Private Function getTrackingLink() As String
            Dim strHTML As String = ""
            Dim objCtrl As New OrderController
            Dim objSCtrl As New SettingsController
            Dim objSHCtrl As New ShipController
            Dim objSHInfo As NB_Store_ShippingMethodInfo
            Dim objSTInfo As NB_Store_SettingsTextInfo
            Dim objOInfo As NB_Store_OrdersInfo

            objOInfo = objCtrl.GetOrder(_OrderID)
            If Not objOInfo Is Nothing Then

                If objOInfo.TrackingCode = "" Then
                    Return ""
                Else
                    objSHInfo = objSHCtrl.GetShippingMethod(objOInfo.ShipMethodID)
                    objSTInfo = objSCtrl.GetSettingsText(objOInfo.PortalID, objSHInfo.URLtracker, _Lang)

                    If Not objSTInfo Is Nothing Then
                        strHTML = Replace(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText), "[TAG:TRACKINGCODE]", objOInfo.TrackingCode)
                    End If

                    Return strHTML
                End If

            End If
            Return ""
        End Function

        Private Function getShippingMethod() As String
            Dim strHTML As String = ""
            Dim objCtrl As New OrderController
            Dim objSCtrl As New SettingsController
            Dim objSHCtrl As New ShipController
            Dim objSHInfo As NB_Store_ShippingMethodInfo = Nothing
            Dim objSTInfo As NB_Store_SettingsTextInfo = Nothing
            Dim objOInfo As NB_Store_OrdersInfo

            objOInfo = objCtrl.GetOrder(_OrderID)
            If Not objOInfo Is Nothing Then
                objSHInfo = objSHCtrl.GetShippingMethod(objOInfo.ShipMethodID)
                If Not objSHInfo Is Nothing Then
                    objSTInfo = objSCtrl.GetSettingsText(objOInfo.PortalID, objSHInfo.TemplateName, _Lang)
                    If Not objSTInfo Is Nothing Then
                        strHTML = Replace(System.Web.HttpUtility.HtmlDecode(objSTInfo.SettingText), "[TAG:SHIPPINGCOST]", FormatToStoreCurrency(objOInfo.ShippingCost))
                    Else
                        strHTML = objSHInfo.MethodDesc
                    End If
                End If
            End If

            Return strHTML
        End Function


        Friend Function replaceTemplates(ByVal sourceText As String, ByVal Lang As String, Optional ByVal RecursiveCount As Integer = 1) As String
            Dim aryTempl As String()
            Dim lp As Integer
            aryTempl = ParseTemplateText(sourceText)
            If aryTempl.Length > 0 And RecursiveCount < 5 Then
                Dim objCtrl As New SettingsController
                Dim objInfo As NB_Store_SettingsTextInfo
                Dim newTemplateText As String = ""
                Dim strTemplName As String
                Dim blnTemplateFound As Boolean = False
                Dim strRoles As String = ""
                Dim strSplit() As String

                For lp = 0 To aryTempl.GetUpperBound(0)
                    If aryTempl(lp).ToUpper.StartsWith("TEMPLATE:") Then
                        strSplit = aryTempl(lp).Split(":"c)

                        strTemplName = strSplit(1)
                        If strSplit.GetUpperBound(0) >= 2 Then
                            strRoles = strSplit(2)
                        End If
                        newTemplateText = ""

                        If strRoles = "" Or DotNetNuke.Entities.Users.UserController.GetCurrentUserInfo.IsInRole(strRoles) Then
                            objInfo = objCtrl.GetSettingsTextNotCached(_PortalID, strTemplName, Lang)
                            If Not objInfo Is Nothing Then
                                newTemplateText = objInfo.SettingText
                            Else
                                Dim objInfo2 As NB_Store_SettingsInfo
                                objInfo2 = objCtrl.GetSettingNotCached(_PortalID, strTemplName, Lang)
                                If Not objInfo2 Is Nothing Then
                                    newTemplateText = objInfo2.SettingValue
                                End If
                            End If
                            newTemplateText = System.Web.HttpUtility.HtmlDecode(newTemplateText)
                        End If

                        sourceText = Replace(sourceText, "[" & aryTempl(lp) & "]", newTemplateText)
                        blnTemplateFound = True
                    End If
                Next
                If blnTemplateFound Then
                    'call recursive to pick up any new template tokens
                    sourceText = replaceTemplates(sourceText, Lang, RecursiveCount + 1)
                End If
            End If
            Return sourceText
        End Function

        Private Function ParseTemplateText(ByVal TemplText As String) As String()
            Dim strOUT As String()
            Dim ParamAry As Char() = {"[", "]"}

            strOUT = TemplText.Split(ParamAry)

            Return strOUT
        End Function

        Private Function ReplaceReportTokens(ByVal PortalID As Integer, ByVal SourceText As String) As String
            Dim rtnSourceText As String = SourceText
            Dim SourceTokenList As String()
            Dim objRCtrl As New SQLReportController

            SourceTokenList = ParseTemplateText(SourceText)

            For Each strToken As String In SourceTokenList
                If strToken.ToLower.StartsWith("report:") Then
                    Dim RepRef As String
                    RepRef = Replace(strToken.ToLower, "report:", "")
                    rtnSourceText = Replace(rtnSourceText, "[" & strToken & "]", objRCtrl.GetReportOutput(PortalID, RepRef))
                End If
            Next

            Return rtnSourceText
        End Function

        Private Function ReplaceSettingTokens(ByVal PortalID As Integer, ByVal SourceText As String) As String
            Dim rtnSourceText As String = SourceText
            Dim SourceTokenList As String()

            SourceTokenList = ParseTemplateText(SourceText)

            For Each strToken As String In SourceTokenList
                If strToken.ToLower.StartsWith("setting:") Then
                    Dim SettingRef As String
                    SettingRef = Replace(strToken.ToLower, "setting:", "")
                    rtnSourceText = Replace(rtnSourceText, "[" & strToken & "]", GetStoreSetting(PortalID, SettingRef))
                End If
            Next

            Return rtnSourceText
        End Function

        Private Function getGoogleAnalyticsEcommerceScript(ByVal PortalID As Integer) As String
            Dim rtnStr As String = ""

            If Not IsDNN4() Then ' only supported in DNN 5 and above


                Dim trackingId As String = String.Empty
                Dim objAnalyticsConfiguration As Object

                'DNN 5 use get AnalyticsConfiguration object
                Try
                    Dim objHandle As Runtime.Remoting.ObjectHandle = Activator.CreateInstance("DotNetNuke", "DotNetNuke.Services.Analytics.Config.AnalyticsConfiguration")
                    objAnalyticsConfiguration = objHandle.Unwrap
                Catch ex As Exception
                    Return "" ' failed so ignore
                End Try

                Dim config As Object = objAnalyticsConfiguration.GetConfig("GoogleAnalytics")
                If Not config Is Nothing Then
                    Dim urlParameter As String = String.Empty

                    For Each setting As Object In config.Settings
                        Select Case setting.SettingName.ToLower
                            Case "trackingid"
                                trackingId = setting.SettingValue
                            Case "urlparameter"
                                urlParameter = setting.SettingValue
                        End Select
                    Next
                End If

                If trackingId = String.Empty Then
                    Return String.Empty
                End If

                Dim objCtrl As New OrderController
                Dim objOInfo As NB_Store_OrdersInfo
                objOInfo = objCtrl.GetOrder(_OrderID)
                If objOInfo Is Nothing Then
                    Return String.Empty
                End If

                Dim objACtrl As New OrderController
                Dim objAInfo As NB_Store_AddressInfo
                objAInfo = objACtrl.GetOrderAddress(objOInfo.ShippingAddressID)
                If objAInfo Is Nothing Then
                    Return String.Empty
                End If

                Dim writer As New StringWriter()
                writer.WriteLine("<script type=""text/javascript"">")
                writer.WriteLine("var _gaq = _gaq || [")
                writer.WriteLine("];")
                writer.WriteLine("_gaq.push(['_setAccount', '" & trackingId & "']);")
                writer.WriteLine("_gaq.push(['_trackPageview']);")
                writer.WriteLine("_gaq.push(['_addTrans',")
                writer.WriteLine("'" & objOInfo.OrderID & "',") ' order ID - required
                writer.WriteLine("'" & GetStoreSetting(PortalID, "store.name") & "',") 'affiliation or store name
                writer.WriteLine("'" & objOInfo.Total & "',") 'total - required
                writer.WriteLine("'" & objOInfo.ShippingCost & "',") 'shipping
                writer.WriteLine("'" & objAInfo.City & "',") 'city
                writer.WriteLine("'" & objAInfo.RegionCode & "',") 'state or province
                writer.WriteLine("'" & objAInfo.CountryCode & "'")
                writer.WriteLine("]);")

                Dim pc As ProductController = New ProductController()

                Dim objODInfo As NB_Store_OrderDetailsInfo
                Dim aryList As ArrayList
                aryList = objCtrl.GetOrderDetailList(_OrderID)
                For Each objODInfo In aryList
                    Dim modelInfo As NB_Store_ModelInfo = pc.GetModel(objODInfo.ModelID, _Lang)
                    Dim productInfo As NB_Store_ProductsInfo = pc.GetProduct(modelInfo.ProductID, _Lang)

                    writer.WriteLine("_gaq.push(['_addItem',")
                    writer.WriteLine("'" & _OrderID & "',") 'order ID - required
                    writer.WriteLine("'" & modelInfo.Barcode & "',") 'SKU/code - required
                    writer.WriteLine("'" & productInfo.ProductName.Replace("'", "").Replace("""", "") & "',") 'product name
                    writer.WriteLine("'" & modelInfo.ModelName.Replace("'", "").Replace("""", "") & "',") 'category or variation
                    writer.WriteLine("'" & objODInfo.UnitCost & "',") 'unit price - required
                    writer.WriteLine("'" & objODInfo.Quantity & "'") 'quantity - required
                    writer.WriteLine("]);")
                Next

                writer.WriteLine("_gaq.push(['_trackTrans']);") 'submits transaction to the Analytics servers

                writer.WriteLine("(function() {")
                writer.WriteLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;")
                writer.WriteLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';")
                writer.WriteLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);")
                writer.WriteLine("})();")
                writer.WriteLine("</script>")
                rtnStr = writer.ToString()
            End If

            Return rtnStr
        End Function

#End Region

    End Class

End Namespace
