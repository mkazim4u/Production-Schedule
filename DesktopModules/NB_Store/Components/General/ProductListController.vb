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


Imports DotNetNuke.Services.Search
Imports System.Xml

Namespace NEvoWeb.Modules.NB_Store

    Public Class ProductListController
        Inherits BasePortController
        Implements Entities.Modules.ISearchable

        Public Function GetSearchItems(ByVal ModInfo As Entities.Modules.ModuleInfo) As DotNetNuke.Services.Search.SearchItemInfoCollection Implements Entities.Modules.ISearchable.GetSearchItems
            '' Get all products
            Dim objCtrlProd As New ProductController
            Dim productList As ArrayList
            'FIXED: Issue 3019
            Dim supportedLanguages As LocaleCollection = DotNetNuke.Services.Localization.Localization.GetSupportedLocales
            Dim i As Integer
            Dim Lang As String = ""
            Dim SkipIndex As Boolean = False

            'don't index if featured module list
            Dim ModSettings As Hashtable
            ModSettings = Portals.PortalSettings.GetModuleSettings(ModInfo.ModuleID)
            If CType(ModSettings("chkIndexProducts"), String) <> "" Then
                SkipIndex = Not CType(ModSettings("chkIndexProducts"), Boolean)
            Else
                SkipIndex = True
            End If

            Dim searchItemList As New SearchItemInfoCollection

            If Not SkipIndex Then

                For i = 0 To supportedLanguages.Count - 1
                    Dim info As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(CType(supportedLanguages(i).Value, Locale).Code)
                    Lang = CType(supportedLanguages(i).Value, Locale).Code

                    'do search in batches of 500 to keep resources under control.
                    Dim PageIndex As Integer = 1
                    Dim PageSize As Integer = 500
                    productList = objCtrlProd.GetProductList(ModInfo.PortalID, -1, Lang, "", False, PageIndex, PageSize, False, False)
                    Do While (productList.Count > 0) And (PageIndex <= 20) ' only index 10000 products, then jump out of loop

                        ' Create search item collection
                        For Each product As ProductListInfo In productList
                            ' Get user identifier
                            Dim userID As Integer = Null.NullInteger
                            userID = Integer.Parse(product.CreatedByUser)

                            Dim title As String = System.Web.HttpUtility.HtmlDecode(product.ProductName)

                            'get xml fields
                            Dim strXML As String = product.XMLData
                            Dim strXMLValues As String = ""
                            Dim xmlNod As XmlNode
                            Dim xmlNodLIst As XmlNodeList
                            Dim xmlDoc As New XmlDataDocument

                            If strXML <> "" Then

                                xmlDoc.LoadXml(strXML)

                                xmlNodLIst = xmlDoc.SelectNodes("genxml/textbox")
                                For Each xmlNod In xmlNodLIst
                                    strXMLValues &= xmlNod.InnerText & " "
                                Next

                                xmlNodLIst = xmlDoc.SelectNodes("genxml/dropdownlist")
                                For Each xmlNod In xmlNodLIst
                                    strXMLValues &= xmlNod.InnerText & " "
                                Next

                                xmlNodLIst = xmlDoc.SelectNodes("genxml/radiobuttonlist")
                                For Each xmlNod In xmlNodLIst
                                    strXMLValues &= xmlNod.InnerText & " "
                                Next

                            End If

                            ' Create content
                            Dim content As String = HtmlUtils.Shorten(HtmlUtils.Clean(HttpUtility.HtmlDecode(title & ": " & product.Manufacturer & ": " & product.ProductRef & ": " & product.Description & " " & product.Summary & " " & strXMLValues), False), title.Length + product.Description.Length + product.Summary.Length + 3, "...")

                            Dim ModifyDate As Date

                            If product.ModifiedDate = Null.NullDate Then
                                ModifyDate = product.CreatedDate
                            Else
                                ModifyDate = product.ModifiedDate
                            End If

                            Dim searchItem As New SearchItemInfo(title, title, userID, ModifyDate, ModInfo.ModuleID, product.ProductID.ToString() & product.Lang, content, "ProdID=" & product.ProductID.ToString())

                            searchItemList.Add(searchItem)

                        Next

                        'get next batch of products
                        PageIndex = PageIndex + 1
                        productList = objCtrlProd.GetProductList(ModInfo.PortalID, -1, Lang, "", False, PageIndex, PageSize, False, False)
                    Loop


                Next
            End If

            Return searchItemList

        End Function

    End Class

End Namespace
