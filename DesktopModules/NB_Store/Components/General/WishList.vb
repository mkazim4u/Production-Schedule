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
Imports System.Configuration
Imports System.Data
Imports System.Xml
Imports System.Web
Imports System.Collections.Generic

Imports DotNetNuke
Imports DotNetNuke.Common
Imports DotNetNuke.Common.Utilities.XmlUtils
Imports DotNetNuke.Common.Utilities
Imports NEvoWeb.Modules.NB_Store.SharedFunctions

Namespace NEvoWeb.Modules.NB_Store

    Public Class WishList


#Region "WishList Public Methods"

        Public Shared Sub RemoveProduct(ByVal PortalID As Integer, ByVal ProductID As String)
            Dim HList As New Hashtable
            Dim strList As String()
            Dim strOut As String = ""

            strList = GetIDList(PortalID).Split(","c)

            For lp As Integer = 0 To strList.GetUpperBound(0)
                HList.Add(strList(lp), strList(lp))
            Next

            If HList.ContainsValue(ProductID) Then
                HList.Remove(ProductID)
            End If

            For Each di As DictionaryEntry In HList
                strOut &= "," & di.Value.ToString
            Next

            strOut = strOut.TrimStart(","c)

            setStoreCookieValue(PortalID, "WishList", "ProductList", strOut)
        End Sub

        Public Shared Sub AddProduct(ByVal PortalID As Integer, ByVal ProductID As String, ByVal UserInfo As DotNetNuke.Entities.Users.UserInfo)
            Dim HList As New Hashtable
            Dim strList As String()
            Dim strOut As String = ""

            strList = GetIDList(PortalID).Split(","c)

            For lp As Integer = 0 To strList.GetUpperBound(0)
                HList.Add(strList(lp), strList(lp))
            Next

            If Not HList.ContainsValue(ProductID) Then
                HList.Add(ProductID, ProductID)
            End If

            For Each di As DictionaryEntry In HList
                strOut &= "," & di.Value.ToString
            Next

            strOut = strOut.Trim(","c)

            setStoreCookieValue(PortalID, "WishList", "ProductList", strOut)

            If Not UserInfo Is Nothing Then 'don't do if adminproduct ctrl, passes nothing as userinfo
                If Not EventInterface.Instance() Is Nothing Then
                    EventInterface.Instance.AddToWishList(PortalID, ProductID, UserInfo)
                End If
            End If


        End Sub

        Public Shared Sub ClearList(ByVal PortalID As Integer)
            removeStoreCookie(PortalID, "WishList")
        End Sub

        Public Shared Function GetIDList(ByVal PortalID As Integer) As String
            Return getStoreCookieValue(PortalID, "WishList", "ProductList")
        End Function

        Public Shared Function GetItemCountInt(ByVal PortalID As Integer) As Integer
            Dim strList As String()
            If GetIDList(PortalID) = "" Then
                Return 0
            Else
                strList = GetIDList(PortalID).Split(","c)
                Return strList.GetLength(0)
            End If
        End Function

        Public Shared Function GetItemCount(ByVal PortalID As Integer) As String
            Return GetItemCountInt(PortalID).ToString
        End Function


        Public Shared Function GetList(ByVal PortalID As Integer) As ArrayList
            Dim aryList As New ArrayList
            Dim objCtrl As New ProductController
            Dim objInfo As ProductListInfo
            Dim strList As String()
            strList = GetIDList(PortalID).Split(","c)

            For lp As Integer = 0 To strList.GetUpperBound(0)
                If IsNumeric(strList(lp)) Then
                    objInfo = objCtrl.GetProductListInfo(strList(lp), GetCurrentCulture)
                    If Not objInfo Is Nothing Then
                        aryList.Add(objInfo)
                    End If
                End If
            Next

            Return aryList
        End Function


        Public Shared Function IsInWishlist(ByVal PortalID As Integer, ByVal ProductID As String) As Boolean
            Dim HList As New Hashtable
            Dim strList As String()
            Dim strOut As String = ""

            strList = GetIDList(PortalID).Split(","c)

            For lp As Integer = 0 To strList.GetUpperBound(0)
                HList.Add(strList(lp), strList(lp))
            Next

            If HList.ContainsValue(ProductID) Then
                Return True
            Else
                Return False
            End If

        End Function

#End Region

    End Class

End Namespace

